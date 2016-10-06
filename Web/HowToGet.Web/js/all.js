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
angular.module('app').controller('AddRouteCtrl', function($scope, Route) {
  'use strict';
  
}).$inject = ['$scope', 'Route'];
angular.module('app').controller('SearchRouteCtrl', function($scope, $routeParams, City, Route) {
  'use strict';

  $scope.query = {
    origin: City.get({
      id: $routeParams.origin
    }),
    destination: City.get({
      id: $routeParams.destination
    })
  };
  $scope.routes = Route.query({
    destination: $routeParams.destination,
    origin: $routeParams.origin
  });
}).$inject = ['$scope', '$routeParams', 'City', 'Route'];
angular.module('app').directive('extAuth', function() {
  'use strict';

  return {
    templateUrl: 'partials/components/extAuth.html',
    replace: true
  };
});
angular.module('app').directive('route', function() {
  'use strict';

  return {
    templateUrl: 'partials/components/route/item.html',
    replace: true
  };
});
angular.module('app').directive('routes', function() {
  'use strict';

  return {
    templateUrl: 'partials/components/route/list.html',
    replace: true
  };
});
angular.module('app').directive('searchRouteForm', function($location) {
  'use strict';

  return {
    replace: true,
    templateUrl: 'partials/components/searchForm.html',
    link: function(scope) {
      scope.search = function(query) {
        if (!query.origin.Id || !query.destination.Id) return;
        $location.path('/route/search/' + query.origin.Id + '/' + query.destination.Id);
      };
    }
  };
}).$inject = ['$location'];
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
angular.module('app').factory('City', function($resource) {
  'use strict';

  return $resource('/api/city');
}).$inject = ['$resource'];
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
angular.module('app').factory('Invite', function($resource) {
  'use strict';

  return $resource('/api/invite/');
}).$inject = ['$resource'];
angular.module('app').factory('Route', function($resource) {
  'use strict';

  return $resource('/api/route/:id', {
    id: '@Id'
  });
}).$inject = ['$resource'];
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
angular.module('app').factory('User', function($resource) {
  'use strict';

  return $resource('/api/user/:userId', {
    userId: '@Id'
  });
}).$inject = ['$resource'];