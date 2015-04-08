function userTk(user) {
    if (user != null) {
        this.FirstName = user.UserTkFirstName;
        this.LastName = user.UserTkLastName;
        this.Email = user.UserTkEmail
        this.Telephone = "";
        this.Id = user.UserTkId;
        this.Entitys = new Array();
        this.Culture = user.UserTkCulture;
        this.GroupUserId = user.UserTkGroupId;
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

userTkService.dashboard = function (userId, success, error) {
    var url = environnement.UrlBase + "identity/dashboard/<userId/>";
    url = url.replace("<userId/>", userId);
    $.ajax({
        type: 'GET',
        url: url,
        success: success,
        error: error
    });
};