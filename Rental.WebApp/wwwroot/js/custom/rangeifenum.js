// Unobtrusive adapter for rangeifenum
$.validator.unobtrusive.adapters.add('rangeifenum', ['minvalue', 'maxvalue', 'checkifname', 'checkifvalue'], function (options) {
    options.rules['rangeifenum'] = options.params;
    options.messages['rangeifenum'] = options.message;
});

$.validator.addMethod('rangeifenum', function (value, element, parameters) {
    var checkifvalue = (parameters.checkifvalue == null ? '' : parameters.checkifvalue).toString();
    var underscoreIndex = element.id.indexOf('_');
    var childinstancename = underscoreIndex >= 0 ? element.id.substring(0, underscoreIndex + 1) : '';
    var actualvalue = $('#' + childinstancename + parameters.checkifname).val();
    if (ValidationUtils.normalize(checkifvalue) === ValidationUtils.normalize(actualvalue)) {
        if (value == null || String(value).trim() === '') {
            return false; // empty not allowed when condition matches
        }
        var valdec = parseFloat(value);
        if (isNaN(valdec)) {
            return false;
        }
        if (parameters.minvalue) {
            var mindec = parseFloat(parameters.minvalue);
            if (!isNaN(mindec) && valdec < mindec) {
                return false;
            }
        }
        if (parameters.maxvalue) {
            var maxdec = parseFloat(parameters.maxvalue);
            if (!isNaN(maxdec) && valdec > maxdec) {
                return false;
            }
        }
        return true;
    }
    return true;
});
