/**
 * Apply Loading JavaScript Module
 * Handles AJAX loading functionality for the application form
 */
var ApplyLoading = {
    /**
     * Initialize the Apply loading functionality
     */
    init: function() {
        if (!$('#loadingIndicatorContainer').length || !$('#applicationFormContainer').length) {
            return; // Exit if the loading containers are not present
        }
        
        this.loadApplicationForm();
    },

    /**
     * Load the application form via AJAX
     */
    loadApplicationForm: function() {
        var self = this;
        
        // Make an AJAX call to load the Apply.cshtml content
        $.ajax({
            url: self.getApplyFormUrl(),
            type: 'GET',
            success: function(response) {
                self.onLoadSuccess(response);
            },
            error: function(xhr, status, error) {
                self.onLoadError(xhr, status, error);
            }
        });
    },

    /**
     * Get the Apply form URL from the current page
     */
    getApplyFormUrl: function() {
        // Get the URL from data attribute
        var url = $('#loadingIndicatorContainer').data('apply-form-url');
        if (!url) {
            // Fallback URL construction - adjust as needed based on your routing
            url = '/ApplyForm';
        }
        return url;
    },

    /**
     * Handle successful form loading
     */
    onLoadSuccess: function(response) {
        $('#applicationFormContainer').html(response);
        
        // Initialize any scripts that are part of Apply.cshtml
        if (typeof initializeApplyFormScripts === 'function') {
            initializeApplyFormScripts();
        }
        
        // Hide loading indicator and show form using Bootstrap classes
        $('#loadingIndicatorContainer').removeClass('d-flex').addClass('d-none'); 
        $('#applicationFormContainer').removeClass('d-none').addClass('d-flex'); 
        
        // Update title
        this.updatePageTitle();
    },

    /**
     * Handle form loading errors
     */
    onLoadError: function(xhr, status, error) {
        $('#loadingIndicatorContainer').html(
            '<div class="alert alert-danger">Could not load the application form. Please try again later.</div>'
        );
        console.error("Error loading application form:", status, error);
    },

    /**
     * Update the page title after successful load
     */
    updatePageTitle: function() {
        var companyName = $('#loadingIndicatorContainer').data('company-name') || 'Company';
        document.title = companyName + ' - Apply';
    }
};

/**
 * Auto-initialize when DOM is ready
 */
$(document).ready(function() {
    ApplyLoading.init();
});

/**
 * Global function for backward compatibility
 */
function initializeApplyLoadingScripts() {
    ApplyLoading.init();
}