'use strict';
var backOffice = angular.module("backOffice", ['ui.router']);

backOffice.run(function ($rootScope,$location) {
    

});

backOffice.config(function ($stateProvider, $urlRouterProvider) {
    var _routeHelper = new routeHelper();
    $stateProvider.state('index', {
        url: "",
        views: {
            "viewMenu": {
                templateUrl: "features/login/login.html",
                controller: 'loginController'
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
    })
    // if none of the above states are matched, use this as the fallback
    $urlRouterProvider.otherwise('/login');
});





