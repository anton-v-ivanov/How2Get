var topUsersCtrl = app.controller('TopUsersCtrl', function($scope, $location, $route) {
    
    if($route.current.locals) {
        var users = $route.current.locals.topUsersResults;
        
        $scope.users = users;
    }
});

topUsersCtrl.loadTopUsers = function($q, User, Client) {
	
	if(!Client.authorized())
		return;

    var defer = $q.defer();

    User.getTop(function(data) {

        defer.resolve(data);

    }, function(data) {

        defer.reject(data);

    });

    return defer.promise;
};