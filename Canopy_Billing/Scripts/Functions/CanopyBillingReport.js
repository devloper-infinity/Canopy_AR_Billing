var cbr_canopydetails;
var canopybilling_table=''

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



function cbr_bindClientReportgrid() {
    $('#load1').show();
    var columns = [];
    var FromDate = document.getElementById("cbr_month").value;
    var ToDate = document.getElementById("cbr_year").value;
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
            
            columnNames = Object.keys(dataArray[0]); //.Table[0]] refers to the propery name of the returned json
            alert(columnNames);
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
                },
                buttons: [
                    {
                        extend: 'excelHtml5', title: "Billing Report", autoFilter: true,
                    },
                ],


            });
        }
    });

    return false;

    return false;
}