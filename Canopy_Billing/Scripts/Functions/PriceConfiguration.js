var bpd_price_table;
var bpd_price_html = '';
const chkIds = [];
const rate_chkIds = [];
var ProjectID_param;

function GetCheckedCheckboxes(ID) {
    if (ID.checked) {
        if (!chkIds.includes(ID.id)) {
            chkIds.push(ID.id);
        }
    }
    else {
        if (chkIds.includes(ID.id)) {
            chkIds.splice(chkIds.indexOf(ID.id), 1);
        }
    }

    return false;
}

function rate_GetCheckedCheckboxes(ID) {
    if (ID.checked) {
        if (!rate_chkIds.includes(ID.id)) {
            rate_chkIds.push(ID.id);
        }
    }
    else {
        if (rate_chkIds.includes(ID.id)) {
            rate_chkIds.splice(rate_chkIds.indexOf(ID.id), 1);
        }
    }

    return false;
}

function blankForNull(s) {
    return s == "null" || s == null ? "" : s;
}

function BindCostingParameters() {
    $('#load1').show();
    const urlParams = new URLSearchParams(window.location.search);
    const ProcessID = urlParams.get('ProcessID');
    bpd_price_html = '';
    $.ajax({
        url: "PriceConfiguration.aspx/GetProjectDetailsbyprocessID",
        type: "POST",
        dataType: "json",
        data: "{ProcessID:" + ProcessID + "}",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//
            $.each(dataArray, function (index, value) {
                var ProjectId = value.ProjectId;
                var ProcessName = value.ProcessName;
                document.getElementById("goback_price").href = "ProjectDetails.aspx?ProjectID=" + ProjectId;
                document.getElementById("price_project_id").innerHTML = ProjectId;
                document.getElementById("price_process_name").innerHTML = ProcessName;
                document.getElementById("bpd_header_price").innerHTML = "Billing Parameter and Costing Configuration : " + ProcessName;
                ProjectID_param = ProjectId;
                //////////////////Bind Grid
                $.ajax({
                    url: "PriceConfiguration.aspx/getAllBillingParameters",
                    type: "POST",
                    dataType: "json",
                    data: "{ProjectID:" + ProjectId + ", ClientProcess:'" + ProcessName + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (data1) {
                        var dataArray1 = JSON.parse(data1.d);//

                        $.each(dataArray1, function (index, value1) {
                            if (value1.IBP_Id == 1) {
                                var baserate = blankForNull(value1.IBV_Remark);
                                if (baserate == "") baserate = "0";
                                document.getElementById("price_baserate").value = baserate;
                            }
                            else {
                                bpd_price_html += '<tr>';
                                bpd_price_html += '<td style="text-wrap: nowrap;text-align:center; display:none;">' + blankForNull((index + 1)) + '</td>';
                                if (blankForNull(value1.IBV_ChargeType) != "Select" && blankForNull(value1.IBV_ChargeType) != "") {
                                    bpd_price_html += '<td style="text-wrap: nowrap;"><input checked="checked" type="checkbox" id="' + value1.IBP_Id + '" onclick="GetCheckedCheckboxes(this,' + index + ')" /></td>';
                                    if (!chkIds.includes(value1.IBP_Id)) {
                                        chkIds.push(value1.IBP_Id);
                                    }
                                }
                                else
                                    bpd_price_html += '<td style="text-wrap: nowrap;"><input type="checkbox" id="' + value1.IBP_Id + '" onclick="GetCheckedCheckboxes(this,' + index + ')" /></td>';
                                bpd_price_html += '<td style="text-wrap: nowrap;" id="bpd_paramname_' + value1.IBP_Id + '">' + blankForNull(value1.IBP_ParameterName) + '</td>';
                                bpd_price_html += '<td style="text-wrap: nowrap;"><select class="form-control" id="bpd_billingtype_' + value1.IBP_Id + '" style="width:100px; height:26px;"><option value="">select</option>';

                                const billingtype = value1.IBP_BillingType.split("/");
                                for (let i = 0; i < billingtype.length; i++) {
                                    let options = billingtype[i];
                                    if (value1.IBV_BillingType == options)
                                        bpd_price_html += '<option value="' + options + '" selected>' + options + '</option>';
                                    else
                                        bpd_price_html += '<option value="' + options + '">' + options + '</option>';
                                }
                                bpd_price_html += '</select></td>';
                                bpd_price_html += '<td style="text-wrap: nowrap;"><input class="form-control" type="text" id="bpd_price_' + value1.IBP_Id + '" style="width:100px; height:26px;" value="' + blankForNull(value1.IBV_Remark) + '"/></td>';
                                if (value1.IBV_ChargeType == "Fix Amount")
                                    bpd_price_html += '<td style="text-wrap: nowrap; "><select class="form-control" id="bpd_chargetype_' + value1.IBP_Id + '" style="width:120px; height:26px;"><option value="">select</option><option value="Fix Amount" selected>Fix Amount</option><option value="Variable">Variable</option></select></td>';
                                else if (value1.IBV_ChargeType == "Variable")
                                    bpd_price_html += '<td style="text-wrap: nowrap; "><select class="form-control" id="bpd_chargetype_' + value1.IBP_Id + '" style="width:120px; height:26px;"><option value="">select</option><option value="Fix Amount">Fix Amount</option><option value="Variable" selected>Variable</option></select></td>';
                                else
                                    bpd_price_html += '<td style="text-wrap: nowrap; "><select class="form-control" id="bpd_chargetype_' + value1.IBP_Id + '" style="width:120px; height:26px;"><option value="">select</option><option value="Fix Amount">Fix Amount</option><option value="Variable">Variable</option></select></td>';
                                bpd_price_html += '</tr>';
                            }
                        });

                        if ($.fn.dataTable.isDataTable('#bpd_price_table')) {
                            bpd_price_table.destroy();
                        }
                        $('#bpd_price_table tbody').html(bpd_price_html);
                        //else
                        bpd_price_table = $('#bpd_price_table').DataTable({
                            dom: 't',
                            scrollX: true,
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
                                    extend: 'excelHtml5', title: 'Bank Names', autoFilter: true,
                                    exportOptions: {
                                        columns: [0, 1, 2],
                                    }

                                },


                            ],

                        });
                    },
                    error: function (error) {
                        CanopyUI.showError(error, 'Unable to complete request.');
                    }
                });

                //////////////////////////////


            });
        },
        error: function (error) {
            CanopyUI.showError(error, 'Unable to complete request.');
        }
    });

    return false;
}

function bpd_price_submit() {
    var params = "";
    var AllParamsters = "";
    $("#waitingpanel").modal("show");
    if (chkIds.length > 0) {
        var baseprice = document.getElementById("price_baserate").value;
        for (let i = 0; i < chkIds.length; i++) {
            var parid = chkIds[i];
            var ddlbilingtype = document.getElementById("bpd_billingtype_" + chkIds[i]);
            var bilingtype = ddlbilingtype.options[ddlbilingtype.selectedIndex].value;
            var price = document.getElementById("bpd_price_" + chkIds[i]).value;
            if (price == "")
                price = "0";
            var ddlchargetype = document.getElementById("bpd_chargetype_" + chkIds[i]);
            var chargetype = ddlchargetype.options[ddlchargetype.selectedIndex].value;
            var paramname = document.getElementById("bpd_paramname_" + chkIds[i]).innerHTML;
            params = parid + "~" + bilingtype + "~" + price + "~" + chargetype + "~" + paramname;
            AllParamsters = params + "|" + AllParamsters;

        }
        var ProjectId_par = document.getElementById("price_project_id").innerHTML;
        var ProcessName_par = document.getElementById("price_process_name").innerHTML;
        if (AllParamsters != "") {
            PageMethods.InsertCosting(ProjectId_par, ProcessName_par, baseprice, AllParamsters, bpd_prices_OnSuccess, bpd_price_OnError);
            return false;
        }
        else {
            alert("Please select atleast one parameter.");
            return false;
        }
    }
    else {
        alert("Please select atleast one parameter.");
        return false;
    }
}

function bpd_prices_OnSuccess(result) {
    $("#waitingpanel").modal("hide");
    if (result > 0) {
        document.getElementById("bpd_price_errmsg").innerHTML = "Process/ Deal is configured successfully. Please click <b>OK</b> to redirect to main page.";
        $('#bpd_price_dverror').modal('show');
        return false;
    }
    else {
        document.getElementById("bpd_price_errmsg").innerHTML = "Oops! System is facing connectivity issue. Please try after some time.";
        $('#bpd_price_dverror').modal('show');
        return false;
    }
    return false;
}
function bpd_price_OnError(error) {
    $("#waitingpanel").modal("hide");
    CanopyUI.showError(error, 'Unable to complete request.');
}

function bpd_price_MessageRedirect() {
    $('#bpd_price_dverror').modal('hide');
    location.href = "ProjectDetails.aspx?ProjectID=" + ProjectID_param;
}

//Billing Header Configuration
function bhconf_BindBillingHeaderConfiguration() {
    $('#load1').show();
    $.ajax({
        url: "BillingHeaderConfiguration.aspx/GetBillingHeaders",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",

        success: function (data) {
            if ($.fn.dataTable.isDataTable('#bhconf_table')) {
                $('#bhconf_table').DataTable().destroy();
            }
            dataArray = JSON.parse(data.d);
            $('#bhconf_table').DataTable({
                dom: 'lBftp',
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
                    { data: 'IBP_ParameterName' }

                ],

                initComplete: function () {
                    $('#load1').hide();

                },
            });

        }
    });

    return false;
}


function bhconf_submit() {
    var header = document.getElementById("bhconf_name").value;
    if (header == "") {
        alert("Please enter Billing Header");
        return false;
    }
    PageMethods.InsertBillingHeader(header, bhconf_OnSuccess, bhconf_OnError);
    return false;
}

function bhconf_OnSuccess(result) {
    if (result > 0) {
        document.getElementById("bhconf_errmsg").innerHTML = "Billing Header/ Parameter added successfully.";
        $('#bhconf_dverror').modal('show');
        return false;
    }
    else {
        document.getElementById("bhconf_errmsg").innerHTML = "Billing Header/ Parameter already exists.";
        $('#bhconf_dverror').modal('show');
        return false;
    }
    return false;
}
function bhconf_OnError(error) {
    CanopyUI.showError(error, 'Unable to complete request.');
}

function bhconf_MessageRedirect() {
    $('#bhconf_dverror').modal('hide');
    document.getElementById("bhconf_name").value = "";
    bhconf_BindBillingHeaderConfiguration();
}

function BindCostingParametersForRateRevision() {
    $('#load1').show();
    const urlParams = new URLSearchParams(window.location.search);
    const ProcessID = urlParams.get('ProcessID');
    var rate_bpd_price_html = '';
    $.ajax({
        url: "PriceConfiguration.aspx/GetProjectDetailsbyprocessID",
        type: "POST",
        dataType: "json",
        data: "{ProcessID:" + ProcessID + "}",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//
            $.each(dataArray, function (index, value) {
                var ProjectId = value.ProjectId;
                var ProcessName = value.ProcessName;
                document.getElementById("rate_goback_price").href = "ProjectDetails.aspx?ProjectID=" + ProjectId;
                document.getElementById("rate_price_project_id").innerHTML = ProjectId;
                document.getElementById("rate_recipients").innerHTML = blankForNull(value.EmailList);
                document.getElementById("rate_bpd_header_price").innerHTML = "Billing Parameter and Costing Configuration : " + ProcessName;
                ProjectID_param = ProjectId;
                //////////////////Bind Grid
                $.ajax({
                    url: "RateRevisionConfiguration.aspx/getAllBillingParameters_Rate",
                    type: "POST",
                    dataType: "json",
                    data: "{ProjectID:" + ProjectId + ", ClientProcess:'" + ProcessName + "'}",
                    contentType: "application/json; charset=utf-8",
                    success: function (data1) {
                        var dataArray1 = JSON.parse(data1.d);//

                        $.each(dataArray1, function (index, value1) {
                            var addeddate = eval(value1.IBP_AddedDate1.replace(/\/Date\((\d+)\)\//gi, "new Date($1).toLocaleDateString(\"en-US\")"));

                            rate_bpd_price_html += '<tr>';
                            rate_bpd_price_html += '<td style="text-wrap: nowrap;text-align:center; display:none;">' + blankForNull((index + 1)) + '</td>';
                            if (blankForNull(value1.Exists1) == "Checked") {
                                rate_bpd_price_html += '<td style="text-wrap: nowrap;"><input checked="checked" type="checkbox" id="' + value1.IBP_Id + '" onclick="rate_GetCheckedCheckboxes(this,' + index + ')" /></td>';
                                if (!chkIds.includes(value1.IBP_Id)) {
                                    chkIds.push(value1.IBP_Id);
                                }
                            }
                            else
                                rate_bpd_price_html += '<td style="text-wrap: nowrap;"><input type="checkbox" id="' + value1.IBP_Id + '" onclick="rate_GetCheckedCheckboxes(this,' + index + ')" /></td>';
                            rate_bpd_price_html += '<td style="text-wrap: nowrap;" id="rate_bpd_paramname_' + value1.IBP_Id + '">' + blankForNull(value1.IBP_ParameterName1) + '</td>';
                            rate_bpd_price_html += '<td style="text-wrap: nowrap;"><input class="form-control disabled" type="text" id="rate_bpd_price_' + value1.IBP_Id + '" style="width:100px; height:26px;" value="' + blankForNull(value1.IBV_Remark) + '"/></td>';
                            rate_bpd_price_html += '<td style="text-wrap: nowrap;"><input class="form-control disabled" type="text" id="rate_bpd_price_effectivedate' + value1.IBP_Id + '" style="width:100px; height:26px;" value="' + blankForNull(addeddate) + '"/></td>';
                            rate_bpd_price_html += '<td style="text-wrap: nowrap;"><select class="form-control" id="rate_bpd_revisiontype_' + value1.IBP_Id + '" style="width:150px; height:28px;"><option value="">select</option>';
                            if (blankForNull(value1.RevisionType) == "Half Yearly")
                                rate_bpd_price_html += '<option value="Half Yearly" selected>Half Yearly</option>';
                            else
                                rate_bpd_price_html += '<option value="Half Yearly">Half Yearly</option>';
                            
                            if (blankForNull(value1.RevisionType) == "Yearly")
                                rate_bpd_price_html += '<option value="Yearly" selected>Yearly</option>';
                            else
                                rate_bpd_price_html += '<option value="Yearly">Yearly</option>';
                            rate_bpd_price_html += '</select></td>';
                            rate_bpd_price_html += '<td style="text-wrap: nowrap;"><input class="form-control" type="date" id="rate_bpd_price_date' + value1.IBP_Id + '" style="width:120px; height:26px;" value="'+blankForNull(value1.ReminderDate)+'"/></td>';
                            rate_bpd_price_html += '<td style="text-wrap: nowrap;"><input class="form-control" type="text" id="rate_bpd_price_days' + value1.IBP_Id + '" style="width:100px; height:26px;" value="' + blankForNull(value1.ReminderDays) +'"/></td>';

                            rate_bpd_price_html += '</tr>';
                        });

                        if ($.fn.dataTable.isDataTable('#rate_bpd_price_table')) {
                            bpd_price_table.destroy();
                        }
                        $('#rate_bpd_price_table tbody').html(rate_bpd_price_html);
                        //else
                        bpd_price_table = $('#rate_bpd_price_table').DataTable({
                            dom: 't',
                            scrollX: true,
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
                                    extend: 'excelHtml5', title: 'Bank Names', autoFilter: true,
                                    exportOptions: {
                                        columns: [0, 1, 2],
                                    }

                                },


                            ],

                        });
                    },
                    error: function (error) {
                        CanopyUI.showError(error, 'Unable to complete request.');
                    }
                });

                //////////////////////////////


            });
        },
        error: function (error) {
            CanopyUI.showError(error, 'Unable to complete request.');
        }
    });

    return false;
}

function rate_updateemails() {
    $("#waitingpanel").modal("show");
    var ProjectId_par = document.getElementById("rate_price_project_id").innerHTML;
    var emaillist = document.getElementById("rate_recipients").value;
    PageMethods.UpdateEmailList(ProjectId_par, emaillist, updateemail_OnSuccess, updateemail_OnError);
    return false;
}

function updateemail_OnSuccess(result) {
    $("#waitingpanel").modal("hide");
    if (result > 0) {
        alert("Emails updated successfully.");
        return false;
    }
    else {
        alert("Oops! System is facing connectivity issue. Please try after some time.");
        return false;
    }
    return false;
}
function updateemail_OnError(error) {
    $("#waitingpanel").modal("hide");
    CanopyUI.showError(error, 'Unable to complete request.');
}

function rate_updatereminder() {
    var params = "";
    var AllParamsters = "";
    const urlParams = new URLSearchParams(window.location.search);
    const ProcessID = urlParams.get('ProcessID');
    $("#waitingpanel").modal("show");
    if (rate_chkIds.length > 0) {
        for (let i = 0; i < rate_chkIds.length; i++) {
            var parid = rate_chkIds[i];
            var ddlrevisiontype = document.getElementById("rate_bpd_revisiontype_" + rate_chkIds[i]);
            var revisiontype = ddlrevisiontype.options[ddlrevisiontype.selectedIndex].value;
            var price = document.getElementById("rate_bpd_price_" + rate_chkIds[i]).value;
            var effectivedate = document.getElementById("rate_bpd_price_effectivedate" + rate_chkIds[i]).value;
            var reminderdate = document.getElementById("rate_bpd_price_date" + rate_chkIds[i]).value;
            var reminderdate = document.getElementById("rate_bpd_price_date" + rate_chkIds[i]).value;
            var reminderdays = document.getElementById("rate_bpd_price_days" + rate_chkIds[i]).value;
            params = parid + "~" + price + "~" + effectivedate + "~" + revisiontype + "~" + reminderdate + "~" + reminderdays;
            AllParamsters = params + "|" + AllParamsters;
        }
    }
    var ProjectId_par = document.getElementById("rate_price_project_id").innerHTML;
    if (AllParamsters != "") {
        PageMethods.InsertRateRevisionConfiguration(ProjectId_par, ProcessID, AllParamsters, rate_bpd_prices_OnSuccess, rate_bpd_price_OnError);
        return false;
    }
    return false;
}

function rate_bpd_prices_OnSuccess(result) {
    $("#waitingpanel").modal("hide");
    if (result > 0) {
        document.getElementById("rate_bpd_price_errmsg").innerHTML = "Rate revision is configured successfully. Please click <b>OK</b> to redirect to main page.";
        $('#rate_bpd_price_dverror').modal('show');
        return false;
    }
    else {
        document.getElementById("rate_bpd_price_errmsg").innerHTML = "Oops! System is facing connectivity issue. Please try after some time.";
        $('#rate_bpd_price_dverror').modal('show');
        return false;
    }
    return false;
}
function rate_bpd_price_OnError(error) {
    $("#waitingpanel").modal("hide");
    CanopyUI.showError(error, 'Unable to complete request.');
}

function rate_bpd_price_MessageRedirect() {
    $('#bpd_price_dverror').modal('hide');
    location.href = "ProjectDetails.aspx?ProjectID=" + ProjectID_param;
}