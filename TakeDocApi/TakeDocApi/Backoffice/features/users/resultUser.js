'use strict';
backOffice.controller('resultUserController', ['$scope', '$rootScope', 'usersResult', function ($scope, $rootScope, usersResult) {

    $scope.$watch(function () { return usersResult.data.calls; }, function () {
        $rootScope.hideLoader();
        $scope.gridResultSearchUser.data = usersResult.data.users;
    });

   $scope.gridResultSearchUser = {
        enableSorting: true,
        columnDefs: [
          { name: 'Prenom', field: 'FirstName' },
          { name: 'Nom', field: 'LastName' },
          { name: 'Email', field: 'Email' }
        ],
        data: usersResult.data.users
    };

}]);