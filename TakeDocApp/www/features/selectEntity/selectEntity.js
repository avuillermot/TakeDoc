'use strict';
takeDoc.controller('selectEntityController', ['$scope', '$rootScope', function ($scope, $rootScope) {
    var step = $rootScope.Scenario.next();

    $scope.nextUrl = step.to;
    $scope.Entitys = $rootScope.User.Entitys;

    $scope.onChoose = function (entityId) {
        $rootScope.User.CurrentEntity = entityId;
    }
}]);

