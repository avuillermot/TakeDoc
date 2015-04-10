'use strict';
backOffice.controller('accountController', ['$scope', '$rootScope', function ($scope, $rootScope) {

    $scope.user = $rootScope.getUser();
    $scope.group = $rootScope.getGroup();
    
    // if external account, all input are readonly
    if ($scope.user.ExternalAccount == true) {
        $("#divAccountInfo div input").attr("readonly", "");
    }

    // return false if an error
    var setStateInputField = function (divId) {
        var elems = $("#" + divId + " input[mandatory='true']");
        $("#"+divId+" div.has-error").removeClass("has-error");

        var i = 0;
        $.each(elems, function (index, value) {
            if (value.value == "") {
                $("#div" + value.id).addClass("has-error");
                i++;
            }
        });

        return !(i > 0);
    };

    // reset data to origine
    $scope.doReset = function () {
        $scope.user = $rootScope.getUser();
        $scope.group = $rootScope.getGroup();
    };

    // save user
    $scope.doSaveUser = function () {
        var ok = setStateInputField("divAccountInfo");

        if ($scope.user.Email == undefined) {
            $("#divinputEmail").addClass("has-error");
            ok = false;
        }

        if (ok == true) {

            var success = function () {
                $rootScope.hideLoader();

                var userToUpdate = $rootScope.getUser();
                userToUpdate.Id = $scope.user.Id;
                userToUpdate.FirstName = $scope.user.FirstName;
                userToUpdate.LastName = $scope.user.LastName;
                userToUpdate.Email = $scope.user.Email;
                userToUpdate.Culture = $scope.user.Culture;
                $rootScope.setUser(userToUpdate);
            };

            var error = function () {
                var data = arguments[0].responseJSON;
                $rootScope.hideLoader();
                $rootScope.showModal("Erreur", data.Message);
            };

            var user = {
                userId: $scope.user.Id,
                firstName: $scope.user.FirstName,
                lastName: $scope.user.LastName,
                email: $scope.user.Email,
                culture: $scope.user.Culture
            };
            $rootScope.showLoader("Mise à jour....");
            userTkService.update(user, success, error);
        }
    };

    $scope.doUpdatePassword = function () {
        var ok = setStateInputField("divPasswordUpdate");
        var older = $("#inputOlderPassword").val();
        var new1 = $("#inputNewPassword1").val();
        var new2 = $("#inputNewPassword2").val();
        if (new1 != new2) $rootScope.showModal("Erreur","Votre saisie de mot de passe n'est pas identique.");
    };
}]);