var GOOGLEVALIDURL    =   'https://www.googleapis.com/oauth2/v1/tokeninfo?access_token=';
var GOOGLESCOPE       =   'https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email';
var GOOGLECLIENTID    =   '345620305312-5nnj1d704h6v95a6v345fevctjm1429u.apps.googleusercontent.com';
var GOOGLEREDIRECT    =   location.origin;
var googleUrl        =   'https://accounts.google.com/o/oauth2/auth?scope=' + GOOGLESCOPE + '&client_id=' + GOOGLECLIENTID + '&redirect_uri=' + GOOGLEREDIRECT + '&response_type=token';

function validateGoogleToken(Client, $scope, token) {
    $.ajax({
        url: GOOGLEVALIDURL + token,
        data: null,
        success: function(responseText){  
            getGoogleUserInfo(Client, $scope, token);
        },  
        dataType: "jsonp"  
    });
};

function getGoogleUserInfo(Client, $scope, token) {
    $.ajax({
        url: 'https://www.googleapis.com/oauth2/v1/userinfo?access_token=' + token,
        data: null,
        success: function(response) {
            var email = response.email,
                thumbnail = '',
                firstName = response.given_name,
                lastName = response.family_name,
                gender = '',
                id = response.id,
                city = '',
                country = '';
            
            Client.extAuth({
                
                email: email,
                thumbnail: thumbnail,
                firstName: firstName,
                lastName: lastName,
                gender: gender,
                id: id,
                city: city,
                country:country,
                authService: 1,
                accessToken: token

            }, function(data) {

                $scope.error.visible = false;

                Client.authToken = data.AuthToken;

            }, function(data) {

                $scope.error.text = data || 'We\'re sorry, a server error occurred. Please wait a bit and try again.';
                $scope.error.visible = true;

            });
        },
        dataType: "jsonp"
    });
};
