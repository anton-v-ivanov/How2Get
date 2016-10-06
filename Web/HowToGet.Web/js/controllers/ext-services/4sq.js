var FOURSQUAREREDIRECT = location.origin;


function validateFoursquareToken(Client, $scope, token) {
    $.ajax({
        url:'https://api.foursquare.com/v2/users/self',
        data: {oauth_token:token},
        success:function(user_data) {
            getFoursquareUserInfo(Client, $scope, user_data.response.user, token);
        },
        error: function(jqXHR, textStatus, errorThrown){
            console.error(errorThrown);
        }
    });
};


function getFoursquareUserInfo(Client, $scope, userData, token) {
    
    var userEmail;
    if(userData.contact && userData.contact.email)
        userEmail = userData.contact.email;

    var email = userEmail,
                thumbnail = userData.photo,
                firstName = userData.firstName,
                lastName = userData.lastName,
                gender = userData.gender,
                id = userData.id,
                city = userData.homeCity,
                country = '',
                accessToken = token;
            
    Client.extAuth({
        
        email: email,
        thumbnail: thumbnail,
        firstName: firstName,
        lastName: lastName,
        gender: gender,
        id: id,
        city: city,
        country:country,
        authService: 3,
        accessToken: accessToken

    }, function(data) {

        $scope.error.visible = false;

        Client.authToken = data.AuthToken;

    }, function(data) {

        $scope.error.text = data || 'We\'re sorry, a server error occurred. Please wait a bit and try again.';
        $scope.error.visible = true;

    });
};

