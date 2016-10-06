app.controller('extraRouteCtrl', function($scope) {
    $scope.paintRoute = function(route) {
        var routeLtLg = [{
                lt: route.RouteParts[0].OriginCityInfo.Latitude,
                lg: route.RouteParts[0].OriginCityInfo.Longitude,
            }];

            $.each(route.RouteParts, function(index, value) {
                routeLtLg.push({
                    lt: value.DestinationCityInfo.Latitude,
                    lg: value.DestinationCityInfo.Longitude
                });
            });

        $scope.drowRoute(routeLtLg, false, true);
    };
});