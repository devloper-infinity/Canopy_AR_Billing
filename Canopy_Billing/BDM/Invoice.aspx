<%@ Page Title="" Language="C#" MasterPageFile="~/BDM/BDM.Master" AutoEventWireup="true" CodeBehind="Invoice.aspx.cs" Inherits="Vendor_Portal.BDM.Invoice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/daterangepicker/daterangepicker.css" />
    <script src="https://cdn.jsdelivr.net/npm/moment/moment.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/daterangepicker/daterangepicker.min.js"></script>
    <style>
        :root {
            --erp-nav: #0f172a;
            --erp-primary: #0f766e;
            --erp-primary-dark: #115e59;
            --erp-accent: #2563eb;
            --erp-bg: #f5f7fb;
            --erp-surface: #ffffff;
            --erp-soft: #f8fafc;
            --erp-border: #d9e2ec;
            --erp-border-strong: #b8c7d8;
            --erp-text: #1f2a37;
            --erp-muted: #667085;
            --erp-shadow: 0 14px 34px rgba(15, 23, 42, .08);
            --erp-radius: 8px;
        }

        body {
            background: var(--erp-bg);
            color: var(--erp-text);
        }

        .content-header {
            padding: 10px 0 10px;
        }

        .erp-page {
            padding: 0 14px 36px;
        }

        .erp-container {
            max-width: 1480px;
            margin: 0 auto;
        }

        .erp-hero {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 16px;
            border: 1px solid rgba(15, 118, 110, .22);
            border-radius: var(--erp-radius);
            padding: 18px 20px;
            background: linear-gradient(135deg, rgba(15, 23, 42, .98), rgba(15, 118, 110, .92));
            color: #fff;
            box-shadow: var(--erp-shadow);
        }

        .erp-hero-title {
            margin: 0;
            font-size: 14px;
            font-weight: 750;
            letter-spacing: .2px;
        }

        .erp-hero-subtitle {
            margin: 4px 0 0;
            color: rgba(255,255,255,.76);
            font-size: 12px;
        }

        .erp-hero-badge {
            border: 1px solid rgba(255,255,255,.22);
            border-radius: 999px;
            padding: 8px 12px;
            background: rgba(255,255,255,.10);
            font-size: 12px;
            font-weight: 700;
            white-space: nowrap;
        }

        .erp-panel {
            border: 1px solid var(--erp-border);
            border-radius: var(--erp-radius);
            background: var(--erp-surface);
            box-shadow: var(--erp-shadow);
            margin-bottom: 16px;
            overflow: hidden;
        }

        .erp-panel-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            gap: 12px;
            padding: 14px 18px;
            border-bottom: 1px solid var(--erp-border);
            background: var(--erp-soft);
        }

        .erp-panel-title {
            margin: 0;
            font-size: 15px;
            font-weight: 750;
            color: var(--erp-text);
        }

        .erp-panel-subtitle {
            margin: 3px 0 0;
            color: var(--erp-muted);
            font-size: 12px;
        }

        .erp-panel-body {
            padding: 18px;
        }

        .erp-tabs.card-header {
            border: 0;
            padding: 0;
            background: transparent;
        }

        .erp-tabs .nav-tabs {
            border-bottom: 1px solid var(--erp-border);
            gap: 4px;
            padding: 0 18px;
            background: var(--erp-soft);
        }

        .erp-tabs .nav-link {
            border: 0 !important;
            border-radius: 6px 6px 0 0;
            color: var(--erp-muted);
            font-weight: 750;
            padding: 12px 16px;
        }

            .erp-tabs .nav-link.active {
                color: var(--erp-primary-dark) !important;
                background: #fff !important;
                border-top: 3px solid var(--erp-primary) !important;
            }

            .erp-tabs .nav-link:hover {
                color: var(--erp-primary-dark);
                background: rgba(15,118,110,.08);
            }

        .erp-form-grid {
            display: grid;
            grid-template-columns: repeat(3, minmax(220px, 1fr));
            gap: 14px 16px;
        }

        .erp-field label {
            display: block;
            margin-bottom: 6px;
            color: #334155;
            font-size: 12px;
            font-weight: 750 !important;
            text-transform: uppercase;
            letter-spacing: .03em;
        }

        .erp-field .form-control, .erp-field select, .erp-field textarea {
            width: 100% !important;
            min-height: 38px;
            border-color: var(--erp-border-strong);
            border-radius: 6px;
            font-size: 14px;
        }

        .erp-field textarea {
            min-height: 38px;
            resize: vertical;
        }

        .erp-contact-grid {
            grid-column: 1 / -1;
            display: grid;
            grid-template-columns: repeat(3, minmax(220px, 1fr));
            gap: 14px 16px;
        }

        .erp-email-config {
            grid-column: 1 / -1;
        }

        .erp-email-card {
            border: 1px solid var(--erp-border);
            border-radius: 8px;
            background: var(--erp-soft);
            padding: 14px;
        }

        .erp-email-card-header {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 12px;
            margin-bottom: 10px;
        }

        .erp-email-card-title {
            color: #334155;
            font-size: 12px;
            font-weight: 750;
            text-transform: uppercase;
            letter-spacing: .03em;
        }

        .erp-email-count {
            border-radius: 999px;
            background: #dff4f1;
            color: var(--erp-primary-dark);
            padding: 3px 9px;
            font-size: 11px;
            font-weight: 750;
        }

        .erp-email-chip-list {
            display: flex;
            flex-wrap: wrap;
            gap: 8px;
            min-height: 42px;
            padding: 8px;
            border: 1px solid var(--erp-border-strong);
            border-radius: 6px;
            background: #fff;
        }

        .erp-email-chip {
            display: inline-flex;
            align-items: center;
            gap: 8px;
            max-width: 100%;
            border: 1px solid rgba(15,118,110,.22);
            border-radius: 999px;
            background: #ecfdf9;
            color: #134e4a;
            padding: 6px 7px 6px 11px;
            font-size: 12px;
            font-weight: 500;
            font-weight: 650;
        }

        .erp-email-chip-text {
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
        }

        .erp-email-remove {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 24px;
            height: 24px;
            border: 0;
            border-radius: 50%;
            background: transparent;
            color: #64748b;
            cursor: pointer;
            transition: background .15s ease, color .15s ease;
        }

            .erp-email-remove:hover, .erp-email-remove:focus {
                background: #fee2e2;
                color: #b42318;
                outline: none;
            }

        .erp-email-empty {
            display: flex;
            align-items: center;
            gap: 8px;
            color: var(--erp-muted);
            font-size: 13px;
            padding: 3px 4px;
        }

        .erp-email-config-row {
            display: grid;
            grid-template-columns: minmax(280px, 1fr) auto;
            gap: 10px;
            align-items: end;
            margin-top: 12px;
        }

        .erp-email-input-wrap {
            position: relative;
        }

            .erp-email-input-wrap .form-control {
                padding-left: 38px;
            }

        .erp-email-input-icon {
            position: absolute;
            left: 13px;
            top: 50%;
            transform: translateY(-50%);
            color: #94a3b8;
            pointer-events: none;
        }

        .erp-email-add-btn {
            min-width: 112px;
            min-height: 38px;
        }

        .erp-email-help {
            display: block;
            margin-top: 8px;
            color: var(--erp-muted);
            font-size: 12px;
        }

        .erp-actions {
            display: flex;
            justify-content: flex-end;
            align-items: flex-end;
            gap: 10px;
        }

            .erp-actions .btn {
                min-width: 118px;
            }

        .erp-filter-grid {
            display: grid;
            grid-template-columns: 1fr 1fr 1.4fr auto;
            gap: 14px;
            align-items: end;
        }

        .erp-verify-box {
            border: 1px dashed var(--erp-border-strong);
            border-radius: var(--erp-radius);
            padding: 14px;
            background: #fff;
        }

        .erp-verify-actions {
            display: flex;
            justify-content: flex-end;
            margin-top: 10px;
        }

        .erp-table-wrap {
            width: 100%;
            overflow-x: auto;
            border: 1px solid var(--erp-border);
            border-radius: var(--erp-radius);
            background: #fff;
        }

            .erp-table-wrap .table {
                margin-bottom: 0;
            }

                .table.dataTable th, .erp-table-wrap .table thead th {
                    background: #eef6f5 !important;
                    color: #334155 !important;
                    border-bottom: 1px solid var(--erp-border-strong) !important;
                    font-size: 12px;
                    text-transform: uppercase;
                    white-space: nowrap;
                }

                .table.dataTable tr td, .erp-table-wrap .table td {
                    background: #fff !important;
                    vertical-align: middle !important;
                    white-space: nowrap;
                }

        .btn {
            border-radius: 6px;
            font-weight: 700;
        }

        .btn-primary {
            border-color: var(--erp-primary);
            background: var(--erp-primary);
        }

            .btn-primary:hover, .btn-primary:focus {
                border-color: var(--erp-primary-dark);
                background: var(--erp-primary-dark);
            }

        .btn-secondary {
            border-color: var(--erp-border-strong);
            background: #fff;
            color: var(--erp-text);
        }

        .btn-success {
            background: #15803d;
            border-color: #15803d;
        }

        .btn-danger {
            background: #b42318;
            border-color: #b42318;
        }

        .buttons-excel, .buttons-html5 {
            color: #fff;
            background: linear-gradient(135deg, var(--erp-primary), var(--erp-accent));
            border: 0;
            font-weight: 750;
            margin: 0 8px;
            border-radius: 6px;
        }

        div.dt-buttons {
            position: static;
            padding-left: 18px;
            float: left;
        }

        .dataTables_length, .dataTables_info {
            float: left !important;
        }

        .erp-import-card {
            border: 1px dashed var(--erp-border-strong);
            border-radius: var(--erp-radius);
            background: linear-gradient(180deg, #fff, #f8fafc);
            padding: 18px;
        }

        .erp-upload-row {
            display: grid;
            grid-template-columns: minmax(280px, 1fr) auto;
            gap: 14px;
            align-items: end;
            max-width: 680px;
        }

        .erp-summary-grid {
            display: grid;
            grid-template-columns: repeat(4, minmax(160px, 1fr));
            gap: 14px;
            margin-bottom: 18px;
        }

        .erp-summary-card {
            border-radius: var(--erp-radius);
            padding: 14px 16px;
            color: #fff;
            box-shadow: 0 10px 24px rgba(15, 23, 42, .08);
        }

            .erp-summary-card span {
                display: block;
                margin-top: 4px;
                font-size: 24px;
                font-weight: 800;
            }

        .erp-summary-total {
            background: linear-gradient(135deg, #0f766e, #2563eb);
        }

        .erp-summary-inserted {
            background: linear-gradient(135deg, #15803d, #16a34a);
        }

        .erp-summary-duplicate {
            background: linear-gradient(135deg, #b45309, #f59e0b);
        }

        .erp-summary-error {
            background: linear-gradient(135deg, #b42318, #ef4444);
        }

        .erp-section-title {
            margin: 18px 0 10px;
            font-size: 15px;
            font-weight: 750;
            color: var(--erp-text);
        }

        .invoice-costing-card { grid-column: 1 / -1; border: 1px solid var(--erp-border); border-radius: 8px; background: var(--erp-soft); padding: 14px; }
        .invoice-costing-grid { display:grid; grid-template-columns:repeat(4,minmax(170px,1fr)); gap:14px; }
        .invoice-detail-toolbar { display:flex; justify-content:space-between; align-items:center; margin-bottom:10px; }
        .invoice-detail-table input { min-width:100px; }
        .invoice-detail-table .invoice-product { min-width:250px; }
        .invoice-remove-product i { color:#fff !important; }
        .costing-adjustment { margin-top:8px; color:var(--erp-muted); font-size:12px; }

        #historyModal .modal-body {
            max-height: 500px;
            overflow-y: auto;
        }

        .modal-content {
            border: 1px solid var(--erp-border);
            border-radius: var(--erp-radius);
            box-shadow: var(--erp-shadow);
        }

        .modal-header {
            border-bottom: 1px solid var(--erp-border);
            background: var(--erp-soft);
        }

        #canopyAjaxAlertModal .modal-title {
            font-size: 18px;
            line-height: 1.4;
        }

        #canopyAjaxAlertModal .modal-body {
            font-size: 14px;
            line-height: 1.5;
            color: var(--erp-text);
        }

        #canopyAjaxAlertModal .modal-footer .btn,
        #canopyAjaxAlertModal .close {
            font-size: 13px;
        }

        .loading {
            display: none;
            position: fixed;
            inset: 0;
            margin: 0;
            width: auto;
            height: auto;
            z-index: 99999;
            background: rgba(15, 23, 42, .28);
            align-items: center;
            justify-content: center;
            text-align: center;
            border-radius: 0;
            opacity: 1;
        }

            .loading img {
                display: block;
                margin: 0 auto 8px;
                max-width: 64px;
            }

        label:not(.form-check-label):not(.custom-file-label) {
            font-weight: normal !important;
            border: none !important;
        }

        #excelPreviewContainer {
            max-height: 70vh;
            max-width: 100%;
            overflow: auto;
            border: 1px solid var(--erp-border);
            border-radius: 6px;
        }

        #excelTable {
            border-collapse: collapse;
            width: max-content;
            min-width: 100%;
            font-family: Calibri, Arial, sans-serif;
            font-size: 13px;
        }

            #excelTable tr:first-child {
                background-color: #f3f3f3;
                font-weight: bold;
            }

                #excelTable tr:first-child th {
                    position: sticky;
                    top: 0;
                    background: #f3f3f3;
                    z-index: 2;
                }

            #excelTable tr:hover td {
                background-color: #f9f9f9;
            }

            #excelTable tr:nth-child(even) {
                background-color: #fcfcfc;
            }

        .action-container {
            display: flex;
            align-items: center;
            gap: 10px;
            justify-content: center;
        }

        .action-icon {
            cursor: pointer;
            font-size: 16px;
            transition: .2s ease;
        }

            .action-icon:hover {
                transform: scale(1.2);
            }

        .fa-edit {
            color: #6f42c1;
        }

        .fa-trash-alt {
            color: #dc3545;
        }

        .fa-history {
            color: #795548;
        }

        .fa-plus-circle {
            color: #28a745;
        }

        .fa-eye {
            color: #ff9800;
        }

        .fa-download {
            color: #007bff;
        }

        .action-icon.disabled {
            color: #ccc !important;
            cursor: not-allowed;
        }

        .invoice-action-menu .dropdown-toggle {
            min-width: 104px;
            padding: 5px 10px;
            font-size: 12px;
        }

        .invoice-action-menu .dropdown-menu {
            min-width: 165px;
            padding: 5px 0;
        }

        .invoice-action-menu .dropdown-item {
            display: flex;
            align-items: center;
            gap: 10px;
            padding: 7px 13px;
            border: 0;
            background: transparent;
            color: var(--erp-text);
            font-size: 13px;
            text-align: left;
            width: 100%;
        }

            .invoice-action-menu .dropdown-item:hover, .invoice-action-menu .dropdown-item:focus {
                background: #f1f5f9;
                outline: none;
            }

            .invoice-action-menu .dropdown-item i {
                width: 16px;
                font-size: 13px;
                text-align: center;
            }

            .invoice-action-menu .dropdown-item:disabled {
                color: #9ca3af;
                cursor: not-allowed;
            }

                .invoice-action-menu .dropdown-item:disabled i {
                    color: #cbd5e1 !important;
                }

        @media (max-width: 992px) {
            .erp-form-grid, .erp-filter-grid, .erp-summary-grid, .erp-contact-grid {
                grid-template-columns: 1fr 1fr;
            }

            .erp-hero {
                align-items: flex-start;
                flex-direction: column;
            }
        }

        @media (max-width: 576px) {
            .erp-form-grid, .erp-filter-grid, .erp-upload-row, .erp-summary-grid, .erp-contact-grid, .erp-email-config-row {
                grid-template-columns: 1fr;
            }

            .erp-panel-body {
                padding: 14px;
            }

            .erp-actions {
                justify-content: stretch;
            }

                .erp-actions .btn {
                    width: 100%;
                }
        }
    </style>
    <script>

     window.onload = function () {
         document.getElementById('rlinvoice_attachment').addEventListener('change', getFileName);

     }
     const getFileName = (event) => {
         const files = event.target.files;
         var file = files[0];
         document.getElementById("rlivoice_filep").value = files[0].name;

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
         //alert(document.getElementById("filep").value);
     }
     $(document).ready(function () {
         rladdinvoice_bindcompany();
         rladdinvoice_bindBillingTable();


     });
     $(document).on("click", ".uploadBtn", function () {
         selectedRowId = $(this).data("id");
         $('#uploadModal').modal("show");
     });

     $(document).on("click", ".downloadBtn", function () {
         var fileName = $(this).attr("data-file");
         if (!fileName) {
             alert("File not found");
             return;
         }
         window.open("../Uploads/" + fileName, "_blank");
     });
     function hideviewpopup() {
         $("#excelModal").modal('hide');
         return false;
     }


 </script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.18.5/xlsx.full.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="bdm-page">
        <div id="excelModal" class="modal fade" tabindex="-1">
            <div class="modal-dialog modal-xl">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">View Excel</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" onclick="return hideviewpopup();">X</button>
                    </div>
                    <div class="modal-body" style="height: 80vh;">
                        <div id="excelPreviewContainer"></div>
                    </div>
                </div>
            </div>
        </div>

        <input id="addivoice_filep" style="display: none;" />
        <input id="rlivoice_filep" style="display: none;" />
        <input type="hidden" id="hdnInvoiceID" />
        <input type="hidden" id="formMode" value="add" />

        <div class="loading" id="load1">
            <div class="loading__box">
                <img src="../images/Load_1.gif" alt="Loading" />
                <div class="loading__text">One moment, please...</div>
            </div>
        </div>
        <div class="content-header">
            <div class="erp-container">
                <div class="erp-hero">
                    <div>
                        <h1 class="erp-hero-title"><i class="fas fa-file-invoice-dollar"></i>&nbsp; Invoice Management</h1>
                        <p class="erp-hero-subtitle">Create RL invoices, import Excel billing data, verify records, and review invoice history.</p>
                    </div>
                    <div class="erp-hero-badge">Billing Operations</div>
                </div>
            </div>
        </div>

        <div class="erp-page">
            <div class="erp-container">
                <div class="erp-panel">
                    <div class="card-header erp-tabs">
                        <ul class="nav nav-tabs" id="custom-tabs-one-tab" role="tablist">
                            <li class="nav-item">
                                <a class="nav-link active" id="custom-tabs-one-home-tab" data-toggle="pill" href="#custom-tabs-one-home" role="tab" aria-controls="custom-tabs-one-home" aria-selected="true">Add Invoice</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" id="custom-tabs-one-profile-tab" data-toggle="pill" href="#custom-tabs-one-profile" role="tab" aria-controls="custom-tabs-one-profile" aria-selected="false">Import Excel</a>
                            </li>
                        </ul>
                    </div>

                    <div class="erp-panel-body">
                        <div class="tab-content" id="custom-tabs-one-tabContent">
                            <div class="tab-pane fade show active" id="custom-tabs-one-home" role="tabpanel" aria-labelledby="custom-tabs-one-home-tab">
                                <div class="erp-panel">
                                    <div class="erp-panel-header">
                                        <div>
                                            <h2 class="erp-panel-title">Invoice Details</h2>
                                            <p class="erp-panel-subtitle">Fill in client, document, billing, and contact information.</p>
                                        </div>
                                    </div>
                                    <div class="erp-panel-body">
                                        <div class="erp-form-grid">
                                            <div class="erp-field">
                                                <label>Our Client</label>
                                                <input type="text" id="rladdinvoice_ourclient" name="rladdinvoice_ourclient" class="form-control" />
                                            </div>
                                            <div class="erp-field">
                                                <label>Recipient</label>
                                                <input type="text" id="rladdinvoice_recipient" name="rladdinvoice_recipient" class="form-control" />
                                            </div>
                                            <div class="erp-field">
                                                <label>Trade Name</label>
                                                <input type="text" id="rladdinvoice_tradename" name="rladdinvoice_tradename" class="form-control" />
                                            </div>
                                            <div class="erp-field">
                                                <label>Invoice Date</label>
                                                <input type="date" id="rladdinvoice_invoicedate" name="rladdinvoice_invoicedate" class="form-control" onchange="rladdinvoice_loadCostingConfiguration();" />
                                            </div>
                                            <div class="erp-field">
                                                <label>Document</label>
                                                <select id="rladdinvoice_document" name="rladdinvoice_document" class="form-control" onchange="rladdinvoice_loadCostingConfiguration();">
                                                    <option value="">Select</option>
                                                    <option value="Reliance Letter">Reliance Letter</option>
                                                    <option value="Securitization">Securitization</option>
                                                </select>
                                            </div>
                                            <div class="erp-field">
                                                <label>Transaction Manager</label>
                                                <select id="rladdinvoice_tm" name="rladdinvoice_tm" class="form-control">
                                                    <option value="">Select</option>
                                                    <option value="Ben Guest">Ben Guest</option>
                                                    <option value="Chris Chenen">Chris Chenen</option>
                                                    <option value="Chris Wallace">Chris Wallace</option>
                                                    <option value="Hannah Sanders">Hannah Sanders</option>
                                                    <option value="Isabel Navar">Isabel Navar</option>
                                                    <option value="ShaDonna Carter">ShaDonna Carter</option>
                                                </select>
                                            </div>
                                            <div class="erp-field">
                                                <label>Document Signed?</label>
                                                <select id="rladdinvoice_docsign" name="rladdinvoice_docsign" class="form-control">
                                                    <option value="">Select</option>
                                                    <option value="Yes">Yes</option>
                                                    <option value="No">No</option>
                                                </select>
                                            </div>
                                            <div class="erp-field">
                                                <label>Document Date</label>
                                                <input type="date" id="rladdinvoice_documentdate" name="rladdinvoice_documentdate" class="form-control" />
                                            </div>
                                            <div class="erp-field">
                                                <label>Executed Date</label>
                                                <input type="date" id="rladdinvoice_executeddate" name="rladdinvoice_executeddate" class="form-control" />
                                            </div>
                                            <div class="erp-field">
                                                <label>Billing Entity</label>
                                                <select id="rladdinvoice_billingentity" name="rladdinvoice_billingentity" class="form-control" onchange="rladdinvoice_getRLSecRate();"></select>
                                            </div>
                                            <div class="erp-field" id="rladdinvoice_loancount_field">
                                                <label id="rladdinvoice_loancount_label">Loan Count</label>
                                                <input type="number" id="rmaddinvoice_loancount" name="rmaddinvoice_loancount" class="form-control" onchange="getexpectedamount();" />
                                            </div>
                                            <div class="erp-field" id="rladdinvoice_cost_field">
                                                <label id="rladdinvoice_cost_label">Cost</label>
                                                <input type="text" id="rladdinvoice_cost" name="rladdinvoice_cost" class="form-control" onchange="getexpectedamount();" />
                                            </div>
                                            <div id="rladdinvoice_flexible_costing" class="invoice-costing-card" style="display:none;">
                                                <div class="invoice-costing-grid">
                                                    <div class="erp-field"><label>Billing Method</label><select id="rladdinvoice_billingmethod" class="form-control" onchange="rladdinvoice_manualMethodChanged();"><option value="">Select</option><option value="PerFile">Per File</option><option value="Hourly">Hourly</option></select></div>
                                                    <div class="erp-field" id="rladdinvoice_hours_field" style="display:none;"><label>Hours Worked</label><input type="number" min="0" step="0.01" id="rladdinvoice_hoursworked" class="form-control" oninput="getexpectedamount();" /></div>
                                                    <div class="erp-field"><label>Minimum Billing</label><input id="rladdinvoice_minimum" type="number" min="0" step="0.01" class="form-control" oninput="getexpectedamount();" /></div>
                                                    <div class="erp-field"><label>Maximum Cap</label><input id="rladdinvoice_cap" type="number" min="0" step="0.01" class="form-control" oninput="getexpectedamount();" /></div>
                                                </div>
                                                <div id="rladdinvoice_adjustment" class="costing-adjustment"></div>
                                            </div>
                                            <div id="rladdinvoice_product_costing" class="invoice-costing-card" style="display:none;">
                                                <div class="invoice-detail-toolbar"><div><strong>Reliance Letter scope-wise billing</strong><div class="erp-panel-subtitle">Enter quantity for configured scopes, or add a missing scope and rate.</div></div><button type="button" class="btn btn-sm btn-primary" onclick="return rladdinvoice_addManualProductRow();"><i class="fas fa-plus"></i>&nbsp; Add scope</button></div>
                                                <div class="table-responsive"><table class="table table-bordered invoice-detail-table"><thead><tr><th>Select</th><th>Scope / Product Type</th><th>Quantity</th><th>Rate / File</th><th>Amount</th><th></th></tr></thead><tbody id="rladdinvoice_product_rows"></tbody></table></div>
                                            </div>
                                            <div id="contactSection" class="erp-email-config" style="display: none;">
                                                <div class="erp-email-card">
                                                    <div class="erp-email-card-header">
                                                        <span class="erp-email-card-title"><i class="fas fa-envelope"></i>&nbsp; Invoice recipients</span>
                                                        <span id="rladdinvoice_emailcount" class="erp-email-count">0 emails</span>
                                                    </div>
                                                    <div id="rladdinvoice_emailchips" class="erp-email-chip-list" role="list" aria-live="polite"></div>
                                                    <div class="erp-email-config-row">
                                                        <div class="erp-field">
                                                            <label for="rladdinvoice_newemail">Add another recipient</label>
                                                            <div class="erp-email-input-wrap">

                                                                <input type="email" id="rladdinvoice_newemail" class="form-control" placeholder="name@example.com" autocomplete="email" />
                                                            </div>
                                                        </div>
                                                        <button type="button" id="rladdinvoice_addemailbtn" class="btn btn-primary erp-email-add-btn" onclick="return rladdinvoice_addEmail();"><i class="fas fa-plus"></i>&nbsp; Add email</button>
                                                    </div>
                                                    <small class="erp-email-help">Use the × beside an address to exclude it from this invoice. Removing it here does not delete it from the client record.</small>
                                                </div>
                                            </div>
                                            <div class="erp-field">
                                                <label>Expected Billing</label>
                                                <input type="text" id="rladdinvoice_expectedbilling" name="rladdinvoice_expectedbilling" class="form-control" readonly />
                                            </div>
                                            <div class="erp-field">
                                                <label>Notes</label>
                                                <textarea id="rladdinvoice_notes" name="rladdinvoice_notes" class="form-control"></textarea>
                                            </div>
                                            <div class="erp-actions">
                                                <button type="button" id="rladdinvoice_btnsubmit" name="rladdinvoice_btnsubmit" class="btn btn-primary" onclick="return rladdinvoice_submit();">Submit</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="erp-panel">
                                    <div class="erp-panel-header">
                                        <div>
                                            <h2 class="erp-panel-title">Invoice Records</h2>
                                            <p class="erp-panel-subtitle">Filter, export, verify, edit, and inspect invoice records.</p>
                                        </div>
                                    </div>
                                    <div class="erp-panel-body">
                                        <div class="erp-filter-grid">
                                            <div class="erp-field">
                                                <label>Trade Name</label>
                                                <input type="text" id="tradeFilter" class="form-control" placeholder="Search Trade Name">
                                            </div>
                                            <div class="erp-field">
                                                <label>Billing Entity</label>
                                                <select id="billingFilter" class="form-control">
                                                    <option value="">All</option>
                                                </select>
                                            </div>
                                            <div class="erp-field">
                                                <label>Invoice Date Range</label>
                                                <input type="text" id="dateRange" class="form-control" placeholder="Select date range">
                                            </div>
                                            <div>
                                                <button id="clearFilters" class="btn btn-secondary w-100">Clear Filters</button>
                                            </div>
                                        </div>

                                        <div id="verifySection" class="erp-verify-box" style="display: none; margin-top: 14px; margin-bottom: 14px;">
                                            <textarea id="verifyRemark" class="form-control" placeholder="Enter remark"></textarea>
                                            <div class="erp-verify-actions">
                                                <button id="btnVerifySubmit" class="btn btn-success">Submit Verification</button>
                                            </div>
                                        </div>

                                        <div class="erp-table-wrap">
                                            <table id="rladdinvoice_billingTable" class="table table-bordered table-striped" style="width: 100%;">
                                                <thead>
                                                    <tr>
                                                        <th>Verify</th>
                                                        <th>Action</th>
                                                        <th>Billing Entity</th>
                                                        <th>Trade Name</th>
                                                        <th>Invoice Date</th>
                                                        <th>Loan Count</th>
                                                        <th>Expected Billing</th>
                                                        <th>Cost</th>

                                                        <th>Recipient</th>
                                                        <th>Our Client</th>
                                                        <th>Document</th>
                                                        <th>Transaction Manager</th>
                                                        <th>Document Signed?</th>
                                                        <th>Document Date</th>
                                                        <th>Executed Date</th>
                                                        <th>Notes</th>
                                                    </tr>
                                                </thead>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="tab-pane fade" id="custom-tabs-one-profile" role="tabpanel" aria-labelledby="custom-tabs-one-profile-tab">
                                <div class="erp-panel">
                                    <div class="erp-panel-header">
                                        <div>
                                            <h2 class="erp-panel-title">Import Excel</h2>
                                            <p class="erp-panel-subtitle">Upload a billing sheet, validate records, and review import results before continuing.</p>
                                        </div>
                                    </div>
                                    <div class="erp-panel-body">
                                        <div class="erp-import-card">
                                            <div class="erp-upload-row">
                                                <div class="erp-field">
                                                    <label>Attachment</label>
                                                    <input type="file" id="rlinvoice_attachment" name="rlinvoice_attachment" class="form-control" />
                                                </div>
                                                <button id="rlinvocie_btnvalidate" type="button" name="rlinvocie_btnvalidate" class="btn btn-primary" onclick="return rlinvoice_uploadExcel();">Validate</button>
                                            </div>
                                        </div>

                                        <div id="rlinvoice_importcount" style="display: none; margin-top: 18px;">
                                            <div class="erp-summary-grid">
                                                <div class="erp-summary-card erp-summary-total">Total<span id="totalCount">0</span></div>
                                                <div class="erp-summary-card erp-summary-inserted">Inserted<span id="insertedCount">0</span></div>
                                                <div class="erp-summary-card erp-summary-duplicate">Duplicates<span id="duplicateCount">0</span></div>
                                                <div class="erp-summary-card erp-summary-error">Errors<span id="errorCount">0</span></div>
                                            </div>

                                            <h5 id="hdup" class="erp-section-title" style="display: none;">Duplicate Records</h5>
                                            <div class="erp-table-wrap">
                                                <table id="duplicateTable" class="table table-bordered"></table>
                                            </div>

                                            <h5 id="herr" class="erp-section-title" style="display: none;">Error Records</h5>
                                            <div class="erp-table-wrap">
                                                <table id="errorTable" class="table table-bordered"></table>
                                            </div>

                                            <h5 id="hins" class="erp-section-title" style="display: none;">Inserted Records</h5>
                                            <div class="erp-table-wrap">
                                                <table id="rlinvoice_validatetable" class="table table-bordered"></table>
                                            </div>
                                        </div>

                                        <div class="erp-table-wrap" style="margin-top: 18px;">
                                            <table id="rlinvoice_billingTable_1" class="table table-bordered table-striped" style="display: none; width: 100%;">
                                                <thead>
                                                    <tr>
                                                        <th>Our Client</th>
                                                        <th>Recipient</th>
                                                        <th>Trade Name</th>
                                                        <th>Invoice Date</th>
                                                        <th>Document</th>
                                                        <th>TM</th>
                                                        <th>Doc Sign</th>
                                                        <th>Document Date</th>
                                                        <th>Executed Date</th>
                                                        <th>Loan Count</th>
                                                        <th>Cost</th>
                                                        <th>Expected Billing</th>
                                                        <th>Billing Entity</th>
                                                        <th>Invoice Issued</th>
                                                        <th>Notes</th>
                                                    </tr>
                                                </thead>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="waitingpanel_1" tabindex="-1" data-bs-backdrop="static" aria-hidden="true">
            <div class="modal-dialog text-center">
                <img src="../Images/Load.gif" />
                <br />
                <span style="color: #fff; font-size: 24px; font-weight: bold; font-style: italic;" id="spntext">System is validating excel. Please wait</span>
                <span style="color: #fff; font-size: 48px; font-weight: bold; font-style: italic; animation: animate 1s linear infinite;">&nbsp;. . . .</span>
            </div>
        </div>

        <div class="modal fade" id="uploadModal">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title" id="dashboard_RecRemark_errmsg">Upload File</h6>
                    </div>
                    <div class="modal-body">
                        <div class="erp-field">
                            <label>Choose File</label>
                            <input type="file" id="fileUploadControl" class="form-control" />
                        </div>
                        <div class="text-right mt-3">
                            <button class="btn btn-success" type="button" onclick="return rladdinvoice_submitFile();">Submit</button>
                            <button class="btn btn-danger" onclick="return rladdinvoice_closeModal();">Close</button>
                        </div>
                    </div>
                    <div class="modal-footer align-content-center"></div>
                </div>
            </div>
        </div>

        <div id="historyModal" class="modal fade">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5>Invoice History</h5>
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">
                        <div class="erp-table-wrap">
                            <table id="historyTable" class="table table-bordered table-striped" style="width: 100%;">
                                <thead>
                                    <tr>
                                        <th>Field</th>
                                        <th>Old Value</th>
                                        <th>New Value</th>
                                        <th>Updated By</th>
                                        <th>Updated On</th>
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
</asp:Content>

