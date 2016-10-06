angular.module('app').factory('Dictionary', function() {
  'use strict';

  var dictionary = {
    IncorrectLoginOrPassword: 'Incorrect login or password',
    DuplicateEmail: 'This email is already registered. Try to restore your password',
    InvalidEmail: 'This email has invalid format. Please try another email',
    UnknownInvite: 'Invite code is not valid :(',
    ServerError: 'We\'re sorry, a server error occurred. Please wait a bit and try again'
  };

  return {
    getValue: function(key) {
      var value = dictionary[key];

      return value ? value : dictionary.ServerError;
    }
  };
});