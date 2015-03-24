'use strict';
takeDoc.controller('createDocumentController', ['$scope', '$rootScope', function ($scope, $rootScope) {

    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        $rootScope.myTakeDoc = new documents();
        $rootScope.myTakeDoc.Pages = new Pictures();
        $rootScope.myTakeDoc.DocumentLabel = ($rootScope.User.CurrentTypeDocument != null) ? $rootScope.User.CurrentTypeDocument.TypeDocumentLabel : "";
        $rootScope.myTakeDoc.EntityId = $rootScope.User.CurrentEntity.Id;
        $rootScope.myTakeDoc.UserCreateData = $rootScope.User.Id;
        $rootScope.myTakeDoc.DocumentTypeId = $rootScope.User.CurrentTypeDocument.TypeDocumentId;

		var step = $rootScope.Scenario.next();
		$scope.nextUrl = step.to;
	});

    $scope.doCheck = function () {
        var ok = !($rootScope.myTakeDoc.DocumentLabel == "");
        if (ok == false) {
            $rootScope.PopupHelper.show("Document", "Veuillez saisir un nom de document.");
        }
        return ok;
    }
}]);
