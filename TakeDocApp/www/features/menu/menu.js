'use strict';
takeDoc.controller('menuController', ['$scope', '$rootScope', '$location', function ($scope, $rootScope, $location) {

    $scope.items = [
        { title: 'Nouveau', id: 1, scenario: "addDocument", url: null, cssClassName: "ion-plus-circled" },
        { title: 'Incomplet', id: 2, scenario: null, url: null, cssClassName: "ion-heart-broken" },
        { title: 'Recherche', id: 3, scenario: "scenarioTestForm", url: null, cssClassName: "ion-android-search" },
        { title: 'Derniers', id: 4, scenario: null, url: null, cssClassName: "ion-clock" },
        { title: 'Profil', id: 5, scenario: null, url: "#/profil", cssClassName: "ion-person" },
        { title: 'Informations', id: 6, scenario: null, url: "#/about", cssClassName: "ion-information-circled" }
    ];

    $scope.show = function (scenario, url) {
        if (url != null && url != "") {
            $location.path(url.substr(2));
        }
        else {
            var step = $rootScope.Scenario.start(scenario);
            $location.path(step.to.substr(2));
        }
    };
}]);
