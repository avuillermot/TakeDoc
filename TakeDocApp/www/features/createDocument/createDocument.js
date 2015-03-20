'use strict';
takeDoc.controller('createDocumentController', ['$scope', '$rootScope', function ($scope, $rootScope) {

    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        $rootScope.documentToAdd = new documents();
        $rootScope.documentToAdd.Pages = new Pictures();
        $rootScope.documentToAdd.DocumentLabel = ($rootScope.User.CurrentTypeDocument != null) ? $rootScope.User.CurrentTypeDocument.TypeDocumentLabel : "";
        $rootScope.documentToAdd.EntityId = $rootScope.User.CurrentEntityId;
        $rootScope.documentToAdd.UserCreateData = $rootScope.User.Id;
        $rootScope.documentToAdd.DocumentTypeId = $rootScope.User.CurrentTypeDocument.TypeDocumentId;

		var step = $rootScope.Scenario.next();
		$scope.nextUrl = step.to;
	});

    $scope.doCheck = function () {
        var ok = !($rootScope.documentToAdd.DocumentLabel == "");
        if (ok == false) {
            $rootScope.PopupHelper.show("Document", "Veuillez saisir un nom de document.");
        }
        return ok;
    }
}]);
