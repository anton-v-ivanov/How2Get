app.controller('AddRouteCtrl', function($scope, $location, Client, Route) {

    $scope.client = Client;
    $scope.error = {};
    $scope.route = {
        RouteParts: [{}]
    };

    $scope.saveRoute = function() {
        Route.create($scope.route, function(data) {

            $scope.error.visible = false;
            var id = data.RouteId.replace(/"/g,'')
            $location.path('/route/' + id);

        }, function(data) {

            $scope.error.text = data || 'We\'re sorry, a server error occurred. Please wait a bit and try again.';
            $scope.error.visible = true;

        });
    };

});