var topRoutesCtrl = app.controller('TopRoutesCtrl', function($scope, $location, $route) {
    
    if($route.current.locals) {
        var routes = $route.current.locals.topRoutesResults;
    
        $scope.routes = routes;
    }
});

topRoutesCtrl.loadTopRoutes = function($q, Route, Client) {

	if(!Client.authorized())
		return;

    var defer = $q.defer();

    Route.getTop(function(data) {

        defer.resolve(data);

    }, function(data) {

        defer.reject(data);

    });

    return defer.promise;
};