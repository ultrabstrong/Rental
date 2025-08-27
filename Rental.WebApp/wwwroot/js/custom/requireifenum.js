// Unobtrusive adapter for requireifenum
$.validator.unobtrusive.adapters.add('requireifenum', ['checkifname', 'checkifvalue'], function (options) {
    options.rules['requireifenum'] = options.params;
    options.messages['requireifenum'] = options.message;
});

$.validator.addMethod('requireifenum', function (value, element, parameters) {
    const expected = parameters.checkifvalue;
    const idx = element.id.indexOf('_');
    const prefix = idx >= 0 ? element.id.substring(0, idx + 1) : '';
    const actual = $('#' + prefix + parameters.checkifname).val();
    if (ValidationUtils.normalize(expected) !== ValidationUtils.normalize(actual)) return true;

    if (value == null) return false;
    if (typeof value === 'string' && value.trim() === '') return false;
    if ($.isArray(value) && value.length === 0) return false;
    return true;
});
