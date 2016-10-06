angular.module('app').service('Session', function($http, Client) {
  'use strict';

  var setHeader = function(token) {
    if (!token) return;
    $http.defaults.headers.common.Authorization = token;
  };

  var removeHeader = function() {
    delete $http.defaults.headers.common.Authorization;
  };

  this.save = function(token) {
    if (!token) return;

    localStorage.Authorization = token;
    this.token = token;
    setHeader(token);

    Client.authToken = token; // TODO: remove it and refactor Client service
  };

  this.remove = function() {
    localStorage.removeItem('Authorization');
    this.token = null;
    removeHeader();

    Client.authToken = null; // TODO: remove it and refactor Client service
  };

  this.restore = function() {
    var token = localStorage.Authorization;

    if (!token) return;
    this.token = token;
    setHeader(token);
  };
}).$inject = ['$http', 'Client'];