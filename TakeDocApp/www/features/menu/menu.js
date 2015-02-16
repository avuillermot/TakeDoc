'use strict';
takeDoc.controller('menuController', ['$scope', '$rootScope', '$ionicNavBarDelegate', function ($scope, $rootScope, $ionicNavBarDelegate) {
    var start1 = $rootScope.Scenario.start("addDocument").to;

    $scope.items = [
        { title: 'Nouveau', id: 1, url: start1, group: "DOCUMENTS" },
        { title: 'Recherche', id: 2, group: "DOCUMENTS" },
        { title: 'Derniers', id: 3, group: "DOCUMENTS" },
        { title: 'Profil', id: 5, url: "#/profil", group: "COMPTE" },
        { title: 'About', id: 6, url: "#/about", group: "AUTRE" }
    ];
}]);
