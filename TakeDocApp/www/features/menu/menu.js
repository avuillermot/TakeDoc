'use strict';
takeDoc.controller('menuController', ['$scope', '$rootScope', '$location', function ($scope, $rootScope, $location) {

    $scope.items = [
        { title: 'Nouveau', id: 1, scenario: "addDocument", url: null, cssClassName: "ion-plus-circled" },
        { title: '', id: 2, scenario: null, url: null, cssClassName: "menu-empty" },
        { title: 'Incomplet', id: 3, scenario: "findIncomplet", url: null, cssClassName: "ion-alert-circled", count: "INCOMPLETE" },
        { title: 'En cours', id: 4, scenario: "findDocument", url: null, cssClassName: "ion-android-search", count: "COMPLETE" },
        { title: 'Validé', id: 5, scenario: null, url: null, cssClassName: "ion-paperclip" },
        { title: 'Refusé', id: 6, scenario: null, url: null, cssClassName: "ion-heart-broken" },
        { title: 'Derniers', id: 7, scenario: "findLast", url: null, cssClassName: "ion-clock" },
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

    var initCounter = function (dashboards, status1, status2) {
        var nb = 0;
        $.each(dashboards.models, function (index, value) {
            if (status1 != null && value.get("Code") == status1) nb = nb + value.get("Value");
            if (status2 != null && value.get("Code") == status2) nb = nb + value.get("Value");
        });
        angular.element("#span-"+status1).html("(" + nb + ")");
    };

    $scope.$on("$ionicView.afterEnter", function (scopes, states) {
        
        var dashboards = new Dashboards();
        var success = function () {
            var data = arguments[0];
            initCounter(data, "INCOMPLETE");
            initCounter(data, "COMPLETE", "SEND");
          };
        var error = function () {

        };
        dashboards.load($rootScope.User.Id, success, error);
    });
}]);
