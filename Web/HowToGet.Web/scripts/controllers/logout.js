angular.module('app').controller('LogoutCtrl', function($scope, Auth, Dictionary, Session) {
  'use strict';

  $scope.logout = function() {
    var startRequest = function() {
        $scope.busy = true;
      },
      finishRequest = function() {
        $scope.busy = false;
      },
      failLogout = function(response) {
        $scope.errorMessage = Dictionary.getValue(response.data);
      },
      successLogout = function() {
        $scope.errorMessage = false;
        Session.remove();
      };

    startRequest();
    Auth.logout()
      .then(successLogout, failLogout)
      .then(finishRequest);
  };
}).$inject = ['$scope', 'Auth', 'Dictionary', 'Session'];