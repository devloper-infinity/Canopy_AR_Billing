<%@ Page Title="" Language="C#" MasterPageFile="~/BDM/BDM.Master" AutoEventWireup="true" CodeBehind="ClientBillingSummary.aspx.cs" Inherits="Canopy_Billing.BDM.ClientBillingSummary" %>

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
            /*     background-color: #28a745;
            border-color: #28a745;*/
            box-shadow: none;
            background: linear-gradient(to right, #ffbf96, #fe7096);
            border: 0;
            font-weight: bold;
            margin: 0px 10px;
        }

        .table.dataTable th {
            background: linear-gradient(to bottom, #cbd0dd, 3%, #fff) !important;
            /*background-color:#e3e6ed!important;*/
            color: #000;
        }

        .table.dataTable tr td {
            background: none !important;
            background-color: #fff !important;
        }

        /*.form-control {
            font-size: 11px !important;
        }*/
    </style>
    <script>
        $(document).ready(function () {
            cbs_bindyear();
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
        <div class="content-header">
            <div class="container">
                <div class="row mb-2 callout callout-info">
                    <div class="col-sm-6">
                        <h6 class="m-0"><i class="fas fa-copy"></i>&nbsp;&nbsp;<b>Client Billing Summary</b></h6>
                    </div>
                </div>
            </div>
            <!-- /.container-fluid -->
        </div>
        <div class="col-lg-12">
            <div class="card">
                <div class="card-body">
                    <h5 class="card-title"></h5>
                    <table class="table">
                        <tr>
                            <td style="width: 50px;"><b>Month:</b></td>
                            <td style="width: 150px;">
                                <select id="cbs_month" name="cbs_month" class="form-control">
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
                            <td style="width: 150px;">
                                <select id="cbs_year" name="cbs_year" class="form-control">
                                    <option value="">Select</option>
                                </select>
                            </td>
                            <td>
                                <button id="cbs_btnShow" class="btn btn-primary" onclick="return cbs_bindGrid();">Show</button>
                            </td>
                        </tr>
                    </table>
                    <hr />
                    <table class="table" id="cbs_table" style="width: 100%;">
                        <thead>
                            <tr>
                                <th class="sort border-top" style="text-wrap: nowrap; text-align: center;">Billing Period</th>
                                <th class="sort border-top" style="text-wrap: nowrap; text-align: center;">Client</th>
                                <th class="sort border-top" style="text-wrap: nowrap; text-align: center;">Process</th>
                                <th class="sort border-top" style="text-wrap: nowrap; text-align: center;">Description</th>
                                <th class="sort border-top" style="text-wrap: nowrap; text-align: center;">Loan Count</th>
                                <th class="sort border-top" style="text-wrap: nowrap; text-align: center;">Price/Loan</th>
                                <th class="sort border-top" style="text-wrap: nowrap; text-align: center;">Total Amount</th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                    </table>
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
    </div>
</asp:Content>

