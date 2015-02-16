'use strict';
takeDoc.controller('formElementController', ['$scope', '$rootScope', '$stateParams', '$window', function ($scope, $rootScope, $stateParams, $window) {
    var _routeHelper = new routeHelper();
    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        var myPage = $stateParams.page;

        var aText = new askText();
        var aBox = new askCheckbox();

        $scope.title = aText.title;
        $scope.description = aText.description;
        $scope.typeData = aText.typeData;
        $scope.label = aText.label;
        $scope.textValue = aText.textValue;
        $scope.observation = aText.observation;
        $scope.mandatory = aText.mandatory;

        $scope.nextUrl = $rootScope.Scenario.next().to;

        $window.location.reload(true);
    });

    $scope.doCheck = function () {
        $scope.textValue = $("#valueZone").val();
        var ok = true;
        if ($scope.mandatory) ok = !($scope.textValue == "");
        if (ok == false) {
            $rootScope.ErrorHelper.show($scope.title, "Cette information est obligatoire.");
        }
        return ok;
    };

}]);
