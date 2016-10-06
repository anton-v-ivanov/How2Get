app.factory('Token', function($http) {
	return {
	    exchange: function(params, success, error) {
            $http({
                method: 'GET',
                url: '/api/token',
                params: {
                    token: params.token
                }
            }).success(success).error(error);
        },
};
});