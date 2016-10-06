var fbAppId;
var fsqAppId = 'NKB5XDB2RW1QMGRYPMFGGMHC5XFRRYVEMLFO43PAZXNY1HHL';
var vkAppId = 3865579;

switch(location.hostname) {
    case 'rutta.me':
        fbAppId = '163144587222538';
        break;
    case 'stage.rutta.me':
        fbAppId = '163144587222538';
        break;
    default:
        fbAppId = '496741520385173';
        break;
};


app.controller('ExtAuthFormCtrl', function($scope, Client) {
    vkUser = {};
    $scope.error = {};

    $scope.fbAuth = function() {
            FB.getLoginStatus(function(response) {
                if (response.status === 'connected') {
                    getFbUserInfo(Client, $scope, response.authResponse.accessToken);
                } 
                else {
                    FB.login(function(response) {
                        if (response.authResponse) {
                            // connected
                            getFbUserInfo(Client, $scope, response.authResponse.accessToken);
                        } 
                        else {
                            // cancelled
                        }
                    }, {scope:'email,user_location'});
                }
            });
    };


    $scope.googleAuth = function() {
        
        var win = window.open(googleUrl, "gwindow", 'width=800, height=600'); 
        
        var pollTimer = window.setInterval(function() { 
            if (win.document && win.document.URL.indexOf(GOOGLEREDIRECT) != -1) {
                window.clearInterval(pollTimer);
                var url = win.document.URL;
                var token = parseUrl(url, 'access_token');
                win.close();
                
                if(token)
                    validateGoogleToken(Client, $scope, token);
            }
        }, 100);
    };


    $scope.foursquareAuth = function() {
        var redirect_uri = location.origin;
        var auth_url = 'https://foursquare.com/oauth2/authenticate?client_id=' + fsqAppId + '&response_type=token&redirect_uri=' + redirect_uri;

        var win = window.open(auth_url, "fwindow", 'width=800, height=600'); 
        
        var pollTimer = window.setInterval(function() { 
            if (win.document && win.document.URL.indexOf(FOURSQUAREREDIRECT) != -1) {
                window.clearInterval(pollTimer);
                var url = win.document.URL;
                var token = parseUrl(url, 'access_token');
                win.close();

                if(token)
                    validateFoursquareToken(Client, $scope, token);
            }
        }, 100);
    };


    $scope.vkAuth = function() {
        var redirect_uri = location.origin;
        var auth_url = 'https://oauth.vk.com/authorize?client_id=' + vkAppId + '&redirect_uri=' + redirect_uri + '&response_type=token';

        var win = window.open(auth_url, "vkwindow", 'width=800, height=600'); 
        
        var pollTimer = window.setInterval(function() { 
            if (win.document && win.document.URL.indexOf(location.origin) != -1) {
                window.clearInterval(pollTimer);
                var url = win.document.URL;
                var token = parseUrl(url, 'access_token');
                var userId = parseUrl(url, 'user_id');
                win.close();

                if(token)
                    validateVkToken(Client, $scope, token, userId);
            }
        }, 100);

    };

    $scope.emailEntered = function() {
        Client.extAuth({
            
            email: $scope.user.email,
            inviteCode: $scope.user.Invite,
            thumbnail: vkUser.thumbnail,
            firstName: vkUser.firstName,
            lastName: vkUser.lastName,
            gender: vkUser.gender,
            id: vkUser.id,
            city: vkUser.city,
            country: vkUser.country,
            authService: vkUser.authService,
            accessToken: vkUser.accessToken

        }, function(data) {

            $scope.error.visible = false;

            Client.authToken = data.AuthToken;
            Client.emailRequired = null;

        }, function(data) {

            $scope.error.text = data || 'We\'re sorry, a server error occurred. Please wait a bit and try again.';
            $scope.error.visible = true;

        });

    };

    $scope.closeEmailRequired = function() {
        vkUser = {};
        $scope.user = {};
        Client.emailRequired = null;
    };

    function parseUrl(url, name) {
        name = name.replace(/[\[]/,"\\\[").replace(/[\]]/,"\\\]");
        var regexS = "[\\#&]"+name+"=([^&#]*)";
        var regex = new RegExp( regexS );
        var results = regex.exec( url );
        if(results == null)
            return "";
        else
            return results[1];
    };
});
