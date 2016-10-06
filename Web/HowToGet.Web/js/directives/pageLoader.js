angular.module('app').directive('pageLoader', function($rootScope) {
    'use strict';

    return {
        link: function(scope, element) {
            $rootScope.$on('$routeChangeStart', function() {
                element.addClass('b-page__loader_state_active');
            });

            $rootScope.$on('$routeChangeSuccess', function() {
                element.removeClass('b-page__loader_state_active');
            });
        }
    };
});