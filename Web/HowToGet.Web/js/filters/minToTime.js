app.filter('minToTime', function() {
    return function(minutes) {

        var days = Math.floor(minutes / 1440),
            hr = Math.floor(minutes / 60) -  days * 24,
            min = minutes % 60,
            result = '';

        if (days > 0) {
            result = result + days + ' days ';
        };

        if (hr > 0) {
            result = result + hr + ' hours ';
        };

        if (min > 0) {
            result = result + min + ' min';
        };

        return result;

    };
});