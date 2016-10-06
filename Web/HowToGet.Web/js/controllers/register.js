app.controller('RegisterFormCtrl', function($scope, $location, Client) {
    $scope.user = {};
    $scope.error = {};

    $scope.register = function() {
        Client.register({

            name: $scope.user.name,
            email: $scope.user.email,
            password: $scope.user.password,
            inviteCode: $scope.user.inviteCode

        }, function(data) {

            $scope.error.visible = false;

            $scope.user.name = null;
            $scope.user.email = null;
            $scope.user.password = null;

            Client.authToken = data.AuthToken;
            $location.path("/");

        }, function(data) {
            switch(data){
                case 'DuplicateEmail':
                    $scope.error.text = 'This email is already registered. You can try to restore your password';
                break;
                case 'InvalidEmail':
                    $scope.error.text = 'This email has invalid format. Please try another email';
                break;
                case 'UnknownInviteCode':
                    $scope.error.text = 'Invite code is not valid :(';
                break;
                default:
                    $scope.error.text = data  || 'We\'re sorry, a server error occurred. Please wait a bit and try again.';
                break;
            }
            $scope.error.visible = true;
        });
    };
});