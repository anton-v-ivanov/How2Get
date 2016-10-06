app.directive('cityAutoComplete', function($parse, Client) {
    return {
        link: function(scope, element, attrs, ctrl) {

            var parsedModel = $parse(attrs.ngModel),
                parsedCityId = $parse(attrs.cityId),
                parsedCityLg = $parse(attrs.cityLg),
                parsedCityLt = $parse(attrs.cityLt),
                parsedCountryId = $parse(attrs.countryId);

            $(element).marcoPolo({
                url: '/api/city',
                param: 'name',
                minChars: 2,
                submitOnEnter: true,
                formatItem: function(data) {

                    if (data.StateName != null) {
                        return data.Name + ', ' + data.Country + ', ' + data.StateName;
                    } else  {
                        return data.Name + ', ' + data.Country;
                    }

                },
                onSelect: function(data, $item, initial) {

                    if (!initial) {
                        scope.$apply(function() {
                            parsedModel.assign(scope, data.Name);
                            parsedCityId.assign(scope, data.Id);

                            parsedCityLt.assign(scope, data.Latitude);
                            parsedCityLg.assign(scope, data.Longitude);

                            parsedCountryId.assign(scope, data.CountryId);
                        });
                    }

                }
            });

        }
    };
});