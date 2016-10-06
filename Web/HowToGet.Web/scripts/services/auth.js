angular.module('app').factory('Auth', function($http) {
  return {
    login: function(data) {
      return $http({
        url: '/api/auth',
        method: 'POST',
        data: data
      });
    },
    registration: function(data) {
      return $http({
        url: '/api/user/register',
        method: 'POST',
        data: data
      });
    },
    restore: function(data) {
      return $http({
        url: '/api/password',
        method: 'POST',
        data: data
      });
    },
    logout: function() {
      return $http({
        url: '/api/auth',
        method: 'DELETE'
      });
    }
  }
}).$inject = ['$http'];