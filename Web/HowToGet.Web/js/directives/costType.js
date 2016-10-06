app.directive('costType', function($parse, Currency) {
    return {
        link: function(scope, element, attrs, ctrl) {
            scope.$watch('routePart.OriginCityInfo.CountryId', function(newValue, oldValue) {
                Currency.get({
                    id: newValue
                }, function(data) {
                    scope.routePart.Currencys = data;
                }, function(data) {
                    //log error
                });
            });
        }
    };
});