function formatPhoneNumber(inputElement) {
    let input = inputElement.value.replace(/\D/g, '');
    let formattedInput = '';

    if (input.length > 0) {
        formattedInput = '(' + input.substring(0, 3);
    }
    if (input.length >= 4) {
        formattedInput += ') ' + input.substring(3, 6);
    }
    if (input.length >= 7) {
        formattedInput += '-' + input.substring(6, 10);
    }

    if (input.length > 10) {
       formattedInput = inputElement.value.substring(0, 14);
    }

    inputElement.value = formattedInput;
}

function applyPhoneMasking() {
    const phoneInputs = document.querySelectorAll('input.phone-mask');

    phoneInputs.forEach(phoneInput => {
        phoneInput.addEventListener('input', function (e) {
            formatPhoneNumber(e.target);
        });

        if (phoneInput.value) {
            formatPhoneNumber(phoneInput);
        }
    });
}

if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', applyPhoneMasking);
} else {
    applyPhoneMasking();
}