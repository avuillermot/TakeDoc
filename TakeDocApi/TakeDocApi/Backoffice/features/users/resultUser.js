'use strict';
backOffice.controller('resultUserController', ['$scope', '$rootScope', 'usersResult', function ($scope, $rootScope, usersResult) {

    $scope.$watch(function () { return usersResult.data.calls; }, function () {
        $scope.gridResultSearchUser.data = usersResult.data.users;
    });

   $scope.gridResultSearchUser = {
        enableSorting: true,
        columnDefs: [
          { name: 'Prenom', field: 'UserTkFirstName' },
          { name: 'Nom', field: 'UserTkLastName' }
        ],
        data: usersResult.data.users
    };

}]);