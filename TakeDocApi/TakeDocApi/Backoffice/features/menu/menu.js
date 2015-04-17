'use strict';
backOffice.controller('menuController', ['$scope', '$rootScope', function ($scope, $rootScope) {

    $scope.$on("$viewContentLoaded", function (scopes) {
        if ($rootScope.getUser() != null) {
            $scope.UserFullName = $rootScope.getUser().FirstName + " " + $rootScope.getUser().LastName;
            $scope.GroupReference = $rootScope.getUser().GroupReference;
        }
    });
}]);
