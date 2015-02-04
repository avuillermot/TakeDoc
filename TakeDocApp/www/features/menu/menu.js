'use strict';
takeDoc.controller('menuController', ['$scope', '$rootScope', 'menuService', '$ionicNavBarDelegate', function ($scope, $rootScope, menuService, $ionicNavBarDelegate) {

    $scope.$on("$ionicView.beforeEnter", function (scopes, states) {
        $rootScope.$viewHistory = {
            histories: { root: { historyId: 'root', parentHistoryId: null, stack: [], cursor: -1 } },
            backView: null,
            forwardView: null,
            currentView: null,
            disabledRegistrableTagNames: []
        };
        $rootScope.hideBackButton = false;
    });
    
    $scope.items = [
        { title: 'Ajouter un document', id: 1, url: $rootScope.Scenario.start("addDocument").to, group: "DOCUMENTS" },
        { title: 'Search', id: 2, group: "DOCUMENTS" },
        /*{ title: 'avt', id: 3, group: "DOCUMENTS" },
        { title: 'Indie', id: 4, group: "DOCUMENTS" },
        { title: 'Rap', id: 5, group: "COMPTE" },*/
        { title: 'About', id: 6, url: "#/about", group: "AUTRE" }
    ];

    $scope.user = { FullName: $rootScope.User.getFullName() };
}]);



takeDoc.service('menuService', ['$http', function ($http) {

}]);

