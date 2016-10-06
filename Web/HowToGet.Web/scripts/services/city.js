angular.module('app').factory('City', function($resource) {
  'use strict';

  return $resource('/api/city');
}).$inject = ['$resource'];