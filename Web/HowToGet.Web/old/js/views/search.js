var app = app || {};

app.SearchView = Backbone.View.extend({
    el: '#search',

    events: {
        'click .button.search': 'search',
        'click .button.add': 'add'
    },

    initialize: function() {
        this.render();
    },

    render: function() {
        var template = _.template($('#search-template').html());

        this.$el.html(template);

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

    search: function() {
        var origin = this.$('.origin').data('id'),
            destination = this.$('.destination').data('id');

        if (origin && destination)
            app.router.navigate('/search/' + origin + '/' + destination, {trigger: true});
    },

    add: function() {
        if (app.userModel.get('auth')) {
            app.router.navigate('add', {trigger: true});
        }
    }
});