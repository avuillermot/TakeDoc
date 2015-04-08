'use strict';
backOffice.controller('menuController', ['$scope', '$rootScope', function ($scope, $rootScope) {

    $scope.$on("$viewContentLoaded", function (scopes) {
        $scope.UserFullName = $rootScope.getUser().FirstName + " " + $rootScope.getUser().LastName;
        $scope.GroupReference = $rootScope.getGroup().reference;
    });
}]);
