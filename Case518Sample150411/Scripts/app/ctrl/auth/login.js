(function() {
    var app = angular.module("app");

    app.controller("AuthLoginController", function($scope, $rootScope, $state, $http) {
        $scope.login = function () {

        };

        $scope.register = function () {
            $state.go("auth.register");
        };

        $scope.forgot = function () {

        };
    });
})();