'use strict';
takeDoc.controller('createDocumentController', ['$scope', '$rootScope', 'createDocumentService', function ($scope, $rootScope, createDocumentService) {

    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        $rootScope.documentToAdd = new documents();
        $rootScope.documentToAdd.DocumentLabel = "";
        $rootScope.documentToAdd.EntityId = $rootScope.User.CurrentEntity.Id;
    });

    var step = $rootScope.Scenario.next();

    $scope.nextUrl = step.to;

    $scope.doCheck = function () {
        var ok = !($rootScope.documentToAdd.DocumentLabel == "");
        if (ok == false) {
            $rootScope.ErrorHelper.show("Erreur", "Veuillez saisir un nom de document.");
        }
        return ok;
    }
}]);



takeDoc.service('createDocumentService', ['$http', function ($http) {

}]);

