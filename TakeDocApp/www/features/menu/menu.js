'use strict';
takeDoc.controller('menuController', ['$scope', '$rootScope', '$location', function ($scope, $rootScope, $location) {

    $scope.items = [
        { title: 'Nouveau', id: 1, scenario: "addDocument", url: null, cssClassName: "ion-plus-circled" },
        { title: '', id: 2, scenario: null, url: null, cssClassName: "menu-empty" },
        { title: 'Incomplet', id: 3, scenario: "findIncomplet", url: null, cssClassName: "ion-alert-circled", count: "INCOMPLETE" },
        { title: 'Complet', id: 4, scenario: "findComplet", url: null, cssClassName: "ion-android-search", count: "COMPLETE" },
        { title: 'Attente', id: 5, scenario: "findWait", url: null, cssClassName: "ion-android-search", count: "TO_VALIDATE" },
        { title: 'Validé', id: 6, scenario: "findApprove", url: null, cssClassName: "ion-paperclip", count: "APPROVE" },
        { title: 'Refusé', id: 7, scenario: "findRefuse", url: null, cssClassName: "ion-heart-broken", count: "REFUSE" },
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

            var count = $rootScope.Dashboards.countStatus("INCOMPLETE") + $rootScope.Dashboards.countStatus("CREATE");
            angular.element("#span-INCOMPLETE").html("(" + count + ")");

            count = $rootScope.Dashboards.countStatus("COMPLETE");
            angular.element("#span-COMPLETE").html("(" + count + ")");

            count = $rootScope.Dashboards.countStatus("TO_VALIDATE");
            angular.element("#span-TO_VALIDATE").html("(" + count + ")");

            count = $rootScope.Dashboards.countStatus("APPROVE");
            angular.element("#span-APPROVE").html("(" + count + ")");

            count = $rootScope.Dashboards.countStatus("REFUSE");
            angular.element("#span-REFUSE").html("(" + count + ")");
          };
        var error = function () {

        };
        $rootScope.Dashboards.load($rootScope.User.Id, success, error);
    });
}]);
