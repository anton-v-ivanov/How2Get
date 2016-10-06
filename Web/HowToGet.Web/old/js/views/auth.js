var app = app || {};

app.AuthView = Backbone.View.extend({
    el: '#sidebar-container .content',

    events: {
        'click .open-reg-form': 'openRegForm',
        'click .open-login-form': 'openLoginForm',
        'click #login-submit': 'login',
        'click #reg-submit': 'register'
    },

    initialize: function() {
        var token = $.cookie('token'),
            name = $.cookie('name'),
            id= $.cookie('id');

        if (token && name && id) {
            this.auth(token, name, id);
        } else {
            this.render();
        }
    },

    render: function() {
        var template;

        if (app.userModel.get('auth')) {
            var variables = {
                name: app.userModel.get('name'),
				id: app.userModel.get('id')
            }

            template = _.template($('#sidebar-profile-template').html(), variables);
        } else {
            template = _.template($('#auth-template').html());
        }

        this.$el.html(template);
    },

    openRegForm: function() {
        this.$('form').removeClass('active');
        this.$('#reg-form').addClass('active');
    },

    openLoginForm: function() {
        this.$('form').removeClass('active');
        this.$('#login-form').addClass('active');
    },

    login: function(event) {
        event.preventDefault();

        var email = this.$('#login-email').val().trim(),
            password = this.$('#login-password').val().trim(),
            self = this;

        app.loaderView.show();

        $.ajax({
            url: '/api/auth/login',
            type: 'POST',
            data: {
                'Email': email,
                'Password' : password
            },
            success: function(response) {
                if (response.AuthToken && response.UserName && response.UserId) {
                    self.auth(response.AuthToken, response.UserName, response.UserId);
                }
                app.loaderView.hide();
            },
            error: function() {
                app.loaderView.hide();
            }
        });
    },

    register: function(event) {
        event.preventDefault();

        var name = this.$('#reg-name').val().trim(),
            email = this.$('#reg-email').val().trim(),
            password = this.$('#reg-password').val().trim()
            self = this;

        app.loaderView.show();

        $.ajax({
            url: '/api/user/register',
            type: 'POST',
            data: {
                'UserName': name,
                'Email': email,
                'Password': password
            },
            success: function(response) {
                if (response.AuthToken && response.UserName && response.UserId) {
                    self.auth(response.AuthToken, response.UserName, response.UserId);
                }

                app.loaderView.hide();
            },
            error: function() {
                app.loaderView.hide();
            }
        });
    },

    auth: function(token, name, id) {
        app.userModel.set({
            'token': token,
            'name': name,
            'id': id,
            'auth': true
        });

        $.cookie('token', token);
        $.cookie('name', name);
        $.cookie('id', id);

        $.ajaxSetup({
            headers: {'Authorization': token}
        });

        this.render();
    }
});