<%@ Page Title="" Language="C#" MasterPageFile="~/BDM/BDM.Master" AutoEventWireup="true" CodeBehind="ClientBillingDeviationReport.aspx.cs" Inherits="Vendor_Portal.BDM.ClientBillingDeviationReport" %>

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
        function cbdr_exporttoexcel() {
            $('#waitingpanel').modal('show');
            document.getElementById("spntext").innerHTML = "Generating excel sheet : IPS - DD - Summary";
            var fromdate = document.getElementById("cbdr_fromdate").value;
            var todate = document.getElementById("cbdr_todate").value;
            PageMethods.GenerateDDSummary(fromdate, todate, ddsum_OnSuccess, ddsum_OnError);

            return false;
        }
        function ddsum_OnSuccess(result) {
            var fromdate = document.getElementById("cbdr_fromdate").value;
            var todate = document.getElementById("cbdr_todate").value;
            document.getElementById("spntext").innerHTML = "Generating excel sheet : IPS - DD - Parameter wise";
            PageMethods.GenerateDDDetails(fromdate, todate, dddet_OnSuccess, dddet_OnError);
            return false;
        }
        function dddet_OnSuccess(result) {
            var fromdate = document.getElementById("cbdr_fromdate").value;
            var todate = document.getElementById("cbdr_todate").value;
            document.getElementById("spntext").innerHTML = "Generating excel sheet : IPS - Non DD - Summary";
            PageMethods.GenerateNonDDSummary(fromdate, todate, nonddsum_OnSuccess, nonddsum_OnError);
            return false;
        }
        function nonddsum_OnSuccess(result) {
            var fromdate = document.getElementById("cbdr_fromdate").value;
            var todate = document.getElementById("cbdr_todate").value;
            document.getElementById("spntext").innerHTML = "Generating excel sheet : IPS - DD - Parameter wise";
            PageMethods.GenerateNonDDDetails(fromdate, todate, nondddet_OnSuccess, nondddet_OnError);
            return false;
        }
        function nondddet_OnSuccess(result) {
            var fromdate = document.getElementById("cbdr_fromdate").value;
            var todate = document.getElementById("cbdr_todate").value;
            document.getElementById("spntext").innerHTML = "Generating excel sheet : Canopy - Summary";
            PageMethods.GenerateCanopySummary(fromdate, todate, canpysum_OnSuccess, canpysum_OnError);
            return false;
        }
        function canpysum_OnSuccess(result) {
            var fromdate = document.getElementById("cbdr_fromdate").value;
            var todate = document.getElementById("cbdr_todate").value;
            document.getElementById("spntext").innerHTML = "Generating excel sheet : Canopy - Parameter wise";
            PageMethods.GenerateCanopyDetails(fromdate, todate, canpydet_OnSuccess, canpydet_OnError);
            return false;
        }
        function canpydet_OnSuccess(result) {
            document.getElementById("spntext").innerHTML = "Report Prepeation Completed. Downloading Report";
            $('#waitingpanel').modal('hide');
            __doPostBack("<%= btn1.UniqueID %>", '');
            return false;
        }
        function canpydet_OnError(error) {
            alert(error.responseText);
        }
        function canpysum_OnError(error) {
            alert(error.responseText);
        }
        function nondddet_OnError(error) {
            alert(error.responseText);
        }
        function nonddsum_OnError(error) {
            alert(error.responseText);
        }
        function dddet_OnError(error) {
            alert(error.responseText);
        }
        function ddsum_OnError(error) {
            alert(error.responseText);
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="bdm-page">
    <asp:Button ID="btn1" runat="server" Style="display: none;" OnClick="btn1_Click" />
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
                    <h6 class="m-0"><i class="fas fa-copy"></i>&nbsp;&nbsp;<b>Client Billing Deviation Report</b></h6>
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
                        <td style="width: 50px;"><b>From Date:</b></td>
                        <td style="width: 150px;">
                            <input type="date" id="cbdr_fromdate" name="cbdr_fromdate" class="form-control" style="width: 220px;" />
                        </td>
                        <td style="width: 50px;">
                            <b>To Date:</b>
                        </td>
                        <td style="width: 150px;">
                            <input type="date" id="cbdr_todate" name="cbdr_todate" class="form-control" style="width: 220px;" />
                        </td>
                        <td style="width: 100px;">
                            <button id="cbdr_btnShow" class="btn btn-primary" onclick="return cbdr_bindCanopySummmarygrid();">Show</button>
                            <button id="cbdr_btnexport" class="btn btn-primary" onclick="return cbdr_exporttoexcel();" style="display:none;">Export to excel</button>
                        </td>
                    </tr>
                </table>
                <hr />
                <div class="card">
                    <div class="card-body">
                        <div class="card card-tabs">
                            <div class="card-header p-0 pt-1">
                                <ul class="nav nav-tabs" id="custom-tabs-one-tab_bpd_tabs" role="tablist">
                                    <li class="nav-item" style="display:none;">
                                        <a class="nav-link" id="custom-tabs-one-home-tab_company" data-toggle="pill" href="#custom-tabs-one-home_company" role="tab" aria-controls="custom-tabs-one-home_company" aria-selected="true"><b>IPS - DD</b></a>
                                    </li>
                                    <li class="nav-item" style="display:none;">
                                        <a class="nav-link" onclick="return cbdr_bindDDDetailsgrid();" id="custom-tabs-one-profile-tab_sales" data-toggle="pill" href="#custom-tabs-one-profile_sales" role="tab" aria-controls="custom-tabs-one-profile_sales" aria-selected="false"><b>IPS - DD - Parameter wise</b></a>
                                    </li>
                                    <li class="nav-item" style="display:none;">
                                        <a class="nav-link" onclick="return cbdr_bindNonDDSummmarygrid();" id="custom-tabs-one-profile-tab_deal" data-toggle="pill" href="#custom-tabs-one-profile_deal" role="tab" aria-controls="custom-tabs-one-profile_deal" aria-selected="false"><b>IPS - Non DD</b></a>
                                    </li>
                                    <li class="nav-item" style="display:none;">
                                        <a class="nav-link" onclick="return cbdr_bindNonDDDetailsgrid();" id="custom-tabs-one-profile-tab_nonddsum" data-toggle="pill" href="#custom-tabs-one-profile_nonddsum" role="tab" aria-controls="custom-tabs-one-profile_nonddsum" aria-selected="false"><b>IPS - Non DD - Parameter wise</b></a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link active" onclick="return cbdr_bindCanopySummmarygrid();" id="custom-tabs-one-profile-tab_canopy" data-toggle="pill" href="#custom-tabs-one-profile_canopy" role="tab" aria-controls="custom-tabs-one-profile_canop" aria-selected="false"><b>Canopy</b></a>
                                    </li>
                                    <li class="nav-item">
                                        <a class="nav-link" onclick="return cbdr_bindCanopyDetailsgrid();" id="custom-tabs-one-profile-tab_canopydetail" data-toggle="pill" href="#custom-tabs-one-profile_canopydetail" role="tab" aria-controls="custom-tabs-one-profile_canopydetail" aria-selected="false"><b>Canopy - Parameter wise</b></a>
                                    </li>
                                </ul>
                            </div>
                            <div class="card-body">
                                <div class="tab-content" id="custom-tabs-one-tabContent_addinvocie">
                                    <div class="tab-pane fade" id="custom-tabs-one-home_company" role="tabpanel" aria-labelledby="custom-tabs-one-home-tab_company">
                                        <table class="table" id="cbdr_ddsummary" style="width: 100%;">
                                        </table>
                                    </div>
                                    <div class="tab-pane fade" id="custom-tabs-one-profile_sales" role="tabpanel" aria-labelledby="custom-tabs-one-profile-tab_sales">
                                        <table class="table" id="cbdr_dddetails" style="width: 100%;">
                                        </table>
                                    </div>
                                    <div class="tab-pane fade" id="custom-tabs-one-profile_deal" role="tabpanel" aria-labelledby="custom-tabs-one-profile_deal">
                                        <table class="table" id="cbdr_nonddsummary" style="width: 100%;">
                                        </table>
                                    </div>
                                    <div class="tab-pane fade" id="custom-tabs-one-profile_nonddsum" role="tabpanel" aria-labelledby="custom-tabs-one-profile-tab_nonddsum">
                                        <table class="table" id="cbdr_nondddetails" style="width: 100%;">
                                        </table>
                                    </div>
                                    <div class="tab-pane fade show active" id="custom-tabs-one-profile_canopy" role="tabpanel" aria-labelledby="custom-tabs-one-profile-tab_canopy">
                                        <table class="table" id="cbdr_canopysummary" style="width: 100%;">
                                        </table>
                                    </div>
                                    <div class="tab-pane fade" id="custom-tabs-one-profile_canopydetail" role="tabpanel" aria-labelledby="custom-tabs-one-profile-tab_canopydetail">
                                        <table class="table" id="cbdr_canopydetails" style="width: 100%;">
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

