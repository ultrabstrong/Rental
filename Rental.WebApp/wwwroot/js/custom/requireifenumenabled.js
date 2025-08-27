// Unobtrusive adapter for requireifenumenabled
$.validator.unobtrusive.adapters.add('requireifenumenabled', ['checkifname', 'checkifvalue', 'ischeckenabled'], function (options) {
    options.rules['requireifenumenabled'] = options.params;
    options.messages['requireifenumenabled'] = options.message;
});

$.validator.addMethod('requireifenumenabled', function (value, element, parameters) {
    // Skip if element is disabled or hidden (improves UX; jQuery validate usually ignores hidden unless overridden)
    if (element.disabled) return true;
    if (!$(element).is(':visible')) return true;

    // Determine the model prefix (asp-for generates ids like Parent_Property or index_Property)
    const idx = element.id.indexOf('_');
    const prefix = idx >= 0 ? element.id.substring(0, idx + 1) : '';

    // Hidden input for the enabling boolean (AllowElectiveRequire)
    const enabledFieldId = prefix + parameters.ischeckenabled;
    const enabled = (document.getElementById(enabledFieldId)?.value || '').toLowerCase() === 'true';
    if (!enabled) return true; // Feature not enabled => no validation

    const expected = parameters.checkifvalue;
    const actualFieldId = prefix + parameters.checkifname;
    const actual = $('#' + actualFieldId).val();

    if (ValidationUtils.normalize(expected) !== ValidationUtils.normalize(actual)) return true;

    if (value == null) return false;
    if (typeof value === 'string' && value.trim() === '') return false;
    if ($.isArray(value) && value.length === 0) return false;
    return true;
});
