<%@ Page Title="" Language="C#" MasterPageFile="~/BDM/BDM.Master" AutoEventWireup="true" CodeBehind="CanopyData.aspx.cs" Inherits="Vendor_Portal.BDM.CanopyData" %>

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

        /*.form-control {
            font-size: 11px !important;
        }*/
    </style>
    <script>
        $(document).ready(function () {
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="bdm-page">
    <div class="loading" id="load1">
        <img src="../images/Load_1.gif" />
        <div style="font-size: 12px; font-weight: bold;">One moment, please . . . .</div>
    </div>
    <div class="content-header">
        <div class="container">
            <div class="row mb-2 callout callout-info">
                <div class="col-sm-6">
                    <h6 class="m-0"><i class="fas fa-copy"></i>&nbsp;&nbsp;<b>Lauramac Database Volume</b></h6>
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
                            <input type="date" id="can_fromdate" name="can_fromdate" class="form-control" style="width: 220px;" />
                        </td>
                        <td style="width: 50px;">
                            <b>Year:</b>
                        </td>
                        <td style="width: 150px;">
                            <input type="date" id="can_todate" name="can_todate" class="form-control" style="width: 220px;" />
                        </td>
                        <td style="width: 100px;">
                            <button id="btnShow" class="btn btn-primary" onclick="return getcanopyData();">Show</button>
                        </td>
                    </tr>
                </table>
                <hr />
                <div class="col-lg-12">
                    <div class="card-header p-0 pt-1">
                        <ul class="nav nav-tabs" id="custom-tabs-one-tab" role="tablist">
                            <li class="nav-item">
                                <a class="nav-link active" id="custom-tabs-one-home-tab" data-toggle="pill" href="#custom-tabs-one-home" role="tab" aria-controls="custom-tabs-one-home" aria-selected="true">Loan Details</a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" id="custom-tabs-one-profile-tab" data-toggle="pill" href="#custom-tabs-one-profile" role="tab" aria-controls="custom-tabs-one-profile" aria-selected="false">Task Details</a>
                            </li>

                        </ul>
                    </div>
                    <div class="card-body">
                        <div class="tab-content" id="custom-tabs-one-tabContent">
                            <div class="tab-pane fade show active" id="custom-tabs-one-home" role="tabpanel" aria-labelledby="custom-tabs-one-home-tab">

                                <table class="table" id="can_table" style="width: 100%;">
                                    <thead>
                                        <tr>
                                            <th class="sort border-top" style="text-wrap: nowrap;">Sr. #</th>
                                            <th class="sort border-top" style="text-wrap: nowrap; display:none;">Task Details</th>
                                            <th class="sort border-top" style="text-wrap: nowrap; display: none;">Loan ID</th>
                                            <th class="sort border-top" style="text-wrap: nowrap;">Loan #</th>
                                            <th class="sort border-top" style="text-wrap: nowrap;">Created Date</th>
                                            <th class="sort border-top" style="text-wrap: nowrap;">Submitted Date</th>
                                            <th class="sort border-top" style="text-wrap: nowrap;">Delivered Date</th>
                                            <th class="sort border-top" style="text-wrap: nowrap;">Transaction Identifier</th>
                                            <th class="sort border-top" style="text-wrap: nowrap;">Script</th>
                                            <th class="sort border-top" style="text-wrap: nowrap;">Buyer</th>
                                            <th class="sort border-top" style="text-wrap: nowrap;">Seller</th>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>

                            </div>
                            <div class="tab-pane fade" id="custom-tabs-one-profile" role="tabpanel" aria-labelledby="custom-tabs-one-profile-tab">
                                <table class="table" id="candetails_table" style="width: 100%;">
                                    <thead>
                                        <tr>
                                            <th class="sort border-top" style="text-wrap: nowrap;">Sr. #</th>
                                            <th class="sort border-top" style="text-wrap: nowrap;">Transaction Identifier</th>
                                            <th class="sort border-top" style="text-wrap: nowrap;">Loan #</th>
                                            <th class="sort border-top" style="text-wrap: nowrap;">Created Date</th>
                                            <th class="sort border-top" style="text-wrap: nowrap;">Submitted Date</th>
                                            <th class="sort border-top" style="text-wrap: nowrap;">Script</th>
                                            <th class="sort border-top" style="text-wrap: nowrap;">Task</th>
                                            <th class="sort border-top" style="text-wrap: nowrap;">Process Flow</th>
                                            <th class="sort border-top" style="text-wrap: nowrap;">Assigned User</th>
                                            <th class="sort border-top" style="text-wrap: nowrap;">Assigned Date</th>
                                            <th class="sort border-top" style="text-wrap: nowrap;">Due Date</th>
                                            <th class="sort border-top" style="text-wrap: nowrap;">Delivered Date</th>
                                            <th class="sort border-top" style="text-wrap: nowrap;">Task Status</th>
                                            <th class="sort border-top" style="text-wrap: nowrap;">Delivered Date</th>
                                            <th class="sort border-top" style="text-wrap: nowrap;">Buyer</th>
                                            <th class="sort border-top" style="text-wrap: nowrap;">Seller</th>
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


    <div class="modal fade" id="can_details">
        <div class="modal-dialog modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Task Details</h4>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <table class="table table-responsive" id="cantaskdetails" style="width: 100%;">
                        <thead>
                            <tr>
                                <th class="sort border-top" style="text-wrap: nowrap;">Sr. #</th>
                                <th class="sort border-top" style="text-wrap: nowrap;">Loan #</th>
                                <th class="sort border-top" style="text-wrap: nowrap;">Process</th>
                                <th class="sort border-top" style="text-wrap: nowrap;">Task</th>
                                <th class="sort border-top" style="text-wrap: nowrap;">Status</th>
                                <th class="sort border-top" style="text-wrap: nowrap;">Assigned User</th>
                                <th class="sort border-top" style="text-wrap: nowrap;">Assigned Date</th>
                                <th class="sort border-top" style="text-wrap: nowrap;">Due Date</th>
                                <th class="sort border-top" style="text-wrap: nowrap;">Completed Date</th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                    </table>
                </div>
                <div class="modal-footer justify-content-between">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>

            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    </div>
</asp:Content>

