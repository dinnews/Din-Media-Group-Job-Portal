function CallAllOtherFunctions_forEmployee() {
    if (!FirstNameValidation())
        return false;
    if (!LastNameValidation())
        return false;
    if (!EmailValidation())
        return false;
    if (!GenderValidation())
        return false;
    if (!PasswordValidation())
        return false;
    if (!ReEnter_PasswordValidation())
        return false
    if (!LocationValidation())
        return false;
    if (!Job_TitleValidation())
        return false;
    if (!CatagoryValidation())
        return false;

    return true;
}

function numberOfEmployeesValidation() {
    var onlydigitsExp = /^[0-9]+$/;
    var noOfEmployees = $("#numberOfEmployees").val();

    if (noOfEmployees == null || noOfEmployees == "") {
        $("#errror_noOfEmployees").text("*Number of Employees are required");
        return false;
    }
    else {
        $("#errror_noOfEmployees").text("");
    }
    if (!noOfEmployees.match(onlydigitsExp) || noOfEmployees.length <= 1) {
        $("#errror_noOfEmployees").text("please enter valid valid number of employees");
        return false;
    } else {
        $("#errror_noOfEmployees").text("");
    }
    return true;
}

function LastNameValidation() {
    var vallast_name = $("#last_name").val();
    var onlydigitsExp2 = /^[0-9]+$/;
    if (vallast_name == null || vallast_name == "") {
        $("#error_lastname").text("*Last Name is required");
        return false;
    }
    else {
        $("#error_lastname").text("");
    }
    if (vallast_name.match(onlydigitsExp2) || vallast_name.length <= 1) {
        $("#error_lastname").text("please enter valid lastname");
        return false;
    } else {
        $("#error_lastname").text("");
    }
    return true;
}

function PasswordValidation() {
    var password = $("#password1").val();
    if (password == null || password == "" || password.length <= 0) {
        $("#error_password").text("*Password is required");
        return false;
    }
    else {
        $("#error_password").text("");
    }
    if (password.length <= 6 || password.length >= 32) {
        $("#error_password").text("Password length must be between 6 and 32");
        return false;
    } else {
        $("#error_password").text("");
    }
    return true;
}

function ReEnter_PasswordValidation() {
    var password = $("#password2").val();
    var password_FirstEntered = $("#password1").val();
    if (password == null || password == "" || password.length <= 0) {
        $("#error_password2").text("*Repeat password is required");
        return false;
    }
    else {
        $("#error_password2").text("");
    }
    if (password_FirstEntered != null || password_FirstEntered != "" || password_FirstEntered.length > 0) {
        if (!password.match(password_FirstEntered)) {
            $("#error_password2").text("Repeat password not matches with already entered password");
            return false;
        }
        else {
            $("#error_password2").text("");
        }

    } else {
        $("#error_password2").text("*Both password fields are required");
    }
    return true;
}

function LocationValidation() {
    var location = $("#work_location").val();
    var onlydigitsExp2 = /^[0-9]+$/;
    if (location == null || location == "") {
        $("#error_location").text("*location is required");
        return false;
    }
    else {
        $("#error_location").text("");
    }
    if (location.match(onlydigitsExp2) || location.length <= 1) {
        $("#error_location").text("please enter valid location");
        return false;
    } else {
        $("#error_location").text("");
    }
    return true;
}

function Job_TitleValidation() {
    var jobtitle = $("#job_title").val();
    var onlydigitsExp3 = /^[0-9]+$/;
    if (jobtitle == null || jobtitle == "") {
        $("#error_jobtitle").text("*job Title is required");
        return false;
    }
    else {
        $("#error_jobtitle").text("");
    }
    if (jobtitle.match(onlydigitsExp3) || jobtitle.length <= 1) {
        $("#error_jobtitle").text("please enter valid job title");
        return false;
    } else {
        $("#error_jobtitle").text("");
    }
    return true;
}

function UrlValidation() {
    var url = $("#url").val();
    var onlydigitsExp8 = /^[0-9\s]/;
    if (url.match(onlydigitsExp8)) {
        $("#error_url").text("please enter valid url");
        return false;
    } else {
        $("#error_url").text("");
    }

    if (url.length <= 1) {
        $("#error_url").text("please enter valid url");
        return false;
    } else {
        $("#error_url").text("");
    }
    return true;
}

function UrlNameValidation() {
    var urlname = $("#url_name").val();
    var onlydigitsExp8 = /^[0-9\s]/;
    if (urlname.match(onlydigitsExp8)) {
        $("#error_urlname").text("please enter valid url name");
        return false;
    } else {
        $("#error_urlname").text("");
    }

    if (urlname.length <= 1) {
        $("#error_urlname").text("please enter valid url name");
        return false;
    } else {
        $("#error_urlname").text("");
    }
    return true;
}

function SummaryValidation() {
    var summary = $("#summary").val();
    var onlydigitsExp8 = /^[0-9\s]/;
    if (summary == null || summary == "") {
        $("#error_Summary").text("*summary is required");
        return false;
    }
    else {
        $("#error_Summary").text("");
    }
    if (summary.match(onlydigitsExp8)) {
        $("#error_Summary").text("please enter valid summary about you");
        return false;
    } else {
        $("#error_Summary").text("");
    }

    if (summary.trim().split(/\s+/).length <= 45) {
        $("#error_Summary").text("your summary must consist more than 50 words");
        return false;
    } else {
        $("#error_Summary").text("");
    }
    return true;
}

function MobileNoValidation() {
    var mobileno = $("#mobile").val();
    var mobileExp = /^((\+92)|(0092))-{0,1}\d{3}-{0,1}\d{7}$|^\d{11}$|^\d{4}-\d{7}$/;
    var mobileExp2 = /^[\s()+-]*([0-9][\s()+-]*){6,20}$/;
    if (mobileno.length <= 0)
    {
        $("#error_mobile").text("*mobile number is required");
        return false;
    }
    else {
        $("#error_mobile").text("");
    }
    if (!mobileno.match(mobileExp) || !mobileno.match(mobileExp2)) {
        $("#error_mobile").text("*valid mobile number is required");
        return false;
    }
    else {
        $("#error_mobile").text("");
    }
    return true;
}

function ProfilePicture_Validation() {
    var size = $('#profile_picture')["0"].firstChild.files["0"].size;
    alert(size);
    if (size > 4000) {
        $("#error_ProfilePhoto").text("*valid Profile Photo is required");
        return false;
    }
    else {
        $("#error_ProfilePhoto").text("");
    }
    return true;

}