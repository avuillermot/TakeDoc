'use strict';
backOffice.controller('passwordController', ['$scope', '$rootScope', function ($scope, $rootScope) {

    $scope.doUpdatePassword = function () {
        var ok = utils.setStateInputField("divPasswordUpdate");
        var older = $("#inputOlderPassword").val();
        var new1 = $("#inputNewPassword1").val();
        var new2 = $("#inputNewPassword2").val();
        if (new1 != new2) $rootScope.showModal("Erreur", "Votre saisie de mot de passe n'est pas identique.");
        else {
            var param = {
                userId: $rootScope.getUser().Id,
                olderPassword: older,
                newPassword: new1
            };
            var success = function () {
                $rootScope.hideLoader();
            };
            var error = function () {
                $rootScope.hideLoader();
                var data = arguments[0].responseJSON;
                $rootScope.showModal("Erreur", data.Message);
            };
            $rootScope.showLoader("Enregistrement en cours...")
            userTkService.changePassword(param, success, error);
        }
    };
}]);