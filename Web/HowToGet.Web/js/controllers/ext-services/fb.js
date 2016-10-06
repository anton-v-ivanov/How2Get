function getFbUserInfo(Client, $scope, token) {
    FB.api('/me', function(response) {
        var email = response.email,
            firstName = response.first_name,
            lastName = response.last_name,
            gender = response.gender,
            id = response.id;

        var location = response.location.name.split(', ');
        var city = location[0];
        var country = location[1];

        FB.api('/me/picture', function(response) {
            var thumbnail = response.data.url;
            Client.extAuth({

                email: email,
                thumbnail: thumbnail,
                firstName: firstName,
                lastName: lastName,
                gender: gender,
                id: id,
                city: city,
                country:country,
                authService: 0,
                accessToken: token

            }, function(data) {

                $scope.error.visible = false;

                Client.authToken = data.AuthToken;

            }, function(data) {

                $scope.error.text = data || 'We\'re sorry, a server error occurred. Please wait a bit and try again.';
                $scope.error.visible = true;

            });
        });

    });
};


window.fbAsyncInit = function() {
    FB.init({
        appId      : fbAppId, // App ID
        channelUrl : '/channel.html', // Channel File
        status     : true, // check login status
        cookie     : true, // enable cookies to allow the server to access the session
        xfbml      : true  // parse XFBML
    });
};

(function(d){
    var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
    if (d.getElementById(id)) {return;}
    js = d.createElement('script'); js.id = id; js.async = true;
    js.src = "//connect.facebook.net/en_US/all.js";
    ref.parentNode.insertBefore(js, ref);
}(document));