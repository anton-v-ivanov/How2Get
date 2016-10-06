angular.module('app').factory('Route', function($resource) {
  'use strict';

  return $resource('/api/route/:id', {
    id: '@Id'
  });
}).$inject = ['$resource'];