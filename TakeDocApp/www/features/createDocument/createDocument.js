'use strict';
takeDoc.controller('createDocumentController', ['$scope', '$rootScope', '$location', function ($scope, $rootScope, $location) {

    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        $rootScope.myTakeDoc = new DocumentComplete();
        $rootScope.myTakeDoc.set("DocumentLabel", ($rootScope.User.CurrentTypeDocument != null) ? $rootScope.User.CurrentTypeDocument.get("label") : "");
        $rootScope.myTakeDoc.set("EntityId", $rootScope.User.CurrentEntity.Id);
        $rootScope.myTakeDoc.set("UserCreateData", $rootScope.User.Id);
        $rootScope.myTakeDoc.set("DocumentTypeId", $rootScope.User.CurrentTypeDocument.get("id"));
        $rootScope.myTakeDoc.set("DocumentPageNeed", ($rootScope.User.CurrentTypeDocument != null) ? $rootScope.User.CurrentTypeDocument.get("pageNeed") : true);
	});

    $scope.doCheck = function () {
        var ok = !($rootScope.myTakeDoc.get("DocumentLabel") == "");
        var success = function () {
            $rootScope.myTakeDoc.set("DocumentId", arguments[0].document.get("id"));
            $rootScope.myTakeDoc.set("DocumentCurrentVersionId", arguments[0].document.get("versionId"));
            $location.path($rootScope.Scenario.next().to.substr(2));
            if (!$scope.$$phase) $scope.$apply();
        };
        var error = function () {
            $rootScope.PopupHelper.show("Document", "Une erreur est survenue lors de l'enregistrement.");
        };
        if (ok == false) {
            $rootScope.PopupHelper.show("Document", "Veuillez saisir un nom de document.");
        }
        else documentService.create($rootScope.myTakeDoc, success, error);
        return false;
    }
}]);
