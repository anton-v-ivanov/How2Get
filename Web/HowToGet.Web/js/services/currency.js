app.factory('Currency', function($http) {
    return {
        get: function(params, success, error) {
            $http({
                method: 'GET',
                url: '/api/currency',
                params: {
                    countryId: params.id
                }
            }).success(success).error(error);
        }
    };
});