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
                    login: function($ocLazyLoad) {
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
                    login: function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            files: ["/Scripts/app/ctrl/auth/register.js"]
                        });
                    }
                }
            });
    });
})();