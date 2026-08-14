var merge_billingtable;
var payent_table;

function blankForNull(s) {
    return s == "null" || s == null ? "" : s;
}

function merge_bindyear() {
    var start = new Date().getFullYear();

    var select = document.getElementById("merge_year");
    let options = select.getElementsByTagName('option');

    for (var i = options.length; i--;) {
        select.removeChild(options[i]);
    }

    $("#merge_year").append($("<option></option>").val("").html("Select"));
    for (var i = start; i > start - 5; i--) {
        $("#merge_year").append($("<option></option>").val(i).html(i));
    }
}

function payent_bindyear() {
    var start = new Date().getFullYear();

    var select = document.getElementById("payent_year");
    let options = select.getElementsByTagName('option');

    for (var i = options.length; i--;) {
        select.removeChild(options[i]);
    }

    $("#payent_year").append($("<option></option>").val("").html("Select"));
    for (var i = start; i > start - 5; i--) {
        $("#payent_year").append($("<option></option>").val(i).html(i));
    }
}

function merge_bindMergedGrid() {
    $('#load1').show();
    var columns = [];
    var ddlmonth = document.getElementById("merge_month");
    var month = ddlmonth.options[ddlmonth.selectedIndex].value;
    var ddlyear = document.getElementById("merge_year");
    var year = ddlyear.options[ddlyear.selectedIndex].value;
    var ddlproject = document.getElementById("merge_project");
    var project = ddlproject.options[ddlproject.selectedIndex].value;
    $.ajax({
        url: "MergedBilling.aspx/GetMergedBillingSummary",
        type: "POST",
        dataType: "json",
        data: "{Month:'" + month + "',Year:'" + year + "', ProjectId:" + project + "}",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//
            $.each(dataArray[0], function (key, value) {

                var my_item = {};
                my_item.data = key;
                my_item.title = key;
                columns.push(my_item);
            });
            if ($.fn.dataTable.isDataTable('#merge_billingtable')) {
                $('#merge_billingtable').DataTable().destroy();
            }
            merge_billingtable = $('#merge_billingtable').DataTable({
                dom: 'Bftip',
                destroy: true,
                scrollX: true,
                "paging": true,
                pageLength: 10,
                select: true,
                "ordering": false,
                processing: true,
                filter: true,
                'select': {
                    'style': 'single'
                },
                "serverSide": false,
                "data": dataArray,
                columns: columns,
                fnCreatedRow: function (nRow, aData, iDataIndex) {
                },

                initComplete: function () {
                    $('#load1').hide();

                },
                buttons: [
                    {
                        extend: 'excelHtml5', title: 'Unbilled Summary', autoFilter: true,


                    },


                ],

            });
        },
        error: function (error) {
            CanopyUI.showError(error, 'Unable to complete request.');
        }
    });
    return false;
}

function payent_bindPendingGrid() {
    $('#load1').show();
    var columns = [];
    var ddlmonth = document.getElementById("payent_month");
    var month = ddlmonth.options[ddlmonth.selectedIndex].value;
    var ddlyear = document.getElementById("payent_year");
    var year = ddlyear.options[ddlyear.selectedIndex].value;
    $.ajax({
        url: "PayingEntityMaster.aspx/GetPendingDealsForPayingEntity",
        type: "POST",
        dataType: "json",
        data: "{Month:'" + month + "',Year:'" + year + "'}",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//
            if (dataArray.length > 0) {
                document.getElementById("payent_btnupdatedata").style.display = 'inline';
            }
            else
                document.getElementById("payent_btnupdatedata").style.display = 'none';

            if ($.fn.dataTable.isDataTable('#payent_table')) {
                $('#payent_table').DataTable().destroy();
            }
            payent_table = $('#payent_table').DataTable({
                dom: 'ftip',
                destroy: true,
                scrollX: true,
                "paging": false,
                pageLength: 10,
                select: true,
                "ordering": false,
                processing: true,
                filter: true,
                'select': {
                    'style': 'single'
                },
                "serverSide": false,
                "data": dataArray,
                columns: [
                    { data: 'DealNo' },
                    { data: 'ClientName' },
                    { data: 'PurchaseEntity' },
                    { data: 'PayingEntity' }
                ],
                columnDefs: [
                    {
                        targets: 3,
                        "width": "250px",
                        render: function (data, type, row, meta) {
                            if (data == null)
                                return '<input id="payent_payingentity_' + meta.row + '" class="form-control" style="width:250px;" />';
                            else
                                return '<input id="payent_payingentity_' + meta.row + '" class="form-control" style="width:250px;" value="' + data + '" />';
                        }
                    }
                ],

                initComplete: function () {
                    $('#load1').hide();

                },
                buttons: [
                    {
                        extend: 'excelHtml5', title: 'Pending Deals', autoFilter: true,
                    },
                ],

            });
        },
        error: function (error) {
            CanopyUI.showError(error, 'Unable to complete request.');
        }
    });
    return false;
}


function payent_verifysubmit() {
    var index = 0;
    var params;

    payent_table.rows().every(function (index, element) {
        var ids = document.getElementById("payent_payingentity_" + index);
        if (ids.value != "") {
            var dealno = payent_table.row(index).data().DealNo;
            var client = payent_table.row(index).data().ClientName;
            var purchaseentity = payent_table.row(index).data().PurchaseEntity;
            if (index == 0)
                params = dealno + '~' + client + '~' + purchaseentity + '~' + ids.value;
            else
                params = params + ':' + dealno + '~' + client + '~' + purchaseentity + '~' + ids.value;
        }
    });
    if (params != "") {
        PageMethods.UpdatePayingEntity(params, payent_OnSuccess, payent_OnError);
        return false;
    }
    return false;
}

function payent_OnSuccess(result) {
    alert("Details updated successfully");
    payent_bindPendingGrid();
    return false;
}
function payent_OnError(error) {
    CanopyUI.showError(error, 'Unable to complete request.');
}