var notificationCtrl = function ($scope, hubProxy, Client) {
    $scope.notifications = [];
    $scope.markRead = function (id) {
        hubProxy.markRead(id, Client.authToken);
    };
    $scope.$on('addNotificationResponse', function (event, notifications) {
        $scope.$apply(function () {
        	var found = false;
			for(var i = 0; i < notifications.length; i++) {
				for(var j = 0; j < $scope.notifications.length; j++) {
		    		if ($scope.notifications[j].Id == notifications[i].Id) {
		        		found = true;
		    			break;
				    }
				}
				if(!found)
            		$scope.notifications.push(notifications[i]);
			}
        });
    });
    $scope.$on('removeNotificationResponse', function (event, id) {
        $scope.$apply(function () {
        	var index = -1;
			for(var i = 0; i < $scope.notifications.length; i++) {
	    		if ($scope.notifications[i].Id === id) {
	        		$scope.notifications.splice(i, 1);
	    			break;
			    }
			}
        });
    });
};
