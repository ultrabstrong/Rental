function showNotificationModal(headertext, bodytext) {
    $('#notificationModalHeader').text(headertext);
    $('.modal-body').text(bodytext);
    $('#notificationModal').modal('show');
}

function showStatusModal(title, message) {
    var deferred = $.Deferred();

    $('#statusModalLabel').text(title);
    $('#statusModal .modal-body').text(message);

    $('#statusModal').modal('show').on('shown.bs.modal', function () {
        $(this).off('shown.bs.modal');
        deferred.resolve();
    });

    return deferred.promise();
}

function hideStatusModal() {
    var deferred = $.Deferred();

    $('#statusModal').modal('hide').on('hidden.bs.modal', function () {
        deferred.resolve(); 
    });

    return deferred.promise(); 
}