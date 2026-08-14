/******************************************************************************
 *  Global Session Timeout Handler
 *  ------------------------------------------------------------
 *  Automatically handles:
 *   ✔ Fetch API calls
 *   ✔ jQuery AJAX calls
 *   ✔ Redirects to login page when session expires (401)
 *
 *  No changes required to your existing code anywhere else.
 ******************************************************************************/

(function () {

    // ----------------------------------------------------------
    // 1️⃣ GLOBAL FETCH() OVERRIDE
    // ----------------------------------------------------------
    const originalFetch = window.fetch;

    window.fetch = function (...args) {

        return originalFetch(...args).then(response => {

            // If session expired
            if (response.status === 401) {
                console.warn("Session expired (fetch). Redirecting...");
                window.location.href = "~/Login.aspx";      // <-- change if needed
                return;
            }

            return response;
        });
    };


    // ----------------------------------------------------------
    // 2️⃣ GLOBAL jQuery AJAX ERROR HANDLER
    // ----------------------------------------------------------
    if (typeof jQuery !== "undefined") {
        $(document).ajaxError(function (event, jqxhr, settings, thrownError) {

            if (jqxhr.status === 401) {
                console.warn("Session expired (AJAX). Redirecting...");
                window.location.href = "~/Login.aspx";      // <-- change if needed
            }

        });
    }

    // ----------------------------------------------------------
    // 3️⃣ AUTO-ATTACH COOKIES TO FETCH (optional but recommended)
    // ----------------------------------------------------------
    const fetchWithCredentials = window.fetch;

    window.fetch = function (...args) {
        let [input, options] = args;

        // Ensure cookies/session are always sent with fetch
        if (typeof options === "object") {
            options.credentials = "include";
        } else {
            options = { credentials: "include" };
        }

        return fetchWithCredentials(input, options);
    };

})();
