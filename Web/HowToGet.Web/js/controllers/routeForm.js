app.controller('RouteFormCtrl', function($scope) {
    $scope.addPart = function() {
        var lastPart = $scope.route.RouteParts[$scope.route.RouteParts.length - 1],
            previousRoute = {};
        
        if (lastPart.DestinationCityId && lastPart.DestinationCityInfo.Name && lastPart.DestinationCityInfo.Name) {
            previousRoute = {
                OriginCityId: lastPart.DestinationCityId || null,
                OriginCityInfo: {
                    Name: lastPart.DestinationCityInfo.Name || null,
                    OriginCityInfo: {
                        CountryId: lastPart.DestinationCityId.CountryId || null,
                        Lt: lastPart.DestinationCityInfo.Lt || null,
                        Lg: lastPart.DestinationCityInfo.Lg || null
                    }
                }
            };
        };

        $scope.route.RouteParts.push(previousRoute);
    };

    $scope.deletePart = function(index) {
        $scope.route.RouteParts.splice(index, 1);
    };
});