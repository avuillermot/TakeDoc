'use strict';
takeDoc.controller('selectEntityController', ['$scope', '$rootScope', function ($scope, $rootScope) {
    var servEntity = new entityService();
    var step = $rootScope.Scenario.next();

    $scope.nextUrl = step.to;
    $scope.Entitys = servEntity.get(null);

    $scope.onChoose = function (entityId) {
        $rootScope.User.CurrentEntity = entityId;
    }
}]);

