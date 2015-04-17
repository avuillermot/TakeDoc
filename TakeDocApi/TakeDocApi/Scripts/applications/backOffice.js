'use strict';
var backOffice = angular.module("backOffice", ['ui.router', 'ui.grid', 'ui.grid.edit','ui.grid.resizeColumns']);

backOffice.run(function ($rootScope, $location) {

    $rootScope.getUser = function () {
        return JSON.parse(localStorage.getItem("TkUser"));
    };
    $rootScope.setUser = function () {
        localStorage.setItem("TkUser", JSON.stringify(arguments[0]));
    };

    $rootScope.showLoader = function () {
        var msg = "Traitement en cours....";
        if (arguments[0] != null) msg = arguments[0];;
        $("#span-loader-message").html(msg);
        $(".btn-loader-container").css("display", "block");
    };
    $rootScope.hideLoader = function () {
        $(".btn-loader-container").css("display", "none");
    };

    $rootScope.showModal = function (title, body) {
        $("#myModalLabel").html(title);
        $("#myModalBody").html(body);
        $('#myModal').modal('show');
    };
    $rootScope.hideModal = function () {
    };

    $rootScope.showError = function (err) {
        if (err.responseJSON == null || err.responseJSON.Message == null) $rootScope.showModal("Erreur", "Une erreur est survenue.")
        else $rootScope.showModal("Erreur", err.responseJSON.Message);
    }

    $rootScope.$on("$viewContentLoaded", function (scopes) {
        $rootScope.hideLoader();

        if ($location.$$path != "/login") {
            if ($rootScope.getUser() == null) $location.path("#/login");
        }

        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        })
    });
});

backOffice.config(function ($stateProvider, $urlRouterProvider) {
   $stateProvider.state('home', {
        url: "/home",
        views: {
            "viewMenu": {
                templateUrl: "features/menu/menu.html",
                controller: 'menuController'
            },
            "viewGrid": {
                templateUrl: "features/myDocument/myDocument.html",
                controller: 'myDocumentController'
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
    $stateProvider.state('account', {
        url: "/account/:user",
        views: {
            "viewMenu": {
                templateUrl: "features/menu/menu.html",
                controller: 'menuController'
            },
            "viewGrid": {
                templateUrl: "features/account/accountInfo.html",
                controller: 'accountController'
            },
            "viewDetail": {
                templateUrl: "features/account/entity.html",
                controller: 'entityController'
            }
        }
    });
    $stateProvider.state('updatePassword', {
        url: "/updatePassword",
        views: {
            "viewMenu": {
                templateUrl: "features/menu/menu.html",
                controller: 'menuController'
            },
            "viewGrid": {
                templateUrl: "features/passwordUpdate/passwordUpdate.html",
                controller: 'passwordController'
            }
        }
    });
    $stateProvider.state('searchUsers', {
        url: "/searchUsers",
        views: {
            "viewMenu": {
                templateUrl: "features/menu/menu.html",
                controller: 'menuController'
            },
            "viewGrid": {
                templateUrl: "features/users/searchUser.html",
                controller: 'searchUserController'
            },
            "viewDetail": {
                templateUrl: "features/users/resultUser.html",
                controller: 'resultUserController'
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
                $rootScope.setUser(null);
                $location.path("login");
            });
        });
    };
});

backOffice.factory('usersResult', function () {
    var data = { users: [], calls: 0 };

    return {
        set: function () {
            data.users = arguments[0];
            data.calls = data.calls + 1;
        },
        data: data
    }
});

backOffice.factory('groups', function () {
    var data = { groups: [], calls: 0 };

    var groups = new GroupTks();
    var success = function () {
        data.groups = arguments[0];
        data.calls = data.calls + 1;
    };
    var error = function()
    {
        alert("Error in factory groups");
    };
    groups.loadAll({success: success, error: error});

    return {
        data: data
    }
});

backOffice.factory('refreshDetail', function () {
    var data = { calls: 0 };

    return {
        data: data
    }
});







