'use strict';
takeDoc.controller('findDocumentController', ['$scope', '$rootScope', '$location', '$ionicLoading', '$timeout', function ($scope, $rootScope, $location, $ionicLoading, $timeout) {

    var documents = null

    var onSuccess = function (collection) {
        $scope.documents = collection.models;
        $ionicLoading.hide();
    };
    var onError = function () {
        $ionicLoading.hide();
        $rootScope.PopupHelper.show("Erreur", "Une erreur est survenue lors de la recherche");
    };

    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        documents = new DocumentsExtended();
        $scope.documents = null;
    });
    
    $scope.$on("$ionicView.afterEnter", function (scopes, states) {
        $(".title.title-center.header-item").html($rootScope.User.CurrentEntity.Label + "/" + $rootScope.User.CurrentTypeDocument.TypeDocumentLabel);

        $ionicLoading.show({
            template: 'Recherche...'
        });
        var params = {
            entityReference: $rootScope.User.CurrentEntity.Reference,
            typeDocumentReference: $rootScope.User.CurrentTypeDocument.TypeDocumentReference,
            success: onSuccess,
            error: onError
        }
        documents.loadComplete(params);

        var step = $rootScope.Scenario.next();
        $scope.nextUrl = step.to;
    });

}]);