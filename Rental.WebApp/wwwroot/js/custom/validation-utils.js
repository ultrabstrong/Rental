// Common validation utility helpers
// Loaded before custom validator rule files.
window.ValidationUtils = window.ValidationUtils || {
    normalize: function (v) {
        return String(v ?? '').trim().toLowerCase();
    }
};
