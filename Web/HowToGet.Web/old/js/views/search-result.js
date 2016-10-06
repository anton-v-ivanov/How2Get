var app = app || {};

app.SearchResultView = Backbone.View.extend({
    el: "#content",

    initialize: function() {
        this.render();
    },

    render: function() {
        var variables,
            template;

        variables = {routes: app.routeCollection.toJSON()};
        template = _.template($('#search-result-template').html(), variables);

        this.$el.html(template);
    }
});