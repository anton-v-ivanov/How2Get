var app = app || {};

app.ProfileView = Backbone.View.extend({
    el: '#content',

    initialize: function() {
        var id = app.userModel.get('id'),
            self = this;

        app.loaderView.show();
        app.userRouteCollection.url = '/api/route?userId=' + id;
        app.userRouteCollection.fetch({
            success: function() {
                self.render();
                app.loaderView.hide();
            }
        });
    },

    render: function() {
        var variables = {
            name: app.userModel.get('name'),
            routes: app.userRouteCollection.toJSON()
        };

        var template = _.template($('#profile-template').html(), variables);

        this.$el.html(template);
    }
});