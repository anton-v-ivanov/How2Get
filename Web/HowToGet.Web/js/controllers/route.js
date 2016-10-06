var routeCtrl = app.controller('RouteCtrl', function($scope, $route) {
    var route = $route.current.locals.route,
        routeLtLg = null;

    $scope.query = {
        origin: {
            name: route.RouteParts[0].OriginCityInfo.Name,
            country: route.RouteParts[0].OriginCityInfo.Country
        },
        destination: {
            name: route.RouteParts[route.RouteParts.length - 1].DestinationCityInfo.Name,
            country: route.RouteParts[route.RouteParts.length - 1].DestinationCityInfo.Country
        }
    };

    $scope.route = route;
    $scope.author = route.UserName ? true : false;


    routeLtLg = [{
        lt: route.RouteParts[0].OriginCityInfo.Latitude,
        lg: route.RouteParts[0].OriginCityInfo.Longitude,
    }];

    $.each(route.RouteParts, function(index, value) {
        routeLtLg.push({
            lt: value.DestinationCityInfo.Latitude,
            lg: value.DestinationCityInfo.Longitude
        });
    });

    $scope.$watch('drowRoute', function() {
        $scope.drowRoute(routeLtLg);
    });
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