app.directive('googleMap', function() {
    return {
        restrict: 'E',
        template: '<div class="map-wrapper"><div id="map-canvas"></div></div>',
        controller: function($scope) {
            var mapOptions = {
                    center: new google.maps.LatLng(55.75222, 37.61556),
                    zoom: 2,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                },
                map = new google.maps.Map(document.getElementById("map-canvas"), mapOptions),
                markersArray = [],
                coordinatesArray = [],
                routePath = null,

                clearOverlays = function(noClearMarkers) {
                    if (!noClearMarkers) {
                        for (var i = 0; i < markersArray.length; i++ ) {
                            markersArray[i].setMap(null);
                        };
                    };

                    markersArray.length = 0;
                    coordinatesArray.length = 0;

                    if (routePath) {
                        routePath.setMap(null);
                    };
                };

            $scope.drowRoute = function(routeLtLg, onlyMarkers, noClearMarkers) {
                clearOverlays(noClearMarkers);

                $.each(routeLtLg, function(index, value) {
                    markersArray.push(new google.maps.Marker({
                        position: new google.maps.LatLng(value.lt, value.lg)
                    }));
                    coordinatesArray.push(new google.maps.LatLng(value.lt, value.lg));

                    markersArray[index].setMap(map);
                });

                if (!onlyMarkers) {
                    routePath = new google.maps.Polyline({
                        path: coordinatesArray,
                        strokeColor: "#FF0000",
                        strokeOpacity: 0.7,
                        strokeWeight: 1.5
                    });

                    routePath.setMap(map);
                };
            };

            $scope.clearMap = function() {
                clearOverlays();
            };
        }
    };
});