function validateVkToken(Client, $scope, token, userId) {
    $.ajax({
        url:'https://api.vk.com/method/users.get?uids=' + userId + '&fields=sex,photo_big,city,country,bdate&access_token=' + token,
        success:function(response) {
            getVkUserInfo(Client, $scope, response.response[0], token, userId);
        },
        error: function(jqXHR, textStatus, errorThrown){
            console.error(errorThrown);
        },
        dataType: "jsonp"
    });
};


function getVkUserInfo(Client, $scope, userData, token, userId) {
    var email;
    
    var firstName = userData.first_name;
    var lastName = userData.last_name;
    var gender;
    switch(userData.sex) {
        case 1:
            gender = "female";
        break;
        case 2:
            gender = "male";
        break;
        default:
        break;
    }
    var thumbnail = userData.photo_big;
    
    var city, country;
    var cityId = userData.city,
        countryId = userData.country;

    $.ajax({
        url:'https://api.vk.com/method/places.getCityById?cids=' + cityId + '&access_token=' + token,
        success:function(r) {
            city = r.response[0].name;


		    $.ajax({
		        url:'https://api.vk.com/method/places.getCountryById?cids=' + countryId + '&access_token=' + token,
		        success:function(r) {
		            country = r.response[0].name;

                    
                    Client.extAuth({
                        
                        email: email,
                        thumbnail: thumbnail,
                        firstName: firstName,
                        lastName: lastName,
                        gender: gender,
                        id: userId,
                        city: city,
                        country:country,
                        authService: 2,
                        accessToken: token

                    }, function(data) {

                        $scope.error.visible = false;

                        Client.authToken = data.AuthToken;

                    }, function(data) {

                        if(data === 'EmailIsEmpty') {
                            vkUser.thumbnail = thumbnail;
                            vkUser.firstName = firstName;
                            vkUser.lastName = lastName;
                            vkUser.gender = gender;
                            vkUser.id = userId;
                            vkUser.city = city;
                            vkUser.country = country;
                            vkUser.authService = 2;
                            vkUser.accessToken = token;

                            Client.emailRequired = true;
                        };

                    });

		        },
		        error: function(jqXHR, textStatus, errorThrown){
		            console.error(errorThrown);
		        },
		        dataType: "jsonp"
		    });

        },
        error: function(jqXHR, textStatus, errorThrown){
            console.error(errorThrown);
        },
		dataType: "jsonp"
    });
};

