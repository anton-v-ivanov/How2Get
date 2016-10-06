var editRouteCtrl =  app.controller('EditRouteCtrl', function($scope, $route, $location, Client, Route) {
    var route = $route.current.locals.route;

    $scope.client = Client;
    $scope.error = {};
    $scope.route = route;
    $scope.isMe = route.UserId == Client.userId ? true : false;

    $scope.saveRoute = function() {

        Route.update($scope.route, function(data) {

            $scope.error.visible = false;
            $location.path('/route/' + data.Id);

        }, function(data) {

            $scope.error.text = data || 'We\'re sorry, a server error occurred. Please wait a bit and try again.';
            $scope.error.visible = true;

        });

    };
});

routeCtrl.loadRoute = function($q, $route, Route) {

    var defer = $q.defer();

    Route.get({

        id: $route.current.params.id

    }, function(data) {

        defer.resolve(data);

    }, function(data) {

        defer.reject(data);

    });

    return defer.promise;

};