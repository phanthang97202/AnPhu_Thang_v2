function processAjaxForm(form, onSumitSuccess) {
    debugger;
    var ajaxFormParam = {
        async: true,
        dataType: 'json',
        beforeSend: function () {
            var formBeforeSend = $(this);
            formBeforeSend.find("[type=submit]").addClass("disabled").attr("disabled", true);
        },
        beforeSubmit: function (arr, $form, options) {
            if (!$form.valid()) {
                return false;
            }
        },
        success: function (response, statusText, xhr, $form) {
            debugger;
            //var form = $form;
            if (response === undefined) {
                return false;
            }

            var responseData = response;

            if (typeof (responseData) === 'string') {

                responseData = $.parseJSON($(responseData).text());
            }



            if (typeof onSumitSuccess !== 'undefined' && $.isFunction(onSumitSuccess)) {

                onSumitSuccess.call(this, responseData, statusText, xhr, $form);
            } else {
                processAjaxFormResult(responseData, statusText, xhr, $form);
            }
        }
    };



    form.submit(function () {
        // reset all the validation messer elements. (Change for textContent form.)
        form.find('.field-validation-error').empty();

        if (!form.valid()) {

            if (form.find('.tabs').length) {
                var selector = 'input.input-validation-error,select.input-validation-error';
                form.find(selector).parents('div.tab-content')
                    .each(function () {
                        var tab = $(this);
                        var li = $('a[href="#' + tab.attr('id') + '"]')
                            .hide().show('pulsate', {}, 100)
                            .show('highlight', {}, 200)
                            .show('pulsate', {}, 300)
                            .show('highlight', {}, 400);
                    });
            }
        }
    });


    if (!form.hasClass('no-ajax')) {

        form.ajaxForm(ajaxFormParam);
        form.submit(function () {

        });
    }



}

function processAjaxFormResult(response) {
    debugger;
    if (response.Success === true) {
        if (response.Messages && response.Messages.length > 0) {
            alert(response.Messages[0]);
        }

        if (!IsNullOrEmpty(response.RedirectUrl)) {
            window.location.href = response.RedirectUrl;
        }
        else if (!IsNullOrEmpty(response.RedirectToOpener)) {
            window.location.href = window.location.href;
        }
    }

    else {
        debugger;
        if (response.IsServiceException === true) {
            _showErrorMsg123("Có lỗi trong quá trình xử lý!", response.Detail);
        }
        else {
            debugger;
            var msgStr = '';
            for (var i = 0; i < response.Messages.length; i++) {
                msgStr += response.Messages[i] + '\r\n';
            }
            debugger;
            //var validator = form.validate();
            //var errors = [];
            for (var j = 0; j < response.FieldErrors.length; j++) {
                debugger;
                var obj = {};
                obj[response.FieldErrors[j].FieldName] = response.FieldErrors[j].ErrorMessage;
                validator.showErrors(obj);
            }
            if (msgStr.length > 0) {
                alert(msgStr);
            }
            //showErrorDialog(msgStr);

        }
    }
}

