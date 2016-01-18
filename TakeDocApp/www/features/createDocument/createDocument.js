'use strict';
takeDoc.controller('createDocumentController', ['$scope', '$rootScope', '$location', '$ionicLoading', function ($scope, $rootScope, $location, $ionicLoading) {

    $scope.myDoc = new DocumentComplete();
    $scope.myDoc.document = new DocumentExtended();
    var context = {};

    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        $scope.loading = false;

        context.typeDocumentId = $rootScope.User.CurrentTypeDocument.get("id");
        context.userId = $rootScope.User.Id;
        context.entityId = $rootScope.User.CurrentEntity.Id;
        $scope.myDoc.document.set("label", $rootScope.User.CurrentTypeDocument.get("label"));
	});

    $scope.doCheck = function () {
        var ok = !($scope.myDoc.document.get("label") == "" || $scope.myDoc.document.get("label") == null);
        var success = function () {
            var versionId = arguments[0].document.get("id");
            var entityId = arguments[0].document.get("entityId");
             var go = $rootScope.Scenario.next().to.substr(2) + versionId + "/entity/" + entityId;
            $location.path(go);
            $scope.loading = false;
            if (!$scope.$$phase) $scope.$apply();
        };
        var error = function () {
            $scope.loading = false;
            if (!$scope.$$phase) $scope.$apply();

            $rootScope.PopupHelper.show("Document", "Une erreur est survenue lors de l'enregistrement.");
        };
        context.success = success;
        context.error = error;
        context.label = $scope.myDoc.document.get("label");

        if (ok == false) {
            $rootScope.PopupHelper.show("Document", "Veuillez saisir un nom de document.");
        }
        else {
            $scope.loading = true;
            if (!$scope.$$phase) $scope.$apply();
            $scope.myDoc.add(context);
        }
        return false;
    }
}]);
