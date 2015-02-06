'use strict';
takeDoc.controller('selectTypeDocumentController', ['$scope', '$rootScope', function ($scope, $rootScope) {
    debugger;
    var servTypeDocument = new typeDocumentService();
    //var step = $rootScope.Scenario.next();

    //$scope.nextUrl = step.to;
    $scope.TypeDocuments = servTypeDocument.get(null);
}]);

