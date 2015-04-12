(function() {
    var app = angular.module("app");

    app.controller("AuthLoginController", function ($scope, $rootScope, $state, $http, $validation, identity) {
        console.log(identity);
        /**
         * 產生驗證碼
         */
        $scope.getvc = function() {
            $scope.vcimg = "/auth/validationCode?" + (new Date()).getTime();
        }

        /**
         * 執行登入
         */
        $scope.login = function () {
            $validation.validate($scope.Form).success(function () {
                //驗證表單
                $scope.onprocess = $http.post("/auth/login", $scope.user).success(function (result) {
                    if (result.Success) {
                        //切換選單狀態
                        identity.isAuthenticated = true;
                        identity.username = result.Data.Name;
                        
                        //回到首頁
                        $state.go("home");
                    } else {
                        alert(result.Message);
                    }
                }).error(function (data, status) {
                    alert(data);
                });
            });
        };

        /**
         * 前往註冊畫面
         */
        $scope.register = function () {
            $state.go("auth.register");
        };

        /**
         * 發送忘記密碼Request
         */
        $scope.forgot = function () {

        };

        //抓取驗證碼
        $scope.getvc();
    });
})();