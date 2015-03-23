'use strict';
var backOffice = angular.module("backOffice", ['ngRoute']);

backOffice.run(function ($rootScope,$location) {
    

});

backOffice.config(function ($stateProvider, $urlRouterProvider) {
    $urlRouterProvider.otherwise('/login');
});





