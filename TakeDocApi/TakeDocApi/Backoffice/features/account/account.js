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

        var user = {
            userId: '991C737E-EB93-4BB3-BB21-BE05E7E43585',
            firstName: 'alexandre2',
            lastName: 'vuillermot-rouhana',
            email: 'avuillermot@hotmail.com1',
            culture: 'fr'
        };
        $rootScope.showLoader("Mise à jour....");
        userTkService.update(user, null, null);
    };
}]);