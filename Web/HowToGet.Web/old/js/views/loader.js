var app = app || {};

app.LoaderView = Backbone.View.extend({
    el: '#loader',

    initialize: function() {
        this.hide();
    },

    show: function() {
        this.$el.addClass('show');
    },

    hide: function() {
        this.$el.removeClass('show');
    }
});