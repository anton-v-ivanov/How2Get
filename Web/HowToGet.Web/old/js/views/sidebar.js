var app = app || {};

app.SidebarView = Backbone.View.extend({
    el: '#sidebar-container',

    events: {
        'click .sidebar-toggle': 'toggle'
    },

    initialize: function() {
        this.render();
    },

    render: function() {
        var template = _.template($('#sidebar-template').html());

        this.$el.html(template);
    },

    toggle: function() {
        if (this.$el.hasClass('open')) {
            this.close();
        } else {
            this.open();
        }
    },

    open: function() {
        if (!this.$el.hasClass('open')) {
            this.$el.addClass('open');
        }
    },

    close: function() {
        if (this.$el.hasClass('open')) {
            this.$el.removeClass('open');
        }
    }
});