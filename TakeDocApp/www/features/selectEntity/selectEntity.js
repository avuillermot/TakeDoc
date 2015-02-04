'use strict';
takeDoc.controller('selectEntityController', ['$scope', '$rootScope', 'selectEntityService', function ($scope, $rootScope, selectEntityService) {
    var step = $rootScope.Scenario.next();
    $scope.nextUrl = step.to;
    $scope.Entitys = $rootScope.User.Entitys;
    $rootScope.User.CurrentEntity = $scope.Entitys[0];
}]);



takeDoc.service('selectEntityService', ['$http', function ($http) {

}]);

