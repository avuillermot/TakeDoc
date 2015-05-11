'use strict';
backOffice.controller('accountController', ['$scope', '$rootScope', '$stateParams', '$location', 'refreshDetail', 'groups', function ($scope, $rootScope, $stateParams, $location, refreshDetail, groups) {

    var userToDisplay = $stateParams.user;
    var userEntity = new UserEntitys();
    $scope.user = null;

    $scope.isBackofficeUser = $rootScope.isBackofficeUser();

    var displayContext = function () {
        // if external account, all input are readonly
        if ($scope.user.ExternalAccount == true) $("#divAccountInfo div input").attr("readonly", "");
        if ($scope.isBackofficeUser == false) $("#divAccountInfo div #inputEmail").attr("readonly", "");

        var success = function () {
            $scope.searchUserName = arguments[0];
            if (!$scope.$$phase) $scope.$apply();
        };
        var error = function () {

        };
        if ($scope.user.ManagerId != null) {
            $scope.searchUserId = $scope.user.ManagerId
            userTkService.getName($scope.user.ManagerId, success, error);
        }

    }

    // init group list from combo
    $scope.$watch(function () { return groups.data.calls; }, function () {
        $scope.groups = groups.data.groups;
    });

    // get data of current user
    var fetchUser = function () {
        if (userToDisplay == "current") {
            $scope.user = $rootScope.getUser();
            displayContext();
        }
        else {
            var success = function () {
                var current = new userTk(arguments[0]);
                $scope.user = current;
                displayContext();
                if (!$scope.$$phase) $scope.$apply();
            };
            var error = function () {
                $rootScope.showModal("Erreur", "Utilisateur indisponible ou inconnu.")
            };
            userTkService.get(userToDisplay, success, error);
        }
    };

    fetchUser();

    // reset data to origine
    $scope.doGoSeach = function () {
        $location.path("/searchUsers");
    };

    // reset data to origine
    $scope.doReset = function () {
        fetchUser();
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
                if (userToDisplay == "current") {
                    var userToUpdate = $rootScope.getUser();
                    userToUpdate.Id = $scope.user.Id;
                    userToUpdate.FirstName = $scope.user.FirstName;
                    userToUpdate.LastName = $scope.user.LastName;
                    userToUpdate.Email = $scope.user.Email;
                    userToUpdate.Culture = $scope.user.Culture;
                    userToUpdate.Enable = $scope.user.Enable;
                    userToUpdate.Activate = $scope.user.Activate;
                    userToUpdate.ManagerId = ($scope.searchUserName != null && $scope.searchUserName != "") ? $scope.searchUserId : null;
                    $rootScope.setUser(userToUpdate);
                }
            };

            var error = function () {
                var data = arguments[0];
                $rootScope.hideLoader();
                $rootScope.showError(data);
            };

            var user = {
                userId: $scope.user.Id,
                firstName: $scope.user.FirstName,
                lastName: $scope.user.LastName,
                email: $scope.user.Email,
                culture: $scope.user.Culture,
                enable: $scope.user.Enable,
                activate: $scope.user.Activate,
                groupId: $scope.user.GroupId,
                managerId: ($scope.searchUserName != null && $scope.searchUserName != "") ? $scope.searchUserId : null
            };
            $rootScope.showLoader("Mise à jour....");
            userTkService.update(user, success, error);
        }
    };

    $scope.doGeneratePassword = function () {
        var param = {
            userId: $scope.user.Id,
            success: function () {
                $rootScope.hideLoader();
                $rootScope.showModal("information","Un nouveau mot de passe a été généré. Un mail contenant le nouveau mote de passe a été envoyé à l'utilisateur.");
            },
            error: function () {
                $rootScope.hideLoader();
                $rootScope.showError(arguments[0]);
            }
        };
        $rootScope.showLoader();
        userTkService.generatePassword(param);
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
            refreshDetail.data.calls = refreshDetail.data.calls + 1;
            $scope.$apply();
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

    /* change groupe */
    $scope.doChangeGroup = function () {
        $("#modalAccountGroup").modal("show");
        var group = $scope.groups.where({ id: $scope.user.GroupId });
        if (group != null && group.length > 0) $scope.selectedGroup = group[0];
    };

    $scope.doSelectedGroup = function () {
        var group = $scope.groups.where({ id: this.group.get("id") });
        if (group != null && group.length > 0) $scope.selectedGroup = group[0];
    };

    $scope.doValidSelectedGroup = function () {
        $scope.user.GroupId = $scope.selectedGroup.get("id");
        $scope.user.GroupLabel = $scope.selectedGroup.get("label");
        $scope.user.GroupReference = $scope.selectedGroup.get("reference");
        $("#modalAccountGroup").modal("hide");
    };
}]);