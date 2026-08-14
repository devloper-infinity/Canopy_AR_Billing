var error_count = 0;
var billingPeriod;
var billingType;


function impbill_uploadData() {

    billingType = $('#impbill_type').val();
    billingPeriod = $('#impbill_billingPeriod').val().trim();
    var month = $('#impbill_month').val();
    var year = $('#impbill_year').val();
    var fileUpload = $('#impbill_attachment')[0];

    document.getElementById("impbill_tableheader").innerHTML = document.getElementById("impbill_type").options[document.getElementById("impbill_type").selectedIndex].text;

    // Validation
    if (billingType === "") {
        Swal.fire({ icon: 'warning', title: 'Validation', text: 'Please select Billing Type.' });
        $('#impbill_type').focus();
        return false;
    }

    if (month === "0" || month === "") {
        Swal.fire({ icon: 'warning', title: 'Validation', text: 'Please select Month.' });
        $('#impbill_month').focus();
        return false;
    }

    if (year === "" || year === "Select") {
        Swal.fire({ icon: 'warning', title: 'Validation', text: 'Please select Year.' });
        $('#impbill_year').focus();
        return false;
    }

    if (billingPeriod === "") {
        Swal.fire({ icon: 'warning', title: 'Validation', text: 'Please enter Billing Cycle.' });
        $('#impbill_billingPeriod').focus();
        return false;
    }

    if (fileUpload.files.length === 0) {
        Swal.fire({ icon: 'warning', title: 'Validation', text: 'Please upload Excel file.' });
        return false;
    }

    var file = fileUpload.files[0];

    var extension = file.name.split('.').pop().toLowerCase();

    if (extension !== "xlsx") {
        Swal.fire({ icon: 'error', title: 'Invalid File', text: 'Only .xlsx file is allowed.' });
        return false;
    }

    // Filename Validation
    // var fileName = file.name.toLowerCase();
    // var billingTypeText = $("#impbill_type option:selected").text().toLowerCase();

    // if (!fileName.includes(billingType.toLowerCase())) {

    //     Swal.fire({
    //         icon: 'warning',
    //         title: 'Invalid File Name',
    //         text: 'Please select proper file'
    //     });

    //     return false;
    // }

    Swal.fire({
        title: 'Please Wait...', text: 'system is importing data.', allowOutsideClick: false, allowEscapeKey: false,
        didOpen: () => { Swal.showLoading(); }
    });

    // PageMethod Call
    PageMethods.GetExcelDataToBindGrid(billingType,
        function (response) {

            // Check mismatched columns
            if (response === "mismatched") {

                Swal.fire({ icon: 'error', title: 'Column Mismatch', text: 'Please check excel columns.' });
                return;
            }

            // Empty response check
            if (!response || response.length === 0) {

                Swal.fire({ icon: 'warning', title: 'Warning', text: "Error in updating data.", zIndex: 999999 });
                return;
            }

            // Success
            var data = JSON.parse(response);

            Swal.fire({ icon: 'success', title: 'Success', text: "Data imported successfully." }).then(function () {

                processExcelData(data, billingType);
            });

        },
        function (error) {

            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: error.get_message()
            });
        }
    );
}

function impbill_VerifyData() {

    Swal.fire({
        title: 'Please Wait...', text: 'system is verifying and submitting data.', allowOutsideClick: false, allowEscapeKey: false,
        didOpen: () => { Swal.showLoading(); }
    });

    // PageMethod Call
    PageMethods.VerifyAndSubmitData(billingType, billingPeriod,
        function (response) {

            var data = response;

            if (response == 0) {
                Swal.fire({
                    icon: 'warning',
                    title: 'Warning',
                    text: "Error in verifying and submitting data.",
                    zIndex: 999999
                });
            } else {

                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: "Data verified and submited successfully.",
                }).then(function () {

                    location.reload();
                });
            }
        },

        function (error) {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: error.get_message()
            });
        }
    );

    return false;
}

function processExcelData(data, type) {

    error_count = 0;

    var dataArray = data.d || data;

    if (typeof dataArray === "string") {
        dataArray = JSON.parse(dataArray);
    }

    dataArray.forEach(row => {

        row["Error in excel column"] = validateRow(row, type);
    });

    if (error_count > 0) {
        document.getElementById("impbill_verify").disabled = true;

        const alertBox = document.getElementById("impbill_alert");

        alertBox.innerHTML = `
        <div style="background-color: #fff3cd; color: #856404; border: 1px solid #ffeeba; padding: 12px 16px; border-radius: 6px; font-weight: 400;">
            ?? Please clear <b>${error_count}</b> error(s) listed in <b>'Error In Excel'</b> Column before clicking <b>Verify & Submit</b>.<br></div>`;
    }
    else {
        document.getElementById("impbill_verify").disabled = false;
        document.getElementById("impbill_alert").innerText = "";
    }

    if (type === 'Griffin')
        griffin_bindgrid(dataArray);

    if (type === 'StewartAVM')
        stewartAVM_bindgrid(dataArray);

    if (type === 'StewartCDA')
        stewartCDA_bindgrid(dataArray);
}


//---------- bind grid ------------//
function griffin_bindgrid(dataArray) {

    document.getElementById("table_griffin").style.display = "";

    // Destroy old table
    if ($.fn.DataTable.isDataTable('#table_griffin')) {
        $('#table_griffin').DataTable().clear().destroy();
    }

    if (!dataArray || dataArray.length === 0) {
        $('#table_griffin tbody').html('<tr><td colspan="8">No Data Found</td></tr>');
        return;
    }

    $('#table_griffin').DataTable({
        dom: 'ft',
        data: dataArray,
        paging: false,
        processing: true,
        ordering: false,
        fixedHeader: true,
        scrollX: true,
        scrollY: '500px',
        scrollCollapse: true,

        columns: [
            {
                data: 'Error in excel column', title: '<span style="white-space:nowrap;">Error in Excel</span>',
                render: function (data) {
                    if (!data) return '';
                    return `<span style="color:red; font-weight:600;">${data}</span>`;
                }
            },
            { data: null, title: '<span style="white-space:nowrap;">Sr. #</span>', render: function (data, type, row, meta) { return meta.row + 1; } },
            { data: null, title: '<span style="white-space:nowrap;">Billing Period</span>', render: function () { return billingPeriod; }, width: '250px' },

            // ? IMPORTANT: Use EXACT keys with spaces
            { data: 'Loan ID', title: 'Loan ID' },
            { data: 'Name', title: 'Name' },
            { data: 'Submission Date', title: '<span style="white-space:nowrap;">Submission Date</span>' },
            { data: 'User', title: 'User' },
            { data: 'Complete Date', title: 'Completion Date', render: formatMMDDYYYY },
            { data: 'Loan Program (Client Lock)', title: '<span style="white-space:nowrap;">Loan Program (Client Lock)</span>' },
            { data: 'Griffin Status', title: '<span style="white-space:nowrap;">Griffin Status</span>' },
            { data: 'Base Price', title: 'Base Price' },
            { data: 'Bank Statements', title: 'Bank Statements' },
            { data: 'Tax Returns', title: 'Tax Returns' },
            { data: 'Multiple Property Calculation', title: 'Multiple Property Calculation' },
            { data: 'Esclator Adjustment', title: 'Esclator Adjustment' },
            { data: 'Total', title: 'Total' }

        ]
    });
}

function stewartAVM_bindgrid(dataArray) {

    document.getElementById("table_StewartAVM").style.display = "";

    // Destroy old table
    if ($.fn.DataTable.isDataTable('#table_StewartAVM')) {
        $('#table_StewartAVM').DataTable().clear().destroy();
    }

    if (!dataArray || dataArray.length === 0) {
        $('#table_StewartAVM tbody').html('<tr><td colspan="8">No Data Found</td></tr>');
        return;
    }

    $('#table_StewartAVM').DataTable({
        dom: 'ft',
        data: dataArray,
        paging: false,
        processing: true,
        ordering: false,
        fixedHeader: true,
        scrollX: true,
        scrollY: '500px',
        scrollCollapse: true,

        columns: [
            {
                data: 'Error in excel column', title: 'Error in Excel',
                render: function (data) {
                    if (!data) return '';
                    return `<span style="color:red; font-weight:600;">${data}</span>`;
                }
            },
            { data: null, title: 'Sr. #', render: function (data, type, row, meta) { return meta.row + 1; } },
            { data: null, title: '<span style="white-space:nowrap;">Billing Cycle</span>', render: function () { return billingPeriod; }, width: '250px' },

            // ? IMPORTANT: Use EXACT keys with spaces
            { data: 'RequestDate', title: 'Request Date', render: formatMMDDYYYY },
            { data: 'CAID', title: 'CAID' },
            { data: 'LoanNo', title: 'Loan No' },
            { data: 'Address', title: 'Address' },
            { data: 'City', title: 'City' },
            { data: 'State', title: 'State' },
            { data: 'ZipCode', title: 'Zip Code' }
        ]
    });
}

function stewartCDA_bindgrid(dataArray) {

    document.getElementById("table_StewartCDA").style.display = "";

    // Destroy old table
    if ($.fn.DataTable.isDataTable('#table_StewartCDA')) {
        $('#table_StewartCDA').DataTable().clear().destroy();
    }

    if (!dataArray || dataArray.length === 0) {
        $('#table_StewartCDA tbody').html('<tr><td colspan="8">No Data Found</td></tr>');
        return;
    }

    $('#table_StewartCDA').DataTable({
        dom: 'ft',
        data: dataArray,
        paging: false,
        processing: true,
        ordering: false,
        fixedHeader: true,
        scrollX: true,
        scrollY: '500px',
        scrollCollapse: true,

        columns: [
            /* { data: null, title: '<input type="checkbox" id="taskselect_all" />', orderable: false, render: function () { return '<input type="checkbox" class="row_checkbox">'; } },*/
            {
                data: 'Error in excel column', title: '<span style="white-space:nowrap;">Error in Excel</span>',
                render: function (data) {
                    if (!data) return '';
                    return `<span style="color:red; font-weight:600;">${data}</span>`;
                }
            },
            { data: null, title: '<span style="white-space:nowrap;">Sr. #</span>', render: function (data, type, row, meta) { return meta.row + 1; } },
            { data: null, title: '<span style="white-space:nowrap;">Billing Cycle</span>', render: function () { return billingPeriod; }, width: '250px' },

            // ? IMPORTANT: Use EXACT keys with spaces
            { data: 'InvoiceDate', title: 'Invoice Date', render: formatMMDDYYYY },
            { data: 'OrderDate', title: 'Order Date', render: formatMMDDYYYY },
            { data: 'CompleteDate', title: 'Complete Date', render: formatMMDDYYYY },
            { data: 'BusinessDays', title: 'Business Days' },
            { data: 'InvoiceNumber', title: 'Invoice Number' },
            { data: 'Fee', title: 'Fee' },
            { data: 'OrderID', title: 'OrderID' },
            { data: 'LoanNumber', title: 'Loan Number' },
            { data: 'CaseNumber', title: 'Case Number' },
            { data: 'Borrower', title: 'Borrower' },
            { data: 'Address1', title: '<span style="white-space:nowrap;">Address 1</span>' },
            { data: 'Address2', title: '<span style="white-space:nowrap;">Address 2</span>' },
            { data: 'City', title: 'City' },
            { data: 'State', title: 'State' },
            { data: 'Zip', title: 'Zip' }
        ]
    });
}



function impbill_clearData() {

    // PageMethod Call
    PageMethods.ClearData(
        function (response) {

            if (response == 0) {
                Swal.fire({
                    icon: 'warning',
                    title: 'Warning',
                    text: "Error in clearing data.",
                    zIndex: 999999
                });
            } else {

                var error_count = 0;
                var dateerror_count = 0;

                // Destroy old table
                if ($.fn.DataTable.isDataTable('#table_impbill')) {
                    $('#table_impbill').DataTable().clear().destroy();
                }

                document.getElementById("impbill_verify").disabled = false;
                document.getElementById("impbill_alert").innerText = "";

                billingPeriod = "";
                billingType = "";

                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: "Data deleted successfully.",
                }).then(function () {
                    location.reload();
                });
            }
        },

        function (error) {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: error.get_message()
            });
        }
    );

    return false;
}

function impbill_changetype() {

    $('#impbill_month').prop('selectedIndex', 0);
    $('#impbill_year').prop('selectedIndex', 0);

    $('#impbill_billingPeriod').val('');
    $('#impbill_attachment').val('');

    billingPeriod = "";
    billingType = "";

    // Clear alert box
    document.getElementById("impbill_alert").innerHTML = "";
    document.getElementById("impbill_tableheader").innerHTML = "";

    // PageMethod Call
    PageMethods.ClearData(

        function (response) {

            if (response > 0) {
                document.getElementById("table_griffin").style.display = "none";
                document.getElementById("table_StewartAVM").style.display = "none";
                document.getElementById("table_StewartCDA").style.display = "none";

                // Griffin Table
                if ($.fn.DataTable.isDataTable('#table_griffin')) {
                    $('#table_griffin').DataTable().clear().destroy();
                    $('#table_griffin').hide();
                }

                // StewartAVM Table
                if ($.fn.DataTable.isDataTable('#table_StewartAVM')) {
                    $('#table_StewartAVM').DataTable().clear().destroy();
                    $('#table_StewartAVM').hide();
                }

                // StewartCDA Table
                if ($.fn.DataTable.isDataTable('#table_StewartCDA')) {
                    $('#table_StewartCDA').DataTable().clear().destroy();
                    $('#table_StewartCDA').hide();
                }
            }
        },

        function (error) {

            CanopyUI.debug(error);

            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: error.get_message()
            });

        }
    );
}

function ipmbill_BindYear() {

    var start = new Date().getFullYear();

    var select = document.getElementById("impbill_year");
    let options = select.getElementsByTagName('option');
    for (var i = options.length; i--;) {
        select.removeChild(options[i]);
    }

    $("#impbill_year").append($("<option></option>").val("").html("Select"));
    for (var i = start; i > start - 5; i--) {
        $("#impbill_year").append($("<option></option>").val(i).html(i));
    }
}

function getMonthDateRange() {

    var monthName = parseInt(document.getElementById("impbill_month").value);
    var year = $('#impbill_year').val();

    if (monthName > 0 && year != "" && year != "Select") {

        var monthIndex = monthName - 1;

        const firstDate = new Date(year, monthIndex, 1);
        const lastDate = new Date(year, monthIndex + 1, 0);

        // Format date as dd-MMM-yyyy
        function formatDate(date) {

            const day = String(date.getDate()).padStart(2, '0');

            const months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
                "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

            const month = months[date.getMonth()];
            const year1 = date.getFullYear();

            return `${day}-${month}-${year1}`;
        }

        var result = `${formatDate(firstDate)} ~ ${formatDate(lastDate)}`;

        CanopyUI.debug(result);

        document.getElementById("impbill_billingPeriod").value = result;
    }
    else {
        document.getElementById("impbill_billingPeriod").value = "";
    }
}

function formatMMDDYYYY(data) {
    if (!data) return '';
    let date = new Date(data);
    return (date.getMonth() + 1) + '/' +
        date.getDate() + '/' +
        date.getFullYear();
}

function validateRow(row, type) {

    let errors = [];

    if (type == "Griffin") {

        let LoanNo = row["Loan ID"];

        if (LoanNo === "" || LoanNo === null || LoanNo.length === 0) {
            error_count++;
            errors.push("Please enter Loan ID.");
        }
    }
    else if (type == "StewartAVM") {

        let LoanNo = row["LoanNo"];

        if (LoanNo === "" || LoanNo === null || LoanNo.length === 0) {
            error_count++;
            errors.push("Please enter Loan No.");
        }
    }
    else if (type == "StewartCDA") {

        let LoanNo = row["LoanNumber"];

        if (LoanNo === "" || LoanNo === null || LoanNo.length === 0) {
            error_count++;
            errors.push("Please enter Loan Number.");
        }
    }

    return errors;
}


