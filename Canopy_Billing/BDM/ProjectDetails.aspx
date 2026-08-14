<%@ Page Title="" Language="C#" MasterPageFile="~/BDM/BDM.Master" AutoEventWireup="true" CodeBehind="ProjectDetails.aspx.cs" Inherits="Vendor_Portal.BDM.ProjectDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
          .loading {
      display: none;
      position: fixed;
      inset: 0;
      margin: 0;
      width: auto;
      height: auto;
      opacity: 1;
      border-radius: 0;
      z-index: 99999;
      background: rgba(15,23,42,.28);
      text-align: center;
      align-items: center;
      justify-content: center;
      padding-top: 280px;
  }
  .loading img { max-width: 62px; }
  .loading div { color: #fff; margin-top: 10px; }

        :root {
            --erp-primary: #0f4c81;
            --erp-primary-dark: #0b355d;
            --erp-accent: #0f766e;
            --erp-bg: #f4f7fb;
            --erp-surface: #fff;
            --erp-border: #d8e2ee;
            --erp-border-strong: #b8c7d8;
            --erp-text: #1f2937;
            --erp-muted: #64748b;
            --erp-shadow: 0 10px 26px rgba(15,23,42,.08);
            --erp-radius: 10px;
        }

        body {
            background: var(--erp-bg);
        }

        .erp-page {
            padding: 5px 18px 0px;
        }

        .erp-hero {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 16px;
            margin-bottom: 16px;
            border-radius: var(--erp-radius);
            padding: 10px 20px;
            color: #fff;
            background: linear-gradient(135deg,var(--erp-primary),var(--erp-accent));
            box-shadow: var(--erp-shadow);
        }

        .erp-kicker {
            display: block;
            margin-bottom: 2px;
            color: rgba(255,255,255,.78);
            font-size: 11px;
            font-weight: 800;
            letter-spacing: .08em;
            text-transform: uppercase;
        }

        .erp-hero h3 {
            margin: 0 0 3px;
            font-size: 21px;
            font-weight: 800;
        }

        .erp-hero p {
            margin: 0;
            color: rgba(255,255,255,.84);
            font-size: 13px;
        }

        .erp-hero-actions {
            display: flex;
            flex-wrap: wrap;
            justify-content: flex-end;
            gap: 8px;
        }

        .erp-icon-btn {
            display: inline-flex;
            align-items: center;
            gap: 7px;
            border: 1px solid rgba(255,255,255,.28);
            border-radius: 8px;
            padding: 8px 13px;
            color: #fff !important;
            background: rgba(255,255,255,.14);
            font-weight: 750;
        }

            .erp-icon-btn:hover {
                color: #fff !important;
                background: rgba(255,255,255,.22);
            }

        .erp-tabs {
            border: 1px solid var(--erp-border) !important;
            border-radius: var(--erp-radius) !important;
            background: #fff !important;
            box-shadow: var(--erp-shadow) !important;
            overflow: hidden;
        }

            .erp-tabs > .card-header {
                border-bottom: 1px solid var(--erp-border) !important;
                background: #fff !important;
            }

            .erp-tabs .card-body {
                padding: 16px !important;
                background: #fff;
            }

        .nav-tabs {
            border-bottom: 0 !important;
        }

            .nav-tabs .nav-link {
                display: inline-flex;
                align-items: center;
                gap: 7px;
                border: 0 !important;
                border-radius: 8px 8px 0 0 !important;
                padding: 11px 16px !important;
                color: var(--erp-muted) !important;
                font-weight: 750 !important;
            }

                .nav-tabs .nav-link.active {
                    border-bottom: 3px solid var(--erp-accent) !important;
                    color: var(--erp-primary) !important;
                    background: #eef6fb !important;
                }

        .erp-panel {
            margin-bottom: 16px;
            border: 1px solid var(--erp-border);
            border-radius: var(--erp-radius);
            background: var(--erp-surface);
            box-shadow: 0 4px 14px rgba(15,23,42,.05);
            overflow: hidden;
        }

        .erp-panel-header {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 12px;
            padding: 13px 16px;
            border-bottom: 1px solid var(--erp-border);
            background: linear-gradient(180deg,#fff,#f8fafc);
        }

            .erp-panel-header h4 {
                display: inline-flex;
                align-items: center;
                gap: 8px;
                margin: 0;
                color: var(--erp-text);
                font-size: 15px;
                font-weight: 800;
            }

        .erp-panel-body {
            padding: 16px;
        }

        .erp-form-grid {
            display: grid;
            grid-template-columns: repeat(3,minmax(220px,1fr));
            gap: 14px 16px;
        }

        .erp-field label {
            display: block;
            margin: 0 0 6px;
            color: #111827;
            font-size: 13px;
            font-weight: 800;
        }

        .erp-field .form-control, .erp-field input, .erp-field select, .erp-field textarea {
            width: 100%;
            min-height: 38px;
            border: 1px solid var(--erp-border-strong) !important;
            border-radius: 7px !important;
            padding: 7px 10px !important;
            color: var(--erp-text) !important;
            background: #fff !important;
            box-shadow: none !important;
        }

        .erp-field textarea {
            min-height: 76px;
        }

            .erp-field .form-control:focus, .erp-field input:focus, .erp-field select:focus, .erp-field textarea:focus {
                border-color: var(--erp-accent) !important;
                box-shadow: 0 0 0 .16rem rgba(15,118,110,.14) !important;
                outline: 0;
            }

        .erp-span-2 {
            grid-column: span 2;
        }

        .erp-span-3 {
            grid-column: span 3;
        }

        .erp-actions {
            display: flex;
            justify-content: flex-end;
            gap: 10px;
            margin-top: 16px;
            padding-top: 14px;
            border-top: 1px solid var(--erp-border);
        }

        .erp-btn-primary {
            display: inline-flex;
            align-items: center;
            gap: 7px;
            border-color: var(--erp-primary) !important;
            border-radius: 7px !important;
            color: #fff !important;
            background: var(--erp-primary) !important;
            font-weight: 750 !important;
        }

            .erp-btn-primary:hover {
                border-color: var(--erp-primary-dark) !important;
                color: #fff !important;
                background: var(--erp-primary-dark) !important;
            }

        .erp-subsection {
            margin-top: 18px;
            border-top: 1px solid var(--erp-border);
            padding-top: 16px;
        }

            .erp-subsection h5 {
                display: inline-flex;
                align-items: center;
                gap: 8px;
                margin: 0 0 14px;
                color: var(--erp-primary);
                font-size: 14px;
                font-weight: 800;
            }

        .erp-table-wrap {
            padding: 14px;
            overflow: auto;
        }

        .table, table.dataTable {
            width: 100% !important;
            color: var(--erp-text);
        }

            .table thead th, table.dataTable thead th {
                border-color: var(--erp-border) !important;
                color: #334155 !important;
                background: #eef3f8 !important;
                font-size: 12px;
                text-transform: uppercase;
            }

        .table-bordered td, .table-bordered th {
            border-color: var(--erp-border) !important;
        }

        .modal-content {
            border: 1px solid var(--erp-border) !important;
            border-radius: var(--erp-radius) !important;
            box-shadow: 0 20px 50px rgba(15,23,42,.18) !important;
        }

        .modal-header {
            border-bottom: 0 !important;
            color: #fff !important;
            background: linear-gradient(135deg,var(--erp-primary),var(--erp-accent)) !important;
        }

            .modal-header .close {
                color: #fff !important;
                opacity: .9 !important;
            }

        .loading {
            z-index: 3000 !important;
        }

        @media(max-width:992px) {
            .erp-form-grid {
                grid-template-columns: repeat(2,minmax(180px,1fr));
            }

            .erp-span-3 {
                grid-column: span 2;
            }
        }

        @media(max-width:576px) {
            .erp-page {
                padding: 12px
            }

            .erp-hero {
                align-items: flex-start;
                flex-direction: column
            }

            .erp-form-grid {
                grid-template-columns: 1fr
            }

            .erp-span-2, .erp-span-3 {
                grid-column: span 1;
            }
        }
    </style>
    <script>
        window.onload = function () {
            document.getElementById('bpd_sales_scopedocument').addEventListener('change', getFileName_scope);
            document.getElementById('bpd_sales_agreementattachment').addEventListener('change', getFileName_agreement);
            document.getElementById('bpd_sales_msaattachment').addEventListener('change', getFileName_sla);

        }
        const getFileName_scope = (event) => {
            const files = event.target.files;
            var file = files[0];
            document.getElementById("sales_scopep").value = files[0].name;

            const fd = new FormData();

            // add all selected files
            fd.append(event.target.name, file, "ScopeDoc_" + file.name);
            // create the request
            const xhr = new XMLHttpRequest();

            xhr.onload = () => {
                if (xhr.status >= 200 && xhr.status < 300) {
                    // we done!
                }
            };
            var url = window.location.href;
            // path to server would be where you'd normally post the form to
            xhr.open('POST', url, true);
            xhr.send(fd);
            //alert(document.getElementById("filep").value);
        }

        const getFileName_agreement = (event) => {
            const files = event.target.files;
            var file = files[0];
            document.getElementById("sales_agreementp").value = files[0].name;

            const fd = new FormData();

            // add all selected files
            fd.append(event.target.name, file, "AgreementDoc_" + file.name);
            // create the request
            const xhr = new XMLHttpRequest();

            xhr.onload = () => {
                if (xhr.status >= 200 && xhr.status < 300) {
                    // we done!
                }
            };
            var url = window.location.href;
            // path to server would be where you'd normally post the form to
            xhr.open('POST', url, true);
            xhr.send(fd);
            //alert(document.getElementById("filep").value);
        }

        const getFileName_sla = (event) => {
            const files = event.target.files;
            var file = files[0];
            document.getElementById("sales_slap").value = files[0].name;

            const fd = new FormData();

            // add all selected files
            fd.append(event.target.name, file, "SLADoc_" + file.name);
            // create the request
            const xhr = new XMLHttpRequest();

            xhr.onload = () => {
                if (xhr.status >= 200 && xhr.status < 300) {
                    // we done!
                }
            };
            var url = window.location.href;
            // path to server would be where you'd normally post the form to
            xhr.open('POST', url, true);
            xhr.send(fd);
            //alert(document.getElementById("filep").value);
        }

        $(document).ready(function () {
            const urlParams = new URLSearchParams(window.location.search);
            const ProjectID = urlParams.get('ProjectID');
            if (ProjectID != null) {
                BindExistingProjectInfo(ProjectID);
                BindExistingConfiguredDeals(ProjectID);
                BindExistingConfiguredDealsForRateRevision(ProjectID);
            }
            bpd_sales_BindBDM();


        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="bdm-page">
<input id="sales_scopep" style="display: none;" />
    <input id="sales_agreementp" style="display: none;" />
    <input id="sales_slap" style="display: none;" />

    <div class="loading" id="load1">
        <img src="../images/Load_1.gif" />
        <div style="font-size: 12px; font-weight: bold;">One moment, please . . . .</div>
    </div>

    <div class="erp-page">
        <div class="erp-hero">
            <div>
                <h3><i class="fas fa-copy"></i>&nbsp;&nbsp;<b id="bpd_header">Edit Project Configuration Details</b></h3>
                <p>Maintain client profile, sales information, deal process, and rate details.</p>
            </div>
            <div class="erp-hero-actions">
                <a href="ProjectDetailsMaster.aspx" class="erp-icon-btn"><i class="fa fa-arrow-left"></i><span>Back to List</span></a>
            </div>
        </div>
        <!-- /.container-fluid -->
    </div>
    <div class="col-lg-12">

        <div class="card">
            <div class="card-body">
                <div class="card card-tabs">
                    <div class="card-header p-0 pt-1">
                        <ul class="nav nav-tabs" id="custom-tabs-one-tab_bpd_tabs" role="tablist">
                            <li class="nav-item">
                                <a class="nav-link active" id="custom-tabs-one-home-tab_company" data-toggle="pill" href="#custom-tabs-one-home_company" role="tab" aria-controls="custom-tabs-one-home_company" aria-selected="true"><i class="fa fa-building"></i><b>Client Information</b></a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" onclick="return bpd_bindSalesInformation();" id="custom-tabs-one-profile-tab_sales" data-toggle="pill" href="#custom-tabs-one-profile_sales" role="tab" aria-controls="custom-tabs-one-profile_sales" aria-selected="false"><i class="fa fa-chart-line"></i><b>Sales Information</b></a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" id="custom-tabs-one-profile-tab_deal" data-toggle="pill" href="#custom-tabs-one-profile_deal" role="tab" aria-controls="custom-tabs-one-profile_deal" aria-selected="false"><i class="fa fa-project-diagram"></i><b>Deal/ Process Information</b></a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" id="custom-tabs-one-profile-tab_rate" data-toggle="pill" href="#custom-tabs-one-profile_rate" role="tab" aria-controls="custom-tabs-one-profile_rate" aria-selected="false"><i class="fa fa-tags"></i><b>Rate Revision Configuration</b></a>
                            </li>

                        </ul>
                    </div>
                    <div class="card-body">
                        <div class="tab-content" id="custom-tabs-one-tabContent_addinvocie">
                            <div class="tab-pane fade show active" id="custom-tabs-one-home_company" role="tabpanel" aria-labelledby="custom-tabs-one-home-tab_company">
                                <table class="table">
                                    <tr>
                                        <td><b>Domain:</b></td>
                                        <td>
                                            <input type="text" class="form-control" style="width: 250px;" disabled="disabled" value="Underwriting" />
                                        </td>
                                        <td><b>Project #:</b></td>
                                        <td>
                                            <input type="text" id="bpd_projectno" name="bpd_projectno" class="form-control" style="width: 250px;" />
                                        </td>
                                        <td><b>Company Name:</b></td>
                                        <td>
                                            <input type="text" id="bpd_companyname" name="bpd_companyname" class="form-control" style="width: 250px;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><b>Contact Person:</b></td>
                                        <td>
                                            <input type="text" id="bpd_contactperson" name="bpd_contactperson" class="form-control" style="width: 250px;" />
                                        </td>
                                        <td><b>Conatct #:</b></td>
                                        <td>
                                            <input type="text" id="bpd_contactno" name="bpd_contactno" class="form-control" style="width: 250px;" />
                                        </td>
                                        <td><b>Email ID:</b></td>
                                        <td>
                                            <input type="email" id="bpd_email" name="bpd_email" class="form-control" style="width: 250px;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><b>Website URL:</b></td>
                                        <td>
                                            <input type="text" id="bpd_weburl" name="bpd_weburl" class="form-control" style="width: 250px;" />
                                        </td>
                                        <td><b>Address:</b></td>
                                        <td>
                                            <textarea id="bpd_address" name="bpd_address" class="form-control" style="width: 250px;"></textarea>
                                        </td>
                                        <td><b>Remark:</b></td>
                                        <td>
                                            <textarea id="bpd_remark" name="bpd_remark" class="form-control" style="width: 250px;"></textarea>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6" style="text-align: center;">
                                            <button id="bpd_btnsubmitstep1" class="btn btn-primary" onclick="return btnsubmitstep1();">Add</button>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="tab-pane fade" id="custom-tabs-one-profile_sales" role="tabpanel" aria-labelledby="custom-tabs-one-profile-tab_sales">
                                <table class="table" style="width: 100%">
                                    <tr>
                                        <td><b>Scope of Project:</b></td>
                                        <td>
                                            <textarea id="bpd_sales_projectscope" name="bpd_sales_projectscope" class="form-control" style="width: 250px;"></textarea>
                                        </td>
                                        <td><b>Scope Document:</b></td>
                                        <td>
                                            <input type="file" id="bpd_sales_scopedocument" name="bpd_sales_scopedocument" class="form-control" style="width: 250px;" />
                                        </td>
                                        <td><b>BDM:</b></td>
                                        <td>
                                            <select id="bpd_sales_bdm" name="bpd_sales_bdm" class="form-control" style="width: 250px;"></select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><b>Project Status:</b></td>
                                        <td>
                                            <select id="bpd_sales_projectstatus" name="bpd_sales_projectstatus" class="form-control" style="width: 250px;">
                                                <option value="">Select</option>
                                                <option value="Live">Live</option>
                                                <option value="Demo">Demo</option>
                                                <option value="Test">Test</option>
                                                <option value="Live Stopped">Live Stopped</option>
                                            </select>
                                        </td>
                                        <td><b>Expected Volume:</b></td>
                                        <td>
                                            <input type="text" id="bpd_sales_expectedvolume" name="bpd_sales_expectedvolume" class="form-control" style="width: 250px;" />
                                        </td>
                                        <td><b>Start Date:</b></td>
                                        <td>
                                            <input type="date" id="bpd_sales_expectedstartdate" name="bpd_sales_expectedstartdate" class="form-control" style="width: 250px;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><b>Duration:</b></td>
                                        <td>
                                            <select id="bpd_sales_projectduration" name="bpd_sales_projectduration" class="form-control" style="width: 250px;">
                                                <option value="">Select</option>
                                                <option value="Short Term">Short Term</option>
                                                <option value="Long Term">Long Term</option>
                                            </select>
                                        </td>
                                        <td><b>Rate Revision Date:</b></td>
                                        <td>
                                            <input type="date" id="bpd_raterevisiondate" name="bpd_raterevisiondate" class="form-control" style="width: 250px;" />
                                        </td>
                                        <td><b>Remark:</b></td>
                                        <td>
                                            <textarea id="bpd_sales_remark" name="bpd_sales_remark" class="form-control" style="width: 250px;"></textarea>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><b>Is NDA Signed?</b></td>
                                        <td>
                                            <select id="bpd_sales_isndasigned" name="bpd_sales_isndasigned" class="form-control" style="width: 250px;" onchange="return getnda(this);">
                                                <option value="">Select</option>
                                                <option value="1">Yes</option>
                                                <option value="0">No</option>
                                                <option value="2">Not Applicable</option>
                                            </select>
                                        </td>
                                    </tr>
                                    <tr id="nda1" style="display: none;">
                                        <td><b>Agreement Date:</b></td>
                                        <td>
                                            <input type="date" id="bpd_sales_agreementdate" name="bpd_sales_agreementdate" class="form-control" style="width: 250px;">
                                        </td>
                                        <td><b>Expiry Date:</b></td>
                                        <td>
                                            <input type="date" id="bpd_sales_agreementexpirydate" name="bpd_sales_agreementexpirydate" class="form-control" style="width: 250px;">
                                        </td>
                                        <td><b>Signed by Client:</b></td>
                                        <td>
                                            <select id="bpd_sales_agreementclientsigned" name="bpd_sales_agreementclientsigned" class="form-control" style="width: 250px;">
                                                <option value="">Select</option>
                                                <option value="1">Yes</option>
                                                <option value="0">No</option>
                                            </select>
                                        </td>
                                    </tr>
                                    <tr id="nda2" style="display: none;">
                                        <td><b>Signed by Infinity:</b></td>
                                        <td>
                                            <select id="bpd_sales_agreementinfinitysigned" name="bpd_sales_agreementinfinitysigned" class="form-control" style="width: 250px;">
                                                <option value="">Select</option>
                                                <option value="1">Yes</option>
                                                <option value="0">No</option>
                                            </select>
                                        </td>
                                        <td><b>Agreement Copy:</b></td>
                                        <td>
                                            <input type="file" id="bpd_sales_agreementattachment" name="bpd_sales_agreementattachment" class="form-control" style="width: 250px;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><b>Is MSA Signed?</b></td>
                                        <td>
                                            <select id="bpd_sales_ismsasigned" name="bpd_sales_ismsasigned" class="form-control" style="width: 250px;" onchange="return getmsa(this);">
                                                <option value="">Select</option>
                                                <option value="1">Yes</option>
                                                <option value="0">No</option>
                                                <option value="2">Not Applicable</option>
                                            </select>
                                        </td>
                                    </tr>
                                    <tr id="msa1" style="display: none;">
                                        <td><b>Agreement Date:</b></td>
                                        <td>
                                            <input type="date" id="bpd_sales_msadate" name="bpd_sales_msadate" class="form-control" style="width: 250px;">
                                        </td>
                                        <td><b>Expiry Date:</b></td>
                                        <td>
                                            <input type="date" id="bpd_sales_msaexpirydate" name="bpd_sales_msaexpirydate" class="form-control" style="width: 250px;">
                                        </td>
                                        <td><b>Signed by Client:</b></td>
                                        <td>
                                            <select id="bpd_sales_msaclientsigned" name="bpd_sales_msaclientsigned" class="form-control" style="width: 250px;">
                                                <option value="">Select</option>
                                                <option value="1">Yes</option>
                                                <option value="0">No</option>
                                            </select>
                                        </td>
                                    </tr>
                                    <tr id="msa2" style="display: none;">
                                        <td><b>Signed by Infinity:</b></td>
                                        <td>
                                            <select id="bpd_sales_msainfinitysigned" name="bpd_sales_msainfinitysigned" class="form-control" style="width: 250px;">
                                                <option value="">Select</option>
                                                <option value="1">Yes</option>
                                                <option value="0">No</option>
                                            </select>
                                        </td>
                                        <td><b>Agreement Copy:</b></td>
                                        <td>
                                            <input type="file" id="bpd_sales_msaattachment" name="bpd_sales_msaattachment" class="form-control" style="width: 250px;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6" style="text-align: center;">
                                            <button id="bpd_sales_btnsubmit" name="bpd_sales_btnsubmit" class="btn btn-primary" onclick="return bpd_sales_submit();">Add</button>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="tab-pane fade" id="custom-tabs-one-profile_deal" role="tabpanel" aria-labelledby="custom-tabs-one-profile_deal">
                                <button id="bpd_deal_btnaddnewprocess" name="bpd_deal_btnaddnewprocess" class="btn btn-primary" onclick="return bpd_deal_addnewprocess();">Add New Deal/ Process</button>
                                <hr />
                                <table class="table" id="bpd_deal_dealtable" style="width: 100%">
                                    <thead>
                                        <tr>
                                            <th>Configure</th>
                                            <th style="display: none;">ProcessID</th>
                                            <th>Sr. #</th>
                                            <th>Deal #/ Process Name</th>
                                            <th>Configured By</th>
                                            <th>Configured On</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div>
                            <div class="tab-pane fade" id="custom-tabs-one-profile_rate" role="tabpanel" aria-labelledby="custom-tabs-one-profile_rate">
                                <table class="table" id="bpd_deal_dealtableRate" style="width: 100%">
                                    <thead>
                                        <tr>
                                            <th>Configure</th>
                                            <th style="display: none;">ProcessID</th>
                                            <th>Sr. #</th>
                                            <th>Deal #/ Process Name</th>
                                            <th>Configured By</th>
                                            <th>Configured On</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="mdl_deal_addnewdeal">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Add New Deal #/ Process Name</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">

                    <table class="table">
                        <tr>
                            <td><b>Deal #/ Process Name:</b></td>
                            <td>
                                <input type="text" id="bpd_deal_dealprocessname" name="bpd_deal_dealprocessname" class="form-control" style="width: 300px;" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <input type="checkbox" onchange="return getcopydeal(this);" style="font-size: 20px;" id="bpd_deal_chkcopy" name="bpd_deal_chkcopy" class="custom-checkbox" />&nbsp;<b> Do you want to copy costing from previous deal?</b>
                            </td>
                        </tr>
                        <tr id="copydeal" style="display: none;">
                            <td><b>Deal #:</b></td>
                            <td>
                                <select id="bpd_deal_existingdeals" name="bpd_deal_existingdeals" class="form-control" style="width: 300px;"></select>
                            </td>
                        </tr>
                    </table>



                </div>
                <div class="modal-footer justify-content-between">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    <button class="btn btn-primary" type="button" id="bpd_deal_btnsubmitnewprocess" onclick="return bpd_deal_submitnewprocess();">ADD</button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    <div class="modal fade" id="bpd_dverror">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <h6 class="modal-title" id="bpd_errmsg"></h6>
                </div>
                <div class="modal-footer align-content-center">
                    <button class="btn btn-primary" type="button" id="bpd_btnMessage" onclick="return bpd_MessageRedirect();">Okay</button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    </div>
</asp:Content>


