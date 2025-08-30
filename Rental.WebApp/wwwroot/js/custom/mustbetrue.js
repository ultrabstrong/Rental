(function ($) {
    if (!$.validator) return;

    $.validator.addMethod('mustbetrue', function (value, element) {
        if (element.type === 'checkbox') {
            return element.checked === true;
        }
        return value === true || value === 'true';
    });

    $.validator.unobtrusive.adapters.addBool('mustbetrue');
})(jQuery);
