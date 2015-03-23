'use strict';
takeDoc.controller('findDocumentController', ['$scope', '$rootScope', '$location', function ($scope, $rootScope, $location) {

    var documents = new DocumentsExtended();

    var onSuccess = function (collection) {
        $scope.documents = collection.models;
    };
    var onError = function () {
        $rootScope.PopupHelper.show("Erreur", "Une erreur est survenue lors de la recherche");
    };

    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        var step = $rootScope.Scenario.next();
        $scope.nextUrl = step.to;

        documents.fetch({ success: onSuccess, error: onError });
    });

}]);