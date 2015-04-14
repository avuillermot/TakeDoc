'use strict';
backOffice.controller('resultUserController', ['$scope', '$rootScope', '$location', 'usersResult', function ($scope, $rootScope, $location, usersResult) {

    $scope.$watch(function () { return usersResult.data.calls; }, function () {
        $rootScope.hideLoader();
        $scope.gridResultSearchUser.data = usersResult.data.users;
    });

    $scope.showMe = function () {
        var current = arguments[0].entity;
        $location.path("/account/" + current.Id);
    };

   $scope.gridResultSearchUser = {
       enableSorting: true,
       columnDefs: [
          { name: 'Prenom', field: '', cellTemplate: '<button class="btn btn-info btn-xs glyphicon glyphicon-pencil" ng-click="grid.appScope.showMe(row)"></button>&#160;{{row.entity.FirstName}}' },
          { name: 'Nom', field: 'LastName' },
          { name: 'Email', field: 'Email' }
        ],
        data: usersResult.data.users
    };

}]);