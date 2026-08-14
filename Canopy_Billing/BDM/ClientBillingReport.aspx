<%@ Page Title="" Language="C#" MasterPageFile="~/BDM/BDM.Master" AutoEventWireup="true" CodeBehind="ClientBillingReport.aspx.cs" Inherits="Canopy_Billing.BDM.ClientBillingReport" %>

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
            cbr_bindyear();
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
                        <h6 class="m-0"><i class="fas fa-copy"></i>&nbsp;&nbsp;<b>Tentative Costing Report</b></h6>
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
                                <select id="cbr_month" name="cbr_month" class="form-control">
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
                                <select id="cbr_year" name="cbr_year" class="form-control">
                                    <option value="">Select</option>
                                </select>
                            </td>



                            <td style="width: 100px;">
                                <button id="cbr_btnShow" class="btn btn-primary" onclick="return cbr_bindClientReportgrid();">Show</button>
                                <%--<button id="cbr_btnexport" class="btn btn-primary" onclick="return cbdr_exporttoexcel();">Export to excel</button>--%>
                            </td>
                        </tr>
                    </table>
                    <hr />
                    <table class="table table-bordered" id="canopybilling_table" style="width: 100%;"></table>
                </div>
            </div>

        </div>
</asp:Content>

