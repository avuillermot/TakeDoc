'use strict';
takeDoc.controller('menuController', ['$scope', '$rootScope', '$location', function ($scope, $rootScope, $location) {

    $scope.items = [
        { title: 'Nouveau', id: 1, scenario: "addDocument", url: null, cssClassName: "ion-plus-circled" },
        { title: '', id: 2, scenario: null, url: null, cssClassName: "menu-empty" },
        { title: 'Incomplet', id: 3, scenario: "findIncomplet", url: null, cssClassName: "ion-alert-circled", count: "INCOMPLETE" },
        { title: 'En attente', id: 4, scenario: "findDocument", url: null, cssClassName: "ion-android-search", count: "COMPLETE" },
        { title: 'Transmis', id: 5, scenario: "findSend", url: null, cssClassName: "ion-android-search", count: "SEND" },
        { title: 'Validé', id: 6, scenario: null, url: null, cssClassName: "ion-paperclip" },
        { title: 'Refusé', id: 7, scenario: null, url: null, cssClassName: "ion-heart-broken" },
        { title: '', id: 8, scenario: null, url: null, cssClassName: "menu-empty" },
        { title: 'Profil', id: 9, scenario: null, url: "#/profil", cssClassName: "ion-person" },
        { title: 'Informations', id: 10, scenario: null, url: "#/about", cssClassName: "ion-information-circled" }
    ];

    $scope.show = function (scenario, url) {
        if (url != null && url != "") {
            $location.path(url.substr(2));
        }
        else if (scenario != null && scenario != "") {
            var step = $rootScope.Scenario.start(scenario);
            $location.path(step.to.substr(2));
        }
    };

    $scope.$on("$ionicView.afterEnter", function (scopes, states) {
        
        var success = function () {
            $rootScope.Dashboards = arguments[0];

            var count = $rootScope.Dashboards.countStatus("INCOMPLETE");
            angular.element("#span-INCOMPLETE").html("(" + count + ")");

            count = $rootScope.Dashboards.countStatus("COMPLETE");
            angular.element("#span-COMPLETE").html("(" + count + ")");

            count = $rootScope.Dashboards.countStatus("SEND");
            angular.element("#span-SEND").html("(" + count + ")");

          };
        var error = function () {

        };
        $rootScope.Dashboards.load($rootScope.User.Id, success, error);
    });
}]);
