'use strict';
takeDoc.controller('selectTypeDocumentController', ['$scope', '$rootScope', 'selectTypeDocumentService', function ($scope, $rootScope, selectTypeDocumentService) {
    var step = $rootScope.Scenario.next();
    $scope.nextUrl = step.to;
    $scope.TypeDocuments = $rootScope.User.Entitys;
}]);



takeDoc.service('selectTypeDocumentService', ['$http', function ($http) {

}]);

