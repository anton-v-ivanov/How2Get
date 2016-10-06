var profileCtrl = app.controller('ProfileCtrl', function($scope, $route, Client, Route) {
    if (Client.authorized()) {
        var data = $route.current.locals,
            routes = data.clientRoutes,
            invites = data.invites;

        $scope.client = Client;
        $scope.isMe = true;

        $scope.routes = routes;
        $scope.routesCount = routes.length;
        $scope.routesFound = routes.length ? true : false;
        
        $scope.invites = invites;

        $scope.remove = function(index, routeId) {
            Route.delete({
                id: routeId
            }, function() {
                $scope.routes.splice(index, 1);
            }, function() {
                //ERROR!      
            });
        };

        var routesLtLg = [];

        $.each(routes, function(index, route) {
            routesLtLg.push({
                lt: route.RouteParts[0].OriginCityInfo.Latitude,
                lg: route.RouteParts[0].OriginCityInfo.Longitude,
            });

            $.each(route.RouteParts, function(index, value) {
                routesLtLg.push({
                    lt: value.DestinationCityInfo.Latitude,
                    lg: value.DestinationCityInfo.Longitude
                });
            });
        });

        $scope.$watch('drowRoute', function() {
            $scope.drowRoute(routesLtLg, true);
        });
    };
});

profileCtrl.loadClientRoutes = function($q, Client) {
    if (Client.authorized()) {
        var defer = $q.defer();

        Client.getRoutes(Client.authToken, function(data) {

            defer.resolve(data);

        }, function(data) {

            defer.reject(data);

        });

        return defer.promise;
    };
};

profileCtrl.loadInvites = function($q, Client) {
    if (Client.authorized()) {
        var defer = $q.defer();

        Client.getInvites(function(data) {

            defer.resolve(data);

        }, function(data) {

            defer.reject(data);

        });

        return defer.promise;
    };
};