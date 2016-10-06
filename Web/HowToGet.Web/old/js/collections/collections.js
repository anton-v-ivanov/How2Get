var app = app || {};

app.RouteCollection = Backbone.Collection.extend();
app.UserRouteCollection = Backbone.Collection.extend();

app.routeCollection = new app.RouteCollection();
app.userRouteCollection = new app.UserRouteCollection();