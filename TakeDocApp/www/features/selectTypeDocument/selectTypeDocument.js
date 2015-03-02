'use strict';
takeDoc.controller('selectTypeDocumentController', ['$scope', '$rootScope', function ($scope, $rootScope) {
    var step = $rootScope.Scenario.next();

    $scope.nextUrl = step.to;
    var success = function () {
        $scope.TypeDocuments = arguments[0].value;
    };

    var error = function () {
        $rootScope.ErrorHelper.show("Type de documents", "La liste des types de document n'est pas disponibles.");
    };
    typeDocumentService.get($rootScope.User.CurrentEntity, success, error);
}]);

