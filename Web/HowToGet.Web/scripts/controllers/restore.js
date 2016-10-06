angular.module('app').controller('RestoreCtrl', function($scope, Auth, Dictionary) {
  'use strict';

  $scope.state = 'default';

  $scope.restore = function(email) {
    var data = {
        email: email
      },
      startRequest = function() {
        $scope.busy = true;
      },
      finishRequest = function() {
        $scope.busy = false;
      },
      restoreFail = function(response) {
        $scope.errorMessage = Dictionary.getValue(response.data);
      },
      restoreSuccess = function() {
        $scope.state = 'success'
      };

    startRequest();
    Auth.restore(data)
      .then(restoreSuccess, restoreFail)
        .then(finishRequest);
  };
}).$inject = ['$scope', 'Auth', 'Dictionary'];