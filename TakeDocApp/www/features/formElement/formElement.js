'use strict';
takeDoc.controller('formElementController', ['$scope', '$rootScope', function ($scope, $rootScope) {

    $scope.title = "Eau froide";
    $scope.description = "Température relevé au robinet aprs 30 secondes d'écoulement d'eau froide. Doit être supérieur à 20°C."
    $scope.typeData = "text";
    $scope.label = "Température";
    $scope.textValue = "valeur";
    $scope.observation = "";
    $scope.mandatory = true;

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
