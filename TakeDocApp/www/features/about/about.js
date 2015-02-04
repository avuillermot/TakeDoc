'use strict';
takeDoc.controller('aboutController', ['$scope', '$rootScope', 'aboutService', function ($scope, $rootScope, aboutService) {
    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        $rootScope.hideBackButton = false;
    });
}]);



takeDoc.service('aboutService', ['$http', function ($http) {
}]);