var tokenAuthCtrl = app.controller('TokenAuthCtrl', function(Client, Token, $scope, $location, $route, $timeout) {
	
	$scope.error = {};

	var url = "/" + $route.current.params.url;
	if(Client.authorized()) {
        $location.path(url);
        return;
    }

	Token.exchange({
    	token: $route.current.params.token

    }, function(data) {
		$scope.error.visible = false;
        Client.saveSession(data.AuthToken);
        Client.authToken = data.AuthToken;
        $location.path(url);
        return;

    }, function(data) {

        $scope.error.text = 'Authorization token is invalid.';
        $scope.error.visible = true;

    });
	

});
