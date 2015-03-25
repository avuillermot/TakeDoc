'use strict';
takeDoc.controller('menuController', ['$scope', '$rootScope', '$location', function ($scope, $rootScope, $location) {

    $scope.items = [
        { title: 'Nouveau', id: 1, scenario: "addDocument", url: null, cssClassName: "ion-plus-circled" },
        { title: '', id: 2, scenario: null, url: null, cssClassName: "menu-empty" },
        { title: 'Incomplet', id: 3, scenario: "findIncomplet", url: null, cssClassName: "ion-alert-circled" },
        { title: 'En cours', id: 4, scenario: "findDocument", url: null, cssClassName: "ion-android-search" },
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
}]);
