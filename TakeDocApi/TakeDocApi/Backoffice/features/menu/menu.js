'use strict';
backOffice.controller('menuController', ['$scope', '$rootScope', 'groups', 'inputTypes', function ($scope, $rootScope, groups, inputTypes) {

    $scope.$on("$viewContentLoaded", function (scopes) {
        if ($rootScope.getUser() != null) {
            $scope.UserFullName = $rootScope.getUser().FirstName + " " + $rootScope.getUser().LastName;
            $scope.GroupReference = $rootScope.getUser().GroupReference;
        }
    });
}]);
