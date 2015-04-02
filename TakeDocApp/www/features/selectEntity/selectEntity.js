'use strict';
takeDoc.controller('selectEntityController', ['$scope', '$rootScope', '$location', function ($scope, $rootScope, $location) {

    var status = null;

    $scope.onChoose = function (entityId) {
        $.each($rootScope.User.Entitys, function (index, value) {
            if (value.Id == entityId) $rootScope.User.CurrentEntity = value;
        });
        $location.path($scope.nextUrl.replace("#/", ""));
    };

    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        $scope.Entitys = $rootScope.User.Entitys;
		
		var step = $rootScope.Scenario.next();
		$scope.nextUrl = step.to;

		if ($scope.Entitys.length == 1) {
		    $rootScope.User.CurrentEntity = $scope.Entitys[0];
		    $location.path($scope.nextUrl.replace("#/", ""));
		}
		status = states.stateParams.status;
    });

    $scope.countStatus = function (entityId) {
        if (status != null && status != "") {
            return "("+ $rootScope.Dashboards.countStatusEntity(entityId, status) + ")";
        }
        else return "";
    };
    
}]);

