'use strict';
backOffice.controller('accountController', ['$scope', '$rootScope', '$stateParams', function ($scope, $rootScope, $stateParams) {

    var userToDisplay = $stateParams.user;
    var userEntity = new UserEntitys();

    var displayUser = function () {
        // if external account, all input are readonly
        if ($scope.user.ExternalAccount == true) $("#divAccountInfo div input").attr("readonly", "");
        $scope.$apply();
    }

    // reset data to origine
    $scope.doReset = function () {
        $scope.user = $rootScope.getUser();
    };

    // save user
    $scope.doSaveUser = function () {
        var ok = utils.setStateInputField("divAccountInfo");

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
       
    $scope.user = null;
    if (userToDisplay == "current") {
        $scope.user = $rootScope.getUser();
    }
    else {
        var success = function () {
            var current = new userTk(arguments[0]);
            $scope.user = current;
            displayUser();
        };
        var error = function () {
            $rootScope.showModal("Erreur", "Utilisateur indisponible ou inconnu.")
        };
        userTkService.get(userToDisplay, success, error);

    }

    /* section add entity to user */
    $scope.addEntity = function () {
        $("#modalAccountAddEntity").modal("show");
        $scope.addEntitysList = $rootScope.getUser().Entitys;
        $scope.selectedEntity = $scope.addEntitysList[0];
    }

    $scope.doSelectEntityToAdd = function () {
        $scope.selectedEntity = this.entity;
    }

    $scope.doSaveEntityToAdd = function () {
        $("#modalAccountAddEntity").modal("hide");
       
        var success = function () {

        };

        var error = function () {
            $rootScope.showError(arguments[0]);
        };

        var param = {
            userId: $scope.user.Id,
            entityId: $scope.selectedEntity.Id,
            success: success,
            error: error
        };
        userEntity.addEntityToUser(param);
    };
}]);