app.value('$', $);

app.factory('hubProxy', ['$', '$rootScope', function($, $rootScope) {
    var hubProxy = {
        connection: null,
        proxy: null,
        intializeClient: function (token) {
            hubProxy.connection = $.hubConnection();
            hubProxy.proxy = hubProxy.connection.createHubProxy('announce');
            hubProxy.configureClientFuntions();
            hubProxy.start(token);
        },
        configureClientFuntions: function () {
            hubProxy.proxy.on('addNotification', function (notification) {
                $rootScope.$broadcast('addNotificationResponse', notification);
            });
            hubProxy.proxy.on('removeNotification', function (id) {
                $rootScope.$broadcast('removeNotificationResponse', id);
            });
        },
        start: function (token) {
            hubProxy.connection.qs = { "t" : token };
            hubProxy.connection.start()
            .done(function() {
                console.log('connected');
            })
            .fail(function() {
                console.log('connection failed');
            })
        },
        stop: function () {
            if(hubProxy.connection) {
                hubProxy.connection.stop();
            console.log('disconnected');
            }
        },
        markRead: function (id, token) {
            hubProxy.connection.qs = { "t" : token };
            hubProxy.proxy.invoke('markRead', id);
        }
    };
    return hubProxy;
}]);