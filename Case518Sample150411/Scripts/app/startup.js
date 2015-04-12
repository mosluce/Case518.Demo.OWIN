(function() {
    var app = angular.module("app", ["ui.router", "oc.lazyLoad", "validation", "validation.rule", "cgBusy"]);

    app.config(function($stateProvider, $urlRouterProvider) {
        $urlRouterProvider.otherwise("home");

        $stateProvider
            .state("home", {
                url: "/home",
                template: "Hello!! ASP.NET OWIN Identity"
            })
            .state("auth", {
                url: "/auth",
                template: "<ui-view />",
                abstract: true
            })
            .state("auth.login", {
                url: "/login",
                templateUrl: "/auth/login",
                controller: "AuthLoginController",
                resolve: {
                    ctrl: function($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            files: ["/Scripts/app/ctrl/auth/login.js"]
                        });
                    }
                }
            })
            .state("auth.register", {
                url: "/register",
                templateUrl: "/auth/register",
                controller: "AuthRegisterController",
                resolve: {
                    ctrl: function($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            files: ["/Scripts/app/ctrl/auth/register.js"]
                        });
                    }
                }
            })
            .state("auth.modify", {
                url: "/modify",
                templateUrl: "/auth/modify",
                controller: "AuthModifyController",
                resolve: {
                    ctrl: function($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            files: ["/Scripts/app/ctrl/auth/modify.js"]
                        });
                    }
                }
            })
            .state("auth.logout", {
                controller: function($http, $state, identity) {
                    $http.post("/auth/logout").success(function () {
                        identity.username = "";
                        identity.isAuthenticated = false;
                        $state.go("home");
                    }).error(function(data) {
                        alert(data);
                    });
                }
            });
    });
    //end of config

    app.factory("identity", function() {
        return {
            isAuthenticated: false,
            username: ""
        };
    });

    app.controller("MenuController", function ($scope, $http, $state, $rootScope, identity) {
        $scope.identity = identity;

        $scope.init = function(isAuthenticated, username) {
            identity.isAuthenticated = isAuthenticated;
            identity.username = username;
        }
    });
    //end of menu controller
})();