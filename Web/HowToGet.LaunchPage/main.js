(function($){
    function isValidEmailAddress(emailAddress) {
        var pattern = new RegExp(/^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$/i);
        return pattern.test(emailAddress);
    };

    var form = $('.subscribe-form'),
        fields = $('.fields'),
        emailField = $('.data'),
        processing = $('.processing'),
        submitBtn = $('.submit'),
        failCount = 0;


    form.submit(function(e) {
        e.preventDefault();

        var data = emailField.val();

        if (isValidEmailAddress(data) && failCount < 4) {
            emailField.attr('disabled', 'disabled');
            submitBtn.attr('disabled', 'disabled');

            fields.removeClass('invalid');
            processing.addClass('show');

            var request = $.ajax({
                url: '/api/subscribe/create',
                type: "POST",
                data: {
                    email: data,
                    referrer: document.referrer
                }
            });

            request.done(function() {
                emailField.removeAttr('disabled');
                submitBtn.removeAttr('disabled');

                processing.removeClass('show');
                form.removeClass('error-duplication-email');
                form.removeClass('error-any');
                form.addClass('success');
            });

            request.fail(function(msg) {
                emailField.removeAttr('disabled');
                submitBtn.removeAttr('disabled');

                processing.removeClass('show');
                form.removeClass('success');


                if (msg.status != 409) {
                    form.removeClass('error-duplication-email');
                    form.addClass('error-any');
                };

                if (msg.status == 409) {
                    failCount = failCount + 1;

                    if (failCount == 1) {
                        $('.duplication-email .text').html('Your email is&nbsp;already in&nbsp;our database and we&rsquo;re happy.<br>But don&rsquo;t try to&nbsp;press subscribe button again!');
                    };

                    if (failCount == 2) {
                        $('.duplication-email .text').html('Hey, easy! We&nbsp;already told you that we&nbsp;recorded your email to&nbsp;our database! Stop trying to&nbsp;press the subscribe button!');
                    };

                    if (failCount == 3) {
                        $('.duplication-email .text').html('That&rsquo;s enough! I quit. See you soon =)');
                    };

                    form.removeClass('error-any');
                    form.addClass('error-duplication-email');
                };
            });
        } else {
            fields.addClass('invalid');
        };
    });
})(jQuery);

(function($){

    var slider = $('.slider'),
        toggle = $('.toggle'),
        step1 = $('.step-1'),
        step2 = $('.step-2'),
        step3 = $('.step-3'),
        step4 = $('.step-4');

    step1.fadeTo(0, 0);
    step2.fadeTo(0, 0);
    step3.fadeTo(0, 0);
    step4.fadeTo(0, 0);

    toggle.click(function() {
        toggle.fadeOut();
        step1.fadeTo(1000, 1, function() {
            setTimeout(function() {
                step1.fadeTo(1000, 0);
                step2.fadeTo(1000, 1, function() {
                    $('.editable-text').teletype({
                        animDelay: 70,
                        text: 'how to get from barcelona to rimini'
                    }, function() {
                        step2.fadeTo(1000, 0);
                        step3.addClass('open');
                        step3.fadeTo(200, 1, function() {
                            setTimeout(function(){
                                step3.fadeTo(200, 0);
                                step3.removeClass('open');
                                step4.fadeTo(200, .7);
                            }, 6000);
                        });
                    });
                });
            }, 3000);
        });
    });
})(jQuery);