/**
 * Maintenance Request Form JavaScript Module
 * Handles all functionality for the maintenance request form
 */
var MaintenanceForm = {
    /**
     * Initialize the Maintenance form functionality
     */
    init: function() {
        if (!$('#application-form').length) {
            return; // Exit if the maintenance form is not present
        }
        
        this.initializeMasking();
        this.initializeValidation();
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
     * Initialize form submission handling
     */
    initializeFormSubmission: function() {
        var self = this;
        
        $('#application-form').submit(async function(e) {
            e.preventDefault();
            
            if (!$(e.target).valid()) {
                return;
            }

            var formData = $(e.target).serialize();

            await showStatusModal("Sending Maintenance Request", "Sending maintenance request...");
            
            $.ajax({
                type: 'POST',
                url: $('#application-form').attr('action'),
                data: formData
            }).done(async function(response) {
                await hideStatusModal();
                
                if (response.hasValidationErrors) { 
                    // Handle validation errors
                    return; 
                }
                
                if (response.isSuccess) {
                    showNotificationModal("Maintenance request sent", "Your maintenance request has been sent");
                    $('#notificationModal').one('hidden.bs.modal', function() {
                        // Clear form so data doesn't display when back button clicked
                        $('#application-form')[0].reset();
                        window.location.href = response.redirectUrl;
                    });
                } else { 
                    self.showSubmitError(); 
                }
            }).fail(async function() {
                await hideStatusModal();
                self.showSubmitError();
            });
        });
    },

    /**
     * Show error message when form submission fails
     */
    showSubmitError: function() {
        // Get the company email from the page data attribute
        var companyEmail = $('body').data('company-email') || 'management@company.com';
        
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

/**
 * Global function for backward compatibility
 * This maintains compatibility with any existing code that calls initializeMaintenanceFormScripts()
 */
function initializeMaintenanceFormScripts() {
    MaintenanceForm.init();
}