<%@ Page Title="" Language="C#" MasterPageFile="~/BDM/BDM.Master" AutoEventWireup="true" CodeBehind="ImportBilling.aspx.cs" Inherits="Canopy_Billing.BDM.ImportBilling" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

       <style>
        :root {
            --billing-navy: #071b34;
            --billing-teal: #238576;
            --billing-teal-dark: #145f58;
            --billing-bg: #f4f7fb;
            --billing-card: #ffffff;
            --billing-border: #dfe7f1;
            --billing-text: #172033;
            --billing-muted: #64748b;
            --billing-soft: #eef6f6;
            --billing-danger: #dc3545;
            --billing-success: #16865f;
        }

        .billing-page {
            background: var(--billing-bg);
            min-height: calc(100vh - 70px);
            padding: 16px 16px 28px;
        }

        .billing-shell {
            width: 100%;
            max-width: none;
            margin: 0 auto;
        }

        .billing-hero {
            position: relative;
            overflow: hidden;
            background: linear-gradient(135deg, var(--billing-navy) 0%, #123f49 45%, var(--billing-teal) 100%);
            color: #fff;
            border-radius: 7px;
            padding: 20px 22px;
            box-shadow: 0 12px 30px rgba(7, 27, 52, .18);
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 20px;
        }

        .billing-hero:before {
            content: "";
            position: absolute;
            inset: 0;
            background: radial-gradient(circle at 75% 18%, rgba(255,255,255,.16), transparent 28%);
            pointer-events: none;
        }

        .billing-hero-left, .billing-hero-actions {
            position: relative;
            z-index: 1;
        }

        .billing-title-row {
            display: flex;
            align-items: center;
            gap: 14px;
            margin-bottom: 6px;
        }

        .billing-title-row i {
            font-size: 28px;
            color: #fff;
        }

        .billing-hero h1 {
            margin: 0;
            font-size: 27px;
            line-height: 1.2;
            font-weight: 800;
            letter-spacing: .2px;
        }

        .billing-hero p {
            margin: 0;
            color: rgba(255,255,255,.9);
            font-size: 13px;
            font-weight: 600;
        }

        .billing-chip {
            display: inline-flex;
            align-items: center;
            gap: 8px;
            border: 1px solid rgba(255,255,255,.22);
            color: #fff;
            background: rgba(255,255,255,.12);
            padding: 10px 16px;
            border-radius: 24px;
            font-size: 12px;
            font-weight: 800;
            white-space: nowrap;
        }

        .billing-card {
            background: var(--billing-card);
            border: 1px solid var(--billing-border);
            border-radius: 7px;
            box-shadow: 0 10px 24px rgba(15, 23, 42, .06);
            margin-top: 12px;
        }

        .billing-tabs {
            display: flex;
            gap: 4px;
            padding: 0 18px;
            border-bottom: 1px solid var(--billing-border);
            background: #f9fbfe;
            border-radius: 7px 7px 0 0;
        }

        .billing-tab {
            position: relative;
            display: inline-flex;
            align-items: center;
            gap: 8px;
            padding: 14px 16px;
            color: #52647d;
            font-size: 12px;
            font-weight: 800;
        }

        .billing-tab.active {
            background: #fff;
            color: #006a65;
        }

        .billing-tab.active:before {
            content: "";
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            height: 3px;
            background: #008577;
            border-radius: 10px 10px 0 0;
        }

        .billing-section {
            padding: 18px;
        }

        .billing-toolbar {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 18px;
            margin-bottom: 16px;
        }

        .billing-section-title h2 {
            margin: 0 0 3px;
            font-size: 14px;
            font-weight: 800;
            color: var(--billing-text);
        }

        .billing-section-title p {
            margin: 0;
            font-size: 12px;
            color: var(--billing-muted);
        }

        .template-links {
            display: flex;
            align-items: center;
            gap: 8px;
            flex-wrap: wrap;
            justify-content: flex-end;
        }

        .template-links a {
            display: inline-flex;
            align-items: center;
            gap: 6px;
            padding: 8px 12px;
            border: 1px solid #cfe4e4;
            background: var(--billing-soft);
            color: #0c6b63 !important;
            border-radius: 18px;
            font-size: 12px;
            font-weight: 800;
            text-decoration: none !important;
        }

        .billing-form-grid {
            display: grid;
            grid-template-columns: repeat(4, minmax(0, 1fr));
            gap: 14px;
            align-items: end;
        }

        .billing-field label {
            display: block;
            margin-bottom: 7px;
            color: #344256;
            font-size: 12px;
            font-weight: 800 !important;
            border: 0 !important;
        }

        .billing-field input,
        .billing-field select,
        .billing-field .form-control {
            width: 100%;
            height: 40px;
            border: 1px solid #d7e0ea;
            border-radius: 7px;
            background: #fff;
            color: var(--billing-text);
            font-size: 13px !important;
            padding: 8px 11px;
            box-shadow: none;
            transition: border-color .2s ease, box-shadow .2s ease;
        }

        .billing-field input:focus,
        .billing-field select:focus {
            border-color: #198d80;
            box-shadow: 0 0 0 3px rgba(35,133,118,.13);
            outline: none;
        }

        .billing-actions {
            display: flex;
            justify-content: flex-end;
            align-items: center;
            gap: 10px;
            margin-top: 16px;
            flex-wrap: wrap;
        }

        .billing-btn {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            gap: 8px;
            height: 40px;
            padding: 0 16px;
            border-radius: 7px;
            border: 1px solid transparent;
            font-size: 13px;
            font-weight: 800;
            cursor: pointer;
            transition: transform .15s ease, box-shadow .15s ease, background .15s ease;
        }

        .billing-btn:hover {
            transform: translateY(-1px);
            box-shadow: 0 8px 16px rgba(15,23,42,.12);
        }

        .billing-btn-primary {
            background: linear-gradient(135deg, #0b7285, #15977f);
            color: #fff;
        }

        .billing-btn-success {
            background: linear-gradient(135deg, #16865f, #20a472);
            color: #fff;
        }

        .billing-btn-danger {
            background: #fff;
            border-color: #f1b8be;
            color: var(--billing-danger);
        }

        .billing-table-card {
            margin-top: 14px;
            overflow: hidden;
        }

        .billing-table-header {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 15px;
            padding: 15px 18px;
            border-bottom: 1px solid var(--billing-border);
            background: #fbfcff;
        }

        .billing-table-title {
            display: flex;
            align-items: center;
            gap: 10px;
            color: var(--billing-text);
            font-size: 15px;
            font-weight: 800;
        }

        #impbill_alert {
            color: var(--billing-danger) !important;
            font-size: 13px !important;
            font-weight: 800 !important;
        }

        .billing-table-wrap {
            padding: 16px;
            overflow: auto;
        }

        .table.dataTable,
        .billing-table-wrap table.table {
            border-collapse: separate !important;
            border-spacing: 0;
            width: 100% !important;
            border: 1px solid #dfe7f1;
            border-radius: 8px;
            overflow: hidden;
            background: #fff;
        }

        .table.dataTable th,
        .billing-table-wrap table.table thead th {
            background: #eef4f8 !important;
            color: #172033 !important;
            font-size: 12px;
            font-weight: 800;
            white-space: nowrap;
            border-bottom: 1px solid #dbe5ef !important;
        }

        .table.dataTable tr td,
        .billing-table-wrap table.table tbody td {
            background: #fff !important;
            color: #2d3748;
            font-size: 12px;
            vertical-align: middle;
            border-color: #edf2f7 !important;
        }

        .dataTables_length,
        .dataTables_info {
            float: left !important;
            color: var(--billing-muted);
            font-size: 12px;
        }

        div.dt-buttons {
            position: static;
            padding-left: 20px;
            float: left;
        }

        .buttons-excel,
        .buttons-html5 {
            color: #fff !important;
            background: linear-gradient(135deg, #0b7285, #15977f) !important;
            border: 0 !important;
            border-radius: 7px !important;
            font-weight: 800 !important;
            margin: 0 8px !important;
            padding: 7px 12px !important;
        }

        .btn-loader {
            width: 18px;
            height: 18px;
            border: 2px solid #fff;
            border-top: 2px solid transparent;
            border-radius: 50%;
            display: inline-block;
            animation: billingSpin .7s linear infinite;
            margin-left: 8px;
        }

        @keyframes billingSpin { to { transform: rotate(360deg); } }

        .loading {
            display: none;
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            background: rgba(255,255,255,.94);
            border: 1px solid var(--billing-border);
            border-radius: 14px;
            width: 190px;
            min-height: 160px;
            z-index: 99999;
            text-align: center;
            padding: 22px 16px;
            box-shadow: 0 16px 36px rgba(15,23,42,.18);
        }

        .loading img { max-width: 64px; }

        @media (max-width: 1100px) {
            .billing-form-grid { grid-template-columns: repeat(2, minmax(0, 1fr)); }
        }

        @media (max-width: 700px) {
            .billing-page { padding: 10px; }
            .billing-hero { align-items: flex-start; flex-direction: column; }
            .billing-hero h1 { font-size: 22px; }
            .billing-toolbar { align-items: flex-start; flex-direction: column; }
            .template-links { justify-content: flex-start; }
            .billing-form-grid { grid-template-columns: 1fr; }
            .billing-actions { justify-content: stretch; }
            .billing-btn { width: 100%; }
        }
    </style>

    <script>

        $(document).ready(function () {

            ipmbill_BindYear();

        });

        window.onload = function () {
            document.getElementById('impbill_attachment').addEventListener('change', getFileName);
        }

        const getFileName = (event) => {
            const files = event.target.files;
            var file = files[0];
            document.getElementById("file_impbilling").value = files[0].name;

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


    </script>

    <link rel="stylesheet" href="https://cdn.datatables.net/fixedheader/3.4.0/css/fixedHeader.dataTables.min.css">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons/font/bootstrap-icons.css" rel="stylesheet">

    <script src="https://cdn.datatables.net/fixedheader/3.4.0/js/dataTables.fixedHeader.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="bdm-page">
<input id="file_impbilling" style="display: none;" />

    <div class="loading" id="load1">
        <img src="../images/Load_1.gif" alt="Loading" />
        <div style="font-size: 12px; font-weight: bold; margin-top: 10px; color: #172033;">One moment, please . . . .</div>
    </div>

    <div class="billing-page">
        <div class="billing-shell">
            <div class="billing-hero">
                <div class="billing-hero-left">
                    <div class="billing-title-row">
                        <i class="fas fa-file-invoice-dollar"></i>
                        <h1>Import Billing Excel</h1>
                    </div>
                    <p>Import and manage Griffin, Stewart AVM and Stewart CDA excels data.</p>
                </div>
                <div class="billing-hero-actions">
                    <span class="billing-chip"><i class="fas fa-layer-group"></i> Billing Operations</span>
                </div>
            </div>

            <div class="billing-card">
           
                <div class="billing-section">
                    <div class="billing-toolbar">
                        <div class="billing-section-title">
                            <h2>Import Billing Data</h2>
                            <p>Select billing type, period, and upload the matching Excel template.</p>
                        </div>
                        <div class="template-links">
                            <a href="Griffin.xlsx"><i class="fas fa-download"></i> Griffin</a>
                            <a href="StewartAVM.xlsx"><i class="fas fa-download"></i> Stewart AVM</a>
                            <a href="StewartCDA.xlsx"><i class="fas fa-download"></i> Stewart CDA</a>
                        </div>
                    </div>

                    <div class="billing-form-grid">
                        <div class="billing-field">
                            <label>Billing Type</label>
                            <select id="impbill_type" onchange="return impbill_changetype();">
                                <option value="">Select Type</option>
                                <option value="Griffin">Griffin</option>
                                <option value="StewartAVM">Stewart-AVM</option>
                                <option value="StewartCDA">Stewart-CDA</option>
                                <option value="Billing">Billing</option>
                            </select>
                        </div>

                        <div class="billing-field">
                            <label>Month</label>
                            <select id="impbill_month" onchange="return getMonthDateRange();">
                                <option value="0">Select</option>
                                <option value="1">January</option>
                                <option value="2">February</option>
                                <option value="3">March</option>
                                <option value="4">April</option>
                                <option value="5">May</option>
                                <option value="6">June</option>
                                <option value="7">July</option>
                                <option value="8">August</option>
                                <option value="9">September</option>
                                <option value="10">October</option>
                                <option value="11">November</option>
                                <option value="12">December</option>
                            </select>
                        </div>

                        <div class="billing-field">
                            <label>Year</label>
                            <select id="impbill_year" onchange="return getMonthDateRange();"></select>
                        </div>

                        <div class="billing-field">
                            <label>Billing Period</label>
                            <input type="text" id="impbill_billingPeriod" placeholder="Auto generated period" />
                        </div>

                        <div class="billing-field">
                            <label>Excel File</label>
                            <input type="file" id="impbill_attachment" class="form-control file-input" accept=".xlsx" />
                        </div>
                    </div>

                    <div class="billing-actions">
                        <button type="button" id="impbill_import" class="billing-btn btn-warning" onclick="return impbill_uploadData();">
                            <span class="btn-text"><i class="bi bi-cloud-upload"></i> Upload</span><span class="btn-loader d-none"></span>
                        </button>
                        <button type="button" class="billing-btn billing-btn-danger" onclick="return impbill_clearData();">
                            <i class="bi bi-x-circle"></i> Clear Uploaded Data
                        </button>
                        <button type="submit" id="impbill_verify" class="billing-btn billing-btn-success" onclick="return impbill_VerifyData();">
                            <i class="bi bi-check-circle"></i> Verify &amp; Submit
                        </button>
                    </div>
                </div>
            </div>

            <div class="billing-card billing-table-card">
                <div class="billing-table-header">
                    <div class="billing-table-title">
                        <i class="fas fa-table"></i>
                        <span id="impbill_tableheader">Uploaded Billing Records</span>
                    </div>
                    <label id="impbill_alert"></label>
                </div>

                <div class="billing-table-wrap" id="impbill_tablediv">
                    <table id="table_griffin" class="table" style="width: 100%; display: none;">
                        <thead></thead>
                        <tbody></tbody>
                    </table>
                    <br />
                    <table id="table_StewartAVM" class="table" style="width: 100%; display: none;">
                        <thead></thead>
                        <tbody></tbody>
                    </table>
                    <br />
                    <table id="table_StewartCDA" class="table" style="width: 100%; display: none;">
                        <thead></thead>
                        <tbody></tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>
    </div>
</asp:Content>


