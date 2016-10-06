app.directive('toggle', function($document, $location) {
    return {
        restrict: 'C',
        link: function(scope, element, attrs) {
            element.bind('click', function() {
                element.parent().toggleClass('open');
            });

            element.parent().bind('click', function(event) {
                event.stopPropagation();
            });
        }
    };
});