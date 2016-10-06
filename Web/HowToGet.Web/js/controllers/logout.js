app.controller('LogoutCtrl', function($scope, Client, $location) {
    $scope.error = {};

    $scope.logout = function() {

        Client.logout(function() {
            $scope.error.visible = false;
            
            Client.authToken = null;

        }, function(data) {

            $scope.error.visible = true;
            $scope.error.text = data || 'We\'re sorry, a server error occurred. Please wait a bit and try again.';

        });

    };

});