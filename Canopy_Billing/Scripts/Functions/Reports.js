var cbdr_ddsummary;
var cbdr_dddetails;
var cbdr_nonddsummary;
var cbdr_nondddetails;
var cbdr_canopysummary;
var cbdr_canopydetails;

var cprr_userwise;
var cprr_domainwise;
var cprr_projectwise;

function blankForNull(s) {
    return s == "null" || s == null ? "" : s;
}

function diff_bindyear() {
    var start = new Date().getFullYear();

    var select = document.getElementById("diff_year");
    let options = select.getElementsByTagName('option');

    for (var i = options.length; i--;) {
        select.removeChild(options[i]);
    }

    $("#diff_year").append($("<option></option>").val("").html("Select"));
    for (var i = start; i > start - 5; i--) {
        $("#diff_year").append($("<option></option>").val(i).html(i));
    }
}

function cppr_bindyear() {
    var start = new Date().getFullYear();

    var select = document.getElementById("cppr_year");
    let options = select.getElementsByTagName('option');

    for (var i = options.length; i--;) {
        select.removeChild(options[i]);
    }

    $("#cppr_year").append($("<option></option>").val("").html("Select"));
    for (var i = start; i > start - 5; i--) {
        $("#cppr_year").append($("<option></option>").val(i).html(i));
    }
}

function cbs_bindyear() {
    var start = new Date().getFullYear();

    var select = document.getElementById("cbs_year");
    let options = select.getElementsByTagName('option');

    for (var i = options.length; i--;) {
        select.removeChild(options[i]);
    }

    $("#cbs_year").append($("<option></option>").val("").html("Select"));
    for (var i = start; i > start - 5; i--) {
        $("#cbs_year").append($("<option></option>").val(i).html(i));
    }
}

function apbr_bindgrid() {
    var fromdate = document.getElementById("apbr_fromdate").value;
    var todate = document.getElementById("apbr_todate").value;
    $('#load1').show();
    $.ajax({
        url: "AllProjectBillingReport.aspx/GetAllProjectBilingReport",
        type: "POST",
        dataType: "json",
        data: "{FromDate:'" + fromdate + "',ToDate:'" + todate + "'}",
        contentType: "application/json; charset=utf-8",

        success: function (data) {
            if ($.fn.dataTable.isDataTable('#apbr_table')) {
                $('#apbr_table').DataTable().destroy();
            }
            dataArray = JSON.parse(data.d);
            $('#apbr_table').DataTable({
                dom: 'Btp',
                scrollX: true,
                destroy: true,
                paging: false,
                "autoWidth": true,
                select: true,
                fixedHeader: true,
                processing: true,
                "aaSorting": [],
                'select': {
                    'style': 'single'
                },
                "data": dataArray,
                columns: [
                    { data: 'Domain' },
                    { data: 'Subdomain' },
                    { data: 'ProjectName' },
                    { data: 'BillingPeriod' },
                    { data: 'InvoiceNo' },
                    { data: 'OrderCount' },
                    { data: 'TotalAmount' },
                    { data: 'InvoiceGeneratedOn' }
                ],
                fnCreatedRow: function (nRow, aData, iDataIndex) {

                    $(nRow).children("td").css("text-align", "center");
                },

                initComplete: function () {
                    $('#load1').hide();
                },
                buttons: [
                    {
                        extend: 'excelHtml5', title: 'All project Billing Report', autoFilter: true,
                    },
                ],

            });

        }
    });

    return false;
}

function cbdr_bindDDSummmarygrid() {
    $('#load1').show();
    var columns = [];
    var FromDate = document.getElementById("cbdr_fromdate").value;
    var ToDate = document.getElementById("cbdr_todate").value;
    $.ajax({
        url: "ClientBillingDeviationReport.aspx/GetDDSummary",
        type: "POST",
        dataType: "json",
        data: "{FromDate:'" + FromDate + "',ToDate:'" + ToDate + "'}",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//
            $.each(dataArray[0], function (key, value) {

                var my_item = {};
                my_item.data = key;
                my_item.title = key;
                columns.push(my_item);
            });

            cbdr_ddsummary = $('#cbdr_ddsummary').DataTable({
                dom: 'Bftip',
                destroy: true,
                orderCellsTop: true,
                fixedColumns: {
                    leftColumns: 2,
                },
                fixedHeader: true,
                scrollCollapse: true,
                scrollX: true,
                "paging": false,
                "autoWidth": true,
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
                    $(nRow).children("td").css("text-wrap", "nowrap");
                    $(nRow).children("td").css("text-align", "center");
                },
                columnDefs: [
                    { className: "dt-center", targets: "_all" } // Centers all columns
                ],


                initComplete: function () {
                    $('#load1').hide();

                },
                buttons: [
                    {
                        extend: 'excelHtml5', title: 'IPS DD - Summary', autoFilter: true,


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

function cbdr_bindDDDetailsgrid() {
    $('#load1').show();
    var columns = [];
    var FromDate = document.getElementById("cbdr_fromdate").value;
    var ToDate = document.getElementById("cbdr_todate").value;
    $.ajax({
        url: "ClientBillingDeviationReport.aspx/GetDDDetails",
        type: "POST",
        dataType: "json",
        data: "{FromDate:'" + FromDate + "',ToDate:'" + ToDate + "'}",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//
            $.each(dataArray[0], function (key, value) {

                var my_item = {};
                my_item.data = key;
                my_item.title = key;
                columns.push(my_item);
            });

            cbdr_dddetails = $('#cbdr_dddetails').DataTable({
                dom: 'Bftip',
                destroy: true,
                orderCellsTop: true,
                fixedColumns: {
                    leftColumns: 3,
                },
                fixedHeader: true,
                scrollCollapse: true,
                scrollX: true,
                "paging": true,
                pageLength: 10,
                "autoWidth": true,
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
                    $(nRow).children("td").css("text-align", "center");
                },
                columnDefs: [
                    { className: "dt-center", targets: "_all" } // Centers all columns
                ],


                initComplete: function () {
                    $('#load1').hide();

                },
                buttons: [
                    {
                        extend: 'excelHtml5', title: 'IPS DD - Parameter wise', autoFilter: true,


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

function cbdr_bindNonDDSummmarygrid() {
    $('#load1').show();
    var columns = [];
    var FromDate = document.getElementById("cbdr_fromdate").value;
    var ToDate = document.getElementById("cbdr_todate").value;
    $.ajax({
        url: "ClientBillingDeviationReport.aspx/GetNonDDSummary",
        type: "POST",
        dataType: "json",
        data: "{FromDate:'" + FromDate + "',ToDate:'" + ToDate + "'}",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//
            $.each(dataArray[0], function (key, value) {

                var my_item = {};
                my_item.data = key;
                my_item.title = key;
                columns.push(my_item);
            });

            cbdr_nonddsummary = $('#cbdr_nonddsummary').DataTable({
                dom: 'Bftip',
                destroy: true,
                orderCellsTop: true,
                fixedColumns: {
                    leftColumns: 2,
                },
                fixedHeader: true,
                scrollCollapse: true,
                scrollX: true,
                "paging": false,
                "autoWidth": true,
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
                    $(nRow).children("td").css("text-wrap", "nowrap");
                    $(nRow).children("td").css("text-align", "center");
                },
                columnDefs: [
                    { className: "dt-center", targets: "_all" } // Centers all columns
                ],


                initComplete: function () {
                    $('#load1').hide();

                },
                buttons: [
                    {
                        extend: 'excelHtml5', title: 'IPS - Non DD - Summary', autoFilter: true,


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

function cbdr_bindNonDDDetailsgrid() {
    $('#load1').show();
    var columns = [];
    var FromDate = document.getElementById("cbdr_fromdate").value;
    var ToDate = document.getElementById("cbdr_todate").value;
    $.ajax({
        url: "ClientBillingDeviationReport.aspx/GetNonDDDetails",
        type: "POST",
        dataType: "json",
        data: "{FromDate:'" + FromDate + "',ToDate:'" + ToDate + "'}",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//
            $.each(dataArray[0], function (key, value) {

                var my_item = {};
                my_item.data = key;
                my_item.title = key;
                columns.push(my_item);
            });

            cbdr_nondddetails = $('#cbdr_nondddetails').DataTable({
                dom: 'Bftip',
                destroy: true,
                orderCellsTop: true,
                fixedColumns: {
                    leftColumns: 2,
                },
                fixedHeader: true,
                scrollCollapse: true,
                scrollX: true,
                "paging": false,
                "autoWidth": true,
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
                    $(nRow).children("td").css("text-wrap", "nowrap");
                    $(nRow).children("td").css("text-align", "center");
                },
                columnDefs: [
                    { className: "dt-center", targets: "_all" } // Centers all columns
                ],


                initComplete: function () {
                    $('#load1').hide();

                },
                buttons: [
                    {
                        extend: 'excelHtml5', title: 'IPS - Non DD- Parameter wise', autoFilter: true,


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

function cbdr_bindCanopySummmarygrid() {
    $('#load1').show();
    var columns = [];
    var FromDate = document.getElementById("cbdr_fromdate").value;
    var ToDate = document.getElementById("cbdr_todate").value;
    $.ajax({
        url: "ClientBillingDeviationReport.aspx/GetCanopySummary",
        type: "POST",
        dataType: "json",
        data: "{FromDate:'" + FromDate + "',ToDate:'" + ToDate + "'}",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//
            $.each(dataArray[0], function (key, value) {

                var my_item = {};
                my_item.data = key;
                my_item.title = key;
                columns.push(my_item);
            });

            cbdr_canopysummary = $('#cbdr_canopysummary').DataTable({
                dom: 'Bftip',
                destroy: true,
                orderCellsTop: true,
                fixedColumns: {
                    leftColumns: 2,
                },
                fixedHeader: true,
                scrollCollapse: true,
                scrollX: true,
                "paging": false,
                "autoWidth": true,
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
                    $(nRow).children("td").css("text-wrap", "nowrap");
                    $(nRow).children("td").css("text-align", "center");
                },
                columnDefs: [
                    { className: "dt-center", targets: "_all" } // Centers all columns
                ],


                initComplete: function () {
                    $('#load1').hide();

                },
                buttons: [
                    {
                        extend: 'excelHtml5', title: 'Canopy Summary', autoFilter: true,


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

function cbdr_bindCanopyDetailsgrid() {
    $('#load1').show();
    var columns = [];
    var FromDate = document.getElementById("cbdr_fromdate").value;
    var ToDate = document.getElementById("cbdr_todate").value;
    $.ajax({
        url: "ClientBillingDeviationReport.aspx/GetCanopyDetails",
        type: "POST",
        dataType: "json",
        data: "{FromDate:'" + FromDate + "',ToDate:'" + ToDate + "'}",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//
            $.each(dataArray[0], function (key, value) {

                var my_item = {};
                my_item.data = key;
                my_item.title = key;
                columns.push(my_item);
            });

            cbdr_canopydetails = $('#cbdr_canopydetails').DataTable({
                dom: 'Bftip',
                destroy: true,
                orderCellsTop: true,
                fixedColumns: {
                    leftColumns: 2,
                },
                fixedHeader: true,
                scrollCollapse: true,
                scrollX: true,
                "paging": false,
                "autoWidth": true,
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
                    $(nRow).children("td").css("text-wrap", "nowrap");
                    $(nRow).children("td").css("text-align", "center");
                },
                columnDefs: [
                    { className: "dt-center", targets: "_all" } // Centers all columns
                ],


                initComplete: function () {
                    $('#load1').hide();

                },
                buttons: [
                    {
                        extend: 'excelHtml5', title: 'Canopy - Parameter wise', autoFilter: true,


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

function billingdifference_bindgrid() {
    var ddlmonth = document.getElementById("diff_month");
    var month = ddlmonth.options[ddlmonth.selectedIndex].value;
    var ddlyear = document.getElementById("diff_year");
    var year = ddlyear.options[ddlyear.selectedIndex].value;
    $('#load1').show();
    $.ajax({
        url: "BillingDifference.aspx/GetBillingDifference",
        type: "POST",
        dataType: "json",
        data: "{Month:'" + month + "',Year:'" + year + "'}",
        contentType: "application/json; charset=utf-8",

        success: function (data) {
            if ($.fn.dataTable.isDataTable('#diff_table')) {
                $('#diff_table').DataTable().destroy();
            }
            dataArray = JSON.parse(data.d);
            $('#diff_table').DataTable({
                dom: 'Btp',
                scrollX: true,
                destroy: true,
                paging: false,
                "autoWidth": true,
                select: true,
                fixedHeader: true,
                processing: true,
                "aaSorting": [],
                'select': {
                    'style': 'single'
                },
                "data": dataArray,
                columns: [
                    { data: 'DomainName' },
                    { data: 'ProjectName' },
                    { data: 'BillingPeriod' },
                    { data: 'AutoCount' },
                    { data: 'ManualCount' },
                    { data: 'CountDifference' }
                ],
                fnCreatedRow: function (nRow, aData, iDataIndex) {
                    $(nRow).children("td").css("text-wrap", "nowrap");
                    $(nRow).children("td").css("text-align", "center");
                },
                columnDefs: [{ visible: false, targets: 0 }],
                drawCallback: function (settings) {
                    var api = this.api();
                    var rows = api.rows({ page: 'current' }).nodes();
                    var last = null;

                    var subTotal = new Array();
                    var groupID = -1;
                    var aData = new Array();
                    var index = 0;

                    api.column(0, { page: 'current' }).data().each(function (group, i) {

                        // CanopyUI.debug(group+">>>"+i);

                        var vals = api.row(api.row($(rows).eq(i)).index()).data();
                        var autocount = vals.AutoCount ? parseInt(vals.AutoCount) : 0;
                        var manualcount = vals.ManualCount ? parseInt(vals.ManualCount) : 0;
                        var diff = vals.CountDifference ? parseInt(vals.CountDifference) : 0;

                        if (typeof aData[group] == 'undefined') {
                            aData[group] = new Array();
                            aData[group].rows = [];
                            aData[group].autocount = [];
                            aData[group].manualcount = [];
                            aData[group].diff = [];
                        }

                        aData[group].rows.push(i);
                        aData[group].autocount.push(autocount);
                        aData[group].manualcount.push(manualcount);
                        aData[group].diff.push(diff);

                    });
                    var idx = 0;


                    for (var office in aData) {

                        idx = Math.max.apply(Math, aData[office].rows);

                        var sum = 0;
                        var summanual = 0;
                        var sumdiff = 0;
                        $.each(aData[office].autocount, function (k, v) {
                            sum = sum + v;
                        });
                        $.each(aData[office].manualcount, function (k, v) {
                            summanual = summanual + v;
                        });
                        $.each(aData[office].diff, function (k, v) {
                            sumdiff = sumdiff + v;
                        });
                        CanopyUI.debug(aData[office].autocount);
                        $(rows).eq(idx).after(
                            '<tr class="group" style="border-top:Solid 1px black;border-bottom:Solid 1px black; font-size:14px;"><td colspan="2" style="font-weight:bold;border-top:Solid 1px black;border-bottom:Solid 1px black;">' + office + '</td>' +
                            '<td style="text-align:center;font-weight:bold;border-top:Solid 1px black;border-bottom:Solid 1px black;">' + sum + '</td><td style="text-align:center;font-weight:bold;border-top:Solid 1px black;border-bottom:Solid 1px black;">' + summanual + '</td><td style="border-top:Solid 1px black;border-bottom:Solid 1px black;text-align:center;font-weight:bold;">' + sumdiff + '</td></tr>'
                        );

                    };
                },

                initComplete: function () {
                    $('#load1').hide();
                },
                buttons: [
                    {
                        extend: 'excelHtml5', title: 'Auto vs manual billing difference', autoFilter: true,
                    },
                ],

            });

        }
    });

    return false;
}


function cprr_bindUserGrid() {
    $('#load1').show();
    var columns = [];
    var ddlmonth = document.getElementById("cppr_month");
    var month = ddlmonth.options[ddlmonth.selectedIndex].value;
    var ddlyear = document.getElementById("cppr_year");
    var year = ddlyear.options[ddlyear.selectedIndex].value;
    $.ajax({
        url: "CostPerRecordReport.aspx/GetCostPerRecord_Userwise",
        type: "POST",
        dataType: "json",
        data: "{Month:'" + month + "',Year:'" + year + "'}",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//
            $.each(dataArray[0], function (key, value) {

                var my_item = {};
                my_item.data = key;
                my_item.title = key;
                columns.push(my_item);
            });
            if ($.fn.dataTable.isDataTable('#cprr_userwise')) {
                $('#cprr_userwise').DataTable().destroy();
            }
            cprr_userwise = $('#cprr_userwise').DataTable({
                dom: 'Bftip',
                destroy: true,
                orderCellsTop: true,
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
                    $(nRow).children("td").css("text-wrap", "nowrap");
                },

                initComplete: function () {
                    $('#load1').hide();

                },
                buttons: [
                    {
                        extend: 'excelHtml5', title: 'Userwise Cost Per Record', autoFilter: true,


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

function cprr_bindDomainGrid() {
    $('#load1').show();
    var columns = [];
    var ddlmonth = document.getElementById("cppr_month");
    var month = ddlmonth.options[ddlmonth.selectedIndex].value;
    var ddlyear = document.getElementById("cppr_year");
    var year = ddlyear.options[ddlyear.selectedIndex].value;
    $.ajax({
        url: "CostPerRecordReport.aspx/GetCostPerRecord_Domain",
        type: "POST",
        dataType: "json",
        data: "{Month:'" + month + "',Year:'" + year + "'}",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//
            $.each(dataArray[0], function (key, value) {
                var my_item = {};
                my_item.data = key;
                my_item.title = key;
                columns.push(my_item);
            });
            if ($.fn.dataTable.isDataTable('#cprr_domainwise')) {
                $('#cprr_domainwise').DataTable().destroy();
            }
            cprr_domainwise = $('#cprr_domainwise').DataTable({
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
                        extend: 'excelHtml5', title: 'Domainwise Cost Per Record', autoFilter: true,
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

function cprr_bindProjectGrid() {
    $('#load1').show();
    var columns = [];
    var ddlmonth = document.getElementById("cppr_month");
    var month = ddlmonth.options[ddlmonth.selectedIndex].value;
    var ddlyear = document.getElementById("cppr_year");
    var year = ddlyear.options[ddlyear.selectedIndex].value;
    $.ajax({
        url: "CostPerRecordReport.aspx/GetCostPerRecord_Project",
        type: "POST",
        dataType: "json",
        data: "{Month:'" + month + "',Year:'" + year + "'}",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//
            $.each(dataArray[0], function (key, value) {

                var my_item = {};
                my_item.data = key;
                my_item.title = key;
                columns.push(my_item);
            });
            if ($.fn.dataTable.isDataTable('#cprr_projectwise')) {
                $('#cprr_projectwise').DataTable().destroy();
            }
            cprr_projectwise = $('#cprr_projectwise').DataTable({
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
                        extend: 'excelHtml5', title: 'Projectwise Cost Per Record', autoFilter: true,


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

function cbs_bindGrid() {
    var ddlmonth = document.getElementById("cbs_month");
    var month = ddlmonth.options[ddlmonth.selectedIndex].value;
    var ddlyear = document.getElementById("cbs_year");
    var year = ddlyear.options[ddlyear.selectedIndex].value;
    $('#load1').show();
    $.ajax({
        url: "ClientBillingSummary.aspx/GetBillingSummary",
        type: "POST",
        dataType: "json",
        data: "{Month:'" + month + "',Year:'" + year + "'}",
        contentType: "application/json; charset=utf-8",

        success: function (data) {
            if ($.fn.dataTable.isDataTable('#cbs_table')) {
                $('#cbs_table').DataTable().destroy();
            }
            dataArray = JSON.parse(data.d);
            $('#cbs_table').DataTable({
                dom: 'Bftp',
                scrollX: true,
                destroy: true,
                paging: false,
                "autoWidth": true,
                select: true,
                fixedHeader: true,
                processing: true,
                "aaSorting": [],
                'select': {
                    'style': 'single'
                },
                "data": dataArray,
                columns: [
                    { data: 'DealNo' },
                    { data: 'ClientName' },
                    { data: 'ProcessName' },
                    { data: 'Description' },
                    { data: 'Loan Count' },
                    { data: 'Price/Loan' },
                    { data: 'Total Amount' }
                ],
                fnCreatedRow: function (nRow, aData, iDataIndex) {

                    $(nRow).children("td").css("text-align", "center");
                },

                initComplete: function () {
                    $('#load1').hide();
                },
                buttons: [
                    {
                        extend: 'excelHtml5', title: 'Client Billing Summary', autoFilter: true,
                    },
                ],

            });

        }
    });

    return false;
}

function sleep(seconds) {
    var e = new Date().getTime() + (seconds * 1000);
    while (new Date().getTime() <= e) { }
}

function dashboard_arBind2() {
    $('#load1').show();
    alert('1');
    $('#dashboard_ar').DataTable({
        dom: 'Bftip',
        //destroy: true,
        orderCellsTop: true,
        fixedHeader: true,
        scrollX: true,
        "paging": false,
        "autoWidth": true,
        select: true,
        "ordering": false,
        filter: true,
        'select': {
            'style': 'single'
        },
        "serverSide": false,

        initComplete: function () {
            $('#load1').hide();
        }

    });


    return false;
}

function dashboard_arBind() {
    $('#load1').show();
    var columns = [];
    var columnlink = [];
    var preheader = '';
    var headers = [];
    var returnstring = '';
    var htmlheader = '';
    var htmlrow = '';
    $.ajax({
        url: "Dashboard.aspx/GetClientwiseDashboard",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//
            var firstData = dataArray[0];
            var rowData = dataArray[1];
            var ColumnNames = Object.keys(firstData);
            var rowValues = Object.keys(rowData);

            htmlheader = '<thead><tr>';
            var thead = document.createElement("thead");
            var trh = document.createElement("tr");
            for (var i = 1; i < ColumnNames.length; i++) {
                if (i == 2 || i == 3) {
                    var tbh = document.createElement("th");
                    tbh.rowSpan = 2;
                    tbh.style.fontWeight = "bold!important";
                    tbh.style.verticalAlign = "middle";
                    var label = document.createElement("label");
                    label.id = i + "_ClientLabel";
                    //label.innerHTML = "Client";
                    label.style.fontWeight = "bold!important";
                    label.style.width = "250px";
                    tbh.style.left = "0px!important";
                    //tbh.appendChild(label);
                    tbh.innerHTML = ColumnNames[i];
                    trh.appendChild(tbh);
                    //thead.appendChild(trh);
                    //htmlheader = htmlheader + '<th style="left: 0px!important;">Client</th>';
                }
                else if (i > 1) {
                    if (i % 2 != 0) {
                        var tbh = document.createElement("th");
                        tbh.colSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.innerHTML = ColumnNames[i].replace("-LoanCount", "").replace("-Amount", "");
                        trh.appendChild(tbh);
                        //thead.appendChild(trh);
                        //htmlheader = htmlheader + '<th colspan=2 style="padding-left:20px;left: 0px!important; text-align:center;">' + ColumnNames[i].replace("-LoanCount", "").replace("-Amount", "") + '</th>';
                        i = i + 1;
                    }
                    //else
                    //    htmlheader = htmlheader + '<th style="padding-left:20px;left: 0px!important; text-align:center;">' + ColumnNames[i] + '</th>';
                }
                else {
                    var tbh = document.createElement("th");
                    tbh.style.display = 'none';
                    tbh.innerHTML = ColumnNames[i];
                    //htmlheader = htmlheader + '<th style="display:none;">' + ColumnNames[i] + '</th>';
                    trh.appendChild(tbh);
                    //thead.appendChild(trh);
                }
            }
            thead.appendChild(trh);
            $('#dashboard_ar_update').append(thead);
            var trh1 = document.createElement("tr");
            //htmlheader = htmlheader + '</tr><tr>';
            for (var i = 1; i < ColumnNames.length; i++) {
                if (i == 2 || i == 3) {
                    //var tbr = document.createElement("th");
                    //tbr.innerHTML = ColumnNames[i];
                    //tbr.style.textAlign = "center";
                    //trh1.appendChild(tbr);
                    ////htmlheader = htmlheader + '<th style="left: 0px!important; ">' + ColumnNames[i] + '</th>';
                }
                else if (i > 1)
                    if (i % 2 != 0) {
                        var tbr = document.createElement("th");
                        tbr.style.textAlign = "center";
                        tbr.innerHTML = 'Amount';
                        //htmlheader = htmlheader + '<th style="padding-left:20px;left: 0px!important; text-align:center;">Amount</th>';
                        trh1.appendChild(tbr);
                        //thead.appendChild(trh1);
                    }
                    else {
                        var tbr = document.createElement("th");
                        tbr.style.textAlign = "center";
                        tbr.innerHTML = 'Loan Count';
                        //htmlheader = htmlheader + '<th style="padding-left:20px;left: 0px!important; text-align:center;">Loan Count</th>';
                        trh1.appendChild(tbr);
                        //thead.appendChild(trh1);
                    }
                else {
                    var tbr = document.createElement("th");
                    tbr.style.display = 'none';
                    tbr.innerHTML = ColumnNames[i];
                    //htmlheader = htmlheader + '<th style="display:none;">' + ColumnNames[i] + '</th>';
                    trh1.appendChild(tbr);
                }
            }
            thead.appendChild(trh1);
            $('#dashboard_ar_update').append(thead);
            var tbody = document.createElement("tbody");
            //$('#dashboard_ar').html("");
            //htmlheader = htmlheader + '</tr></thead>';
            //htmlrow = '<tbody>';
            $.each(dataArray, function (index, item) {
                var tr1 = document.createElement("tr");
                //htmlrow = htmlrow + '<tr>';
                for (var i = 1; i < ColumnNames.length; i++) {
                    var targetColumnName = ColumnNames[i];
                    if (i == 2 || i == 3) {
                        var td = document.createElement("td");
                        //var label = document.createElement("label");
                        //label.id = i + "_" + item[targetColumnName].replace(" ", "") + "ClientLabel";
                        td.innerHTML = item[targetColumnName];
                        //label.style.width = "250px";
                        //td.appendChild(label);
                        tr1.appendChild(td);
                    }
                    else if (i > 1) {
                        if (i % 2 != 0) {
                            var td = document.createElement("td");
                            td.style.textAlign = "center";
                            td.innerHTML = '<input type="text" id="loancount_' + targetColumnName + '_' + index + '" onchange="return getValue(this,' + index + ');" style="width:70px;" value="' + blankForNull(item[targetColumnName]) + '"></input>';
                            tr1.appendChild(td);
                        }
                        else {
                            var td = document.createElement("td");
                            td.style.textAlign = "center";
                            td.innerHTML = '<input type="text" id="loancount_' + targetColumnName + '_' + index + '" onchange="return getValue(this,' + index + ');" style="width:50px;" value="' + blankForNull(item[targetColumnName]) + '"></input>';
                            tr1.appendChild(td);
                        }
                    }
                    else {
                        var td = document.createElement("td");
                        td.style.display = 'none';
                        td.innerHTML = item[targetColumnName];
                        tr1.appendChild(td);
                    }
                }
                tbody.appendChild(tr1);
                $('#dashboard_ar_update').append(tbody);
            });

            $('#dashboard_ar_update').DataTable({
                dom: 't',
                destroy: true,
                orderCellsTop: true,
                fixedHeader: true,
                fixedColumns: {
                    leftColumns: 2,
                },
                scrollX: true,
                scrollY: '400px',
                "paging": false,
                "autoWidth": true,
                select: true,
                "ordering": false,
                filter: true,
                'select': {
                    'style': 'single'
                },
                "serverSide": false,

                initComplete: function () {
                    $('#load1').hide();
                }

            });
        },
        error: function (error) {
            CanopyUI.showError(error, 'Unable to complete request.');
        }
    });
    return false;
}

function dashboard_arBind_displayERP() {
    $('#load1').show();
    var columns = [];
    var columnlink = [];
    var preheader = '';
    var headers = [];
    var returnstring = '';
    var htmlheader = '';
    var htmlrow = '';
    $.ajax({
        url: "Dashboard.aspx/GetClientwiseDashboard_ERpData",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//
            var firstData = dataArray[0];
            var rowData = dataArray[1];
            var ColumnNames = Object.keys(firstData);
            var rowValues = Object.keys(rowData);

            htmlheader = '<thead><tr>';
            var thead = document.createElement("thead");
            var trh = document.createElement("tr");
            for (var i = 1; i < ColumnNames.length; i++) {
                if (i == 2 || i == 3) {
                    var tbh = document.createElement("th");
                    tbh.rowSpan = 2;
                    tbh.style.fontWeight = "bold!important";
                    tbh.style.verticalAlign = "middle";
                    tbh.id = i + "_ClientLabel";
                    tbh.style.left = "0px!important";
                    //tbh.appendChild(label);
                    tbh.innerHTML = ColumnNames[i];
                    trh.appendChild(tbh);

                    //thead.appendChild(trh);
                    //htmlheader = htmlheader + '<th style="left: 0px!important;">Client</th>';
                }
                else if (i > 1) {
                    if (i % 2 == 0) {
                        var tbh = document.createElement("th");
                        tbh.colSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.innerHTML = ColumnNames[i].replace("-ERP-LoanCount", "").replace("-ERP-Amount", "");
                        trh.appendChild(tbh);
                        //thead.appendChild(trh);
                        //htmlheader = htmlheader + '<th colspan=2 style="padding-left:20px;left: 0px!important; text-align:center;">' + ColumnNames[i].replace("-LoanCount", "").replace("-Amount", "") + '</th>';
                        i = i + 1;
                    }
                    //else
                    //    htmlheader = htmlheader + '<th style="padding-left:20px;left: 0px!important; text-align:center;">' + ColumnNames[i] + '</th>';
                }
                else {
                    var tbh = document.createElement("th");
                    //tbh.style.display = 'none';
                    tbh.innerHTML = ColumnNames[i];
                    //htmlheader = htmlheader + '<th style="display:none;">' + ColumnNames[i] + '</th>';
                    trh.appendChild(tbh);
                    //thead.appendChild(trh);
                }
            }
            thead.appendChild(trh);
            $('#dashboard_ar').append(thead);
            var trh1 = document.createElement("tr");
            //htmlheader = htmlheader + '</tr><tr>';
            for (var i = 1; i < ColumnNames.length; i++) {
                if (i == 2 || i == 3) {
                    //var tbr = document.createElement("th");
                    //tbr.innerHTML = ColumnNames[i];
                    //tbr.style.textAlign = "center";
                    //trh1.appendChild(tbr);
                    ////htmlheader = htmlheader + '<th style="left: 0px!important; ">' + ColumnNames[i] + '</th>';
                }
                else if (i > 1)
                    if (i % 2 != 0) {
                        var tbr = document.createElement("th");
                        tbr.style.textAlign = "center";
                        tbr.style.verticalAlign = "middle";
                        tbr.innerHTML = 'US $';
                        tbr.id = "lbl_" + (i - 1);
                        trh1.appendChild(tbr);
                    }
                    else {
                        var tbr = document.createElement("th");
                        tbr.style.textAlign = "center";
                        tbr.style.verticalAlign = "middle";
                        tbr.innerHTML = 'Count';
                        tbr.id = "lbl_" + (i - 1);
                        trh1.appendChild(tbr);
                    }
                else {
                    var tbr = document.createElement("th");
                    //tbr.style.display = 'none';
                    tbr.innerHTML = ColumnNames[i];

                    //htmlheader = htmlheader + '<th style="display:none;">' + ColumnNames[i] + '</th>';
                    trh1.appendChild(tbr);
                }
            }
            thead.appendChild(trh1);
            $('#dashboard_ar').append(thead);
            var tbody = document.createElement("tbody");
            //$('#dashboard_ar').html("");
            //htmlheader = htmlheader + '</tr></thead>';
            //htmlrow = '<tbody>';
            $.each(dataArray, function (index, item) {
                var tr1 = document.createElement("tr");
                //htmlrow = htmlrow + '<tr>';
                for (var i = 1; i < ColumnNames.length; i++) {
                    var targetColumnName = ColumnNames[i];
                    if (i == 2 || i == 3) {
                        var td = document.createElement("td");
                        var label = document.createElement("label");
                        label.id = i + "_" + item[targetColumnName].replace(" ", "") + "ClientLabel";
                        label.innerHTML = item[targetColumnName];
                        if (ColumnNames[i] == "Process")
                            label.style.width = "180px";
                        else
                            label.style.width = "250px";
                        /*label.style.width = "250px";*/
                        td.appendChild(label);
                        tr1.appendChild(td);
                    }
                    else if (i > 1) {
                        //if (i % 2 == 0) {
                        var td = document.createElement("td");
                        td.style.textAlign = "center";
                        //if (targetColumnName].includes("Amount")) {
                        //    td.innerHTML = "" + formattedAmount.toLocaleString("en-US", { style: "currency", currency: "USD" });
                        //}
                        //else
                        if (item[targetColumnName] != null)
                            td.innerHTML = new Intl.NumberFormat().format(Number(item[targetColumnName]))
                        else
                            td.innerHTML = item[targetColumnName];
                        //td.innerHTML = '<input type="text" id="loancount_' + targetColumnName + '_' + index + '" onchange="return getValue(this,' + index + ');" style="width:70px;" value="' + blankForNull(item[targetColumnName]) + '"></input>';
                        tr1.appendChild(td);
                        //}
                        //else {
                        //    var td = document.createElement("td");
                        //    td.style.textAlign = "center";
                        //    td.innerHTML = '<input type="text" id="loancount_' + targetColumnName + '_' + index + '" onchange="return getValue(this,' + index + ');" style="width:50px;" value="' + blankForNull(item[targetColumnName]) + '"></input>';
                        //    tr1.appendChild(td);
                        //}
                    }
                    else {
                        var td = document.createElement("td");
                        //td.style.display = 'none';
                        td.innerHTML = item[targetColumnName];
                        tr1.appendChild(td);
                    }
                }
                tbody.appendChild(tr1);
                $('#dashboard_ar').append(tbody);
            });


            //Footer
            var tfoot = document.createElement("tfoot");
            var trf1 = document.createElement("tr");
            for (var i = 1; i < ColumnNames.length; i++) {
                if (i == 2 || i == 3) {
                    var tbr = document.createElement("th");
                    tbr.style.textAlign = "center";
                    trf1.appendChild(tbr);
                }
                else if (i > 1) {
                    var tbr = document.createElement("th");
                    tbr.style.textAlign = "center";
                    trf1.appendChild(tbr);
                }
                else {
                    var tbr = document.createElement("th");
                    // tbr.style.display = 'none';
                    trf1.appendChild(tbr);
                }
            }
            tfoot.appendChild(trf1);
            $('#dashboard_ar').append(tfoot);

            $('#dashboard_ar').DataTable({
                dom: 'Bft',
                //destroy: true,
                orderCellsTop: true,
                fixedColumns: {
                    leftColumns: 2,
                },
                scrollCollapse: false,
                scrollY: '400px',
                scrollX: true,
                "paging": false,
                "autoWidth": true,
                select: true,
                "ordering": false,
                filter: true,
                'select': {
                    'style': 'single'
                },
                "serverSide": false,

                initComplete: function () {
                    $('#load1').hide();
                },
                columnDefs: [
                    { targets: 0, visible: false }  // hides column index 2
                ],
                buttons: [
                    {
                        extend: 'excelHtml5',
                        text: 'Export to Excel',
                        filename: 'Monthwise Summary',
                        exportOptions: {
                            columns: ':visible',
                            modifier: { header: false }
                        },
                        customize: function (xlsx) {
                            const sheetDoc = xlsx.xl.worksheets['sheet1.xml'];
                            const worksheet = sheetDoc.documentElement;
                            const sheetData = worksheet.getElementsByTagName('sheetData')[0];
                            const $worksheet = $(worksheet);


                            // Remove all existing rows (clean slate)
                            $worksheet.find('row').remove();

                            // Remove existing mergeCells if any
                            $worksheet.find('mergeCells').remove();

                            function colLetter(n) {
                                let s = '', t;
                                while (n > 0) {
                                    t = (n - 1) % 26;
                                    s = String.fromCharCode(65 + t) + s;
                                    n = Math.floor((n - 1) / 26);
                                }
                                return s;
                            }

                            const occupied = {};
                            const mergeRanges = [];

                            const stylesDoc = xlsx.xl['styles.xml'];
                            const fonts = stylesDoc.getElementsByTagName('fonts')[0];
                            const borders = stylesDoc.getElementsByTagName('borders')[0];
                            const cellXfs = stylesDoc.getElementsByTagName('cellXfs')[0];

                            // --- Create bold font for header
                            const headerFont = stylesDoc.createElement('font');
                            const bold = stylesDoc.createElement('b');
                            const color = stylesDoc.createElement('color');
                            color.setAttribute('rgb', 'FF000000'); // black
                            headerFont.appendChild(bold);
                            headerFont.appendChild(color);
                            fonts.appendChild(headerFont);

                            const globalFont = stylesDoc.createElement('font');

                            const globalFontName = stylesDoc.createElement('name');
                            globalFontName.setAttribute('val', 'biome');
                            headerFont.appendChild(globalFontName);

                            const globalFontSize = stylesDoc.createElement('sz');
                            globalFontSize.setAttribute('val', '8');
                            headerFont.appendChild(globalFontSize);

                            fonts.appendChild(headerFont);

                            const headerFontId = fonts.childNodes.length - 1;
                            fonts.setAttribute('count', fonts.childNodes.length.toString());

                            // --- Create border for all cells
                            const border = stylesDoc.createElement('border');
                            ['left', 'right', 'top', 'bottom'].forEach(side => {
                                const sideElem = stylesDoc.createElement(side);
                                sideElem.setAttribute('style', 'thin');
                                const colorElem = stylesDoc.createElement('color');
                                colorElem.setAttribute('auto', '1');
                                sideElem.appendChild(colorElem);
                                border.appendChild(sideElem);
                            });

                            borders.appendChild(border);
                            const borderId = borders.childNodes.length - 1;
                            borders.setAttribute('count', borders.childNodes.length.toString());

                            // add header color
                            const fills = stylesDoc.getElementsByTagName('fills')[0];

                            const headerFill = stylesDoc.createElement('fill');
                            const patternFill = stylesDoc.createElement('patternFill');
                            patternFill.setAttribute('patternType', 'solid');

                            const fgColor = stylesDoc.createElement('fgColor');
                            fgColor.setAttribute('rgb', 'ffe2efda'); // Yellow fill (ARGB format)

                            const bgColor = stylesDoc.createElement('bgColor');
                            bgColor.setAttribute('indexed', '64');

                            patternFill.appendChild(fgColor);
                            patternFill.appendChild(bgColor);

                            headerFill.appendChild(patternFill);
                            fills.appendChild(headerFill);

                            const headerFillId = fills.childNodes.length - 1;
                            fills.setAttribute('count', fills.childNodes.length.toString());


                            const headerStyle = stylesDoc.createElement('xf');
                            headerStyle.setAttribute('numFmtId', '0');
                            headerStyle.setAttribute('fontId', headerFontId.toString());
                            headerStyle.setAttribute('fillId', headerFillId.toString());
                            headerStyle.setAttribute('applyFill', '1');
                            headerStyle.setAttribute('xfId', '0');
                            headerStyle.setAttribute('applyFont', '1');
                            headerStyle.setAttribute('borderId', borderId.toString());
                            headerStyle.setAttribute('applyBorder', '1');
                            headerStyle.setAttribute('applyAlignment', '1');

                            // Add alignment child
                            const alignment = stylesDoc.createElement('alignment');
                            alignment.setAttribute('horizontal', 'center');
                            alignment.setAttribute('vertical', 'center');
                            alignment.setAttribute('wrapText', '1');
                            headerStyle.appendChild(alignment);

                            cellXfs.appendChild(headerStyle);

                            // Append the new xf
                            //cellXfs.appendChild(headerAlignXf);
                            const headerStyleIndex = cellXfs.childNodes.length - 1;
                            cellXfs.setAttribute('count', cellXfs.childNodes.length.toString());
                            // Start rowIndex at 1 to insert headers at the top
                            let rowIndex = 1;

                            // 1. Add header rows from thead
                            $('#dashboard_ar thead tr').each(function () {
                                let colIndex = 1;
                                const trElm = sheetDoc.createElement('row');
                                trElm.setAttribute('r', rowIndex);

                                $(this).children('th, td').each(function () {
                                    const $cell = $(this);
                                    const colspan = parseInt($cell.attr('colspan')) || 1;
                                    const rowspan = parseInt($cell.attr('rowspan')) || 1;

                                    while (occupied[rowIndex + '-' + colIndex]) {
                                        colIndex++;
                                    }

                                    const startCol = colIndex;
                                    const endCol = colIndex + colspan - 1;
                                    const endRow = rowIndex + rowspan - 1;

                                    const cellRef = colLetter(startCol) + rowIndex;
                                    const c = sheetDoc.createElement('c');
                                    c.setAttribute('r', cellRef);
                                    c.setAttribute('t', 'str');
                                    c.setAttribute('s', headerStyleIndex.toString());

                                    const v = sheetDoc.createElement('v');
                                    let cellText = $cell.text().trim().replace(/\s+/g, ' ');
                                    if (!cellText) cellText = ' ';
                                    v.textContent = cellText;
                                    c.appendChild(v);
                                    trElm.appendChild(c);

                                    for (let rr = rowIndex; rr <= endRow; rr++) {
                                        for (let cc = startCol; cc <= endCol; cc++) {
                                            occupied[rr + '-' + cc] = true;
                                        }
                                    }

                                    if (colspan > 1 || rowspan > 1) {
                                        mergeRanges.push(colLetter(startCol) + rowIndex + ':' + colLetter(endCol) + endRow);
                                    }

                                    colIndex += colspan;
                                });

                                sheetData.appendChild(trElm);
                                rowIndex++;
                            });

                            // Create global font (e.g., Calibri 11)

                            const globalFont1 = stylesDoc.createElement('font');

                            const globalFontName1 = stylesDoc.createElement('name');
                            globalFontName1.setAttribute('val', 'biome');
                            globalFont1.appendChild(globalFontName1);

                            const globalFontSize1 = stylesDoc.createElement('sz');
                            globalFontSize1.setAttribute('val', '8');
                            globalFont1.appendChild(globalFontSize1);
                            fonts.appendChild(globalFont1);

                            const globalFontId = fonts.childNodes.length - 1;
                            fonts.setAttribute('count', fonts.childNodes.length.toString());

                            const dataStyle = stylesDoc.createElement('xf');
                            dataStyle.setAttribute('numFmtId', '0');
                            //dataStyle.setAttribute('fontId', '0'); // default font
                            dataStyle.setAttribute('fontId', globalFontId.toString());
                            dataStyle.setAttribute('applyFont', '1');
                            dataStyle.setAttribute('borderId', borderId.toString());
                            dataStyle.setAttribute('applyBorder', '1');
                            dataStyle.setAttribute('fillId', '0');
                            dataStyle.setAttribute('xfId', '0');
                            dataStyle.setAttribute('applyAlignment', '1');

                            const dataAlignment = stylesDoc.createElement('alignment');
                            dataAlignment.setAttribute('horizontal', 'center');
                            dataAlignment.setAttribute('vertical', 'center');
                            dataAlignment.setAttribute('wrapText', '1');
                            dataStyle.appendChild(dataAlignment);

                            cellXfs.appendChild(dataStyle);
                            const centerStyleIndex = cellXfs.childNodes.length - 1;
                            cellXfs.setAttribute('count', cellXfs.childNodes.length.toString());

                            // 2. Add data rows from tbody
                            $('#dashboard_ar tbody tr').each(function () {
                                let colIndex = 1;
                                const trElm = sheetDoc.createElement('row');
                                trElm.setAttribute('r', rowIndex);

                                $(this).children('td').each(function () {
                                    const $cell = $(this);
                                    const colspan = parseInt($cell.attr('colspan')) || 1;
                                    const rowspan = parseInt($cell.attr('rowspan')) || 1;

                                    while (occupied[rowIndex + '-' + colIndex]) {
                                        colIndex++;
                                    }

                                    const startCol = colIndex;
                                    const endCol = colIndex + colspan - 1;
                                    const endRow = rowIndex + rowspan - 1;

                                    const cellRef = colLetter(startCol) + rowIndex;
                                    const c = sheetDoc.createElement('c');
                                    c.setAttribute('r', cellRef);
                                    c.setAttribute('t', 'str');
                                    //if (colIndex > 2)
                                    c.setAttribute('s', centerStyleIndex.toString());

                                    const v = sheetDoc.createElement('v');
                                    let cellText = $cell.text().trim().replace(/\s+/g, ' ');
                                    if (!cellText) cellText = ' ';
                                    v.textContent = cellText;
                                    c.appendChild(v);
                                    trElm.appendChild(c);

                                    for (let rr = rowIndex; rr <= endRow; rr++) {
                                        for (let cc = startCol; cc <= endCol; cc++) {
                                            occupied[rr + '-' + cc] = true;
                                        }
                                    }

                                    if (colspan > 1 || rowspan > 1) {
                                        mergeRanges.push(colLetter(startCol) + rowIndex + ':' + colLetter(endCol) + endRow);
                                    }

                                    colIndex += colspan;
                                });

                                sheetData.appendChild(trElm);
                                rowIndex++;
                            });

                            // Add mergeCells element if any merges needed
                            if (mergeRanges.length > 0) {
                                const mergeCells = sheetDoc.createElement('mergeCells');
                                mergeCells.setAttribute('count', mergeRanges.length);

                                mergeRanges.forEach(range => {
                                    const mergeCell = sheetDoc.createElement('mergeCell');
                                    mergeCell.setAttribute('ref', range);
                                    mergeCells.appendChild(mergeCell);
                                });

                                if (sheetData.nextSibling) {
                                    worksheet.insertBefore(mergeCells, sheetData.nextSibling);
                                } else {
                                    worksheet.appendChild(mergeCells);
                                }
                            }
                        }




                    },
                ],
                footerCallback: function (row, data, start, end, display) {
                    let api = this.api();

                    // Remove the formatting to get integer data for summation
                    let intVal = function (i) {
                        return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0;
                    };
                    for (var i = 3; i < api.columns().count(); i++) {

                        var totalLoan = api.column(i, { page: 'current' }).data().reduce((a, b) => intVal(a) + intVal(b), 0);
                        var totalAmt = api.column(i, { page: 'current' }).data().reduce((a, b) => intVal(a) + intVal(b), 0);

                        api.column(i).footer().innerHTML = Number(totalLoan).toFixed(2);
                        api.column(i).footer().innerHTML = Number(totalAmt).toFixed(2);

                        if (i % 2 != 0)
                            document.getElementById("lbl_" + (i)).innerHTML = "Count (" + new Intl.NumberFormat().format(Number(totalLoan)) + ")";
                        else
                            document.getElementById("lbl_" + (i)).innerHTML = "US $ (" + new Intl.NumberFormat().format(Number(totalAmt).toFixed(2)) + ")";

                        //document.getElementById("lbl_" + (i)).innerHTML = "(" + Number(totalLoan).toFixed(2) + ")";
                        //document.getElementById("lbl_" + (i)).innerHTML = "(" + Number(totalAmt).toFixed(2) + ")";


                    }
                }


            });

        },
        error: function (error) {
            CanopyUI.showError(error, 'Unable to complete request.');
        }
    });
    return false;
}

function dashboard_arBind_DifferenceQuickbook() {
    $('#load1').show();
    var columns = [];
    var columnlink = [];
    var preheader = '';
    var headers = [];
    var returnstring = '';
    var htmlheader = '';
    var htmlrow = '';
    $.ajax({
        url: "Dashboard.aspx/GetClientwiseDashboard_DifferencewithQuickbook",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//
            var firstData = dataArray[0];
            var rowData = dataArray[1];
            var ColumnNames = Object.keys(firstData);
            var rowValues = Object.keys(rowData);

            htmlheader = '<thead><tr>';
            //Main Header
            var thead = document.createElement("thead");
            var trh = document.createElement("tr");
            for (var i = 0; i < ColumnNames.length; i++) {
                if (i == 0) {
                    var tbh = document.createElement("th");
                    tbh.rowSpan = 3;
                    tbh.style.fontWeight = "bold!important";
                    tbh.style.verticalAlign = "middle";
                    tbh.innerHTML = "Actions";
                    trh.appendChild(tbh);
                }
                else if (i == 2 || i == 3) {
                    var tbh = document.createElement("th");
                    tbh.rowSpan = 3;
                    tbh.style.fontWeight = "bold!important";
                    tbh.style.verticalAlign = "middle";
                    if (ColumnNames[i] == "Process")
                        tbh.style.width = "180px";
                    else
                        tbh.style.width = "250px";
                    tbh.style.left = "0px!important";
                    tbh.innerHTML = ColumnNames[i];
                    trh.appendChild(tbh);
                }
                else if (i > 1) {
                    if (i % 2 == 0) {
                        var tbh = document.createElement("th");
                        tbh.colSpan = 6;
                        tbh.style.textAlign = "center";
                        tbh.innerHTML = ColumnNames[i].replace("-ERP-LoanCount", "").replace("-ERP-Amount", "").replace("-QB-LoanCount", "").replace("-QB-Amount", "").replace("-Difference-LoanCount", "").replace("-Difference-Amount", "");
                        tbh.style.borderRight = "Solid 2px black";
                        trh.appendChild(tbh);
                        //thead.appendChild(trh);
                        //htmlheader = htmlheader + '<th colspan=2 style="padding-left:20px;left: 0px!important; text-align:center;">' + ColumnNames[i].replace("-LoanCount", "").replace("-Amount", "") + '</th>';
                        i = i + 5;
                    }
                    //else
                    //    htmlheader = htmlheader + '<th style="padding-left:20px;left: 0px!important; text-align:center;">' + ColumnNames[i] + '</th>';
                }
                else {
                    var tbh = document.createElement("th");
                    tbh.rowSpan = 3;
                    tbh.style.display = 'none';
                    tbh.innerHTML = ColumnNames[i];
                    tbh.classList.remove();
                    tbh.classList.add("no-export");
                    //htmlheader = htmlheader + '<th style="display:none;">' + ColumnNames[i] + '</th>';
                    trh.appendChild(tbh);
                    //thead.appendChild(trh);
                }
            }
            thead.appendChild(trh);

            //seocnd header row
            trh = document.createElement("tr");
            var oddeven = 1;
            for (var i = 0; i < ColumnNames.length; i++) {
                if (i == 0 || i == 2 || i == 3) {
                }
                else if (i > 1) {
                    if (oddeven == 1) {
                        var tbh = document.createElement("th");
                        tbh.colSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.innerHTML = "ERP";
                        trh.appendChild(tbh);
                        oddeven++;
                        i = i + 1;
                    }
                    else if (oddeven == 2) {
                        var tbh = document.createElement("th");
                        tbh.colSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.innerHTML = "Quickbook";
                        //tbh.style.color = "green";
                        trh.appendChild(tbh);
                        oddeven++;
                        i = i + 1;
                    }
                    else {
                        var tbh = document.createElement("th");
                        tbh.colSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.innerHTML = "Difference";
                        tbh.style.borderRight = "Solid 2px black";
                        //tbh.style.color = "red";
                        trh.appendChild(tbh);
                        i = i + 1;
                        if (oddeven == 3) {
                            oddeven = 1;
                        }
                    }

                }
                else {

                    //var tbh = document.createElement("th");
                    //tbh.style.display = 'none';
                    //tbh.innerHTML = ColumnNames[i];
                    ////htmlheader = htmlheader + '<th style="display:none;">' + ColumnNames[i] + '</th>';
                    //trh.appendChild(tbh);
                    //thead.appendChild(trh);
                }
            }
            thead.appendChild(trh);
            // $('#dashboard_ar').append(thead);

            //Third header row
            var trh1 = document.createElement("tr");
            //htmlheader = htmlheader + '</tr><tr>';
            for (var i = 0; i < ColumnNames.length; i++) {
                if (i == 0 || i == 2 || i == 3) {
                    //var tbr = document.createElement("th");
                    //tbr.innerHTML = ColumnNames[i];
                    //tbr.style.textAlign = "center";
                    //trh1.appendChild(tbr);
                    ////htmlheader = htmlheader + '<th style="left: 0px!important; ">' + ColumnNames[i] + '</th>';
                }
                else if (i > 1)
                    if (i % 2 != 0) {
                        var tbr = document.createElement("th");
                        tbr.style.textAlign = "center";
                        tbr.innerHTML = 'US $';
                        tbr.id = "lblDiff_" + (i);
                        trh1.appendChild(tbr);
                        if (ColumnNames[i].includes("Difference")) {
                            tbr.style.borderRight = "Solid 2px black";
                        }
                    }
                    else {
                        var tbr = document.createElement("th");
                        tbr.style.textAlign = "center";
                        tbr.innerHTML = 'Count';
                        tbr.id = "lblDiff_" + (i);
                        trh1.appendChild(tbr);
                    }
                else {
                }

            }
            thead.appendChild(trh1);
            $('#dashboard_ardifference').append(thead);
            // document.getElementById("newlabl").innerText = thead.innerHTML;
            var tbody = document.createElement("tbody");
            $.each(dataArray, function (index, item) {
                var tr1 = document.createElement("tr");
                //htmlrow = htmlrow + '<tr>';
                for (var i = 0; i < ColumnNames.length; i++) {
                    var targetColumnName = ColumnNames[i];
                    if (i == 0) {
                        var td = document.createElement("td");
                        var link = '<a class="dropdown-item" title="Send Invoice To Client" href="#!" id="Actions" onclick="dashboard_AddRemark(\'' + index + '\');" style="width:30px; display:inline;padding: .25rem .25rem!important;"><img src="../Images/edit.png" style="width:20px; display:inline;" /></a>';
                        td.innerHTML = link;
                        tr1.appendChild(td);
                    }
                    else if (i == 2 || i == 3) {
                        var td = document.createElement("td");
                        var label = document.createElement("label");
                        label.id = i + "_" + item[targetColumnName].replace(" ", "") + "ClientLabel";
                        label.innerHTML = item[targetColumnName];
                        if (ColumnNames[i] == "Process")
                            label.style.width = "180px";
                        else
                            label.style.width = "250px";
                        td.appendChild(label);
                        tr1.appendChild(td);
                    }
                    else if (i > 1) {
                        var td = document.createElement("td");
                        td.style.textAlign = "center";
                        td.innerHTML = item[targetColumnName];
                        tr1.appendChild(td);
                        if (targetColumnName.includes("Difference") && i % 2 != 0) {
                            td.style.borderRight = "Solid 2px black";
                        }
                    }
                    else {
                        var td = document.createElement("td");
                        td.style.display = 'none';
                        td.classList.remove();
                        td.classList.add("no-export");
                        td.innerHTML = item[targetColumnName];
                        tr1.appendChild(td);
                    }
                }
                tbody.appendChild(tr1);
                $('#dashboard_ardifference').append(tbody);
            });


            //Footer
            var tfoot = document.createElement("tfoot");
            var trf1 = document.createElement("tr");
            for (var i = 0; i < ColumnNames.length; i++) {
                if (i == 0 || i == 2 || i == 3) {
                    var tbr = document.createElement("th");
                    tbr.style.textAlign = "center";
                    trf1.appendChild(tbr);
                }
                else if (i > 1) {
                    var tbr = document.createElement("th");
                    tbr.style.textAlign = "center";
                    trf1.appendChild(tbr);
                }
                else {
                    var tbr = document.createElement("th");
                    tbr.style.display = 'none';
                    trf1.appendChild(tbr);
                }
            }
            tfoot.appendChild(trf1);
            $('#dashboard_ardifference').append(tfoot);

            $('#dashboard_ardifference').DataTable({
                dom: 'Bft',
                destroy: true,
                orderCellsTop: true,
                fixedColumns: {
                    leftColumns: 2,
                },
                scrollCollapse: false,
                scrollY: '400px',
                scrollX: true,
                "paging": false,
                "autoWidth": true,
                select: true,
                "ordering": false,
                filter: true,
                'select': {
                    'style': 'single'
                },
                "serverSide": false,

                initComplete: function () {
                    $('#load1').hide();
                },
                //columnDefs: [
                //    { targets: 0, visible: false }  // hides column index 2
                //],
                buttons: [
                    {
                        extend: 'excelHtml5',
                        text: 'Export to Excel',
                        filename: 'Comparison with Quickbok',
                        exportOptions: {
                            columns: function (idx, data, node) {
                                return idx >= 2; // skip first two columns (0 and 1)
                            }
                        },
                        customize: function (xlsx) {
                            const sheetDoc = xlsx.xl.worksheets['sheet1.xml'];



                            const worksheet = sheetDoc.documentElement;
                            const sheetData = worksheet.getElementsByTagName('sheetData')[0];
                            const $worksheet = $(worksheet);


                            // Remove all existing rows (clean slate)
                            $worksheet.find('row').remove();

                            // Remove existing mergeCells if any
                            $worksheet.find('mergeCells').remove();

                            function colLetter(n) {
                                let s = '', t;
                                while (n > 0) {
                                    t = (n - 1) % 26;
                                    s = String.fromCharCode(65 + t) + s;
                                    n = Math.floor((n - 1) / 26);
                                }
                                return s;
                            }

                            const occupied = {};
                            const mergeRanges = [];

                            const stylesDoc = xlsx.xl['styles.xml'];
                            const fonts = stylesDoc.getElementsByTagName('fonts')[0];
                            const borders = stylesDoc.getElementsByTagName('borders')[0];
                            const cellXfs = stylesDoc.getElementsByTagName('cellXfs')[0];

                            // --- Create bold font for header
                            const headerFont = stylesDoc.createElement('font');
                            const bold = stylesDoc.createElement('b');
                            const color = stylesDoc.createElement('color');
                            color.setAttribute('rgb', 'FF000000'); // black
                            headerFont.appendChild(bold);
                            headerFont.appendChild(color);
                            fonts.appendChild(headerFont);
                            const headerFontId = fonts.childNodes.length - 1;
                            fonts.setAttribute('count', fonts.childNodes.length.toString());

                            // --- Create border for all cells
                            const border = stylesDoc.createElement('border');
                            ['left', 'right', 'top', 'bottom'].forEach(side => {
                                const sideElem = stylesDoc.createElement(side);
                                sideElem.setAttribute('style', 'thin');
                                const colorElem = stylesDoc.createElement('color');
                                colorElem.setAttribute('auto', '1');
                                sideElem.appendChild(colorElem);
                                border.appendChild(sideElem);
                            });

                            const headerStyle = stylesDoc.createElement('xf');
                            headerStyle.setAttribute('numFmtId', '0');
                            headerStyle.setAttribute('fontId', headerFontId.toString());
                            headerStyle.setAttribute('fillId', '0');
                            headerStyle.setAttribute('xfId', '0');
                            headerStyle.setAttribute('applyFont', '1');
                            headerStyle.setAttribute('applyBorder', '1');
                            headerStyle.setAttribute('applyAlignment', '1');

                            // Add alignment child
                            const alignment = stylesDoc.createElement('alignment');
                            alignment.setAttribute('horizontal', 'center');
                            alignment.setAttribute('vertical', 'center');
                            alignment.setAttribute('wrapText', '1');
                            headerStyle.appendChild(alignment);

                            cellXfs.appendChild(headerStyle);

                            // Append the new xf
                            //cellXfs.appendChild(headerAlignXf);
                            const headerStyleIndex = cellXfs.childNodes.length - 1;
                            cellXfs.setAttribute('count', cellXfs.childNodes.length.toString());



                            // Start rowIndex at 1 to insert headers at the top
                            let rowIndex = 1;

                            // 1. Add header rows from thead
                            $('#dashboard_ardifference thead tr').each(function () {
                                let colIndex = 1;
                                const trElm = sheetDoc.createElement('row');
                                trElm.setAttribute('r', rowIndex);

                                $(this).children('th, td').each(function () {
                                    const $cell = $(this);
                                    const colspan = parseInt($cell.attr('colspan')) || 1;
                                    const rowspan = parseInt($cell.attr('rowspan')) || 1;

                                    while (occupied[rowIndex + '-' + colIndex]) {
                                        colIndex++;
                                    }

                                    const startCol = colIndex;
                                    const endCol = colIndex + colspan - 1;
                                    const endRow = rowIndex + rowspan - 1;

                                    const cellRef = colLetter(startCol) + rowIndex;
                                    const c = sheetDoc.createElement('c');
                                    c.setAttribute('r', cellRef);
                                    c.setAttribute('t', 'str');
                                    //if (colIndex > 2)
                                    c.setAttribute('s', headerStyleIndex.toString());

                                    const v = sheetDoc.createElement('v');
                                    let cellText = $cell.text().trim().replace(/\s+/g, ' ');
                                    if (!cellText) cellText = ' ';
                                    v.textContent = cellText;
                                    c.appendChild(v);
                                    trElm.appendChild(c);

                                    for (let rr = rowIndex; rr <= endRow; rr++) {
                                        for (let cc = startCol; cc <= endCol; cc++) {
                                            occupied[rr + '-' + cc] = true;
                                        }
                                    }

                                    if (colspan > 1 || rowspan > 1) {
                                        mergeRanges.push(colLetter(startCol) + rowIndex + ':' + colLetter(endCol) + endRow);
                                    }

                                    colIndex += colspan;
                                });

                                sheetData.appendChild(trElm);
                                rowIndex++;
                            });


                            const dataStyle = stylesDoc.createElement('xf');
                            dataStyle.setAttribute('numFmtId', '0');
                            dataStyle.setAttribute('fontId', '0'); // default font
                            dataStyle.setAttribute('fillId', '0');
                            dataStyle.setAttribute('xfId', '0');
                            dataStyle.setAttribute('applyAlignment', '1');

                            const dataAlignment = stylesDoc.createElement('alignment');
                            dataAlignment.setAttribute('horizontal', 'center');
                            dataAlignment.setAttribute('vertical', 'center');
                            dataAlignment.setAttribute('wrapText', '1');
                            dataStyle.appendChild(dataAlignment);

                            cellXfs.appendChild(dataStyle);
                            const centerStyleIndex = cellXfs.childNodes.length - 1;
                            cellXfs.setAttribute('count', cellXfs.childNodes.length.toString());


                            // 2. Add data rows from tbody
                            $('#dashboard_ardifference tbody tr').each(function () {
                                let colIndex = 1;
                                const trElm = sheetDoc.createElement('row');
                                trElm.setAttribute('r', rowIndex);

                                $(this).children('td').each(function () {
                                    const $cell = $(this);
                                    const colspan = parseInt($cell.attr('colspan')) || 1;
                                    const rowspan = parseInt($cell.attr('rowspan')) || 1;

                                    while (occupied[rowIndex + '-' + colIndex]) {
                                        colIndex++;
                                    }

                                    const startCol = colIndex;
                                    const endCol = colIndex + colspan - 1;
                                    const endRow = rowIndex + rowspan - 1;

                                    const cellRef = colLetter(startCol) + rowIndex;
                                    const c = sheetDoc.createElement('c');
                                    c.setAttribute('r', cellRef);
                                    c.setAttribute('t', 'str');
                                    if (colIndex > 2)
                                        c.setAttribute('s', centerStyleIndex.toString());

                                    const v = sheetDoc.createElement('v');
                                    let cellText = $cell.text().trim().replace(/\s+/g, ' ');
                                    if (!cellText) cellText = ' ';
                                    v.textContent = cellText;
                                    c.appendChild(v);
                                    trElm.appendChild(c);

                                    for (let rr = rowIndex; rr <= endRow; rr++) {
                                        for (let cc = startCol; cc <= endCol; cc++) {
                                            occupied[rr + '-' + cc] = true;
                                        }
                                    }

                                    if (colspan > 1 || rowspan > 1) {
                                        mergeRanges.push(colLetter(startCol) + rowIndex + ':' + colLetter(endCol) + endRow);
                                    }

                                    colIndex += colspan;
                                });

                                sheetData.appendChild(trElm);
                                rowIndex++;
                            });

                            // Add mergeCells element if any merges needed
                            if (mergeRanges.length > 0) {
                                const mergeCells = sheetDoc.createElement('mergeCells');
                                mergeCells.setAttribute('count', mergeRanges.length);

                                mergeRanges.forEach(range => {
                                    const mergeCell = sheetDoc.createElement('mergeCell');
                                    mergeCell.setAttribute('ref', range);
                                    mergeCells.appendChild(mergeCell);
                                });

                                if (sheetData.nextSibling) {
                                    worksheet.insertBefore(mergeCells, sheetData.nextSibling);
                                } else {
                                    worksheet.appendChild(mergeCells);
                                }
                            }


                        }




                    },
                ],

                footerCallback: function (row, data, start, end, display) {
                    let api = this.api();

                    // Remove the formatting to get integer data for summation
                    let intVal = function (i) {
                        return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0;
                    };
                    for (var i = 0; i < api.columns().count(); i++) {
                        if (i > 3) {
                            var totalLoan = api.column(i, { page: 'current' }).data().reduce((a, b) => intVal(a) + intVal(b), 0);
                            var totalAmt = api.column(i, { page: 'current' }).data().reduce((a, b) => intVal(a) + intVal(b), 0);

                            api.column(i).footer().innerHTML = Number(totalLoan);
                            api.column(i).footer().innerHTML = Number(totalAmt).toFixed(2);
                            if (i % 2 == 0) {
                                document.getElementById("lblDiff_" + (i)).innerHTML = "Count (" + new Intl.NumberFormat().format(Number(totalLoan)) + ")";
                            }
                            else {
                                document.getElementById("lblDiff_" + (i)).innerHTML = "US $ (" + new Intl.NumberFormat().format(Number(totalAmt).toFixed(2)) + ")";
                            }
                        }
                    }
                }


            });

        },
        error: function (error) {
            CanopyUI.showError(error, 'Unable to complete request.');
        }
    });
    return false;
}

function dashboard_arBind_DifferenceQuickbook_Working() {
    $('#load1').show();
    var columns = [];
    var columnlink = [];
    var preheader = '';
    var headers = [];
    var returnstring = '';
    var htmlheader = '';
    var htmlrow = '';
    $.ajax({
        url: "Dashboard.aspx/GetClientwiseDashboard_DifferencewithQuickbook",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//
            var firstData = dataArray[0];
            var rowData = dataArray[1];
            var ColumnNames = Object.keys(firstData);
            var rowValues = Object.keys(rowData);

            htmlheader = '<thead><tr>';
            //Main Header
            var thead = document.createElement("thead");
            var trh = document.createElement("tr");
            for (var i = 1; i < ColumnNames.length; i++) {
                if (i == 2 || i == 3) {
                    var tbh = document.createElement("th");
                    tbh.rowSpan = 3;
                    tbh.style.fontWeight = "bold!important";
                    tbh.style.verticalAlign = "middle";
                    if (ColumnNames[i] == "Process")
                        tbh.style.width = "180px";
                    else
                        tbh.style.width = "250px";
                    tbh.style.left = "0px!important";
                    tbh.innerHTML = ColumnNames[i];
                    trh.appendChild(tbh);
                    //thead.appendChild(trh);
                    //htmlheader = htmlheader + '<th style="left: 0px!important;">Client</th>';
                }
                else if (i > 1) {
                    if (i % 2 == 0) {
                        var tbh = document.createElement("th");
                        tbh.colSpan = 6;
                        tbh.style.textAlign = "center";
                        tbh.innerHTML = ColumnNames[i].replace("-ERP-LoanCount", "").replace("-ERP-Amount", "").replace("-QB-LoanCount", "").replace("-QB-Amount", "").replace("-Difference-LoanCount", "").replace("-Difference-Amount", "");
                        trh.appendChild(tbh);
                        //thead.appendChild(trh);
                        //htmlheader = htmlheader + '<th colspan=2 style="padding-left:20px;left: 0px!important; text-align:center;">' + ColumnNames[i].replace("-LoanCount", "").replace("-Amount", "") + '</th>';
                        i = i + 5;
                    }
                    //else
                    //    htmlheader = htmlheader + '<th style="padding-left:20px;left: 0px!important; text-align:center;">' + ColumnNames[i] + '</th>';
                }
                else {
                    var tbh = document.createElement("th");
                    tbh.rowSpan = 3;
                    //tbh.style.display = 'none';
                    tbh.innerHTML = ColumnNames[i];
                    //htmlheader = htmlheader + '<th style="display:none;">' + ColumnNames[i] + '</th>';
                    trh.appendChild(tbh);
                    //thead.appendChild(trh);
                }
            }
            thead.appendChild(trh);

            //seocnd header row
            trh = document.createElement("tr");
            var oddeven = 1;
            for (var i = 1; i < ColumnNames.length; i++) {
                if (i == 2 || i == 3) {
                }
                else if (i > 1) {
                    if (oddeven == 1) {
                        var tbh = document.createElement("th");
                        tbh.colSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.innerHTML = "ERP";
                        trh.appendChild(tbh);
                        oddeven++;
                        i = i + 1;
                    }
                    else if (oddeven == 2) {
                        var tbh = document.createElement("th");
                        tbh.colSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.innerHTML = "Quickbook";
                        //tbh.style.color = "green";
                        trh.appendChild(tbh);
                        oddeven++;
                        i = i + 1;
                    }
                    else {
                        var tbh = document.createElement("th");
                        tbh.colSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.innerHTML = "Difference";
                        //tbh.style.color = "red";
                        trh.appendChild(tbh);
                        i = i + 1;
                        if (oddeven == 3) {
                            oddeven = 1;
                        }
                    }

                }
                else {

                    //var tbh = document.createElement("th");
                    //tbh.style.display = 'none';
                    //tbh.innerHTML = ColumnNames[i];
                    ////htmlheader = htmlheader + '<th style="display:none;">' + ColumnNames[i] + '</th>';
                    //trh.appendChild(tbh);
                    //thead.appendChild(trh);
                }
            }
            thead.appendChild(trh);
            // $('#dashboard_ar').append(thead);

            //Third header row
            var trh1 = document.createElement("tr");
            //htmlheader = htmlheader + '</tr><tr>';
            for (var i = 1; i < ColumnNames.length; i++) {
                if (i == 2 || i == 3) {
                    //var tbr = document.createElement("th");
                    //tbr.innerHTML = ColumnNames[i];
                    //tbr.style.textAlign = "center";
                    //trh1.appendChild(tbr);
                    ////htmlheader = htmlheader + '<th style="left: 0px!important; ">' + ColumnNames[i] + '</th>';
                }
                else if (i > 1)
                    if (i % 2 != 0) {
                        var tbr = document.createElement("th");
                        tbr.style.textAlign = "center";
                        tbr.innerHTML = 'US $';
                        tbr.id = "lblDiff_" + (i - 1);
                        trh1.appendChild(tbr);
                    }
                    else {
                        var tbr = document.createElement("th");
                        tbr.style.textAlign = "center";
                        tbr.innerHTML = 'Count';
                        tbr.id = "lblDiff_" + (i - 1);
                        trh1.appendChild(tbr);
                    }
                else {
                }
            }
            thead.appendChild(trh1);
            $('#dashboard_ardifference').append(thead);
            //document.getElementById("newlabl").innerText = thead.innerHTML;
            var tbody = document.createElement("tbody");
            $.each(dataArray, function (index, item) {
                var tr1 = document.createElement("tr");
                //htmlrow = htmlrow + '<tr>';
                for (var i = 1; i < ColumnNames.length; i++) {
                    var targetColumnName = ColumnNames[i];
                    if (i == 2 || i == 3) {
                        var td = document.createElement("td");
                        var label = document.createElement("label");
                        label.id = i + "_" + item[targetColumnName].replace(" ", "") + "ClientLabel";
                        label.innerHTML = item[targetColumnName];
                        if (ColumnNames[i] == "Process")
                            label.style.width = "180px";
                        else
                            label.style.width = "250px";
                        /*label.style.width = "250px";*/
                        td.appendChild(label);
                        tr1.appendChild(td);
                    }
                    else if (i > 1) {
                        //if (i % 2 == 0) {
                        var td = document.createElement("td");
                        td.style.textAlign = "center";

                        //td.style.backColor = "green";
                        td.innerHTML = item[targetColumnName];
                        //if (targetColumnName.includes("Difference")) {
                        //    td.style.color = "red";
                        //    td.style.backgroundColor = "#e2e2e2!important";
                        //}
                        //if (targetColumnName.includes("QB")) {
                        //    td.style.color = "green";
                        //    td.style.backgroundColor = "#e2e2e2!important";
                        //}
                        //td.innerHTML = '<input type="text" id="loancount_' + targetColumnName + '_' + index + '" onchange="return getValue(this,' + index + ');" style="width:70px;" value="' + blankForNull(item[targetColumnName]) + '"></input>';
                        tr1.appendChild(td);
                        //}
                        //else {
                        //    var td = document.createElement("td");
                        //    td.style.textAlign = "center";
                        //    td.innerHTML = '<input type="text" id="loancount_' + targetColumnName + '_' + index + '" onchange="return getValue(this,' + index + ');" style="width:50px;" value="' + blankForNull(item[targetColumnName]) + '"></input>';
                        //    tr1.appendChild(td);
                        //}
                    }
                    else {
                        var td = document.createElement("td");
                        td.style.display = 'none';
                        td.innerHTML = item[targetColumnName];
                        tr1.appendChild(td);
                    }
                }
                tbody.appendChild(tr1);
                $('#dashboard_ardifference').append(tbody);
            });


            //Footer
            var tfoot = document.createElement("tfoot");
            var trf1 = document.createElement("tr");
            for (var i = 1; i < ColumnNames.length; i++) {
                if (i == 2 || i == 3) {
                    var tbr = document.createElement("th");
                    tbr.style.textAlign = "center";
                    trf1.appendChild(tbr);
                }
                else if (i > 1) {
                    var tbr = document.createElement("th");
                    tbr.style.textAlign = "center";
                    trf1.appendChild(tbr);
                }
                else {
                    var tbr = document.createElement("th");
                    tbr.style.display = 'none';
                    trf1.appendChild(tbr);
                }
            }
            tfoot.appendChild(trf1);
            $('#dashboard_ardifference').append(tfoot);

            $('#dashboard_ardifference').DataTable({
                dom: 'Bft',
                destroy: true,
                orderCellsTop: true,
                fixedColumns: {
                    leftColumns: 2,
                },
                scrollCollapse: false,
                scrollY: '400px',
                scrollX: true,
                "paging": false,
                "autoWidth": true,
                select: true,
                "ordering": false,
                filter: true,
                'select': {
                    'style': 'single'
                },
                "serverSide": false,

                initComplete: function () {
                    $('#load1').hide();
                },
                columnDefs: [
                    { targets: 0, visible: false }  // hides column index 2
                ],
                buttons: [
                    {
                        extend: 'excelHtml5',
                        text: 'Export to Excel',
                        filename: 'Comparison with Quickbok',
                        exportOptions: {
                            columns: ':visible',
                            modifier: { header: false }
                        },
                        customize: function (xlsx) {
                            const sheetDoc = xlsx.xl.worksheets['sheet1.xml'];
                            const worksheet = sheetDoc.documentElement;
                            const sheetData = worksheet.getElementsByTagName('sheetData')[0];
                            const $worksheet = $(worksheet);


                            // Remove all existing rows (clean slate)
                            $worksheet.find('row').remove();

                            // Remove existing mergeCells if any
                            $worksheet.find('mergeCells').remove();

                            function colLetter(n) {
                                let s = '', t;
                                while (n > 0) {
                                    t = (n - 1) % 26;
                                    s = String.fromCharCode(65 + t) + s;
                                    n = Math.floor((n - 1) / 26);
                                }
                                return s;
                            }

                            const occupied = {};
                            const mergeRanges = [];

                            const stylesDoc = xlsx.xl['styles.xml'];
                            const fonts = stylesDoc.getElementsByTagName('fonts')[0];
                            const borders = stylesDoc.getElementsByTagName('borders')[0];
                            const cellXfs = stylesDoc.getElementsByTagName('cellXfs')[0];

                            // --- Create bold font for header
                            const headerFont = stylesDoc.createElement('font');
                            const bold = stylesDoc.createElement('b');
                            const color = stylesDoc.createElement('color');
                            color.setAttribute('rgb', 'FF000000'); // black
                            headerFont.appendChild(bold);
                            headerFont.appendChild(color);
                            fonts.appendChild(headerFont);
                            const headerFontId = fonts.childNodes.length - 1;
                            fonts.setAttribute('count', fonts.childNodes.length.toString());

                            // --- Create border for all cells
                            const border = stylesDoc.createElement('border');
                            ['left', 'right', 'top', 'bottom'].forEach(side => {
                                const sideElem = stylesDoc.createElement(side);
                                sideElem.setAttribute('style', 'thin');
                                const colorElem = stylesDoc.createElement('color');
                                colorElem.setAttribute('auto', '1');
                                sideElem.appendChild(colorElem);
                                border.appendChild(sideElem);
                            });

                            const headerStyle = stylesDoc.createElement('xf');
                            headerStyle.setAttribute('numFmtId', '0');
                            headerStyle.setAttribute('fontId', headerFontId.toString());
                            headerStyle.setAttribute('fillId', '0');
                            headerStyle.setAttribute('xfId', '0');
                            headerStyle.setAttribute('applyFont', '1');
                            headerStyle.setAttribute('applyBorder', '1');
                            headerStyle.setAttribute('applyAlignment', '1');

                            // Add alignment child
                            const alignment = stylesDoc.createElement('alignment');
                            alignment.setAttribute('horizontal', 'center');
                            alignment.setAttribute('vertical', 'center');
                            alignment.setAttribute('wrapText', '1');
                            headerStyle.appendChild(alignment);

                            cellXfs.appendChild(headerStyle);

                            // Append the new xf
                            //cellXfs.appendChild(headerAlignXf);
                            const headerStyleIndex = cellXfs.childNodes.length - 1;
                            cellXfs.setAttribute('count', cellXfs.childNodes.length.toString());



                            // Start rowIndex at 1 to insert headers at the top
                            let rowIndex = 1;

                            // 1. Add header rows from thead
                            $('#dashboard_ardifference thead tr').each(function () {
                                let colIndex = 1;
                                const trElm = sheetDoc.createElement('row');
                                trElm.setAttribute('r', rowIndex);

                                $(this).children('th, td').each(function () {
                                    const $cell = $(this);
                                    const colspan = parseInt($cell.attr('colspan')) || 1;
                                    const rowspan = parseInt($cell.attr('rowspan')) || 1;

                                    while (occupied[rowIndex + '-' + colIndex]) {
                                        colIndex++;
                                    }

                                    const startCol = colIndex;
                                    const endCol = colIndex + colspan - 1;
                                    const endRow = rowIndex + rowspan - 1;

                                    const cellRef = colLetter(startCol) + rowIndex;
                                    const c = sheetDoc.createElement('c');
                                    c.setAttribute('r', cellRef);
                                    c.setAttribute('t', 'str');
                                    //if (colIndex > 2)
                                    c.setAttribute('s', headerStyleIndex.toString());

                                    const v = sheetDoc.createElement('v');
                                    let cellText = $cell.text().trim().replace(/\s+/g, ' ');
                                    if (!cellText) cellText = ' ';
                                    v.textContent = cellText;
                                    c.appendChild(v);
                                    trElm.appendChild(c);

                                    for (let rr = rowIndex; rr <= endRow; rr++) {
                                        for (let cc = startCol; cc <= endCol; cc++) {
                                            occupied[rr + '-' + cc] = true;
                                        }
                                    }

                                    if (colspan > 1 || rowspan > 1) {
                                        mergeRanges.push(colLetter(startCol) + rowIndex + ':' + colLetter(endCol) + endRow);
                                    }

                                    colIndex += colspan;
                                });

                                sheetData.appendChild(trElm);
                                rowIndex++;
                            });


                            const dataStyle = stylesDoc.createElement('xf');
                            dataStyle.setAttribute('numFmtId', '0');
                            dataStyle.setAttribute('fontId', '0'); // default font
                            dataStyle.setAttribute('fillId', '0');
                            dataStyle.setAttribute('xfId', '0');
                            dataStyle.setAttribute('applyAlignment', '1');

                            const dataAlignment = stylesDoc.createElement('alignment');
                            dataAlignment.setAttribute('horizontal', 'center');
                            dataAlignment.setAttribute('vertical', 'center');
                            dataAlignment.setAttribute('wrapText', '1');
                            dataStyle.appendChild(dataAlignment);

                            cellXfs.appendChild(dataStyle);
                            const centerStyleIndex = cellXfs.childNodes.length - 1;
                            cellXfs.setAttribute('count', cellXfs.childNodes.length.toString());


                            // 2. Add data rows from tbody
                            $('#dashboard_ardifference tbody tr').each(function () {
                                let colIndex = 1;
                                const trElm = sheetDoc.createElement('row');
                                trElm.setAttribute('r', rowIndex);

                                $(this).children('td').each(function () {
                                    const $cell = $(this);
                                    const colspan = parseInt($cell.attr('colspan')) || 1;
                                    const rowspan = parseInt($cell.attr('rowspan')) || 1;

                                    while (occupied[rowIndex + '-' + colIndex]) {
                                        colIndex++;
                                    }

                                    const startCol = colIndex;
                                    const endCol = colIndex + colspan - 1;
                                    const endRow = rowIndex + rowspan - 1;

                                    const cellRef = colLetter(startCol) + rowIndex;
                                    const c = sheetDoc.createElement('c');
                                    c.setAttribute('r', cellRef);
                                    c.setAttribute('t', 'str');
                                    if (colIndex > 2)
                                        c.setAttribute('s', centerStyleIndex.toString());

                                    const v = sheetDoc.createElement('v');
                                    let cellText = $cell.text().trim().replace(/\s+/g, ' ');
                                    if (!cellText) cellText = ' ';
                                    v.textContent = cellText;
                                    c.appendChild(v);
                                    trElm.appendChild(c);

                                    for (let rr = rowIndex; rr <= endRow; rr++) {
                                        for (let cc = startCol; cc <= endCol; cc++) {
                                            occupied[rr + '-' + cc] = true;
                                        }
                                    }

                                    if (colspan > 1 || rowspan > 1) {
                                        mergeRanges.push(colLetter(startCol) + rowIndex + ':' + colLetter(endCol) + endRow);
                                    }

                                    colIndex += colspan;
                                });

                                sheetData.appendChild(trElm);
                                rowIndex++;
                            });

                            // Add mergeCells element if any merges needed
                            if (mergeRanges.length > 0) {
                                const mergeCells = sheetDoc.createElement('mergeCells');
                                mergeCells.setAttribute('count', mergeRanges.length);

                                mergeRanges.forEach(range => {
                                    const mergeCell = sheetDoc.createElement('mergeCell');
                                    mergeCell.setAttribute('ref', range);
                                    mergeCells.appendChild(mergeCell);
                                });

                                if (sheetData.nextSibling) {
                                    worksheet.insertBefore(mergeCells, sheetData.nextSibling);
                                } else {
                                    worksheet.appendChild(mergeCells);
                                }
                            }


                        }




                    },
                ],

                footerCallback: function (row, data, start, end, display) {
                    let api = this.api();

                    // Remove the formatting to get integer data for summation
                    let intVal = function (i) {
                        return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0;
                    };
                    for (var i = 3; i < api.columns().count(); i++) {
                        var totalLoan = api.column(i, { page: 'current' }).data().reduce((a, b) => intVal(a) + intVal(b), 0);
                        var totalAmt = api.column(i, { page: 'current' }).data().reduce((a, b) => intVal(a) + intVal(b), 0);

                        api.column(i).footer().innerHTML = Number(totalLoan);
                        api.column(i).footer().innerHTML = Number(totalAmt).toFixed(2);
                        if (i % 2 != 0)
                            document.getElementById("lblDiff_" + (i)).innerHTML = "Count (" + new Intl.NumberFormat().format(Number(totalLoan)) + ")";
                        else
                            document.getElementById("lblDiff_" + (i)).innerHTML = "US $ (" + new Intl.NumberFormat().format(Number(totalAmt).toFixed(2)) + ")";

                    }
                }


            });

        },
        error: function (error) {
            CanopyUI.showError(error, 'Unable to complete request.');
        }
    });
    return false;
}

function dashboard_arBind_DifferenceQuickbook_BillNo_WorkingWithourRemark() {
    $('#load1').show();
    var columns = [];
    var columnlink = [];
    var preheader = '';
    var headers = [];
    var returnstring = '';
    var htmlheader = '';
    var htmlrow = '';
    $.ajax({
        url: "Dashboard.aspx/GetClientwiseDashboard_DifferencewithQuickbook",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//
            var firstData = dataArray[0];
            var rowData = dataArray[1];
            var ColumnNames = Object.keys(firstData);
            var rowValues = Object.keys(rowData);

            htmlheader = '<thead><tr>';
            //Main Header
            var thead = document.createElement("thead");
            var trh = document.createElement("tr");
            for (var i = 1; i < ColumnNames.length; i++) {
                if (i == 2 || i == 3) {
                    var tbh = document.createElement("th");
                    tbh.rowSpan = 3;
                    tbh.style.fontWeight = "bold!important";
                    tbh.style.verticalAlign = "middle";
                    if (ColumnNames[i] == "Process")
                        tbh.style.width = "180px";
                    else
                        tbh.style.width = "250px";
                    tbh.style.left = "0px!important";
                    tbh.innerHTML = ColumnNames[i];
                    trh.appendChild(tbh);
                    //thead.appendChild(trh);
                    //htmlheader = htmlheader + '<th style="left: 0px!important;">Client</th>';
                }
                else if (i > 1) {
                    if (i % 2 == 0) {

                        var tbh = document.createElement("th");
                        if (ColumnNames[i].includes('Total'))
                            tbh.colSpan = 4;
                        else
                            tbh.colSpan = 8;
                        tbh.style.textAlign = "center";
                        tbh.style.borderRight = "Solid 2px gray";
                        tbh.innerHTML = ColumnNames[i].replace("-ERP-LoanCount", "").replace("-ERP-Amount", "").replace("-QB-LoanCount", "").replace("-QB-Amount", "").replace("-Difference-LoanCount", "").replace("-Difference-Amount", "").replace("-Actions", "").replace("-InvocieNo", "");
                        trh.appendChild(tbh);
                        //thead.appendChild(trh);
                        //htmlheader = htmlheader + '<th colspan=2 style="padding-left:20px;left: 0px!important; text-align:center;">' + ColumnNames[i].replace("-LoanCount", "").replace("-Amount", "") + '</th>';
                        i = i + 7;
                    }
                    //else
                    //    htmlheader = htmlheader + '<th style="padding-left:20px;left: 0px!important; text-align:center;">' + ColumnNames[i] + '</th>';
                }
                else {
                    var tbh = document.createElement("th");
                    tbh.rowSpan = 3;
                    tbh.style.display = 'none';
                    tbh.innerHTML = ColumnNames[i];
                    //htmlheader = htmlheader + '<th style="display:none;">' + ColumnNames[i] + '</th>';
                    trh.appendChild(tbh);
                    //thead.appendChild(trh);
                }
            }
            thead.appendChild(trh);

            //seocnd header row
            trh = document.createElement("tr");
            var oddeven = 1;
            for (var i = 1; i < ColumnNames.length; i++) {
                if (i == 2 || i == 3) {
                }
                else if (i > 1) {
                    if (oddeven == 1) {
                        var tbh = document.createElement("th");
                        tbh.colSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.innerHTML = "ERP";
                        trh.appendChild(tbh);
                        oddeven++;
                        i = i + 1;
                    }
                    else if (oddeven == 2) {
                        var tbh = document.createElement("th");
                        tbh.colSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.innerHTML = "Quickbook";
                        //tbh.style.color = "green";
                        trh.appendChild(tbh);
                        oddeven++;
                        i = i + 1;
                    }
                    else if (oddeven == 3) {
                        var tbh = document.createElement("th");
                        tbh.colSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.innerHTML = "Difference";
                        //tbh.style.color = "red";
                        trh.appendChild(tbh);
                        oddeven++;
                        i = i + 1;
                        //if (oddeven == 3) {
                        //    oddeven = 1;
                        //}
                    }
                    else if (oddeven == 4) {
                        var tbh = document.createElement("th");
                        tbh.rowSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.style.verticalAlign = "middle";
                        tbh.innerHTML = "Invoice #";
                        //tbh.style.color = "red";
                        trh.appendChild(tbh);
                        oddeven++;
                        //i = i + 1;
                        //if (oddeven == 3) {
                        //    oddeven = 1;
                        //}
                    }
                    else {
                        //alert(oddeven);
                        var tbh = document.createElement("th");
                        tbh.rowSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.style.verticalAlign = "middle";
                        tbh.innerHTML = "Actions";
                        tbh.style.borderRight = "Solid 2px gray";
                        //tbh.style.color = "red";
                        trh.appendChild(tbh);
                        //oddeven++;
                        //i = i + 1;
                        if (oddeven == 5) {
                            oddeven = 1;
                        }
                    }

                }
                else {

                    //var tbh = document.createElement("th");
                    //tbh.style.display = 'none';
                    //tbh.innerHTML = ColumnNames[i];
                    ////htmlheader = htmlheader + '<th style="display:none;">' + ColumnNames[i] + '</th>';
                    //trh.appendChild(tbh);
                    //thead.appendChild(trh);
                }
            }
            thead.appendChild(trh);
            // $('#dashboard_ar').append(thead);

            //Third header row
            var trh1 = document.createElement("tr");
            //htmlheader = htmlheader + '</tr><tr>';
            var oddeven_3 = 1;
            for (var i = 1; i < ColumnNames.length; i++) {
                if (i == 2 || i == 3) {
                    //var tbr = document.createElement("th");
                    //tbr.innerHTML = ColumnNames[i];
                    //tbr.style.textAlign = "center";
                    //trh1.appendChild(tbr);
                    ////htmlheader = htmlheader + '<th style="left: 0px!important; ">' + ColumnNames[i] + '</th>';
                }
                else if (i > 1) {
                    if (oddeven_3 == 1 || oddeven_3 == 3 || oddeven_3 == 5) {
                        var tbr = document.createElement("th");
                        tbr.style.textAlign = "center";
                        tbr.innerHTML = 'Count';
                        tbr.id = "lblDiff_" + (i - 1);
                        trh1.appendChild(tbr);
                        oddeven_3++;
                        //i = i + 1;
                    }
                    else if (oddeven_3 == 2 || oddeven_3 == 4 || oddeven_3 == 6) {
                        var tbr = document.createElement("th");
                        tbr.style.textAlign = "center";
                        tbr.innerHTML = 'US $';
                        tbr.id = "lblDiff_" + (i - 1);
                        trh1.appendChild(tbr);
                        oddeven_3++;
                        //i = i + 1;
                    }
                    else if (oddeven_3 == 7) {
                        oddeven_3++;
                    }
                    else {
                        if (oddeven_3 == 8) {
                            oddeven_3 = 1;
                        }
                    }

                }
                //if (i % 2 != 0) {
                //    var tbr = document.createElement("th");
                //    tbr.style.textAlign = "center";
                //    tbr.innerHTML = 'US $';
                //    tbr.id = "lblDiff_" + (i - 1);
                //    trh1.appendChild(tbr);
                //}
                //else {
                //    var tbr = document.createElement("th");
                //    tbr.style.textAlign = "center";
                //    tbr.innerHTML = 'Count';
                //    tbr.id = "lblDiff_" + (i - 1);
                //    trh1.appendChild(tbr);
                //}
                else {
                }
            }
            thead.appendChild(trh1);
            $('#dashboard_ardifference').append(thead);
            //document.getElementById("newlabl").innerText = thead.innerHTML;
            var tbody = document.createElement("tbody");
            $.each(dataArray, function (index, item) {
                var tr1 = document.createElement("tr");
                //htmlrow = htmlrow + '<tr>';
                for (var i = 1; i < ColumnNames.length; i++) {
                    var targetColumnName = ColumnNames[i];
                    if (i == 2 || i == 3) {
                        var td = document.createElement("td");
                        //var label = document.createElement("label");
                        //label.id = i + "_" + item[targetColumnName].replace(" ", "") + "ClientLabel";
                        //label.innerHTML = item[targetColumnName];
                        //if (ColumnNames[i] == "Process")
                        //    label.style.width = "180px";
                        //else
                        //    label.style.width = "250px";
                        /*label.style.width = "250px";*/
                        //td.appendChild(label);
                        td.innerHTML = item[targetColumnName];
                        tr1.appendChild(td);
                    }
                    else if (i > 1) {
                        //if (i % 2 == 0) {
                        var td = document.createElement("td");
                        td.style.textAlign = "center";

                        //td.style.backColor = "green";

                        if (targetColumnName.includes("Action")) {
                            td.style.borderRight = "Solid 2px gray!important";
                            var td = document.createElement("td");
                            var link = '<a class="dropdown-item" title="Send Invoice To Client" href="#!" id="Actions" onclick="dashboard_AddRemark(\'' + index + '\',\'' + targetColumnName + '\');" style="width:30px; display:inline;padding: .25rem .25rem!important;"><img src="../Images/edit.png" style="width:20px; display:inline;" /></a>';
                            td.innerHTML = link;
                            tr1.appendChild(td);
                        }
                        else
                            td.innerHTML = item[targetColumnName];

                        tr1.appendChild(td);

                    }
                    else {
                        var td = document.createElement("td");
                        td.style.display = 'none';
                        td.innerHTML = item[targetColumnName];
                        tr1.appendChild(td);
                    }
                }
                tbody.appendChild(tr1);
                $('#dashboard_ardifference').append(tbody);
            });


            //Footer
            var tfoot = document.createElement("tfoot");
            var trf1 = document.createElement("tr");
            for (var i = 1; i < ColumnNames.length; i++) {
                if (i == 2 || i == 3) {
                    var tbr = document.createElement("th");
                    tbr.style.textAlign = "center";
                    trf1.appendChild(tbr);
                }
                else if (i > 1) {
                    var tbr = document.createElement("th");
                    tbr.style.textAlign = "center";
                    trf1.appendChild(tbr);
                }
                else {
                    var tbr = document.createElement("th");
                    tbr.style.display = 'none';
                    trf1.appendChild(tbr);
                }
            }
            tfoot.appendChild(trf1);
            $('#dashboard_ardifference').append(tfoot);

            $('#dashboard_ardifference').DataTable({
                dom: 'Bft',
                destroy: true,
                orderCellsTop: true,
                fixedColumns: {
                    leftColumns: 2,
                },
                scrollCollapse: false,
                scrollY: '400px',
                scrollX: true,
                "paging": false,
                "autoWidth": true,
                select: true,
                "ordering": false,
                filter: true,
                'select': {
                    'style': 'single'
                },
                "serverSide": false,

                initComplete: function () {
                    $('#load1').hide();
                },
                columnDefs: [
                    { targets: 0, visible: false }  // hides column index 2
                ],
                buttons: [
                    {
                        extend: 'excelHtml5',
                        text: 'Export to Excel',
                        filename: 'Comparison with Quickbok',
                        exportOptions: {
                            columns: ':visible',
                            modifier: { header: false }
                        },
                        customize: function (xlsx) {
                            const sheetDoc = xlsx.xl.worksheets['sheet1.xml'];
                            const worksheet = sheetDoc.documentElement;
                            const sheetData = worksheet.getElementsByTagName('sheetData')[0];
                            const $worksheet = $(worksheet);


                            // Remove all existing rows (clean slate)
                            $worksheet.find('row').remove();

                            // Remove existing mergeCells if any
                            $worksheet.find('mergeCells').remove();

                            function colLetter(n) {
                                let s = '', t;
                                while (n > 0) {
                                    t = (n - 1) % 26;
                                    s = String.fromCharCode(65 + t) + s;
                                    n = Math.floor((n - 1) / 26);
                                }
                                return s;
                            }

                            const occupied = {};
                            const mergeRanges = [];

                            const stylesDoc = xlsx.xl['styles.xml'];
                            const fonts = stylesDoc.getElementsByTagName('fonts')[0];
                            const borders = stylesDoc.getElementsByTagName('borders')[0];
                            const cellXfs = stylesDoc.getElementsByTagName('cellXfs')[0];

                            // --- Create bold font for header
                            const headerFont = stylesDoc.createElement('font');
                            const bold = stylesDoc.createElement('b');
                            const color = stylesDoc.createElement('color');
                            color.setAttribute('rgb', 'FF000000'); // black
                            headerFont.appendChild(bold);
                            headerFont.appendChild(color);
                            fonts.appendChild(headerFont);
                            const headerFontId = fonts.childNodes.length - 1;
                            fonts.setAttribute('count', fonts.childNodes.length.toString());

                            // --- Create border for all cells
                            const border = stylesDoc.createElement('border');
                            ['left', 'right', 'top', 'bottom'].forEach(side => {
                                const sideElem = stylesDoc.createElement(side);
                                sideElem.setAttribute('style', 'thin');
                                const colorElem = stylesDoc.createElement('color');
                                colorElem.setAttribute('auto', '1');
                                sideElem.appendChild(colorElem);
                                border.appendChild(sideElem);
                            });

                            const headerStyle = stylesDoc.createElement('xf');
                            headerStyle.setAttribute('numFmtId', '0');
                            headerStyle.setAttribute('fontId', headerFontId.toString());
                            headerStyle.setAttribute('fillId', '0');
                            headerStyle.setAttribute('xfId', '0');
                            headerStyle.setAttribute('applyFont', '1');
                            headerStyle.setAttribute('applyBorder', '1');
                            headerStyle.setAttribute('applyAlignment', '1');

                            // Add alignment child
                            const alignment = stylesDoc.createElement('alignment');
                            alignment.setAttribute('horizontal', 'center');
                            alignment.setAttribute('vertical', 'center');
                            alignment.setAttribute('wrapText', '1');
                            headerStyle.appendChild(alignment);

                            cellXfs.appendChild(headerStyle);

                            // Append the new xf
                            //cellXfs.appendChild(headerAlignXf);
                            const headerStyleIndex = cellXfs.childNodes.length - 1;
                            cellXfs.setAttribute('count', cellXfs.childNodes.length.toString());



                            // Start rowIndex at 1 to insert headers at the top
                            let rowIndex = 1;

                            // 1. Add header rows from thead
                            $('#dashboard_ardifference thead tr').each(function () {
                                let colIndex = 1;
                                const trElm = sheetDoc.createElement('row');
                                trElm.setAttribute('r', rowIndex);

                                $(this).children('th, td').each(function () {
                                    const $cell = $(this);
                                    const colspan = parseInt($cell.attr('colspan')) || 1;
                                    const rowspan = parseInt($cell.attr('rowspan')) || 1;

                                    while (occupied[rowIndex + '-' + colIndex]) {
                                        colIndex++;
                                    }

                                    const startCol = colIndex;
                                    const endCol = colIndex + colspan - 1;
                                    const endRow = rowIndex + rowspan - 1;

                                    const cellRef = colLetter(startCol) + rowIndex;
                                    const c = sheetDoc.createElement('c');
                                    c.setAttribute('r', cellRef);
                                    c.setAttribute('t', 'str');
                                    //if (colIndex > 2)
                                    c.setAttribute('s', headerStyleIndex.toString());

                                    const v = sheetDoc.createElement('v');
                                    let cellText = $cell.text().trim().replace(/\s+/g, ' ');
                                    if (!cellText) cellText = ' ';
                                    v.textContent = cellText;
                                    c.appendChild(v);
                                    trElm.appendChild(c);

                                    for (let rr = rowIndex; rr <= endRow; rr++) {
                                        for (let cc = startCol; cc <= endCol; cc++) {
                                            occupied[rr + '-' + cc] = true;
                                        }
                                    }

                                    if (colspan > 1 || rowspan > 1) {
                                        mergeRanges.push(colLetter(startCol) + rowIndex + ':' + colLetter(endCol) + endRow);
                                    }

                                    colIndex += colspan;
                                });

                                sheetData.appendChild(trElm);
                                rowIndex++;
                            });


                            const dataStyle = stylesDoc.createElement('xf');
                            dataStyle.setAttribute('numFmtId', '0');
                            dataStyle.setAttribute('fontId', '0'); // default font
                            dataStyle.setAttribute('fillId', '0');
                            dataStyle.setAttribute('xfId', '0');
                            dataStyle.setAttribute('applyAlignment', '1');

                            const dataAlignment = stylesDoc.createElement('alignment');
                            dataAlignment.setAttribute('horizontal', 'center');
                            dataAlignment.setAttribute('vertical', 'center');
                            dataAlignment.setAttribute('wrapText', '1');
                            dataStyle.appendChild(dataAlignment);

                            cellXfs.appendChild(dataStyle);
                            const centerStyleIndex = cellXfs.childNodes.length - 1;
                            cellXfs.setAttribute('count', cellXfs.childNodes.length.toString());


                            // 2. Add data rows from tbody
                            $('#dashboard_ardifference tbody tr').each(function () {
                                let colIndex = 1;
                                const trElm = sheetDoc.createElement('row');
                                trElm.setAttribute('r', rowIndex);

                                $(this).children('td').each(function () {
                                    const $cell = $(this);
                                    const colspan = parseInt($cell.attr('colspan')) || 1;
                                    const rowspan = parseInt($cell.attr('rowspan')) || 1;

                                    while (occupied[rowIndex + '-' + colIndex]) {
                                        colIndex++;
                                    }

                                    const startCol = colIndex;
                                    const endCol = colIndex + colspan - 1;
                                    const endRow = rowIndex + rowspan - 1;

                                    const cellRef = colLetter(startCol) + rowIndex;
                                    const c = sheetDoc.createElement('c');
                                    c.setAttribute('r', cellRef);
                                    c.setAttribute('t', 'str');
                                    if (colIndex > 2)
                                        c.setAttribute('s', centerStyleIndex.toString());

                                    const v = sheetDoc.createElement('v');
                                    let cellText = $cell.text().trim().replace(/\s+/g, ' ');
                                    if (!cellText) cellText = ' ';
                                    v.textContent = cellText;
                                    c.appendChild(v);
                                    trElm.appendChild(c);

                                    for (let rr = rowIndex; rr <= endRow; rr++) {
                                        for (let cc = startCol; cc <= endCol; cc++) {
                                            occupied[rr + '-' + cc] = true;
                                        }
                                    }

                                    if (colspan > 1 || rowspan > 1) {
                                        mergeRanges.push(colLetter(startCol) + rowIndex + ':' + colLetter(endCol) + endRow);
                                    }

                                    colIndex += colspan;
                                });

                                sheetData.appendChild(trElm);
                                rowIndex++;
                            });

                            // Add mergeCells element if any merges needed
                            if (mergeRanges.length > 0) {
                                const mergeCells = sheetDoc.createElement('mergeCells');
                                mergeCells.setAttribute('count', mergeRanges.length);

                                mergeRanges.forEach(range => {
                                    const mergeCell = sheetDoc.createElement('mergeCell');
                                    mergeCell.setAttribute('ref', range);
                                    mergeCells.appendChild(mergeCell);
                                });

                                if (sheetData.nextSibling) {
                                    worksheet.insertBefore(mergeCells, sheetData.nextSibling);
                                } else {
                                    worksheet.appendChild(mergeCells);
                                }
                            }


                        }




                    },
                ],

                footerCallback: function (row, data, start, end, display) {
                    let api = this.api();
                    var oddeven_foot = 1;
                    // Remove the formatting to get integer data for summation
                    let intVal = function (i) {
                        return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0;
                    };

                    var totalLoan = 0;
                    var totalAmt = 0;
                    for (var i = 3; i < api.columns().count(); i++) {
                        totalLoan = api.column(i, { page: 'current' }).data().reduce((a, b) => intVal(a) + intVal(b), 0);
                        if (oddeven_foot == 1 || oddeven_foot == 3 || oddeven_foot == 5) {
                            document.getElementById("lblDiff_" + (i)).innerHTML = "Count (" + new Intl.NumberFormat().format(Number(totalLoan)) + ")";
                            oddeven_foot++;
                        }
                        else if (oddeven_foot == 2 || oddeven_foot == 4 || oddeven_foot == 6) {
                            totalAmt = api.column(i, { page: 'current' }).data().reduce((a, b) => intVal(a) + intVal(b), 0);
                            document.getElementById("lblDiff_" + (i)).innerHTML = "US $ (" + new Intl.NumberFormat().format(Number(totalAmt).toFixed(2)) + ")";
                            oddeven_foot++;
                        }
                        else if (oddeven_foot == 7) {
                            oddeven_foot++;
                        }
                        else {
                            if (oddeven_foot == 8) {
                                oddeven_foot = 1;
                            }
                        }

                        //api.column(i).footer().innerHTML = Number(totalLoan);
                        //api.column(i).footer().innerHTML = Number(totalAmt).toFixed(2);

                    }
                }


            });

        },
        error: function (error) {
            CanopyUI.showError(error, 'Unable to complete request.');
        }
    });
    return false;
}

function dashboard_arBind_DifferenceQuickbook_BillNo_FinalWorking_WithoutYTD() {
    $('#load1').show();
    var columns = [];
    var columnlink = [];
    var preheader = '';
    var headers = [];
    var returnstring = '';
    var htmlheader = '';
    var htmlrow = '';
    $.ajax({
        url: "Dashboard.aspx/GetClientwiseDashboard_DifferencewithQuickbook",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//
            var firstData = dataArray[0];
            var rowData = dataArray[1];
            var ColumnNames = Object.keys(firstData);
            var rowValues = Object.keys(rowData);

            htmlheader = '<thead><tr>';
            //Main Header
            var thead = document.createElement("thead");
            var trh = document.createElement("tr");
            for (var i = 1; i < ColumnNames.length; i++) {
                if (i == 2 || i == 3) {
                    var tbh = document.createElement("th");
                    tbh.rowSpan = 3;
                    tbh.style.fontWeight = "bold!important";
                    tbh.style.verticalAlign = "middle";
                    if (ColumnNames[i] == "Process")
                        tbh.style.width = "180px";
                    else
                        tbh.style.width = "250px";
                    tbh.style.left = "0px!important";
                    tbh.innerHTML = ColumnNames[i];
                    trh.appendChild(tbh);
                    //thead.appendChild(trh);
                    //htmlheader = htmlheader + '<th style="left: 0px!important;">Client</th>';
                }
                else if (i > 1) {
                    if (i % 2 == 0) {

                        var tbh = document.createElement("th");
                        if (ColumnNames[i].includes('Total'))
                            tbh.colSpan = 4;
                        else
                            tbh.colSpan = 9;
                        tbh.style.textAlign = "center";
                        tbh.style.borderRight = "Solid 2px gray";
                        tbh.innerHTML = ColumnNames[i].replace("-ERP-LoanCount", "").replace("-ERP-Amount", "").replace("-QB-LoanCount", "").replace("-QB-Amount", "").replace("-Difference-LoanCount", "").replace("-Difference-Amount", "").replace("-Actions", "").replace("-InvocieNo", "");
                        trh.appendChild(tbh);
                        i = i + 8;
                    }
                }
                else {
                    var tbh = document.createElement("th");
                    tbh.rowSpan = 3;
                    tbh.style.display = 'none';
                    tbh.innerHTML = ColumnNames[i];
                    //htmlheader = htmlheader + '<th style="display:none;">' + ColumnNames[i] + '</th>';
                    trh.appendChild(tbh);
                    //thead.appendChild(trh);
                }
            }
            thead.appendChild(trh);

            //seocnd header row
            trh = document.createElement("tr");
            var oddeven = 1;
            for (var i = 1; i < ColumnNames.length; i++) {
                if (i == 2 || i == 3) {
                }
                else if (i > 1) {
                    if (oddeven == 1) {
                        var tbh = document.createElement("th");
                        tbh.colSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.innerHTML = "ERP";
                        trh.appendChild(tbh);
                        oddeven++;
                        i = i + 1;
                    }
                    else if (oddeven == 2) {
                        var tbh = document.createElement("th");
                        tbh.colSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.innerHTML = "Quickbook";
                        //tbh.style.color = "green";
                        trh.appendChild(tbh);
                        oddeven++;
                        i = i + 1;
                    }
                    else if (oddeven == 3) {
                        var tbh = document.createElement("th");
                        tbh.colSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.innerHTML = "Difference";
                        //tbh.style.color = "red";
                        trh.appendChild(tbh);
                        if (ColumnNames[i].includes('Total')) {
                            oddeven = 1
                        }
                        else {
                            oddeven++;
                            i = i + 1;
                        }
                        //if (oddeven == 3) {
                        //    oddeven = 1;
                        //}
                    }
                    else if (oddeven == 4) {
                        var tbh = document.createElement("th");
                        tbh.rowSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.style.verticalAlign = "middle";
                        tbh.innerHTML = "Invoice #";
                        //tbh.style.color = "red";
                        trh.appendChild(tbh);
                        oddeven++;
                        //i = i + 1;
                        //if (oddeven == 3) {
                        //    oddeven = 1;
                        //}
                    }
                    else if (oddeven == 5) {
                        var tbh = document.createElement("th");
                        tbh.rowSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.style.verticalAlign = "middle";
                        tbh.innerHTML = "Actions";
                        //tbh.style.color = "red";
                        trh.appendChild(tbh);
                        oddeven++;
                        //i = i + 1;
                        //if (oddeven == 3) {
                        //    oddeven = 1;
                        //}
                    }
                    else {
                        //alert(oddeven);
                        var tbh = document.createElement("th");
                        tbh.rowSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.style.verticalAlign = "middle";
                        tbh.innerHTML = "Remark";
                        tbh.style.borderRight = "Solid 2px gray";
                        //tbh.style.color = "red";
                        trh.appendChild(tbh);
                        //oddeven++;
                        //i = i + 1;
                        if (oddeven == 6) {
                            oddeven = 1;
                        }
                    }

                }
                else {

                    //var tbh = document.createElement("th");
                    //tbh.style.display = 'none';
                    //tbh.innerHTML = ColumnNames[i];
                    ////htmlheader = htmlheader + '<th style="display:none;">' + ColumnNames[i] + '</th>';
                    //trh.appendChild(tbh);
                    //thead.appendChild(trh);
                }
            }
            thead.appendChild(trh);
            // $('#dashboard_ar').append(thead);

            //Third header row
            var trh1 = document.createElement("tr");
            //htmlheader = htmlheader + '</tr><tr>';
            var oddeven_3 = 1;
            for (var i = 1; i < ColumnNames.length; i++) {
                if (i == 2 || i == 3) {
                    //var tbr = document.createElement("th");
                    //tbr.innerHTML = ColumnNames[i];
                    //tbr.style.textAlign = "center";
                    //trh1.appendChild(tbr);
                    ////htmlheader = htmlheader + '<th style="left: 0px!important; ">' + ColumnNames[i] + '</th>';
                }
                else if (i > 1) {
                    if (oddeven_3 == 1 || oddeven_3 == 3 || oddeven_3 == 5) {
                        var tbr = document.createElement("th");
                        tbr.style.textAlign = "center";
                        tbr.innerHTML = 'Count';
                        tbr.id = "lblDiff_" + (i - 1);
                        trh1.appendChild(tbr);
                        oddeven_3++;
                        //i = i + 1;
                    }
                    else if (oddeven_3 == 2 || oddeven_3 == 4 || oddeven_3 == 6) {
                        var tbr = document.createElement("th");
                        tbr.style.textAlign = "center";
                        tbr.innerHTML = 'US $';
                        tbr.id = "lblDiff_" + (i - 1);
                        trh1.appendChild(tbr);
                        oddeven_3++;
                        //i = i + 1;
                    }
                    else if (oddeven_3 == 7) {
                        oddeven_3++;
                    }
                    else if (oddeven_3 == 8) {
                        oddeven_3++;
                    }
                    else if (oddeven_3 == 9) {
                        oddeven_3 = 1;
                    }


                }
                //if (i % 2 != 0) {
                //    var tbr = document.createElement("th");
                //    tbr.style.textAlign = "center";
                //    tbr.innerHTML = 'US $';
                //    tbr.id = "lblDiff_" + (i - 1);
                //    trh1.appendChild(tbr);
                //}
                //else {
                //    var tbr = document.createElement("th");
                //    tbr.style.textAlign = "center";
                //    tbr.innerHTML = 'Count';
                //    tbr.id = "lblDiff_" + (i - 1);
                //    trh1.appendChild(tbr);
                //}
                else {
                }
            }
            thead.appendChild(trh1);
            $('#dashboard_ardifference').append(thead);
            //document.getElementById("newlabl").innerText = thead.innerHTML;
            var tbody = document.createElement("tbody");
            $.each(dataArray, function (index, item) {
                var tr1 = document.createElement("tr");
                //htmlrow = htmlrow + '<tr>';
                for (var i = 1; i < ColumnNames.length; i++) {
                    var targetColumnName = ColumnNames[i];
                    if (i == 2 || i == 3) {
                        var td = document.createElement("td");
                        //var label = document.createElement("label");
                        //label.id = i + "_" + item[targetColumnName].replace(" ", "") + "ClientLabel";
                        //label.innerHTML = item[targetColumnName];
                        //if (ColumnNames[i] == "Process")
                        //    label.style.width = "180px";
                        //else
                        //    label.style.width = "250px";
                        /*label.style.width = "250px";*/
                        //td.appendChild(label);
                        td.innerHTML = item[targetColumnName];
                        tr1.appendChild(td);
                    }
                    else if (i > 1) {
                        //if (i % 2 == 0) {
                        var td = document.createElement("td");
                        td.style.textAlign = "center";

                        //td.style.backColor = "green";

                        if (targetColumnName.includes("Action")) {
                            td.style.borderRight = "Solid 2px gray!important";
                            var td = document.createElement("td");
                            var link = '<a class="dropdown-item" title="Send Invoice To Client" href="#!" id="Actions" onclick="dashboard_AddRemark(\'' + index + '\',\'' + targetColumnName + '\');" style="width:30px; display:inline;padding: .25rem .25rem!important;"><img src="../Images/edit.png" style="width:20px; display:inline;" /></a>';
                            td.innerHTML = link;
                            tr1.appendChild(td);
                        }
                        else
                            td.innerHTML = item[targetColumnName];

                        tr1.appendChild(td);

                    }
                    else {
                        var td = document.createElement("td");
                        td.style.display = 'none';
                        td.innerHTML = item[targetColumnName];
                        tr1.appendChild(td);
                    }
                }
                tbody.appendChild(tr1);
                $('#dashboard_ardifference').append(tbody);
            });


            //Footer
            var tfoot = document.createElement("tfoot");
            var trf1 = document.createElement("tr");
            for (var i = 1; i < ColumnNames.length; i++) {
                if (i == 2 || i == 3) {
                    var tbr = document.createElement("th");
                    tbr.style.textAlign = "center";
                    trf1.appendChild(tbr);
                }
                else if (i > 1) {
                    var tbr = document.createElement("th");
                    tbr.style.textAlign = "center";
                    trf1.appendChild(tbr);
                }
                else {
                    var tbr = document.createElement("th");
                    tbr.style.display = 'none';
                    trf1.appendChild(tbr);
                }
            }
            tfoot.appendChild(trf1);
            $('#dashboard_ardifference').append(tfoot);

            if ($.fn.DataTable.isDataTable('#dashboard_ardifference')) {
                $('#dashboard_ardifference').DataTable().clear().destroy();
            }

            $('#dashboard_ardifference').DataTable({
                dom: 'Bft',
                destroy: true,
                orderCellsTop: true,
                fixedColumns: {
                    leftColumns: 2,
                },
                scrollCollapse: false,
                scrollY: '400px',
                scrollX: true,
                "paging": false,
                "autoWidth": true,
                select: true,
                "ordering": false,
                filter: true,
                'select': {
                    'style': 'single'
                },
                "serverSide": false,

                initComplete: function () {
                    $('#load1').hide();
                },
                columnDefs: [
                    { targets: 0, visible: false }  // hides column index 2
                ],
                buttons: [
                    {
                        extend: 'excelHtml5',
                        text: 'Export to Excel',
                        filename: 'Comparison with Quickbok',
                        exportOptions: {
                            columns: ':visible',
                            modifier: { header: false }
                        },
                        customize: function (xlsx) {
                            const sheetDoc = xlsx.xl.worksheets['sheet1.xml'];
                            const worksheet = sheetDoc.documentElement;
                            const sheetData = worksheet.getElementsByTagName('sheetData')[0];
                            const $worksheet = $(worksheet);


                            // Remove all existing rows (clean slate)
                            $worksheet.find('row').remove();

                            // Remove existing mergeCells if any
                            $worksheet.find('mergeCells').remove();

                            function colLetter(n) {
                                let s = '', t;
                                while (n > 0) {
                                    t = (n - 1) % 26;
                                    s = String.fromCharCode(65 + t) + s;
                                    n = Math.floor((n - 1) / 26);
                                }
                                return s;
                            }

                            const occupied = {};
                            const mergeRanges = [];

                            const stylesDoc = xlsx.xl['styles.xml'];
                            const fonts = stylesDoc.getElementsByTagName('fonts')[0];
                            const borders = stylesDoc.getElementsByTagName('borders')[0];
                            const cellXfs = stylesDoc.getElementsByTagName('cellXfs')[0];

                            // --- Create bold font for header
                            const headerFont = stylesDoc.createElement('font');
                            const bold = stylesDoc.createElement('b');
                            const color = stylesDoc.createElement('color');
                            color.setAttribute('rgb', 'FF000000'); // black
                            headerFont.appendChild(bold);
                            headerFont.appendChild(color);
                            fonts.appendChild(headerFont);
                            const headerFontId = fonts.childNodes.length - 1;
                            fonts.setAttribute('count', fonts.childNodes.length.toString());

                            // --- Create border for all cells
                            const border = stylesDoc.createElement('border');
                            ['left', 'right', 'top', 'bottom'].forEach(side => {
                                const sideElem = stylesDoc.createElement(side);
                                sideElem.setAttribute('style', 'thin');
                                const colorElem = stylesDoc.createElement('color');
                                colorElem.setAttribute('auto', '1');
                                sideElem.appendChild(colorElem);
                                border.appendChild(sideElem);
                            });

                            const headerStyle = stylesDoc.createElement('xf');
                            headerStyle.setAttribute('numFmtId', '0');
                            headerStyle.setAttribute('fontId', headerFontId.toString());
                            headerStyle.setAttribute('fillId', '0');
                            headerStyle.setAttribute('xfId', '0');
                            headerStyle.setAttribute('applyFont', '1');
                            headerStyle.setAttribute('applyBorder', '1');
                            headerStyle.setAttribute('applyAlignment', '1');

                            // Add alignment child
                            const alignment = stylesDoc.createElement('alignment');
                            alignment.setAttribute('horizontal', 'center');
                            alignment.setAttribute('vertical', 'center');
                            alignment.setAttribute('wrapText', '1');
                            headerStyle.appendChild(alignment);

                            cellXfs.appendChild(headerStyle);

                            // Append the new xf
                            //cellXfs.appendChild(headerAlignXf);
                            const headerStyleIndex = cellXfs.childNodes.length - 1;
                            cellXfs.setAttribute('count', cellXfs.childNodes.length.toString());



                            // Start rowIndex at 1 to insert headers at the top
                            let rowIndex = 1;

                            // 1. Add header rows from thead
                            $('#dashboard_ardifference thead tr').each(function () {
                                let colIndex = 1;
                                const trElm = sheetDoc.createElement('row');
                                trElm.setAttribute('r', rowIndex);

                                $(this).children('th, td').each(function () {
                                    const $cell = $(this);
                                    const colspan = parseInt($cell.attr('colspan')) || 1;
                                    const rowspan = parseInt($cell.attr('rowspan')) || 1;

                                    while (occupied[rowIndex + '-' + colIndex]) {
                                        colIndex++;
                                    }

                                    const startCol = colIndex;
                                    const endCol = colIndex + colspan - 1;
                                    const endRow = rowIndex + rowspan - 1;

                                    const cellRef = colLetter(startCol) + rowIndex;
                                    const c = sheetDoc.createElement('c');
                                    c.setAttribute('r', cellRef);
                                    c.setAttribute('t', 'str');
                                    //if (colIndex > 2)
                                    c.setAttribute('s', headerStyleIndex.toString());

                                    const v = sheetDoc.createElement('v');
                                    let cellText = $cell.text().trim().replace(/\s+/g, ' ');
                                    if (!cellText) cellText = ' ';
                                    v.textContent = cellText;
                                    c.appendChild(v);
                                    trElm.appendChild(c);

                                    for (let rr = rowIndex; rr <= endRow; rr++) {
                                        for (let cc = startCol; cc <= endCol; cc++) {
                                            occupied[rr + '-' + cc] = true;
                                        }
                                    }

                                    if (colspan > 1 || rowspan > 1) {
                                        mergeRanges.push(colLetter(startCol) + rowIndex + ':' + colLetter(endCol) + endRow);
                                    }

                                    colIndex += colspan;
                                });

                                sheetData.appendChild(trElm);
                                rowIndex++;
                            });


                            const dataStyle = stylesDoc.createElement('xf');
                            dataStyle.setAttribute('numFmtId', '0');
                            dataStyle.setAttribute('fontId', '0'); // default font
                            dataStyle.setAttribute('fillId', '0');
                            dataStyle.setAttribute('xfId', '0');
                            dataStyle.setAttribute('applyAlignment', '1');

                            const dataAlignment = stylesDoc.createElement('alignment');
                            dataAlignment.setAttribute('horizontal', 'center');
                            dataAlignment.setAttribute('vertical', 'center');
                            dataAlignment.setAttribute('wrapText', '1');
                            dataStyle.appendChild(dataAlignment);

                            cellXfs.appendChild(dataStyle);
                            const centerStyleIndex = cellXfs.childNodes.length - 1;
                            cellXfs.setAttribute('count', cellXfs.childNodes.length.toString());


                            // 2. Add data rows from tbody
                            $('#dashboard_ardifference tbody tr').each(function () {
                                let colIndex = 1;
                                const trElm = sheetDoc.createElement('row');
                                trElm.setAttribute('r', rowIndex);

                                $(this).children('td').each(function () {
                                    const $cell = $(this);
                                    const colspan = parseInt($cell.attr('colspan')) || 1;
                                    const rowspan = parseInt($cell.attr('rowspan')) || 1;

                                    while (occupied[rowIndex + '-' + colIndex]) {
                                        colIndex++;
                                    }

                                    const startCol = colIndex;
                                    const endCol = colIndex + colspan - 1;
                                    const endRow = rowIndex + rowspan - 1;

                                    const cellRef = colLetter(startCol) + rowIndex;
                                    const c = sheetDoc.createElement('c');
                                    c.setAttribute('r', cellRef);
                                    c.setAttribute('t', 'str');
                                    if (colIndex > 2)
                                        c.setAttribute('s', centerStyleIndex.toString());

                                    const v = sheetDoc.createElement('v');
                                    let cellText = $cell.text().trim().replace(/\s+/g, ' ');
                                    if (!cellText) cellText = ' ';
                                    v.textContent = cellText;
                                    c.appendChild(v);
                                    trElm.appendChild(c);

                                    for (let rr = rowIndex; rr <= endRow; rr++) {
                                        for (let cc = startCol; cc <= endCol; cc++) {
                                            occupied[rr + '-' + cc] = true;
                                        }
                                    }

                                    if (colspan > 1 || rowspan > 1) {
                                        mergeRanges.push(colLetter(startCol) + rowIndex + ':' + colLetter(endCol) + endRow);
                                    }

                                    colIndex += colspan;
                                });

                                sheetData.appendChild(trElm);
                                rowIndex++;
                            });

                            // Add mergeCells element if any merges needed
                            if (mergeRanges.length > 0) {
                                const mergeCells = sheetDoc.createElement('mergeCells');
                                mergeCells.setAttribute('count', mergeRanges.length);

                                mergeRanges.forEach(range => {
                                    const mergeCell = sheetDoc.createElement('mergeCell');
                                    mergeCell.setAttribute('ref', range);
                                    mergeCells.appendChild(mergeCell);
                                });

                                if (sheetData.nextSibling) {
                                    worksheet.insertBefore(mergeCells, sheetData.nextSibling);
                                } else {
                                    worksheet.appendChild(mergeCells);
                                }
                            }


                        }




                    },
                ],

                footerCallback: function (row, data, start, end, display) {
                    let api = this.api();
                    const columnsCount = api.columns().count();
                    var oddeven_foot = 1;
                    // Remove the formatting to get integer data for summation
                    let intVal = function (i) {
                        return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0;
                    };

                    var totalLoan = 0;
                    var totalAmt = 0;
                    for (let i = 3; i < columnsCount; i++) {
                        const colData = api.column(i, { page: 'current' }).data();

                        // Skip if column has no data
                        if (!colData || colData.length === 0) continue;

                        // Pre-sum the column
                        const total = colData.reduce((a, b) => intVal(a) + intVal(b), 0);

                        // Get label element once
                        const lbl = document.getElementById("lblDiff_" + (i));
                        if (!lbl) continue;

                        // Determine label type based on column index pattern
                        const position = (i - 3) % 6;

                        if (position === 0 || position === 2 || position === 4) {
                            // Loan count type
                            lbl.innerHTML = 'Count (' + new Intl.NumberFormat().format(total) + ')';
                        } else if (position === 1 || position === 3 || position === 5) {
                            // Amount type
                            lbl.innerHTML = 'US $(' + new Intl.NumberFormat().format(total.toFixed(2)) + ')';
                        } else {
                            // Optional: handle other cases
                            lbl.innerHTML = '';
                        }
                    }
                }


            });

        },
        error: function (error) {
            CanopyUI.showError(error, 'Unable to complete request.');
        }
    });

    $('#dashboard_ardifference').on('click', '.buttons-excel', function () {
        let table = $('#dashboard_ardifference').DataTable();
        let button = table.button('.buttons-excel');
        button.conf.customize = function (xlsx) {
            // your custom export logic here...
        };
    });
    return false;
}

function dashboard_arBind_DifferenceQuickbook_BillNo() {
    $('#load1').show();
    var columns = [];
    var columnlink = [];
    var preheader = '';
    var headers = [];
    var returnstring = '';
    var htmlheader = '';
    var htmlrow = '';
    $.ajax({
        url: "Dashboard.aspx/GetClientwiseDashboard_DifferencewithQuickbook",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//
            var firstData = dataArray[0];
            var rowData = dataArray[1];
            var ColumnNames = Object.keys(firstData);
            var rowValues = Object.keys(rowData);

            htmlheader = '<thead><tr>';
            //Main Header
            var thead = document.createElement("thead");
            var trh = document.createElement("tr");
            for (var i = 1; i < ColumnNames.length; i++) {
                if (i == 2 || i == 3) {
                    var tbh = document.createElement("th");
                    tbh.rowSpan = 3;
                    tbh.style.fontWeight = "bold!important";
                    tbh.style.verticalAlign = "middle";
                    if (ColumnNames[i] == "Process")
                        tbh.style.width = "180px";
                    else
                        tbh.style.width = "250px";
                    tbh.style.left = "0px!important";
                    tbh.innerHTML = ColumnNames[i];
                    trh.appendChild(tbh);
                    //thead.appendChild(trh);
                    //htmlheader = htmlheader + '<th style="left: 0px!important;">Client</th>';
                }
                else if (i > 1) {

                    //if (i % 2 == 0) {

                    var tbh = document.createElement("th");
                    if (ColumnNames[i].includes('Total'))
                        tbh.colSpan = 4;
                    else if (ColumnNames[i].includes('YTD')) {
                        tbh.colSpan = 6;
                        tbh.style.textAlign = "center";
                        tbh.style.borderRight = "Solid 2px gray";
                        tbh.innerHTML = ColumnNames[i].replace("-ERP-LoanCount", "").replace("-ERP-Amount", "").replace("-QB-LoanCount", "").replace("-QB-Amount", "").replace("-Difference-LoanCount", "").replace("-Difference-Amount", "").replace("-Actions", "").replace("-InvoiceNo", "");
                        trh.appendChild(tbh);
                        i = i + 5;
                    }
                    else {
                        tbh.colSpan = 9;
                        tbh.style.textAlign = "center";
                        tbh.style.borderRight = "Solid 2px gray";
                        tbh.innerHTML = ColumnNames[i].replace("-ERP-LoanCount", "").replace("-ERP-Amount", "").replace("-QB-LoanCount", "").replace("-QB-Amount", "").replace("-Difference-LoanCount", "").replace("-Difference-Amount", "").replace("-Actions", "").replace("-InvoiceNo", "");
                        trh.appendChild(tbh);
                        i = i + 8;
                    }


                    //}
                }
                //else if (i == ColumnNames.length - 1) {
                //    tbh.colSpan = 9;
                //    tbh.style.textAlign = "center";
                //    tbh.style.borderRight = "Solid 2px gray";
                //    tbh.innerHTML = ColumnNames[i].replace("-ERP-LoanCount", "").replace("-ERP-Amount", "").replace("-QB-LoanCount", "").replace("-QB-Amount", "").replace("-Difference-LoanCount", "").replace("-Difference-Amount", "").replace("-Actions", "").replace("-InvoiceNo", "");
                //    trh.appendChild(tbh);
                //}
                else {
                    var tbh = document.createElement("th");
                    tbh.rowSpan = 3;
                    tbh.style.display = 'none';
                    tbh.innerHTML = ColumnNames[i];
                    //htmlheader = htmlheader + '<th style="display:none;">' + ColumnNames[i] + '</th>';
                    trh.appendChild(tbh);
                    //thead.appendChild(trh);
                }
            }
            thead.appendChild(trh);

            //seocnd header row
            trh = document.createElement("tr");
            var oddeven = 1;
            for (var i = 1; i < ColumnNames.length; i++) {
                if (i == 2 || i == 3) {
                }
                else if (i > 1) {
                    if (oddeven == 1) {
                        var tbh = document.createElement("th");
                        tbh.colSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.innerHTML = "ERP";
                        trh.appendChild(tbh);
                        oddeven++;
                        i = i + 1;
                    }
                    else if (oddeven == 2) {
                        var tbh = document.createElement("th");
                        tbh.colSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.innerHTML = "Quickbook";
                        //tbh.style.color = "green";
                        trh.appendChild(tbh);
                        oddeven++;
                        i = i + 1;
                    }
                    else if (oddeven == 3) {
                        var tbh = document.createElement("th");
                        tbh.colSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.innerHTML = "Difference";
                        //tbh.style.borderRight = "Solid 2px gray";
                        //tbh.style.color = "red";
                        if (ColumnNames[i].includes("YTD")) {
                            oddeven = 1;
                            tbh.style.borderRight = "Solid 2px gray";
                        }
                        else
                            oddeven++;
                        trh.appendChild(tbh);
                        i = i + 1;
                        //if (oddeven == 3) {
                        //    oddeven = 1;
                        //}
                    }
                    else if (oddeven == 4) {
                        var tbh = document.createElement("th");
                        tbh.rowSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.style.verticalAlign = "middle";
                        tbh.innerHTML = "Invoice #";
                        if (ColumnNames[i].includes("YTD"))
                            tbh.style.display = "none";
                        //tbh.style.color = "red";
                        trh.appendChild(tbh);
                        oddeven++;
                        //i = i + 1;
                        //if (oddeven == 3) {
                        //    oddeven = 1;
                        //}
                    }
                    else if (oddeven == 5) {
                        var tbh = document.createElement("th");
                        tbh.rowSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.style.verticalAlign = "middle";
                        tbh.innerHTML = "Actions";
                        if (ColumnNames[i].includes("YTD"))
                            tbh.style.display = "none";
                        //tbh.style.color = "red";
                        trh.appendChild(tbh);
                        oddeven++;
                        //i = i + 1;
                        //if (oddeven == 3) {
                        //    oddeven = 1;
                        //}
                    }
                    else {
                        //alert(oddeven);
                        var tbh = document.createElement("th");
                        tbh.rowSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.style.verticalAlign = "middle";
                        tbh.innerHTML = "Remark";
                        tbh.style.borderRight = "Solid 2px gray";
                        if (ColumnNames[i].includes("YTD"))
                            tbh.style.display = "none";
                        //tbh.style.color = "red";
                        trh.appendChild(tbh);
                        //oddeven++;
                        //i = i + 1;
                        if (oddeven == 6) {
                            oddeven = 1;
                        }
                    }

                }
                else {

                    //var tbh = document.createElement("th");
                    //tbh.style.display = 'none';
                    //tbh.innerHTML = ColumnNames[i];
                    ////htmlheader = htmlheader + '<th style="display:none;">' + ColumnNames[i] + '</th>';
                    //trh.appendChild(tbh);
                    //thead.appendChild(trh);
                }
            }
            thead.appendChild(trh);
            // $('#dashboard_ar').append(thead);

            //Third header row
            var trh1 = document.createElement("tr");
            //htmlheader = htmlheader + '</tr><tr>';
            var oddeven_3 = 1;
            for (var i = 1; i < ColumnNames.length; i++) {
                if (i == 2 || i == 3) {
                    //var tbr = document.createElement("th");
                    //tbr.innerHTML = ColumnNames[i];
                    //tbr.style.textAlign = "center";
                    //trh1.appendChild(tbr);
                    ////htmlheader = htmlheader + '<th style="left: 0px!important; ">' + ColumnNames[i] + '</th>';
                }
                else if (i > 1) {
                    if (oddeven_3 == 1 || oddeven_3 == 3 || oddeven_3 == 5) {
                        var tbr = document.createElement("th");
                        tbr.style.textAlign = "center";
                        tbr.innerHTML = 'Count';
                        tbr.id = "lblDiff_" + (i - 1);
                        trh1.appendChild(tbr);
                        oddeven_3++;
                        //i = i + 1;
                    }
                    else if (oddeven_3 == 2 || oddeven_3 == 4 || oddeven_3 == 6) {
                        var tbr = document.createElement("th");
                        tbr.style.textAlign = "center";
                        tbr.innerHTML = 'US $';
                        tbr.id = "lblDiff_" + (i - 1);
                        //tbr.style.borderRight = "Solid 2px gray";

                        if (ColumnNames[i].includes("YTD") && oddeven_3 == 6) {
                            oddeven_3 = 1;
                            tbr.style.borderRight = "Solid 2px gray";
                        }
                        else
                            oddeven_3++;
                        trh1.appendChild(tbr);
                        //i = i + 1;
                    }
                    else if (oddeven_3 == 7) {
                        //if (ColumnNames[i].includes("YTD"))
                        //    oddeven_3 = 1;
                        //else
                        oddeven_3++;
                    }
                    else if (oddeven_3 == 8) {
                        //if (ColumnNames[i].includes("YTD"))
                        //    oddeven_3 = 1;
                        //else
                        oddeven_3++;
                    }
                    else if (oddeven_3 == 9) {
                        oddeven_3 = 1;
                    }


                }
                //if (i % 2 != 0) {
                //    var tbr = document.createElement("th");
                //    tbr.style.textAlign = "center";
                //    tbr.innerHTML = 'US $';
                //    tbr.id = "lblDiff_" + (i - 1);
                //    trh1.appendChild(tbr);
                //}
                //else {
                //    var tbr = document.createElement("th");
                //    tbr.style.textAlign = "center";
                //    tbr.innerHTML = 'Count';
                //    tbr.id = "lblDiff_" + (i - 1);
                //    trh1.appendChild(tbr);
                //}
                else {
                }
            }
            thead.appendChild(trh1);
            $('#dashboard_ardifference').append(thead);
            // document.getElementById("newlabl").innerText = thead.innerHTML;
            var tbody = document.createElement("tbody");
            $.each(dataArray, function (index, item) {
                var tr1 = document.createElement("tr");
                //htmlrow = htmlrow + '<tr>';
                for (var i = 1; i < ColumnNames.length; i++) {
                    var targetColumnName = ColumnNames[i];
                    if (i == 2 || i == 3) {
                        var td = document.createElement("td");
                        td.innerHTML = item[targetColumnName];
                        tr1.appendChild(td);
                    }
                    else if (i > 1) {
                        var td = document.createElement("td");
                        td.style.textAlign = "center";
                        if (targetColumnName.includes("Action")) {
                            var td = document.createElement("td");
                            td.style.textAlign = "center";
                            var link = '';
                            /*var link = '<a class="dropdown-item" title="Send Invoice To Client" href="#!" id="Actions" onclick="dashboard_AddRemark(\'' + index + '\',\'' + targetColumnName + '\');" style="width:30px; display:inline;padding: .25rem .25rem!important;"><img src="../Images/edit.png" style="width:20px; display:inline;" /></a>';*/
                            if (blankForNull(item[targetColumnName]) == "Yes")
                                link = '<div class="btn-group"><div class="btn-group"><div type="button" data-toggle="dropdown" aria-expanded="false"><i class="uil fs-0 me-2 uil-file-check" style="color:green;"></i><i style="color: dodgerblue; font-size:14px;" class="uil fs-0 me-2 uil-cog"></i>';
                            else
                                link = '<div class="btn-group"><div class="btn-group"><div type="button" data-toggle="dropdown" aria-expanded="false"><i style="color: dodgerblue; font-size:14px;" class="uil fs-0 me-2 uil-cog"></i>';

                            link = link + '<span class="sr-only"></span></div><div class="dropdown-menu" role="menu" style="">';
                            link = link + '<a class="dropdown-item" title="Add Reconciliation Remark" href="#!" id="Actions" onclick="dashboard_AddRemark(\'' + index + '\',\'' + targetColumnName + '\');"><span style="color: green;"><i class="uil fs-0 me-2 uil-pen"></i></span>&nbsp; Add Remark</a>';
                            link = link + '<a class="dropdown-item" href="#!" title="Upload Excel Attachment" onclick="dashboard_AddAttachment(\'' + index + '\',\'' + targetColumnName + '\');"><span style="color: dodgerblue;"><i class="uil fs-0 me-2 uil-file"></i></span>&nbsp;&nbsp;Upload Attachment</a>';
                            link = link + '<a class="dropdown-item" title="Download Excel Attachment" href="#!" onclick="dashboard_DownloadAttachment(\'' + index + '\',\'' + targetColumnName + '\');" id="DownloadAtt"><span style="color: brown;"><i class="uil fs-0 me-2 uil-download-alt"></i></span>&nbsp;&nbsp;Download Attachment</a></div></div></td>';
                            td.innerHTML = link;
                            tr1.appendChild(td);
                        }
                        else {
                            if (targetColumnName.includes("Remark"))
                                td.style.borderRight = "Solid 2px gray";
                            else if (targetColumnName.includes("YTD") && targetColumnName.includes("Difference") && targetColumnName.includes("Amount")) {
                                td.style.borderRight = "Solid 2px gray";
                                td.style.setProperty("background-color", "#e2e2e2", "important");
                            }
                            else if (targetColumnName.includes("YTD"))
                                td.style.setProperty("background-color", "#e2e2e2", "important");

                            if (targetColumnName.includes("Count") || targetColumnName.includes("Amount")) {
                                td.innerHTML = new Intl.NumberFormat().format(Number(item[targetColumnName]))
                            }
                            else
                                td.innerHTML = item[targetColumnName];

                        }

                        tr1.appendChild(td);

                    }
                    else {
                        var td = document.createElement("td");
                        td.style.display = 'none';
                        td.innerHTML = item[targetColumnName];
                        tr1.appendChild(td);
                    }
                }
                tbody.appendChild(tr1);
                $('#dashboard_ardifference').append(tbody);
            });


            //Footer
            var tfoot = document.createElement("tfoot");
            var trf1 = document.createElement("tr");
            for (var i = 1; i < ColumnNames.length; i++) {
                if (i == 2 || i == 3) {
                    var tbr = document.createElement("th");
                    tbr.style.textAlign = "center";
                    trf1.appendChild(tbr);
                }
                else if (i > 1) {
                    var tbr = document.createElement("th");
                    tbr.style.textAlign = "center";
                    trf1.appendChild(tbr);
                }
                else {
                    var tbr = document.createElement("th");
                    tbr.style.display = 'none';
                    trf1.appendChild(tbr);
                }
            }
            tfoot.appendChild(trf1);
            $('#dashboard_ardifference').append(tfoot);

            if ($.fn.DataTable.isDataTable('#dashboard_ardifference')) {
                $('#dashboard_ardifference').DataTable().clear().destroy();
            }

            $('#dashboard_ardifference').DataTable({
                dom: 'Bft',
                destroy: true,
                orderCellsTop: true,
                fixedColumns: {
                    leftColumns: 2,
                },
                scrollCollapse: false,
                scrollY: '400px',
                scrollX: true,
                "paging": false,
                "autoWidth": false,
                select: true,
                "ordering": false,
                filter: true,
                'select': {
                    'style': 'single'
                },
                "serverSide": false,

                initComplete: function () {
                    $('#load1').hide();
                },
                columnDefs: [
                    {
                        targets: 0,
                        width: '200px',
                        className: 'dt-hidden-col',
                        render: () => ''
                    },  // hides column index 2
                    { targets: 1, width: '200px' },  // hides column index 2
                    { targets: 2, width: '200px' }  // hides column index 2
                ],
                buttons: [
                    {
                        extend: 'excelHtml5',
                        text: 'Export to Excel',
                        filename: 'Comparison with Quickbok',
                        exportOptions: {
                            columns: ':visible',
                            modifier: { header: false }
                        },
                        customize: function (xlsx) {
                            const sheetDoc = xlsx.xl.worksheets['sheet1.xml'];
                            const worksheet = sheetDoc.documentElement;
                            const sheetData = worksheet.getElementsByTagName('sheetData')[0];
                            const $worksheet = $(worksheet);


                            // Remove all existing rows (clean slate)
                            $worksheet.find('row').remove();

                            // Remove existing mergeCells if any
                            $worksheet.find('mergeCells').remove();

                            function colLetter(n) {
                                let s = '', t;
                                while (n > 0) {
                                    t = (n - 1) % 26;
                                    s = String.fromCharCode(65 + t) + s;
                                    n = Math.floor((n - 1) / 26);
                                }
                                return s;
                            }

                            const occupied = {};
                            const mergeRanges = [];

                            const stylesDoc = xlsx.xl['styles.xml'];
                            const fonts = stylesDoc.getElementsByTagName('fonts')[0];
                            const borders = stylesDoc.getElementsByTagName('borders')[0];
                            const cellXfs = stylesDoc.getElementsByTagName('cellXfs')[0];

                            // --- Create bold font for header
                            const headerFont = stylesDoc.createElement('font');
                            const bold = stylesDoc.createElement('b');
                            const color = stylesDoc.createElement('color');
                            color.setAttribute('rgb', 'FF000000'); // black
                            headerFont.appendChild(bold);
                            headerFont.appendChild(color);
                            fonts.appendChild(headerFont);

                            const globalFont = stylesDoc.createElement('font');

                            const globalFontName = stylesDoc.createElement('name');
                            globalFontName.setAttribute('val', 'biome');
                            headerFont.appendChild(globalFontName);

                            const globalFontSize = stylesDoc.createElement('sz');
                            globalFontSize.setAttribute('val', '8');
                            headerFont.appendChild(globalFontSize);

                            fonts.appendChild(headerFont);

                            const headerFontId = fonts.childNodes.length - 1;
                            fonts.setAttribute('count', fonts.childNodes.length.toString());

                            // --- Create border for all cells
                            const border = stylesDoc.createElement('border');
                            ['left', 'right', 'top', 'bottom'].forEach(side => {
                                const sideElem = stylesDoc.createElement(side);
                                sideElem.setAttribute('style', 'thin');
                                const colorElem = stylesDoc.createElement('color');
                                colorElem.setAttribute('auto', '1');
                                sideElem.appendChild(colorElem);
                                border.appendChild(sideElem);
                            });

                            borders.appendChild(border);
                            const borderId = borders.childNodes.length - 1;
                            borders.setAttribute('count', borders.childNodes.length.toString());

                            // add header color
                            const fills = stylesDoc.getElementsByTagName('fills')[0];
                            const headerFill = stylesDoc.createElement('fill');
                            const patternFill = stylesDoc.createElement('patternFill');
                            patternFill.setAttribute('patternType', 'solid');

                            const fgColor = stylesDoc.createElement('fgColor');
                            fgColor.setAttribute('rgb', 'ffe2efda'); // Yellow fill (ARGB format)

                            const bgColor = stylesDoc.createElement('bgColor');
                            bgColor.setAttribute('indexed', '64');

                            patternFill.appendChild(fgColor);
                            patternFill.appendChild(bgColor);

                            headerFill.appendChild(patternFill);
                            fills.appendChild(headerFill);

                            const headerFillId = fills.childNodes.length - 1;
                            fills.setAttribute('count', fills.childNodes.length.toString());

                            const headerStyle = stylesDoc.createElement('xf');
                            headerStyle.setAttribute('numFmtId', '0');
                            headerStyle.setAttribute('fontId', headerFontId.toString());
                            headerStyle.setAttribute('applyFont', '1');
                            headerStyle.setAttribute('fillId', headerFillId.toString());
                            headerStyle.setAttribute('applyFill', '1');
                            headerStyle.setAttribute('xfId', '0');
                            headerStyle.setAttribute('applyFont', '1');
                            headerStyle.setAttribute('borderId', borderId.toString());
                            headerStyle.setAttribute('applyBorder', '1');
                            headerStyle.setAttribute('applyAlignment', '1');

                            // Add alignment child
                            const alignment = stylesDoc.createElement('alignment');
                            alignment.setAttribute('horizontal', 'center');
                            alignment.setAttribute('vertical', 'center');
                            alignment.setAttribute('wrapText', '1');
                            headerStyle.appendChild(alignment);

                            cellXfs.appendChild(headerStyle);

                            // Append the new xf
                            //cellXfs.appendChild(headerAlignXf);
                            const headerStyleIndex = cellXfs.childNodes.length - 1;
                            cellXfs.setAttribute('count', cellXfs.childNodes.length.toString());

                            // Start rowIndex at 1 to insert headers at the top
                            let rowIndex = 1;

                            // 1. Add header rows from thead
                            $('#dashboard_ardifference thead tr').each(function () {
                                let colIndex = 1;
                                const trElm = sheetDoc.createElement('row');
                                trElm.setAttribute('r', rowIndex);

                                $(this).children('th, td').each(function () {
                                    const $cell = $(this);
                                    const colspan = parseInt($cell.attr('colspan')) || 1;
                                    const rowspan = parseInt($cell.attr('rowspan')) || 1;

                                    while (occupied[rowIndex + '-' + colIndex]) {
                                        colIndex++;
                                    }

                                    const startCol = colIndex;
                                    const endCol = colIndex + colspan - 1;
                                    const endRow = rowIndex + rowspan - 1;

                                    const cellRef = colLetter(startCol) + rowIndex;
                                    const c = sheetDoc.createElement('c');
                                    c.setAttribute('r', cellRef);
                                    c.setAttribute('t', 'str');
                                    //if (colIndex > 2)
                                    c.setAttribute('s', headerStyleIndex.toString());

                                    const v = sheetDoc.createElement('v');
                                    let cellText = $cell.text().trim().replace(/\s+/g, ' ');
                                    if (!cellText) cellText = ' ';
                                    v.textContent = cellText;
                                    c.appendChild(v);
                                    trElm.appendChild(c);

                                    for (let rr = rowIndex; rr <= endRow; rr++) {
                                        for (let cc = startCol; cc <= endCol; cc++) {
                                            occupied[rr + '-' + cc] = true;
                                        }
                                    }

                                    //if (colspan > 1 || rowspan > 1) {
                                    //    mergeRanges.push(colLetter(startCol) + rowIndex + ':' + colLetter(endCol) + endRow);
                                    //}
                                    if (colspan > 1 || rowspan > 1) {
                                        mergeRanges.push(colLetter(startCol) + rowIndex + ':' + colLetter(endCol) + endRow);

                                        // --- ADD DUMMY CELLS INSIDE MERGED RANGE TO FIX BORDER ---
                                        for (let cc = startCol + 1; cc <= endCol; cc++) {
                                            const cellRef2 = colLetter(cc) + rowIndex;
                                            const c2 = sheetDoc.createElement('c');
                                            c2.setAttribute('r', cellRef2);
                                            c2.setAttribute('s', headerStyleIndex.toString()); // apply header border + color

                                            const v2 = sheetDoc.createElement('v');
                                            v2.textContent = ""; // empty cell
                                            c2.appendChild(v2);

                                            trElm.appendChild(c2);
                                        }
                                    }

                                    colIndex += colspan;
                                });

                                sheetData.appendChild(trElm);
                                rowIndex++;
                            });

                            // Create global font (e.g., Calibri 11)

                            const globalFont1 = stylesDoc.createElement('font');

                            const globalFontName1 = stylesDoc.createElement('name');
                            globalFontName1.setAttribute('val', 'biome');
                            globalFont1.appendChild(globalFontName1);

                            const globalFontSize1 = stylesDoc.createElement('sz');
                            globalFontSize1.setAttribute('val', '8');
                            globalFont1.appendChild(globalFontSize1);
                            fonts.appendChild(globalFont1);

                            const globalFontId = fonts.childNodes.length - 1;
                            fonts.setAttribute('count', fonts.childNodes.length.toString());



                            const dataStyle = stylesDoc.createElement('xf');
                            dataStyle.setAttribute('numFmtId', '0');
                            dataStyle.setAttribute('fontId', globalFontId.toString());
                            dataStyle.setAttribute('applyFont', '1');
                            dataStyle.setAttribute('fillId', '0');
                            dataStyle.setAttribute('xfId', '0');
                            dataStyle.setAttribute('borderId', borderId.toString());
                            dataStyle.setAttribute('applyBorder', '1');
                            dataStyle.setAttribute('applyAlignment', '1');

                            const dataAlignment = stylesDoc.createElement('alignment');
                            dataAlignment.setAttribute('horizontal', 'center');
                            dataAlignment.setAttribute('vertical', 'center');
                            dataAlignment.setAttribute('wrapText', '1');
                            dataStyle.appendChild(dataAlignment);

                            cellXfs.appendChild(dataStyle);
                            const centerStyleIndex = cellXfs.childNodes.length - 1;
                            cellXfs.setAttribute('count', cellXfs.childNodes.length.toString());


                            // 2. Add data rows from tbody
                            $('#dashboard_ardifference tbody tr').each(function () {
                                let colIndex = 1;
                                const trElm = sheetDoc.createElement('row');
                                trElm.setAttribute('r', rowIndex);

                                $(this).children('td').each(function () {
                                    const $cell = $(this);
                                    const colspan = parseInt($cell.attr('colspan')) || 1;
                                    const rowspan = parseInt($cell.attr('rowspan')) || 1;

                                    while (occupied[rowIndex + '-' + colIndex]) {
                                        colIndex++;
                                    }

                                    const startCol = colIndex;
                                    const endCol = colIndex + colspan - 1;
                                    const endRow = rowIndex + rowspan - 1;

                                    const cellRef = colLetter(startCol) + rowIndex;
                                    const c = sheetDoc.createElement('c');
                                    c.setAttribute('r', cellRef);
                                    c.setAttribute('t', 'str');
                                    //if (colIndex > 2)
                                    c.setAttribute('s', centerStyleIndex.toString());

                                    const v = sheetDoc.createElement('v');
                                    let cellText = $cell.text().trim().replace(/\s+/g, ' ');
                                    if (!cellText) cellText = ' ';
                                    v.textContent = cellText;
                                    c.appendChild(v);
                                    trElm.appendChild(c);

                                    for (let rr = rowIndex; rr <= endRow; rr++) {
                                        for (let cc = startCol; cc <= endCol; cc++) {
                                            occupied[rr + '-' + cc] = true;
                                        }
                                    }

                                    if (colspan > 1 || rowspan > 1) {
                                        mergeRanges.push(colLetter(startCol) + rowIndex + ':' + colLetter(endCol) + endRow);
                                    }

                                    colIndex += colspan;
                                });

                                sheetData.appendChild(trElm);
                                rowIndex++;
                            });

                            // Add mergeCells element if any merges needed
                            if (mergeRanges.length > 0) {
                                const mergeCells = sheetDoc.createElement('mergeCells');
                                mergeCells.setAttribute('count', mergeRanges.length);

                                mergeRanges.forEach(range => {
                                    const mergeCell = sheetDoc.createElement('mergeCell');
                                    mergeCell.setAttribute('ref', range);
                                    mergeCells.appendChild(mergeCell);
                                });

                                if (sheetData.nextSibling) {
                                    worksheet.insertBefore(mergeCells, sheetData.nextSibling);
                                } else {
                                    worksheet.appendChild(mergeCells);
                                }
                            }


                        }




                    },
                ],

                footerCallback: function (row, data, start, end, display) {
                    let api = this.api();
                    const columnsCount = api.columns().count();
                    var oddeven_foot = 1;
                    // Remove the formatting to get integer data for summation
                    let intVal = function (i) {
                        return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0;
                    };

                    var totalLoan = 0;
                    var totalAmt = 0;
                    for (let i = 3; i < columnsCount; i++) {
                        const colData = api.column(i, { page: 'current' }).data();

                        // Skip if column has no data
                        if (!colData || colData.length === 0) {
                            continue;
                        }

                        // Pre-sum the column
                        const total = colData.reduce((a, b) => intVal(a) + intVal(b), 0);

                        // Get label element once
                        const lbl = document.getElementById("lblDiff_" + (i));
                        if (!lbl) continue;

                        // Determine label type based on column index pattern
                        const position = (i - 3) % 6;
                        if (lbl.innerHTML == "Count") {
                            //if (position === 0 || position === 2 || position === 4) {
                            // Loan count type
                            lbl.innerHTML = 'Count (' + new Intl.NumberFormat().format(total) + ')';
                        }
                        else if (lbl.innerHTML == "US $") {
                            //else if (position === 1 || position === 3 || position === 5) {
                            // Amount type
                            lbl.innerHTML = 'US $ (' + new Intl.NumberFormat().format(total.toFixed(2)) + ')';
                        } else {
                            // Optional: handle other cases
                            lbl.innerHTML = '';
                        }
                    }
                }


            });


        },
        error: function (error) {
            CanopyUI.showError(error, 'Unable to complete request.');
        }
    });

    $('#dashboard_ardifference').on('click', '.buttons-excel', function () {
        let table = $('#dashboard_ardifference').DataTable();
        let button = table.button('.buttons-excel');
        button.conf.customize = function (xlsx) {
            // your custom export logic here...
        };
    });
    return false;
}

function dashboard_arBind_DifferenceQuickbook_BillNoRL() {
    $('#load1').show();
    var columns = [];
    var columnlink = [];
    var preheader = '';
    var headers = [];
    var returnstring = '';
    var htmlheader = '';
    var htmlrow = '';
    $.ajax({
        url: "Dashboard.aspx/GetClientwiseDashboard_DifferencewithQuickbookRL",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//
            var firstData = dataArray[0];
            var rowData = dataArray[1];
            var ColumnNames = Object.keys(firstData);
            var rowValues = Object.keys(rowData);

            htmlheader = '<thead><tr>';
            //Main Header
            var thead = document.createElement("thead");
            var trh = document.createElement("tr");
            for (var i = 1; i < ColumnNames.length; i++) {
                if (i == 2 || i == 3) {
                    var tbh = document.createElement("th");
                    tbh.rowSpan = 3;
                    tbh.style.fontWeight = "bold!important";
                    tbh.style.verticalAlign = "middle";
                    if (ColumnNames[i] == "Process")
                        tbh.style.width = "180px";
                    else
                        tbh.style.width = "250px";
                    tbh.style.left = "0px!important";
                    tbh.innerHTML = ColumnNames[i];
                    trh.appendChild(tbh);
                    //thead.appendChild(trh);
                    //htmlheader = htmlheader + '<th style="left: 0px!important;">Client</th>';
                }
                else if (i > 1) {

                    //if (i % 2 == 0) {

                    var tbh = document.createElement("th");
                    if (ColumnNames[i].includes('Total'))
                        tbh.colSpan = 4;
                    else if (ColumnNames[i].includes('YTD')) {
                        tbh.colSpan = 6;
                        tbh.style.textAlign = "center";
                        tbh.style.borderRight = "Solid 2px gray";
                        tbh.innerHTML = ColumnNames[i].replace("-ERP-LoanCount", "").replace("-ERP-Amount", "").replace("-QB-LoanCount", "").replace("-QB-Amount", "").replace("-Difference-LoanCount", "").replace("-Difference-Amount", "").replace("-Actions", "").replace("-InvoiceNo", "");
                        trh.appendChild(tbh);
                        i = i + 5;
                    }
                    else {
                        tbh.colSpan = 9;
                        tbh.style.textAlign = "center";
                        tbh.style.borderRight = "Solid 2px gray";
                        tbh.innerHTML = ColumnNames[i].replace("-ERP-LoanCount", "").replace("-ERP-Amount", "").replace("-QB-LoanCount", "").replace("-QB-Amount", "").replace("-Difference-LoanCount", "").replace("-Difference-Amount", "").replace("-Actions", "").replace("-InvoiceNo", "");
                        trh.appendChild(tbh);
                        i = i + 8;
                    }


                    //}
                }
                //else if (i == ColumnNames.length - 1) {
                //    tbh.colSpan = 9;
                //    tbh.style.textAlign = "center";
                //    tbh.style.borderRight = "Solid 2px gray";
                //    tbh.innerHTML = ColumnNames[i].replace("-ERP-LoanCount", "").replace("-ERP-Amount", "").replace("-QB-LoanCount", "").replace("-QB-Amount", "").replace("-Difference-LoanCount", "").replace("-Difference-Amount", "").replace("-Actions", "").replace("-InvoiceNo", "");
                //    trh.appendChild(tbh);
                //}
                else {
                    var tbh = document.createElement("th");
                    tbh.rowSpan = 3;
                    tbh.style.display = 'none';
                    tbh.innerHTML = ColumnNames[i];
                    //htmlheader = htmlheader + '<th style="display:none;">' + ColumnNames[i] + '</th>';
                    trh.appendChild(tbh);
                    //thead.appendChild(trh);
                }
            }
            thead.appendChild(trh);

            //seocnd header row
            trh = document.createElement("tr");
            var oddeven = 1;
            for (var i = 1; i < ColumnNames.length; i++) {
                if (i == 2 || i == 3) {
                }
                else if (i > 1) {
                    if (oddeven == 1) {
                        var tbh = document.createElement("th");
                        tbh.colSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.innerHTML = "ERP";
                        trh.appendChild(tbh);
                        oddeven++;
                        i = i + 1;
                    }
                    else if (oddeven == 2) {
                        var tbh = document.createElement("th");
                        tbh.colSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.innerHTML = "Quickbook";
                        //tbh.style.color = "green";
                        trh.appendChild(tbh);
                        oddeven++;
                        i = i + 1;
                    }
                    else if (oddeven == 3) {
                        var tbh = document.createElement("th");
                        tbh.colSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.innerHTML = "Difference";
                        //tbh.style.borderRight = "Solid 2px gray";
                        //tbh.style.color = "red";
                        if (ColumnNames[i].includes("YTD")) {
                            oddeven = 1;
                            tbh.style.borderRight = "Solid 2px gray";
                        }
                        else
                            oddeven++;
                        trh.appendChild(tbh);
                        i = i + 1;
                        //if (oddeven == 3) {
                        //    oddeven = 1;
                        //}
                    }
                    else if (oddeven == 4) {
                        var tbh = document.createElement("th");
                        tbh.rowSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.style.verticalAlign = "middle";
                        tbh.innerHTML = "Invoice #";
                        if (ColumnNames[i].includes("YTD"))
                            tbh.style.display = "none";
                        //tbh.style.color = "red";
                        trh.appendChild(tbh);
                        oddeven++;
                        //i = i + 1;
                        //if (oddeven == 3) {
                        //    oddeven = 1;
                        //}
                    }
                    else if (oddeven == 5) {
                        var tbh = document.createElement("th");
                        tbh.rowSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.style.verticalAlign = "middle";
                        tbh.innerHTML = "Actions";
                        if (ColumnNames[i].includes("YTD"))
                            tbh.style.display = "none";
                        //tbh.style.color = "red";
                        trh.appendChild(tbh);
                        oddeven++;
                        //i = i + 1;
                        //if (oddeven == 3) {
                        //    oddeven = 1;
                        //}
                    }
                    else {
                        //alert(oddeven);
                        var tbh = document.createElement("th");
                        tbh.rowSpan = 2;
                        tbh.style.textAlign = "center";
                        tbh.style.verticalAlign = "middle";
                        tbh.innerHTML = "Remark";
                        tbh.style.borderRight = "Solid 2px gray";
                        if (ColumnNames[i].includes("YTD"))
                            tbh.style.display = "none";
                        //tbh.style.color = "red";
                        trh.appendChild(tbh);
                        //oddeven++;
                        //i = i + 1;
                        if (oddeven == 6) {
                            oddeven = 1;
                        }
                    }

                }
                else {

                    //var tbh = document.createElement("th");
                    //tbh.style.display = 'none';
                    //tbh.innerHTML = ColumnNames[i];
                    ////htmlheader = htmlheader + '<th style="display:none;">' + ColumnNames[i] + '</th>';
                    //trh.appendChild(tbh);
                    //thead.appendChild(trh);
                }
            }
            thead.appendChild(trh);
            // $('#dashboard_ar').append(thead);

            //Third header row
            var trh1 = document.createElement("tr");
            //htmlheader = htmlheader + '</tr><tr>';
            var oddeven_3 = 1;
            for (var i = 1; i < ColumnNames.length; i++) {
                if (i == 2 || i == 3) {
                    //var tbr = document.createElement("th");
                    //tbr.innerHTML = ColumnNames[i];
                    //tbr.style.textAlign = "center";
                    //trh1.appendChild(tbr);
                    ////htmlheader = htmlheader + '<th style="left: 0px!important; ">' + ColumnNames[i] + '</th>';
                }
                else if (i > 1) {
                    if (oddeven_3 == 1 || oddeven_3 == 3 || oddeven_3 == 5) {
                        var tbr = document.createElement("th");
                        tbr.style.textAlign = "center";
                        tbr.innerHTML = 'Count';
                        tbr.id = "lblDiff_" + (i - 1);
                        trh1.appendChild(tbr);
                        oddeven_3++;
                        //i = i + 1;
                    }
                    else if (oddeven_3 == 2 || oddeven_3 == 4 || oddeven_3 == 6) {
                        var tbr = document.createElement("th");
                        tbr.style.textAlign = "center";
                        tbr.innerHTML = 'US $';
                        tbr.id = "lblDiff_" + (i - 1);
                        //tbr.style.borderRight = "Solid 2px gray";

                        if (ColumnNames[i].includes("YTD") && oddeven_3 == 6) {
                            oddeven_3 = 1;
                            tbr.style.borderRight = "Solid 2px gray";
                        }
                        else
                            oddeven_3++;
                        trh1.appendChild(tbr);
                        //i = i + 1;
                    }
                    else if (oddeven_3 == 7) {
                        //if (ColumnNames[i].includes("YTD"))
                        //    oddeven_3 = 1;
                        //else
                        oddeven_3++;
                    }
                    else if (oddeven_3 == 8) {
                        //if (ColumnNames[i].includes("YTD"))
                        //    oddeven_3 = 1;
                        //else
                        oddeven_3++;
                    }
                    else if (oddeven_3 == 9) {
                        oddeven_3 = 1;
                    }


                }
                //if (i % 2 != 0) {
                //    var tbr = document.createElement("th");
                //    tbr.style.textAlign = "center";
                //    tbr.innerHTML = 'US $';
                //    tbr.id = "lblDiff_" + (i - 1);
                //    trh1.appendChild(tbr);
                //}
                //else {
                //    var tbr = document.createElement("th");
                //    tbr.style.textAlign = "center";
                //    tbr.innerHTML = 'Count';
                //    tbr.id = "lblDiff_" + (i - 1);
                //    trh1.appendChild(tbr);
                //}
                else {
                }
            }
            thead.appendChild(trh1);
            $('#dashboard_ardifferenceRL').append(thead);
            // document.getElementById("newlabl").innerText = thead.innerHTML;
            var tbody = document.createElement("tbody");
            $.each(dataArray, function (index, item) {
                var tr1 = document.createElement("tr");
                //htmlrow = htmlrow + '<tr>';
                for (var i = 1; i < ColumnNames.length; i++) {
                    var targetColumnName = ColumnNames[i];
                    if (i == 2 || i == 3) {
                        var td = document.createElement("td");
                        td.innerHTML = item[targetColumnName];
                        tr1.appendChild(td);
                    }
                    else if (i > 1) {
                        var td = document.createElement("td");
                        td.style.textAlign = "center";
                        if (targetColumnName.includes("Action")) {
                            var td = document.createElement("td");
                            td.style.textAlign = "center";
                            var link = '';
                            /*var link = '<a class="dropdown-item" title="Send Invoice To Client" href="#!" id="Actions" onclick="dashboard_AddRemark(\'' + index + '\',\'' + targetColumnName + '\');" style="width:30px; display:inline;padding: .25rem .25rem!important;"><img src="../Images/edit.png" style="width:20px; display:inline;" /></a>';*/
                            if (blankForNull(item[targetColumnName]) == "Yes")
                                link = '<div class="btn-group"><div class="btn-group"><div type="button" data-toggle="dropdown" aria-expanded="false"><i class="uil fs-0 me-2 uil-file-check" style="color:green;"></i><i style="color: dodgerblue; font-size:14px;" class="uil fs-0 me-2 uil-cog"></i>';
                            else
                                link = '<div class="btn-group"><div class="btn-group"><div type="button" data-toggle="dropdown" aria-expanded="false"><i style="color: dodgerblue; font-size:14px;" class="uil fs-0 me-2 uil-cog"></i>';

                            link = link + '<span class="sr-only"></span></div><div class="dropdown-menu" role="menu" style="">';
                            link = link + '<a class="dropdown-item" title="Add Reconciliation Remark" href="#!" id="Actions" onclick="dashboard_AddRemark(\'' + index + '\',\'' + targetColumnName + '\');"><span style="color: green;"><i class="uil fs-0 me-2 uil-pen"></i></span>&nbsp; Add Remark</a>';
                            link = link + '<a class="dropdown-item" href="#!" title="Upload Excel Attachment" onclick="dashboard_AddAttachment(\'' + index + '\',\'' + targetColumnName + '\');"><span style="color: dodgerblue;"><i class="uil fs-0 me-2 uil-file"></i></span>&nbsp;&nbsp;Upload Attachment</a>';
                            link = link + '<a class="dropdown-item" title="Download Excel Attachment" href="#!" onclick="dashboard_DownloadAttachment(\'' + index + '\',\'' + targetColumnName + '\');" id="DownloadAtt"><span style="color: brown;"><i class="uil fs-0 me-2 uil-download-alt"></i></span>&nbsp;&nbsp;Download Attachment</a></div></div></td>';
                            td.innerHTML = link;
                            tr1.appendChild(td);
                        }
                        else {
                            if (targetColumnName.includes("Remark"))
                                td.style.borderRight = "Solid 2px gray";
                            else if (targetColumnName.includes("YTD") && targetColumnName.includes("Difference") && targetColumnName.includes("Amount")) {
                                td.style.borderRight = "Solid 2px gray";
                                td.style.setProperty("background-color", "#e2e2e2", "important");
                            }
                            else if (targetColumnName.includes("YTD"))
                                td.style.setProperty("background-color", "#e2e2e2", "important");

                            if (targetColumnName.includes("Count") || targetColumnName.includes("Amount")) {
                                td.innerHTML = new Intl.NumberFormat().format(Number(item[targetColumnName]))
                            }
                            else
                                td.innerHTML = item[targetColumnName];

                        }

                        tr1.appendChild(td);

                    }
                    else {
                        var td = document.createElement("td");
                        td.style.display = 'none';
                        td.innerHTML = item[targetColumnName];
                        tr1.appendChild(td);
                    }
                }
                tbody.appendChild(tr1);
                $('#dashboard_ardifferenceRL').append(tbody);
            });


            //Footer
            var tfoot = document.createElement("tfoot");
            var trf1 = document.createElement("tr");
            for (var i = 1; i < ColumnNames.length; i++) {
                if (i == 2 || i == 3) {
                    var tbr = document.createElement("th");
                    tbr.style.textAlign = "center";
                    trf1.appendChild(tbr);
                }
                else if (i > 1) {
                    var tbr = document.createElement("th");
                    tbr.style.textAlign = "center";
                    trf1.appendChild(tbr);
                }
                else {
                    var tbr = document.createElement("th");
                    tbr.style.display = 'none';
                    trf1.appendChild(tbr);
                }
            }
            tfoot.appendChild(trf1);
            $('#dashboard_ardifferenceRL').append(tfoot);

            if ($.fn.DataTable.isDataTable('#dashboard_ardifferenceRL')) {
                $('#dashboard_ardifferenceRL').DataTable().clear().destroy();
            }

            $('#dashboard_ardifferenceRL').DataTable({
                dom: 'Bft',
                destroy: true,
                orderCellsTop: true,
                fixedColumns: {
                    leftColumns: 2,
                },
                scrollCollapse: false,
                scrollY: '400px',
                scrollX: true,
                "paging": false,
                "autoWidth": false,
                select: true,
                "ordering": false,
                filter: true,
                'select': {
                    'style': 'single'
                },
                "serverSide": false,

                initComplete: function () {
                    $('#load1').hide();
                },
                columnDefs: [
                    {
                        targets: 0,
                        width: '200px',
                        className: 'dt-hidden-col',
                        render: () => ''
                    },  // hides column index 2
                    { targets: 1, width: '200px' },  // hides column index 2
                    { targets: 2, width: '200px' }  // hides column index 2
                ],
                buttons: [
                    {
                        extend: 'excelHtml5',
                        text: 'Export to Excel',
                        filename: 'Comparison with Quickbok',
                        exportOptions: {
                            columns: ':visible',
                            modifier: { header: false }
                        },
                        customize: function (xlsx) {
                            const sheetDoc = xlsx.xl.worksheets['sheet1.xml'];
                            const worksheet = sheetDoc.documentElement;
                            const sheetData = worksheet.getElementsByTagName('sheetData')[0];
                            const $worksheet = $(worksheet);


                            // Remove all existing rows (clean slate)
                            $worksheet.find('row').remove();

                            // Remove existing mergeCells if any
                            $worksheet.find('mergeCells').remove();

                            function colLetter(n) {
                                let s = '', t;
                                while (n > 0) {
                                    t = (n - 1) % 26;
                                    s = String.fromCharCode(65 + t) + s;
                                    n = Math.floor((n - 1) / 26);
                                }
                                return s;
                            }

                            const occupied = {};
                            const mergeRanges = [];

                            const stylesDoc = xlsx.xl['styles.xml'];
                            const fonts = stylesDoc.getElementsByTagName('fonts')[0];
                            const borders = stylesDoc.getElementsByTagName('borders')[0];
                            const cellXfs = stylesDoc.getElementsByTagName('cellXfs')[0];

                            // --- Create bold font for header
                            const headerFont = stylesDoc.createElement('font');
                            const bold = stylesDoc.createElement('b');
                            const color = stylesDoc.createElement('color');
                            color.setAttribute('rgb', 'FF000000'); // black
                            headerFont.appendChild(bold);
                            headerFont.appendChild(color);
                            fonts.appendChild(headerFont);

                            const globalFont = stylesDoc.createElement('font');

                            const globalFontName = stylesDoc.createElement('name');
                            globalFontName.setAttribute('val', 'biome');
                            headerFont.appendChild(globalFontName);

                            const globalFontSize = stylesDoc.createElement('sz');
                            globalFontSize.setAttribute('val', '8');
                            headerFont.appendChild(globalFontSize);

                            fonts.appendChild(headerFont);

                            const headerFontId = fonts.childNodes.length - 1;
                            fonts.setAttribute('count', fonts.childNodes.length.toString());

                            // --- Create border for all cells
                            const border = stylesDoc.createElement('border');
                            ['left', 'right', 'top', 'bottom'].forEach(side => {
                                const sideElem = stylesDoc.createElement(side);
                                sideElem.setAttribute('style', 'thin');
                                const colorElem = stylesDoc.createElement('color');
                                colorElem.setAttribute('auto', '1');
                                sideElem.appendChild(colorElem);
                                border.appendChild(sideElem);
                            });

                            borders.appendChild(border);
                            const borderId = borders.childNodes.length - 1;
                            borders.setAttribute('count', borders.childNodes.length.toString());

                            // add header color
                            const fills = stylesDoc.getElementsByTagName('fills')[0];
                            const headerFill = stylesDoc.createElement('fill');
                            const patternFill = stylesDoc.createElement('patternFill');
                            patternFill.setAttribute('patternType', 'solid');

                            const fgColor = stylesDoc.createElement('fgColor');
                            fgColor.setAttribute('rgb', 'ffe2efda'); // Yellow fill (ARGB format)

                            const bgColor = stylesDoc.createElement('bgColor');
                            bgColor.setAttribute('indexed', '64');

                            patternFill.appendChild(fgColor);
                            patternFill.appendChild(bgColor);

                            headerFill.appendChild(patternFill);
                            fills.appendChild(headerFill);

                            const headerFillId = fills.childNodes.length - 1;
                            fills.setAttribute('count', fills.childNodes.length.toString());

                            const headerStyle = stylesDoc.createElement('xf');
                            headerStyle.setAttribute('numFmtId', '0');
                            headerStyle.setAttribute('fontId', headerFontId.toString());
                            headerStyle.setAttribute('applyFont', '1');
                            headerStyle.setAttribute('fillId', headerFillId.toString());
                            headerStyle.setAttribute('applyFill', '1');
                            headerStyle.setAttribute('xfId', '0');
                            headerStyle.setAttribute('applyFont', '1');
                            headerStyle.setAttribute('borderId', borderId.toString());
                            headerStyle.setAttribute('applyBorder', '1');
                            headerStyle.setAttribute('applyAlignment', '1');

                            // Add alignment child
                            const alignment = stylesDoc.createElement('alignment');
                            alignment.setAttribute('horizontal', 'center');
                            alignment.setAttribute('vertical', 'center');
                            alignment.setAttribute('wrapText', '1');
                            headerStyle.appendChild(alignment);

                            cellXfs.appendChild(headerStyle);

                            // Append the new xf
                            //cellXfs.appendChild(headerAlignXf);
                            const headerStyleIndex = cellXfs.childNodes.length - 1;
                            cellXfs.setAttribute('count', cellXfs.childNodes.length.toString());

                            // Start rowIndex at 1 to insert headers at the top
                            let rowIndex = 1;

                            // 1. Add header rows from thead
                            $('#dashboard_ardifferenceRL thead tr').each(function () {
                                let colIndex = 1;
                                const trElm = sheetDoc.createElement('row');
                                trElm.setAttribute('r', rowIndex);

                                $(this).children('th, td').each(function () {
                                    const $cell = $(this);
                                    const colspan = parseInt($cell.attr('colspan')) || 1;
                                    const rowspan = parseInt($cell.attr('rowspan')) || 1;

                                    while (occupied[rowIndex + '-' + colIndex]) {
                                        colIndex++;
                                    }

                                    const startCol = colIndex;
                                    const endCol = colIndex + colspan - 1;
                                    const endRow = rowIndex + rowspan - 1;

                                    const cellRef = colLetter(startCol) + rowIndex;
                                    const c = sheetDoc.createElement('c');
                                    c.setAttribute('r', cellRef);
                                    c.setAttribute('t', 'str');
                                    //if (colIndex > 2)
                                    c.setAttribute('s', headerStyleIndex.toString());

                                    const v = sheetDoc.createElement('v');
                                    let cellText = $cell.text().trim().replace(/\s+/g, ' ');
                                    if (!cellText) cellText = ' ';
                                    v.textContent = cellText;
                                    c.appendChild(v);
                                    trElm.appendChild(c);

                                    for (let rr = rowIndex; rr <= endRow; rr++) {
                                        for (let cc = startCol; cc <= endCol; cc++) {
                                            occupied[rr + '-' + cc] = true;
                                        }
                                    }

                                    //if (colspan > 1 || rowspan > 1) {
                                    //    mergeRanges.push(colLetter(startCol) + rowIndex + ':' + colLetter(endCol) + endRow);
                                    //}
                                    if (colspan > 1 || rowspan > 1) {
                                        mergeRanges.push(colLetter(startCol) + rowIndex + ':' + colLetter(endCol) + endRow);

                                        // --- ADD DUMMY CELLS INSIDE MERGED RANGE TO FIX BORDER ---
                                        for (let cc = startCol + 1; cc <= endCol; cc++) {
                                            const cellRef2 = colLetter(cc) + rowIndex;
                                            const c2 = sheetDoc.createElement('c');
                                            c2.setAttribute('r', cellRef2);
                                            c2.setAttribute('s', headerStyleIndex.toString()); // apply header border + color

                                            const v2 = sheetDoc.createElement('v');
                                            v2.textContent = ""; // empty cell
                                            c2.appendChild(v2);

                                            trElm.appendChild(c2);
                                        }
                                    }

                                    colIndex += colspan;
                                });

                                sheetData.appendChild(trElm);
                                rowIndex++;
                            });

                            // Create global font (e.g., Calibri 11)

                            const globalFont1 = stylesDoc.createElement('font');

                            const globalFontName1 = stylesDoc.createElement('name');
                            globalFontName1.setAttribute('val', 'biome');
                            globalFont1.appendChild(globalFontName1);

                            const globalFontSize1 = stylesDoc.createElement('sz');
                            globalFontSize1.setAttribute('val', '8');
                            globalFont1.appendChild(globalFontSize1);
                            fonts.appendChild(globalFont1);

                            const globalFontId = fonts.childNodes.length - 1;
                            fonts.setAttribute('count', fonts.childNodes.length.toString());



                            const dataStyle = stylesDoc.createElement('xf');
                            dataStyle.setAttribute('numFmtId', '0');
                            dataStyle.setAttribute('fontId', globalFontId.toString());
                            dataStyle.setAttribute('applyFont', '1');
                            dataStyle.setAttribute('fillId', '0');
                            dataStyle.setAttribute('xfId', '0');
                            dataStyle.setAttribute('borderId', borderId.toString());
                            dataStyle.setAttribute('applyBorder', '1');
                            dataStyle.setAttribute('applyAlignment', '1');

                            const dataAlignment = stylesDoc.createElement('alignment');
                            dataAlignment.setAttribute('horizontal', 'center');
                            dataAlignment.setAttribute('vertical', 'center');
                            dataAlignment.setAttribute('wrapText', '1');
                            dataStyle.appendChild(dataAlignment);

                            cellXfs.appendChild(dataStyle);
                            const centerStyleIndex = cellXfs.childNodes.length - 1;
                            cellXfs.setAttribute('count', cellXfs.childNodes.length.toString());


                            // 2. Add data rows from tbody
                            $('#dashboard_ardifferenceRL tbody tr').each(function () {
                                let colIndex = 1;
                                const trElm = sheetDoc.createElement('row');
                                trElm.setAttribute('r', rowIndex);

                                $(this).children('td').each(function () {
                                    const $cell = $(this);
                                    const colspan = parseInt($cell.attr('colspan')) || 1;
                                    const rowspan = parseInt($cell.attr('rowspan')) || 1;

                                    while (occupied[rowIndex + '-' + colIndex]) {
                                        colIndex++;
                                    }

                                    const startCol = colIndex;
                                    const endCol = colIndex + colspan - 1;
                                    const endRow = rowIndex + rowspan - 1;

                                    const cellRef = colLetter(startCol) + rowIndex;
                                    const c = sheetDoc.createElement('c');
                                    c.setAttribute('r', cellRef);
                                    c.setAttribute('t', 'str');
                                    //if (colIndex > 2)
                                    c.setAttribute('s', centerStyleIndex.toString());

                                    const v = sheetDoc.createElement('v');
                                    let cellText = $cell.text().trim().replace(/\s+/g, ' ');
                                    if (!cellText) cellText = ' ';
                                    v.textContent = cellText;
                                    c.appendChild(v);
                                    trElm.appendChild(c);

                                    for (let rr = rowIndex; rr <= endRow; rr++) {
                                        for (let cc = startCol; cc <= endCol; cc++) {
                                            occupied[rr + '-' + cc] = true;
                                        }
                                    }

                                    if (colspan > 1 || rowspan > 1) {
                                        mergeRanges.push(colLetter(startCol) + rowIndex + ':' + colLetter(endCol) + endRow);
                                    }

                                    colIndex += colspan;
                                });

                                sheetData.appendChild(trElm);
                                rowIndex++;
                            });

                            // Add mergeCells element if any merges needed
                            if (mergeRanges.length > 0) {
                                const mergeCells = sheetDoc.createElement('mergeCells');
                                mergeCells.setAttribute('count', mergeRanges.length);

                                mergeRanges.forEach(range => {
                                    const mergeCell = sheetDoc.createElement('mergeCell');
                                    mergeCell.setAttribute('ref', range);
                                    mergeCells.appendChild(mergeCell);
                                });

                                if (sheetData.nextSibling) {
                                    worksheet.insertBefore(mergeCells, sheetData.nextSibling);
                                } else {
                                    worksheet.appendChild(mergeCells);
                                }
                            }


                        }




                    },
                ],

                footerCallback: function (row, data, start, end, display) {
                    let api = this.api();
                    const columnsCount = api.columns().count();
                    var oddeven_foot = 1;
                    // Remove the formatting to get integer data for summation
                    let intVal = function (i) {
                        return typeof i === 'string' ? i.replace(/[\$,]/g, '') * 1 : typeof i === 'number' ? i : 0;
                    };

                    var totalLoan = 0;
                    var totalAmt = 0;
                    for (let i = 3; i < columnsCount; i++) {
                        const colData = api.column(i, { page: 'current' }).data();

                        // Skip if column has no data
                        if (!colData || colData.length === 0) {
                            continue;
                        }

                        // Pre-sum the column
                        const total = colData.reduce((a, b) => intVal(a) + intVal(b), 0);

                        // Get label element once
                        const lbl = document.getElementById("lblDiff_" + (i));
                        if (!lbl) continue;

                        // Determine label type based on column index pattern
                        const position = (i - 3) % 6;
                        if (lbl.innerHTML == "Count") {
                            //if (position === 0 || position === 2 || position === 4) {
                            // Loan count type
                            lbl.innerHTML = 'Count (' + new Intl.NumberFormat().format(total) + ')';
                        }
                        else if (lbl.innerHTML == "US $") {
                            //else if (position === 1 || position === 3 || position === 5) {
                            // Amount type
                            lbl.innerHTML = 'US $ (' + new Intl.NumberFormat().format(total.toFixed(2)) + ')';
                        } else {
                            // Optional: handle other cases
                            lbl.innerHTML = '';
                        }
                    }
                }


            });


        },
        error: function (error) {
            CanopyUI.showError(error, 'Unable to complete request.');
        }
    });

    $('#dashboard_ardifferenceRL').on('click', '.buttons-excel', function () {
        let table = $('#dashboard_ardifferenceRL').DataTable();
        let button = table.button('.buttons-excel');
        button.conf.customize = function (xlsx) {
            // your custom export logic here...
        };
    });
    return false;
}




function dashboard_DownloadAttachmentRL(index, MonthYear) {
    var row = $('#dashboard_ardifferenceRL').DataTable().row(index).data();
    var ProjectID = blankForNull(row[0]);
    var Process = blankForNull(row[2]);
    var myarr = [];
    myarr = MonthYear.split('-');
    var Month = myarr[0];
    var Year = myarr[1];
    var currenturl = window.location.href;
    var urlindex = currenturl.lastIndexOf('/');
    var firstpart = currenturl.substring(0, urlindex + 1);
    var secondpart = "DownloadFiles.aspx?ProjectID=" + ProjectID + "&Month=" + Month + "&Year=" + Year + "&Process=" + Process;
    var actualurl = firstpart + secondpart;
    var filename = row[1] + '_' + Month + '_' + Year + '_' + Process + '.xlsx';
    fetch(actualurl)
        // check to make sure you didn't have an unexpected failure (may need to check other things here depending on use case / backend)
        .then(resp => resp.status === 200 ? resp.blob() : Promise.reject('something went wrong'))
        .then(blob => {

            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.style.display = 'none';
            a.href = url;
            // the filename you want
            a.download = filename;
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);
            // or you know, something with better UX...
            //alert('your file has downloaded!');
        })
        .catch(() => alert('Oops! It seems that there is an error while retriving attachment. Please contact administrator.'));
}

function dashboard_DownloadAttachment(index, MonthYear) {
    var row = $('#dashboard_ardifference').DataTable().row(index).data();
    var ProjectID = blankForNull(row[0]);
    var Process = blankForNull(row[2]);
    var myarr = [];
    myarr = MonthYear.split('-');
    var Month = myarr[0];
    var Year = myarr[1];
    var currenturl = window.location.href;
    var urlindex = currenturl.lastIndexOf('/');
    var firstpart = currenturl.substring(0, urlindex + 1);
    var secondpart = "DownloadFiles.aspx?ProjectID=" + ProjectID + "&Month=" + Month + "&Year=" + Year + "&Process=" + Process;
    var actualurl = firstpart + secondpart;
    var filename = row[1] + '_' + Month + '_' + Year + '_' + Process + '.xlsx';
    fetch(actualurl)
        // check to make sure you didn't have an unexpected failure (may need to check other things here depending on use case / backend)
        .then(resp => resp.status === 200 ? resp.blob() : Promise.reject('something went wrong'))
        .then(blob => {

            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.style.display = 'none';
            a.href = url;
            // the filename you want
            a.download = filename;
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);
            // or you know, something with better UX...
            //alert('your file has downloaded!');
        })
        .catch(() => alert('Oops! It seems that there is an error while retriving attachment. Please contact administrator.'));
}

function dashboard_AddAttachment(index, MonthYear) {
    var row = $('#dashboard_ardifference').DataTable().row(index).data();
    document.getElementById("dashboard_projectid_upload").innerHTML = blankForNull(row[0]);
    document.getElementById("dashboard_projectno_upload").innerHTML = blankForNull(row[1]);
    document.getElementById("dashboard_process_upload").innerHTML = blankForNull(row[2]);
    document.getElementById("dashboard_monthyear_upload").innerHTML = MonthYear;
    $("#dashboard_uploadattachment").modal("show");
}

function dashboard_upload_submit() {
    var ProjectId = document.getElementById("dashboard_projectid_upload").innerHTML;
    var MonthYear = document.getElementById("dashboard_monthyear_upload").innerHTML;
    var Process = document.getElementById("dashboard_process_upload").innerHTML;
    var myarr = [];
    myarr = MonthYear.split('-');
    var Month = myarr[0];
    var Year = myarr[1];
    PageMethods.UploadAttachment(ProjectId, Month, Year, Process, dashboard_Recatt_OnSuccess, dashboard_Recatt_OnError);
    return false;
}

function dashboard_Recatt_OnSuccess(result) {
    $("#dashboard_uploadattachment").modal("hide");
    if (result > 0) {
        document.getElementById("dashboard_remark").value = "";
        document.getElementById("dashboard_RecRemark_errmsg").innerHTML = "File uploaded successfully.";
        $('#dashboard_RecRemark_dverror').modal('show');
        return false;
    }
    else {
        document.getElementById("dashboard_RecRemark_errmsg").innerHTML = "Oops! System is facing connectivity issue. Please try after some time.";
        $('#dashboard_RecRemark_dverror').modal('show');
        return false;
    }
    return false;
}

function dashboard_Recatt_OnError(error) {
    $("#dashboard_uploadattachment").modal("hide");
    CanopyUI.showError(error, 'Unable to complete request.');
}

function dashboard_AddRemark(index, MonthYear) {
    var row = $('#dashboard_ardifference').DataTable().row(index).data();
    document.getElementById("dashboard_projectid").innerHTML = blankForNull(row[0]);
    document.getElementById("dashboard_projectno").innerHTML = blankForNull(row[1]);
    document.getElementById("dashboard_process").innerHTML = blankForNull(row[2]);
    document.getElementById("dashboard_monthyear").innerHTML = MonthYear;
    $("#dashboard_updateremark").modal("show");
}

function dashboard_remark_submit() {
    var ProjectId = document.getElementById("dashboard_projectid").innerHTML;
    var MonthYear = document.getElementById("dashboard_monthyear").innerHTML;
    var Process = document.getElementById("dashboard_process").innerHTML;
    var myarr = [];
    myarr = MonthYear.split('-');
    var Month = myarr[0];
    var Year = myarr[1];
    var Reamrk = document.getElementById("dashboard_remark").value;
    if (Reamrk == "") {
        alert("Please enter Remark");
        return false;
    }
    PageMethods.InsertReconciliationRemark(ProjectId, Month, Year, Process, Reamrk, dashboard_RecRemark_OnSuccess, dashboard_RecRemark_OnError);
    return false;
}

function dashboard_RecRemark_OnSuccess(result) {
    $("#dashboard_updateremark").modal("hide");
    if (result > 0) {
        document.getElementById("dashboard_remark").value = "";
        document.getElementById("dashboard_RecRemark_errmsg").innerHTML = "Remark added successfully.";
        $('#dashboard_RecRemark_dverror').modal('show');
        return false;
    }
    else {
        document.getElementById("dashboard_RecRemark_errmsg").innerHTML = "Oops! System is facing connectivity issue. Please try after some time.";
        $('#dashboard_RecRemark_dverror').modal('show');
        return false;
    }

    return false;
}

function dashboard_RecRemark_OnError(error) {
    $("#dashboard_updateremark").modal("hide");
    CanopyUI.showError(error, 'Unable to complete request.');
}

function dashboard_RecRemark_MessageRedirect() {
    $("#dashboard_RecRemark_dverror").modal("hide");

}

function binddatatable() {


}

function bindNewDashboard() {
    $('#load1').show();
    var columns = [];
    var columnlink = [];
    var preheader = '';
    var headers = [];
    var returnstring = '';
    var htmlheader = '';
    var htmlrow = '';
    $.ajax({
        url: "Dashboard.aspx/GetClientwiseDashboard",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//
            var columncount = 0;
            $.each(dataArray[0], function (key, value) {
                var my_item = {};
                my_item.data = key;
                my_item.title = key;
                columns.push(my_item);
                headers.push(my_item.title);
                if (columncount > 2) {
                    columnlink.push(columncount);
                }
                columncount++;
            });

            $('#dashboard_ar').DataTable({
                dom: 'Bftip',
                destroy: true,
                orderCellsTop: true,
                fixedColumns: {
                    leftColumns: 3,
                },
                fixedHeader: true,
                scrollCollapse: true,
                scrollX: true,
                "paging": false,
                "autoWidth": true,
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
                    $(nRow).children("td").css("text-wrap", "nowrap");
                    $(nRow).children("td").css("text-align", "center");
                },
                columnDefs: [
                    { className: "dt-center", targets: "_all" }, // Centers all columns
                    { visible: false, targets: 0 },
                    { visible: false, targets: 1 },
                    {
                        targets: columnlink,
                        render: function (data, type, row, meta) {

                            if (headers[meta.col].includes("LoanCount"))
                                returnstring = '<input type="text" id="loancount_' + headers[meta.col] + '_' + meta.row + '" onchange="return getValue(this,' + meta.row + ');" style="width:50px;" value="' + blankForNull(data) + '"></input>';
                            else
                                returnstring = '<input type="text" id="loancount_' + headers[meta.col] + '_' + meta.row + '" onchange="return getValue(this,' + meta.row + ');" style="width:70px;" value="' + blankForNull(data) + '"></input>';
                            return returnstring;
                        }
                    }
                ],

                initComplete: function () {
                    $('#load1').hide();

                },
                buttons: [
                    {
                        extend: 'excelHtml5', title: 'AR Billing', autoFilter: true,
                    },
                ],


            });

            var loopindex = 0;

        },
        error: function (error) {
            CanopyUI.showError(error, 'Unable to complete request.');
        }
    });
}


function getValue(txt, index) {
    var rows = $('#dashboard_ar_update').DataTable().row(index).data();
    PageMethods.UpdateQuickbookData(rows[0], rows[2], txt.id, txt.value, qb_OnSuccess, qb_OnError);
    return false;
}

function qb_OnSuccess(result) {
    return false;
}
function qb_OnError(error) {
    CanopyUI.showError(error, 'Unable to complete request.');
    return false;
}