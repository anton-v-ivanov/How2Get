angular.module('app').controller('SearchRouteCtrl', function($scope, $routeParams, City, Route) {
  'use strict';

  $scope.query = {
    origin: City.get({
      id: $routeParams.origin
    }),
    destination: City.get({
      id: $routeParams.destination
    })
  };
  $scope.routes = Route.query({
    destination: $routeParams.destination,
    origin: $routeParams.origin
  });
}).$inject = ['$scope', '$routeParams', 'City', 'Route'];