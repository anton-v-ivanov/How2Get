app.factory('Client', function($http) {
    return {
        authToken: localStorage['Authorization'] || null,
        gender: null,
        picture: null,
        userId: null,
        userName: null,
        email: null,
        emailRequired: null,
        authorized: function() {

            return localStorage['Authorization'] ? true : false;

        },
        login: function(data, success, error) {

            $http({
                method: 'POST',
                url: '/api/auth',
                data: {
                    email: data.email,
                    password: data.password
                }
            }).success(success).error(error);

        },
        extAuth: function(data, success, error) {

            $http({
                method: 'POST',
                url: '/api/oauth',
                data: { 
                    Email: data.email,
                    Picture: data.thumbnail, 
                    FirstName: data.firstName, 
                    LastName: data.lastName, 
                    Gender: data.gender, 
                    Id: data.id,
                    City: data.city,
                    Country: data.country,
                    AuthService: data.authService,
                    referrer: document.referrer,
                    AccessToken: data.accessToken,
                    InviteCode: data.inviteCode
                }
            }).success(success).error(error);

        },
        register: function(data, success, error) {

            $http({
                method: 'POST',
                url: '/api/user/register',
                data: {
                    userName: data.name,
                    email: data.email,
                    password: data.password,
                    referrer: document.referrer,
                    inviteCode: data.inviteCode
                }
            }).success(success).error(error);

        },
        logout: function(success, error) {

            $http({
                method: 'DELETE',
                url: '/api/auth'
            }).success(success).error(error);

        },
        saveSession: function(token) {

            localStorage.setItem('Authorization', token);

        },
        deleteSession: function() {

            localStorage.removeItem('Authorization');

        },
        getInfo: function(success, error) {

            $http({
                method: 'GET',
                url: '/api/user/self'
            }).success(success).error(error);

        },
        getRoutes: function(token, success, error) {

            $http({
                method: 'GET',
                url: '/api/route'
            }).success(success).error(error);

        },
        getInvites: function(success, error) {

            $http({
                method: 'GET',
                url: '/api/invite'
            }).success(success).error(error);

        },
        sendInvite: function(data, success, error) {

            $http({
                method: 'POST',
                url: '/api/invite',
                data: {
                    inviteId: data.inviteId,
                    email: data.email
                }
               
            }).success(success).error(error);

        },
        cancelInvite: function(data, success, error) {

            $http({
                method: 'DELETE',
                url: '/api/invite',
                data: {
                    inviteId: data.inviteId
                }
               
            }).success(success).error(error);

        }

    };
});