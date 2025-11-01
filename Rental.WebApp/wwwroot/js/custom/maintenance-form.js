/**
 * Maintenance Request Form JavaScript Module
 * Handles all functionality for the maintenance request form
 */
var MaintenanceForm = {
    /**
     * Initialize the Maintenance form functionality
     */
    init: function() {
        if (!$('#maintenance-form').length) {
            return; // Exit if the maintenance form is not present
        }
        
        this.initializeMasking();
        this.initializeValidation();
        this.ensureTurnstileRendered();
        this.initializeFormSubmission();
    },

    /**
     * Initialize input masking for phone fields
     */
    initializeMasking: function() {
        // Apply phone masking if the function exists
        if (typeof applyPhoneMasking === 'function') {
            applyPhoneMasking();
        }
    },

    /**
     * Initialize form validation
     */
    initializeValidation: function() {
        var form = $('#maintenance-form');
        if (form.length) {
            // Remove existing validation data before re-parsing
            form.removeData('validator');
            form.removeData('unobtrusiveValidation');
            // Re-parse the form for jQuery Unobtrusive Validation
            $.validator.unobtrusive.parse(form);
        }
    },

    /**
     * Render Cloudflare Turnstile widget when present (supports dynamically injected/replaced forms)
     */
    ensureTurnstileRendered: function() {
        try {
            if (window.turnstile) {
                document.querySelectorAll('.cf-turnstile').forEach(function(el) {
                    if (!el.dataset.turnstileWidgetId) {
                        var id = window.turnstile.render(el, {
                            callback: function(token) {
                                el.dataset.turnstileTokenReady = 'true';
                            },
                            'expired-callback': function() {
                                el.dataset.turnstileTokenReady = '';
                            },
                            'error-callback': function() {
                                el.dataset.turnstileTokenReady = '';
                            }
                        });
                        el.dataset.turnstileWidgetId = id;
                    }
                });
            }
        } catch (e) {
            console.warn('Turnstile render failed (maintenance):', e);
        }
    },

    /**
     * Get the current Turnstile token, if available
     */
    getTurnstileToken: function() {
        try {
            var el = document.querySelector('.cf-turnstile');
            if (!el) return '';
            var id = el.dataset.turnstileWidgetId;
            if (window.turnstile && id) {
                return window.turnstile.getResponse(id) || '';
            }
            var hidden = el.querySelector('input[name="cf-turnstile-response"]') || document.querySelector('input[name="cf-turnstile-response"]');
            return hidden && hidden.value ? hidden.value : '';
        } catch (e) {
            return '';
        }
    },

    /**
     * Initialize form submission handling
     */
    initializeFormSubmission: function() {
        var self = this;
        
        $('#maintenance-form').submit(async function(e) {
            e.preventDefault();
            
            if (!$(e.target).valid()) {
                return;
            }

            // Ensure Turnstile produced a token before sending
            var token = self.getTurnstileToken();
            if (!token) {
                showNotificationModal("Complete verification", "Please complete the verification challenge, then submit again.");
                return;
            }

            var formData = $(e.target).serialize();

            await showStatusModal("Sending Maintenance Request", "Sending maintenance request...");
            
            $.ajax({
                type: 'POST',
                url: $('#maintenance-form').attr('action'),
                data: formData
            }).done(async function(response) {
                await hideStatusModal();
                
                if (response.isSuccess) {
                    showNotificationModal("Maintenance request sent", "Your maintenance request has been sent");
                    $('#notificationModal').one('hidden.bs.modal', function() {
                        // Clear form so data doesn't display when back button clicked
                        $('#maintenance-form')[0].reset();
                        window.location.href = response.redirectUrl;
                    });
                } else { 
                    self.showSubmitError(); 
                }
            }).fail(async function(xhr) {
                await hideStatusModal();
                
                if (xhr.status ===400 && xhr.responseText) {
                    // Replace the form HTML with the returned partial containing validation messages
                    var container = $('#maintenanceFormContainer');
                    container.html(xhr.responseText);
                    // Re-run initialization on new markup
                    MaintenanceForm.init();
                    // Ensure Turnstile widget is rendered on the refreshed form
                    MaintenanceForm.ensureTurnstileRendered();
                    return;
                }
                
                self.showSubmitError();
            });
        });
    },

    /**
     * Show error message when form submission fails
     */
    showSubmitError: function() {
        // Get the company email from the container data attribute
        var companyEmail = $('.container[data-company-email]').data('company-email') || 'management@company.com';
        
        showNotificationModal(
            "Error sending maintenance request", 
            "An error occurred while sending the maintenance request. Please send directly to " + companyEmail + ". We apologize for the inconvenience"
        );
    }
};

/**
 * Auto-initialize when DOM is ready
 */
$(document).ready(function() {
    MaintenanceForm.init();
});
