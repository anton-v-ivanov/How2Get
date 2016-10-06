app.directive('time', function($parse) {
    return {
        restrict: 'E',
        scope: {
            model: '=',
            origin: '=',
            destination: '=',
            type: '='
        },

        template: '<span class="data-time"><input type="text" size="1" ng-model="time.days" ng-disabled="!origin || !destination || !type" required /> days <input type="text" size="1" ng-model="time.hours" ng-disabled="!origin || !destination || !type" required /> hours <input type="text" size="1" ng-model="time.minutes" ng-disabled="!origin || !destination || !type" required /> min </span>',

        link: function(scope, element, attrs) {

            var minToTime = function(minutes) {
                var days = Math.floor(scope.model / 1440),
                    hours = Math.floor(scope.model / 60) - days * 24,
                    minutes = scope.model % 60;

                return {
                    days: days,
                    hours: hours,
                    minutes: minutes
                };
            };

            var time = minToTime(scope.model);

            scope.time = {
                days: time.days,
                hours: time.hours,
                minutes: time.minutes
            };

            scope.$watch('time', function(newValue) {
                var daysToMin = parseInt(newValue.days || 0) * 1440,
                    hoursToMin = parseInt(newValue.hours || 0) * 60,
                    minutesToMin = parseInt(newValue.minutes || 0);

                scope.model = daysToMin + hoursToMin + minutesToMin;
            }, true);

            scope.$watch('model', function(newValue, oldValue) {
                if (newValue !== oldValue) {
                    var time = minToTime(scope.model);

                    scope.time.days = time.days;
                    scope.time.hours = time.hours;
                    scope.time.minutes = time.minutes;
                };
            });
        }
    };
});