angular.module('app').controller('LoginCtrl', function($scope, $location, Session, Auth, Dictionary) {
  'use strict';

  $scope.login = function(email, password) {
    var data = {
        password: password,
        email: email
      },
      startRequest = function() {
        $scope.busy = true;
      },
      finishRequest = function() {
        $scope.busy = false;
      },
      loginFail = function(response) {
        $scope.errorMessage = Dictionary.getValue(response.data);
      },
      loginSuccess = function(response) {
        var token = response.data.AuthToken;

        if (!token) return;
        Session.save(token);
        $location.path('/');
      };

    startRequest();
    Auth.login(data)
      .then(loginSuccess, loginFail)
        .then(finishRequest);
  };
}).$inject = ['$scope', '$location', 'Session', 'Auth', 'Dictionary'];