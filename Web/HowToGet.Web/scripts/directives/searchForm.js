angular.module('app').directive('searchRouteForm', function($location) {
  'use strict';

  return {
    replace: true,
    templateUrl: 'partials/components/searchForm.html',
    link: function(scope) {
      scope.search = function(query) {
        if (!query.origin.Id || !query.destination.Id) return;
        $location.path('/route/search/' + query.origin.Id + '/' + query.destination.Id);
      };
    }
  };
}).$inject = ['$location'];