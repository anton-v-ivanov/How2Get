angular.module('app').controller('HomeCtrl', function($scope, $location) {
    'use strict';

    $scope.$location = $location;
    $scope.routeLtLg = [];
    $scope.query = { origin: { id: null, lt: null, lg: null }, destination: { id: null, lt: null, lg: null } };
    
    $scope.$watch('query', function(newValue) {
        var i;

        $scope.routeLtLg = [];
        for (i = 0; i < newValue.length; i++) {
            if (newValue[i].lt && newValue[i].lg) {
                $scope.routeLtLg.push({
                    lt: newValue[i].lt,
                    lg: newValue[i].lg
                });
            }
        }
    }, true);

    $scope.$watch('routeLtLg', function(newValue, oldValue) {
        if (!angular.equals(newValue, oldValue)) {
            $scope.drowRoute($scope.routeLtLg);
        }
    });
});