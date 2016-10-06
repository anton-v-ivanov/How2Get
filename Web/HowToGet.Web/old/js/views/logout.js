var app = app || {};

app.LogoutView = Backbone.View.extend({
    initialize: function() {
        this.logout();
    },

    logout: function() {
        $.ajax({
            url: '/api/auth/logout',
            type: 'POST',
            success: function() {
                app.userModel.clear();
            },
            error: function() {

            }
        });
    }
});