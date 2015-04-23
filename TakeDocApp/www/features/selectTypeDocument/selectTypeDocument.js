'use strict';
takeDoc.controller('selectTypeDocumentController', ['$scope', '$rootScope', '$location', '$ionicLoading', '$stateParams', '$timeout', function ($scope, $rootScope, $location, $ionicLoading, $stateParams, $timeout) {

    var mode = null;
    var status = null;
    var typeDocuments = null;

    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        $scope.nextUrl = $rootScope.Scenario.next().to;
    });

    $scope.$on("$ionicView.afterEnter", function (scopes, states) {
        typeDocuments = new TypeDocuments();
        mode = states.stateParams.mode;
        status = states.stateParams.status;

        $ionicLoading.show({
            template: 'Chargement...'
        }); 

        $scope.TypeDocuments = null;
        var param = {
            entityId: $rootScope.User.CurrentEntity.Id, 
            success: success, 
            error: error
        };
        typeDocuments.load(param);
    });

    var success = function () {
        var types = typeDocuments.where({delete:  false});
        if (types.length == 0) {
            $rootScope.PopupHelper.show("Type de documents", "Aucun type de document disponible");
            $location.path("menu");
        }
        if (mode == "SEARCH") {
            var all = new TypeDocument();
            all.set("id", "");
            all.set("reference", "");
            all.set("label", "(Tous)");
            all.set("entityId", $rootScope.User.CurrentEntity.Id);
            all.set("delete", false);

            types.push(all);
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
        if (typeDocumentId == "") {
            var all = new TypeDocument();
            all.set("id", "");
            all.set("reference", "");
            all.set("label", "(Tous)");
            all.set("entityId", $rootScope.User.CurrentEntity.Id);
            all.set("delete", false);
            $rootScope.User.CurrentTypeDocument = all;
        }
        else {
            var current = typeDocuments.where({ id: typeDocumentId });
            $rootScope.User.CurrentTypeDocument = current[0];
        }
        $location.path($scope.nextUrl.replace("#/", ""));
    };

    $scope.countStatus = function (typeDocumentId, entityId) {
        if (status != null && status != "" && typeDocumentId != "" && typeDocumentId != null) {
            return  "("+ $rootScope.Dashboards.countTypeStatusEntity(typeDocumentId, entityId, status) + ")";
        }
        else return "";
    };
}]);

