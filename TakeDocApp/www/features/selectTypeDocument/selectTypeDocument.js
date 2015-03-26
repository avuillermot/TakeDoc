'use strict';
takeDoc.controller('selectTypeDocumentController', ['$scope', '$rootScope', '$location', '$ionicLoading', '$stateParams', function ($scope, $rootScope, $location, $ionicLoading, $stateParams) {

    var mode = null;
    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        $scope.nextUrl = $rootScope.Scenario.next().to;
    });

    $scope.$on("$ionicView.afterEnter", function (scopes, states) {
        mode = states.stateParams.mode;
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
        var types = arguments[0].value;
        var nb = 0;
        $.each(types, function(index, value) {
            if (value.EtatDeleteData == false)nb++;
        });

        if (nb == 0) {
            $rootScope.PopupHelper.show("Type de documents", "Aucun type de document disponible");
            $location.path("menu");
        }
        if (mode == "search") {
            var all = {
                TypeDocumentId: "",
                EntityId: $rootScope.User.CurrentEntity.Id,
                TypeDocumentLabel: "(Tous)",
                TypeDocumentReference: "",
                EtatDeleteData: false
            };
            arguments[0].value.push(all);
        }

        $scope.TypeDocuments = types;

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

