function blankForNull(s) {
    return s == "null" || s == null ? "" : s;
}

function dashboard_arBind_displayERP_New_Working() {

    $('#load1').show();

    $.ajax({
        url: "DashboardNew.aspx/GetClientwiseDashboardNew",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",

        success: function (data) {

            var dataArray = JSON.parse(data.d);

            if (!dataArray || dataArray.length === 0) {
                $('#load1').hide();
                alert("No data found");
                return;
            }

            var firstRow = dataArray[0];

            function formatNumber(value, decimals) {

                value = parseFloat(value || 0);

                var absValue = Math.abs(value).toLocaleString('en-US', {
                    minimumFractionDigits: decimals,
                    maximumFractionDigits: decimals
                });

                return value < 0 ? '(' + absValue + ')' : absValue;
            }

            // =========================
            // GROUP MONTHS
            // =========================
            var groups = {};
            var totals = {};

            Object.keys(firstRow).forEach(function (key) {

                if (key === "ResultID" || key === "Process")
                    return;

                var parts = key.split("-ERP-");

                if (!parts || parts.length < 2)
                    return;

                var month = parts[0];
                var type = parts[1];

                if (!groups[month]) {
                    groups[month] = {
                        LoanCount: null,
                        Amount: null
                    };
                }

                if (type === "LoanCount")
                    groups[month].LoanCount = key;

                if (type === "Amount")
                    groups[month].Amount = key;
            });

            var monthKeys = Object.keys(groups);

            // =========================
            // YTD KEYS
            // =========================
            var ytdCountKey = "__YTD_COUNT__";
            var ytdAmountKey = "__YTD_AMOUNT__";

            // =========================
            // CALCULATE YTD PER ROW
            // =========================
            dataArray.forEach(function (row) {

                var ytdCount = 0;
                var ytdAmt = 0;

                monthKeys.forEach(function (m) {

                    var cKey = groups[m].LoanCount;
                    var aKey = groups[m].Amount;

                    ytdCount += parseFloat(row[cKey] || 0);
                    ytdAmt += parseFloat(row[aKey] || 0);
                });

                row[ytdCountKey] = ytdCount;
                row[ytdAmountKey] = ytdAmt;
            });

            // =========================
            // INIT TOTALS (MONTH ONLY)
            // =========================
            monthKeys.forEach(function (m) {

                if (groups[m].LoanCount)
                    totals[groups[m].LoanCount] = 0;

                if (groups[m].Amount)
                    totals[groups[m].Amount] = 0;
            });

            // =========================
            // CALCULATE TOTALS
            // =========================
            dataArray.forEach(function (row) {

                monthKeys.forEach(function (m) {

                    var cKey = groups[m].LoanCount;
                    var aKey = groups[m].Amount;

                    if (cKey)
                        totals[cKey] += parseFloat(row[cKey] || 0);

                    if (aKey)
                        totals[aKey] += parseFloat(row[aKey] || 0);
                });
            });

            // =========================
            // HEADER (YTD FIRST)
            // =========================
            var header1 = "<tr>";
            var header2 = "<tr>";

            header1 += "<th rowspan='2' style='text-align:left;'>Process</th>";

            // =========================
            // YTD FIRST
            // =========================
            header1 += "<th colspan='3' style='text-align:center;background:#e6e6e6;'>YTD</th>";

            header2 += "<th style='text-align:right;'>Count</th>";
            header2 += "<th style='text-align:right;'>US $</th>";
            header2 += "<th style='text-align:right;'>Rate</th>";

            // =========================
            // MONTHS AFTER
            // =========================
            monthKeys.forEach(function (m) {

                header1 += "<th colspan='3' style='text-align:center;'>" + m + "</th>";

                header2 += "<th style='text-align:right;'>Count</th>";
                header2 += "<th style='text-align:right;'>US $</th>";
                header2 += "<th style='text-align:right;'>Rate</th>";
            });

            header1 += "</tr>";
            header2 += "</tr>";

            var theadHtml = header1 + header2;

            // =========================
            // DESTROY TABLE
            // =========================
            if ($.fn.DataTable.isDataTable('#dashboard_arProcess')) {
                $('#dashboard_arProcess').DataTable().destroy();
            }

            $('#dashboard_arProcess').html(
                "<thead>" + theadHtml + "</thead><tbody></tbody>"
            );

            // =========================
            // COLUMNS (YTD FIRST)
            // =========================
            var columns = [];

            columns.push({
                data: "Process",
                className: "dt-left"
            });

            // =========================
            // YTD COLUMNS FIRST
            // =========================
            columns.push({
                data: ytdCountKey,
                className: "dt-right",
                render: function (data) {
                    return formatNumber(data, 0);
                }
            });

            columns.push({
                data: ytdAmountKey,
                className: "dt-right",
                render: function (data) {
                    return formatNumber(data, 2);
                }
            });

            columns.push({
                data: null,
                className: "dt-right",
                render: function (data, type, row) {

                    var c = parseFloat(row[ytdCountKey] || 0);
                    var a = parseFloat(row[ytdAmountKey] || 0);

                    if (c === 0) return "0.00";

                    return formatNumber(a / c, 2);
                }
            });

            // =========================
            // MONTH COLUMNS
            // =========================
            monthKeys.forEach(function (m) {

                columns.push({
                    data: groups[m].LoanCount,
                    className: "dt-right",
                    render: function (data) {
                        return formatNumber(data, 0);
                    }
                });

                columns.push({
                    data: groups[m].Amount,
                    className: "dt-right",
                    render: function (data) {
                        return formatNumber(data, 2);
                    }
                });

                columns.push({
                    data: null,
                    className: "dt-right",
                    render: function (data, type, row) {

                        var c = parseFloat(row[groups[m].LoanCount] || 0);
                        var a = parseFloat(row[groups[m].Amount] || 0);

                        if (c === 0) return "0.00";

                        return formatNumber(a / c, 2);
                    }
                });
            });

            // =========================
            // DATATABLE INIT
            // =========================
            $('#dashboard_arProcess').DataTable({

                data: dataArray,
                columns: columns,

                dom: 't',

                paging: false,
                searching: false,
                ordering: false,

                scrollX: true,
                scrollY: "400px",
                scrollCollapse: true,
                fixedColumns: {
                    leftColumns: 1
                },

                autoWidth: false,

                columnDefs: [
                    { targets: 0, className: "dt-left" },
                    { targets: "_all", className: "dt-right" }
                ],

                initComplete: function () {
                    $('#load1').hide();
                }
            });
        },

        error: function (err) {
            $('#load1').hide();
            CanopyUI.debug(err);
        }
    });

    return false;
}

function dashboard_arBind_displayERP_New_1() {

    $('#load1').show();

    $.ajax({
        url: "DashboardNew.aspx/GetClientwiseDashboardNew",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",

        success: function (data) {

            var dataArray = JSON.parse(data.d);

            if (!dataArray || dataArray.length === 0) {
                $('#load1').hide();
                alert("No data found");
                return;
            }

            var firstRow = dataArray[0];

            // =========================
            // FORMAT FUNCTION
            // =========================
            function formatNumber(value, decimals) {

                value = parseFloat(value || 0);

                var absValue = Math.abs(value).toLocaleString('en-US', {
                    minimumFractionDigits: decimals,
                    maximumFractionDigits: decimals
                });

                return value < 0 ? '(' + absValue + ')' : absValue;
            }

            // =========================
            // GROUP MONTHS
            // =========================
            var groups = {};
            var totals = {};

            Object.keys(firstRow).forEach(function (key) {

                if (key === "ResultID" || key === "Process")
                    return;

                var parts = key.split("-ERP-");

                if (!parts || parts.length < 2)
                    return;

                var month = parts[0];
                var type = parts[1];

                if (!groups[month]) {
                    groups[month] = {
                        LoanCount: null,
                        Amount: null
                    };
                }

                if (type === "LoanCount")
                    groups[month].LoanCount = key;

                if (type === "Amount")
                    groups[month].Amount = key;
            });

            var monthKeys = Object.keys(groups);

            // =========================
            // YTD KEYS
            // =========================
            var ytdCountKey = "__YTD_COUNT__";
            var ytdAmountKey = "__YTD_AMOUNT__";

            var ytdTotalCount = 0;
            var ytdTotalAmount = 0;

            // =========================
            // CALCULATE ROW YTD + GRAND YTD
            // =========================
            dataArray.forEach(function (row) {

                var rowYTDCount = 0;
                var rowYTDAmount = 0;

                monthKeys.forEach(function (m) {

                    var cKey = groups[m].LoanCount;
                    var aKey = groups[m].Amount;

                    var c = parseFloat(row[cKey] || 0);
                    var a = parseFloat(row[aKey] || 0);

                    rowYTDCount += c;
                    rowYTDAmount += a;
                });

                row[ytdCountKey] = rowYTDCount;
                row[ytdAmountKey] = rowYTDAmount;

                // GRAND TOTALS (FIXED)
                ytdTotalCount += rowYTDCount;
                ytdTotalAmount += rowYTDAmount;
            });

            // =========================
            // INIT MONTH TOTALS
            // =========================
            monthKeys.forEach(function (m) {

                if (groups[m].LoanCount)
                    totals[groups[m].LoanCount] = 0;

                if (groups[m].Amount)
                    totals[groups[m].Amount] = 0;
            });

            // =========================
            // CALCULATE MONTH TOTALS
            // =========================
            dataArray.forEach(function (row) {

                monthKeys.forEach(function (m) {

                    var cKey = groups[m].LoanCount;
                    var aKey = groups[m].Amount;

                    if (cKey)
                        totals[cKey] += parseFloat(row[cKey] || 0);

                    if (aKey)
                        totals[aKey] += parseFloat(row[aKey] || 0);
                });
            });

            // =========================
            // HEADER (YTD FIRST + FIXED TOTALS)
            // =========================
            var header1 = "<tr>";
            var header2 = "<tr>";

            header1 += "<th rowspan='2' style='text-align:left;'>Process</th>";

            // =========================
            // YTD FIRST
            // =========================
            header1 += "<th colspan='3' style='text-align:center;background:#e6e6e6;'>YTD</th>";

            header2 += "<th style='text-align:right;'>Count : <br><b>" + formatNumber(ytdTotalCount, 0) + "</b></th>";
            header2 += "<th style='text-align:right;'>US $ : <br><b>" + formatNumber(ytdTotalAmount, 2) + "</b></th>";
            header2 += "<th style='text-align:right;'>Rate</th>";

            // =========================
            // MONTHS
            // =========================
            monthKeys.forEach(function (m) {

                header1 += "<th colspan='3' style='text-align:center;'>" + m + "</th>";

                header2 += "<th style='text-align:right;'>Count</th>";
                header2 += "<th style='text-align:right;'>US $</th>";
                header2 += "<th style='text-align:right;'>Rate</th>";
            });

            header1 += "</tr>";
            header2 += "</tr>";

            var theadHtml = header1 + header2;

            // =========================
            // DESTROY TABLE
            // =========================
            if ($.fn.DataTable.isDataTable('#dashboard_arProcess')) {
                $('#dashboard_arProcess').DataTable().destroy();
            }

            $('#dashboard_arProcess').html(
                "<thead>" + theadHtml + "</thead><tbody></tbody>"
            );

            // =========================
            // COLUMNS
            // =========================
            var columns = [];

            columns.push({
                data: "Process",
                className: "dt-left"
            });

            // =========================
            // YTD COLUMNS FIRST
            // =========================
            columns.push({
                data: ytdCountKey,
                className: "dt-right",
                render: function (data) {
                    return formatNumber(data, 0);
                }
            });

            columns.push({
                data: ytdAmountKey,
                className: "dt-right",
                render: function (data) {
                    return formatNumber(data, 2);
                }
            });

            columns.push({
                data: null,
                className: "dt-right",
                render: function (data, type, row) {

                    var c = parseFloat(row[ytdCountKey] || 0);
                    var a = parseFloat(row[ytdAmountKey] || 0);

                    if (c === 0) return "0.00";

                    return formatNumber(a / c, 2);
                }
            });

            // =========================
            // MONTH COLUMNS
            // =========================
            monthKeys.forEach(function (m) {

                columns.push({
                    data: groups[m].LoanCount,
                    className: "dt-right",
                    render: function (data) {
                        return formatNumber(data, 0);
                    }
                });

                columns.push({
                    data: groups[m].Amount,
                    className: "dt-right",
                    render: function (data) {
                        return formatNumber(data, 2);
                    }
                });

                columns.push({
                    data: null,
                    className: "dt-right",
                    render: function (data, type, row) {

                        var c = parseFloat(row[groups[m].LoanCount] || 0);
                        var a = parseFloat(row[groups[m].Amount] || 0);

                        if (c === 0) return "0.00";

                        return formatNumber(a / c, 2);
                    }
                });
            });

            // =========================
            // DATATABLE INIT
            // =========================
            $('#dashboard_arProcess').DataTable({

                data: dataArray,
                columns: columns,

                dom: 't',

                paging: false,
                searching: false,
                ordering: false,

                scrollX: true,
                scrollY: '400px',

                fixedHeader: true,

                fixedColumns: {
                    leftColumns: 1
                },

                autoWidth: false,

                columnDefs: [
                    { targets: 0, className: "dt-left" },
                    { targets: "_all", className: "dt-right" }
                ],

                initComplete: function () {
                    $('#load1').hide();
                }
            });
        },

        error: function (err) {
            $('#load1').hide();
            CanopyUI.debug(err);
        }
    });

    return false;
}


function dashboard_arBind_displayERP_New_2() {

    $('#load1').show();

    $.ajax({
        url: "DashboardNew.aspx/GetClientwiseDashboardNew",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",

        success: function (data) {

            var dataArray = JSON.parse(data.d);

            if (!dataArray || dataArray.length === 0) {
                $('#load1').hide();
                alert("No data found");
                return;
            }

            var firstRow = dataArray[0];

            // =========================
            // FORMAT FUNCTION
            // =========================
            function formatNumber(value, decimals) {

                value = parseFloat(value || 0);

                var absValue = Math.abs(value).toLocaleString('en-US', {
                    minimumFractionDigits: decimals,
                    maximumFractionDigits: decimals
                });

                return value < 0 ? '(' + absValue + ')' : absValue;
            }

            // =========================
            // MONTH PARSE (SORT FIX)
            // =========================
            function parseMonthKey(m) {

                var parts = m.split("-");

                var monthNames = {
                    "Jan": 0, "Feb": 1, "Mar": 2, "Apr": 3,
                    "May": 4, "Jun": 5, "Jul": 6, "Aug": 7,
                    "Sep": 8, "Oct": 9, "Nov": 10, "Dec": 11
                };

                var month = parts[0];
                var year = parseInt(parts[1] || "0");

                return new Date(year, monthNames[month] || 0, 1);
            }

            // =========================
            // GROUP MONTHS
            // =========================
            var groups = {};
            var totals = {};

            Object.keys(firstRow).forEach(function (key) {

                if (key === "ResultID" || key === "Process")
                    return;

                var parts = key.split("-ERP-");

                if (!parts || parts.length < 2)
                    return;

                var month = parts[0];
                var type = parts[1];

                if (!groups[month]) {
                    groups[month] = {
                        LoanCount: null,
                        Amount: null
                    };
                }

                if (type === "LoanCount")
                    groups[month].LoanCount = key;

                if (type === "Amount")
                    groups[month].Amount = key;
            });

            // =========================
            // SORT MONTHS (IMPORTANT FIX)
            // =========================
            var monthKeys = Object.keys(groups).sort(function (a, b) {
                return parseMonthKey(a) - parseMonthKey(b);
            });

            // =========================
            // YTD KEYS
            // =========================
            var ytdCountKey = "__YTD_COUNT__";
            var ytdAmountKey = "__YTD_AMOUNT__";

            var ytdTotalCount = 0;
            var ytdTotalAmount = 0;

            // =========================
            // CALCULATE YTD PER ROW + GRAND TOTAL
            // =========================
            dataArray.forEach(function (row) {

                var rowYTDCount = 0;
                var rowYTDAmount = 0;

                monthKeys.forEach(function (m) {

                    var cKey = groups[m].LoanCount;
                    var aKey = groups[m].Amount;

                    var c = parseFloat(row[cKey] || 0);
                    var a = parseFloat(row[aKey] || 0);

                    rowYTDCount += c;
                    rowYTDAmount += a;
                });

                row[ytdCountKey] = rowYTDCount;
                row[ytdAmountKey] = rowYTDAmount;

                ytdTotalCount += rowYTDCount;
                ytdTotalAmount += rowYTDAmount;
            });

            // =========================
            // INIT MONTH TOTALS
            // =========================
            monthKeys.forEach(function (m) {

                if (groups[m].LoanCount)
                    totals[groups[m].LoanCount] = 0;

                if (groups[m].Amount)
                    totals[groups[m].Amount] = 0;
            });

            // =========================
            // CALCULATE MONTH TOTALS
            // =========================
            dataArray.forEach(function (row) {

                monthKeys.forEach(function (m) {

                    var cKey = groups[m].LoanCount;
                    var aKey = groups[m].Amount;

                    if (cKey)
                        totals[cKey] += parseFloat(row[cKey] || 0);

                    if (aKey)
                        totals[aKey] += parseFloat(row[aKey] || 0);
                });
            });

            // =========================
            // HEADER (YTD FIRST)
            // =========================
            var header1 = "<tr>";
            var header2 = "<tr>";

            header1 += "<th rowspan='2' style='text-align:left;'>Process</th>";

            // YTD FIRST
            header1 += "<th colspan='3' style='text-align:center;background:#e6e6e6;'>YTD</th>";

            header2 += "<th style='text-align:right;'>Count : <br><b>" + formatNumber(ytdTotalCount, 0) + "</b></th>";
            header2 += "<th style='text-align:right;'>US $ : <br><b>" + formatNumber(ytdTotalAmount, 2) + "</b></th>";
            header2 += "<th style='text-align:right;'>Rate</th>";

            // MONTHS SORTED
            monthKeys.forEach(function (m) {

                header1 += "<th colspan='3' style='text-align:center;'>" + m + "</th>";

                header2 += "<th style='text-align:right;'>Count</th>";
                header2 += "<th style='text-align:right;'>US $</th>";
                header2 += "<th style='text-align:right;'>Rate</th>";
            });

            header1 += "</tr>";
            header2 += "</tr>";

            var theadHtml = header1 + header2;

            // =========================
            // DESTROY TABLE
            // =========================
            if ($.fn.DataTable.isDataTable('#dashboard_arProcess')) {
                $('#dashboard_arProcess').DataTable().destroy();
            }

            $('#dashboard_arProcess').html(
                "<thead>" + theadHtml + "</thead><tbody></tbody>"
            );

            // =========================
            // COLUMNS
            // =========================
            var columns = [];

            columns.push({
                data: "Process",
                className: "dt-left"
            });

            // YTD COLUMNS FIRST
            columns.push({
                data: ytdCountKey,
                className: "dt-right",
                render: function (data) {
                    return formatNumber(data, 0);
                }
            });

            columns.push({
                data: ytdAmountKey,
                className: "dt-right",
                render: function (data) {
                    return formatNumber(data, 2);
                }
            });

            columns.push({
                data: null,
                className: "dt-right",
                render: function (data, type, row) {

                    var c = parseFloat(row[ytdCountKey] || 0);
                    var a = parseFloat(row[ytdAmountKey] || 0);

                    return c === 0 ? "0.00" : formatNumber(a / c, 2);
                }
            });

            // MONTH COLUMNS
            monthKeys.forEach(function (m) {

                columns.push({
                    data: groups[m].LoanCount,
                    className: "dt-right",
                    render: function (data) {
                        return formatNumber(data, 0);
                    }
                });

                columns.push({
                    data: groups[m].Amount,
                    className: "dt-right",
                    render: function (data) {
                        return formatNumber(data, 2);
                    }
                });

                columns.push({
                    data: null,
                    className: "dt-right",
                    render: function (data, type, row) {

                        var c = parseFloat(row[groups[m].LoanCount] || 0);
                        var a = parseFloat(row[groups[m].Amount] || 0);

                        return c === 0 ? "0.00" : formatNumber(a / c, 2);
                    }
                });
            });

            // =========================
            // DATATABLE INIT
            // =========================
            $('#dashboard_arProcess').DataTable({

                data: dataArray,
                columns: columns,

                dom: 't',

                paging: false,
                searching: false,
                ordering: false,

                scrollX: true,
                scrollY: '400px',

                fixedHeader: true,

                fixedColumns: {
                    leftColumns: 1
                },

                autoWidth: false,

                columnDefs: [
                    { targets: 0, className: "dt-left" },
                    { targets: "_all", className: "dt-right" }
                ],

                initComplete: function () {
                    $('#load1').hide();
                }
            });
        },

        error: function (err) {
            $('#load1').hide();
            CanopyUI.debug(err);
        }
    });

    return false;
}

function dashboard_arBind_displayERP_New_22() {

    $('#load1').show();

    $.ajax({
        url: "DashboardNew.aspx/GetClientwiseDashboardNew",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",

        success: function (data) {

            var dataArray = JSON.parse(data.d);

            if (!dataArray || dataArray.length === 0) {
                $('#load1').hide();
                alert("No data found");
                return;
            }

            var firstRow = dataArray[0];

            // =========================
            // FORMAT FUNCTION
            // =========================
            function formatNumber(value, decimals) {
                value = parseFloat(value || 0);

                var absValue = Math.abs(value).toLocaleString('en-US', {
                    minimumFractionDigits: decimals,
                    maximumFractionDigits: decimals
                });

                return value < 0 ? '(' + absValue + ')' : absValue;
            }

            // =========================
            // MONTH SORT FUNCTION
            // =========================
            function parseMonthKey(m) {

                var parts = m.split("-");

                var monthNames = {
                    "Jan": 0, "Feb": 1, "Mar": 2, "Apr": 3,
                    "May": 4, "Jun": 5, "Jul": 6, "Aug": 7,
                    "Sep": 8, "Oct": 9, "Nov": 10, "Dec": 11
                };

                var month = parts[0];
                var year = parseInt(parts[1] || "0");

                return new Date(year, monthNames[month] || 0, 1);
            }

            // =========================
            // GROUP MONTH COLUMNS
            // =========================
            var groups = {};

            Object.keys(firstRow).forEach(function (key) {

                if (key === "ResultID" || key === "Process")
                    return;

                var parts = key.split("-ERP-");
                if (!parts || parts.length < 2)
                    return;

                var month = parts[0];
                var type = parts[1];

                if (!groups[month]) {
                    groups[month] = {
                        LoanCount: null,
                        Amount: null
                    };
                }

                if (type === "LoanCount")
                    groups[month].LoanCount = key;

                if (type === "Amount")
                    groups[month].Amount = key;
            });

            // SORT MONTHS
            var monthKeys = Object.keys(groups).sort(function (a, b) {
                return parseMonthKey(a) - parseMonthKey(b);
            });

            // =========================
            // YTD + MONTH SUMMARY OBJECTS
            // =========================
            var ytdKeyC = "__YTD_C__";
            var ytdKeyA = "__YTD_A__";

            var monthSummary = {};
            var ytdTotalC = 0;
            var ytdTotalA = 0;

            // =========================
            // CALCULATE ALL TOTALS
            // =========================
            dataArray.forEach(function (row) {

                var rowYTD_C = 0;
                var rowYTD_A = 0;

                monthKeys.forEach(function (m) {

                    var cKey = groups[m].LoanCount;
                    var aKey = groups[m].Amount;

                    var c = parseFloat(row[cKey] || 0);
                    var a = parseFloat(row[aKey] || 0);

                    rowYTD_C += c;
                    rowYTD_A += a;

                    if (!monthSummary[m]) {
                        monthSummary[m] = { count: 0, amount: 0 };
                    }

                    monthSummary[m].count += c;
                    monthSummary[m].amount += a;
                });

                row[ytdKeyC] = rowYTD_C;
                row[ytdKeyA] = rowYTD_A;

                ytdTotalC += rowYTD_C;
                ytdTotalA += rowYTD_A;
            });

            // MONTH RATE
            Object.keys(monthSummary).forEach(function (m) {
                var c = monthSummary[m].count;
                var a = monthSummary[m].amount;
                monthSummary[m].rate = (c === 0) ? 0 : (a / c);
            });

            var ytdRate = (ytdTotalC === 0) ? 0 : (ytdTotalA / ytdTotalC);

            // =========================
            // DESTROY OLD TABLE
            // =========================
            if ($.fn.DataTable.isDataTable('#dashboard_arProcess')) {
                $('#dashboard_arProcess').DataTable().destroy();
            }

            // =========================
            // HEADER BUILD
            // =========================
            var header1 = "<tr>";
            var header2 = "<tr>";

            header1 += "<th rowspan='2'>Process</th>";

            // YTD
            header1 += "<th colspan='3' style='background:#e6e6e6;'>YTD</th>";

            header2 += "<th>Count : <br><b>" + formatNumber(ytdTotalC, 0) + "</b></th>";
            header2 += "<th>US $ : <br><b>" + formatNumber(ytdTotalA, 2) + "</b></th>";
            header2 += "<th>Rate : <br><b>" + formatNumber(ytdRate, 2) + "</b></th>";

            // MONTHS
            monthKeys.forEach(function (m) {

                header1 += "<th colspan='3'>" + m + "</th>";

                header2 += "<th>Count : <br><b>" + formatNumber(monthSummary[m].count, 0) + "</b></th>";
                header2 += "<th>US $ : <br><b>" + formatNumber(monthSummary[m].amount, 2) + "</b></th>";
                header2 += "<th>Rate : <br><b>" + formatNumber(monthSummary[m].rate, 2) + "</b></th>";
            });

            header1 += "</tr>";
            header2 += "</tr>";

            var theadHtml = header1 + header2;

            $('#dashboard_arProcess').html("<thead>" + theadHtml + "</thead><tbody></tbody>");

            // =========================
            // COLUMNS
            // =========================
            var columns = [];

            columns.push({
                data: "Process",
                className: "dt-left"
            });

            // YTD
            columns.push({
                data: ytdKeyC,
                className: "dt-right",
                render: d => formatNumber(d, 0)
            });

            columns.push({
                data: ytdKeyA,
                className: "dt-right",
                render: d => formatNumber(d, 2)
            });

            columns.push({
                data: null,
                className: "dt-right",
                render: function (d, t, r) {
                    var c = r[ytdKeyC];
                    var a = r[ytdKeyA];
                    return c === 0 ? "0.00" : formatNumber(a / c, 2);
                }
            });

            // MONTHS
            monthKeys.forEach(function (m) {

                columns.push({
                    data: groups[m].LoanCount,
                    className: "dt-right",
                    render: d => formatNumber(d, 0)
                });

                columns.push({
                    data: groups[m].Amount,
                    className: "dt-right",
                    render: d => formatNumber(d, 2)
                });

                columns.push({
                    data: null,
                    className: "dt-right",
                    render: function (d, t, r) {
                        var c = r[groups[m].LoanCount];
                        var a = r[groups[m].Amount];
                        return c === 0 ? "0.00" : formatNumber(a / c, 2);
                    }
                });
            });

            // =========================
            // INIT DATATABLE
            // =========================
            $('#dashboard_arProcess').DataTable({

                data: dataArray,
                columns: columns,

                dom: 't',
                paging: false,
                searching: false,
                ordering: false,

                scrollX: true,
                scrollY: '400px',

                fixedHeader: true,
                fixedColumns: { leftColumns: 1 },

                autoWidth: false,

                initComplete: function () {
                    $('#load1').hide();
                }
            });
        },

        error: function (err) {
            $('#load1').hide();
            CanopyUI.debug(err);
        }
    });

    return false;
}

function dashboard_arBind_displayERP_New_33() {

    $('#load1').show();

    $.ajax({
        url: "DashboardNew.aspx/GetClientwiseDashboardNew",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",

        success: function (data) {

            var dataArray = JSON.parse(data.d);

            if (!dataArray || dataArray.length === 0) {
                $('#load1').hide();
                alert("No data found");
                return;
            }

            var firstRow = dataArray[0];

            // =========================
            // NUMBER FORMAT (ERP STYLE)
            // =========================
            function formatNumber(value, decimals) {
                value = Number(value || 0);

                var formatted = value.toLocaleString('en-IN', {
                    minimumFractionDigits: decimals,
                    maximumFractionDigits: decimals
                });

                return value < 0 ? '(' + formatted + ')' : formatted;
            }

            // =========================
            // MONTH SORT FUNCTION
            // =========================
            function parseMonthKey(m) {

                var parts = m.split("-");

                var monthNames = {
                    "Jan": 0, "Feb": 1, "Mar": 2, "Apr": 3,
                    "May": 4, "Jun": 5, "Jul": 6, "Aug": 7,
                    "Sep": 8, "Oct": 9, "Nov": 10, "Dec": 11
                };

                var month = parts[0];
                var year = parseInt(parts[1] || "0");

                return new Date(year, monthNames[month] || 0, 1);
            }

            // =========================
            // GROUP MONTH COLUMNS
            // =========================
            var groups = {};

            Object.keys(firstRow).forEach(function (key) {

                if (key === "ResultID" || key === "Process")
                    return;

                var parts = key.split("-ERP-");
                if (!parts || parts.length < 2)
                    return;

                var month = parts[0];
                var type = parts[1];

                if (!groups[month]) {
                    groups[month] = { LoanCount: null, Amount: null };
                }

                if (type === "LoanCount")
                    groups[month].LoanCount = key;

                if (type === "Amount")
                    groups[month].Amount = key;
            });

            var monthKeys = Object.keys(groups).sort(function (a, b) {
                return parseMonthKey(a) - parseMonthKey(b);
            });

            // =========================
            // YTD KEYS
            // =========================
            var ytdKeyC = "__YTD_C__";
            var ytdKeyA = "__YTD_A__";

            var monthSummary = {};
            var ytdTotalC = 0;
            var ytdTotalA = 0;

            // =========================
            // CALCULATIONS
            // =========================
            dataArray.forEach(function (row) {

                var rowYTD_C = 0;
                var rowYTD_A = 0;

                monthKeys.forEach(function (m) {

                    var cKey = groups[m].LoanCount;
                    var aKey = groups[m].Amount;

                    var c = Number(row[cKey] || 0);
                    var a = Number(row[aKey] || 0);

                    rowYTD_C += c;
                    rowYTD_A += a;

                    if (!monthSummary[m]) {
                        monthSummary[m] = { count: 0, amount: 0 };
                    }

                    monthSummary[m].count += c;
                    monthSummary[m].amount += a;
                });

                row[ytdKeyC] = rowYTD_C;
                row[ytdKeyA] = rowYTD_A;

                ytdTotalC += rowYTD_C;
                ytdTotalA += rowYTD_A;
            });

            Object.keys(monthSummary).forEach(function (m) {
                var c = monthSummary[m].count;
                var a = monthSummary[m].amount;
                monthSummary[m].rate = c ? (a / c) : 0;
            });

            var ytdRate = ytdTotalC ? (ytdTotalA / ytdTotalC) : 0;

            // =========================
            // DESTROY OLD TABLE
            // =========================
            if ($.fn.DataTable.isDataTable('#dashboard_arProcess')) {
                $('#dashboard_arProcess').DataTable().destroy();
            }

            // =========================
            // HEADER BUILD
            // =========================
            var header1 = "<tr>";
            var header2 = "<tr>";

            header1 += "<th rowspan='2'>Process</th>";
            header1 += "<th colspan='3' style='background:#d9edf7;'>YTD</th>";

            header2 += "<th>Count<br><b>" + formatNumber(ytdTotalC, 0) + "</b></th>";
            header2 += "<th>Amount<br><b>" + formatNumber(ytdTotalA, 2) + "</b></th>";
            header2 += "<th>Rate<br><b>" + formatNumber(ytdRate, 2) + "</b></th>";

            monthKeys.forEach(function (m) {

                header1 += "<th colspan='3' style='background:#f9f9f9;'>" + m + "</th>";

                header2 += "<th>Count<br><b>" + formatNumber(monthSummary[m].count, 0) + "</b></th>";
                header2 += "<th>Amount<br><b>" + formatNumber(monthSummary[m].amount, 2) + "</b></th>";
                header2 += "<th>Rate<br><b>" + formatNumber(monthSummary[m].rate, 2) + "</b></th>";
            });

            header1 += "</tr>";
            header2 += "</tr>";

            $('#dashboard_arProcess').html(
                "<thead>" + header1 + header2 + "</thead><tbody></tbody>"
            );

            // =========================
            // COLUMNS
            // =========================
            var columns = [];

            columns.push({
                data: "Process",
                className: "dt-left"
            });

            // YTD
            columns.push({
                data: ytdKeyC,
                className: "dt-right",
                render: d => formatNumber(d, 0)
            });

            columns.push({
                data: ytdKeyA,
                className: "dt-right",
                render: d => formatNumber(d, 2)
            });

            columns.push({
                data: null,
                className: "dt-right",
                render: function (d, t, r) {
                    var c = r[ytdKeyC];
                    var a = r[ytdKeyA];
                    return c ? formatNumber(a / c, 2) : "0.00";
                }
            });

            // MONTHS
            monthKeys.forEach(function (m) {

                columns.push({
                    data: groups[m].LoanCount,
                    className: "dt-right",
                    render: d => formatNumber(d, 0)
                });

                columns.push({
                    data: groups[m].Amount,
                    className: "dt-right",
                    render: d => formatNumber(d, 2)
                });

                columns.push({
                    data: null,
                    className: "dt-right",
                    render: function (d, t, r) {
                        var c = r[groups[m].LoanCount];
                        var a = r[groups[m].Amount];
                        return c ? formatNumber(a / c, 2) : "0.00";
                    }
                });
            });

            // =========================
            // INIT DATATABLE
            // =========================
            $('#dashboard_arProcess').DataTable({

                data: dataArray,
                columns: columns,

                dom: 't',
                paging: false,
                searching: false,
                ordering: false,

                scrollX: true,
                scrollY: '400px',

                fixedHeader: true,
                fixedColumns: { leftColumns: 1 },

                autoWidth: false,

                createdRow: function (row) {
                    $(row).css("font-size", "13px");
                },

                initComplete: function () {
                    $('#load1').hide();
                }
            });
        },

        error: function (err) {
            $('#load1').hide();
            CanopyUI.debug(err);
        }
    });

    return false;
}

function dashboard_arBind_displayERP_New() {

    $('#load1').show();

    $.ajax({
        url: "DashboardNew.aspx/GetClientwiseDashboardNew",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",

        success: function (data) {

            var dataArray = JSON.parse(data.d);

            if (!dataArray || dataArray.length === 0) {
                $('#load1').hide();
                alert("No data found");
                return;
            }

            var firstRow = dataArray[0];

            // =========================
            // FORMAT NUMBER (ERP + INDIA STYLE)
            // =========================
            function formatNumber(value, decimals) {
                value = Number(value || 0);

                var formatted = value.toLocaleString('en-IN', {
                    minimumFractionDigits: decimals,
                    maximumFractionDigits: decimals
                });

                return value < 0 ? '(' + formatted + ')' : formatted;
            }

            // =========================
            // MONTH SORT
            // =========================
            function parseMonthKey(m) {
                var parts = m.split("-");

                var monthNames = {
                    "Jan": 0, "Feb": 1, "Mar": 2, "Apr": 3,
                    "May": 4, "Jun": 5, "Jul": 6, "Aug": 7,
                    "Sep": 8, "Oct": 9, "Nov": 10, "Dec": 11
                };

                return new Date(
                    parseInt(parts[1] || "0"),
                    monthNames[parts[0]] || 0,
                    1
                );
            }

            // =========================
            // GROUP MONTH COLUMNS
            // =========================
            var groups = {};

            Object.keys(firstRow).forEach(function (key) {

                if (key === "ResultID" || key === "Process") return;

                var parts = key.split("-ERP-");
                if (parts.length < 2) return;

                var month = parts[0];
                var type = parts[1];

                if (!groups[month]) {
                    groups[month] = { LoanCount: null, Amount: null };
                }

                if (type === "LoanCount") groups[month].LoanCount = key;
                if (type === "Amount") groups[month].Amount = key;
            });

            //var monthKeys = Object.keys(groups).sort(function (a, b) {
            //    return parseMonthKey(a) - parseMonthKey(b);
            //});

            // =========================
            // MONTH SORT (LATEST FIRST)
            // =========================
            var monthOrder = {
                Jan: 1,
                Feb: 2,
                Mar: 3,
                Apr: 4,
                May: 5,
                Jun: 6,
                Jul: 7,
                Aug: 8,
                Sep: 9,
                Oct: 10,
                Nov: 11,
                Dec: 12
            };

            var monthKeys = Object.keys(groups).sort(function (a, b) {

                var pa = a.trim().split('-');
                var pb = b.trim().split('-');

                var monthA = pa[0];
                var yearA = parseInt(pa[1], 10);

                var monthB = pb[0];
                var yearB = parseInt(pb[1], 10);

                // Sort by year descending
                if (yearA !== yearB) {
                    return yearB - yearA;
                }

                // Sort by month descending
                return monthOrder[monthB] - monthOrder[monthA];
            });

            CanopyUI.debug("Sorted Months:", monthKeys);

            // =========================
            // YTD KEYS
            // =========================
            var ytdKeyC = "__YTD_C__";
            var ytdKeyA = "__YTD_A__";

            var monthSummary = {};
            var ytdTotalC = 0;
            var ytdTotalA = 0;

            // =========================
            // CALCULATE TOTALS
            // =========================
            dataArray.forEach(function (row) {

                var rowC = 0;
                var rowA = 0;

                monthKeys.forEach(function (m) {

                    var cKey = groups[m].LoanCount;
                    var aKey = groups[m].Amount;

                    var c = Number(row[cKey] || 0);
                    var a = Number(row[aKey] || 0);

                    rowC += c;
                    rowA += a;

                    if (!monthSummary[m]) {
                        monthSummary[m] = { count: 0, amount: 0 };
                    }

                    monthSummary[m].count += c;
                    monthSummary[m].amount += a;
                });

                row[ytdKeyC] = rowC;
                row[ytdKeyA] = rowA;

                ytdTotalC += rowC;
                ytdTotalA += rowA;
            });

            Object.keys(monthSummary).forEach(function (m) {
                var c = monthSummary[m].count;
                var a = monthSummary[m].amount;
                monthSummary[m].rate = c ? (a / c) : 0;
            });

            var ytdRate = ytdTotalC ? (ytdTotalA / ytdTotalC) : 0;

            // =========================
            // DESTROY OLD TABLE
            // =========================
            if ($.fn.DataTable.isDataTable('#dashboard_arProcess')) {
                $('#dashboard_arProcess').DataTable().destroy();
            }

            // =========================
            // KPI CARDS (TOP SUMMARY)
            // =========================
            var kpiHtml = `
                <div style="display:flex; gap:15px; margin-bottom:10px; font-family:Segoe UI;">

                    <div style="flex:1; padding:12px; background:#2f5597; color:#fff; border-radius:6px;">
                        <b>Total YTD Count</b><br>
                        <span style="font-size:16px;">${formatNumber(ytdTotalC, 0)}</span>
                    </div>

                    <div style="flex:1; padding:12px; background:#1f4e79; color:#fff; border-radius:6px;">
                        <b>Total YTD Amount</b><br>
                        <span style="font-size:16px;">${formatNumber(ytdTotalA, 2)}</span>
                    </div>

                    <div style="flex:1; padding:12px; background:#385723; color:#fff; border-radius:6px;">
                        <b>YTD Rate</b><br>
                        <span style="font-size:16px;">${formatNumber(ytdRate, 2)}</span>
                    </div>

                </div>
            `;

            $("#kpiContainer").html(kpiHtml);

            // =========================
            // HEADER BUILD
            // =========================
            var header1 = "<tr>";
            var header2 = "<tr>";

            header1 += "<th rowspan='2'>Process</th>";
            header1 += "<th colspan='3' style='text-align:center;'>YTD</th>";

            header2 += "<th style='text-align:center'>Count<br><b>" + formatNumber(ytdTotalC, 0) + "</b></th>";
            header2 += "<th style='text-align:center'>Amount<br><b>" + formatNumber(ytdTotalA, 2) + "</b></th>";
            /*header2 += "<th>Rate<br><b>" + formatNumber(ytdRate, 2) + "</b></th>";*/
            header2 += "<th style='text-align:center'>Rate</th>";

            monthKeys.forEach(function (m) {

                header1 += "<th colspan='3'>" + m + "</th>";

                header2 += "<th style='text-align:center'>Count<br><b>" + formatNumber(monthSummary[m].count, 0) + "</b></th>";
                header2 += "<th style='text-align:center'>Amount<br><b>" + formatNumber(monthSummary[m].amount, 2) + "</b></th>";
                /*header2 += "<th>Rate<br><b>" + formatNumber(monthSummary[m].rate, 2) + "</b></th>";*/
                header2 += "<th style='text-align:center'>Rate</th>";
            });

            header1 += "</tr>";
            header2 += "</tr>";

            $('#dashboard_arProcess').html(
                "<thead>" + header1 + header2 + "</thead><tbody></tbody>"
            );

            // =========================
            // COLUMNS
            // =========================
            var columns = [];

            columns.push({
                data: "Process",
                className: "dt-left"
            });

            columns.push({
                data: ytdKeyC,
                className: "dt-right",
                render: d => formatNumber(d, 0)
            });

            columns.push({
                data: ytdKeyA,
                className: "dt-right",
                render: d => formatNumber(d, 2)
            });

            columns.push({
                data: null,
                className: "dt-right",
                render: function (d, t, r) {
                    var c = r[ytdKeyC];
                    var a = r[ytdKeyA];
                    var rate = c ? (a / c) : 0;

                    var formatted = formatNumber(rate, 2);

                    return rate >= 1
                        ? "<span>" + formatted + "</span>"
                        : "<span>" + formatted + "</span>";
                }
            });

            // MONTHS
            monthKeys.forEach(function (m) {

                columns.push({
                    data: groups[m].LoanCount,
                    className: "dt-right",
                    render: d => formatNumber(d, 0)
                });

                columns.push({
                    data: groups[m].Amount,
                    className: "dt-right",
                    render: d => formatNumber(d, 2)
                });

                columns.push({
                    data: null,
                    className: "dt-right",
                    render: function (d, t, r) {
                        var c = r[groups[m].LoanCount];
                        var a = r[groups[m].Amount];
                        var rate = c ? (a / c) : 0;

                        return formatNumber(rate, 2);
                    }
                });
            });

            // =========================
            // INIT DATATABLE
            // =========================
            $('#dashboard_arProcess').DataTable({

                data: dataArray,
                columns: columns,

                dom: 't',

            

                paging: false,
                searching: false,
                ordering: false,

                scrollX: true,
                scrollY: '450px',

                fixedHeader: true,
                fixedColumns: { leftColumns: 1 },

                autoWidth: false,

                createdRow: function (row) {
                    $(row).css({
                        "font-size": "11px",
                        "line-height": "1.4"
                    });
                },

               
               

                initComplete: function () {
                    $('#load1').hide();
                }
             
                

              
            });
        },

        error: function (err) {
            $('#load1').hide();
            CanopyUI.debug(err);
        }
    });

    return false;
}





function dashboard_arBind_displayERP_New_Client_Working() {

    $('#load1').show();

    $.ajax({
        url: "DashboardNew.aspx/GetClientwiseDashboardNew_Client",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",

        success: function (data) {

            var dataArray = JSON.parse(data.d);

            if (!dataArray || dataArray.length === 0) {
                $('#load1').hide();
                alert("No data found");
                return;
            }

            var firstRow = dataArray[0];

            var groups = {};
            var totals = {};

            // =========================
            // GROUP MONTH COLUMNS
            // =========================
            Object.keys(firstRow).forEach(function (key) {

                if (
                    key === "ResultID" ||
                    key === "Client" ||
                    key === "Process"
                ) return;

                var parts = key.split("-ERP-");
                if (parts.length < 2) return;

                var month = parts[0];
                var type = parts[1];

                if (!groups[month]) {
                    groups[month] = {
                        LoanCount: "",
                        Amount: ""
                    };
                }

                if (type === "LoanCount") groups[month].LoanCount = key;
                if (type === "Amount") groups[month].Amount = key;
            });

            // =========================
            // CALCULATE TOTALS
            // =========================
            Object.keys(groups).forEach(function (month) {
                totals[groups[month].LoanCount] = 0;
                totals[groups[month].Amount] = 0;
            });

            dataArray.forEach(function (row) {

                Object.keys(groups).forEach(function (month) {

                    var cKey = groups[month].LoanCount;
                    var aKey = groups[month].Amount;

                    totals[cKey] += parseFloat(row[cKey] || 0);
                    totals[aKey] += parseFloat(row[aKey] || 0);
                });
            });

            // =========================
            // HEADER (2 ROW FIXED)
            // =========================
            var header1 = "<tr>";
            var header2 = "<tr>";

            header1 += "<th style='min-width:150px'>Client</th>";
            header1 += "<th style='min-width:150px'>Process</th>";

            header2 += "<th>Client</th>";
            header2 += "<th>Process</th>";

            Object.keys(groups).forEach(function (month) {

                header1 += "<th colspan='2' style='text-align:center; min-width:240px;'>" +
                    month +
                    "</th>";

                header2 += "<th style='min-width:100px'>Count</th>";
                header2 += "<th style='min-width:140px'>US $</th>";
            });

            header1 += "</tr>";
            header2 += "</tr>";

            var theadHtml = header1 + header2;

            // =========================
            // FOOTER (FIXED ALIGNMENT)
            // =========================
            var footer = "<tr>";

            footer += "<th style='min-width:150px'>Grand Total</th>";
            footer += "<th style='min-width:150px'></th>";

            Object.keys(groups).forEach(function (month) {

                footer += "<th style='min-width:100px; text-align:center;'>" +
                    (totals[groups[month].LoanCount] || 0).toLocaleString() +
                    "</th>";

                footer += "<th style='min-width:140px; text-align:center;'>" +
                    (totals[groups[month].Amount] || 0).toLocaleString() +
                    "</th>";
            });

            footer += "</tr>";

            // =========================
            // RESET TABLE
            // =========================
            if ($.fn.DataTable.isDataTable('#dashboard_arClient')) {
                $('#dashboard_arClient').DataTable().destroy();
            }

            $('#dashboard_arClient').empty();

            $('#dashboard_arClient').html(
                "<thead>" + theadHtml + "</thead>" +
                "<tbody></tbody>" +
                "<tfoot>" + footer + "</tfoot>"
            );

            // =========================
            // COLUMNS
            // =========================
            var columns = [];

            columns.push({
                data: "Client",
                width: "150px",
                defaultContent: ""
            });

            columns.push({
                data: "Process",
                width: "150px",
                defaultContent: ""
            });

            Object.keys(groups).forEach(function (month) {

                columns.push({
                    data: groups[month].LoanCount,
                    width: "100px",
                    className: "dt-center",
                    defaultContent: 0,
                    render: function (data) {
                        return parseFloat(data || 0).toLocaleString();
                    }
                });

                columns.push({
                    data: groups[month].Amount,
                    width: "140px",
                    className: "dt-center",
                    defaultContent: 0,
                    render: function (data) {
                        return parseFloat(data || 0).toLocaleString();
                    }
                });
            });

            // =========================
            // DATATABLE INIT
            // =========================
            var table = $('#dashboard_arClient').DataTable({

                data: dataArray,
                columns: columns,

                dom: 't',

                paging: false,
                searching: false,
                ordering: false,
                info: false,

                scrollX: true,
                scrollY: "400px",
                scrollCollapse: true,

                autoWidth: false,
                fixedHeader: false,

                fixedColumns: {
                    leftColumns: 2
                },

                initComplete: function () {

                    var api = this.api();

                    setTimeout(function () {

                        api.columns.adjust();

                        try {
                            api.fixedColumns().relayout();
                        } catch (e) { }

                        $('#load1').hide();

                    }, 300);
                }
            });

            // FINAL ALIGNMENT FIX
            setTimeout(function () {

                table.columns.adjust();

                try {
                    table.fixedColumns().relayout();
                } catch (e) { }

            }, 500);
        },

        error: function (err) {
            $('#load1').hide();
            CanopyUI.debug(err);
        }
    });

    return false;
}
function dashboard_arBind_displayERP_New_Client_1() {

    $('#load1').show();

    $.ajax({
        url: "DashboardNew.aspx/GetClientwiseDashboardNew_Client",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",

        success: function (data) {

            var dataArray = JSON.parse(data.d);

            if (!dataArray || dataArray.length === 0) {
                $('#load1').hide();
                alert("No data found");
                return;
            }

            var firstRow = dataArray[0];

            var groups = {};

            // =========================
            // GROUP MONTH COLUMNS
            // =========================
            Object.keys(firstRow).forEach(function (key) {

                if (key === "ResultID" || key === "Client" || key === "Process") return;

                var parts = key.split("-ERP-");
                if (parts.length < 2) return;

                var month = parts[0];
                var type = parts[1];

                if (!groups[month]) {
                    groups[month] = { LoanCount: "", Amount: "" };
                }

                if (type === "LoanCount") groups[month].LoanCount = key;
                if (type === "Amount") groups[month].Amount = key;
            });

            var monthKeys = Object.keys(groups);

            // =========================
            // YTD CALCULATION
            // =========================
            var ytdKeyC = "__YTD_C__";
            var ytdKeyA = "__YTD_A__";

            var ytdTotalC = 0;
            var ytdTotalA = 0;

            dataArray.forEach(function (row) {

                var rowC = 0;
                var rowA = 0;

                monthKeys.forEach(function (m) {

                    var c = Number(row[groups[m].LoanCount] || 0);
                    var a = Number(row[groups[m].Amount] || 0);

                    rowC += c;
                    rowA += a;
                });

                row[ytdKeyC] = rowC;
                row[ytdKeyA] = rowA;

                ytdTotalC += rowC;
                ytdTotalA += rowA;
            });

            // =========================
            // HEADER
            // =========================
            var header1 = "<tr>";
            var header2 = "<tr>";

            header1 += "<th rowspan='2' style='min-width:150px;text-align:center'>Client</th>";
            header1 += "<th rowspan='2' style='min-width:150px;text-align:center'>Process</th>";

            header1 += "<th colspan='3' style='background:#2f5597;color:#fff;text-align:center;font-weight:600;'>YTD Performance</th>";

            header2 += "<th style='text-align:center'>Count<br><b>" + ytdTotalC.toLocaleString() + "</b></th>";
            header2 += "<th style='text-align:center'>Amount<br><b>" + ytdTotalA.toLocaleString() + "</b></th>";
            header2 += "<th style='text-align:center'>Rate</th>";

            monthKeys.forEach(function (m) {

                header1 += "<th colspan='3' style='background:#e9edf5;color:#000;text-align:center;font-weight:600;'>" + m + "</th>";

                header2 += "<th style='text-align:center'>Count</th>";
                header2 += "<th style='text-align:center'>Amount</th>";
                header2 += "<th style='text-align:center'>Rate</th>";
            });

            header1 += "</tr>";
            header2 += "</tr>";

            var theadHtml = header1 + header2;

            // =========================
            // FOOTER (NO RATE TOTALS)
            // =========================
            var footer = "<tr>";

            footer += "<th>Grand Total</th>";
            footer += "<th></th>";

            footer += "<th style='text-align:center'>" + ytdTotalC.toLocaleString() + "</th>";
            footer += "<th style='text-align:center'>" + ytdTotalA.toLocaleString() + "</th>";
            footer += "<th></th>";

            monthKeys.forEach(function (m) {
                var cKey = groups[m].LoanCount;
                var aKey = groups[m].Amount;

                var cSum = 0;
                var aSum = 0;

                dataArray.forEach(function (row) {
                    cSum += Number(row[cKey] || 0);
                    aSum += Number(row[aKey] || 0);
                });

                footer += "<th style='text-align:center'>" + cSum.toLocaleString() + "</th>";
                footer += "<th style='text-align:center'>" + aSum.toLocaleString() + "</th>";
                footer += "<th></th>";
            });

            footer += "</tr>";

            // =========================
            // TABLE RESET
            // =========================
            if ($.fn.DataTable.isDataTable('#dashboard_arClient')) {
                $('#dashboard_arClient').DataTable().destroy();
            }

            $('#dashboard_arClient').empty().html(
                "<thead>" + theadHtml + "</thead><tbody></tbody><tfoot>" + footer + "</tfoot>"
            );

            // =========================
            // COLUMNS
            // =========================
            var columns = [];

            columns.push({ data: "Client", width: "150px" });
            columns.push({ data: "Process", width: "150px" });

            // YTD
            columns.push({
                data: ytdKeyC,
                className: "dt-center",
                render: d => Number(d || 0).toLocaleString()
            });

            columns.push({
                data: ytdKeyA,
                className: "dt-center",
                render: d => Number(d || 0).toLocaleString()
            });

            columns.push({
                data: null,
                className: "dt-center",
                render: function (d, t, r) {
                    var c = Number(r[ytdKeyC] || 0);
                    var a = Number(r[ytdKeyA] || 0);
                    return c ? (a / c).toFixed(2) : "0.00";
                }
            });

            // MONTHS
            monthKeys.forEach(function (m) {

                columns.push({
                    data: groups[m].LoanCount,
                    className: "dt-center",
                    render: d => Number(d || 0).toLocaleString()
                });

                columns.push({
                    data: groups[m].Amount,
                    className: "dt-center",
                    render: d => Number(d || 0).toLocaleString()
                });

                columns.push({
                    data: null,
                    className: "dt-center",
                    render: function (d, t, r) {
                        var c = Number(r[groups[m].LoanCount] || 0);
                        var a = Number(r[groups[m].Amount] || 0);
                        return c ? (a / c).toFixed(2) : "0.00";
                    }
                });
            });

            // =========================
            // INIT DATATABLE
            // =========================
            var table = $('#dashboard_arClient').DataTable({

                data: dataArray,
                columns: columns,

                dom: 't',
                paging: false,
                searching: false,
                ordering: false,
                info: false,

                scrollX: true,
                scrollY: "400px",
                scrollCollapse: true,

                autoWidth: false,

                fixedColumns: {
                    leftColumns: 2
                },

                initComplete: function () {
                    var api = this.api();

                    setTimeout(function () {
                        api.columns.adjust();
                        try { api.fixedColumns().relayout(); } catch (e) { }
                        $('#load1').hide();
                    }, 300);
                }
            });

        },

        error: function (err) {
            $('#load1').hide();
            CanopyUI.debug(err);
        }
    });

    return false;
}

function dashboard_arBind_displayERP_New_Client() {

    $('#load1').show();

    $.ajax({
        url: "DashboardNew.aspx/GetClientwiseDashboardNew_Client",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",

        success: function (data) {

            var dataArray = JSON.parse(data.d);

            if (!dataArray || dataArray.length === 0) {
                $('#load1').hide();
                alert("No data found");
                return;
            }

            var firstRow = dataArray[0];

            var groups = {};

            // =========================
            // GROUP MONTH COLUMNS
            // =========================
            Object.keys(firstRow).forEach(function (key) {

                if (key === "ResultID" || key === "Client" || key === "Process") return;

                var parts = key.split("-ERP-");
                if (parts.length < 2) return;

                var month = parts[0];
                var type = parts[1];

                if (!groups[month]) {
                    groups[month] = { LoanCount: "", Amount: "" };
                }

                if (type === "LoanCount") groups[month].LoanCount = key;
                if (type === "Amount") groups[month].Amount = key;
            });

            var monthKeys = Object.keys(groups);

            // =========================
            // TOTAL CALCULATION (YTD + MONTH)
            // =========================
            var monthTotals = {};
            var ytdCount = 0;
            var ytdAmount = 0;

            monthKeys.forEach(function (m) {
                monthTotals[m] = { count: 0, amount: 0 };
            });

            dataArray.forEach(function (row) {

                var rowC = 0;
                var rowA = 0;

                monthKeys.forEach(function (m) {

                    var c = Number(row[groups[m].LoanCount] || 0);
                    var a = Number(row[groups[m].Amount] || 0);

                    monthTotals[m].count += c;
                    monthTotals[m].amount += a;

                    rowC += c;
                    rowA += a;
                });

                ytdCount += rowC;
                ytdAmount += rowA;

                row.__YTD_C__ = rowC;
                row.__YTD_A__ = rowA;
            });

            // =========================
            // HEADER BUILD
            // =========================
            var header1 = "<tr>";
            var header2 = "<tr>";

            header1 += "<th rowspan='2' style='min-width:125px;text-align:center'>Client</th>";
            header1 += "<th rowspan='2' style='min-width:125px;text-align:center'>Process</th>";

            // ================= YTD =================
            header1 += "<th colspan='3' style='text-align:center;'>YTD</th>";

            header2 += "<th style='text-align:center'>Count<br><b>" + ytdCount.toLocaleString() + "</b></th>";
            header2 += "<th style='text-align:center'>US $<br><b>" + ytdAmount.toLocaleString() + "</b></th>";
            header2 += "<th style='text-align:center'>Rate</th>";

            // ================= MONTHS =================
            monthKeys.forEach(function (m) {

                var c = monthTotals[m].count;
                var a = monthTotals[m].amount;

                var rate = c ? (a / c) : 0;

                header1 += "<th colspan='3' style='text-align:center;'>" + m + "</th>";

                header2 += "<th style='text-align:center'>Count<br><b>" + c.toLocaleString() + "</b></th>";
                header2 += "<th style='text-align:center'>US $<br><b>" + a.toLocaleString() + "</b></th>";
                /*header2 += "<th style='text-align:center'>Rate<br><b>" + rate.toFixed(2) + "</b></th>";*/
                header2 += "<th style='text-align:center'>Rate</th>";
            });

            header1 += "</tr>";
            header2 += "</tr>";

            var theadHtml = header1 + header2;

            // =========================
            // TABLE RESET (NO FOOTER)
            // =========================
            if ($.fn.DataTable.isDataTable('#dashboard_arClient')) {
                $('#dashboard_arClient').DataTable().destroy();
            }

            $('#dashboard_arClient').empty();

            $('#dashboard_arClient').html(
                "<thead>" + theadHtml + "</thead><tbody></tbody>"
            );

            // =========================
            // COLUMNS
            // =========================
            var columns = [];

            columns.push({ data: "Client", width: "125px" });
            columns.push({ data: "Process", width: "125px" });

            // YTD
            columns.push({
                data: "__YTD_C__",
                className: "dt-center",
                render: d => Number(d || 0).toLocaleString()
            });

            columns.push({
                data: "__YTD_A__",
                className: "dt-center",
                render: d => Number(d || 0).toLocaleString()
            });

            columns.push({
                data: null,
                className: "dt-center",
                render: function (d, t, r) {
                    var c = Number(r.__YTD_C__ || 0);
                    var a = Number(r.__YTD_A__ || 0);
                    return c ? (a / c).toFixed(2) : "0.00";
                }
            });

            // MONTHS
            monthKeys.forEach(function (m) {

                columns.push({
                    data: groups[m].LoanCount,
                    className: "dt-center",
                    render: d => Number(d || 0).toLocaleString()
                });

                columns.push({
                    data: groups[m].Amount,
                    className: "dt-center",
                    render: d => Number(d || 0).toLocaleString()
                });

                columns.push({
                    data: null,
                    className: "dt-center",
                    render: function (d, t, r) {
                        var c = Number(r[groups[m].LoanCount] || 0);
                        var a = Number(r[groups[m].Amount] || 0);
                        return c ? (a / c).toFixed(2) : "0.00";
                    }
                });
            });

            // =========================
            // DATATABLE INIT
            // =========================
            var table = $('#dashboard_arClient').DataTable({

                data: dataArray,
                columns: columns,

                dom: 't',
                paging: false,
                searching: false,
                ordering: false,
                info: false,

                scrollX: true,
                scrollY: '450px',
                scrollCollapse: true,

                autoWidth: false,

                fixedHeader: true,
                fixedColumns: { leftColumns: 2 },

                createdRow: function (row) {
                    $(row).css({
                        "font-size": "11px",
                        "line-height": "1.4"
                    });
                },



                initComplete: function () {
                    $('#load1').hide();
                }
            });

            table.columns.adjust().draw();

            setTimeout(function () {
                table.columns.adjust().draw();
            }, 500);

        },



        error: function (err) {
            $('#load1').hide();
            CanopyUI.debug(err);
        }
    });

    return false;
}


function dashboard_arBind_displayERP_New_Client_RL() {

    $('#load1').show();

    $.ajax({
        url: "DashboardNew.aspx/GetClientwiseDashboardNew_Client_RL",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",

        success: function (data) {

            var dataArray = JSON.parse(data.d);

            if (!dataArray || dataArray.length === 0) {
                $('#load1').hide();
                alert("No data found");
                return;
            }

            var firstRow = dataArray[0];

            var groups = {};

            // =========================
            // GROUP MONTH COLUMNS
            // =========================
            Object.keys(firstRow).forEach(function (key) {

                if (key === "ResultID" || key === "Client" || key === "Process") return;

                var parts = key.split("-ERP-");
                if (parts.length < 2) return;

                var month = parts[0];
                var type = parts[1];

                if (!groups[month]) {
                    groups[month] = { LoanCount: "", Amount: "" };
                }

                if (type === "LoanCount") groups[month].LoanCount = key;
                if (type === "Amount") groups[month].Amount = key;
            });

            var monthKeys = Object.keys(groups);

            // =========================
            // TOTAL CALCULATION (YTD + MONTH)
            // =========================
            var monthTotals = {};
            var ytdCount = 0;
            var ytdAmount = 0;

            monthKeys.forEach(function (m) {
                monthTotals[m] = { count: 0, amount: 0 };
            });

            dataArray.forEach(function (row) {

                var rowC = 0;
                var rowA = 0;

                monthKeys.forEach(function (m) {

                    var c = Number(row[groups[m].LoanCount] || 0);
                    var a = Number(row[groups[m].Amount] || 0);

                    monthTotals[m].count += c;
                    monthTotals[m].amount += a;

                    rowC += c;
                    rowA += a;
                });

                ytdCount += rowC;
                ytdAmount += rowA;

                row.__YTD_C__ = rowC;
                row.__YTD_A__ = rowA;
            });

            // =========================
            // HEADER BUILD
            // =========================
            var header1 = "<tr>";
            var header2 = "<tr>";

            header1 += "<th rowspan='2' style='min-width:155px;text-align:center'>Client</th>";
            header1 += "<th rowspan='2' style='min-width:155px;text-align:center'>Process</th>";

            // ================= YTD =================
            header1 += "<th colspan='3' style='text-align:center;'>YTD</th>";

            header2 += "<th style='text-align:center'>Count<br><b>" + ytdCount.toLocaleString() + "</b></th>";
            header2 += "<th style='text-align:center'>US $<br><b>" + ytdAmount.toLocaleString() + "</b></th>";
            header2 += "<th style='text-align:center'>Rate</th>";

            // ================= MONTHS =================
            monthKeys.forEach(function (m) {

                var c = monthTotals[m].count;
                var a = monthTotals[m].amount;

                var rate = c ? (a / c) : 0;

                header1 += "<th colspan='3' style='text-align:center;'>" + m + "</th>";

                header2 += "<th style='text-align:center'>Count<br><b>" + c.toLocaleString() + "</b></th>";
                header2 += "<th style='text-align:center'>US $<br><b>" + a.toLocaleString() + "</b></th>";
                /*header2 += "<th style='text-align:center'>Rate<br><b>" + rate.toFixed(2) + "</b></th>";*/
                header2 += "<th style='text-align:center'>Rate</th>";
            });

            header1 += "</tr>";
            header2 += "</tr>";

            var theadHtml = header1 + header2;

            // =========================
            // TABLE RESET (NO FOOTER)
            // =========================
            if ($.fn.DataTable.isDataTable('#dashboard_arRL')) {
                $('#dashboard_arRL').DataTable().destroy();
            }

            $('#dashboard_arRL').empty();

            $('#dashboard_arRL').html(
                "<thead>" + theadHtml + "</thead><tbody></tbody>"
            );

            // =========================
            // COLUMNS
            // =========================
            var columns = [];

            columns.push({ data: "Client", width: "155px" });
            columns.push({ data: "Process", width: "155px" });

            // YTD
            columns.push({
                data: "__YTD_C__",
                className: "dt-center",
                render: d => Number(d || 0).toLocaleString()
            });

            columns.push({
                data: "__YTD_A__",
                className: "dt-center",
                render: d => Number(d || 0).toLocaleString()
            });

            columns.push({
                data: null,
                className: "dt-center",
                render: function (d, t, r) {
                    var c = Number(r.__YTD_C__ || 0);
                    var a = Number(r.__YTD_A__ || 0);
                    return c ? (a / c).toFixed(2) : "0.00";
                }
            });

            // MONTHS
            monthKeys.forEach(function (m) {

                columns.push({
                    data: groups[m].LoanCount,
                    className: "dt-center",
                    render: d => Number(d || 0).toLocaleString()
                });

                columns.push({
                    data: groups[m].Amount,
                    className: "dt-center",
                    render: d => Number(d || 0).toLocaleString()
                });

                columns.push({
                    data: null,
                    className: "dt-center",
                    render: function (d, t, r) {
                        var c = Number(r[groups[m].LoanCount] || 0);
                        var a = Number(r[groups[m].Amount] || 0);
                        return c ? (a / c).toFixed(2) : "0.00";
                    }
                });
            });

            // =========================
            // DATATABLE INIT
            // =========================
            var table = $('#dashboard_arRL').DataTable({

                data: dataArray,
                columns: columns,

                dom: 't',
                paging: false,
                searching: false,
                ordering: false,
                info: false,

                scrollX: true,
                scrollY: "400px",
                scrollCollapse: true,

                autoWidth: false,

                fixedHeader: true,
                fixedColumns: { leftColumns: 1 },

                createdRow: function (row) {
                    $(row).css({
                        "font-size": "11px",
                        "line-height": "1.4"
                    });
                },

                initComplete: function () {
                    $('#load1').hide();
                }
            });

        },

        error: function (err) {
            $('#load1').hide();
            CanopyUI.debug(err);
        }
    });

    return false;
}

function dashboard_arBind_displayERP_New_Client_SS() {

    $('#load1').show();

    $.ajax({
        url: "DashboardNew.aspx/GetClientwiseDashboardNew_Client_SS",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",

        success: function (data) {

            var dataArray = JSON.parse(data.d);

            if (!dataArray || dataArray.length === 0) {
                $('#load1').hide();
                alert("No data found");
                return;
            }

            var firstRow = dataArray[0];

            var groups = {};

            // =========================
            // GROUP MONTH COLUMNS
            // =========================
            Object.keys(firstRow).forEach(function (key) {

                if (key === "ResultID" || key === "Client" || key === "Process") return;

                var parts = key.split("-ERP-");
                if (parts.length < 2) return;

                var month = parts[0];
                var type = parts[1];

                if (!groups[month]) {
                    groups[month] = { LoanCount: "", Amount: "" };
                }

                if (type === "LoanCount") groups[month].LoanCount = key;
                if (type === "Amount") groups[month].Amount = key;
            });

            var monthKeys = Object.keys(groups);

            // =========================
            // TOTAL CALCULATION (YTD + MONTH)
            // =========================
            var monthTotals = {};
            var ytdCount = 0;
            var ytdAmount = 0;

            monthKeys.forEach(function (m) {
                monthTotals[m] = { count: 0, amount: 0 };
            });

            dataArray.forEach(function (row) {

                var rowC = 0;
                var rowA = 0;

                monthKeys.forEach(function (m) {

                    var c = Number(row[groups[m].LoanCount] || 0);
                    var a = Number(row[groups[m].Amount] || 0);

                    monthTotals[m].count += c;
                    monthTotals[m].amount += a;

                    rowC += c;
                    rowA += a;
                });

                ytdCount += rowC;
                ytdAmount += rowA;

                row.__YTD_C__ = rowC;
                row.__YTD_A__ = rowA;
            });

            // =========================
            // HEADER BUILD
            // =========================
            var header1 = "<tr>";
            var header2 = "<tr>";

            header1 += "<th rowspan='2' style='min-width:185px;text-align:center'>Client</th>";
            header1 += "<th rowspan='2' style='min-width:165px;text-align:center'>Process</th>";

            // ================= YTD =================
            header1 += "<th colspan='3' style='text-align:center;'>YTD</th>";

            header2 += "<th style='text-align:center'>Count<br><b>" + ytdCount.toLocaleString() + "</b></th>";
            header2 += "<th style='text-align:center'>US $<br><b>" + ytdAmount.toLocaleString() + "</b></th>";
            header2 += "<th style='text-align:center'>Rate</th>";

            // ================= MONTHS =================
            monthKeys.forEach(function (m) {

                var c = monthTotals[m].count;
                var a = monthTotals[m].amount;

                var rate = c ? (a / c) : 0;

                header1 += "<th colspan='3' style='text-align:center;'>" + m + "</th>";

                header2 += "<th style='text-align:center'>Count<br><b>" + c.toLocaleString() + "</b></th>";
                header2 += "<th style='text-align:center'>US $<br><b>" + a.toLocaleString() + "</b></th>";
                /*header2 += "<th style='text-align:center'>Rate<br><b>" + rate.toFixed(2) + "</b></th>";*/
                header2 += "<th style='text-align:center'>Rate</th>";
            });

            header1 += "</tr>";
            header2 += "</tr>";

            var theadHtml = header1 + header2;

            // =========================
            // TABLE RESET (NO FOOTER)
            // =========================
            if ($.fn.DataTable.isDataTable('#dashboard_arSS')) {
                $('#dashboard_arSS').DataTable().destroy();
            }

            $('#dashboard_arSS').empty();

            $('#dashboard_arSS').html(
                "<thead>" + theadHtml + "</thead><tbody></tbody>"
            );

            // =========================
            // COLUMNS
            // =========================
            var columns = [];

            columns.push({ data: "Client", width: "185px" });
            columns.push({ data: "Process", width: "165px" });

            // YTD
            columns.push({
                data: "__YTD_C__",
                className: "dt-center",
                render: d => Number(d || 0).toLocaleString()
            });

            columns.push({
                data: "__YTD_A__",
                className: "dt-center",
                render: d => Number(d || 0).toLocaleString()
            });

            columns.push({
                data: null,
                className: "dt-center",
                render: function (d, t, r) {
                    var c = Number(r.__YTD_C__ || 0);
                    var a = Number(r.__YTD_A__ || 0);
                    return c ? (a / c).toFixed(2) : "0.00";
                }
            });

            // MONTHS
            monthKeys.forEach(function (m) {

                columns.push({
                    data: groups[m].LoanCount,
                    className: "dt-center",
                    render: d => Number(d || 0).toLocaleString()
                });

                columns.push({
                    data: groups[m].Amount,
                    className: "dt-center",
                    render: d => Number(d || 0).toLocaleString()
                });

                columns.push({
                    data: null,
                    className: "dt-center",
                    render: function (d, t, r) {
                        var c = Number(r[groups[m].LoanCount] || 0);
                        var a = Number(r[groups[m].Amount] || 0);
                        return c ? (a / c).toFixed(2) : "0.00";
                    }
                });
            });

            // =========================
            // DATATABLE INIT
            // =========================
            var table = $('#dashboard_arSS').DataTable({

                data: dataArray,
                columns: columns,

                dom: 't',
                paging: false,
                searching: false,
                ordering: false,
                info: false,

                scrollX: true,
                scrollY: "400px",
                scrollCollapse: true,

                autoWidth: false,

                fixedHeader: true,
                fixedColumns: { leftColumns: 1 },

                createdRow: function (row) {
                    $(row).css({
                        "font-size": "11px",
                        "line-height": "1.4"
                    });
                },

                initComplete: function () {
                    $('#load1').hide();
                }
            });

        },

        error: function (err) {
            $('#load1').hide();
            CanopyUI.debug(err);
        }
    });

    return false;
}




function exportAllDashboardTabsExcel() {

    //var wb = XLSX.utils.book_new();

    //var sheets = [
    //    { id: "dashboard_arProcess", name: "Summary Process" },
    //    { id: "dashboard_arClient", name: "Summary Client" },
    //    { id: "dashboard_arRL", name: "RL" },
    //    { id: "dashboard_arSS", name: "SS" }
    //];

    //sheets.forEach(function (s) {

    //    var table = document.getElementById(s.id);

    //    if (!table) return;

    //    var ws = XLSX.utils.table_to_sheet(table, {
    //        raw: false
    //    });

    //    XLSX.utils.book_append_sheet(wb, ws, s.name);
    //});

    //XLSX.writeFile(wb, "Dashboard_All_Tabs.xlsx");
}


