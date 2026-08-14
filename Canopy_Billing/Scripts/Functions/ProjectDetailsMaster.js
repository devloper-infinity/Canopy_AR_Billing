var bpm_table;
var bpm_html = '';
var bpd_deal_dealtable;
var bpd_deal_html = '';
var ProjectApprovalID;
var newProjectID;

function blankForNull(s) {
    return s == "null" || s == null ? "" : s;
}

function EditConfiguration(ProjectID, Index) {
    location.href = "ProjectDetails.aspx?ProjectID=" + ProjectID;
}

function bdmprojectmaster_bindgrid() {
    $('#load1').show();

    bpm_html = '';
    $.ajax({
        url: "ProjectDetailsMaster.aspx/GteAllConfiguredProjects",
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//
            $.each(dataArray, function (index, value) {
                bpm_html += '<tr>';
                bpm_html += '<td style="text-wrap: nowrap;text-align:center; display:none">' + blankForNull(value.PAI_ErpProjectID) + '</td>';
                bpm_html += '<td style="text-wrap: nowrap;"><a class="dropdown-item" href="#!" id="ActionsEx" onclick="EditConfiguration(' + value.PAI_ErpProjectID + ',' + index + ');"><span style="color: dodgerblue;"><i class="uil fs-0 me-2 uil-pen"></i></span></a></td>';
                bpm_html += '<td style="text-wrap: nowrap;">' + blankForNull((index + 1)) + '</td>';
                bpm_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.PAI_Project_Name) + '</td>';
                bpm_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.PAI_ProcessName) + '</td>';
                bpm_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.PAI_Company_Name) + '</td>';
                bpm_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.PAI_Contact_Person) + '</td>';
                bpm_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.PAI_Phone_Number) + '</td>';
                bpm_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.PAI_Address) + '</td>';
                bpm_html += '<td style="text-wrap: nowrap; display:none;">' + blankForNull(value.PAI_Remark) + '</td>';
                bpm_html += '</tr>';
            });
            if ($.fn.dataTable.isDataTable('#bpm_table')) {
                bpm_table.destroy();
            }

            $('#bpm_table tbody').html(bpm_html);
            //else
            bpm_table = $('#bpm_table').DataTable({
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


function bpd_sales_Bindexistingdealsforcopy(ProjectID) {
    var select = document.getElementById("bpd_deal_existingdeals");
    let options = select.getElementsByTagName('option');

    for (var i = options.length; i--;) {
        select.removeChild(options[i]);
    }
    $("#bpd_deal_existingdeals").append($("<option></option>").val("").html("Select"));
    $.ajax({
        type: "POST", url: "ProjectDetails.aspx/GetCOnfiguredProcessDeals", dataType: "json", contentType: "application/json",
        data: "{ProjectID:" + ProjectID + "}",
        success: function (res) {
            var dataArray = JSON.parse(res.d);
            $.each(dataArray, function (data, value) {
                $("#bpd_deal_existingdeals").append($("<option></option>").val(value.ProcessId).html(value.ProcessName));
            })
        }
    });
}

function getcopydeal(chk) {
    if (chk.checked == true) {
        document.getElementById("copydeal").style.display = '';
    }
    else {
        document.getElementById("copydeal").style.display = 'none';
    }
}

function bpd_deal_addnewprocess() {
    const urlParams = new URLSearchParams(window.location.search);
    const ProjectID = urlParams.get('ProjectID');
    bpd_sales_Bindexistingdealsforcopy(ProjectID);
    $("#mdl_deal_addnewdeal").modal("show");

    return false;
}

function EditDealprocessPrice(ProcessID, Index) {
    location.href = "PriceConfiguration.aspx?ProcessID=" + ProcessID;
}

function ConfigureRateRevision(ProcessID, Index) {
    location.href = "RateRevisionConfiguration.aspx?ProcessID=" + ProcessID;
}


function BindExistingConfiguredDeals(ProjectID) {
    $('#load1').show();

    bpd_deal_html = '';
    $.ajax({
        url: "ProjectDetails.aspx/GetCOnfiguredProcessDeals",
        type: "POST",
        dataType: "json",
        data: "{ProjectID:" + ProjectID + "}",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//

            $.each(dataArray, function (index, value) {
                var addeddate = eval(value.AddedDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1).toLocaleDateString(\"en-US\")"));
                bpd_deal_html += '<tr>';
                bpd_deal_html += '<td style="text-wrap: nowrap;"><a class="dropdown-item" href="#!" id="ActionsEx" onclick="EditDealprocessPrice(' + value.ProcessId + ',' + index + ');"><span style="color: dodgerblue;"><i class="uil fs-0 me-2 uil-pen"></i></span></a></td>';
                bpd_deal_html += '<td style="text-wrap: nowrap;display:none;">' + blankForNull(value.ProcessId) + '</td>';
                bpd_deal_html += '<td style="text-wrap: nowrap;">' + blankForNull((index + 1)) + '</td>';
                bpd_deal_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.ProcessName) + '</td>';
                bpd_deal_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.AddedByName) + '</td>';
                bpd_deal_html += '<td style="text-wrap: nowrap;">' + blankForNull(addeddate) + '</td>';
                bpd_deal_html += '</tr>';
            });
            if ($.fn.dataTable.isDataTable('#bpd_deal_dealtable')) {
                bpd_deal_dealtable.destroy();
            }

            $('#bpd_deal_dealtable tbody').html(bpd_deal_html);
            //else
            bpd_deal_dealtable = $('#bpd_deal_dealtable').DataTable({
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
                    jQuery('.dataTable').wrap('<div class="dataTables_scroll" />');
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

function BindExistingConfiguredDealsForRateRevision(ProjectID) {
    $('#load1').show();

    var rate_bpd_deal_html = '';
    $.ajax({
        url: "ProjectDetails.aspx/GetCOnfiguredProcessDeals",
        type: "POST",
        dataType: "json",
        data: "{ProjectID:" + ProjectID + "}",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//

            $.each(dataArray, function (index, value) {
                var addeddate = eval(value.AddedDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1).toLocaleDateString(\"en-US\")"));
                rate_bpd_deal_html += '<tr>';
                rate_bpd_deal_html += '<td style="text-wrap: nowrap;"><a class="dropdown-item" href="#!" id="ActionsEx" onclick="ConfigureRateRevision(' + value.ProcessId + ',' + index + ');"><span style="color: dodgerblue;"><i class="uil fs-0 me-2 uil-pen"></i></span></a></td>';
                rate_bpd_deal_html += '<td style="text-wrap: nowrap;display:none;">' + blankForNull(value.ProcessId) + '</td>';
                rate_bpd_deal_html += '<td style="text-wrap: nowrap;">' + blankForNull((index + 1)) + '</td>';
                rate_bpd_deal_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.ProcessName) + '</td>';
                rate_bpd_deal_html += '<td style="text-wrap: nowrap;">' + blankForNull(value.AddedByName) + '</td>';
                rate_bpd_deal_html += '<td style="text-wrap: nowrap;">' + blankForNull(addeddate) + '</td>';
                rate_bpd_deal_html += '</tr>';
            });
            if ($.fn.dataTable.isDataTable('#bpd_deal_dealtableRate')) {
                bpd_deal_dealtable.destroy();
            }

            $('#bpd_deal_dealtableRate tbody').html(rate_bpd_deal_html);
            //else
            bpd_deal_dealtable = $('#bpd_deal_dealtableRate').DataTable({
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
                    jQuery('.dataTable').wrap('<div class="dataTables_scroll" />');
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

function BindExistingProjectInfo(ProjectID) {
    $.ajax({
        url: "ProjectDetails.aspx/GetProjectInformation",
        type: "POST",
        dataType: "json",
        data: "{ProjectID:" + ProjectID + "}",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dataArray = JSON.parse(data.d);//
            $.each(dataArray, function (index, value) {
                document.getElementById("bpd_header").innerHTML = "Edit Project Configuration - " + blankForNull(value.PAI_Project_Name) + " :: " + blankForNull(value.PAI_Company_Name);
                document.getElementById("bpd_projectno").value = blankForNull(value.PAI_Project_Name);
                document.getElementById("bpd_companyname").value = blankForNull(value.PAI_Company_Name);
                document.getElementById("bpd_contactperson").value = blankForNull(value.PAI_Contact_Person);
                document.getElementById("bpd_contactno").value = blankForNull(value.PAI_Phone_Number);
                document.getElementById("bpd_email").value = blankForNull(value.PAI_Email_Id);
                document.getElementById("bpd_weburl").value = blankForNull(value.PAI_Url);
                document.getElementById("bpd_address").value = blankForNull(value.PAI_Address);
                document.getElementById("bpd_remark").value = blankForNull(value.PAI_Remark);
                ProjectApprovalID = blankForNull(value.PAI_Id);
                document.getElementById("bpd_btnsubmitstep1").innerHTML = "Update";
            });
        },
        error: function (error) {
            CanopyUI.showError(error, 'Unable to complete request.');
        }
    });


    return false;
}

function btnsubmitstep1() {
    const urlParams = new URLSearchParams(window.location.search);
    const ProjectID = urlParams.get('ProjectID');
    if (ProjectID != null) {
        var projectname = document.getElementById("bpd_projectno").value;
        var companyname = document.getElementById("bpd_companyname").value;
        var contactperson = document.getElementById("bpd_contactperson").value;
        var phone = document.getElementById("bpd_contactno").value;
        var email = document.getElementById("bpd_email").value;
        var weburl = document.getElementById("bpd_weburl").value;
        var address = document.getElementById("bpd_address").value;
        var remark = document.getElementById("bpd_remark").value;

        PageMethods.UpdateProject(ProjectApprovalID, projectname, companyname, contactperson, phone, email, weburl, address, remark, ProjectID, bpd_step1_OnSuccess, bpd_step1_OnError);
    }
    else {
        var projectname = document.getElementById("bpd_projectno").value;
        var companyname = document.getElementById("bpd_companyname").value;
        var contactperson = document.getElementById("bpd_contactperson").value;
        var phone = document.getElementById("bpd_contactno").value;
        var email = document.getElementById("bpd_email").value;
        var weburl = document.getElementById("bpd_weburl").value;
        var address = document.getElementById("bpd_address").value;
        var remark = document.getElementById("bpd_remark").value;
        PageMethods.InsertProject(projectname, companyname, contactperson, phone, email, weburl, address, remark, bpd_step1_OnSuccess, bpd_step1_OnError);

    }
    return false;
}

function bpd_step1_OnSuccess(result) {
    if (result > 0) {
        newProjectID = result;
        document.getElementById("bpd_errmsg").innerHTML = "Project details updated successfully.";
        $('#bpd_dverror').modal('show');
        return false;
    }
    else {
        document.getElementById("bpd_errmsg").innerHTML = "Oops! System is facing connectivity issue. Please try after some time.";
        $('#bpd_dverror').modal('show');
        return false;
    }
    return false;
}
function bpd_step1_OnError(error) {
    CanopyUI.showError(error, 'Unable to complete request.');
}

function bpd_deal_submitnewprocess() {
    const urlParams = new URLSearchParams(window.location.search);
    const ProjectID = urlParams.get('ProjectID');
    var ProcessName = document.getElementById("bpd_deal_dealprocessname").value;
    var chkcopy = document.getElementById("bpd_deal_chkcopy");
    var fromdealno = "";
    if (chkcopy.checked) {
        var ddlFromDeal = document.getElementById("bpd_deal_existingdeals");
        fromdealno = ddlFromDeal.options[ddlFromDeal.selectedIndex].text;
    }
    PageMethods.AddNewProcessDeal(ProcessName, ProjectID, chkcopy.checked, fromdealno, bpd_addProcess_OnSuccess, bpd_addProcess_OnError);
    return false;
}

function bpd_addProcess_OnSuccess(result) {
    if (result > 0) {
        $('#mdl_deal_addnewdeal').modal('hide');
        document.getElementById("bpd_errmsg").innerHTML = "Process/ Deal is added successfully. Please click <b>OK</b> to add costing.";
        $('#bpd_dverror').modal('show');
        return false;
    }
    else {
        $('#mdl_deal_addnewdeal').modal('hide');
        document.getElementById("bpd_errmsg").innerHTML = "Oops! System is facing connectivity issue. Please try after some time.";
        $('#bpd_dverror').modal('show');
        return false;
    }
    return false;
}
function bpd_addProcess_OnError(error) {
    CanopyUI.showError(error, 'Unable to complete request.');
}

function bpd_MessageRedirect() {
    $('#bpd_dverror').modal('hide');
    const urlParams = new URLSearchParams(window.location.search);
    const ProjectID = urlParams.get('ProjectID');
    if (ProjectID != null)
        BindExistingConfiguredDeals(ProjectID);
    else
        location.href = "ProjectDetails.aspx?ProjectID=" + newProjectID;
}

//////////////// Sales
function bpd_sales_BindBDM() {
    var select = document.getElementById("bpd_sales_bdm");
    let options = select.getElementsByTagName('option');

    for (var i = options.length; i--;) {
        select.removeChild(options[i]);
    }
    $("#bpd_sales_bdm").append($("<option></option>").val("").html("Select"));
    $.ajax({
        type: "POST", url: "ProjectDetails.aspx/GetAllSalesBDM", dataType: "json", contentType: "application/json",
        success: function (res) {
            var dataArray = JSON.parse(res.d);
            $.each(dataArray, function (data, value) {
                $("#bpd_sales_bdm").append($("<option></option>").val(value.EmployeeID).html(value.EmpName));
            })
        }
    });
}


function getnda(ddlnda) {
    var ddlvalue = ddlnda.options[ddlnda.selectedIndex].value;
    if (ddlvalue == 1) {
        document.getElementById("nda1").style.display = '';
        document.getElementById("nda2").style.display = '';
    }
    else {
        document.getElementById("nda1").style.display = 'none';
        document.getElementById("nda2").style.display = 'none';
    }
}
function getmsa(ddlmsa) {
    var ddlvalue = ddlmsa.options[ddlmsa.selectedIndex].value;
    if (ddlvalue == 1) {
        document.getElementById("msa1").style.display = '';
        document.getElementById("msa2").style.display = '';
    }
    else {
        document.getElementById("msa1").style.display = 'none';
        document.getElementById("msa2").style.display = 'none';
    }
}

function bpd_bindSalesInformation() {
    const urlParams = new URLSearchParams(window.location.search);
    const ProjectID = urlParams.get('ProjectID');
    if (ProjectID != null) {
        $('#load1').show();

        bpd_deal_html = '';
        $.ajax({
            url: "ProjectDetails.aspx/GetSalesInformation",
            type: "POST",
            dataType: "json",
            data: "{ProjectID:" + ProjectID + "}",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                var dataArray = JSON.parse(data.d);//
                if (dataArray != "" || dataArray != undefined || dataArray != null) {
                    $.each(dataArray, function (index, value) {
                        document.getElementById("bpd_sales_projectscope").value = blankForNull(value.IBS_ScopeOfProject);
                        var select = document.getElementById("bpd_sales_bdm");
                        let options = select.getElementsByTagName('option');

                        for (var i = options.length; i--;) {
                            select.removeChild(options[i]);
                        }
                        $("#bpd_sales_bdm").append($("<option></option>").val("").html("Select"));
                        $.ajax({
                            type: "POST", url: "ProjectDetails.aspx/GetAllSalesBDM", dataType: "json", contentType: "application/json",
                            success: function (res) {
                                var dataArray = JSON.parse(res.d);
                                $.each(dataArray, function (data, value) {
                                    $("#bpd_sales_bdm").append($("<option></option>").val(value.EmployeeID).html(value.EmpName));
                                })
                                $("#bpd_sales_bdm").val(blankForNull(value.IBS_BDM));
                            }
                        });

                        $("#bpd_sales_bdm").val(blankForNull(value.IBS_ProjectStatus));
                        document.getElementById("bpd_sales_expectedvolume").value = blankForNull(value.IBS_ExpectedVolume);
                        if (blankForNull(value.IBS_ExpectedStartDate1) != "") {
                            var date = new Date(value.IBS_ExpectedStartDate1);
                            var day = date.getDate();
                            if (day < 10)
                                day = '0' + day
                            var month = date.getMonth() + 1;
                            if (month < 10)
                                month = '0' + month
                            var year = date.getFullYear();
                            var actualdate = year + "-" + (month) + "-" + (day);
                            $("#bpd_sales_expectedstartdate").val(actualdate);
                        }
                        if (blankForNull(value.RateRevisionDate) != "") {
                            var date = new Date(value.RateRevisionDate);
                            var day = date.getDate();
                            if (day < 10)
                                day = '0' + day
                            var month = date.getMonth() + 1;
                            if (month < 10)
                                month = '0' + month
                            var year = date.getFullYear();
                            var actualdate = year + "-" + (month) + "-" + (day);
                            $("#bpd_raterevisiondate").val(actualdate);
                        }
                        $("#bpd_sales_projectstatus").val(blankForNull(value.IBS_ProjectStatus));
                        $("#bpd_sales_projectduration").val(blankForNull(value.IBS_ProjectDuration));
                        if (blankForNull(value.IBS_NDASigned) == "False")
                            $("#bpd_sales_isndasigned").val("0");
                        else
                            $("#bpd_sales_isndasigned").val(blankForNull(value.IBS_NDASigned));
                        if (blankForNull(value.IBS_NDASigned) == "1") {
                            document.getElementById("nda1").style.display = '';
                            document.getElementById("nda2").style.display = '';

                            if (blankForNull(value.IBS_DateOfNDAAgreement1) != "") {
                                var date = new Date(value.IBS_DateOfNDAAgreement1);
                                var day = date.getDate();
                                if (day < 10)
                                    day = '0' + day
                                var month = date.getMonth() + 1;
                                if (month < 10)
                                    month = '0' + month
                                var year = date.getFullYear();
                                var actualdate = year + "-" + (month) + "-" + (day);
                                $("#bpd_sales_agreementdate").val(actualdate);
                            }
                            if (blankForNull(value.IBS_ExpirationDateofNDAAgreement1) != "") {
                                var date = new Date(value.IBS_ExpirationDateofNDAAgreement1);
                                var day = date.getDate();
                                if (day < 10)
                                    day = '0' + day
                                var month = date.getMonth() + 1;
                                if (month < 10)
                                    month = '0' + month
                                var year = date.getFullYear();
                                var actualdate = year + "-" + (month) + "-" + (day);
                                $("#bpd_sales_agreementexpirydate").val(actualdate);
                            }
                            if (blankForNull(value.IBS_NDASignedByClient) == "true")
                                $("#bpd_sales_agreementclientsigned").val("1");
                            else
                                $("#bpd_sales_agreementclientsigned").val("0");
                            if (blankForNull(value.IBS_NDASignedBYInfinity) == "true")
                                $("#bpd_sales_agreementinfinitysigned").val("1");
                            else
                                $("#bpd_sales_agreementinfinitysigned").val("0");

                        }
                        else {
                            document.getElementById("nda1").style.display = 'none';
                            document.getElementById("nda2").style.display = 'none';
                        }


                        if (blankForNull(value.IBS_SLASigned) == "False")
                            $("#bpd_sales_ismsasigned").val("0");
                        else
                            $("#bpd_sales_ismsasigned").val(blankForNull(value.IBS_SLASigned));
                        if (blankForNull(value.IBS_SLASigned) == "1") {
                            document.getElementById("msa1").style.display = '';
                            document.getElementById("msa2").style.display = '';
                            if (blankForNull(value.IBS_DateOfSLAAgreement1) != "") {
                                var date = new Date(value.IBS_DateOfSLAAgreement1);
                                var day = date.getDate();
                                if (day < 10)
                                    day = '0' + day
                                var month = date.getMonth() + 1;
                                if (month < 10)
                                    month = '0' + month
                                var year = date.getFullYear();
                                var actualdate = year + "-" + (month) + "-" + (day);
                                $("#bpd_sales_msadate").val(actualdate);
                            }
                            if (blankForNull(value.IBS_ExpirationDateofSLAAgreement1) != "") {
                                var date = new Date(value.IBS_ExpirationDateofSLAAgreement1);
                                var day = date.getDate();
                                if (day < 10)
                                    day = '0' + day
                                var month = date.getMonth() + 1;
                                if (month < 10)
                                    month = '0' + month
                                var year = date.getFullYear();
                                var actualdate = year + "-" + (month) + "-" + (day);
                                $("#bpd_sales_msaexpirydate").val(actualdate);
                            }
                            if (blankForNull(value.IBS_SLASignedByClient) == "true")
                                $("#bpd_sales_msaclientsigned").val("1");
                            else
                                $("#bpd_sales_msaclientsigned").val("0");
                            if (blankForNull(value.IBS_SLASignedByInfinity) == "true")
                                $("#bpd_sales_msainfinitysigned").val("1");
                            else
                                $("#bpd_sales_msainfinitysigned").val("0");
                        }
                        else {
                            document.getElementById("msa1").style.display = 'none';
                            document.getElementById("msa2").style.display = 'none';
                        }
                        document.getElementById("bpd_sales_remark").value = blankForNull(value.IBS_Remark);
                    });
                }

            },
            error: function (error) {
                CanopyUI.showError(error, 'Unable to complete request.');
            }
        });
        $('#load1').hide();
    }
    return false;
}

function bpd_sales_submit() {
    const urlParams = new URLSearchParams(window.location.search);
    const ProjectID = urlParams.get('ProjectID');
    var projectname = document.getElementById("bpd_projectno").value;
    var ddlbdm = document.getElementById("bpd_sales_bdm");
    var bdm = ddlbdm.options[ddlbdm.selectedIndex].value;
    var scope = document.getElementById("bpd_sales_projectscope").value;
    var expvolume = document.getElementById("bpd_sales_expectedvolume").value;
    var expstartdate = document.getElementById("bpd_sales_expectedstartdate").value;
    var raterevisiondate = document.getElementById("bpd_raterevisiondate").value;
    var ddlnda = document.getElementById("bpd_sales_isndasigned");
    var nda = ddlnda.options[ddlnda.selectedIndex].value;
    var ddndaclient = document.getElementById("bpd_sales_agreementclientsigned");
    var ddndainfinity = document.getElementById("bpd_sales_agreementinfinitysigned");
    var ndadate = '';
    var ndaexpdate = ''
    var ndaclient = '';
    var ndainfinity = '';
    if (nda == "1") {
        ndadate = document.getElementById("bpd_sales_agreementdate").value;
        ndaexpdate = document.getElementById("bpd_sales_agreementexpirydate").value;
        ndaclient = ddndaclient.options[ddlnda.selectedIndex].value;
        ndainfinity = ddndainfinity.options[ddndainfinity.selectedIndex].value;
    }
    var ddlsla = document.getElementById("bpd_sales_ismsasigned");
    var sla = ddlsla.options[ddlsla.selectedIndex].value;
    var ddslaclient = document.getElementById("bpd_sales_msaclientsigned");
    var ddslainfinity = document.getElementById("bpd_sales_msainfinitysigned");
    var sladate = '';
    var slaexpdate = ''
    var slaclient = '';
    var slainfinity = '';
    if (sla == "1") {
        sladate = document.getElementById("bpd_sales_msadate").value;
        slaexpdate = document.getElementById("bpd_sales_msaexpirydate").value;
        slaclient = ddslaclient.options[ddslaclient.selectedIndex].value;
        slainfinity = ddslainfinity.options[ddslainfinity.selectedIndex].value;
    }
    var ddlprjstatus = document.getElementById("bpd_sales_projectstatus");
    var prjstatus = ddlprjstatus.options[ddlprjstatus.selectedIndex].value;
    var ddlprjduration = document.getElementById("bpd_sales_projectduration");
    var prjduration = ddlprjduration.options[ddlprjduration.selectedIndex].value;
    var remark = document.getElementById("bpd_sales_remark").value;
    PageMethods.AddSalesInformation(ProjectID, projectname, bdm, scope, expvolume, expstartdate, nda, ndadate, ndaexpdate, ndaclient, ndainfinity, sla, sladate, slaexpdate, slaclient, slainfinity, prjstatus, prjduration, remark, raterevisiondate, bpd_sales_OnSuccess, bpd_sales_OnError);
    return false;
}

function bpd_sales_OnSuccess(result) {
    if (result > 0) {
        alert("Sales information updated successfully.");
        bpd_bindSalesInformation();
        return false;
    }
    else {
        alert("Oops! System is facing connectivity issue. Please try after some time.");
        return false;
    }
    return false;
}
function bpd_sales_OnError(error) {
    CanopyUI.showError(error, 'Unable to complete request.');
}