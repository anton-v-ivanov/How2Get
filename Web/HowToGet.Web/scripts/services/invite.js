angular.module('app').factory('Invite', function($resource) {
  'use strict';

  return $resource('/api/invite/');
}).$inject = ['$resource'];