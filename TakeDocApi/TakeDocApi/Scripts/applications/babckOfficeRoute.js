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
    $stateProvider.state('inbox', {
        url: "/inbox",
        views: {
            "viewMenu": {
                templateUrl: "features/menu/menu.html",
                controller: 'menuController'
            },
            "viewGrid": {
                templateUrl: "features/inbox/inbox.html",
                controller: 'inboxController'
            },
            "viewDetail": {
                templateUrl: "features/inbox/display.html",
                controller: 'displayController'
            }
        }
    });

    // if none of the above states are matched, use this as the fallback
    $urlRouterProvider.otherwise('/login');
});