/**
 * Apply Form JavaScript Module
 * Handles all functionality for the rental application form
 */
var ApplyForm = {
    /**
     * Initialize the Apply form functionality
     */
    init: function() {
        if (!$('#application-form').length) {
            return; // Exit if the application form is not present
        }
        
        this.initializeMasking();
        this.initializeDatePickers();
        this.initializeValidation();
        this.initializeCertificationNameUpdates();
        this.ensureTurnstileRendered();
        this.initializeFormSubmission();
    },

    /**
     * Initialize input masking for SSN and phone fields
     */
    initializeMasking: function() {
        // Apply SSN masking if the function exists
        if (typeof applySSNMasking === 'function') {
            applySSNMasking();
        }
        
        // Apply phone masking if the function exists
        if (typeof applyPhoneMasking === 'function') {
            applyPhoneMasking();
        }
    },

    /**
     * Initialize jQuery UI datepickers for datetime inputs
     */
    initializeDatePickers: function() {
        $('input[type=datetime]').datepicker({
            dateFormat: "m/dd/yy",
            changeMonth: true,
            changeYear: true,
            yearRange: "-10:+1"
        });
    },

    /**
     * Initialize form validation
     */
    initializeValidation: function() {
        var form = $('#application-form');
        if (form.length) {
            // Remove existing validation data before re-parsing
            form.removeData('validator');
            form.removeData('unobtrusiveValidation');
            // Re-parse the form for jQuery Unobtrusive Validation
            $.validator.unobtrusive.parse(form);
        }
    },

    /**
     * Render Cloudflare Turnstile widget when present (supports dynamically injected forms)
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
            console.warn('Turnstile render failed:', e);
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
     * Initialize dynamic name updates in certification text
     */
    initializeCertificationNameUpdates: function() {
        var self = this;
        
        // Function to update the certification text with current name values
        function updateCertificationNames() {
            var firstName = $('#PersonalInfo_FirstName').val() || '';
            var lastName = $('#PersonalInfo_LastName').val() || '';
            
            // Create full name by concatenating and trimming
            var fullName = (firstName + ' ' + lastName).trim();
            
            // Update the names display
            $('#applicant-name').text(fullName);
        }

        // Set up event listeners for real-time updates
        $(document).on('input keyup paste blur', '#PersonalInfo_FirstName, #PersonalInfo_LastName', function() {
            updateCertificationNames();
        });

        // Initialize with current values (in case form is pre-populated)
        updateCertificationNames();
    },

    /**
     * Initialize form submission handling
     */
    initializeFormSubmission: function() {
        var self = this;
        
        $('#application-form').submit(async function(e) {
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

            // Restore real SSN values before form submission
            const ssnInputs = document.querySelectorAll('input.ssn-mask');
            ssnInputs.forEach(input => {
                if (input.dataset.realSsn) {
                    input.value = input.dataset.realSsn;
                }
            });

            var formData = $(e.target).serialize();

            await showStatusModal("Sending Application", "Sending your application for review...");
            
            $.ajax({
                type: 'POST',
                url: $('#application-form').attr('action'),
                data: formData
            }).done(async function(response) {
                await hideStatusModal();
                
                if (response.isSuccess) {
                    showNotificationModal("Application sent", "Your application has been sent");
                    $('#notificationModal').one('hidden.bs.modal', function() {
                        // Clear form so data doesn't display when back button clicked
                        $('#application-form')[0].reset();
                        window.location.href = response.redirectUrl;
                    });
                } else { 
                    self.showSubmitError(); 
                }
            }).fail(async function(xhr) {
                await hideStatusModal();
                if (xhr.status === 400 && xhr.responseText) {
                    // Replace the form HTML with the returned partial containing validation messages
                    var container = $('#applicationFormContainer');
                    container.html(xhr.responseText);
                    // Re-run initialization on new markup
                    ApplyForm.init();
                    // Ensure the Turnstile widget is rendered on the refreshed form
                    ApplyForm.ensureTurnstileRendered();
                    // Scroll to first validation error
                    var firstError = container.find('.input-validation-error, .text-danger').filter(function(){ return $(this).text().trim().length > 0; }).first();
                    if (firstError.length) {
                        $('html, body').animate({ scrollTop: firstError.offset().top - 40 }, 400);
                    }
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
        showNotificationModal(
            "Error sending application", 
            "An error occurred while sending the application. Please use the 'Apply Offline' option on the homepage. We apologize for the inconvenience"
        );
    }
};

/**
 * Auto-initialize when DOM is ready
 */
$(document).ready(function() {
    ApplyForm.init();
});

/**
 * Currently invoked by apply-loading.js after the Apply form partial is loaded via AJAX
 * so that unobtrusive validation and event handlers are re-wired on the new DOM.
 */
function initializeApplyFormScripts() {
    ApplyForm.init();
}