var cbr_canopydetails;
var canopybilling_table=''
var cbr_requestInProgress = false;

function blankForNull(s) {
    return s == "null" || s == null ? "" : s;
}


function cbr_bindyear() {
    var start = new Date().getFullYear();

    var select = document.getElementById("cbr_year");
    let options = select.getElementsByTagName('option');

    for (var i = options.length; i--;) {
        select.removeChild(options[i]);
    }

    $("#cbr_year").append($("<option></option>").val("").html("Select"));
    for (var i = start; i > start - 5; i--) {
        $("#cbr_year").append($("<option></option>").val(i).html(i));
    }
}



function cbr_bindClientReportgrid(skipSyncCheck) {
    if (cbr_requestInProgress) return false;

    if (!skipSyncCheck) {
        cbr_checkSyncFreshness();
        return false;
    }

    cbr_requestInProgress = true;
    $('#cbr_btnShow, #cbr_btnSync, #cbr_btnNoSync').prop('disabled', true);
    $('#cbr_loadingText').text('Generating report...');
    $('#load1').show();
    var columns = [];
    var FromDate = document.getElementById("cbr_month").value;
    var ToDate = document.getElementById("cbr_year").value;
    // alert(FromDate);
    // alert(ToDate);
    $.ajax({
        url: "ClientBillingReport.aspx/GetData",
        type: "POST",
        dataType: "json",
        data: "{FromDate:'" + FromDate + "',ToDate:'" + ToDate + "'}",
        contentType: "application/json; charset=utf-8",
        success: function (data) {

            if ($.fn.dataTable.isDataTable('#canopybilling_table')) {
                $('#canopybilling_table').DataTable().destroy();
            }  /*alert(data.d);*/
            dataArray = JSON.parse(data.d);

            if (dataArray.length === 0) {
                $('#load1').hide();
                cbr_closeProgressPopup();
                cbr_finishRequest();
                return;
            }

            columnNames = Object.keys(dataArray[0]); //.Table[0]] refers to the propery name of the returned json

            for (var i in columnNames) {
                columns.push({
                    data: columnNames[i],
                    title: columnNames[i]
                });
            }
            $('#canopybilling_table').DataTable({
                dom: 'Blftp',
                scrollX: true,
                destroy: true,
                paging: true,
                "autoWidth": true,
                select: true,
                processing: true,
                "aaSorting": [],
                'select': {
                    'style': 'single'
                },
                "data": dataArray,
                columns: columns,

                initComplete: function () {
                    $('#load1').hide();
                    cbr_closeProgressPopup();
                    cbr_finishRequest();
                },
                buttons: [
                    {
                        extend: 'excelHtml5', title: "Billing Report", autoFilter: true,
                    },
                ],


            });
        },
        error: function () {
            $('#load1').hide();
            cbr_closeProgressPopup();
            cbr_finishRequest();
            alert('Unable to generate the report. Please try again.');
        }
    });

    return false;

    return false;
}

function cbr_checkSyncFreshness() {
    cbr_requestInProgress = true;
    $('#cbr_btnShow').prop('disabled', true);
    $('#cbr_loadingText').text('Checking data synchronization status...');
    $('#load1').show();
    // alert('1');
    $.ajax({
        url: 'ClientBillingReport.aspx/GetLauraMacSyncStatus',
        type: 'POST',
        dataType: 'json',
        data: '{}',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            $('#load1').hide();
            var status = JSON.parse(data.d);
            cbr_finishRequest();
            if (status.IsFresh) {
                cbr_bindClientReportgrid(true);
                return;
            }
            $('#cbr_syncMessage').text("Data from LauraMac ODS was last synchronised on " + status.LastSuccessfulSyncDisplay + ". If you want the latest data, click 'Sync with LauraMac ODS and Generate Report'; otherwise, click 'Show without Sync'.");
            $('#cbr_syncModal').modal('show');
        },
        error: function () {
            $('#load1').hide();
            cbr_finishRequest();
            alert('Unable to verify the LauraMac ODS synchronization status. The report was not generated.');
        }
    });
}

function cbr_syncAndShow() {
    if (cbr_requestInProgress) return false;
    cbr_requestInProgress = true;
    $('#cbr_btnSync, #cbr_btnNoSync, #cbr_btnShow').prop('disabled', true);
    $('#cbr_syncMessage').text('Data synchronization from LauraMac is in progress and may take some time. Please wait until this popup disappears. Do not click anywhere on the page during synchronization.');
    $('#cbr_syncModal .modal-footer').hide();
    $('#cbr_loadingText').text('Data synchronization from LauraMac is in progress and may take some time. Please wait until this popup disappears. Do not click anywhere on the page during synchronization.');
    $('#load1').show();
    $.ajax({
        url: 'ClientBillingReport.aspx/SyncLauraMac',
        type: 'POST',
        dataType: 'json',
        data: '{}',
        contentType: 'application/json; charset=utf-8',
        timeout: 1200000,
        success: function (data) {
            var result = JSON.parse(data.d);
            if (!result.Success) {
                $('#load1').hide();
                cbr_closeProgressPopup();
                cbr_finishRequest();
                alert(result.Message || 'LauraMac ODS synchronization failed. The report was not generated.');
                return;
            }
            $('#cbr_loadingText').text('Generating report...');
            $('#cbr_syncMessage').text('Generating report...');
            cbr_finishRequest();
            cbr_bindClientReportgrid(true);
        },
        error: function () {
            $('#load1').hide();
            cbr_closeProgressPopup();
            cbr_finishRequest();
            alert('LauraMac ODS synchronization failed or timed out. The report was not generated.');
        }
    });
    return false;
}

function cbr_showWithoutSync() {
    $('#cbr_syncModal').modal('hide');
    cbr_bindClientReportgrid(true);
    return false;
}

function cbr_finishRequest() {
    cbr_requestInProgress = false;
    $('#cbr_btnShow, #cbr_btnSync, #cbr_btnNoSync').prop('disabled', false);
}

function cbr_closeProgressPopup() {
    $('#cbr_syncModal').modal('hide');
    $('#cbr_syncModal .modal-footer').show();
}
