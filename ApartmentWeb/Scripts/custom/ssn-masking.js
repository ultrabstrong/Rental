function formatSSN(inputElement, maskValue = false) {
    let digits = inputElement.value.replace(/\D/g, ''); // Remove non-digits
    let formattedInput = '';

    if (digits.length > 0) {
        formattedInput = digits.substring(0, 3);
    }
    if (digits.length >= 4) {
        formattedInput += '-' + digits.substring(3, 5);
    }
    if (digits.length >= 6) {
        formattedInput += '-' + digits.substring(5, 9);
    }

    // Prevent further input if max length (9 digits) is reached
    if (digits.length > 9) {
       formattedInput = formattedInput.substring(0, 11); // Keep the formatted version up to 11 chars: XXX-XX-XXXX
       digits = digits.substring(0, 9); // Keep only 9 digits
    }

    // Apply visual mask (XXX-XX-XXXX) if requested and complete
    if (maskValue && digits.length === 9) {
        inputElement.value = 'XXX-XX-' + digits.substring(5, 9);
        // Store the real formatted value
        inputElement.dataset.realSsn = formattedInput;
    } else {
        inputElement.value = formattedInput;
        // Clear any stored real value if not masking
        if (inputElement.dataset.realSsn) {
            delete inputElement.dataset.realSsn;
        }
    }
}

function applySSNMasking() {
    const ssnInputs = document.querySelectorAll('input.ssn-mask');

    ssnInputs.forEach(ssnInput => {
        // Format as user types
        ssnInput.addEventListener('input', function (e) {
            formatSSN(e.target, false); // Don't apply visual mask while typing
        });

        // Reveal full SSN on focus
        ssnInput.addEventListener('focus', function (e) {
            if (e.target.dataset.realSsn) {
                e.target.value = e.target.dataset.realSsn;
                delete e.target.dataset.realSsn; // Clear stored value
            }
            // Reformat in case focus happened without blur (e.g., tabbing)
            formatSSN(e.target, false);
        });

        // Apply visual mask on blur if SSN is complete
        ssnInput.addEventListener('blur', function (e) {
            formatSSN(e.target, true); // Apply visual mask if complete
        });

        // Format on initial load (without visual mask)
        if (ssnInput.value) {
            formatSSN(ssnInput, false);
            // Apply initial blur mask if value is complete on load
            formatSSN(ssnInput, true);
        }
    });
}

// Initialize the masking on page load
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', applySSNMasking);
} else {
    // DOMContentLoaded has already fired
    applySSNMasking();
}