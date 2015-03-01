'use strict';
takeDoc.controller('selectTypeDocumentController', ['$scope', '$rootScope', function ($scope, $rootScope) {
    var step = $rootScope.Scenario.next();

    $scope.nextUrl = step.to;
    var success = function () {
        $scope.TypeDocuments = arguments[0].value;
    };

    var error = function () {
        alert("mm");
    };
    typeDocumentService.get($rootScope.User.CurrentEntity, success, error);
}]);

