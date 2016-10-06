app.directive('dropdownToggle', function($document, $location) {
    var opened;

    return {
        restrict: 'C',
        link: function(scope, element, attrs) {
            var open = function() {

                    if (opened) {
                        opened.removeClass('open');
                    };

                    opened = element.parent();
                    element.parent().addClass('open');
                    $document.bind('click', close);

                },
                close = function() {

                    element.parent().removeClass('open');
                    $document.unbind('click', close);

                };

            element.bind('click', open);
            element.parent().bind('click', function(event) {
                event.stopPropagation();
            });
        }
    };
});