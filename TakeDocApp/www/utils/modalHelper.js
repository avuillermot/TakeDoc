function modalHelper(ionicModal, scope, template) {
    this.scope = scope;
    this.ionicModal = ionicModal;

    this.ionicModal.fromTemplateUrl(template, {
        scope: this.scope,
        animation: 'slide-in-up'
    }).then(function(modal) {
        scope.modal = modal;
    });
    this.scope.closeModal = function() {
        scope.modal.hide();
    };
}

modalHelper.prototype.show = function (title, message) {
    this.scope.modal.show();

    if (title != null) $("#modalTitle").html(title);
    if (message != null) $("#modalMessage").html(message);
}

modalHelper.prototype.hide = function () {
    this.scope.modal.hide();
}
