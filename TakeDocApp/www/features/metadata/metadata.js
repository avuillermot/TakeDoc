'use strict';
takeDoc.controller('metadataController', ['$scope', '$rootScope', '$stateParams', '$route', function ($scope, $rootScope, $stateParams, $route) {

    var step = $rootScope.Scenario.next();
    $scope.nextUrl = step.to;

}]);
