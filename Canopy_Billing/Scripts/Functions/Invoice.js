function addinvoice_closepopup() {
    $('#addinvoice_dverror').modal('hide');
}

// #region Infinity

var addinvoice_table;
var addinvoice_html;
var InvoiceID;
var addinvoice_sciennadetails;
var sciennadetails_html = '';
var addinvoice_laborcharges;
var laborcharges_html = '';
var addinvoice_loandetailstable;
var loandetails_html = '';

function parseDate(str) {
    var mdy = str.split('/');
    return new Date(mdy[2], mdy[0] - 1, mdy[1]);
}

function datediff(first, second) {
    return Math.round((second - first) / (1000 * 60 * 60 * 24));
}


function blankForNull(s) {
    return s == "null" || s == null ? "" : s;

}
function addinvoice_BindYear() {
    var start = new Date().getFullYear();

    var select = document.getElementById("addinvoice_year");
    let options = select.getElementsByTagName('option');

    for (var i = options.length; i--;) {
        select.removeChild(options[i]);
    }

    $("#addinvoice_year").append($("<option></option>").val("").html("Select"));
    for (var i = start; i > start - 5; i--) {
        $("#addinvoice_year").append($("<option></option>").val(i).html(i));
    }
}

function getprojectenabledisable(ddlInvoiceType) {
    var invoicetype = ddlInvoiceType.options[ddlInvoiceType.selectedIndex].value;

    if (invoicetype == "Abstractor") {
        tdloanorder.innerHTML = "Order Count";
        document.getElementById("addinvoice_othertype").disabled = "disabled";
        document.getElementById("trScienna1").style.display = "none";
        document.getElementById("trCompliance").style.display = "none";
        document.getElementById("trRemoteUW").style.display = "none";
    }
    else if (invoicetype == "Other") {
        tdloanorder.innerHTML = "Order Count";
        document.getElementById("addinvoice_othertype").disabled = "";
        document.getElementById("trScienna1").style.display = "none";
        document.getElementById("trCompliance").style.display = "none";
        document.getElementById("trRemoteUW").style.display = "none";
    }
    else if (invoicetype == "Scienna") {
        tdloanorder.innerHTML = "Loan Count";
        document.getElementById("addinvoice_othertype").disabled = "disabled";
        document.getElementById("trScienna1").style.display = "";
        document.getElementById("trCompliance").style.display = "none";
        document.getElementById("trRemoteUW").style.display = "none";
    }
    else if (invoicetype == "Compliance") {
        tdloanorder.innerHTML = "Loan Count";
        document.getElementById("addinvoice_othertype").disabled = "disabled";
        document.getElementById("trScienna1").style.display = "none";
        document.getElementById("trCompliance").style.display = "";
        document.getElementById("trRemoteUW").style.display = "none";
    }
    else if (invoicetype == "Remote UW") {
        tdloanorder.innerHTML = "Loan Count";
        document.getElementById("addinvoice_othertype").disabled = "disabled";
        document.getElementById("trScienna1").style.display = "none";
        document.getElementById("trCompliance").style.display = "none";
        document.getElementById("trRemoteUW").style.display = "";
    }
    else {
        tdloanorder.innerHTML = "Loan Count";
        document.getElementById("addinvoice_othertype").disabled = "disabled";
        document.getElementById("trScienna1").style.display = "none";
        document.getElementById("trCompliance").style.display = "none";
        document.getElementById("trRemoteUW").style.display = "none";
    }
}

function addinvoice_bindgrid() {
    $('#load1').show();

    addinvoice_html = '';
    $.ajax({
        url: "AddInvoice.aspx/GetAllInvoices",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//

            $.each(dataArray, function (index, value) {
                var addeddate = eval(value.Addeddate.replace(/\/Date\((\d+)\)\//gi, "new Date($1).toLocaleDateString(\"en-US\")"));
                addinvoice_html += '<tr>';
                addinvoice_html += '<td style="text-wrap: nowrap;text-align:center;"><a class="dropdown-item" href="#!" id="ActionsEx" onclick="AddAttachment(' + value.InvoiceId + ',' + index + ');"><span style="color: dodgerblue;"><i class="uil fs-0 me-2 uil-download-alt"></i></span></a></td>';
                addinvoice_html += '<td style="text-wrap: nowrap;text-align:center;">' + blankForNull((index + 1)) + '</td>';
                addinvoice_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.Month) + '</td>';
                addinvoice_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.Year) + '</td>';
                addinvoice_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.InvoiceType) + '</td>';
                addinvoice_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.InvoiceNo) + '</td>';
                addinvoice_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.InvoiceDate) + '</td>';
                addinvoice_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.DueDate) + '</td>';
                addinvoice_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.BillTo) + '</td>';
                addinvoice_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.Balance) + '</td>';
                addinvoice_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.Delay) + '</td>';
                addinvoice_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.Remark) + '</td>';
                addinvoice_html += '<td style="text-wrap: nowrap; ">' + blankForNull(addeddate) + '</td>';
                addinvoice_html += '</tr>';
            });

            if ($.fn.dataTable.isDataTable('#addinvoice_table')) {
                addinvoice_table.destroy();
            }
            $('#addinvoice_table tbody').html(addinvoice_html);
            //else
            addinvoice_table = $('#addinvoice_table').DataTable({
                dom: 'lBftip',
                scrollX: true,
                destroy: true,
                "paging": true,
                "autoWidth": true,
                select: true,
                "ordering": false,
                processing: true,
                'select': {
                    'style': 'single'
                },

                initComplete: function () {
                    $('#load1').hide();
                },

                buttons: [
                    {
                        extend: 'excelHtml5', title: 'Bank Names', autoFilter: true,
                        exportOptions: {
                            columns: [0, 1, 2],
                        }

                    },


                ],

            });

            //$('#fnalize tbody').on('click', 'tr', function () {
            //    row = table.row(this).data();
            //});
        },
        error: function (error) {
            CanopyUI.showError(error, 'Unable to complete request.');
        }
    });
    return false;
}

function addinvoice_bindsciennaexcel() {
    document.getElementById("dvScienna").style.display = '';
    $('#load1').show();
    var ddlMonth = document.getElementById("addinvoice_month");
    var month = ddlMonth.options[ddlMonth.selectedIndex].value;
    var ddlYear = document.getElementById("addinvoice_year");
    var year = ddlYear.options[ddlYear.selectedIndex].value;
    InvoiceID = 849;
    month = 'March';
    year = '2025';
    sciennadetails_html = '';
    $.ajax({
        url: "AddInvoice.aspx/GetSciennaDetails_AfterImport",
        type: "POST",
        dataType: "json",
        data: "{InvoiceID:" + InvoiceID + ", Month:'" + month + "', Year:'" + year + "'}",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//

            $.each(dataArray, function (index, value) {

                sciennadetails_html += '<tr>';
                sciennadetails_html += '<td style="text-wrap: nowrap;text-align:center;">' + blankForNull((index + 1)) + '</td>';
                sciennadetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.Month) + '</td>';
                sciennadetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.Year) + '</td>';
                sciennadetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.Client) + '</td>';
                sciennadetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.Project) + '</td>';
                sciennadetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.PeriodEnding) + '</td>';
                sciennadetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.LoansReviewed) + '</td>';
                sciennadetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.PerLoanUsageFees) + '</td>';
                sciennadetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.UsageFees) + '</td>';
                sciennadetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.Labor) + '</td>';
                sciennadetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.Total) + '</td>';
                sciennadetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.SysVerificationStatus) + '</td>';
                sciennadetails_html += '</tr>';
            });



            if ($.fn.dataTable.isDataTable('#addinvoice_sciennadetails')) {
                addinvoice_sciennadetails.destroy();
            }
            $('#addinvoice_sciennadetails tbody').html(sciennadetails_html);
            //else
            addinvoice_sciennadetails = $('#addinvoice_sciennadetails').DataTable({
                dom: 'lBftip',
                scrollX: true,
                destroy: true,
                "paging": true,
                "autoWidth": true,
                select: true,
                "ordering": false,
                processing: true,
                'select': {
                    'style': 'single'
                },

                initComplete: function () {
                    $('#load1').hide();
                },

                buttons: [
                    {
                        extend: 'excelHtml5', title: 'Bank Names', autoFilter: true,
                        exportOptions: {
                            columns: [0, 1, 2],
                        }

                    },


                ],

            });

            //$('#fnalize tbody').on('click', 'tr', function () {
            //    row = table.row(this).data();
            //});
        },
        error: function (error) {
            CanopyUI.showError(error, 'Unable to complete request.');
        }
    });
    return false;
}

function addinvoice_laborcharges() {
    document.getElementById("dvScienna").style.display = '';
    $('#load1').show();
    var ddlMonth = document.getElementById("addinvoice_month");
    var month = ddlMonth.options[ddlMonth.selectedIndex].value;
    var ddlYear = document.getElementById("addinvoice_year");
    var year = ddlYear.options[ddlYear.selectedIndex].value;
    InvoiceID = 849;
    month = 'March';
    year = '2025';
    laborcharges_html = '';
    $.ajax({
        url: "AddInvoice.aspx/GetLaborCharges_AfterImport",
        type: "POST",
        dataType: "json",
        data: "{InvoiceID:" + InvoiceID + ", Month:'" + month + "', Year:'" + year + "'}",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//

            $.each(dataArray, function (index, value) {

                laborcharges_html += '<tr>';
                laborcharges_html += '<td style="text-wrap: nowrap;text-align:center;">' + blankForNull((index + 1)) + '</td>';
                laborcharges_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.Month) + '</td>';
                laborcharges_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.Year) + '</td>';
                laborcharges_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.Personnel) + '</td>';
                laborcharges_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.Client) + '</td>';
                laborcharges_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.Project) + '</td>';
                laborcharges_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.Date) + '</td>';
                laborcharges_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.BeginTime) + '</td>';
                laborcharges_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.EndTime) + '</td>';
                laborcharges_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.Hours) + '</td>';
                laborcharges_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.Rate) + '</td>';
                laborcharges_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.PreliminaryFee) + '</td>';
                laborcharges_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.ChargedAt) + '</td>';
                laborcharges_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.Reason) + '</td>';
                laborcharges_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.Fee) + '</td>';
                laborcharges_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.Activity) + '</td>';
                laborcharges_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.DescriptionOfSessionActivities) + '</td>';
                laborcharges_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.IsDuplicate) + '</td>';
                laborcharges_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.SysVerificationStatus) + '</td>';
                laborcharges_html += '</tr>';
            });



            if ($.fn.dataTable.isDataTable('#addinvoice_laborcharges')) {
                addinvoice_laborcharges.destroy();
            }
            $('#addinvoice_laborcharges tbody').html(laborcharges_html);
            //else
            addinvoice_laborcharges = $('#addinvoice_laborcharges').DataTable({
                dom: 'lBftip',
                destroy: true,
                "paging": true,
                "autoWidth": true,
                select: true,
                "ordering": false,
                processing: true,
                'select': {
                    'style': 'single'
                },

                initComplete: function () {
                    $('#load1').hide();
                    jQuery('.dataTable').wrap('<div class="dataTables_scroll" />');

                },

                buttons: [
                    {
                        extend: 'excelHtml5', title: 'Bank Names', autoFilter: true,
                        exportOptions: {
                            columns: [0, 1, 2],
                        }

                    },


                ],

            });

            //$('#fnalize tbody').on('click', 'tr', function () {
            //    row = table.row(this).data();
            //});
        },
        error: function (error) {
            CanopyUI.showError(error, 'Unable to complete request.');
        }
    });
    return false;
}

function addinvoice_loandetails() {
    document.getElementById("dvScienna").style.display = '';
    $('#load1').show();

    loandetails_html = '';
    $.ajax({
        url: "AddInvoice.aspx/GetLoanDetails_AfterImport",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//

            $.each(dataArray, function (index, value) {

                loandetails_html += '<tr>';
                loandetails_html += '<td style="text-wrap: nowrap;text-align:center;">' + blankForNull((index + 1)) + '</td>';
                loandetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.ClientName) + '</td>';
                loandetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.ProjectName) + '</td>';
                loandetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.Loan1) + '</td>';
                loandetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.SciennaId) + '</td>';
                loandetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.StartDate) + '</td>';
                loandetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.SignOffDate) + '</td>';
                loandetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.RawBillable) + '</td>';
                laborcharges_html += '</tr>';
            });



            if ($.fn.dataTable.isDataTable('#addinvoice_loandetailstable')) {
                addinvoice_loandetailstable.destroy();
            }
            $('#addinvoice_loandetailstable tbody').html(loandetails_html);
            //else
            addinvoice_loandetailstable = $('#addinvoice_loandetailstable').DataTable({
                dom: 'lBftip',
                destroy: true,
                "paging": true,
                "autoWidth": true,
                select: true,
                "ordering": false,
                processing: true,
                'select': {
                    'style': 'single'
                },

                initComplete: function () {
                    $('#load1').hide();
                    jQuery('.dataTable').wrap('<div class="dataTables_scroll" />');

                },

                buttons: [
                    {
                        extend: 'excelHtml5', title: 'Bank Names', autoFilter: true,
                        exportOptions: {
                            columns: [0, 1, 2],
                        }

                    },


                ],

            });

            //$('#fnalize tbody').on('click', 'tr', function () {
            //    row = table.row(this).data();
            //});
        },
        error: function (error) {
            CanopyUI.showError(error, 'Unable to complete request.');
        }
    });
    return false;
}

function addinvoice_submit() {
    var ddlMonth = document.getElementById("addinvoice_month");
    var month = ddlMonth.options[ddlMonth.selectedIndex].value;
    var ddlYear = document.getElementById("addinvoice_year");
    var year = ddlYear.options[ddlYear.selectedIndex].value;
    if (month == "") {
        alert("Please select month");
        return false;
    }
    if (year == "") {
        alert("Please select year");
        return false;
    }
    var ddlinvoiceType = document.getElementById("addinvoice_invoicetype");
    var invoicetype = ddlinvoiceType.options[ddlinvoiceType.selectedIndex].value;
    var otherinvoice = document.getElementById("addinvoice_othertype").value;
    var invoicedate = document.getElementById("addinvoice_invoicedate").value;
    var loancount = document.getElementById("addinvoice_loancount").value;
    var duedate = document.getElementById("addinvoice_duedate").value;
    var ddlCurrency = document.getElementById("addinvoice_currency");
    var currency = ddlCurrency.options[ddlCurrency.selectedIndex].value;
    if (currency == "") {
        alert("Please select currency");
        return false;
    }
    var ddlDomain = document.getElementById("addinvoice_domain");
    var domain = ddlDomain.options[ddlDomain.selectedIndex].value;
    if (domain == "") {
        alert("Please select domain");
        return false;
    }

    var invoiceno = document.getElementById("addinvoice_invoicenumber").value;
    var invoiceamount = document.getElementById("addinvoice_invoiceamount").value;
    var billto = document.getElementById("addinvoice_billto").value;
    var remark = document.getElementById("addinvoice_delayremark").value;

    if (invoicetype == "Other" && otherinvoice == "") {
        alert("Please enter other invoice type.");
        return false;
    }
    //var ddlproject = document.getElementById("addinvoice_projectno");
    //var projectid = ddlproject.options[ddlproject.selectedIndex].value;
    var delaycause = document.getElementById("addinvoice_delayremark").value;
    var invdate = new Date(invoicedate);
    var q = new Date();
    var m = q.getMonth() + 1;
    var d = q.getDay();
    var y = q.getFullYear();
    var date = new Date();
    date = date.toLocaleDateString("en-US");
    invdate = invdate.toLocaleDateString("en-US");

    var diffDays = datediff(parseDate(invdate), parseDate(date));
    if (diffDays >= 3 && delaycause == "") {
        alert("Please enter delay remark");
        return false;
    }

    PageMethods.InsertInfinityInvoice_Scienna(month, year, invoicedate, duedate, domain, currency, loancount, invoiceno, invoiceamount, billto, invoicetype, otherinvoice, delaycause, remark, addinvoice_OnSuccess, addinvoice_OnError);

    return false;
}

function addinvoice_OnSuccess(result) {
    InvoiceID = result;
    if (result > 0) {
        document.getElementById("addinvoice_errmsg").innerHTML = "Invoice added successfully!";
        $('#addinvoice_dverror').modal('show');
        //addinvoice_bindgrid();
        var ddlMonth = document.getElementById("addinvoice_month");
        var month = ddlMonth.options[ddlMonth.selectedIndex].value;
        var ddlYear = document.getElementById("addinvoice_year");
        var year = ddlYear.options[ddlYear.selectedIndex].value;

        PageMethods.InsertSciennaInvoice(result, month, year, uploadexcel_OnSuccess, uploadexcel_OnError);
        return false;
    }
    else {
        document.getElementById("addinvoice_errmsg").innerHTML = "Oops! Error occured while adding invoice. Please contact administrator!";
        document.getElementById("addinvoice_errmsg").style.color = 'red';
        $('#addinvoice_dverror').modal('show');
        return false;
    }
    return false;
}
function addinvoice_OnError(error) {
    CanopyUI.showError(error, 'Unable to complete request.');
}

function uploadexcel_OnSuccess(result) {
    var ddlMonth = document.getElementById("addinvoice_month");
    var month = ddlMonth.options[ddlMonth.selectedIndex].value;
    var ddlYear = document.getElementById("addinvoice_year");
    var year = ddlYear.options[ddlYear.selectedIndex].value;
    if (result > 0) {
        document.getElementById("addinvoice_errmsg").innerHTML = "Invoice added successfully!";
        $('#addinvoice_dverror').modal('show');
        PageMethods.InsertLaborCharges(InvoiceID, month, year, labor_OnSuccess, labor_OnError);
        return false;
    }
    else if (result == -1) {
        document.getElementById("addinvoice_errmsg").innerHTML = "" + month + " - " + year + " scienna billing already sent to accounts!";
        document.getElementById("addinvoice_errmsg").style.color = 'red';
        $('#addinvoice_dverror').modal('show');
        return false;
    }
    else if (result == -2) {
        document.getElementById("addinvoice_errmsg").innerHTML = "" + month + " - " + year + " scienna records are already available in system!";
        document.getElementById("addinvoice_errmsg").style.color = 'red';
        $('#addinvoice_dverror').modal('show');
        return false;
    }
    else if (result == -4) {
        document.getElementById("addinvoice_errmsg").innerHTML = "File with extension .xls or .xlsx are accepted. Please check file uploaded!";
        document.getElementById("addinvoice_errmsg").style.color = 'red';
        $('#addinvoice_dverror').modal('show');
        return false;
    }
    else {
        document.getElementById("addinvoice_errmsg").innerHTML = "Oops! Error occured while adding invoice. Please contact administrator!";
        document.getElementById("addinvoice_errmsg").style.color = 'red';
        $('#addinvoice_dverror').modal('show');
        return false;
    }
    return false;
}
function uploadexcel_OnError(error) {
    CanopyUI.showError(error, 'Unable to complete request.');
}

function labor_OnSuccess(result) {

    if (result > 0) {
        document.getElementById("addinvoice_errmsg").innerHTML = "Data imported successfully!";
        //$('#addinvoice_dverror').modal('show');
        PageMethods.InsertSciennaLoans(loandetails_OnSuccess, loandetails_OnError);
        return false;
    }
    if (result == -4) {
        document.getElementById("addinvoice_errmsg").innerHTML = "File with extension .xls or .xlsx are accepted. Please check file uploaded!";
        document.getElementById("addinvoice_errmsg").style.color = 'red';
        $('#addinvoice_dverror').modal('show');
        return false;
    }
    else {
        document.getElementById("addinvoice_errmsg").innerHTML = "Oops! Error occured while adding loan details. Please check excel columns headers!";
        document.getElementById("addinvoice_errmsg").style.color = 'red';
        $('#addinvoice_dverror').modal('show');
        return false;
    }
    return false;
}
function labor_OnError(error) {
    CanopyUI.showError(error, 'Unable to complete request.');
}

function loandetails_OnSuccess(result) {

    if (result > 0) {
        document.getElementById("addinvoice_errmsg").innerHTML = "Data imported successfully!";
        $('#addinvoice_dverror').modal('show');
        addinvoice_bindsciennaexcel();
        addinvoice_laborcharges();
        addinvoice_loandetails();
        return false;
    }
    else if (result == -1) {
        document.getElementById("addinvoice_errmsg").innerHTML = "" + month + " - " + year + " labor charges already sent to accounts!";
        document.getElementById("addinvoice_errmsg").style.color = 'red';
        $('#addinvoice_dverror').modal('show');
        return false;
    }
    else if (result == -2) {
        document.getElementById("addinvoice_errmsg").innerHTML = "" + month + " - " + year + " labor charges are already available in system!";
        document.getElementById("addinvoice_errmsg").style.color = 'red';
        $('#addinvoice_dverror').modal('show');
        return false;
    }
    else if (result == -4) {
        document.getElementById("addinvoice_errmsg").innerHTML = "File with extension .xls or .xlsx are accepted. Please check file uploaded!";
        document.getElementById("addinvoice_errmsg").style.color = 'red';
        $('#addinvoice_dverror').modal('show');
        return false;
    }
    else {
        document.getElementById("addinvoice_errmsg").innerHTML = "Oops! Error occured while adding labor charges. Please check excel columns headers!";
        document.getElementById("addinvoice_errmsg").style.color = 'red';
        $('#addinvoice_dverror').modal('show');
        return false;
    }
    return false;
}
function loandetails_OnError(error) {
    CanopyUI.showError(error, 'Unable to complete request.');
}

// #endregion Infinity

// #region Canopy

var InvoiceID_Canopy;
var addinvoice_stewartgrid;
var stewart_html = '';

function addinvoice_BindYear_canopy() {
    var start = new Date().getFullYear();

    var select = document.getElementById("addinvoice_year_canopy");
    let options = select.getElementsByTagName('option');

    for (var i = options.length; i--;) {
        select.removeChild(options[i]);
    }

    $("#addinvoice_year_canopy").append($("<option></option>").val("").html("Select"));
    for (var i = start; i > start - 5; i--) {
        $("#addinvoice_year_canopy").append($("<option></option>").val(i).html(i));
    }
}

function addinvoice_submit_canopy() {
    var ddlmonth = document.getElementById("addinvoice_month_canopy");
    var month = ddlmonth.options[ddlmonth.selectedIndex].value;
    
    if (month == "") {
        alert("Please select month");
        return false;
    }
    var ddlyear = document.getElementById("addinvoice_year_canopy");
    var year = ddlyear.options[ddlyear.selectedIndex].value;
    if (year == "") {
        alert("Please select year");
        return false;
    }
    var ddlinvoicetype = document.getElementById("addinvoice_invoicetype_canopy");
    var invoicetype = ddlinvoicetype.options[ddlinvoicetype.selectedIndex].value;
    if (invoicetype == "") {
        alert("Please select invoice type");
        return false;
    }
    var invoiceno = document.getElementById("addinvoice_invoiceno_canopy").value;
    var invoiceamount = document.getElementById("addinvoice_invoiceamount_canopy").value;
    var invoicedate = document.getElementById("addinvoice_invoicedate_canopy").value;
    var billto = document.getElementById("addinvoice_billto_canopy").value;

    PageMethods.InsertStewartInvoice(month, year, invoicedate, invoiceno, invoiceamount, billto, stewart_OnSuccess, stewart_OnError);
    return false;
}

function stewart_OnSuccess(result) {
    InvoiceID_Canopy = result;
    var ddlMonth = document.getElementById("addinvoice_month_canopy");
    var month = ddlMonth.options[ddlMonth.selectedIndex].value;
    var ddlYear = document.getElementById("addinvoice_year_canopy");
    var year = ddlYear.options[ddlYear.selectedIndex].value;
    if (result > 0) {
        document.getElementById("addinvoice_errmsg").innerHTML = "Invoice added successfully!";
        $('#addinvoice_dverror').modal('show');
        PageMethods.InsertStewartExcel(InvoiceID_Canopy, month, year, uploadstewartexcel_OnSuccess, uploadstewartexcel_OnError);
        return false;
    }
    else if (result == -1) {
        document.getElementById("addinvoice_errmsg").innerHTML = "" + month + " - " + year + " stewart billing already sent to accounts!";
        document.getElementById("addinvoice_errmsg").style.color = 'red';
        $('#addinvoice_dverror').modal('show');
        return false;
    }
    else if (result == -2) {
        document.getElementById("addinvoice_errmsg").innerHTML = "" + month + " - " + year + " stewart records are already available in system!";
        document.getElementById("addinvoice_errmsg").style.color = 'red';
        $('#addinvoice_dverror').modal('show');
        return false;
    }
    else if (result == -4) {
        document.getElementById("addinvoice_errmsg").innerHTML = "File with extension .xls or .xlsx are accepted. Please check file uploaded!";
        document.getElementById("addinvoice_errmsg").style.color = 'red';
        $('#addinvoice_dverror').modal('show');
        return false;
    }
    else {
        document.getElementById("addinvoice_errmsg").innerHTML = "Oops! Error occured while adding invoice. Please contact administrator!";
        document.getElementById("addinvoice_errmsg").style.color = 'red';
        $('#addinvoice_dverror').modal('show');
        return false;
    }
    return false;
}
function stewart_OnError(error) {
    CanopyUI.showError(error, 'Unable to complete request.');
}

function uploadstewartexcel_OnSuccess(result) {
    var ddlMonth = document.getElementById("addinvoice_month_canopy");
    var month = ddlMonth.options[ddlMonth.selectedIndex].value;
    var ddlYear = document.getElementById("addinvoice_year_canopy");
    var year = ddlYear.options[ddlYear.selectedIndex].value;
    if (result > 0) {
        document.getElementById("addinvoice_errmsg").innerHTML = "Data imported successfully!";
        $('#addinvoice_dverror').modal('show');
        addinvocie_BindStewartGridAfterImport(InvoiceID_Canopy, month, year);
        return false;
    }
    if (result == -4) {
        document.getElementById("addinvoice_errmsg").innerHTML = "File with extension .xls or .xlsx are accepted. Please check file uploaded!";
        document.getElementById("addinvoice_errmsg").style.color = 'red';
        $('#addinvoice_dverror').modal('show');
        return false;
    }
    else {
        document.getElementById("addinvoice_errmsg").innerHTML = "Oops! Error occured while adding loan details. Please check excel columns headers!";
        document.getElementById("addinvoice_errmsg").style.color = 'red';
        $('#addinvoice_dverror').modal('show');
        return false;
    }
    return false;
}
function uploadstewartexcel_OnError(error) {
    CanopyUI.showError(error, 'Unable to complete request.');
}

function addinvocie_BindStewartGridAfterImport(InvID, Month, Year) {
    $('#load1').show();

    stewart_html = '';
    $.ajax({
        url: "AddInvoice.aspx/VerifyStewart",
        type: "POST",
        dataType: "json",
        data: "{InvoiceID:" + InvID + ", Month:'" + Month + "', Year:'" + Year + "'}",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//

            $.each(dataArray, function (index, value) {
                stewart_html += '<tr>';
                stewart_html += '<td style="text-wrap: nowrap;text-align:center;">' + blankForNull((index + 1)) + '</td>';
                stewart_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.Month) + '</td>';
                stewart_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.Year) + '</td>';
                stewart_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.LoanNumber) + '</td>';
                stewart_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.CaseNumber) + '</td>';
                stewart_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.InvoiceNumber) + '</td>';
                stewart_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.InvoiceDate) + '</td>';
                stewart_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.OrderDate) + '</td>';
                stewart_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.Fee) + '</td>';
                stewart_html += '</tr>';
            });

            if ($.fn.dataTable.isDataTable('#addinvoice_stewartgrid')) {
                addinvoice_stewartgrid.destroy();
            }
            $('#addinvoice_stewartgrid tbody').html(stewart_html);
            //else
            addinvoice_stewartgrid = $('#addinvoice_stewartgrid').DataTable({
                dom: 'lBftip',
                scrollX: true,
                destroy: true,
                "paging": true,
                "autoWidth": true,
                select: true,
                "ordering": false,
                processing: true,
                'select': {
                    'style': 'single'
                },

                initComplete: function () {
                    $('#load1').hide();
                },

            });

            //$('#fnalize tbody').on('click', 'tr', function () {
            //    row = table.row(this).data();
            //});
        },
        error: function (error) {
            CanopyUI.showError(error, 'Unable to complete request.');
        }
    });
    return false;
}

// #endregion Canopy

