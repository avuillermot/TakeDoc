'use strict';
takeDoc.controller('selectTypeDocumentController', ['$scope', '$rootScope', function ($scope, $rootScope) {
    var step = $rootScope.Scenario.next();

    $scope.nextUrl = step.to;
    $scope.TypeDocuments = typeDocumentService.get(null);
}]);

