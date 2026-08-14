<%@ Page Title="" Language="C#" MasterPageFile="~/BDM/BDM.Master" AutoEventWireup="true" CodeBehind="BillToMaster.aspx.cs" Inherits="Canopy_Billing.BDM.BillToMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        :root {
            --erp-primary: #2563eb;
            --erp-primary-dark: #1d4ed8;
            --erp-primary-soft: #eff6ff;
            --erp-success: #16a34a;
            --erp-surface: #ffffff;
            --erp-body: #f5f7fb;
            --erp-border: #e5e7eb;
            --erp-border-strong: #d1d5db;
            --erp-text: #111827;
            --erp-muted: #6b7280;
            --erp-shadow: 0 16px 40px rgba(15, 23, 42, .08);
            --erp-radius: 16px;
        }

        .content-wrapper,
        .right-side {
            background: var(--erp-body) !important;
        }

        .billto-page {
            padding: 16px 18px 28px;
        }

        .erp-hero {
            display: flex;
            justify-content: space-between;
            align-items: center;
            gap: 16px;
            margin-bottom: 16px;
            padding: 18px 20px;
            border: 1px solid #dbeafe;
            border-radius: var(--erp-radius);
            background: linear-gradient(135deg, #ffffff 0%, #eff6ff 100%);
            box-shadow: 0 10px 28px rgba(37, 99, 235, .08);
        }

        .erp-hero__eyebrow {
            margin: 0 0 4px;
            color: var(--erp-primary);
            font-size: 11px;
            font-weight: 800;
            letter-spacing: .1em;
            text-transform: uppercase;
        }

        .erp-hero__title {
            margin: 0;
            color: var(--erp-text);
            font-size: 22px;
            font-weight: 800;
            line-height: 1.25;
        }

        .erp-hero__subtitle {
            margin: 5px 0 0;
            color: var(--erp-muted);
            font-size: 13px;
        }

        .erp-hero__icon {
            width: 46px;
            height: 46px;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            flex: 0 0 46px;
            color: #ffffff;
            border-radius: 14px;
            background: linear-gradient(135deg, var(--erp-primary), #38bdf8);
            box-shadow: 0 12px 26px rgba(37, 99, 235, .25);
            font-size: 18px;
        }

        .erp-panel {
            margin-bottom: 16px;
            border: 1px solid var(--erp-border);
            border-radius: var(--erp-radius);
            background: var(--erp-surface);
            box-shadow: var(--erp-shadow);
            overflow: hidden;
        }

        .erp-panel__header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            gap: 12px;
            padding: 14px 18px;
            border-bottom: 1px solid var(--erp-border);
            background: #fbfdff;
        }

        .erp-panel__title {
            margin: 0;
            color: var(--erp-text);
            font-size: 15px;
            font-weight: 800;
        }

        .erp-panel__hint {
            margin: 3px 0 0;
            color: var(--erp-muted);
            font-size: 12px;
        }

        .erp-panel__body {
            padding: 18px;
        }

        .erp-form-grid {
            display: grid;
            grid-template-columns: repeat(4, minmax(0, 1fr));
            gap: 14px 16px;
        }

        .erp-field {
            min-width: 0;
        }

        .erp-field--wide {
            grid-column: span 2;
        }

        .erp-label {
            display: block;
            margin-bottom: 6px;
            color: #374151;
            font-size: 12px;
            font-weight: 700 !important;
            border: 0 !important;
        }

            .erp-label .required {
                color: #dc2626;
            }

        .erp-input.form-control {
            width: 100%;
            height: 38px;
            padding: 8px 11px;
            border: 1px solid var(--erp-border-strong);
            border-radius: 10px;
            color: var(--erp-text);
            background: #ffffff;
            font-size: 13px !important;
            box-shadow: none;
            transition: border-color .16s ease, box-shadow .16s ease;
        }

            .erp-input.form-control:focus {
                border-color: var(--erp-primary);
                box-shadow: 0 0 0 3px rgba(37, 99, 235, .12);
                outline: none;
            }

        .erp-actions {
            display: flex;
            justify-content: flex-end;
            align-items: center;
            gap: 10px;
            margin-top: 16px;
            padding-top: 14px;
            border-top: 1px solid var(--erp-border);
        }

        .erp-btn {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            gap: 8px;
            min-width: 112px;
            height: 38px;
            padding: 0 16px;
            border: 0;
            border-radius: 10px;
            font-size: 13px;
            font-weight: 800;
            letter-spacing: .01em;
            box-shadow: none;
            transition: transform .16s ease, box-shadow .16s ease, background .16s ease;
        }

            .erp-btn:hover {
                transform: translateY(-1px);
            }

        .erp-btn--primary {
            color: #ffffff !important;
            background: linear-gradient(135deg, var(--erp-primary), var(--erp-primary-dark));
            box-shadow: 0 10px 20px rgba(37, 99, 235, .2);
        }

            .erp-btn--primary:hover,
            .erp-btn--primary:focus {
                color: #ffffff !important;
                background: linear-gradient(135deg, #1d4ed8, #1e40af);
            }

        .erp-table-wrap {
            width: 100%;
            overflow-x: auto;
            border: 1px solid var(--erp-border);
            border-radius: 14px;
            background: #ffffff;
        }

        #BillToMaster_table {
            width: 100% !important;
            margin: 0 !important;
            border-collapse: separate !important;
            border-spacing: 0;
        }

            #BillToMaster_table.table-bordered,
            #BillToMaster_table.table-bordered td,
            #BillToMaster_table.table-bordered th {
                border-color: var(--erp-border) !important;
            }

            #BillToMaster_table thead th,
            .table.dataTable#BillToMaster_table thead th,
            .table.dataTable#BillToMaster_table th {
                padding: 12px 14px !important;
                border-bottom: 1px solid var(--erp-border-strong) !important;
                color: #1f2937 !important;
                background: #f8fafc !important;
                font-size: 12px;
                font-weight: 800;
                text-align: left !important;
                white-space: nowrap;
            }

                #BillToMaster_table thead th:first-child,
                #BillToMaster_table tbody td:first-child {
                    text-align: center !important;
                    width: 76px;
                }

            #BillToMaster_table tbody td,
            .table.dataTable#BillToMaster_table tbody td {
                padding: 11px 14px !important;
                color: #374151;
                background: #ffffff !important;
                font-size: 12.5px;
                vertical-align: middle;
            }

            #BillToMaster_table tbody tr:hover td {
                background: #f9fafb !important;
            }

        .dataTables_wrapper {
            padding: 0;
        }

            .dataTables_wrapper .row:first-child,
            .dataTables_wrapper .row:last-child {
                margin: 0 0 12px;
                align-items: center;
            }

        .dataTables_length,
        .dataTables_info {
            float: left !important;
            color: var(--erp-muted);
            font-size: 12px;
        }

        .dataTables_filter {
            float: right !important;
            text-align: right;
            font-size: 12px;
        }

            .dataTables_filter input,
            .dataTables_length select {
                min-height: 32px;
                border: 1px solid var(--erp-border-strong);
                border-radius: 8px;
                font-size: 12px;
                outline: none;
            }

        div.dt-buttons {
            position: static;
            float: left;
            padding-left: 12px;
        }

        .buttons-excel,
        .buttons-html5 {
            margin: 0 6px 8px 0 !important;
            padding: 7px 12px !important;
            border: 0 !important;
            border-radius: 9px !important;
            color: #ffffff !important;
            background: linear-gradient(135deg, var(--erp-success), #22c55e) !important;
            font-size: 12px !important;
            font-weight: 800 !important;
            box-shadow: 0 8px 16px rgba(22, 163, 74, .18) !important;
        }

        .loading {
            display: none;
            position: fixed;
            inset: 0;
            width: auto;
            height: auto;
            margin: 0;
            z-index: 99999;
            background: rgba(15, 23, 42, .28);
            opacity: 1;
            border-radius: 0;
            text-align: center;
        }

        .loading__box {
            position: absolute;
            top: 50%;
            left: 50%;
            width: 180px;
            padding: 22px 18px;
            transform: translate(-50%, -50%);
            border: 1px solid rgba(255, 255, 255, .7);
            border-radius: 16px;
            background: #ffffff;
            box-shadow: 0 20px 48px rgba(15, 23, 42, .18);
        }

            .loading__box img {
                max-width: 54px;
                height: auto;
            }

        .loading__text {
            margin-top: 10px;
            color: #374151;
            font-size: 12px;
            font-weight: 800;
        }

        label:not(.form-check-label):not(.custom-file-label) {
            font-weight: 700 !important;
            border: none !important;
        }

        @media (max-width: 1199px) {
            .erp-form-grid {
                grid-template-columns: repeat(2, minmax(0, 1fr));
            }
        }

        @media (max-width: 767px) {
            .billto-page {
                padding: 12px;
            }

            .erp-hero,
            .erp-panel__header {
                align-items: flex-start;
                flex-direction: column;
            }

            .erp-form-grid {
                grid-template-columns: 1fr;
            }

            .erp-field--wide {
                grid-column: span 1;
            }

            .erp-actions {
                justify-content: stretch;
            }

            .erp-btn {
                width: 100%;
            }
        }

        .page-header {
            background: linear-gradient(135deg, #0f2744 0%, #1f6f73 100%);
            border-radius: 12px;
            padding: 22px 28px;
            margin-bottom: 18px;
            box-shadow: 0 4px 12px rgba(0,0,0,.08);
        }

        .header-content {
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .header-left {
            display: flex;
            align-items: center;
            gap: 16px;
        }

        .header-icon {
            width: 52px;
            height: 52px;
            background: rgba(255,255,255,.12);
            border-radius: 10px;
            display: flex;
            align-items: center;
            justify-content: center;
            color: #fff;
            font-size: 24px;
        }

        .header-left h1 {
            margin: 0;
            color: #fff;
            font-size: 18px;
            font-weight: 700;
            line-height: 1.2;
        }

        .header-left p {
            margin: 6px 0 0;
            color: rgba(255,255,255,.85);
            font-size: 14px;
        }

        .module-badge {
            display: inline-block;
            padding: 10px 18px;
            border-radius: 24px;
            color: #fff;
            font-weight: 600;
            background: rgba(255,255,255,.12);
            border: 1px solid rgba(255,255,255,.2);
        }
    </style>

    <script>
        $(document).ready(function () {
            BindBillToMaster();
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="loading" id="load1">
        <div class="loading__box">
            <img src="../images/Load_1.gif" alt="Loading" />
            <div class="loading__text">One moment, please...</div>
        </div>
    </div>

    <div class="billto-page">
        <div class="page-header">
            <div class="header-content">
                <div class="header-left">
                    <div class="header-icon">
                        <i class="fas fa-file-invoice-dollar"></i>
                    </div>
                    <div>
                        <h1>Bill To Master</h1>
                        <p>Create, manage and maintain Bill To mappings for billing operations.</p>
                    </div>
                </div>

                <div class="header-right">
                    <span class="module-badge">Billing Operations</span>
                </div>
            </div>
        </div>

        <section class="erp-panel">
            <div class="erp-panel__header">
                <div>
                    <h2 class="erp-panel__title">Add Bill To Details</h2>
                    <p class="erp-panel__hint">Enter the required billing master information and save it to the list.</p>
                </div>
            </div>
            <div class="erp-panel__body">
                <div class="erp-form-grid">
                    <div class="erp-field erp-field--wide">
                        <label for="txtBillTo" class="erp-label">Bill To <span class="required">*</span></label>
                        <input type="text" id="txtBillTo" class="form-control erp-input" placeholder="Enter Bill To" autocomplete="off" />
                    </div>
                    <div class="erp-field erp-field--wide">
                        <label for="txtbuyerName" class="erp-label">Buyer Name <span class="required">*</span></label>
                        <input type="text" id="txtbuyerName" class="form-control erp-input" placeholder="Enter buyer name" autocomplete="off" />
                    </div>
                    <div class="erp-field erp-field--wide">
                        <label for="txttransactionIdentifier" class="erp-label">Transaction Identifier <span class="required">*</span></label>
                        <input type="text" id="txttransactionIdentifier" class="form-control erp-input" placeholder="Enter transaction identifier" autocomplete="off" />
                    </div>
                    <div class="erp-field erp-field--wide">
                        <label for="txtsellerName" class="erp-label">Seller Name <span class="required">*</span></label>
                        <input type="text" id="txtsellerName" class="form-control erp-input" placeholder="Enter seller name" autocomplete="off" />
                    </div>
                </div>

                <div class="erp-actions">
                    <button type="button" onclick="return saveBillToMaster();" class="btn erp-btn erp-btn--primary">
                        <i class="fas fa-plus"></i>
                        Add Record
                   
                    </button>
                </div>
            </div>
        </section>

        <section class="erp-panel">
            <div class="erp-panel__header">
                <div>
                    <h2 class="erp-panel__title">Bill To Master List</h2>
                    <p class="erp-panel__hint">Review all configured billing master records.</p>
                </div>
            </div>
            <div class="erp-panel__body">
                <div class="erp-table-wrap">
                    <table class="table table-bordered" id="BillToMaster_table">
                        <thead>
                            <tr>
                                <th class="sort border-top ps-3">Sr. #</th>
                                <th class="sort border-top">Bill To</th>
                                <th class="sort border-top">Seller Name</th>
                                <th class="sort border-top">Buyer Name</th>
                                <th class="sort border-top">Transaction Identifier</th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                    </table>
                </div>
            </div>
        </section>
    </div>
</asp:Content>
