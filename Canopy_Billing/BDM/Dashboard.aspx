<%@ Page Title="" Language="C#" MasterPageFile="~/BDM/BDM.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Vendor_Portal.BDM.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <style>
     :root {
         --erp-primary: #123f6d;
         --erp-primary-dark: #0b2f53;
         --erp-accent: #0f766e;
         --erp-highlight: #2dd4bf;
         --erp-border: #e5e7eb;
         --erp-muted: #6b7280;
         --erp-soft: #f3f7fb;
         --erp-text: #111827;
         --erp-shadow: 0 12px 32px rgba(15, 23, 42, .08);
     }

     .erp-dashboard-page {
         padding: 14px 12px 28px;
     }

     .erp-hero {
         display: flex;
         align-items: center;
         justify-content: space-between;
         gap: 16px;
         padding: 18px 22px;
         margin-bottom: 14px;
         border: 1px solid var(--erp-border);
         border-radius: 14px;
         background: linear-gradient(135deg, #ffffff 0%, #f6fbff 52%, #e8f1fb 100%);
         box-shadow: var(--erp-shadow);
         position: relative;
         overflow: hidden;
     }

     .erp-hero:before {
         content: "";
         position: absolute;
         inset: 0 0 auto 0;
         height: 4px;
         background: linear-gradient(90deg, var(--erp-primary), var(--erp-highlight), var(--erp-accent));
     }

     .erp-hero h1 {
         margin: 0;
         color: var(--erp-text);
         font-size: 21px;
         font-weight: 700;
         letter-spacing: -.01em;
     }

     .erp-hero p {
         margin: 4px 0 0;
         color: var(--erp-muted);
         font-size: 13px;
     }

     .erp-hero-badge {
         display: inline-flex;
         align-items: center;
         gap: 7px;
         padding: 8px 12px;
         border-radius: 999px;
         background: #e8f1fb;
         color: var(--erp-primary-dark);
         border: 1px solid #cfe1f5;
         font-size: 12px;
         font-weight: 700;
         white-space: nowrap;
     }

     .erp-panel {
         border: 1px solid var(--erp-border);
         border-radius: 14px;
         background: #fff;
         box-shadow: var(--erp-shadow);
         overflow: hidden;
     }

     .erp-panel-header {
         display: flex;
         align-items: center;
         justify-content: space-between;
         gap: 12px;
         padding: 14px 16px;
         border-bottom: 1px solid var(--erp-border);
         background: linear-gradient(180deg, #ffffff, #f8fbfd);
     }

     .erp-panel-title {
         margin: 0;
         color: var(--erp-text);
         font-size: 15px;
         font-weight: 700;
     }

     .erp-panel-subtitle {
         margin: 2px 0 0;
         color: var(--erp-muted);
         font-size: 12px;
     }

     .erp-tabs {
         display: flex;
         flex-wrap: wrap;
         gap: 8px;
         padding: 12px 16px 0;
         border-bottom: 1px solid var(--erp-border);
         background: linear-gradient(180deg, #f6f9fc, #edf4fb);
     }

     .erp-tabs .nav-item {
         margin-bottom: -1px;
     }

     .erp-tabs .nav-link {
         border: 1px solid transparent !important;
         border-radius: 10px 10px 0 0 !important;
         color: #374151;
         font-size: 13px;
         font-weight: 700;
         padding: 10px 14px;
         background: transparent;
     }

     .erp-tabs .nav-link:hover {
         color: var(--erp-primary-dark);
         background: #fff;
         border-color: #cfe1f5 !important;
     }

     .erp-tabs .nav-link.active {
         color: var(--erp-primary-dark) !important;
         background: #fff !important;
         border-color: var(--erp-border) var(--erp-border) #fff !important;
     }

     .erp-tab-body {
         padding: 16px;
     }

     .erp-toolbar {
         display: flex;
         align-items: center;
         justify-content: space-between;
         gap: 12px;
         margin-bottom: 12px;
     }

     .erp-table-wrap {
         width: 100%;
         overflow: auto;
         border: 1px solid var(--erp-border);
         border-radius: 12px;
         background: #fff;
     }

     .erp-table-wrap .table {
         margin-bottom: 0 !important;
     }

     .table.dataTable {
         border-collapse: separate !important;
         border-spacing: 0;
         width: 100% !important;
     }

     .table.dataTable th {
         background: #edf4fb !important;
         color: #111827 !important;
         border-bottom: 1px solid var(--erp-border) !important;
         font-size: 12px;
         font-weight: 700;
         white-space: nowrap;
         vertical-align: middle;
     }

     .table.dataTable tr td {
         background: #fff !important;
         color: #1f2937;
         font-size: 12px;
         vertical-align: middle;
     }

     .dataTables_wrapper {
         padding: 0;
     }

     .dataTables_length, .dataTables_info {
         float: left !important;
         color: var(--erp-muted);
         font-size: 12px;
     }

     .dataTables_filter {
         color: var(--erp-muted);
         font-size: 12px;
     }

     div.dt-buttons {
         position: static;
         float: left;
         padding-left: 14px;
     }

     .buttons-excel, .buttons-html5,
     .btn-primary {
         color: #fff !important;
         background: linear-gradient(135deg, var(--erp-primary-dark), var(--erp-primary)) !important;
         border-color: var(--erp-primary) !important;
         box-shadow: none !important;
         font-weight: 700;
         border-radius: 8px;
         padding: 7px 13px;
     }

     .buttons-excel:hover, .buttons-html5:hover,
     .btn-primary:hover {
         background: var(--erp-primary-dark) !important;
         border-color: var(--erp-primary-dark) !important;
     }

     .btn-default {
         border: 1px solid var(--erp-border) !important;
         background: #fff !important;
         color: #374151 !important;
         border-radius: 8px;
         font-weight: 600;
     }

     label:not(.form-check-label):not(.custom-file-label) {
         font-weight: 600 !important;
         border: none !important;
         color: #374151;
     }

     .dt-hidden-col {
         color: transparent !important;
         background: transparent !important;
         border: none !important;
         padding: 0 !important;
     }

     .dtfc-fixed-left {
         left: 0 !important;
         box-shadow: 6px 0 12px rgba(15, 23, 42, .05);
     }

     .loading {
         display: none;
         position: fixed;
         inset: 0;
         width: auto;
         height: auto;
         margin: 0;
         background: rgba(255, 255, 255, .72);
         backdrop-filter: blur(2px);
         z-index: 99999;
         text-align: center;
         padding-top: 24vh;
     }

     .loading img {
         width: 68px;
         height: 68px;
     }

     .loading div {
         margin-top: 10px;
         color: #374151;
         font-size: 13px !important;
         font-weight: 700 !important;
     }

     .modal-content {
         border: 0;
         border-radius: 14px;
         box-shadow: var(--erp-shadow);
     }

     .modal-header {
         border-bottom: 1px solid var(--erp-border);
         background: var(--erp-soft);
         border-radius: 14px 14px 0 0;
     }

     .modal-title {
         font-size: 16px;
         font-weight: 700;
         color: var(--erp-text);
     }

     .modal-footer {
         border-top: 1px solid var(--erp-border);
     }

     .erp-form-grid {
         width: 100%;
         border-collapse: separate;
         border-spacing: 0 12px;
     }

     .erp-form-grid td {
         vertical-align: top;
         padding: 0 10px 0 0;
     }

     .erp-form-grid td:nth-child(odd) {
         width: 110px;
         padding-top: 8px;
         color: #374151;
         font-weight: 700;
     }

     .erp-form-grid .form-control {
         width: 100% !important;
         min-height: 38px;
         border-radius: 8px;
         border: 1px solid #d1d5db;
         font-size: 13px;
     }

     @media (max-width: 768px) {
         .erp-hero, .erp-panel-header, .erp-toolbar {
             align-items: flex-start;
             flex-direction: column;
         }

         .erp-tabs .nav-link {
             padding: 9px 10px;
         }
     }
 </style>
 <script>
     window.onload = function () {
         document.getElementById('dashboard_attachment_upload').addEventListener('change', getFileName);
         var currentUserName = '<%= HttpContext.Current.User.Identity.Name.ToString() %>';

     }
     const getFileName = (event) => {
         const files = event.target.files;
         var file = files[0];
         const fd = new FormData();

         // add all selected files
         fd.append(event.target.name, file, file.name);
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
     }
     function getUserAccess() {
         $.ajax({
             url: "Dashboard.aspx/GetUserAccesInfo",
             type: "POST",
             dataType: "json",
             contentType: "application/json; charset=utf-8",
             success: function (data) {
                 dataArray = JSON.parse(data.d);
                 $.each(dataArray, function (index, item) {
                     if (item["Company"] == "6") {
                         document.getElementById("quickbookcomparison").style.display = "none";
                         document.getElementById("quickbookentry").style.display = "none";
                         document.getElementById("mainsummary").style.display = "none";
                         document.getElementById("quickbookcomparisonRL").style.display = "none";

                     }
                     else {
                         document.getElementById("quickbookcomparison").style.display = "";
                         document.getElementById("quickbookentry").style.display = "";
                         document.getElementById("mainsummary").style.display = "";
                         document.getElementById("quickbookcomparisonRL").style.display = "";
                     }
                 });
             }
         });
     }
     $(document).ready(function () {



         //dashboard_arBind();
         getUserAccess();
         //dashboard_arBind_DifferenceQuickbook_BillNoRL();
         dashboard_arBind_displayERP();
         //dashboard_arBind_DifferenceQuickbook_BillNo();

         //bindNewDashboard();
         //$('#dashboard_ar').DataTable();

     });
 </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="bdm-page">
        <div class="page-header" style="display:none;">
            <div class="header-content">
                <div class="header-left">
                    <div class="header-icon"><i class="fas fa-chart-line"></i></div>
                    <div>
                        <h1>Main Dashboard</h1>
                        <p>Review client reconciliation, ERP summary, QuickBook comparison and monthly billing updates.</p>
                    </div>
                </div>
                <div class="header-right">
                    <span class="module-badge">Dashboard</span>
                </div>
            </div>
        </div>
<div class="loading" id="load1">
    <div class="loading__box">
        <img src="../images/Load_1.gif" alt="Loading" />
        <div class="loading__text">One moment, please...</div>
    </div>
</div> <%--<div class="content-header">
     <div class="container">
         <div class="row mb-2 callout callout-info">
             <div class="col-sm-6">
                 <h6 class="m-0"><i class="fas fa-copy"></i>&nbsp;&nbsp;<b>Main Dashboard</b></h6>
             </div>
         </div>
     </div>
 </div>--%>
 <div class="erp-dashboard-page" id="dvmain" runat="server">
     <div class="erp-hero" style="display:none;">
         <div>
             <h1><i class="fas fa-chart-line"></i>&nbsp; Main Dashboard</h1>
             <p>Review client reconciliation, ERP summary, QuickBook comparison, and monthly billing updates.</p>
         </div>
         <span class="erp-hero-badge"><i class="fas fa-sync-alt"></i> Live dashboard</span>
     </div>

     <div class="erp-panel">
         <div class="erp-panel-header">
             <div>
                 <h2 class="erp-panel-title">Dashboard Workspace</h2>
                 <p class="erp-panel-subtitle">Use the tabs below to review summary, compare QuickBook data, and update billing entries.</p>
             </div>
         </div>

         <ul class="nav nav-tabs erp-tabs" id="custom-tabs-one-tab_bpd_tabs" role="tablist">
             <li class="nav-item" id="mainsummary" style="display: none;">
                 <a class="nav-link active" id="custom-tabs-one-home-tab_company" data-toggle="pill" href="#custom-tabs-one-home_company" role="tab" aria-controls="custom-tabs-one-home_company" aria-selected="true">Summary</a>
             </li>
             <li class="nav-item" id="quickbookcomparison" style="display: none;">
                 <a class="nav-link" onclick="return dashboard_arBind_DifferenceQuickbook_BillNo();" id="custom-tabs-one-profile-tab_sales" data-toggle="pill" href="#custom-tabs-one-profile_sales" role="tab" aria-controls="custom-tabs-one-profile_sales" aria-selected="false">QuickBook Review</a>
             </li>
             <li class="nav-item" id="quickbookcomparisonRL" style="display: none;">
                 <a class="nav-link" onclick="return  dashboard_arBind_DifferenceQuickbook_BillNoRL();" id="custom-tabs-one-profile-tab_salesRL" data-toggle="pill" href="#custom-tabs-one-profile_salesRL" role="tab" aria-controls="custom-tabs-one-profile_salesRL" aria-selected="false">QuickBook RL</a>
             </li>
             <li class="nav-item" id="quickbookentry" style="display: none;">
                 <a class="nav-link" onclick="return dashboard_arBind();" id="custom-tabs-one-profile-tab_deal" data-toggle="pill" href="#custom-tabs-one-profile_deal" role="tab" aria-controls="custom-tabs-one-profile_deal" aria-selected="false">Update QuickBook Data</a>
             </li>
         </ul>

         <div class="tab-content erp-tab-body" id="custom-tabs-one-tabContent_addinvocie">
             <div class="tab-pane fade show active" id="custom-tabs-one-home_company" role="tabpanel" aria-labelledby="custom-tabs-one-home-tab_company">
                 <div class="erp-table-wrap">
                     <table class="table table-bordered" id="dashboard_ar" style="width: 100%;"></table>
                 </div>
             </div>
             <div class="tab-pane fade" id="custom-tabs-one-profile_sales" role="tabpanel" aria-labelledby="custom-tabs-one-profile-tab_sales">
                 <div class="erp-toolbar">
                     <label id="newlabl"></label>
                     <button id="dashboard_btnrefreshgrid" name="dashboard_btnrefreshgrid" class="btn btn-primary" onclick="return dashboard_arBind_DifferenceQuickbook_BillNo();"><i class="fas fa-redo-alt"></i>&nbsp; Refresh Data</button>
                 </div>
                 <div class="erp-table-wrap">
                     <table class="table table-bordered" id="dashboard_ardifference" style="width: 100%;"></table>
                 </div>
             </div>
             <div class="tab-pane fade" id="custom-tabs-one-profile_salesRL" role="tabpanel" aria-labelledby="custom-tabs-one-profile-tab_salesRL">
                 <div class="erp-toolbar">
                     <label id="newlablRL"></label>
                     <button id="dashboard_btnrefreshgridRL" name="dashboard_btnrefreshgridRL" class="btn btn-primary" onclick="return dashboard_arBind_DifferenceQuickbook_BillNoRL();"><i class="fas fa-redo-alt"></i>&nbsp; Refresh Data</button>
                 </div>
                 <div class="erp-table-wrap">
                     <table class="table table-bordered" id="dashboard_ardifferenceRL" style="width: 100%;"></table>
                 </div>
             </div>
             <div class="tab-pane fade" id="custom-tabs-one-profile_deal" role="tabpanel" aria-labelledby="custom-tabs-one-profile_deal">
                 <div class="erp-table-wrap">
                     <table class="table table-bordered" id="dashboard_ar_update" style="width: 100%;"></table>
                 </div>
             </div>
         </div>
     </div>
 </div>
 <div class="modal fade" id="waitingpanel" tabindex="-1" data-bs-backdrop="static" aria-hidden="true">
     <div class="modal-dialog text-center">
         <img src="../Images/Load.gif" />
         <br />
         <span style="color: #fff; font-size: 24px; font-weight: bold; font-style: italic;" id="spntext">System is updating details. Please wait</span>
         <span style="color: #fff; font-size: 48px; font-weight: bold; font-style: italic; animation: animate 1s linear infinite;">&nbsp;. . . .</span>
     </div>
 </div>
 <%--Add Reconciliation Remark--%>
 <div class="modal fade" id="dashboard_updateremark">
     <div class="modal-dialog modal-xl">
         <div class="modal-content">
             <div class="modal-header">
                 <h4 class="modal-title">Add Reconciliation Remark</h4>
                 <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                     <span aria-hidden="true">&times;</span>
                 </button>
             </div>
             <div class="modal-body">
                 <table class="erp-form-grid">
                     <tr>
                         <td><b>Client:</b></td>
                         <td>
                             <label id="dashboard_projectid" class="form-control" style="width: 300px; display: none;"></label>
                             <label id="dashboard_monthyear" class="form-control" style="width: 300px; display: none;"></label>
                             <label id="dashboard_projectno" class="form-control" style="width: 300px;"></label>
                         </td>
                         <td><b>Process:</b></td>
                         <td>
                             <label id="dashboard_process" class="form-control" style="width: 300px;"></label>
                         </td>
                     </tr>
                     <tr>

                         <td><b>Remark:</b></td>
                         <td colspan="3">
                             <textarea id="dashboard_remark" name="dashboard_remark" class="form-control" style="width: 300px;"></textarea>
                         </td>

                     </tr>
                 </table>
             </div>
             <div class="modal-footer justify-content-end">
                 <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                 <button class="btn btn-primary" type="button" id="dashboard_remark_btnsubmit" onclick="return dashboard_remark_submit();">Submit</button>
             </div>
         </div>
         <!-- /.modal-content -->
     </div>
     <!-- /.modal-dialog -->
 </div>
 <%--Upload Excel Attachment--%>
 <div class="modal fade" id="dashboard_uploadattachment">
     <div class="modal-dialog modal-xl">
         <div class="modal-content">
             <div class="modal-header">
                 <h4 class="modal-title">Upload Excel Attachment</h4>
                 <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                     <span aria-hidden="true">&times;</span>
                 </button>
             </div>
             <div class="modal-body">
                 <table class="erp-form-grid">
                     <tr>
                         <td><b>Client:</b></td>
                         <td>
                             <label id="dashboard_projectid_upload" class="form-control" style="width: 300px; display: none;"></label>
                             <label id="dashboard_monthyear_upload" class="form-control" style="width: 300px; display: none;"></label>
                             <label id="dashboard_projectno_upload" class="form-control" style="width: 300px;"></label>
                         </td>
                         <td><b>Process:</b></td>
                         <td>
                             <label id="dashboard_process_upload" class="form-control" style="width: 300px;"></label>
                         </td>
                     </tr>
                     <tr>

                         <td><b>Attachment:</b></td>
                         <td>
                             <input type="file" id="dashboard_attachment_upload" class="form-control" style="width: 300px;" />
                         </td>
                         <td></td>
                         <td></td>

                     </tr>
                 </table>
             </div>
             <div class="modal-footer justify-content-end">
                 <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                 <button class="btn btn-primary" type="button" id="dashboard_upload_btnsubmit" onclick="return dashboard_upload_submit();">Submit</button>
             </div>
         </div>
         <!-- /.modal-content -->
     </div>
     <!-- /.modal-dialog -->
 </div>

 <div class="modal fade" id="dashboard_RecRemark_dverror">
     <div class="modal-dialog modal-sm">
         <div class="modal-content">
             <div class="modal-header">
                 <h6 class="modal-title" id="dashboard_RecRemark_errmsg"></h6>
             </div>
             <div class="modal-footer align-content-center">
                 <button class="btn btn-primary" type="button" id="dashboard_RecRemark_btnMessage" onclick="return dashboard_RecRemark_MessageRedirect();">Okay</button>
             </div>
         </div>
         <!-- /.modal-content -->
     </div>
     <!-- /.modal-dialog -->
 </div>
    </div>
</asp:Content>


