app.controller('PromoCtrl', function($scope) {
    $scope.isVisibility = localStorage['promoVisibility'] || true;

    $scope.hide = function() {
        $scope.isVisibility = false;
        localStorage.setItem('promoVisibility', false);
    };
});