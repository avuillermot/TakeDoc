'use strict';
takeDoc.controller('menuController', ['$scope', '$rootScope', '$location', function ($scope, $rootScope, $location) {

    $scope.items = [
        { title: 'Nouveau', id: 1, scenario: "addDocument", url: null, group: "DOCUMENTS" },
        { title: 'Recherche', id: 2, scenario: "scenarioTestForm", url: null, group: "DOCUMENTS" },
        { title: 'Derniers', id: 3, scenario: null, url: null, group: "DOCUMENTS" },
        { title: 'Profil', id: 5, scenario: null, url: "#/profil", group: "COMPTE" },
        { title: 'About', id: 6, scenario: null, url: "#/about", group: "AUTRE" }
    ];

    $scope.show = function (scenario, url) {
        debugger;
        if (url != null && url != "") {
            $location.path(url.substr(2));
        }
        else {
            var step = $rootScope.Scenario.start(scenario);
            $location.path(step.to.substr(2));
        }
    };
}]);
