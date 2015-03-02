'use strict';
takeDoc.controller('menuController', ['$scope', '$rootScope', '$location', function ($scope, $rootScope, $location) {

    $scope.items = [
        { title: 'Nouveau', id: 1, scenario: "addDocument", url: null, group: "DOCUMENTS", cssClassName: "menuItem-Nouveau" },
        { title: 'Recherche', id: 2, scenario: "scenarioTestForm", url: null, group: "DOCUMENTS", cssClassName: "menuItem-Recherche" },
        { title: 'Derniers', id: 3, scenario: null, url: null, group: "DOCUMENTS", cssClassName: "menuItem-Derniers" },
        { title: 'Profil', id: 5, scenario: null, url: "#/profil", group: "COMPTE", cssClassName: "menuItem-Profil" },
        { title: 'Informations', id: 6, scenario: null, url: "#/about", group: "AUTRE", cssClassName: "menuItem-informations" }
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
