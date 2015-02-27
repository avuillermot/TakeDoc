function userTk(user) {
    if (user != null) {
        this.FirstName = user.UserTkFirstName;
        this.LastName = user.UserTkLastName;
        this.Email = user.UserTkEmail
        this.Telephone = "";
        this.Id = user.UserTkId;
        this.Entitys = new Array();
        that = this;
        $.each(user.Entitys, function(index, value) {
            if (value.EtatDeleteData == false){
                var myEntity =  new entity(value);
                that.Entitys.push(myEntity);
            }
        });
        this.IsLog = true;
    }
};

function userTkService() {

}

userTkService.logon = function (login, pwd, success, error) {
    var url = environnement.UrlBase + "identity/logon";
    var data = { login: login, password: pwd };
    $.ajax({
        type: 'POST',
        url: url,
        data: { '': JSON.stringify(data)},
        success: success,
        error: error
    });
};