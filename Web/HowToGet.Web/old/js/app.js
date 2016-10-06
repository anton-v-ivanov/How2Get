var app = app || {};

app.Router = Backbone.Router.extend({
    routes: {
        '': 'home',
        'search/:origin/:destination': 'searchResult',
        'add': 'addRoute',
        'registration': 'registration',
        'login': 'login',
        'logout': 'logout',
        'profile': 'profile'
    },

    initialize: function() {
        app.userModel = new app.UserModel();
        app.sidebarView = new app.SidebarView();
        app.authView = new app.AuthView();
        app.searchView = new app.SearchView();
        app.loaderView = new app.LoaderView();
    },

    home: function() {
        if (!app.searchView) {
            app.searchView = new app.SearchView();
        } else {
            app.searchView.render();
        }

        $('#content').html('');
    },

    profile: function() {
        if (app.userModel.get('auth')) {
            app.profileView = new app.ProfileView();
        } else {
            app.sidebarView.open();
        }
    },

    searchResult: function(origin, destination) {
        app.routeCollection.url = '/api/route/?origin=' + origin + '&destination=' + destination;
        app.loaderView.show();
        app.routeCollection.fetch({
            success: function() {
                app.searchResultView = new app.SearchResultView();
                app.loaderView.hide();
            }
        });
    },

    addRoute: function(){
        console.log('ok')
        if (!app.addRouteView)
            app.addRouteView = new app.AddRouteView();
        else
            app.addRouteView.render();
    },

    registration: function() {
        if(!app.registrationView)
            app.registrationView = new app.RegistrationView();
        else
            app.registrationView.render();
    },

    login: function() {
        if (!app.loginView)
            app.loginView = new app.LoginView();
        else
            app.loginView.render();
    },

    logout: function() {
        app.logoutView = new app.LogoutView();
    }
});