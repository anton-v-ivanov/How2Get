var app = app || {};

app.AddRouteView = Backbone.View.extend({
    el: '#content',

    events: {
        'click #add-part': 'addPart',
        'click .delete-part': 'deletePart',
        'click #save-route': 'saveRoute'
    },

    initialize: function() {
        this.render();
    },

    render: function() {
        var template = _.template($('#add-route-template').html());

        this.$el.html(template);
        this.addPart();
    },

    addPart: function() {
        var partTemplate = _.template($('#add-route-part-template').html());

        this.$('#route').append(partTemplate);

        this.$('.autocomplete').marcoPolo({
            url: '/api/city',
            param: 'name',
            required: true,
            formatItem: function(data) {
                if (data.StateName != null)
                    return data.Name + ', ' + data.Country + ', ' + data.StateName;
                else
                    return data.Name + ', ' + data.Country;
            },
            onSelect: function(data, $item) {
                this.val(data.Name);
                this.data('id', data.Id);
            }
        });
    },

    deletePart: function() {
        var parts = this.$el.find('.route-part');

        if (parts.length > 1) $(event.target).parents('.route-part').remove();
    },

    saveRoute: function() {
        var description = $('#route-description').val().trim();
        var route = [];

        _.each($('.route-part'), function(num, key) {
            route.push({
                OriginCityId: $(num).find('.origin').data('id'),
                DestinationCityId: $(num).find('.destination').data('id'),
                Description: $(num).find('.description').val().trim(),
                CarrierName: $(num).find('.carrier-name').val().trim(),
                Time: $(num).find('.time').val().trim(),
                TimeType: $(num).find('.time-type').val().trim(),
                Cost: $(num).find('.cost').val().trim(),
                CostCurrency: $(num).find('.cost-currency').val().trim(),
                Date: $(num).find('.date').val().trim()
            });
        });
        console.log(route);

        $.ajax({
            url: '/api/route/add',
            type: 'POST',
            data: {
                Description: description,
                RouteParts: route
            },
            success: function() {

            },
            error: function() {

            }
        });
    }
});