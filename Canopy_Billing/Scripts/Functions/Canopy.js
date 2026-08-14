var can_table;
var can_html = '';
var task_html = '';
var cantaskdetails;
var candetails_html = '';
var candetails_table;

function getcanopyData() {
    var fromdate = document.getElementById("can_fromdate").value;
    var todate = document.getElementById("can_todate").value;
    if (fromdate == "") {
        alert("Please select from date");
        return false;
    }
    if (todate == "") {
        alert("Please select to date");
        return false;
    }

    BindCanopyData(fromdate, todate);
    BindCanopyDataDetails(fromdate, todate);
    return false;

}

function can_ViewDetails(LoanID, Index) {
    BindCanopyTask(LoanID);
    $("#can_details").modal("show");
}

function BindCanopyData(FromDate, ToDate) {
    $('#load1').show();
    can_html = '';
    var i = 0;
    $.ajax({
        url: "CanopyData.aspx/GetCanopyData",
        type: "POST",
        data: "{FromDate:'" + FromDate + "', ToDate:'" + ToDate + "'}",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//
            $.each(dataArray, function (index, value) {
                i++;
                var CreatedDate = '';
                var CreatedTime = '';
                var SubmittedDate = '';
                var SubmittedTime = '';
                var SnapDate = '';
                var SnapTime = '';
                if (value.createdDate != null) {
                    CreatedDate = eval(value.createdDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1).toLocaleDateString(\"en-US\")"));
                    CreatedTime = eval(value.createdDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1).toLocaleTimeString(\"en-US\")"));
                }
                if (value.submittedDate != null) {
                    SubmittedDate = eval(value.submittedDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1).toLocaleDateString(\"en-US\")"));
                    SubmittedTime = eval(value.submittedDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1).toLocaleTimeString(\"en-US\")"));
                }
                if (value.snapshotTakenDate != null) {
                    SnapDate = eval(value.snapshotTakenDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1).toLocaleDateString(\"en-US\")"));
                    SnapTime = eval(value.snapshotTakenDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1).toLocaleTimeString(\"en-US\")"));
                }
                can_html += '<tr>';
                can_html += '<td>' + (i) + '</td>';
                can_html += '<td style="display:none;"><a class="dropdown-item" href="#!" id="ActionsEx" onclick="can_ViewDetails(' + value.id + ',' + index + ');"><span style="color: dodgerblue;"><i class="uil fs-0 me-2 uil-file"></i></span></a></td>';
                can_html += '<td style="display:none;"></td>';
                can_html += '<td>' + blankForNull(value.loanid) + '</td>';
                can_html += '<td style="text-wrap: nowrap;">' + blankForNull(CreatedDate) + ' ' + blankForNull(CreatedTime) + '</td>';
                can_html += '<td style="text-wrap: nowrap;">' + blankForNull(SubmittedDate) + ' ' + blankForNull(SubmittedTime) + '</td>';
                can_html += '<td style="text-wrap: nowrap;">' + blankForNull(SnapDate) + ' ' + blankForNull(SnapTime) + '</td>';
                can_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.transactionIdentifier) + '</td>';
                can_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.scriptName) + '</td>';
                can_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.buyerName) + '</td>';
                can_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.sellerName) + '</td>';
                can_html += '</tr>';
            });

            if ($.fn.dataTable.isDataTable('#can_table')) {
                can_table.destroy();
            }
            $('#can_table tbody').html(can_html);
            //else
            can_table = $('#can_table').DataTable({
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
                    //document.getElementById("filterboxrow").style.display = '';
                    $('#load1').hide();
                    //this.api()
                    //    .columns()
                    //    .every(function () {
                    //        let column = this;
                    //        let title = column.header().textContent;

                    //        // Create input element
                    //        let input = document.createElement('input');
                    //        input.placeholder = title;
                    //        input.className = "filterinput";
                    //        column.header().replaceChildren(input);

                    //        // Event listener for user input
                    //        input.addEventListener('keyup', () => {
                    //            if (column.search() !== this.value) {
                    //                column.search(input.value).draw();
                    //            }
                    //        });
                    //    });

                },
                "rowCallback": function (row, data) {
                    // Cell at index 5 in the row is 'Active'.
                    var val = data[3];
                },
                buttons: [
                    {
                        extend: 'excelHtml5', title: 'Loan Details', autoFilter: true
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

function BindCanopyDataDetails(FromDate, ToDate) {
    $('#load1').show();
    candetails_html = '';
    var i = 0;
    $.ajax({
        url: "CanopyData.aspx/GetCanopyDataDetails",
        type: "POST",
        data: "{FromDate:'" + FromDate + "', ToDate:'" + ToDate + "'}",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//
            $.each(dataArray, function (index, value) {
                i++;
                var CreatedDate = '';
                var CreatedTime = '';
                var SubmittedDate = '';
                var SubmittedTime = '';
                var SnapDate = '';
                var SnapTime = '';

                var assignedDate = '';
                var assignedTime = '';
                var dueDate = '';
                var dueime = '';
                var compDate = '';
                var compTime = '';

                if (value.createdDate != null) {
                    CreatedDate = eval(value.createdDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1).toLocaleDateString(\"en-US\")"));
                    CreatedTime = eval(value.createdDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1).toLocaleTimeString(\"en-US\")"));
                }
                if (value.submittedDate != null) {
                    SubmittedDate = eval(value.submittedDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1).toLocaleDateString(\"en-US\")"));
                    SubmittedTime = eval(value.submittedDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1).toLocaleTimeString(\"en-US\")"));
                }
                if (value.snapshotTakenDate != null) {
                    SnapDate = eval(value.snapshotTakenDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1).toLocaleDateString(\"en-US\")"));
                    SnapTime = eval(value.snapshotTakenDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1).toLocaleTimeString(\"en-US\")"));
                }

                if (value.assignedDate != null) {
                    assignedDate = eval(value.assignedDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1).toLocaleDateString(\"en-US\")"));
                    assignedTime = eval(value.assignedDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1).toLocaleTimeString(\"en-US\")"));
                }
                if (value.dueDate != null) {
                    dueDate = eval(value.dueDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1).toLocaleDateString(\"en-US\")"));
                    dueime = eval(value.dueDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1).toLocaleTimeString(\"en-US\")"));
                }
                if (value.completedDate != null) {
                    compDate = eval(value.completedDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1).toLocaleDateString(\"en-US\")"));
                    compTime = eval(value.completedDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1).toLocaleTimeString(\"en-US\")"));
                }

                candetails_html += '<tr>';
                candetails_html += '<td>' + (i) + '</td>';
                candetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.TransactionIdentifier) + '</td>';
                candetails_html += '<td>' + blankForNull(value.loanId) + '</td>';
                candetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(CreatedDate) + ' ' + blankForNull(CreatedTime) + '</td>';
                candetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(SubmittedDate) + ' ' + blankForNull(SubmittedTime) + '</td>';
                candetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.scriptName) + '</td>';
                candetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.taskName) + '</td>';
                candetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.processFlowName) + '</td>';
                candetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.taskAssignedUser) + '</td>';

                candetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(assignedDate) + ' ' + blankForNull(assignedTime) + '</td>';
                candetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(dueDate) + ' ' + blankForNull(dueime) + '</td>';
                candetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(compDate) + ' ' + blankForNull(compTime) + '</td>';

                //candetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.assignedDate) + '</td>';
                //candetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.dueDate) + '</td>';
                //candetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.completedDate) + '</td>';


                candetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.taskStatus) + '</td>';
                candetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(SnapDate) + ' ' + blankForNull(SnapTime) + '</td>';
                candetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.buyerName) + '</td>';
                candetails_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.sellerName) + '</td>';
                candetails_html += '</tr>';
            });

            if ($.fn.dataTable.isDataTable('#candetails_table')) {
                candetails_table.destroy();
            }
            $('#candetails_table tbody').html(candetails_html);
            //else
            candetails_table = $('#candetails_table').DataTable({
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
                "rowCallback": function (row, data) {
                },
                buttons: [
                    {
                        extend: 'excelHtml5', title: 'Task Details', autoFilter: true
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


function BindCanopyTask(LoanID) {
    $('#load1').show();
    task_html = '';
    var i = 0;
    $.ajax({
        url: "CanopyData.aspx/GetCanopyTask",
        type: "POST",
        data: "{LoanID:" + LoanID + "}",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//
            $.each(dataArray, function (index, value) {
                i++;
                var CreatedDate = '';
                var CreatedTime = '';
                var SubmittedDate = '';
                var SubmittedTime = '';
                var SnapDate = '';
                var SnapTime = '';

                task_html += '<tr>';
                task_html += '<td>' + (i) + '</td>';
                task_html += '<td>' + blankForNull(value.loanId) + '</td>';
                task_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.processFlowName) + '</td>';
                task_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.taskName) + '</td>';
                task_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.taskStatus) + '</td>';
                task_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.taskAssignedUser) + '</td>';
                task_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.assignedDate) + '</td>';
                task_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.dueDate) + '</td>';
                task_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.completedDate) + '</td>';
                task_html += '</tr>';
            });
            if ($.fn.dataTable.isDataTable('#cantaskdetails')) {
                cantaskdetails.destroy();
            }
            $('#cantaskdetails tbody').html(task_html);
            //else
            cantaskdetails = $('#cantaskdetails').DataTable({
                dom: 'lftip',
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
                "rowCallback": function (row, data) {
                    var val = data[3];
                },
            });
        },
        error: function (error) {
            CanopyUI.showError(error, 'Unable to complete request.');
        }
    });
    return false;
}