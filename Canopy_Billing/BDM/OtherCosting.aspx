<%@ Page Title="" Language="C#" MasterPageFile="~/BDM/BDM.Master" AutoEventWireup="true" CodeBehind="OtherCosting.aspx.cs" Inherits="Vendor_Portal.BDM.OtherCosting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <style>
    :root {
        --erp-primary: #0f4c81;
        --erp-primary-dark: #0b355c;
        --erp-accent: #176b87;
        --erp-bg: #f4f7fb;
        --erp-surface: #ffffff;
        --erp-border: #d8e2ee;
        --erp-text: #1f2a37;
        --erp-muted: #667085;
        --erp-shadow: 0 10px 28px rgba(15, 23, 42, .08);
    }

    .loading {
        display: none;
        position: fixed;
        top: 350px;
        left: 50%;
        margin-top: -96px;
        margin-left: -96px;
        opacity: .85;
        border-radius: 25px;
        width: 192px;
        height: 192px;
        z-index: 99999;
        text-align: center;
    }

    .erp-page {
        padding: 5px 12px 34px;
        background: linear-gradient(180deg, rgba(15, 76, 129, .07), rgba(23, 107, 135, 0)), var(--erp-bg);
        min-height: calc(100vh - 70px);
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
        margin-bottom: 16px;
        border: 1px solid rgba(255, 255, 255, .18);
        border-radius: 12px;
        padding: 16px 20px;
        color: #fff;
        background: linear-gradient(135deg, var(--erp-primary-dark), var(--erp-primary) 55%, var(--erp-accent));
        box-shadow: 0 16px 36px rgba(15, 76, 129, .18);
    }

    .erp-hero h4 {
        margin: 0;
        font-size: 18px;
        font-weight: 750;
        letter-spacing: .2px;
    }

    .erp-hero p {
        margin: 4px 0 0;
        color: rgba(255, 255, 255, .78);
        font-size: 13px;
    }

    .erp-hero-badge {
        display: inline-flex;
        align-items: center;
        border: 1px solid rgba(255,255,255,.24);
        border-radius: 999px;
        padding: 7px 12px;
        color: rgba(255,255,255,.92);
        background: rgba(255,255,255,.12);
        font-size: 12px;
        font-weight: 700;
        white-space: nowrap;
    }

    .erp-panel {
        margin-bottom: 16px;
        border: 1px solid var(--erp-border);
        border-radius: 12px;
        background: var(--erp-surface);
        box-shadow: var(--erp-shadow);
        overflow: hidden;
    }

    .erp-panel-header {
        display: flex;
        align-items: center;
        justify-content: space-between;
        gap: 12px;
        border-bottom: 1px solid var(--erp-border);
        padding: 13px 16px;
        background: linear-gradient(180deg, #ffffff, #f8fbff);
    }

    .erp-panel-title {
        margin: 0;
        color: var(--erp-text);
        font-size: 15px;
        font-weight: 750;
    }

    .erp-panel-subtitle {
        margin: 2px 0 0;
        color: var(--erp-muted);
        font-size: 12px;
    }

    .erp-panel-body {
        padding: 16px;
    }

    .erp-form-grid {
        display: grid;
        grid-template-columns: repeat(4, minmax(180px, 1fr));
        gap: 14px;
        align-items: end;
    }

    .erp-field label {
        display: block;
        margin-bottom: 6px;
        color: #344054;
        font-size: 12px;
        font-weight: 700 !important;
        border: none !important;
    }

    .erp-field .form-control {
        width: 100% !important;
        min-height: 38px;
        border-color: #b9c7d8;
        border-radius: 7px;
        color: var(--erp-text);
        font-size: 13px;
    }

    .erp-field .form-control:focus {
        border-color: var(--erp-primary);
        box-shadow: 0 0 0 .2rem rgba(15, 76, 129, .14);
    }

    .erp-actions {
        display: flex;
        justify-content: flex-end;
        gap: 8px;
    }

    .erp-detail-card { grid-column: 1 / -1; border: 1px solid var(--erp-border); border-radius: 9px; padding: 14px; background: #f8fbff; }
    .erp-detail-toolbar { display:flex; justify-content:space-between; align-items:center; gap:12px; margin-bottom:10px; }
    .erp-detail-table input { min-width:110px; }
    .erp-detail-table .product-description { min-width:260px; }
    .erp-detail-table .invoice-comment { min-width:260px; }
    .remove-product-row i { color:#fff !important; }

    .erp-actions .btn,
    #secrel_btnsubmit,
    #secrel_btnMessage {
        border-radius: 7px;
        padding: 8px 18px;
        font-weight: 750;
    }

    #secrel_btnsubmit,
    #secrel_btnMessage,
    .btn-primary {
        border-color: var(--erp-primary) !important;
        background: linear-gradient(135deg, var(--erp-primary), var(--erp-accent)) !important;
        color: #fff !important;
    }

    .erp-table-wrap {
        padding: 0 16px 16px;
    }

    .erp-table-scroll {
        border: 1px solid var(--erp-border);
        border-radius: 10px;
        overflow-x: auto;
        background: #fff;
    }

    #secrel_table {
        margin-bottom: 0 !important;
        width: 100% !important;
    }

    #secrel_table thead th,
    .table.dataTable th {
        border-bottom: 1px solid #bfd0e3 !important;
        color: #334155 !important;
        background: linear-gradient(180deg, #eef5fb, #ffffff) !important;
        font-size: 12px;
        font-weight: 800;
        text-transform: uppercase;
        white-space: nowrap;
    }

    #secrel_table tbody td,
    .table.dataTable tr td {
        background: #fff !important;
        color: var(--erp-text);
        font-size: 13px;
        vertical-align: middle !important;
    }

    #secrel_table tbody tr:hover td {
        background: #f8fbff !important;
    }

    .dataTables_length, .dataTables_info {
        float: left !important;
    }

    div.dt-buttons {
        position: static;
        padding-left: 16px;
        float: left;
    }

    .buttons-excel, .buttons-html5 {
        color: #fff !important;
        box-shadow: none;
        background: linear-gradient(135deg, var(--erp-primary), var(--erp-accent)) !important;
        border: 0 !important;
        border-radius: 7px !important;
        font-weight: 750;
        margin: 0 8px;
    }

    label:not(.form-check-label):not(.custom-file-label) {
        font-weight: normal !important;
        border: none !important;
    }

    .modal-content {
        border: 1px solid var(--erp-border);
        border-radius: 12px;
        box-shadow: var(--erp-shadow);
    }

    .modal-header,
    .modal-footer {
        border-color: var(--erp-border);
    }

    @media (max-width: 992px) {
        .erp-form-grid {
            grid-template-columns: repeat(2, minmax(220px, 1fr));
        }
        .erp-actions {
            justify-content: flex-start;
        }
    }

    @media (max-width: 576px) {
        .erp-page {
            padding: 12px 8px 24px;
        }
        .erp-hero {
            align-items: flex-start;
            flex-direction: column;
        }
        .erp-form-grid {
            grid-template-columns: 1fr;
        }
        #secrel_btnsubmit {
            width: 100%;
        }
    }
</style>
<script>
    $(document).ready(function () {
        secrel_bindcompany();
        secrel_BindOtherCosting();
        secrel_toggleConfigurationFields();
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="bdm-page">
  <div class="loading" id="load1">
    <div class="loading__box">
        <img src="../images/Load_1.gif" alt="Loading" />
        <div class="loading__text">One moment, please...</div>
    </div>
</div>
<div class="erp-page">
    <div class="erp-container">
        <div class="erp-hero">
            <div>
                <h4><i class="fas fa-copy"></i>&nbsp;&nbsp;Securitization / Reliance Letter Costing</h4>
                <p>Maintain client-wise rates with the same professional ERP navigation theme.</p>
            </div>
            <span class="erp-hero-badge">Costing Master</span>
        </div>

        <div class="erp-panel">
            <div class="erp-panel-header">
                <div>
                    <h5 class="erp-panel-title">Cost Details</h5>
                    <p class="erp-panel-subtitle">Configure the billing basis used when invoices are created.</p>
                </div>
            </div>
            <div class="erp-panel-body">
                <div class="erp-form-grid">
                    <div class="erp-field">
                        <label for="secrel_project">Client</label>
                        <select id="secrel_project" name="secrel_project" class="form-control"></select>
                    </div>
                    <div class="erp-field">
                        <label for="secrel_type">Document Type</label>
                        <select id="secrel_type" name="secrel_type" class="form-control">
                            <option value="">Select</option>
                            <option value="Securitization">Securitization</option>
                            <option value="Reliance Letter">Reliance Letter</option>
                        </select>
                    </div>
                    <div class="erp-field" id="secrel_method_field">
                        <label for="secrel_method">Billing Method</label>
                        <select id="secrel_method" class="form-control">
                            <option value="">Select</option>
                            <option value="PerFile">Per File</option>
                            <option value="Hourly">Hourly</option>
                        </select>
                    </div>
                    <div class="erp-field" id="secrel_rate_field">
                        <label for="secrel_rate">Rate</label>
                        <input id="secrel_rate" name="secrel_rate" type="number" min="0" step="0.01" class="form-control" />
                    </div>
                    <div class="erp-field" id="secrel_minimum_field">
                        <label for="secrel_minimum">Minimum Amount</label>
                        <input id="secrel_minimum" type="number" min="0" step="0.01" class="form-control" />
                    </div>
                    <div class="erp-field" id="secrel_cap_field">
                        <label for="secrel_cap">Maximum Cap</label>
                        <input id="secrel_cap" type="number" min="0" step="0.01" class="form-control" />
                    </div>
                    <div class="erp-field">
                        <label for="secrel_effectivefrom">Effective From</label>
                        <input id="secrel_effectivefrom" type="date" class="form-control" />
                    </div>
                    <div class="erp-actions">
                        <button id="secrel_btnsubmit" name="secrel_btnsubmit" class="btn btn-primary" onclick="return secrel_submit();">Submit</button>
                    </div>
                    <div id="secrel_product_details" class="erp-detail-card" style="display:none;">
                        <div class="erp-detail-toolbar">
                            <div><strong>Reliance Letter product rates</strong><div class="erp-panel-subtitle">Add only the product/rate combinations available for this client.</div></div>
                            <button type="button" class="btn btn-sm btn-primary" onclick="return secrel_addProductRow();"><i class="fas fa-plus"></i>&nbsp; Add product</button>
                        </div>
                        <div class="table-responsive">
                            <table class="table table-bordered erp-detail-table">
                                <thead><tr><th>Reliance Letter Scope / Product Type</th><th>Rate / File</th><th></th></tr></thead>
                                <tbody id="secrel_product_rows"></tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="erp-panel">
            <div class="erp-panel-header">
                <div>
                    <h5 class="erp-panel-title">Costing Records</h5>
                    <p class="erp-panel-subtitle">Review saved securitization and reliance letter costing entries.</p>
                </div>
            </div>
            <div class="erp-table-wrap">
                <div class="erp-table-scroll">
                    <table class="table table-bordered" id="secrel_table" style="width: 100%">
                        <thead>
                            <tr>
                                <th class="sort border-top ps-3">Action</th>
                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Client</th>
                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Rate</th>
                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Document Type</th>
                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Billing Method</th>
                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Minimum</th>
                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Cap</th>
                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Added By</th>
                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Added Date</th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
</div>

<div class="modal fade" id="secrel_dverror">
    <div class="modal-dialog modal-sm">
        <div class="modal-content">
            <div class="modal-header">
                <h6 class="modal-title" id="secrel_errmsg"></h6>
            </div>
            <div class="modal-footer align-content-center">
                <button class="btn btn-primary" type="button" id="secrel_btnMessage" onclick="return secrel_MessageRedirect();">Okay</button>
            </div>
        </div>
    </div>
</div>
    </div>
</asp:Content>


