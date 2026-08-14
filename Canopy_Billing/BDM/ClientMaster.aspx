<%@ Page Title="" Language="C#" MasterPageFile="~/BDM/BDM.Master" AutoEventWireup="true" CodeBehind="ClientMaster.aspx.cs" Inherits="Canopy_Billing.BDM.ClientMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        :root {
            --erp-primary: #0f766e;
            --erp-primary-dark: #115e59;
            --erp-navy: #0f172a;
            --erp-blue: #2563eb;
            --erp-bg: #f5f7fb;
            --erp-surface: #ffffff;
            --erp-surface-soft: #f8fafc;
            --erp-border: #d9e2ec;
            --erp-border-strong: #b8c7d8;
            --erp-text: #1f2a37;
            --erp-muted: #667085;
            --erp-shadow: 0 14px 36px rgba(15, 23, 42, 0.08);
            --erp-radius: 8px;
        }

        .content-wrapper,
        .right-side,
        body {
            background: linear-gradient(180deg, rgba(15, 118, 110, 0.06), rgba(37, 99, 235, 0)), var(--erp-bg) !important;
        }

        .loading {
            display: none;
            position: fixed;
            inset: 0;
            align-items: center;
            justify-content: center;
            z-index: 99999;
            background: rgba(15, 23, 42, 0.28);
            text-align: center;
        }

        .loading-inner {
            border: 1px solid rgba(184, 199, 216, 0.75);
            border-radius: var(--erp-radius);
            padding: 22px 26px;
            background: #fff;
            box-shadow: var(--erp-shadow);
            color: var(--erp-text);
            font-size: 13px;
            font-weight: 700;
        }

        .loading img {
            display: block;
            max-width: 56px;
            margin: 0 auto 12px;
        }

        .client-master-page {
            max-width: 1480px;
            margin: 0 auto;
            padding: 5px 14px 34px;
        }

        .erp-page-hero {
            overflow: hidden;
            position: relative;
            border: 1px solid rgba(184, 199, 216, 0.7);
            border-radius: var(--erp-radius);
            margin-bottom: 16px;
            padding: 10px 22px;
            background: linear-gradient(135deg, var(--erp-navy), var(--erp-primary-dark) 58%, var(--erp-blue));
            color: #fff;
            box-shadow: 0 16px 38px rgba(15, 23, 42, 0.15);
        }

            .erp-page-hero:after {
                content: "";
                position: absolute;
                right: -80px;
                top: -85px;
                width: 230px;
                height: 230px;
                border-radius: 50%;
                background: rgba(255, 255, 255, 0.09);
            }

        .erp-hero-content {
            position: relative;
            z-index: 1;
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 18px;
        }

        .erp-eyebrow {
            margin: 0 0 4px;
            color: rgba(255, 255, 255, 0.78);
            font-size: 12px;
            font-weight: 750;
            text-transform: uppercase;
            letter-spacing: .08em;
        }

        .erp-page-title {
            margin: 0;
            font-size: 24px;
            font-weight: 800;
            line-height: 1.2;
        }

        .erp-page-subtitle {
            margin: 6px 0 0;
            color: rgba(255, 255, 255, 0.82);
            font-size: 13px;
        }

        .erp-hero-icon {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 52px;
            height: 52px;
            border: 1px solid rgba(255, 255, 255, 0.22);
            border-radius: 14px;
            background: rgba(255, 255, 255, 0.12);
            font-size: 22px;
        }

        .erp-panel {
            border: 1px solid var(--erp-border);
            border-radius: var(--erp-radius);
            margin-bottom: 16px;
            background: var(--erp-surface);
            box-shadow: var(--erp-shadow);
        }

        .erp-panel-header {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 14px;
            border-bottom: 1px solid var(--erp-border);
            padding: 14px 18px;
            background: linear-gradient(180deg, #ffffff, #f8fafc);
        }

        .erp-panel-title {
            margin: 0;
            color: var(--erp-text);
            font-size: 15px;
            font-weight: 800;
        }

        .erp-panel-note {
            margin: 3px 0 0;
            color: var(--erp-muted);
            font-size: 12px;
        }

        .erp-panel-body {
            padding: 18px;
        }

        .erp-form-grid {
            display: grid;
            grid-template-columns: repeat(3, minmax(210px, 1fr));
            gap: 14px 16px;
            align-items: end;
        }

        .erp-field label {
            display: block;
            margin-bottom: 6px;
            border: 0 !important;
            color: #334155;
            font-size: 12px;
            font-weight: 750 !important;
            text-transform: uppercase;
            letter-spacing: .03em;
        }

        .erp-field .form-control {
            height: 38px;
            border: 1px solid var(--erp-border-strong);
            border-radius: 6px;
            color: var(--erp-text);
            box-shadow: none;
        }

            .erp-field .form-control:focus {
                border-color: var(--erp-primary);
                box-shadow: 0 0 0 0.2rem rgba(15, 118, 110, 0.14);
            }

        .erp-actions {
            display: flex;
            justify-content: flex-end;
            gap: 10px;
        }

        .btn {
            border-radius: 6px;
            font-weight: 750;
        }

        .btn-primary,
        .buttons-excel,
        .buttons-html5 {
            border-color: var(--erp-primary) !important;
            background: var(--erp-primary) !important;
            color: #fff !important;
            box-shadow: 0 8px 18px rgba(15, 118, 110, 0.18) !important;
        }

            .btn-primary:hover,
            .btn-primary:focus,
            .buttons-excel:hover,
            .buttons-html5:hover {
                border-color: var(--erp-primary-dark) !important;
                background: var(--erp-primary-dark) !important;
                color: #fff !important;
            }

        .erp-table-wrap {
            overflow: auto;
            border: 1px solid var(--erp-border);
            border-radius: var(--erp-radius);
            background: #fff;
        }

        #clm_table {
            margin-bottom: 0 !important;
        }

        .table.dataTable,
        .table {
            color: var(--erp-text);
        }

            .table.dataTable thead th,
            .table thead th {
                border-color: var(--erp-border-strong) !important;
                border-bottom: 1px solid var(--erp-border-strong) !important;
                background: #eef6f6 !important;
                color: #334155 !important;
                font-size: 12px;
                font-weight: 800;
                text-transform: uppercase;
                letter-spacing: .03em;
                vertical-align: middle !important;
                white-space: nowrap;
            }

            .table.dataTable tbody td,
            .table tbody td {
                border-color: var(--erp-border) !important;
                background: #fff !important;
                vertical-align: middle !important;
            }

        .table-hover tbody tr:hover td,
        #clm_table tbody tr:hover td {
            background: #f8fafc !important;
        }

        .dataTables_wrapper {
            padding: 0;
        }

            .dataTables_wrapper .row:first-child,
            .dataTables_wrapper .row:last-child {
                align-items: center;
                padding: 12px 0;
            }

        .dataTables_length,
        .dataTables_info {
            float: left !important;
            color: var(--erp-muted);
            font-size: 13px;
        }

        .dataTables_filter label {
            color: var(--erp-muted);
            font-size: 13px;
            font-weight: 650 !important;
        }

        .dataTables_filter input,
        .dataTables_length select {
            border: 1px solid var(--erp-border-strong);
            border-radius: 6px;
            padding: 5px 9px;
            color: var(--erp-text);
            background: #fff;
        }

        div.dt-buttons {
            position: static;
            float: left;
            padding-left: 12px;
        }

        .buttons-excel,
        .buttons-html5 {
            margin: 0 6px;
            padding: 6px 12px;
            border: 0;
            font-size: 13px;
        }

        .page-item.active .page-link {
            border-color: var(--erp-primary) !important;
            background: var(--erp-primary) !important;
        }

        .page-link {
            color: var(--erp-primary-dark);
        }

        .child-table {
            width: 100%;
            margin-top: 10px;
            border: 1px solid var(--erp-border);
            border-radius: var(--erp-radius);
            overflow: hidden;
        }

        .contact-row {
            display: flex;
            gap: 10px;
            align-items: center;
            margin-top: 10px;
        }

            .contact-row input {
                flex: 1;
                min-width: 150px;
            }

            .contact-row button {
                white-space: nowrap;
            }

        @media (max-width: 992px) {
            .erp-form-grid {
                grid-template-columns: repeat(2, minmax(210px, 1fr));
            }
        }

        @media (max-width: 640px) {
            .client-master-page {
                padding: 12px 10px 24px;
            }

            .erp-hero-content,
            .erp-panel-header {
                align-items: flex-start;
                flex-direction: column;
            }

            .erp-form-grid {
                grid-template-columns: 1fr;
            }

            .erp-actions,
            .erp-actions .btn {
                width: 100%;
            }
        }
    </style>
    <script>
        $(document).ready(function () {
            clm_loadTable();
        });

        function clm_addnewproject() {
            location.href = "ProjectDetails.aspx";
            return false;
        }
</script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="bdm-page">
        <div class="loading" id="load1">
            <div class="loading-inner">
                <img src="../images/Load_1.gif" alt="Loading" />
                <div>One moment, please . . . .</div>
            </div>
        </div>

        <div class="client-master-page">
            <div class="page-header">
                <div class="header-content">
                    <div class="header-left">
                        <div class="header-icon">
                            <i class="fas fa-file-invoice-dollar"></i>
                        </div>
                        <div>
                            <h1>Client Master</h1>
                            <p>Create client records and manage primary contact details from one clean workspace.</p>
                        </div>
                    </div>

                    <div class="header-right">
                        <span class="module-badge">Billing Operations</span>
                    </div>
                </div>
            </div>


            <section class="erp-panel">
                <div class="erp-panel-header">
                    <div>
                        <h2 class="erp-panel-title">Add Client</h2>
                        <p class="erp-panel-note">Enter client details and primary contact information.</p>
                    </div>
                    <button id="clm_btnaddnewproject" name="clm_btnaddnewproject" class="btn btn-primary" style="display: none;" onclick="return clm_addnewproject();">
                        <i class="fas fa-plus-circle"></i>&nbsp; Add New Project
             
                    </button>
                </div>
                <div class="erp-panel-body">
                    <div class="erp-form-grid">
                        <div class="erp-field">
                            <label for="txtClientName">Client</label>
                            <input type="text" id="txtClientName" class="form-control" placeholder="Enter client name" />
                        </div>
                        <div class="erp-field">
                            <label for="txtAddress">Address</label>
                            <input type="text" id="txtAddress" class="form-control" placeholder="Enter address" />
                        </div>
                        <div class="erp-field">
                            <label for="txtPrimaryPerson">Contact Person</label>
                            <input type="text" id="txtPrimaryPerson" class="form-control" placeholder="Primary contact" />
                        </div>
                        <div class="erp-field">
                            <label for="txtPrimaryPhone">Phone</label>
                            <input type="text" id="txtPrimaryPhone" class="form-control" placeholder="Contact number" />
                        </div>
                        <div class="erp-field">
                            <label for="txtPrimaryEmail">Email Address</label>
                            <input type="text" id="txtPrimaryEmail" class="form-control" placeholder="name@example.com" />
                        </div>
                        <div class="erp-actions">
                            <button onclick="return saveClient();" class="btn btn-primary">
                                <i class="fas fa-save"></i>&nbsp; Add Client
                     
                            </button>
                        </div>
                    </div>
                </div>
            </section>

            <section class="erp-panel">
                <div class="erp-panel-header">
                    <div>
                        <h2 class="erp-panel-title">Client Directory</h2>
                        <p class="erp-panel-note">Review clients, primary contacts, phone numbers, and email addresses.</p>
                    </div>
                </div>
                <div class="erp-panel-body">
                    <div class="erp-table-wrap">
                        <table class="table table-bordered table-hover" id="clm_table" style="width: 100%;">
                            <thead>
                                <tr>
                                    <th></th>
                                    <th>Client</th>
                                    <th>Address</th>
                                    <th>Primary Contact</th>
                                    <th>Phone</th>
                                    <th>Email</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>


