//#region For Invoice Excel Upload

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
        EmailConfiguration: rladdinvoice_getConfiguredEmails().join(','),
        LoanCount: $('#rmaddinvoice_loancount').val(),
        RLCost: $('#rladdinvoice_cost').val(),
        ExpectedBilling: $('#rladdinvoice_expectedbilling').val(),
        Notes: $('#rladdinvoice_notes').val()
    };
    CanopyUI.debug(data);
    let url = ($('#formMode').val() === 'edit')
        ? "Invoice.aspx/UpdateInvoice"
        : "Invoice.aspx/SaveInvoice";
    $.ajax({
        url: url,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ model: data }),
        success: function (res) {
            var result = res.d;
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
    });

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


                    let actions = '';

                    // ?? Edit
                    if (!row.IsVerify) {
                        actions += `<i class="fa fa-edit action-icon editBtn" 
                        title="Edit" data-id="${row.InvoiceID}"></i>`;
                    } else {
                        actions += `<i class="fa fa-edit action-icon disabled" 
                        title="Locked (Verified)"></i>`;
                    }

                    // ?? History
                    actions += `<i class="fa fa-history action-icon historyBtn" 
                    title="History" data-id="${row.InvoiceID}"></i>`;

                    // ? Upload
                    actions += `<i class="fa fa-plus-circle action-icon uploadBtn" 
                    title="Upload" data-id="${row.InvoiceID}"></i>`;

                    // ?? View (if file exists)
                    if (row.FilePath) {
                        actions += `<i class="fa fa-eye action-icon viewBtn" 
                        title="View" data-file="${row.FilePath}"></i>`;

                        actions += `<i class="fa fa-download action-icon downloadBtn" 
                        title="Download" data-file="${row.FilePath}"></i>`;
                    }

                    return `<div class="action-container">${actions}</div>`;
                }
            },
            { data: "BillingEntity" },
            { data: "TradeName" },
            { data: "InvoiceDate" },
            { data: "LoanCount" },
            { data: "ExpectedBilling" },
            { data: "Recipient" },
            { data: "RLCost" },
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
    var projectname = select.options[select.selectedIndex].text;

    $.ajax({
        type: "POST", url: "Invoice.aspx/GetRLCostingByProject", data: "{ProjectID:" + projectid + "}", dataType: "json", contentType: "application/json",
        success: function (res) {
            var dataArray = JSON.parse(res.d);
            if (dataArray.length <= 0) {
                Swal.fire({
                    icon: 'warning',
                    title: 'Rate Configuration',
                    text: "Rate not configured for '" + projectname + "'"
                });
                //alert("Rate not configured for '" + projectname + "'");
                document.getElementById("rladdinvoice_cost").value = "0";
            }
            else {
                $.each(dataArray, function (data, value) {
                    document.getElementById("rladdinvoice_cost").value = value.Rate;
                })
            }
        }
    });

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
    var loancount = document.getElementById("rmaddinvoice_loancount").value;
    var rate = document.getElementById("rladdinvoice_cost").value;
    if (rate != "") {
        if (loancount != "") {
            document.getElementById("rladdinvoice_expectedbilling").value = parseFloat(loancount) * parseFloat(rate);
        }
    }
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

    $('#secrel_project').val(data.ProjectId);
    $('#secrel_rate').val(data.Rate);
    $('#secrel_type').val(data.Type);

    $('#secrel_submitBtn').data('edit-id', data.ID);
});

function secrel_submit() {
    var ddlproject = document.getElementById("secrel_project");
    var projectid = ddlproject.options[ddlproject.selectedIndex].value;
    if (projectid == "") {
        alert("Please select project.");
        return false;
    }
    var rate = document.getElementById("secrel_rate").value;
    if (rate == "") {
        alert("Please enter rate.");
        return false;
    }
    var ddltype = document.getElementById("secrel_type");
    var type = ddltype.options[ddltype.selectedIndex].value;
    if (type == "") {
        alert("Please enter type.");
        return false;
    }
    var editId = $('#secrel_submitBtn').data('edit-id');

    if (editId) {
        // ? UPDATE
        PageMethods.UpdateOtherRates(editId, projectid, rate, type, secrel_OnSuccess, secrel_OnError);
    } else {
        // ? INSERT
        PageMethods.InsertOtherRates(projectid, rate, type, secrel_OnSuccess, secrel_OnError);
    }

    //PageMethods.InsertOtherRates(projectid, rate, type, secrel_OnSuccess, secrel_OnError);
    return false;
}

function secrel_OnSuccess(result) {
    if (result > 0) {
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: 'Rate added auccessfully.'
        });
        // Reset form
        $('#secrel_project').val('');
        $('#secrel_rate').val('');
        $('#secrel_type').val('');
        $('#secrel_submitBtn').removeData('edit-id');
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
