'use strict';
backOffice.controller('resultUserController', ['$scope', '$rootScope', '$location', 'usersResult', function ($scope, $rootScope, $location, usersResult) {

    $scope.$watch(function () { return usersResult.data.calls; }, function () {
        $rootScope.hideLoader();
        $scope.gridResultSearchUser.data = usersResult.data.users;
        resizeGridInbox();
    });

    $scope.showMe = function () {
        var toShow = arguments[0].entity;
        $location.path("/account/" + toShow.Id);
    };

    var resizeGridInbox = function () {
        var h = ($(document).height() - 150);
        $("#gridResultSearchUser").css('height', h + 'px');
    };

    $scope.deleteMe = function () {
        var toDel = arguments[0].entity;

        var param = {
            userId: toDel.Id,
            currentUserId: $rootScope.getUser().Id,
            success: function () {
                                
                var myIndex = -1;
                $.each(usersResult.data.users, function (index, value) {
                    if (value.Id == toDel.Id) myIndex = index;
                });
                if (myIndex > -1) {
                    usersResult.data.users.splice(myIndex, 1);
                    usersResult.data.calls = usersResult.data.calls + 1;
                    $scope.$apply();
                }
                $rootScope.hideLoader();

            },
            error: function () {
                $rootScope.hideLoader();
                $rootScope.showModal("Erreur","Il est impossible de supprimer cet utilisateur.");
            }
        }
        $rootScope.showLoader("Suppression en cours...");
        userTkService.delete(param);

    };

   $scope.gridResultSearchUser = {
       enableSorting: true,
       columnDefs: [
          { name: ' ', field: '', cellTemplate: '<button class="btn btn-info btn-xs glyphicon glyphicon-pencil" ng-click="grid.appScope.showMe(row)"></button>&#160;&#160;&#160;&#160;&#160;<button class="btn btn-danger btn-xs glyphicon glyphicon-remove" ng-click="grid.appScope.deleteMe(row)"></button>' },
          { name: 'Nom', field: 'LastName' },
          { name: 'Prenom', field: 'FirstName' },
          { name: 'Email', field: 'Email' }
        ],
        data: usersResult.data.users
    };

}]);