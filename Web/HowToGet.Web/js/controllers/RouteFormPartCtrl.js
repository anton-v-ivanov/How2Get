app.controller('RouteFormPartCtrl', function($scope, Route) {
    $scope.carrierTypeValid = false;

    $scope.$watch('routePart.OriginCityId + routePart.DestinationCityId + routePart.CarrierType', function(newValue) {
        if ($scope.routePart.OriginCityId && $scope.routePart.DestinationCityId && $scope.routePart.CarrierType) {
            Route.getRouteTime({
                originId: $scope.routePart.OriginCityId,
                destinationId: $scope.routePart.DestinationCityId,
                carrierType: $scope.routePart.CarrierType
            }, function(data) {
                    $scope.routePart.Time = data.Time;
            }, function() {
                /*Log error*/
            });
        };
    });

    $scope.$watch('routePart.CarrierType', function(newValue, oldValue) {
        $scope.carrierTypeValid = newValue % 1 === 0 ? true : false;

        if (newValue !== oldValue) {
            $scope.routePart.CarrierId = null;
        };
    });
});