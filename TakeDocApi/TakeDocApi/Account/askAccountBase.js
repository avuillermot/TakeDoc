$(document).ready(function () {
    $("#ask-account-template").load("askAccountTemplate/askAccountTemplate.html");
});

askAccountBase = {

    entity: null,
    firstName: null,
    lastName: null,
    email: null,
    password1: null,
    password2: null,
    culture: null,

    collectData: function() {
        this.entity = $("#inputEntityRef").val();
        this.firstName = $("#inputFirstName").val();
        this.lastName = $("#inputLastName").val();
        this.email = $("#inputMail").val();
        this.password1 = $("#inputPassword1").val();
        this.password2 = $("#inputPassword2").val();
        this.culture = $("#inputCulture").val();
    },
    checkData: function () {
        $(".form-group").removeClass("has-error");

        if (this.password1 != this.password2) this.setControlState("divInputPassword");
        if (this.password1.length < 5) this.setControlState("divInputPassword");
        if (this.entity == "") this.setControlState("divInputEntity");
        if (this.firstName == "") this.setControlState("divInputFirstName");
        if (this.lastName == "") this.setControlState("divInputLastName");
        if (this.email == "") this.setControlState("divInputMail");
        if (this.culture == "") this.setControlState("divInputCulture");
    },
    setControlState: function (input) {
        var elem = $("#" + input);
        elem.addClass("has-error");
    },
    requestAccount: function() {
        this.collectData();
        this.checkData();

        var hasError = $(".form-group.has-error").length > 0;
        if (hasError == false) {
            var url = environnement.UrlBase + "/identity/request/account";

            var data = {
                entity: this.entity,
                firstName: this.firstName,
                lastName: this.lastName,
                email: this.email,
                password1: this.password1,
                password2: this.password2,
                culture: this.culture
            };
            $.ajax({
                type: 'POST',
                url: url,
                data: { '': JSON.stringify(data) },
                success: this.requestAccount_success,
                error: this.requestAccount_error
            });
        }
    },
    requestAccount_success: function () {
        alert("ok");
    },
    requestAccount_error: function () {
        var error = arguments[0];
        alert(error.responseJSON.Message);
    },
}

