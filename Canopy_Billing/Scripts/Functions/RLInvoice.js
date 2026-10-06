//#region For Invoice Excel Upload

function canopyModalFire(options) {
    options = options || {};
    let modal = $('#canopyAjaxAlertModal');
    if (!modal.length) {
        $('body').append('<div class="modal fade" id="canopyAjaxAlertModal" tabindex="-1"><div class="modal-dialog modal-dialog-centered"><div class="modal-content"><div class="modal-header"><h5 class="modal-title"></h5><button type="button" class="close" data-dismiss="modal">&times;</button></div><div class="modal-body"></div><div class="modal-footer"><button type="button" class="btn btn-secondary modal-cancel" data-dismiss="modal">No</button><button type="button" class="btn btn-primary modal-confirm">OK</button></div></div></div></div>');
        modal = $('#canopyAjaxAlertModal');
    }
    modal.find('.modal-title').text(options.title || 'Message');
    const body = modal.find('.modal-body').empty();
    options.html ? body.html(options.html) : body.text(options.text || '');
    modal.find('.modal-cancel').toggle(!!options.showCancelButton).text(options.cancelButtonText || 'No');
    modal.find('.modal-confirm').text(options.confirmButtonText || 'OK');
    return new Promise(function (resolve) {
        modal.off('.canopyAlert');
        modal.on('click.canopyAlert', '.modal-confirm', function () { modal.modal('hide'); resolve({ isConfirmed: true }); });
        modal.on('hidden.bs.modal.canopyAlert', function () { resolve({ isConfirmed: false }); });
        modal.modal('show');
    });
}

window.Swal = { fire: canopyModalFire };

let rladdinvoice_currentCosting = null;
let rladdinvoice_pendingEdit = null;

function rlinvoice_bindBillingTable1() {
    $('#rlinvoice_billingTable').DataTable({
        processing: true,
        serverSide: false,
        destroy: true,
        ajax: {
            url: "Invoice.aspx/GetBillingData",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataSrc: function (response) {
                return response.d;
            }
        },
        columns: [
            { data: "OurClient" },
            { data: "Recipient" },
            { data: "TradeName" },
            { data: "InvoiceDate" },
            { data: "Document" },
            { data: "TM" },
            { data: "DocSign" },
            { data: "DocumentDate" },
            { data: "ExecutedDate" },
            { data: "LoanCount" },
            { data: "RLCost" },
            { data: "ExpectedBilling" },
            { data: "BillingEntity" },
            { data: "InvoiceIssued" },
            { data: "Notes" }
        ],
        scrollX: true, // important for many columns
        responsive: true
    });
}

function rlinvoice_uploadExcel() {
    $("#load1").show();
    $.ajax({
        url: "Invoice.aspx/BulkImport",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (res) {
            document.getElementById("rlinvoice_importcount").style.display = '';
            var result = typeof res.d === "string" ? JSON.parse(res.d) : res.d;
            $("#load1").hide();

            bindSummary(result.Summary);
            bindInsertedTable(result.Inserted);
            bindDuplicateTable(result.Duplicates);
            bindErrorTable(result.Errors);

            rladdinvoice_bindBillingTable();
        },
        error: function (err) {
            CanopyUI.debug(err);
            $("#load1").hide();
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Error occured while validating excel. Please contact administrator.'
            });
        }
    });
    return false;
}

function bindDuplicateTable(data) {

    if (!data || data.length === 0) {
        $('#duplicateTable').hide();              // hide table
        document.getElementById("hdup").style.display = 'none';
        //$('#duplicateTable').DataTable().clear().destroy(); // optional cleanup
        return false;
    }
    document.getElementById("hdup").style.display = '';

    $('#duplicateTable').show();

    $('#duplicateTable').DataTable({
        data: data,
        destroy: true,
        columns: [
            { title: "Our Client", data: "OurClient" },
            { title: "Trade Name", data: "TradeName" },
            { title: "Billing Entity", data: "BillingEntity" },
            { title: "Invoice Date", data: "InvoiceDate" }
        ]
    });
}

function bindErrorTable(data) {
    if (!data || data.length === 0) {
        $('#errorTable').hide();
        document.getElementById("herr").style.display = 'none';
        //$('#errorTable').DataTable().clear().destroy(); // optional
        return false;
    }
    document.getElementById("herr").style.display = '';

    $('#errorTable').show();

    $('#errorTable').DataTable({
        data: data,
        destroy: true,
        columns: [
            { title: "Our Client", data: "OurClient" },
            { title: "Trade Name", data: "TradeName" },
            { title: "Billing Entity", data: "BillingEntity" },
            { title: "Invoice Date", data: "InvoiceDate" },
            { title: "Transaction Manager", data: "TM" },
            { title: "Error", data: "ErrorMessage" }
        ]
    });
}

function bindInsertedTable(data) {

    if (!data || data.length === 0) {
        $('#rlinvoice_validatetable').hide();
        document.getElementById("hins").style.display = 'none';
        //$('#rlinvoice_validatetable').DataTable().clear().destroy();
        return false;
    }

    document.getElementById("hins").style.display = '';

    $('#rlinvoice_validatetable').show();

    $('#rlinvoice_validatetable').DataTable({
        data: data,
        columns: [
            { title: "Our Client", data: "OurClient" },
            { title: "Trade Name", data: "TradeName" },
            { title: "Billing Entity", data: "BillingEntity" },
            { title: "Invoice Date", data: "InvoiceDate" }
        ]
    });
}

function bindSummary(summary) {
    $('#totalCount').text(summary.Total);
    $('#insertedCount').text(summary.Inserted);
    $('#duplicateCount').text(summary.Duplicate);
    $('#errorCount').text(summary.Error || 0);
}


function rlinvocie_validate() {
    $('#rlinvoice_validatetable').DataTable({
        processing: true,
        serverSide: false,
        destroy: true,
        ajax: {
            url: "Invoice.aspx/ValidateInvoiceExel",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataSrc: function (response) {
                return JSON.parse(response.d);
            }
        },
        columns: [
            { data: "OurClient" },
            { data: "Recipient" },
            { data: "TradeName" },
            { data: "InvoiceDate" },
            { data: "Document" },
            { data: "TM" },
            { data: "DocSign" },
            { data: "DocumentDate" },
            { data: "ExecutedDate" },
            { data: "LoanCount" },
            { data: "RLCost" },
            { data: "ExpectedBilling" },
            { data: "BillingEntity" },
            { data: "InvoiceIssued" },
            { data: "Notes" }
        ],
        scrollX: true, // important for many columns
        responsive: true
    });

}

// #endregion For Invoice Excel Upload

// #region add single entry invoice

function rladdinvoice_submit() {
    let isEdit = $('#formMode').val() === 'edit';
    const documentType = $('#rladdinvoice_document').val();
    if (!$('#rladdinvoice_invoicedate').val() || !documentType || !$('#rladdinvoice_billingentity').val()) {
        Swal.fire({ icon: 'warning', title: 'Required fields', text: 'Invoice Date, Document, and Billing Entity are required.' });
        return false;
    }
    const invoiceMethod = rladdinvoice_currentCosting ? (rladdinvoice_currentCosting.BillingMethod || $('#rladdinvoice_billingmethod').val()) : $('#rladdinvoice_billingmethod').val();
    if (documentType === 'Securitization' && !invoiceMethod) {
        Swal.fire({ icon: 'warning', title: 'Billing Method', text: 'Select Per File or Hourly.' });
        return false;
    }
    if (invoiceMethod === 'Hourly' && Number($('#rladdinvoice_hoursworked').val() || 0) <= 0) {
        Swal.fire({ icon: 'warning', title: 'Hours Worked', text: 'Enter Hours Worked greater than zero.' });
        return false;
    }
    if (invoiceMethod === 'PerFile' && Number($('#rmaddinvoice_loancount').val() || 0) <= 0) {
        Swal.fire({ icon: 'warning', title: 'File Count', text: 'Enter File Count greater than zero.' });
        return false;
    }
    if ($('#rladdinvoice_product_costing').is(':visible') && rladdinvoice_collectProductDetails().length === 0) {
        Swal.fire({ icon: 'warning', title: 'Reliance Letter scope', text: 'Select one scope and enter its quantity.' });
        return false;
    }
    var data = {
        //InvoiceID: $('#hdnInvoiceID').val(),
        InvoiceID: isEdit ? $('#hdnInvoiceID').val() : 0,
        OurClient: $('#rladdinvoice_ourclient').val(),
        Recipient: $('#rladdinvoice_recipient').val(),
        TradeName: $('#rladdinvoice_tradename').val(),
        InvoiceDate: $('#rladdinvoice_invoicedate').val(),
        Document: $('#rladdinvoice_document').val(),
        TM: $('#rladdinvoice_tm').val(),
        DocSign: $('#rladdinvoice_docsign').val(),
        DocumentDate: $('#rladdinvoice_documentdate').val(),
        ExecutedDate: $('#rladdinvoice_executeddate').val(),
        BillingEntity: $('#rladdinvoice_billingentity option:selected').text(),
        ProjectID: Number($('#rladdinvoice_billingentity').val() || 0),
        EmailConfiguration: rladdinvoice_getConfiguredEmails().join(','),
        LoanCount: $('#rmaddinvoice_loancount').val(),
        RLCost: $('#rladdinvoice_cost').val(),
        ExpectedBilling: $('#rladdinvoice_expectedbilling').val(),
        Notes: $('#rladdinvoice_notes').val(),
        CostingRateID: rladdinvoice_currentCosting ? Number(rladdinvoice_currentCosting.RateID) : 0,
        BillingMethod: invoiceMethod,
        HoursWorked: Number($('#rladdinvoice_hoursworked').val() || 0),
        MinimumAmount: Number($('#rladdinvoice_minimum').val() || 0),
        MaximumCap: Number($('#rladdinvoice_cap').val() || 0),
        ProductDetails: rladdinvoice_collectProductDetails(),
        UpdateMasterRates: false
    };
    CanopyUI.debug(data);
    let url = ($('#formMode').val() === 'edit')
        ? "Invoice.aspx/UpdateInvoice"
        : "Invoice.aspx/SaveInvoice";
    const saveInvoice = function () { $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ model: data }),
        success: function (res) {
            var result = res.d;
            if (result && result.Status === false) {
                Swal.fire({ icon: 'error', title: 'Unable to save', text: result.Message });
                return;
            }
            Swal.fire({
                icon: 'success',
                title: 'Success',
                text: result.Message
            });
            $('#formMode').val('');
            $('#hdnInvoiceID').val('');
            rladdinvoice_bindBillingTable(); // refresh table
            clearForm();

        },
        error: function (xhr, status, error) {
            CanopyUI.debug(xhr);
            let errMsg = "Something went wrong";
            if (xhr.responseJSON && xhr.responseJSON.Message) {
                errMsg = xhr.responseJSON.Message;
            }
            else if (xhr.responseText) {
                try {
                    let res = JSON.parse(xhr.responseText);
                    errMsg = res.Message || res.ExceptionMessage || xhr.responseText;
                } catch {
                    errMsg = xhr.responseText;
                }
            }

            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: errMsg
            });
        }
    }); };

    if (rladdinvoice_hasMasterRateChanges()) {
        canopyModalFire({ title: 'Rate changed', text: 'The rate for this client has changed. Do you want to update the master data?', showCancelButton: true, confirmButtonText: 'Yes, update master', cancelButtonText: 'No' })
            .then(function (result) { data.UpdateMasterRates = result.isConfirmed; saveInvoice(); });
    } else saveInvoice();

    return false;
}

function clearForm() {
    $('#rladdinvoice_ourclient').val('');
    $('#rladdinvoice_recipient').val('');
    $('#rladdinvoice_tradename').val('');
    $('#rladdinvoice_invoicedate').val('');
    $('#rladdinvoice_document').val('');
    $('#rladdinvoice_tm').val('');
    $('#rladdinvoice_docsign').val('');
    $('#rladdinvoice_documentdate').val('');
    $('#rladdinvoice_executeddate').val('');
    $('#rladdinvoice_billingentity').val('');
    $('#rladdinvoice_emailchips').empty();
    rladdinvoice_refreshEmailUI();
    $('#rladdinvoice_newemail').val('');
    $('#contactSection').hide();
    $('#rmaddinvoice_loancount').val('');
    $('#rladdinvoice_cost').val('');
    $('#rladdinvoice_expectedbilling').val('');
    $('#rladdinvoice_notes').val('');
    $('#rladdinvoice_billingmethod, #rladdinvoice_hoursworked, #rladdinvoice_minimum, #rladdinvoice_cap').val('');
    $('#rladdinvoice_product_rows').empty();
    $('#rladdinvoice_flexible_costing, #rladdinvoice_product_costing').hide();
    $('#rladdinvoice_loancount_field, #rladdinvoice_cost_field').show();
    $('#rladdinvoice_loancount_label').text('Loan Count');
    $('#rladdinvoice_cost_label').text('Cost');
    $('#rladdinvoice_cost').prop('readonly', false);
    rladdinvoice_currentCosting = null;
}

function rladdinvoice_bindBillingTable() {

    $('#rladdinvoice_billingTable').DataTable({
        destroy: true,
        scrollX: true,
        ajax: {
            url: "Invoice.aspx/GetBillingData",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataSrc: function (res) {
                return res.d;
            }
        },
        columns: [
            {
                data: null,
                render: function (data, type, row) {

                    // ? If already verified ? show text
                    if (row.IsVerify === true) {
                        return `<span style="color:green; font-weight:bold;">Verified</span>`;
                    }

                    // ? Else show checkbox
                    return `<input type="checkbox" class="verifyChk" data-id="${row.InvoiceID}" />`;
                },
                orderable: false
            },
            {
                data: null,
                render: function (data, type, row) {
                    let actions = `<div class="dropdown invoice-action-menu">
                        <button type="button" class="btn btn-sm btn-secondary dropdown-toggle"
                            data-toggle="dropdown" data-boundary="viewport" aria-haspopup="true" aria-expanded="false">
                            <i class="fas fa-cog"></i>&nbsp; Actions
                        </button>
                        <div class="dropdown-menu dropdown-menu-right">`;

                    if (!row.IsVerify) {
                        actions += `<button type="button" class="dropdown-item editBtn" data-id="${row.InvoiceID}">
                            <i class="fas fa-edit"></i><span>Edit</span>
                        </button>`;
                        actions += `<button type="button" class="dropdown-item deleteBtn" data-id="${row.InvoiceID}">
                            <i class="fas fa-trash-alt"></i><span>Delete</span>
                        </button>`;
                    } else {
                        actions += `<button type="button" class="dropdown-item" disabled title="Locked (Verified)">
                            <i class="fas fa-edit"></i><span>Edit (Verified)</span>
                        </button>`;
                    }

                    actions += `<button type="button" class="dropdown-item historyBtn" data-id="${row.InvoiceID}">
                        <i class="fas fa-history"></i><span>History</span>
                    </button>`;
                    actions += `<button type="button" class="dropdown-item uploadBtn" data-id="${row.InvoiceID}">
                        <i class="fas fa-plus-circle"></i><span>Upload</span>
                    </button>`;

                    if (row.FilePath) {
                        actions += `<button type="button" class="dropdown-item viewBtn" data-file="${row.FilePath}">
                            <i class="fas fa-eye"></i><span>View</span>
                        </button>`;
                        actions += `<button type="button" class="dropdown-item downloadBtn" data-file="${row.FilePath}">
                            <i class="fas fa-download"></i><span>Download</span>
                        </button>`;
                    }

                    actions += `</div></div>`;
                    return actions;
                }
            },
            { data: "BillingEntity" },
            { data: "TradeName" },
            { data: "InvoiceDate" },
            { data: "LoanCount" },
            { data: "ExpectedBilling" },
            { data: "RLCost" },
            { data: "Recipient" },

            { data: "OurClient" },
            { data: "Document" },
            { data: "TM" },
            { data: "DocSign" },
            { data: "DocumentDate" },
            { data: "ExecutedDate" },
            { data: "Notes" }
        ],
        initComplete: function () {

            var api = this.api();

            // Billing Entity Dropdown
            var billingColumn = api.column(14);

            billingColumn.data().unique().sort().each(function (d) {
                $('#billingFilter').append(`<option value="${d}">${d}</option>`);
            });
        }
    });
    let startDate = null;
    let endDate = null;

    $('#dateRange').daterangepicker({
        autoUpdateInput: false,
        locale: {
            cancelLabel: 'Clear'
        }
    });

    $('#dateRange').on('apply.daterangepicker', function (ev, picker) {

        startDate = picker.startDate.clone();
        endDate = picker.endDate.clone();

        CanopyUI.debug("Start:", startDate.format('YYYY-MM-DD'));
        CanopyUI.debug("End:", endDate.format('YYYY-MM-DD'));

        $(this).val(
            picker.startDate.format('MM/DD/YYYY') + ' - ' +
            picker.endDate.format('MM/DD/YYYY')
        );

        $('#rladdinvoice_billingTable').DataTable().draw();
    });

    $('#dateRange').on('cancel.daterangepicker', function () {
        $(this).val('');
        startDate = null;
        endDate = null;
        $('#rladdinvoice_billingTable').DataTable().draw();
    });

    $.fn.dataTable.ext.search.push(function (settings, data) {

        // If no date selected ? show all
        if (!startDate && !endDate) return true;

        var invoiceDate = data[5]; // your InvoiceDate column

        if (!invoiceDate) return true;

        // ? STEP 1: Convert "03.16.2026" ? Date
        var parts = invoiceDate.split('.'); // ["03","16","2026"]

        if (parts.length !== 3) return true; // safety

        var rowDate = new Date(
            parseInt(parts[2], 10),      // year
            parseInt(parts[0], 10) - 1,  // month (0-based)
            parseInt(parts[1], 10)       // day
        );

        // ? STEP 2: Convert picker dates
        var min = startDate ? startDate.startOf('day').toDate() : null;
        var max = endDate ? endDate.endOf('day').toDate() : null;

        // ? STEP 3: Compare
        if (
            (min === null && max === null) ||
            (min === null && rowDate <= max) ||
            (min <= rowDate && max === null) ||
            (rowDate >= min && rowDate <= max)
        ) {
            return true;
        }

        return false;
    });

    $('#tradeFilter').on('keyup', function () {
        $('#rladdinvoice_billingTable').DataTable().column(4).search(this.value).draw();
    });
    $('#billingFilter').on('change', function () {
        $('#rladdinvoice_billingTable').DataTable().column(14).search(this.value).draw();
    });
    $('#clearFilters').on('click', function () {

        $('#tradeFilter').val('');
        $('#billingFilter').val('');
        $('#dateRange').val('');

        startDate = null;
        endDate = null;

        $('#rladdinvoice_billingTable').DataTable().search('').columns().search('').draw();
        return false;
    });

    $(document).on('click', '.viewBtn', function () {

        let filePath = $(this).data('file');

        fetch("DownloadFile.ashx?file=" + filePath)
            .then(res => res.arrayBuffer())
            .then(data => {
                setTimeout(() => {
                    $("#excelTable td").each(function () {
                        let textLength = $(this).text().length;
                        let width = Math.min(Math.max(textLength * 8, 80), 300);
                        $(this).css("min-width", width + "px");
                    });
                }, 100);

                let workbook = XLSX.read(data, { type: "array" });

                let sheet = workbook.Sheets[workbook.SheetNames[0]];

                //let html = XLSX.utils.sheet_to_html(sheet);
                let html = XLSX.utils.sheet_to_html(sheet, {
                    id: "excelTable",
                    editable: false
                });

                $("#excelPreviewContainer").html(html);

                $("#excelModal").modal('show');
            });
    });
    $(document).on('change', '.verifyChk', function () {

        let checkedCount = $('.verifyChk:checked').length;

        if (checkedCount > 0) {
            $('#verifySection').show();
        } else {
            $('#verifySection').hide();
        }
    });


}

$(document).on('click', '#btnVerifySubmit', function () {

    let selectedIds = [];
    let remark = $('#verifyRemark').val().trim();

    if (remark === "") {
        alert("Please enter remark");
        return;
    }

    $('.verifyChk:checked').each(function () {
        selectedIds.push($(this).data('id'));
    });

    if (selectedIds.length === 0) {
        alert("Please select at least one row");
        return;
    }

    $.ajax({
        type: "POST",
        url: "Invoice.aspx/VerifyInvoices",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            ids: selectedIds,
            remark: remark
        }),
        success: function (res) {

            Swal.fire({
                icon: 'success',
                title: 'Success',
                text: 'Record verified successfully.'
            });

            $('#verifyRemark').val('');
            $('#verifySection').hide();

            $('#rladdinvoice_billingTable').DataTable().ajax.reload();
        },
        error: function () {

            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Error occurred while verifying record.'
            });
        }
    });

    return false;
});

$(document).on('click', '.deleteBtn', function () {
    const invoiceId = Number($(this).data('id'));

    Swal.fire({
        icon: 'warning',
        title: 'Delete invoice?',
        text: 'This action cannot be undone.',
        showCancelButton: true,
        confirmButtonColor: '#dc3545',
        confirmButtonText: 'Delete'
    }).then(function (result) {
        if (!result.isConfirmed) return;

        $.ajax({
            type: 'POST',
            url: 'Invoice.aspx/DeleteInvoice',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ invoiceId: invoiceId }),
            success: function (res) {
                if (!res.d.Success) {
                    Swal.fire({ icon: 'error', title: 'Unable to delete', text: res.d.Message });
                    $('#rladdinvoice_billingTable').DataTable().ajax.reload(null, false);
                    return;
                }

                Swal.fire({ icon: 'success', title: 'Deleted', text: res.d.Message });
                $('#verifySection').hide();
                $('#rladdinvoice_billingTable').DataTable().ajax.reload(null, false);
            },
            error: function () {
                Swal.fire({ icon: 'error', title: 'Error', text: 'The invoice could not be deleted.' });
            }
        });
    });
});

function rladdinvoice_bindcompany() {
    var select = document.getElementById("rladdinvoice_billingentity");
    let options = select.getElementsByTagName('option');

    for (var i = options.length; i--;) {
        select.removeChild(options[i]);
    }
    $("#rladdinvoice_billingentity").append($("<option></option>").val("").html("Select"));
    $.ajax({
        type: "POST", url: "Invoice.aspx/GetAllClientList", dataType: "json", contentType: "application/json",
        success: function (res) {
            var dataArray = JSON.parse(res.d);
            $.each(dataArray, function (data, value) {
                $("#rladdinvoice_billingentity").append($("<option></option>").val(value.ProjectID).html(value.ClientName));
            })
        }
    });
}

function rladdinvoice_getRLSecRate(emailConfiguration) {

    var select = document.getElementById("rladdinvoice_billingentity");
    var projectid = select.options[select.selectedIndex].value;
    rladdinvoice_loadCostingConfiguration();

    // let clientId = $('#rladdinvoice_billingentity').val();
    let clientName = $('#rladdinvoice_billingentity option:selected').text();
    if (projectid && clientName && clientName !== "Select") {

        // ? Show section
        $('#contactSection').show();

        rladdinvoice_loadEmails(clientName, emailConfiguration);

    } else {

        // ? Hide section
        $('#contactSection').hide();

        // ? Clear values
        $('#rladdinvoice_emailchips').empty();
        rladdinvoice_refreshEmailUI();
        $('#rladdinvoice_newemail').val('');
    }
}

function rladdinvoice_resetCostingUI() {
    rladdinvoice_currentCosting = null;
    $('#rladdinvoice_flexible_costing, #rladdinvoice_product_costing').hide();
    $('#rladdinvoice_product_rows').empty();
    $('#rladdinvoice_loancount_field, #rladdinvoice_cost_field').show();
    $('#rladdinvoice_loancount_label').text('Loan Count');
    $('#rladdinvoice_cost_label').text('Cost');
    $('#rladdinvoice_cost').prop('readonly', false).removeAttr('data-master-rate');
    $('#rladdinvoice_billingmethod').prop('disabled', false);
    $('#rladdinvoice_billingmethod, #rladdinvoice_hoursworked, #rladdinvoice_minimum, #rladdinvoice_cap').val('');
    $('#rladdinvoice_minimum, #rladdinvoice_cap').removeAttr('data-master-value');
    $('#rladdinvoice_adjustment').text('');
}

function rladdinvoice_loadCostingConfiguration() {
    const projectId = Number($('#rladdinvoice_billingentity').val() || 0);
    const documentType = $('#rladdinvoice_document').val();
    const invoiceDate = $('#rladdinvoice_invoicedate').val();
    rladdinvoice_resetCostingUI();
    if (!projectId || !documentType) return;

    $.ajax({
        type: 'POST', url: 'Invoice.aspx/GetFlexibleCosting', contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ projectId: projectId, documentType: documentType, invoiceDate: invoiceDate }),
        success: function (res) {
            const result = res.d || {}, header = result.Header;
            if (!header) {
                $('#rladdinvoice_cost').val('0');
                if (documentType === 'Securitization') {
                    $('#rladdinvoice_flexible_costing').show();
                    $('#rladdinvoice_billingmethod').prop('disabled', false).val('');
                    $('#rladdinvoice_cost').prop('readonly', false);
                    rladdinvoice_manualMethodChanged();
                } else if (documentType === 'Reliance Letter') {
                    $('#rladdinvoice_product_costing').show();
                    $('#rladdinvoice_loancount_field, #rladdinvoice_cost_field').hide();
                    rladdinvoice_addManualProductRow();
                }
                Swal.fire({ icon: 'info', title: 'Rate not configured', text: 'Enter the rate while raising this invoice. It will also be saved in costing configuration.' });
                return;
            }
            rladdinvoice_currentCosting = header;
            $('#rladdinvoice_cost').val(header.Rate || 0);
            const pending = rladdinvoice_pendingEdit;

            if (documentType === 'Securitization') {
                const configuredMethod = header.BillingMethod || '';
                const hourly = configuredMethod === 'Hourly';
                $('#rladdinvoice_flexible_costing').show();
                $('#rladdinvoice_billingmethod').prop('disabled', !!configuredMethod).val(configuredMethod);
                $('#rladdinvoice_hours_field').toggle(hourly);
                $('#rladdinvoice_loancount_field').toggle(!hourly);
                $('#rladdinvoice_loancount_label').text('File Count');
                $('#rladdinvoice_cost_label').text(hourly ? 'Hourly Rate' : 'Rate / File');
                $('#rladdinvoice_cost').prop('readonly', false).attr('data-master-rate', Number(header.Rate || 0));
                $('#rladdinvoice_minimum').val(header.MinimumAmount || 0).attr('data-master-value', Number(header.MinimumAmount || 0));
                $('#rladdinvoice_cap').val(header.MaximumCap || 0).attr('data-master-value', Number(header.MaximumCap || 0));
                if (pending) $('#rladdinvoice_hoursworked').val(pending.HoursWorked || '');
                if (!configuredMethod) rladdinvoice_manualMethodChanged();
            } else if (documentType === 'Reliance Letter') {
                $('#rladdinvoice_product_costing').show();
                $('#rladdinvoice_loancount_field, #rladdinvoice_cost_field').hide();
                const saved = pending && pending.ProductDetails ? pending.ProductDetails : [];
                (result.Details || []).forEach(function (detail) {
                    const prior = saved.find(function (item) { return Number(item.CostingDetailID) === Number(detail.CostingDetailID); }) || {};
                    rladdinvoice_addInvoiceProductRow(detail, prior);
                });
                if (!result.Details || !result.Details.length) rladdinvoice_addManualProductRow(header.Rate);
                if (result.Details && result.Details.length > 1 && saved.length === 0) {
                    $('#rladdinvoice_product_rows .scope-select').prop('checked', false);
                    rladdinvoice_updateScopeSelection();
                }
                rladdinvoice_refreshDeleteButtons();
            }
            getexpectedamount();
            rladdinvoice_pendingEdit = null;
        }
    });
}

function rladdinvoice_addInvoiceProductRow(config, saved) {
    const quantity = saved.Quantity == null ? 0 : saved.Quantity;
    const row = $('<tr></tr>').attr('data-detail-id', config.CostingDetailID).attr('data-rate', config.Rate).attr('data-master-rate', config.Rate);
    row.append($('<td class="text-center"></td>').append($('<input type="radio" name="rlInvoiceScope" class="scope-select" />').prop('checked', saved.Quantity != null)));
    row.append($('<td class="invoice-product"></td>').text(config.ProductType));
    row.append($('<td></td>').append($('<input type="number" min="0" step="0.01" class="form-control invoice-product-quantity" />').val(quantity)));
    row.append($('<td></td>').append($('<input type="number" min="0" step="0.01" class="form-control invoice-manual-rate" />').val(Number(config.Rate).toFixed(2))));
    row.append($('<td class="invoice-product-amount"></td>').text((Number(quantity) * Number(config.Rate)).toFixed(2)));
    row.append('<td><button type="button" class="btn btn-sm btn-danger invoice-remove-product" title="Delete"><i class="fas fa-trash-alt"></i></button></td>');
    $('#rladdinvoice_product_rows').append(row);
    rladdinvoice_refreshDeleteButtons();
}

$(document).on('input', '.invoice-product-quantity', getexpectedamount);

function rladdinvoice_collectProductDetails() {
    const details = [];
    $('#rladdinvoice_product_rows tr').has('.scope-select:checked').each(function () {
        const row = $(this), quantity = Number(row.find('.invoice-product-quantity').val() || 0), rate = Number(row.attr('data-rate') || 0);
        if (quantity > 0) details.push({ CostingDetailID: Number(row.attr('data-detail-id') || 0), ProductType: row.find('.invoice-product-input').length ? row.find('.invoice-product-input').val().trim() : row.find('.invoice-product').text(), Quantity: quantity, Rate: rate, Amount: quantity * rate });
    });
    return details;
}

function rladdinvoice_manualMethodChanged() {
    const method = $('#rladdinvoice_billingmethod').val();
    const hourly = method === 'Hourly';
    $('#rladdinvoice_hours_field').toggle(hourly);
    $('#rladdinvoice_loancount_field').toggle(!hourly);
    $('#rladdinvoice_loancount_label').text('File Count');
    $('#rladdinvoice_cost_label').text(hourly ? 'Hourly Rate' : 'Rate / File');
    $('#rladdinvoice_minimum').closest('.erp-field').toggle(method === 'PerFile');
    getexpectedamount();
}

function rladdinvoice_addManualProductRow(defaultRate) {
    defaultRate = Number(defaultRate || 0);
    const scopeNumber = $('#rladdinvoice_product_rows tr').length + 1;
    const row = $('<tr data-detail-id="0"></tr>').attr('data-rate', defaultRate).attr('data-master-rate', defaultRate);
    row.append($('<td class="text-center"></td>').append('<input type="radio" name="rlInvoiceScope" class="scope-select" checked />'));
    row.append($('<td></td>').append($('<input type="text" maxlength="250" class="form-control invoice-product-input" />').val('Reliance Letter Scope ' + scopeNumber)));
    row.append($('<td></td>').append('<input type="number" min="0" step="0.01" class="form-control invoice-product-quantity" />'));
    row.append($('<td></td>').append($('<input type="number" min="0" step="0.01" class="form-control invoice-manual-rate" />').val(defaultRate || '')));
    row.append('<td class="invoice-product-amount">0.00</td>');
    row.append('<td><button type="button" class="btn btn-sm btn-danger invoice-remove-product"><i class="fas fa-trash-alt"></i></button></td>');
    $('#rladdinvoice_product_rows').append(row);
    rladdinvoice_refreshDeleteButtons();
    return false;
}

$(document).on('click', '.invoice-remove-product', function () { $(this).closest('tr').remove(); rladdinvoice_refreshDeleteButtons(); getexpectedamount(); });
$(document).on('input', '.invoice-manual-rate', function () { $(this).closest('tr').attr('data-rate', Number($(this).val() || 0)); getexpectedamount(); });
$(document).on('change', '.scope-select', function () { rladdinvoice_updateScopeSelection(); getexpectedamount(); });

function rladdinvoice_hasMasterRateChanges() {
    if (!rladdinvoice_currentCosting) return false;
    if ($('#rladdinvoice_document').val() === 'Securitization')
        return Number($('#rladdinvoice_cost').val() || 0) !== Number($('#rladdinvoice_cost').attr('data-master-rate') || 0)
            || Number($('#rladdinvoice_minimum').val() || 0) !== Number($('#rladdinvoice_minimum').attr('data-master-value') || 0)
            || Number($('#rladdinvoice_cap').val() || 0) !== Number($('#rladdinvoice_cap').attr('data-master-value') || 0);
    let changed = false;
    $('#rladdinvoice_product_rows tr').each(function () {
        if (Number($(this).attr('data-detail-id') || 0) > 0 && Number($(this).attr('data-rate') || 0) !== Number($(this).attr('data-master-rate') || 0)) changed = true;
    });
    return changed;
}

function rladdinvoice_refreshDeleteButtons() {
    const showDelete = $('#rladdinvoice_product_rows tr').length > 1;
    $('#rladdinvoice_product_rows .invoice-remove-product').toggle(showDelete);
    if ($('#rladdinvoice_product_rows tr').length === 1) $('#rladdinvoice_product_rows .scope-select').prop('checked', true);
    rladdinvoice_updateScopeSelection();
}

function rladdinvoice_updateScopeSelection() {
    $('#rladdinvoice_product_rows tr').each(function () {
        const selected = $(this).find('.scope-select').is(':checked');
        $(this).toggleClass('table-active', selected);
        $(this).find('.invoice-product-quantity, .invoice-manual-rate').prop('disabled', !selected);
    });
}

function rladdinvoice_loadEmails(clientName, configuredEmails) {
    if (!clientName || clientName === "Select") {
        CanopyUI.debug("Invalid Billing Entity");
        return;
    }
    $.ajax({
        type: "POST",
        url: "Invoice.aspx/GetBillingEntityEmails",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ clientName: clientName }),
        success: function (res) {
            let response = res.d || {};
            let emails = configuredEmails === undefined || configuredEmails === null
                ? (response.DefaultEmails || [])
                : rladdinvoice_parseEmails(configuredEmails);

            $('#rladdinvoice_emailchips').empty();
            emails.forEach(function (email) {
                rladdinvoice_appendEmail(email);
            });
            rladdinvoice_refreshEmailUI();
        },
        error: function (xhr) {
            CanopyUI.debug(xhr.responseText);

            let errMsg = "Error occurred";

            try {
                let res = JSON.parse(xhr.responseText);
                errMsg = res.Message || res.ExceptionMessage;
            } catch {
                errMsg = xhr.responseText;
            }

            alert(errMsg);
        }
    });



}

function rladdinvoice_parseEmails(value) {
    return (value || '').split(/[;,]/).map(function (email) {
        return email.trim();
    }).filter(function (email, index, all) {
        return email && all.findIndex(function (item) {
            return item.toLowerCase() === email.toLowerCase();
        }) === index;
    });
}

function rladdinvoice_getConfiguredEmails() {
    return $('#rladdinvoice_emailchips .erp-email-chip').map(function () {
        return $(this).attr('data-email');
    }).get();
}

function rladdinvoice_appendEmail(email) {
    let exists = rladdinvoice_getConfiguredEmails().some(function (item) {
        return item.toLowerCase() === email.toLowerCase();
    });
    if (!exists) {
        let chip = $('<div></div>')
            .addClass('erp-email-chip')
            .attr('role', 'listitem')
            .attr('data-email', email);
        chip.append($('<i></i>').addClass('fas fa-envelope').attr('aria-hidden', 'true'));
        chip.append($('<span></span>').addClass('erp-email-chip-text').text(email));
        chip.append($('<button></button>')
            .attr('type', 'button')
            .addClass('erp-email-remove')
            .attr('title', 'Remove ' + email + ' from this invoice')
            .attr('aria-label', 'Remove ' + email)
            .append($('<i></i>').addClass('fas fa-times').attr('aria-hidden', 'true')));
        $('#rladdinvoice_emailchips').append(chip);
        rladdinvoice_refreshEmailUI();
    }
}

function rladdinvoice_refreshEmailUI() {
    let list = $('#rladdinvoice_emailchips');
    let count = rladdinvoice_getConfiguredEmails().length;
    list.find('.erp-email-empty').remove();
    if (count === 0) {
        list.append($('<div></div>')
            .addClass('erp-email-empty')
            .append($('<i></i>').addClass('far fa-envelope-open').attr('aria-hidden', 'true'))
            .append($('<span></span>').text('No recipients selected. Add an email below.')));
    }
    $('#rladdinvoice_emailcount').text(count + (count === 1 ? ' email' : ' emails'));
}

function rladdinvoice_addEmail() {
    let clientName = $('#rladdinvoice_billingentity option:selected').text();
    let email = $('#rladdinvoice_newemail').val().trim();
    if (!$('#rladdinvoice_billingentity').val()) {
        Swal.fire({ icon: 'warning', title: 'Billing Entity', text: 'Select a Billing Entity first.' });
        return false;
    }
    if (!email) {
        $('#rladdinvoice_newemail').focus();
        return false;
    }

    let addButton = $('#rladdinvoice_addemailbtn');
    addButton.prop('disabled', true);
    $.ajax({
        type: 'POST',
        url: 'Invoice.aspx/AddBillingEntityEmail',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ clientName: clientName, email: email }),
        success: function (res) {
            if (!res.d.Status) {
                Swal.fire({ icon: 'warning', title: 'Email', text: res.d.Message });
                return;
            }
            rladdinvoice_appendEmail(res.d.Email);
            $('#rladdinvoice_newemail').val('');
        },
        error: function (xhr) {
            CanopyUI.debug(xhr.responseText);
            Swal.fire({ icon: 'error', title: 'Email', text: 'Unable to add the email address.' });
        },
        complete: function () {
            addButton.prop('disabled', false);
        }
    });
    return false;
}

$(document).on('click', '.erp-email-remove', function () {
    $(this).closest('.erp-email-chip').remove();
    rladdinvoice_refreshEmailUI();
});

$(document).on('keydown', '#rladdinvoice_newemail', function (event) {
    if (event.key === 'Enter') {
        event.preventDefault();
        rladdinvoice_addEmail();
    }
});

$(document).on('click', '.editBtn', function () {

    let table = $('#rladdinvoice_billingTable').DataTable();
    let data = table.row($(this).parents('tr')).data();

    $('#hdnInvoiceID').val(data.InvoiceID);
    $('#formMode').val('edit');

    $('#rladdinvoice_ourclient').val(data.OurClient);
    $('#rladdinvoice_recipient').val(data.Recipient);
    $('#rladdinvoice_tradename').val(data.TradeName);

    $('#rladdinvoice_invoicedate').val(formatDateForInput(data.InvoiceDate));
    $('#rladdinvoice_document').val(data.Document);
    $('#rladdinvoice_tm').val(data.TM);
    $('#rladdinvoice_docsign').val(data.DocSign);

    $('#rladdinvoice_documentdate').val(formatDateForInput(data.DocumentDate));
    $('#rladdinvoice_executeddate').val(formatDateForInput(data.ExecutedDate));

    $('#rmaddinvoice_loancount').val(data.LoanCount);
    $('#rladdinvoice_cost').val(data.RLCost);
    $('#rladdinvoice_expectedbilling').val(data.ExpectedBilling);
    $('#rladdinvoice_notes').val(data.Notes);
    rladdinvoice_pendingEdit = data;

    // ? Billing Entity
    //$('#rladdinvoice_billingentity').val(data.BillingEntity);

    setBillingEntityByText(data.BillingEntity, data.EmailConfiguration);
    $('#rladdinvoice_btnsubmit').text('Update');

    $('html, body').animate({ scrollTop: 0 }, 500);
    // ? Load contacts + select existing
    let clientId = data.BillingEntity;
    // ? safety check
    if (!clientId || clientId === "" || clientId === null) {
        CanopyUI.debug("Invalid BillingEntity:", clientId);
        return;
    }

    return false;
});

$(document).on('click', '.historyBtn', function () {

    //let invoiceId = $(this).data('id');
    //$('#historyModal').data('invoiceId', invoiceId);
    let table = $('#rladdinvoice_billingTable').DataTable();
    let data = table.row($(this).parents('tr')).data();

    let invoiceId = data.InvoiceID;
    let tradeName = data.TradeName;

    $('#historyModal').data('invoiceId', invoiceId);
    $('#historyModal').data('tradeName', tradeName);

    $.ajax({
        type: "POST",
        url: "Invoice.aspx/GetInvoiceHistory",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ invoiceId: invoiceId }),
        success: function (res) {

            let data = res.d;

            // Destroy if already initialized
            if ($.fn.DataTable.isDataTable('#historyTable')) {
                $('#historyTable').DataTable().clear().destroy();
            }

            $('#historyTable').DataTable({
                data: data,
                paging: true,
                pageLength: 10,
                searching: true,
                ordering: true,
                destroy: true,

                columns: [
                    { data: "FieldName" },
                    { data: "OldValue" },
                    { data: "NewValue" },
                    { data: "UpdatedBy" },
                    { data: "UpdatedOn" }
                ],

                dom: 'Bfrtip', // Buttons + filter + table

                buttons: [
                    {
                        extend: 'excelHtml5',
                        text: 'Export to Excel',
                        filename: function () {

                            let name = $('#historyModal').data('tradeName') || 'Invoice';

                            // ? clean filename (important)
                            //name = name.replace(/[^a-z0-9]/gi, '_');
                            name = name.replace(/[^a-z0-9 ]/gi, '').replace(/\s+/g, '_');

                            let date = new Date().toISOString().slice(0, 10);

                            return `History_${name}_${date}`;
                        }
                    }
                ]
            });

            $('#historyModal').modal('show');
        }
    });
});

function setBillingEntityByText(text, emailConfiguration) {

    let ddl = $('#rladdinvoice_billingentity');

    ddl.find('option').each(function () {
        if ($(this).text().trim() === text.trim()) {
            ddl.val($(this).val());
            rladdinvoice_getRLSecRate(emailConfiguration);
            return false; // break loop
        }
    });
}

function getexpectedamount() {
    let amount = 0;
    if ($('#rladdinvoice_product_costing').is(':visible')) {
        $('#rladdinvoice_product_rows tr').has('.scope-select:checked').each(function () {
            const row = $(this), line = Number(row.find('.invoice-product-quantity').val() || 0) * Number(row.attr('data-rate') || 0);
            row.find('.invoice-product-amount').text(line.toFixed(2));
            amount += line;
        });
    } else {
        const rate = Number($('#rladdinvoice_cost').val() || 0);
        const method = rladdinvoice_currentCosting ? rladdinvoice_currentCosting.BillingMethod : $('#rladdinvoice_billingmethod').val();
        const quantity = method === 'Hourly' ? Number($('#rladdinvoice_hoursworked').val() || 0) : Number($('#rmaddinvoice_loancount').val() || 0);
        const base = quantity * rate, minimum = Number($('#rladdinvoice_minimum').val() || 0), cap = Number($('#rladdinvoice_cap').val() || 0);
        amount = base;
        const adjustments = [];
        if (method === 'PerFile' && minimum > 0 && amount < minimum) { amount = minimum; adjustments.push('Minimum billing applied'); }
        if (cap > 0 && amount > cap) { amount = cap; adjustments.push('Maximum cap applied'); }
        $('#rladdinvoice_adjustment').text(adjustments.join(' • '));
    }
    $('#rladdinvoice_expectedbilling').val(amount.toFixed(2));
}

function rladdinvoice_closeModal() {
    $('#uploadModal').modal("hide");
    $('#fileUploadControl').val('');
    return false;
}


function rladdinvoice_submitFile() {

    var file = $('#fileUploadControl')[0].files[0];

    if (!file) {
        alert("Please select file");
        return;
    }

    var formData = new FormData();
    formData.append("file", file);
    formData.append("id", selectedRowId);

    $.ajax({
        url: "FileUpload.ashx",
        type: "POST",
        data: formData,
        contentType: false,
        processData: false,
        success: function (res) {

            alert("File uploaded successfully");

            rladdinvoice_closeModal();
            rladdinvoice_bindBillingTable();
        },
        error: function (err) {
            CanopyUI.debug(err);
        }
    });
}

function formatDateForInput(dateString) {

    if (!dateString) return "";

    // Handle format like "03.16.2026"
    let parts = dateString.split('.');

    if (parts.length === 3) {
        let mm = parts[0].padStart(2, '0');
        let dd = parts[1].padStart(2, '0');
        let yyyy = parts[2];

        return `${yyyy}-${mm}-${dd}`; // yyyy-MM-dd
    }

    const jsonDate = /\/Date\((\d+)\)\//.exec(dateString);
    if (jsonDate) dateString = new Date(Number(jsonDate[1]));
    // Handle ISO or other formats
    let date = new Date(dateString);
    if (isNaN(date)) return "";

    let yyyy = date.getFullYear();
    let mm = String(date.getMonth() + 1).padStart(2, '0');
    let dd = String(date.getDate()).padStart(2, '0');

    return `${yyyy}-${mm}-${dd}`;
}
// #endregion add single entry invoice

//#region Costing of SEc and RL
function secrel_bindcompany() {
    var select = document.getElementById("secrel_project");
    let options = select.getElementsByTagName('option');

    for (var i = options.length; i--;) {
        select.removeChild(options[i]);
    }
    $("#secrel_project").append($("<option></option>").val("").html("Select"));
    $.ajax({
        type: "POST", url: "Invoice.aspx/GetAllClientList", dataType: "json", contentType: "application/json",
        success: function (res) {
            var dataArray = JSON.parse(res.d);
            $.each(dataArray, function (data, value) {
                $("#secrel_project").append($("<option></option>").val(value.ProjectID).html(value.ClientName));
            })
        }
    });
}

function secrel_toggleConfigurationFields() {
    const type = $('#secrel_type').val();
    const method = $('#secrel_method').val();
    const isSec = type === 'Securitization';
    const isRl = type === 'Reliance Letter';

    $('#secrel_method_field').toggle(isSec);
    $('#secrel_rate_field').toggle(isSec);
    $('#secrel_minimum_field').toggle(isSec && method === 'PerFile');
    $('#secrel_cap_field').toggle(isSec);
    $('#secrel_product_details').toggle(isRl);
    $('#secrel_rate_field label').text(method === 'Hourly' ? 'Hourly Rate' : 'Rate / File');
}

$(document).on('change', '#secrel_type, #secrel_method', secrel_toggleConfigurationFields);

function secrel_addProductRow(detail) {
    detail = detail || {};
    if (!detail.ProductType) detail.ProductType = 'Reliance Letter Scope ' + ($('#secrel_product_rows tr').length + 1);
    const row = $('<tr></tr>');
    row.append($('<td></td>').append($('<input type="text" maxlength="250" class="form-control product-description" />').val(detail.ProductType || '')));
    row.append($('<td></td>').append($('<input type="number" min="0" step="0.01" class="form-control product-rate" />').val(detail.Rate == null ? '' : detail.Rate)));
    row.append($('<td></td>').append('<button type="button" class="btn btn-sm btn-danger remove-product-row" title="Delete"><i class="fas fa-trash-alt"></i></button>'));
    $('#secrel_product_rows').append(row);
    secrel_refreshDeleteButtons();
    return false;
}

$(document).on('click', '.remove-product-row', function () { $(this).closest('tr').remove(); secrel_refreshDeleteButtons(); });

function secrel_refreshDeleteButtons() {
    $('#secrel_product_rows .remove-product-row').toggle($('#secrel_product_rows tr').length > 1);
}

function secrel_BindOtherCosting() {
    $('#load1').show();
    $.ajax({
        url: "OtherCosting.aspx/GetAllOtherCosting",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",

        success: function (data) {
            if ($.fn.dataTable.isDataTable('#secrel_table')) {
                $('#secrel_table').DataTable().destroy();
            }
            dataArray = JSON.parse(data.d);
            $('#secrel_table').DataTable({
                dom: 'ftp',
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
                columns: [
                    {
                        data: null,
                        render: function (data, type, row) {
                            return `<i class="fa fa-edit text-primary editBtncost" style="cursor:pointer;" title="Edit"></i>
                                    <i class="fa fa-history historyBtncost text-info" style="cursor:pointer;"></i>`;
                        }
                    },
                    { data: 'ProjectName' },
                    { data: 'Rate' },
                    { data: 'Type' },
                    { data: 'BillingMethod', defaultContent: '' },
                    { data: 'MinimumAmount', defaultContent: '' },
                    { data: 'MaximumCap', defaultContent: '' },
                    { data: 'AddedByName' },
                    { data: 'AddedDate1' }

                ],

                initComplete: function () {
                    $('#load1').hide();

                },
            });

        }
    });

    return false;
}

function formatDate(jsonDate) {
    if (!jsonDate) return '';

    let date = new Date(parseInt(jsonDate.replace(/\/Date\((\d+)\)\//, '$1')));
    return date.toLocaleString('en-IN');
}


$(document).on('click', '.historyBtncost', function () {
    var table = $('#secrel_table').DataTable();
    var data = table.row($(this).closest('tr')).data();

    PageMethods.GetOtherCostingHistory(data.RateID, function (res) {

        let history = JSON.parse(res);

        let html = `
                <style>
                .history-table {
                    width: 100%;
                    border-collapse: collapse;
                    font-size: 13px;
                    text-align: left;
                }
                .history-table th {
                    background: #f1f1f1;
                    padding: 6px;
                    font-weight: 600;
                    border: 1px solid #ddd;
                    white-space: nowrap;
                }
                .history-table td {
                    padding: 6px;
                    border: 1px solid #ddd;
                }
                .history-table tr:nth-child(even) {
                    background: #fafafa;
                }
                </style>

                <table class="history-table">
                <thead>
                <tr>
                    <th>Project</th>
                    <th>Rate</th>
                    <th>Type</th>
                    <th>Action</th>
                    <th>User</th>
                    <th>Date</th>
                </tr>
                </thead>
                <tbody>
                `;

        history.forEach(x => {
            html += `
                    <tr>
                        <td>${x.ClientName || '-'}</td>
                        <td>${x.Rate}</td>
                        <td>${x.Type}</td>
                        <td>${x.ActionType}</td>
                        <td>${x.ChangedByName}</td>
                        <td>${formatDate(x.ChangedDate)}</td>
                    </tr>`;
        });

        html += `</tbody></table>`;

        Swal.fire({
            title: 'History',
            html: html,
            width: '800px'
        });

    });
});

$(document).on('click', '#secrel_table tbody .editBtncost', function () {

    var table = $('#secrel_table').DataTable();
    var row = $(this).closest('tr');

    // Fix for child rows (responsive / scroll)
    if (row.hasClass('child')) {
        row = row.prev();
    }

    var data = table.row(row).data();

    CanopyUI.debug(data);

    $.ajax({
        type: 'POST',
        url: 'OtherCosting.aspx/GetCostingConfiguration',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ rateId: data.RateID }),
        success: function (res) {
            const result = res.d;
            if (!result.Status) { Swal.fire({ icon: 'error', title: 'Error', text: result.Message }); return; }
            const header = result.Header;
            $('#secrel_project').val(header.ProjectId);
            $('#secrel_rate').val(header.Rate);
            $('#secrel_type').val(header.Type);
            $('#secrel_method').val(header.BillingMethod || '');
            $('#secrel_minimum').val(header.MinimumAmount == null ? '' : header.MinimumAmount);
            $('#secrel_cap').val(header.MaximumCap == null ? '' : header.MaximumCap);
            $('#secrel_effectivefrom').val(formatDateForInput(header.EffectiveFrom));
            $('#secrel_product_rows').empty();
            (result.Details || []).forEach(secrel_addProductRow);
            $('#secrel_btnsubmit').data('edit-id', header.RateID).text('Update');
            secrel_toggleConfigurationFields();
            $('html, body').animate({ scrollTop: 0 }, 300);
        }
    });
});

function secrel_submit() {
    var ddlproject = document.getElementById("secrel_project");
    var projectid = ddlproject.options[ddlproject.selectedIndex].value;
    if (projectid == "") {
        alert("Please select project.");
        return false;
    }
    var ddltype = document.getElementById("secrel_type");
    var type = ddltype.options[ddltype.selectedIndex].value;
    if (type == "") {
        alert("Please enter type.");
        return false;
    }
    const method = $('#secrel_method').val();
    if (type === 'Securitization' && !method) {
        Swal.fire({ icon: 'warning', title: 'Billing Method', text: 'Select Per File or Hourly.' });
        return false;
    }
    if (type === 'Securitization' && Number($('#secrel_rate').val() || 0) <= 0) {
        Swal.fire({ icon: 'warning', title: 'Rate', text: 'Enter a rate greater than zero.' });
        return false;
    }

    const details = [];
    $('#secrel_product_rows tr').each(function () {
        const row = $(this);
        const product = row.find('.product-description').val().trim();
        const rate = row.find('.product-rate').val();
        if (product || rate) details.push({ ProductType: product, Rate: Number(rate) });
    });

    const model = {
        RateID: Number($('#secrel_btnsubmit').data('edit-id') || 0),
        ProjectID: Number(projectid), DocumentType: type, BillingMethod: method,
        Rate: Number($('#secrel_rate').val() || 0),
        MinimumAmount: Number($('#secrel_minimum').val() || 0),
        MaximumCap: Number($('#secrel_cap').val() || 0),
        EffectiveFrom: $('#secrel_effectivefrom').val() || null,
        Details: details
    };
    $.ajax({
        type: 'POST', url: 'OtherCosting.aspx/SaveCostingConfiguration',
        contentType: 'application/json; charset=utf-8', data: JSON.stringify({ model: model }),
        success: function (res) { res.d.Status ? secrel_OnSuccess(1) : Swal.fire({ icon: 'error', title: 'Unable to save', text: res.d.Message }); },
        error: secrel_OnError
    });
    return false;
}

function secrel_OnSuccess(result) {
    if (result > 0) {
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: 'Costing configuration saved successfully.'
        });
        // Reset form
        $('#secrel_project').val('');
        $('#secrel_rate').val('');
        $('#secrel_type').val('');
        $('#secrel_method, #secrel_minimum, #secrel_cap, #secrel_effectivefrom').val('');
        $('#secrel_product_rows').empty();
        $('#secrel_btnsubmit').removeData('edit-id').text('Submit');
        secrel_toggleConfigurationFields();
        secrel_BindOtherCosting();
        return false;
    }
    else {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Rate already exists for selected client.'
        });
        return false;
    }
    return false;
}
function secrel_OnError(error) {
    CanopyUI.showError(error, 'Unable to complete request.');
}


//#endregion Costing of SEc and RL
