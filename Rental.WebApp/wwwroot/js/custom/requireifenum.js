// Unobtrusive adapter for requireifenum
$.validator.unobtrusive.adapters.add('requireifenum', ['checkifname', 'checkifvalue'], function (options) {
    options.rules['requireifenum'] = options.params;
    options.messages['requireifenum'] = options.message;
});

$.validator.addMethod('requireifenum', function (value, element, parameters) {
    var checkifvalue = (parameters.checkifvalue == null ? '' : parameters.checkifvalue).toString();
    var underscoreIndex = element.id.indexOf('_');
    var childinstancename = underscoreIndex >= 0 ? element.id.substring(0, underscoreIndex + 1) : '';
    var actualvalue = $('#' + childinstancename + parameters.checkifname).val();
    if (ValidationUtils.normalize(checkifvalue) === ValidationUtils.normalize(actualvalue)) {
        return $.validator.methods.required.call(this, value, element, parameters);
    }
    return true;
});
