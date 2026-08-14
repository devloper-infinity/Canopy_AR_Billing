
function login_submit() {
    var stageElement = document.getElementById("login_stage");
    var stage = stageElement ? stageElement.value : "password";
    var submitButton = document.getElementById("login_btnsubmit");

    if (stage === "password") {
        var username = document.getElementById("login_username");
        var password = document.getElementById("login_password");

        if (!username || username.value.trim() === "") {
            alert("Please enter username");
            if (username) {
                username.focus();
            }
            return false;
        }

        if (!password || password.value === "") {
            alert("Please enter password");
            if (password) {
                password.focus();
            }
            return false;
        }
    }

    if (stage === "mfa") {
        var code = document.getElementById("mfa_code");
        var normalizedCode = code ? code.value.replace(/\s+/g, "") : "";

        if (!/^\d{6}$/.test(normalizedCode)) {
            alert("Please enter the 6-digit verification code");
            if (code) {
                code.focus();
            }
            return false;
        }

        code.value = normalizedCode;
    }

    if (submitButton) {
        submitButton.disabled = true;
        submitButton.innerHTML = stage === "mfa" ? "Verifying..." : "Signing in...";
    }

    var target = document.getElementById("login_postback_target");
    if (target && typeof __doPostBack === "function") {
        __doPostBack(target.value, "");
        return false;
    }

    var form = document.getElementById("form1");
    if (form) {
        form.submit();
    }

    return false;
}
