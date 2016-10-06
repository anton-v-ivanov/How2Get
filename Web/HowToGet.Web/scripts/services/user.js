angular.module('app').factory('User', function($resource) {
  'use strict';

  return $resource('/api/user/:userId', {
    userId: '@Id'
  });
}).$inject = ['$resource'];