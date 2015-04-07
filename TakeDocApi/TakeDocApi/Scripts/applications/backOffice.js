'use strict';
var backOffice = angular.module("backOffice", ['ui.router']);

backOffice.run(function ($rootScope, $location) {

   $rootScope.User = null;

   $rootScope.showLoader = function () {
        $("#span-loader-message").html(arguments[0]);
        $(".btn-loader-container").css("display", "block");
    };
    $rootScope.hideLoader = function () {
        $(".btn-loader-container").css("display", "none");
    };

    $rootScope.$on("$viewContentLoaded", function (scopes) {
        $rootScope.hideLoader();
    });
});

backOffice.config(function ($stateProvider, $urlRouterProvider) {
    $stateProvider.state('test', {
        url: "/test",
        views: {
            "viewMenu": {
                templateUrl: "features/menu/menu.html",
                controller: 'menuController'
            },
            "viewGrid": {
                templateUrl: "features/login/login.html",
                controller: 'loginController'
            },
            "viewDetail": {
                templateUrl: "features/login/login.html",
                controller: 'loginController'
            }
        }
    });
    $stateProvider.state('home', {
        url: "/home",
        views: {
            "viewMenu": {
                templateUrl: "features/menu/menu.html",
                controller: 'menuController'
            }
        }
    });
    $stateProvider.state('login', {
        url: "/login",
        views: {
            "viewGrid": {
                templateUrl: "features/welcome/welcome.html"
            },
            "viewDetail": {
                templateUrl: "features/login/login.html",
                controller: 'loginController'
            }
        }
    });
    // if none of the above states are matched, use this as the fallback
    $urlRouterProvider.otherwise('/login');
});

backOffice.directive('tdLogout', function ($rootScope, $location) {
    return function (scope, element, attrs) {
        element.bind('click', function () {
            scope.$apply(function () {
                $rootScope.User = null;
                $location.path("login");
            });
        });
    };
});





