'use strict';
takeDoc.controller('createDocumentController', ['$scope', '$rootScope', function ($scope, $rootScope) {

    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        $rootScope.myTakeDoc = new TkDocument();
        $rootScope.myTakeDoc.set("DocumentLabel", ($rootScope.User.CurrentTypeDocument != null) ? $rootScope.User.CurrentTypeDocument.TypeDocumentLabel : "");
        $rootScope.myTakeDoc.set("EntityId", $rootScope.User.CurrentEntity.Id);
        $rootScope.myTakeDoc.set("UserCreateData", $rootScope.User.Id);
        $rootScope.myTakeDoc.set("DocumentTypeId", $rootScope.User.CurrentTypeDocument.TypeDocumentId);

		var step = $rootScope.Scenario.next();
		$scope.nextUrl = step.to;
	});

    $scope.doCheck = function () {
        var ok = !($rootScope.myTakeDoc.get("DocumentLabel") == "");
        if (ok == false) {
            $rootScope.PopupHelper.show("Document", "Veuillez saisir un nom de document.");
        }
        return ok;
    }
}]);
