app.directive('carrierAutoComplete', function($parse) {
    return {
        link: function(scope, element, attrs, ctrl) {

            var parsedModel = $parse(attrs.ngModel),
                type = 0,
                countryId = 0;

            scope.$watch('routePart.CarrierType', function(newValue, oldValue) {
                type = newValue || 0;
                autocomplete();
            });

            scope.$watch('routePart.OriginCityInfo.CountryId', function(newValue, oldValue) {
                countryId = newValue || 0;
                autocomplete();
            });

            function autocomplete() {
                $(element).marcoPolo({
                    url: '/api/carrier?type=' + type + '&countryId=' + countryId,
                    param: 'name',
                    minChars: 2,
                    submitOnEnter: true,
                    formatItem: function(data) {
                        return data.Name;
                    },
                    onSelect: function(data, $item, initial) {

                        if (!initial) {
                            scope.$apply(function() {
                                parsedModel.assign(scope, data.Name);
                            });
                        };
                    }
                });
            };
        }
    };
});
