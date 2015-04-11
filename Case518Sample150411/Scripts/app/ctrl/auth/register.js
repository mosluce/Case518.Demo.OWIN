(function() {
    var app = angular.module("app");

    app.controller("AuthRegisterController", function ($scope, $state, $http, $validation) {

        $scope.getvc = function() {
            $scope.vcimg = "/auth/validationCode?" + (new Date()).getTime();
        };

        $scope.submit = function () {
            $validation.validate($scope.Form).success(function() {
                $http.post("/auth/register", $scope.user).success(function(result) {
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

        $scope.getvc();
    });
})();