app.controller('HeaderCtrl', function($scope, $location, Client) {
    $scope.$location = $location;
    $scope.client = Client;
});