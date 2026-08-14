//#region Canopy
var flag = false;
function blankForNull(s) {
    return s == "null" || s == null ? "" : s;

}

var invrec_canopy;
var invrec_html_canopy = '';
var invrecdetails_canopy_stewart;
var stewart_html = '';


function getallSelectdeselect(chkall) {
    if (chkall.checked == true) {
        var data = invrecdetails_canopy_stewart.rows().data();
        data.each(function (value, index) {
            var billingid = value[2];
            document.getElementById("chkRec_" + billingid).checked = true;
        });
    }
    else {
        var data = invrecdetails_canopy_stewart.rows().data();
        data.each(function (value, index) {
            var billingid = value[2];
            document.getElementById("chkRec_" + billingid).checked = false;
        });
    }
}

function ReconcileInvoice_redirect(InvType, month, year, index) {
    location.href = "InvoiceReconciliationDetails.aspx?Type=" + InvType + "&Month=" + month + "&Year=" + year;
}

function BindCanopyGrid() {
    $('#load1').show();

    invrec_html_canopy = '';
    $.ajax({
        url: "InvoiceReconciliation.aspx/GetAllCanopyInvoiceForReconcile",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//
            $.each(dataArray, function (index, value) {
                invrec_html_canopy += '<tr>';
                invrec_html_canopy += '<td><div class="btn-group">';
                invrec_html_canopy += '<div class="btn-group">';
                invrec_html_canopy += '<div type="button" data-toggle="dropdown" aria-expanded="false"><i style="color: dodgerblue; font-size:14px;" class="uil fs-0 me-2 uil-cog"></i>';
                invrec_html_canopy += '<span class="sr-only"></span></div><div class="dropdown-menu" role="menu" style="">';
                invrec_html_canopy += '<a class="dropdown-item" href="#!" id="Actions" onclick="ReconcileInvoice_redirect(\'' + blankForNull(value.InvoiceType) + '\',\'' + blankForNull(value.Month) + '\',\'' + blankForNull(value.Year) + '\',' + index + ',1);"><span style="color: forestgreen;"><i class="uil fs-0 me-2 uil-align-center-v"></i></span>&nbsp;&nbsp;Reconcile Invoice</a>';
                invrec_html_canopy += '<a class="dropdown-item" href="#!" id="ActionsEx" onclick="Step3Approval(\'' + blankForNull(value.InvoiceType) + '\',\'' + blankForNull(value.Month) + '\',\'' + blankForNull(value.Year) + '\',' + index + ');"><span style="color: red;"><i class="uil fs-0 me-2 uil-document-layout-left"></i></span>&nbsp;&nbsp;View Loan Details</a><div class="dropdown-divider"></div></div></div></td>';

                invrec_html_canopy += '<td style="text-wrap: nowrap;text-align:center;">' + blankForNull((index + 1)) + '</td>';
                invrec_html_canopy += '<td style="text-wrap: nowrap; display:none;">' + blankForNull(value.InvoiceId) + '</td>';
                invrec_html_canopy += '<td style="text-wrap: nowrap;">' + blankForNull(value.Company) + '</td>';
                invrec_html_canopy += '<td style="text-wrap: nowrap;">' + blankForNull(value.Month) + '</td>';
                invrec_html_canopy += '<td style="text-wrap: nowrap;">' + blankForNull(value.Year) + '</td>';
                invrec_html_canopy += '<td style="text-wrap: nowrap;">' + blankForNull(value.InvoiceType) + '</td>';
                invrec_html_canopy += '<td style="text-wrap: nowrap;">' + blankForNull(value.InvoiceNo) + '</td>';
                invrec_html_canopy += '<td style="text-wrap: nowrap;">' + blankForNull(value.InvoiceDate) + '</td>';
                invrec_html_canopy += '<td style="text-wrap: nowrap;">' + blankForNull(value.DueDate) + '</td>';
                invrec_html_canopy += '<td style="text-wrap: nowrap;">' + blankForNull(value.BalanceNew) + '</td>';
                invrec_html_canopy += '<td style="text-wrap: nowrap;">' + blankForNull(value.NoOfLoans) + '</td>';
                invrec_html_canopy += '<td style="text-wrap: nowrap;">' + blankForNull(value.Remark) + '</td>';
                invrec_html_canopy += '</tr>';
            });

            if ($.fn.dataTable.isDataTable('#invrec_canopy')) {
                invrec_canopy.destroy();
            }
            $('#invrec_canopy tbody').html(invrec_html_canopy);
            //else
            invrec_canopy = $('#invrec_canopy').DataTable({
                dom: 'lftip',
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

function BindStewartForeconcile() {
    $('#load1').show();
    const urlParams = new URLSearchParams(window.location.search);
    const Month = urlParams.get('Month');
    const Year = urlParams.get('Year');
    stewart_html = '';
    $.ajax({
        url: "InvoiceReconciliationDetails.aspx/GetStewartDataForReconcile",
        type: "POST",
        dataType: "json",
        data: "{Month:'" + Month + "',Year:'" + Year + "'}",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//
            $.each(dataArray, function (index, value) {
                stewart_html += '<tr>';
                if (blankForNull(value.IsVerify) == "1") {
                    stewart_html += '<td style="text-wrap: nowrap;text-align:center;"><input type="checkbox" disabled="disabled" checked="checked" id="chkRec_' + blankForNull(value.BillingId) + '" /></td>';
                    document.getElementById("chkall").checked = true;
                    document.getElementById("chkall").disabled = true;
                }
                else {
                    stewart_html += '<td style="text-wrap: nowrap;text-align:center;"><input type="checkbox" id="chkRec_' + blankForNull(value.BillingId) + '" /></td>';
                    document.getElementById("chkall").checked = false;
                    document.getElementById("chkall").disabled = false;
                }
                stewart_html += '<td style="text-wrap: nowrap;text-align:center;">' + blankForNull((index + 1)) + '</td>';
                stewart_html += '<td style="text-wrap: nowrap; display:none;">' + blankForNull(value.BillingId) + '</td>';
                stewart_html += '<td style="text-wrap: nowrap; display:none;">' + blankForNull(value.InvoiceID) + '</td>';
                stewart_html += '<td style="text`-wrap: nowrap;">' + blankForNull(value.Month) + '</td>';
                stewart_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.Year) + '</td>';
                stewart_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.LoanNumber) + '</td>';
                stewart_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.CompleteDate) + '</td>';
                stewart_html += '<td style="text-wrap: nowrap;text-align:center;">' + blankForNull(value.Fee) + '</td>';
                stewart_html += '<td style="text-wrap: nowrap;text-align:center;">' + blankForNull(value.BilledRemark) + '</td>';
                stewart_html += '<td style="text-wrap: nowrap;text-align:center;display:none;">' + blankForNull(value.DisputeValue) + '</td>';
                stewart_html += '<td style="text-wrap: nowrap;text-align:center;"><input type="number" id="rec_disputevalue_' + index + '" style="text-align:center; width:50px;" value="' + blankForNull(value.DisputeValue) + '" /></td>';
                stewart_html += '<td style="text-wrap: nowrap;"><input type="text" id="rec_userremark_' + index + '" style="width:250px;" value="' + blankForNull(value.UserRemark) + '" /></td>';
                stewart_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.SysRemarkNew) + '</td>';
                stewart_html += '<td style="text-wrap: nowrap; display:none;">' + blankForNull(value.UserRemark) + '</td>';
                stewart_html += '</tr>';
            });

            if ($.fn.dataTable.isDataTable('#invrecdetails_canopy_stewart')) {
                invrecdetails_canopy_stewart.destroy();
            }
            $('#invrecdetails_canopy_stewart tbody').html(stewart_html);
            //else
            invrecdetails_canopy_stewart = $('#invrecdetails_canopy_stewart').DataTable({
                dom: 'lBftip',
                scrollx: true,
                destroy: true,
                "paging": false,
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
                        text: 'Verify',
                        class: 'html5',
                        action: function (e, dt, node, config) {
                            flag = false;
                            dt.rows().every(function (rowIdx, tableLoop, rowLoop) {
                                var data = this.data()
                                var billid = data[2];

                                if (document.getElementById("chkRec_" + billid).checked == true) {
                                    var disputevalue = document.getElementById("rec_disputevalue_" + rowIdx).value;
                                    var disputeremark = document.getElementById("rec_userremark_" + rowIdx).value;
                                    if (parseFloat(disputevalue) > 0 && disputeremark == "") {
                                        alert("Please enter remark for dispute record for loan # " + data[6]);
                                        document.getElementById("rec_userremark_" + rowIdx).focus();
                                        flag = true;
                                        return false;
                                    }
                                    else if (disputeremark == "") {
                                        alert("Please enter remark for loan # " + data[6]);
                                        document.getElementById("rec_userremark_" + rowIdx).focus();
                                        flag = true;
                                        return false;
                                    }
                                    else if (disputeremark == "RecordNotFound") {
                                        alert("Please enter remark for loan # " + data[6]);
                                        document.getElementById("rec_userremark_" + rowIdx).focus();
                                        flag = true;
                                        return false;
                                    }
                                    else {

                                        if (flag == false) {
                                            var billingid = data[2];
                                            var userremark = disputeremark;
                                            PageMethods.VerifyLoans(billingid, disputevalue, userremark, verifystewart_OnSuccess, verifystewart_OnError);
                                            flag = false;
                                        }
                                    }
                                    if (flag == true) {
                                        return false;
                                    }
                                }

                            });

                        }
                    }
                ],

                footerCallback: function (row, data, start, end, display) {
                    let api = this.api();

                    // Remove the formatting to get integer data for summation
                    let intVal = function (i) {
                        return typeof i === 'string'
                            ? i.replace(/[\$,]/g, '') * 1
                            : typeof i === 'number'
                                ? i
                                : 0;
                    };

                    // Total over all pages
                    total = api
                        .column(8)
                        .data()
                        .reduce((a, b) => intVal(a) + intVal(b), 0);

                    clienttotal = api
                        .column(9)
                        .data()
                        .reduce((a, b) => intVal(a) + intVal(b), 0);

                    disputetotal = api
                        .column(10)
                        .data()
                        .reduce((a, b) => intVal(a) + intVal(b), 0);

                    // Total over this page

                    // Update footer
                    api.column(6).footer().innerHTML =
                        total;
                    api.column(7).footer().innerHTML =
                        clienttotal;
                }


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

function verifystewart_OnSuccess(result) {
    InvoiceID_Canopy = result;

    if (result > 0) {
        document.getElementById("recinvoice_errmsg").innerHTML = "Loans verified successfully!";
        $('#recinvoice_dverror').modal('show');
        return false;
    }
    else if (result == -1) {
        document.getElementById("recinvoice_errmsg").innerHTML = "Loans are already verified!";
        document.getElementById("recinvoice_errmsg").style.color = 'red';
        $('#recinvoice_dverror').modal('show');
        return false;
    }
    else {
        document.getElementById("recinvoice_errmsg").innerHTML = "Oops! Error occured while verifying loans. Please contact administrator!";
        document.getElementById("recinvoice_errmsg").style.color = 'red';
        $('#recinvoice_dverror').modal('show');
        return false;
    }
    return false;
}
function verifystewart_OnError(error) {
    CanopyUI.showError(error, 'Unable to complete request.');
}

function recinvoice_closepopup() {
    BindStewartForeconcile();
    $('#recinvoice_dverror').modal('hide');
}

//#endregion Canopy