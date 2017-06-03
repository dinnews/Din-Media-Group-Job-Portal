function CallAllOtherFunctions_forJobSeeker() {
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

function CallAllOtherFunctions_for_Employer() {
    if (!FirstNameValidation())
        return false;
    if (!LastNameValidation())
        return false;
    if (!EmailValidation())
        return false;
    if (!CompanyValidation())
        return false;
    if (!PasswordValidation())
        return false;
    if (!ReEnter_PasswordValidation())
        return false
    if (!MobileNoValidation())
        return false;
    if (!CNIC_Validation())
        return false;
   

    return true;
}

function FirstNameValidation() {
    var onlydigitsExp = /^[0-9]+$/;
    var valfirst_name = $("#first_name").val();

    if (valfirst_name == null || valfirst_name == "") {
        $("#errror_firstname").text("*First Name is required");
        return false;
    }
    else {
        $("#errror_firstname").text("");
    }
    if (valfirst_name.match(onlydigitsExp) || valfirst_name.length <= 1) {
        $("#errror_firstname").text("please enter valid firstname");
        return false;
    } else {
        $("#errror_firstname").text("");
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

function CompanyValidation() {
    var companyName = $("#companyName").val();
    var onlydigitsExp8 = /^[0-9]+$/;
    if (companyName == null || companyName == "") {
        $("#error_company").text("*company name is required");
        return false;
    }
    else {
        $("#error_company").text("");
    }
    if (companyName.match(onlydigitsExp8) || companyName.length <= 1) {
        $("#error_company").text("please enter valid company name");
        return false;
    } else {
        $("#error_company").text("");
    }
    return true;
}

function MobileNoValidation() {
    var mobileno = $("#mobile").val();
    var mobileExp = /^((\+92)|(0092))-{0,1}\d{3}-{0,1}\d{7}$|^\d{11}$|^\d{4}-\d{7}$/;
    var mobileExp2 = /^[\s()+-]*([0-9][\s()+-]*){6,20}$/;
    if (!mobileno.match(mobileExp) || !mobileno.match(mobileExp2)) {
        $("#error_mobile").text("*valid mobile number is required");
        return false;
    }
    else {
        $("#error_mobile").text("");
    }
    return true;
} 

function CNIC_Validation() {
    var cnicExp = /^[0-9+]{5}-[0-9+]{7}-[0-9]{1}$/;
    var cnic = $("#cnic").val();
    if (!cnic.match(cnicExp) || !cnic.match(cnicExp)) {
        $("#error_cnic").text("*valid cnic number is required");
        return false;
    }
    else {
        $("#error_cnic").text("");
    }
    return true;

}