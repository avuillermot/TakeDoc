'use strict';
var backOffice = angular.module("backOffice", ['ui.router']);

backOffice.run(function ($rootScope,$location) {
    

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
            "viewDetail": {
                templateUrl: "features/login/login.html",
                controller: 'loginController'
            }
        }
    });
    // if none of the above states are matched, use this as the fallback
    $urlRouterProvider.otherwise('/login');
});





