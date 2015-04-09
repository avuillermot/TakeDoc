'use strict';
backOffice.controller('accountController', ['$scope', '$rootScope', function ($scope, $rootScope) {

    $scope.user = $rootScope.getUser();
    $scope.group = $rootScope.getGroup();

    $scope.doCheck = function () {
        var elems = $("input[mandatory='true']");
        $("#divAccountInfo div.has-error").removeClass("has-error");

        var i = 0;
        $.each(elems, function (index, value) {
            if (value.value == "") {
                $("#div"+value.id).addClass("has-error");
                i++;
            }
        });
    };
}]);