'use strict';
takeDoc.controller('menuController', ['$scope', '$rootScope', '$location', function ($scope, $rootScope, $location) {
    $scope.items = [
        { title: 'Nouveau', id: 1, scenario:"addDocument", group: "DOCUMENTS" },
        { title: 'Recherche', id: 2, scenario:"scenarioTestForm", group: "DOCUMENTS" },
        { title: 'Derniers', id: 3, group: "DOCUMENTS" },
        { title: 'Profil', id: 5, scenario: "#/profil", group: "COMPTE" },
        { title: 'About', id: 6, scenario: "#/about", group: "AUTRE" }
    ];

    $scope.start = function () {
        var step = $rootScope.Scenario.start(arguments[0]);
        var path = (step != null) ? step.to : arguments[0];
        if (path.substring(0, 2) == "#/") path = path.substring(2);
        if (path.indexOf("@") > -1) path = path.substr(0, path.indexOf("@"));
        $location.path(path);
    };
}]);
