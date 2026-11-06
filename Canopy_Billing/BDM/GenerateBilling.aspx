<%@ Page Title="" Language="C#" MasterPageFile="~/BDM/BDM.Master" AutoEventWireup="true" CodeBehind="GenerateBilling.aspx.cs" Inherits="Canopy_Billing.BDM.GenerateBilling" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .loading {
            display: none;
            position: fixed;
            top: 350px;
            left: 50%;
            margin-top: -96px;
            margin-left: -96px;
            /*  background-color: #ccc;*/
            opacity: .85;
            border-radius: 25px;
            width: 192px;
            height: 192px;
            z-index: 99999;
        }

        .dataTables_length, .dataTables_info {
            float: left !important;
        }

        label:not(.form-check-label):not(.custom-file-label) {
            font-weight: normal !important;
            border: none !important;
        }

        div.dt-buttons {
            position: static;
            padding-left: 50px;
            float: left;
        }

        .buttons-excel, .buttons-html5 {
            color: #fff;
            box-shadow: none;
            background: linear-gradient(to right, #ffbf96, #fe7096);
            border: 0;
            font-weight: bold;
            margin: 0px 10px;
        }

        .table.dataTable th {
            background: linear-gradient(to bottom, #007bff, 3%, #fff) !important;
            color: #000;
        }

        .table.dataTable tr td {
            background: none !important;
            background-color: #fff !important;
        }

        .dataTables_scroll {
            overflow: auto;
        }

        :root {
            --erp-btprimary: #0f766e;
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

        .btn-billprimary {
            border-color: var(--erp-primary) !important;
            background: linear-gradient(135deg, var(--erp-primary), var(--erp-accent)) !important;
            color: #fff !important;
        }

        .col-lg-12 {
            position: relative;
            width: 100%;
            padding-right: 0px;
            padding-left: 0px;
        }

        .erp-tabs .nav-link.active {
            color: var(--erp-primary-dark) !important;
            background: #fff !important;
            border-top: 3px solid var(--erp-btprimary) !important;
        }
    </style>

    <script>
        $(document).ready(function () {
            BillingDecription_bindyear();
        });

    </script>
    <script src="../Scripts/Functions/GenerateBilling.js" type="text/javascript"></script>
    <link rel="stylesheet" href="https://cdn.datatables.net/buttons/2.4.2/css/buttons.dataTables.min.css">
    <script src="https://cdn.datatables.net/buttons/2.4.2/js/dataTables.buttons.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.10.1/jszip.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/2.4.2/js/buttons.html5.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/xlsx/0.18.5/xlsx.full.min.js"></script>
     <script src="https://cdn.jsdelivr.net/npm/xlsx-js-style@1.2.0/dist/xlsx.min.js"></script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="bdm-page">
        <div class="loading" id="load1">
            <div class="loading__box">
                <img src="../images/Load_1.gif" alt="Loading" />
                <div class="loading__text">One moment, please...</div>
            </div>
        </div>
        <div class="content-header">
            <div class="container">
                <div class="erp-hero">
                    <div>
                        <h4><i class="fas fa-receipt"></i>&nbsp;&nbsp;Billing Description</h4>
                        <p>Display billing summary & details</p>
                    </div>
                    <span class="erp-hero-badge">Billing Description</span>
                </div>

            </div>
        </div>
        <div class="col-lg-12">
            <div class="card">
                <div class="card-body">
                    <h5 class="card-title"></h5>
                    <table class="table">
                        <tr>
                            <td style="width: 50px;"><b>Month:</b></td>
                            <td style="width: 100px;">
                                <select id="billingdecription_month" name="billingdecription_month" class="form-control">
                                    <option value="">Select</option>
                                    <option value="January">January</option>
                                    <option value="February">February</option>
                                    <option value="March">March</option>
                                    <option value="April">April</option>
                                    <option value="May">May</option>
                                    <option value="June">June</option>
                                    <option value="July">July</option>
                                    <option value="August">August</option>
                                    <option value="September">September</option>
                                    <option value="October">October</option>
                                    <option value="November">November</option>
                                    <option value="December">December</option>
                                </select>
                            </td>
                            <td style="width: 50px;">
                                <b>Year:</b>
                            </td>
                            <td style="width: 100px;">
                                <select id="billingdecription_year" name="billingdecription_year" class="form-control">
                                    <option value="">Select</option>
                                </select>
                            </td>
                            <td style="width: 150px;">
                                <button id="btnbillingdecription" class="btn btn-primary" onclick="return ShowBillingDecriptionData();">Show</button>
                                <button id="btngeneratebilling" class="btn btn-billprimary">Generate Billing</button>
                                                                <button id="btngeneratebillingsendemail" class="btn btn-billprimary">Send Email</button>

                            </td>
                        </tr>
                    </table>
                    <hr />
                    <div class="col-lg-12">
                        <div class="card-header p-0 pt-1">
                            <ul class="nav nav-tabs erp-tabs" id="custom-tabs-one-tab" role="tablist">
                                <li class="nav-item">
                                    <a class="nav-link active" id="custom-tabs-one-home-tab" data-toggle="pill" href="#custom-tabs-one-home" role="tab" aria-controls="custom-tabs-one-home" aria-selected="true">Summary</a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" id="custom-tabs-one-profile-tab" data-toggle="pill" href="#custom-tabs-one-profile" role="tab" aria-controls="custom-tabs-one-profile" aria-selected="false">Details</a>
                                </li>

                            </ul>
                        </div>
                        <div class="card-body">
                            <div class="tab-content" id="custom-tabs-one-tabContent">
                                <div class="tab-pane fade show active " id="custom-tabs-one-home" role="tabpanel" aria-labelledby="custom-tabs-one-home-tab">

                                    <table class="table" id="billingdecription_Summarytable" style="width: 100%;">
                                        <thead>
                                            <tr>
                                                <th class="sort border-top" style="text-wrap: nowrap;">Sr. #</th>
                                                <th class="sort border-top" style="text-wrap: nowrap;">Client</th>
                                                <th class="sort border-top" style="text-wrap: nowrap; display: none;">Project ID</th>
                                                <th class="sort border-top" style="text-wrap: nowrap;">Project Name</th>
                                                <th class="sort border-top" style="text-wrap: nowrap;">Billing Period</th>
                                                <th class="sort border-top" style="text-wrap: nowrap;">Billing Sent Date</th>
                                                <th class="sort border-top" style="text-wrap: nowrap;">Loan Count</th>
                                            </tr>
                                        </thead>
                                        <tbody></tbody>
                                    </table>
                                    <div class="card-header mb-3 p-3">
                                        <h6 class="m-0"><i class="fas fa-copy"></i>&nbsp;&nbsp;<b>Project Wise Loan Details</b></h6>
                                    </div>
                                    <div class="table-responsive">
                                        <table class="table" id="billingdecription_table" style="width: 100%;">
                                            <thead>
                                                <tr>
                                                    <th class="sort border-top text-center" style="width: 40px; text-wrap: nowrap;">
                                                        <input type="checkbox" id="selectAllCheckbox" />
                                                    </th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Sr. #</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap; display: none;">ID</th>

                                                    <th class="sort border-top" style="text-wrap: nowrap;">Loan ID</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Created Date</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Submitted Date</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Snapshot Taken Date</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Bill To</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Buyer Name</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Seller Name</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Script Name</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Transaction Identifier</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">DSCR</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Lien Position</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">B1 Self Employment Flag</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">B2 Self Employment Flag</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Occupancy</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Number of Mortgaged Properties</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Note Type</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Note Date</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Number of Units</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Process</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Project ID</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Billing Type</th>
                                                </tr>
                                            </thead>
                                            <tbody></tbody>
                                        </table>
                                    </div>
                                </div>
                                <div class="tab-pane fade" id="custom-tabs-one-profile" role="tabpanel" aria-labelledby="custom-tabs-one-profile-tab">
                                    <div class="table-responsive">
                                        <table class="table" id="billingdecription_detailstable" style="width: 100%;">
                                            <thead>
                                                <tr>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Sr. #</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Loan ID</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Created Date</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Submitted Date</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Snapshot Taken Date</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Bill To</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Buyer Name</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Seller Name</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Script Name</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Transaction Identifier</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">DSCR</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Lien Position</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">B1 Self Employment Flag</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">B2 Self Employment Flag</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Occupancy</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Number of Mortgaged Properties</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Note Type</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Note Date</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Number of Units</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Process</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Project ID</th>
                                                    <th class="sort border-top" style="text-wrap: nowrap;">Billing Type</th>
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
        </div>
    </div>
</asp:Content>



