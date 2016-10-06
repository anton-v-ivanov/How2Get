var searchCtrl = app.controller('SearchCtrl', function($scope, $location, $route) {
    var data = $route.current.locals;

    $scope.$location = $location;

    $scope.query = {
        origin: {
            name: data.originInfo.Name,
            id: data.originInfo.Id
        },

        destination: {
            name: data.destinationInfo.Name,
            id: data.destinationInfo.Id
        }
    };

    $scope.routes = data.searchResult;
    $scope.routesFound = $scope.routes.length ? true : false;
});

searchCtrl.loadSearchResult = function($q, $route, Route) {
    var defer = $q.defer();

    Route.search({

        originId: $route.current.params.originId,
        destinationId: $route.current.params.destinationId

    }, function(data) {

        defer.resolve(data);

    }, function(data) {

        defer.reject(data);

    });

    return defer.promise;
};

searchCtrl.loadOriginInfo = function($q, $route, City) {
    var defer = $q.defer();

    City.get({

        id: $route.current.params.originId

    }, function(data) {

        defer.resolve(data);

    }, function(data) {

        defer.reject(data);

    });

    return defer.promise;
};

searchCtrl.loadDestinationInfo = function($q, $route, City) {
    var defer = $q.defer();

    City.get({

        id: $route.current.params.destinationId

    }, function(data) {

        defer.resolve(data);

    }, function(data) {

        defer.reject(data);

    });

    return defer.promise;
};