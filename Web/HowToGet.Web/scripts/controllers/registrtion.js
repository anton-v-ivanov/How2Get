angular.module('app').controller('RegistrationCtrl', function($scope, $location, Session, Auth, Dictionary) {
  'use strict';

  $scope.registration = function(name, email, password) {
    var data = {
        password: password,
        userName: name,
        email: email
      },
      startRequest = function() {
        $scope.registrationFormBusy = true;
      },
      finishRequest = function() {
        $scope.registrationFormBusy = false;
      },
      registrationFail = function(response) {
        $scope.registrationFormError = Dictionary.getValue(response.data);
      },
      registrationSuccess = function(response) {
        var token = response.data.AuthToken;

        if (!token) return;
        Session.save(token);
        $location.path('/');
      };

    startRequest();
    Auth.registration(data)
      .then(registrationSuccess, registrationFail)
        .then(finishRequest);
  };
}).$inject = ['$scope', '$location', 'Session', 'Auth', 'Dictionary'];