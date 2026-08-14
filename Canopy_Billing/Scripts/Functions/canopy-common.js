(function (window, document) {
    "use strict";

    function getErrorMessage(error, fallback) {
        if (!error) {
            return fallback || "Something went wrong. Please try again.";
        }

        if (typeof error === "string") {
            return error;
        }

        if (typeof error.get_message === "function") {
            return error.get_message();
        }

        if (error.responseText) {
            return error.responseText;
        }

        if (error.message) {
            return error.message;
        }

        return fallback || "Something went wrong. Please try again.";
    }

    function showError(error, fallback) {
        var message = getErrorMessage(error, fallback);

        if (window.toastr && typeof window.toastr.error === "function") {
            window.toastr.error(message);
            return false;
        }

        window.alert(message);
        return false;
    }

    function debug() {
        try {
            if (window.localStorage && window.localStorage.getItem("canopyDebug") === "true" && window.console) {
                window.console.log.apply(window.console, arguments);
            }
        } catch (ignore) {
        }
    }

    window.CanopyUI = window.CanopyUI || {};
    window.CanopyUI.getErrorMessage = getErrorMessage;
    window.CanopyUI.showError = showError;
    window.CanopyUI.debug = debug;

    document.addEventListener("click", function (event) {
        var disabledLink = event.target.closest ? event.target.closest("a.disabled, button.disabled") : null;
        if (disabledLink) {
            event.preventDefault();
        }
    });
})(window, document);
