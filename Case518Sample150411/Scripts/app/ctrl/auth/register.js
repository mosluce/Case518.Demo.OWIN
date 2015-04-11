(function() {
    var app = angular.module("app");

    app.controller("AuthRegisterController", function ($scope, $state, $http, $validation) {
        //產生驗證碼
        $scope.getvc = function() {
            $scope.vcimg = "/auth/validationCode?" + (new Date()).getTime();
        };

        //送出表單
        $scope.submit = function () {
            //驗證
            $validation.validate($scope.Form).success(function () {
                //送出請求
                $scope.onprocess = $http.post("/auth/register", $scope.user).success(function (result) {
                    if (result.Success) {
                        alert("註冊成功，開通信件已發送至您填寫的電子郵件中。");
                        $state.go("home");
                    } else {
                        alert(result.Message);
                    }
                }).error(function(data, status) {
                    alert(data);
                });
            });
        };

        //取得驗證碼
        $scope.getvc();
    });
})();