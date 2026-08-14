<%@ Page Title="" Language="C#" MasterPageFile="~/BDM/BDM.Master" AutoEventWireup="true" CodeBehind="ProjectDetailsMaster.aspx.cs" Inherits="Vendor_Portal.BDM.ProjectDetailsMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>

     :root {
         --erp-primary: #0f4c81;
         --erp-primary-dark: #0b355d;
         --erp-accent: #0f766e;
         --erp-bg: #f4f7fb;
         --erp-surface: #ffffff;
         --erp-border: #d7e0ea;
         --erp-text: #1f2937;
         --erp-muted: #64748b;
         --erp-shadow: 0 10px 26px rgba(15, 23, 42, .08);
         --erp-radius: 10px;
     }

     body { background: var(--erp-bg); }

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

     .erp-page-shell { padding: 5px 18px 32px; }
     .erp-hero {
         display: flex;
         align-items: center;
         justify-content: space-between;
         gap: 16px;
         margin-bottom: 16px;
         border-radius: var(--erp-radius);
         padding: 10px 20px;
         color: #fff;
         background: linear-gradient(135deg, var(--erp-primary), var(--erp-accent));
         box-shadow: var(--erp-shadow);
     }
     .erp-hero h3 { margin: 0; font-size: 20px; font-weight: 750; }
     .erp-hero p { margin: 4px 0 0; color: rgba(255,255,255,.84); font-size: 13px; }
     .erp-hero .erp-hero-actions { display: flex; gap: 10px; align-items: center; }
     .erp-back-link, .erp-light-btn {
         display: inline-flex;
         align-items: center;
         gap: 8px;
         border: 1px solid rgba(255,255,255,.38);
         border-radius: 8px;
         padding: 8px 12px;
         color: #fff !important;
         background: rgba(255,255,255,.13);
         font-weight: 700;
         font-size: 13px;
     }
     .erp-back-link:hover, .erp-light-btn:hover { background: rgba(255,255,255,.22); color: #fff !important; }

     .project-details-modern > .col-lg-12,
     .project-master-modern > .col-lg-12 { padding: 0; }

     .project-details-modern .card,
     .project-master-modern .card {
         border: 1px solid var(--erp-border) !important;
         border-radius: var(--erp-radius) !important;
         box-shadow: var(--erp-shadow) !important;
         overflow: hidden;
         background: var(--erp-surface);
     }
     .project-details-modern .card-body,
     .project-master-modern .card-body { padding: 16px; }
     .project-details-modern .card.card-tabs { box-shadow: none !important; border: 0 !important; }
     .project-details-modern .card.card-tabs > .card-header {
         padding: 0 !important;
         border-bottom: 1px solid var(--erp-border) !important;
         background: linear-gradient(180deg,#fff,#f8fafc) !important;
     }
     .project-details-modern .nav-tabs { border: 0; padding: 0 8px; gap: 4px; }
     .project-details-modern .nav-tabs .nav-link {
         border: 0 !important;
         border-radius: 8px 8px 0 0 !important;
         color: var(--erp-muted) !important;
         font-weight: 750;
         padding: 12px 14px;
     }
     .project-details-modern .nav-tabs .nav-link.active {
         color: var(--erp-primary) !important;
         background: #edf7f6 !important;
         border-bottom: 3px solid var(--erp-accent) !important;
     }

     .project-details-modern .tab-pane > table.table,
     .project-details-modern .tab-pane > table.table > tbody,
     .project-details-modern .tab-pane > table.table > tbody > tr,
     .project-details-modern .tab-pane > table.table > tr { display: block; width: 100%; }

     .project-details-modern .tab-pane > table.table > tbody > tr,
     .project-details-modern .tab-pane > table.table > tr {
         display: grid;
         grid-template-columns: 145px minmax(220px, 1fr) 145px minmax(220px, 1fr) 145px minmax(220px, 1fr);
         gap: 10px 12px;
         align-items: center;
         margin-bottom: 12px;
         border: 0;
     }
     .project-details-modern .tab-pane > table.table td {
         display: block;
         border: 0 !important;
         padding: 0 !important;
         vertical-align: middle;
     }
     .project-details-modern .tab-pane > table.table td[colspan] {
         grid-column: 1 / -1;
         text-align: right !important;
         padding-top: 6px !important;
     }
     .project-details-modern .tab-pane > table.table b,
     .project-details-modern label { color: var(--erp-text); font-size: 13px; font-weight: 750; }
     .project-details-modern .form-control,
     .project-master-modern .form-control,
     .project-details-modern select,
     .project-details-modern textarea {
         width: 100% !important;
         min-height: 36px;
         border: 1px solid #b8c7d8 !important;
         border-radius: 7px !important;
         color: var(--erp-text);
         box-shadow: none !important;
     }
     .project-details-modern textarea.form-control { min-height: 72px; }
     .project-details-modern .form-control:focus,
     .project-master-modern .form-control:focus {
         border-color: var(--erp-accent) !important;
         box-shadow: 0 0 0 .16rem rgba(15,118,110,.14) !important;
     }

     .project-details-modern .btn,
     .project-master-modern .btn,
     .erp-icon-btn {
         display: inline-flex;
         align-items: center;
         justify-content: center;
         gap: 8px;
         border-radius: 8px !important;
         font-weight: 750 !important;
         min-height: 36px;
         padding: 7px 14px !important;
     }
     .project-details-modern .btn-primary,
     .project-master-modern .btn-primary,
     .project-details-modern .btn-info,
     .project-master-modern .btn-info {
         border-color: var(--erp-primary) !important;
         background: var(--erp-primary) !important;
         color: #fff !important;
     }
     .project-details-modern .btn-primary:hover,
     .project-master-modern .btn-primary:hover,
     .project-details-modern .btn-info:hover,
     .project-master-modern .btn-info:hover { background: var(--erp-primary-dark) !important; }
     .project-details-modern .btn-success,
     .project-master-modern .btn-success { border-color: var(--erp-accent) !important; background: var(--erp-accent) !important; color:#fff!important; }
     .project-details-modern .btn-warning,
     .project-master-modern .btn-warning { color:#1f2937!important; }

     .project-master-modern .erp-toolbar {
         display: flex;
         align-items: center;
         justify-content: space-between;
         gap: 12px;
         margin-bottom: 14px;
     }
     .project-master-modern .erp-toolbar-title { font-size: 15px; font-weight: 750; color: var(--erp-text); }
     .project-master-modern .table-responsive,
     .project-master-modern .dataTables_wrapper { width: 100%; overflow-x: auto; }
     .project-master-modern table,
     .project-details-modern table.dataTable { width: 100% !important; }
     .project-master-modern .table thead th,
     .project-details-modern .table.dataTable thead th,
     .project-details-modern table.dataTable thead th {
         color: #334155 !important;
         background: #eef3f8 !important;
         border-color: var(--erp-border) !important;
         font-size: 12px;
         text-transform: uppercase;
         white-space: nowrap;
     }
     .project-master-modern .table td,
     .project-master-modern .table th,
     .project-details-modern .table.dataTable td,
     .project-details-modern .table.dataTable th { border-color: var(--erp-border) !important; vertical-align: middle !important; }
     .buttons-excel, .buttons-html5 {
         color: #fff !important;
         box-shadow: none !important;
         background: linear-gradient(135deg, var(--erp-primary), var(--erp-accent)) !important;
         border: 0 !important;
         font-weight: bold !important;
         margin: 0 8px !important;
         border-radius: 7px !important;
     }
     div.dt-buttons { position: static; padding-left: 10px; float: left; }
     .dataTables_length, .dataTables_info { float: left !important; }

     .project-details-modern .modal-content {
         border: 1px solid var(--erp-border);
         border-radius: var(--erp-radius);
         box-shadow: 0 20px 50px rgba(15,23,42,.18);
         overflow: hidden;
     }
     .project-details-modern .modal-header {
         border-bottom: 1px solid var(--erp-border);
         background: linear-gradient(135deg, var(--erp-primary), var(--erp-accent));
         color: #fff;
     }
     .project-details-modern .modal-header .close { color: #fff; opacity: .9; }

     @media (max-width: 1200px) {
         .project-details-modern .tab-pane > table.table > tbody > tr,
         .project-details-modern .tab-pane > table.table > tr { grid-template-columns: 140px minmax(220px, 1fr) 140px minmax(220px, 1fr); }
     }
     @media (max-width: 768px) {
         .erp-page-shell { padding: 12px; }
         .erp-hero { align-items: flex-start; flex-direction: column; }
         .project-details-modern .tab-pane > table.table > tbody > tr,
         .project-details-modern .tab-pane > table.table > tr { grid-template-columns: 1fr; }
     }

 </style>
 <script>

     $(document).ready(function () {
         bdmprojectmaster_bindgrid();
     });

     function bpm_addnewproject() {
         location.href = "ProjectDetails.aspx";
         return false;
     }
 </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="bdm-page">
        <div class="page-header">
            <div class="header-content">
                <div class="header-left">
                    <div class="header-icon"><i class="fas fa-clipboard-list"></i></div>
                    <div>
                        <h1>Project Details Master</h1>
                        <p>Create and manage project billing templates and pricing details.</p>
                    </div>
                </div>
                <div class="header-right">
                    <span class="module-badge">Masters</span>
                </div>
            </div>
        </div>
    <div class="loading" id="load1">
    <div class="loading__box">
        <img src="../images/Load_1.gif" alt="Loading" />
        <div class="loading__text">One moment, please...</div>
    </div>
</div>
  <div class="erp-page-shell project-master-modern">
      <div class="erp-hero" style="display:none;">
          <div>
              <h3><i class="fas fa-folder-open"></i>&nbsp; Project Details Master</h3>
              <p>Review configured client billing templates and open project details for maintenance.</p>
          </div>
          <div class="erp-hero-actions">
              <button type="button" class="erp-light-btn" disabled="disabled"><i class="fas fa-plus-circle"></i><span>New Project</span></button>
          </div>
      </div>
  <div class="col-lg-12">
      <div class="card">
          <div class="card-body">
              <div class="erp-toolbar"><div class="erp-toolbar-title"><i class="fas fa-list"></i>&nbsp; Project List</div><button id="bpm_btnaddnewproject" name="bpm_btnaddnewproject" class="btn btn-primary" onclick="return bpm_addnewproject();"><i class="fas fa-plus-circle"></i><span>Add New Project</span></button></div>
              <div class="table-responsive"><table class="table table-bordered" id="bpm_table" style="width:100%;">
                  <thead>
                      <tr>
                          <th class="sort border-top ps-3" style="text-wrap: nowrap;display:none;">ProjectID</th>
                          <th class="sort border-top ps-3" style="text-wrap: nowrap; ">Edit</th>
                          <th class="sort border-top ps-3" style="text-wrap: nowrap;">Sr. #</th>
                          <th class="sort border-top ps-3" style="text-wrap: nowrap;">Project #</th>
                          <th class="sort border-top ps-3" style="text-wrap: nowrap;">Process</th>
                          <th class="sort border-top ps-3" style="text-wrap: nowrap;">Company</th>
                          <th class="sort border-top ps-3" style="text-wrap: nowrap; ">Contact Person</th>
                          <th class="sort border-top ps-3" style="text-wrap: nowrap; ">Contact #</th>
                          <th class="sort border-top ps-3" style="text-wrap: nowrap; ">Address</th>
                          <th class="sort border-top ps-3" style="text-wrap: nowrap; display:none;">Remark</th>
                      </tr>
                      
                  </thead>
                  <tbody></tbody>
              </table></div>
          </div>
      </div>
  </div>
  </div>
    </div>
</asp:Content>

