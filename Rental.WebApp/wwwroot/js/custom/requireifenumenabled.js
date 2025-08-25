// Unobtrusive adapter for requireifenumenabled
$.validator.unobtrusive.adapters.add('requireifenumenabled', ['checkifname', 'checkifvalue', 'ischeckenabled'], function (options) {
    options.rules['requireifenumenabled'] = options.params;
    options.messages['requireifenumenabled'] = options.message;
});

$.validator.addMethod('requireifenumenabled', function (value, element, parameters) {
    var ischeckenabled = (parameters.ischeckenabled != null && parameters.ischeckenabled.toString().toLowerCase() === 'true');
    if (ischeckenabled) {
        var checkifvalue = (parameters.checkifvalue == null ? '' : parameters.checkifvalue).toString();
        var underscoreIndex = element.id.indexOf('_');
        var childinstancename = underscoreIndex >= 0 ? element.id.substring(0, underscoreIndex + 1) : '';
        var actualvalue = $('#' + childinstancename + parameters.checkifname).val();
        if (ValidationUtils.normalize(checkifvalue) === ValidationUtils.normalize(actualvalue)) {
            return $.validator.methods.required.call(this, value, element, parameters);
        }
    }
    return true;
});
