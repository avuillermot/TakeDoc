'use strict';
takeDoc.controller('createDocumentController', ['$scope', '$rootScope', function ($scope, $rootScope) {

	$scope.$on("$ionicView.beforeEnter", function (scopes, states) {
		var step = $rootScope.Scenario.next();
		$scope.nextUrl = step.to;
	});

    $scope.doCheck = function () {
        var ok = !($rootScope.documentToAdd.DocumentLabel == "");
        if (ok == false) {
            $rootScope.ErrorHelper.show("Document", "Veuillez saisir un nom de document.");
        }
        return ok;
    }
}]);
