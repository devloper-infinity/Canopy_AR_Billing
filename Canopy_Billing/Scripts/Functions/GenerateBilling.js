//Year Bind
function BillingDecription_bindyear() {
    var start = new Date().getFullYear();

    var select = document.getElementById("billingdecription_year");
    let options = select.getElementsByTagName('option');

    for (var i = options.length; i--;) {
        select.removeChild(options[i]);
    }

    $("#billingdecription_year").append($("<option></option>").val("").html("Select"));
    for (var i = start; i > start - 5; i--) {
        $("#billingdecription_year").append($("<option></option>").val(i).html(i));
    }
}

//Get Summary Data
function GetBillingDecriptionSummary() {
    $('#load1').show();
    $.ajax({
        url: "GenerateBilling.aspx/GetBillingDecriptionSummary",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",

        success: function (data) {
            if ($.fn.dataTable.isDataTable('#billingdecription_Summarytable')) {
                $('#billingdecription_Summarytable').DataTable().destroy();
            }
            var dataArray = JSON.parse(data.d);

            $('#billingdecription_Summarytable').DataTable({
                dom: 'Bftp',
                buttons: [
                    {
                        text: '<i class="fas fa-file-excel"></i> Excel (Summary & Details)',
                        className: 'buttons-excel btn btn-success btn-sm',
                        action: function (e, dt, node, config) {
                            var summaryData = $('#billingdecription_Summarytable').DataTable().rows().data().toArray();

                            if (summaryData.length === 0) {
                                Swal.fire("warning", "No data available in Summary!", "warning");
                                return;
                            }

                            if (typeof $('#load1') !== 'undefined') { $('#load1').show(); }

                            $.ajax({
                                url: "AP_BillingDescription.aspx/GetBillingDecriptionDetails",
                                type: "POST",
                                dataType: "json",
                                contentType: "application/json; charset=utf-8",
                                success: function (res) {
                                    if (typeof $('#load1') !== 'undefined') { $('#load1').hide(); }

                                    var detailsData = JSON.parse(res.d);

                                    if (!detailsData || detailsData.length === 0) {
                                        Swal.fire("warning", "No data available in Details!", "warning");
                                        return;
                                    }

                                    var formattedSummary = summaryData.map(function (row, i) {
                                        return {
                                            "Sr. #": i + 1,
                                            "Client": row.Client || '',
                                            "Project Name": row.ProjectName || '',
                                            "Billing Period": row.BillingPeriod || '',
                                            "Billing Sent Date": row.BillingSentDate || '',
                                            "Loan Count": row.LoanCount || ''
                                        };
                                    });

                                    var formattedDetails = detailsData.map(function (row, i) {
                                        return {
                                            "Sr. #": i + 1,
                                            "Loan ID": row.loanId || '',
                                            "Created Date": row.createdDate || '',
                                            "Submitted Date": row.submittedDate || '',
                                            "Snapshot Taken Date": row.snapshotTakenDate || '',
                                            "Bill To": row.BillTo || '',
                                            "Buyer Name": row.buyerName || '',
                                            "Seller Name": row.sellerName || '',
                                            "Script Name": row.scriptName || '',
                                            "Transaction Identifier": row.TransactionIdentifier || '',
                                            "DSCR": row.DSCR || '',
                                            "Lien Position": row.LienPosition || '',
                                            "B1 Self Employment Flag": row.B1SelfEmploymentFlag || '',
                                            "B2 Self Employment Flag": row.B2SelfEmploymentFlag || '',
                                            "Occupancy": row.Occupancy || '',
                                            "Number of Mortgaged Properties": row.NumberofMortgagedProperties || '',
                                            "Note Type": row.NoteType || '',
                                            "Note Date": row.NoteDate || '',
                                            "Number of Units": row.NumberofUnits || '',
                                            "Process": row.Process || '',
                                            "Project ID": row.ProjectId || '',
                                            "Billing Type": row.BillingType || ''
                                        };
                                    });

                                    function getColumnWidths(data) {
                                        var cols = [];
                                        if (data.length === 0) return cols;
                                        var keys = Object.keys(data[0]);

                                        keys.forEach(function (key) {
                                            var maxLen = key.toString().length;
                                            data.forEach(function (row) {
                                                var val = row[key] ? row[key].toString() : '';
                                                if (val.length > maxLen) maxLen = val.length;
                                            });
                                            cols.push({ wch: Math.max(maxLen + 3, 10) });
                                        });
                                        return cols;
                                    }
                                    function styleHeader(ws) {
                                        var range = XLSX.utils.decode_range(ws['!ref']);
                                        for (let C = range.s.c; C <= range.e.c; ++C) {
                                            var address = XLSX.utils.encode_cell({ r: 0, c: C });
                                            if (!ws[address]) continue;

                                            ws[address].s = {
                                                fill: { fgColor: { rgb: "D3D3D3" } },
                                                font: { bold: true, color: { rgb: "000000" } }
                                            };
                                        }
                                    }

                                    var wsSummary = XLSX.utils.json_to_sheet(formattedSummary);
                                    var wsDetails = XLSX.utils.json_to_sheet(formattedDetails);

                                    wsSummary['!cols'] = getColumnWidths(formattedSummary);
                                    wsDetails['!cols'] = getColumnWidths(formattedDetails);

                                    styleHeader(wsSummary);
                                    styleHeader(wsDetails);

                                    var wb = XLSX.utils.book_new();
                                    XLSX.utils.book_append_sheet(wb, wsSummary, "Summary");
                                    XLSX.utils.book_append_sheet(wb, wsDetails, "Details");

                                    XLSX.writeFile(wb, "Billing_Description_Report.xlsx");
                                },
                                error: function (err) {
                                    if (typeof $('#load1') !== 'undefined') { $('#load1').hide(); }
                                    Swal.fire("error", "error show", "error");
                                }
                            });
                        }
                    }
                ],
                scrollX: true,
                destroy: true,
                paging: true,
                "ordering": false,
                "autoWidth": true,
                select: true,
                processing: true,
                "aaSorting": [],
                'select': {
                    'style': 'single'
                },
                "data": dataArray,
                columns: [
                    {
                        data: null,
                        render: function (data, type, row, meta) {
                            return meta.row + 1;
                        }
                    },
                    { data: 'Client' },
                    { data: 'ProjectID', visible: false },
                    { data: 'ProjectName' },
                    { data: 'BillingPeriod' },
                    { data: 'BillingSentDate' },
                    {
                        data: 'LoanCount',
                        render: function (data, type, row) {
                            return '<a href="#" class="loan-link" data-projectid="' + row.ProjectID + '">' + data + '</a>';
                        }
                    }

                ],
                initComplete: function () {
                    $('#load1').hide();
                }
            });
        },
        error: function (err) {
            $('#load1').hide();
            console.log(err);
        }
    });

    return false;
}

//Get Project Wise Summary Data
$(document).on('click', '.loan-link', function (e) {
    e.preventDefault();
    var projectId = $(this).data('projectid');
    GetBillingProjectWise(projectId);

});
function GetBillingProjectWise(projectId) {
    $('#load1').show();
    var selectedLoanIds = [];

    $.ajax({
        url: "GenerateBilling.aspx/GetBillingProjectWise",
        type: "POST",
        data: JSON.stringify({ ProjectID: projectId }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if ($.fn.dataTable.isDataTable('#billingdecription_table')) {
                $('#billingdecription_table').DataTable().clear().destroy();
                $('#billingdecription_table').empty();
            }
            var dataArray = JSON.parse(data.d);
            var table = $('#billingdecription_table').DataTable({
                dom: 'Bftp',
                buttons: [
                    {
                        extend: 'excelHtml5',
                        text: '<i class="fas fa-file-excel"></i> Excel',
                        className: 'buttons-excel btn btn-success btn-sm',
                        title: 'Billing_Description_ProjectWiseDetails'
                    },
                    {
                        text: '<i class="fas fa-check-circle"></i> Verify',
                        className: 'btn btn-primary btn-sm ml-1',
                        action: function (e, dt, node, config) {
                            if (selectedLoanIds.length === 0) {

                                Swal.fire("warning", "Please select at least one loan!", "warning");
                                return;
                            }

                            $('#load1').show();

                            var updatePromises = selectedLoanIds.map(function (id) {
                                return $.ajax({
                                    url: "AP_BillingDescription.aspx/UpdateBillingStatus",
                                    type: "POST",
                                    data: JSON.stringify({ loanId: parseInt(id) }),
                                    dataType: "json",
                                    contentType: "application/json; charset=utf-8"
                                });
                            });

                            Promise.all(updatePromises)
                                .then(function (responses) {
                                    $('#load1').hide();
                                    selectedLoanIds = [];
                                    $('#selectAllCheckbox').prop('checked', false);

                                    Swal.fire({
                                        icon: 'success',
                                        title: 'Success!',
                                        text: 'Selected loans have been verified successfully!',
                                        showConfirmButton: true
                                    }).then((result) => {
                                        GetBillingProjectWise(projectId);
                                    });
                                })
                                .catch(function (err) {
                                    $('#load1').hide();
                                    console.log(err);
                                    Swal.fire({
                                        icon: 'error',
                                        title: 'Error!',
                                        text: 'An error occurred, please try again.'
                                    });
                                });
                        }
                    },
                    {
                        text: '<i class="fas fa-sync-alt"></i> Recalculate',
                        className: 'btn btn-info btn-sm ml-1',
                        action: function (e, dt, node, config) {
                            console.log("Recalculate clicked for:", selectedLoanIds);
                        }
                    },
                    {
                        text: '<i class="fas fa-paper-plane"></i> Sent to Billing',
                        className: 'btn btn-info btn-sm ml-1',
                        action: function (e, dt, node, config) {
                            console.log("Sent to Billing clicked for:", selectedLoanIds);
                        }
                    }
                ],
                scrollX: true,
                destroy: true,
                paging: true,
                "ordering": false,
                "autoWidth": true,
                "data": dataArray,
                columns: [
                    {
                        data: null,
                        className: 'text-center',
                        orderable: false,
                        render: function (data, type, row, meta) {
                            var isChecked = selectedLoanIds.includes(String(row.id)) ? 'checked' : '';
                            return '<input type="checkbox" class="row-select" value="' + row.id + '" ' + isChecked + ' />';
                        }
                    },
                    {
                        data: null,
                        render: function (data, type, row, meta) {
                            return meta.row + 1;
                        }
                    },
                    { data: 'id', visible: false },
                    { data: 'loanId' },
                    { data: 'createdDate' },
                    { data: 'submittedDate' },
                    { data: 'snapshotTakenDate' },
                    { data: 'BillTo' },
                    { data: 'buyerName' },
                    { data: 'sellerName' },
                    { data: 'scriptName' },
                    { data: 'TransactionIdentifier' },
                    { data: 'DSCR' },
                    { data: 'LienPosition' },
                    { data: 'B1SelfEmploymentFlag' },
                    { data: 'B2SelfEmploymentFlag' },
                    { data: 'Occupancy' },
                    { data: 'NumberofMortgagedProperties' },
                    { data: 'NoteType' },
                    { data: 'NoteDate' },
                    { data: 'NumberofUnits' },
                    { data: 'Process' },
                    { data: 'ProjectId' },
                    { data: 'BillingType' }
                ],
                drawCallback: function () {
                    $('#billingdecription_table tbody .row-select').each(function () {
                        var id = $(this).val();
                        if (selectedLoanIds.includes(id)) {
                            $(this).prop('checked', true);
                        } else {
                            $(this).prop('checked', false);
                        }
                    });
                },
                initComplete: function () {
                    $('#load1').hide();
                    $('html, body').animate({
                        scrollTop: $('#billingdecription_table').offset().top - 100
                    }, 500);
                }
            });

            $('#billingdecription_table tbody').off('change', '.row-select').on('change', '.row-select', function () {
                var id = $(this).val();
                if ($(this).is(':checked')) {
                    if (!selectedLoanIds.includes(id)) {
                        selectedLoanIds.push(id);
                    }
                } else {
                    selectedLoanIds = selectedLoanIds.filter(item => item !== id);
                    $('#selectAllCheckbox').prop('checked', false);
                }
            });

            $('#selectAllCheckbox').off('change').on('change', function () {
                var isChecked = $(this).is(':checked');

                if (isChecked) {
                    selectedLoanIds = [];
                    table.rows().every(function () {
                        selectedLoanIds.push(String(this.data().id));
                    });
                    $('#billingdecription_table tbody .row-select').prop('checked', true);
                } else {
                    selectedLoanIds = [];
                    $('#billingdecription_table tbody .row-select').prop('checked', false);
                }
            });
        },
        error: function (err) {
            $('#load1').hide();
            console.log(err);
        }
    });
}

//Get Details Data
function GetBillingDecriptionDetails() {
    $('#load1').show();
    $.ajax({
        url: "GenerateBilling.aspx/GetBillingDecriptionDetails",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",

        success: function (data) {
            if ($.fn.dataTable.isDataTable('#billingdecription_detailstable')) {
                $('#billingdecription_detailstable').DataTable().destroy();
            }
            var dataArray = JSON.parse(data.d);
            console.log(dataArray[0]);
            $('#billingdecription_detailstable').DataTable({
                dom: 'Bftp',
                buttons: [
                    {
                        extend: 'excelHtml5',
                        text: 'Excel',
                        className: 'buttons-excel',
                        title: 'Billing_Description_Details'
                    }
                ],
                scrollX: true,
                destroy: true,
                paging: true,
                "ordering": false,
                "autoWidth": true,
                "data": dataArray,
                columns: [
                    {
                        data: null,
                        render: function (data, type, row, meta) {
                            return meta.row + 1;
                        }
                    },

                    { data: 'loanId' },
                    { data: 'createdDate' },
                    { data: 'submittedDate' },
                    { data: 'snapshotTakenDate' },
                    { data: 'BillTo' },
                    { data: 'buyerName' },
                    { data: 'sellerName' },
                    { data: 'scriptName' },
                    { data: 'TransactionIdentifier' },
                    { data: 'DSCR' },
                    { data: 'LienPosition' },
                    { data: 'B1SelfEmploymentFlag' },
                    { data: 'B2SelfEmploymentFlag' },
                    { data: 'Occupancy' },
                    { data: 'NumberofMortgagedProperties' },
                    { data: 'NoteType' },
                    { data: 'NoteDate' },
                    { data: 'NumberofUnits' },
                    { data: 'Process' },
                    { data: 'ProjectId' },
                    { data: 'BillingType' }


                ],
                initComplete: function () {
                    $('#load1').hide();
                }
            });
        },
        error: function (err) {
            $('#load1').hide();
            console.log(err);
        }
    });

    return false;
}

//When click show button show summary and details data 
function ShowBillingDecriptionData() {
    GetBillingDecriptionSummary();
    GetBillingDecriptionDetails();

    return false;
}




$(document).ready(function () {

    $(document).on('click', '#btngeneratebillingsendemail', function (e) {
        e.preventDefault();
        console.log("Button clicked!");

        if (typeof $('#load1') !== 'undefined' && $('#load1').length > 0) {
            $('#load1').show();
        }

        $.ajax({
            url: "GenerateBilling.aspx/SendBillingEmailWithExcel",
            type: "POST",
            data: "{}",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                if (typeof $('#load1') !== 'undefined' && $('#load1').length > 0) {
                    $('#load1').hide();
                }
                Swal.fire({
                    icon: 'success',
                    title: 'Success!',
                    text: 'Email sent successfully!',
                    showConfirmButton: true
                });
            },
            error: function (err) {
                if (typeof $('#load1') !== 'undefined' && $('#load1').length > 0) {
                    $('#load1').hide();
                }
                console.log(err);
                Swal.fire("Error", "Failed to send email. Please try again.", "error");
            }
        });

        return false;
    });

});