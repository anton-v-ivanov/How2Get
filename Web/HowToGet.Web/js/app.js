var app = angular.module('app', ['ngResource', 'ngRoute']);

app.config(function($routeProvider, $locationProvider) {
    $routeProvider
        .when('/', {
            templateUrl: 'partials/pages/home.html'
        })
        .when('/route/search/:origin/:destination', {
            templateUrl: 'partials/pages/route/search.html',
            controller: 'SearchRouteCtrl'
        })
        .when('/login', {
            templateUrl: 'partials/pages/login.html',
            controller: 'LoginCtrl'
        })
        .when('/registration', {
            templateUrl: 'partials/pages/registration.html',
            controller: 'RegistrationCtrl'
        })
        .when('/restore', {
            templateUrl: 'partials/pages/restore.html',
            controller: 'RestoreCtrl'
        });

    $routeProvider.when('/route/:id', {
        templateUrl: 'partials/route.html',
        controller: 'RouteCtrl',
        resolve: {
            route: routeCtrl.loadRoute
        }
    });

    $routeProvider.when('/add-route/:origin?/:destination?', {
        templateUrl: 'partials/addRoute.html',
        controller: 'AddRouteCtrl'
    });

    $routeProvider.when('/edit-route/:id', {
        templateUrl: 'partials/editRoute.html',
        controller: 'EditRouteCtrl',
        resolve: {
            route: editRouteCtrl.loadRoute
        }
    });

    $routeProvider.when('/profile', {
        templateUrl: 'partials/profile.html',
        controller: 'ProfileCtrl',
        resolve: {
            clientRoutes: profileCtrl.loadClientRoutes,
            invites: profileCtrl.loadInvites
        }
    });

    $routeProvider
        .when('/auth/:token/:url*', {
            templateUrl: 'partials/pages/token.html'
    });

    $routeProvider.otherwise({
        redirectTo: '/404'
    });

});

app.run(function($http, $rootScope, $route, $location, Client, hubProxy) {
    $rootScope.client = Client;

    $rootScope.$watch('client.authToken', function(newValue) {
        if (newValue == null) {
            
            Client.deleteSession();

            Client.userId = null;
            Client.userName = null;
            Client.picture = null;
            Client.email = null;
            Client.gender = null;

            hubProxy.stop();

        } else {
            $http.defaults.headers.common['Authorization'] = Client.authToken;

            Client.getInfo(function(data) {
                Client.saveSession(newValue);

                Client.userId = data.UserId;
                Client.userName = data.UserName;
                Client.picture = data.Picture;
                Client.email = data.Email;
                Client.gender = data.Gender;
                $route.reload();
                hubProxy.intializeClient(Client.authToken);

                if ($location.path() == "/login" || $location.path() == "/register") {
                    $location.path( "/" );
                }
            });
        };
    });
});
