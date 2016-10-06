app.factory('Route', function($http) {
    return {
        search: function(params, success, error) {

            $http({
                method: 'GET',
                url: '/api/route',
                params: {
                    origin: params.originId,
                    destination: params.destinationId
                }
            }).success(success).error(error);

        },
        get: function(params, success, error) {

            $http({
                method: 'GET',
                url: '/api/route',
                params: {
                    id: params.id
                }
            }).success(success).error(error);

        },
        create: function(data, success, error) {

            $http({
                data: data,
                method: 'PUT',
                url: '/api/route/add'
            }).success(success).error(error);

        },
        update: function(data, success, error) {

            $http({
                data: data,
                method: 'POST',
                url: '/api/route/update'
            }).success(success).error(error);
        },
        getTop: function(success, error) {
            $http({
                method: 'GET',
                url: '/api/route/top'
            }).success(success).error(error);
        },
        getRouteTime: function(params, success, error) {
            $http({
                method: 'GET',
                url: '/api/route/calc',
                params: {
                    originId: params.originId,
                    destinationId: params.destinationId,
                    carrierType: params.carrierType
                }
            }).success(success).error(error);
        },
        delete: function(params, success, error) {
            $http({
                method: 'DELETE',
                url: '/api/route?id=' + params.id,
            }).success(success).error(error);
        }
    };
});