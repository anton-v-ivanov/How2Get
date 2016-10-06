app.controller('LoginFormCtrl', function($scope, Client) {
    $scope.user = {};
    $scope.error = {};

    $scope.login = function() {
        Client.login({

            email: $scope.user.email,
            password: $scope.user.password

        }, function(data) {

            $scope.error.visible = false;

            $scope.user.email = null;
            $scope.user.password = null;

            Client.authToken = data.AuthToken;

        }, function(data) {

            if(data == "IncorrectLoginOrPassword") {

                $scope.error.text = 'Incorrect login or password';
            }
            else {
                $scope.error.text = data || 'We\'re sorry, a server error occurred. Please wait a bit and try again.';
            }
            $scope.error.visible = true;

        });
    };
});