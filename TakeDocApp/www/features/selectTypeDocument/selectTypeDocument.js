'use strict';
takeDoc.controller('selectTypeDocumentController', ['$scope', '$rootScope', '$location', '$ionicLoading', function ($scope, $rootScope, $location, $ionicLoading) {

    $scope.$on("$ionicView.beforeEnter", function (scopes, states)  {
        $scope.nextUrl = $rootScope.Scenario.next().to;
    });

    $scope.$on("$ionicView.afterEnter", function (scopes, states) {
        $ionicLoading.show({
            template: 'Chargement...'
        }); 

        $scope.TypeDocuments = null;
        typeDocumentService.get($rootScope.User.CurrentEntity.Id, success, error);
    });

    $scope.searchTypeDocument = function () {
        return (arguments[0].EtatDeleteData == false);
    };

    var success = function () {
        $scope.TypeDocuments = arguments[0].value;
        var nb = 0;
        $.each($scope.TypeDocuments, function(index, value) {
            if (value.EtatDeleteData == false)nb++;
        });

        if (nb == 0) {
            $rootScope.PopupHelper.show("Type de documents", "Aucun type de document disponible");
            $location.path("menu");
        }
        $ionicLoading.hide();
    };

    var error = function () {
        $scope.TypeDocuments = null;
        $ionicLoading.hide();
        $rootScope.PopupHelper.show("Type de documents", "La liste des types de document n'est pas disponibles.");
    };
	
    $scope.onChoose = function (typeDocumentId) {
        $.each($scope.TypeDocuments, function (index, value) {
            if (value.TypeDocumentId == typeDocumentId) $rootScope.User.CurrentTypeDocument = value;
        });
        $location.path($scope.nextUrl.replace("#/", ""));
    };
}]);

