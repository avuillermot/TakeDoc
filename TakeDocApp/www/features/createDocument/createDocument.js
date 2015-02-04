'use strict';
takeDoc.controller('createDocumentController', ['$scope', '$rootScope', 'createDocumentService', function ($scope, $rootScope, createDocumentService) {
    $rootScope.documentToAdd = new documents();
    $rootScope.hideBackButton = true;

    var step = $rootScope.Scenario.next();

    $scope.nextUrl = step.to;
    $rootScope.documentToAdd.EntityId = $rootScope.User.CurrentEntity.Id;

    $scope.setLabel = function () {
        $rootScope.documentToAdd.DocumentLabel = $("#documentLabel").val();
    };

    $scope.doCheck = function () {
        var ok = !($("#documentLabel").val() == "");
        if (ok == false) {
            $rootScope.ErrorHelper.show("Erreur", "Veuillez saisir un nom de document.");
        }
        return ok;
    }
}]);



takeDoc.service('createDocumentService', ['$http', function ($http) {

}]);

