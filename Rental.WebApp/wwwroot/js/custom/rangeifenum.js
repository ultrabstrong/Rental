// Unobtrusive adapter for rangeifenum
$.validator.unobtrusive.adapters.add('rangeifenum', ['minvalue', 'maxvalue', 'checkifname', 'checkifvalue'], function (options) {
    options.rules['rangeifenum'] = options.params;
    options.messages['rangeifenum'] = options.message;
});

$.validator.addMethod('rangeifenum', function (value, element, parameters) {
    const expected = parameters.checkifvalue;
    const idx = element.id.indexOf('_');
    const prefix = idx >= 0 ? element.id.substring(0, idx + 1) : '';
    const actual = $('#' + prefix + parameters.checkifname).val();
    if (ValidationUtils.normalize(expected) !== ValidationUtils.normalize(actual)) return true;

    if (value == null || String(value).trim() === '') return false;
    const num = parseFloat(value);
    if (Number.isNaN(num)) return false;

    if (parameters.minvalue) {
        const min = parseFloat(parameters.minvalue);
        if (!Number.isNaN(min) && num < min) return false;
    }
    if (parameters.maxvalue) {
        const max = parseFloat(parameters.maxvalue);
        if (!Number.isNaN(max) && num > max) return false;
    }
    return true;
});
