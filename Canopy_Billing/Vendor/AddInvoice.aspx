<%@ Page Title="" Language="C#" MasterPageFile="~/Vendor/Vendor.Master" AutoEventWireup="true" CodeBehind="AddInvoice.aspx.cs" Inherits="Vendor_Portal.Vendor.AddInvoice" %>

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

        /*.form-control {
            font-size: 11px !important;
        }*/
    </style>
    <script>
        window.onload = function () {
            document.getElementById('addinvoice_file').addEventListener('change', getFileName);
            document.getElementById('addinvoice_sciennadocument').addEventListener('change', getFileNamesciennadocument);
            document.getElementById('addinvoice_sciennaexcel').addEventListener('change', getFileNamesciennaexcel);
            document.getElementById('addinvoice_laborexcel').addEventListener('change', getFileNamelaborexcel);
            document.getElementById('addinvoice_loandetails').addEventListener('change', getFileNameloandetails);
            document.getElementById('addinvoice_complianceinvoice').addEventListener('change', getFileNamecompinvoice);
            document.getElementById('addinvoice_complianceexcel').addEventListener('change', getFileNamecompexcel);
            document.getElementById('addinvoice_remoteuw').addEventListener('change', getFileNameremoteuw);
            document.getElementById('addinvoice_invoice_canopy').addEventListener('change', getFileNamestewartinvoice);
            document.getElementById('addinvoice_excel_canopy').addEventListener('change', getFileNamestewartexcel);
        }

        const getFileName = (event) => {
            const files = event.target.files;
            var file = files[0];
            document.getElementById("filep").value = files[0].name;

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
            document.getElementById("dropzone").classList.add("dz-max-files-reached");
            document.getElementById("conentdiv").style.display = '';
            document.getElementById("filesdiv").innerHTML = file.name;
            //alert(document.getElementById("filep").value);
        }

        const getFileNamesciennadocument = (event) => {
            const files = event.target.files;
            var file = files[0];
            document.getElementById("file_sciennadoc").value = files[0].name;

            const fd = new FormData();

            // add all selected files
            fd.append(event.target.name, file, "SciennaDoc_" + file.name);
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
            document.getElementById("dropzonesciennadoc").classList.add("dz-max-files-reached");
            document.getElementById("conentdivsciennadoc").style.display = '';
            document.getElementById("filesdivsciennadoc").innerHTML = file.name;
            //alert(document.getElementById("filep").value);
        }

        const getFileNamesciennaexcel = (event) => {
            const files = event.target.files;
            var file = files[0];
            document.getElementById("file_sciennaexcel").value = files[0].name;

            const fd = new FormData();

            // add all selected files
            fd.append(event.target.name, file, "SciennaExcel_" + file.name);
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
            document.getElementById("dropzonesciennaexcel").classList.add("dz-max-files-reached");
            document.getElementById("conentdivsciennaexcel").style.display = '';
            document.getElementById("filesdivsciennaexcel").innerHTML = file.name;
            //alert(document.getElementById("filep").value);
        }

        const getFileNamelaborexcel = (event) => {
            const files = event.target.files;
            var file = files[0];
            document.getElementById("file_laborcharges").value = files[0].name;

            const fd = new FormData();

            // add all selected files
            fd.append(event.target.name, file, "LaborCharges_" + file.name);
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
            document.getElementById("dropzonelaborexcel").classList.add("dz-max-files-reached");
            document.getElementById("conentdivlaborexcel").style.display = '';
            document.getElementById("filesdivlaborexcel").innerHTML = file.name;
            //alert(document.getElementById("filep").value);
        }

        const getFileNameloandetails = (event) => {
            const files = event.target.files;
            var file = files[0];
            document.getElementById("file_loandetails").value = files[0].name;

            const fd = new FormData();

            // add all selected files
            fd.append(event.target.name, file, "LoanDetails_" + file.name);
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
            document.getElementById("dropzoneloandetails").classList.add("dz-max-files-reached");
            document.getElementById("conentdivloandetails").style.display = '';
            document.getElementById("filesdivloandetails").innerHTML = file.name;
            //alert(document.getElementById("filep").value);
        }

        const getFileNamecompinvoice = (event) => {
            const files = event.target.files;
            var file = files[0];
            document.getElementById("file_compinvoice").value = files[0].name;

            const fd = new FormData();

            // add all selected files
            fd.append(event.target.name, file, "ComplianceInvoice_" + file.name);
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
            document.getElementById("dropzonecomplianceinvoice").classList.add("dz-max-files-reached");
            document.getElementById("conentdivcomplianceinvoice").style.display = '';
            document.getElementById("filesdivcomplianceinvoice").innerHTML = file.name;
            //alert(document.getElementById("filep").value);
        }

        const getFileNamecompexcel = (event) => {
            const files = event.target.files;
            var file = files[0];
            document.getElementById("file_compexcel").value = files[0].name;

            const fd = new FormData();

            // add all selected files
            fd.append(event.target.name, file, "ComplianceExcel_" + file.name);
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
            document.getElementById("dropzonecomplianceexcel").classList.add("dz-max-files-reached");
            document.getElementById("conentdivcomplianceexcel").style.display = '';
            document.getElementById("filesdivcomplianceexcel").innerHTML = file.name;
            //alert(document.getElementById("filep").value);
        }

        const getFileNameremoteuw = (event) => {
            const files = event.target.files;
            var file = files[0];
            document.getElementById("file_remoteuw").value = files[0].name;

            const fd = new FormData();

            // add all selected files
            fd.append(event.target.name, file, "RemoteUW_" + file.name);
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
            document.getElementById("dropzoneremoteuw").classList.add("dz-max-files-reached");
            document.getElementById("conentdivremoteuw").style.display = '';
            document.getElementById("filesdivremoteuw").innerHTML = file.name;
            //alert(document.getElementById("filep").value);
        }

        const getFileNamestewartinvoice = (event) => {
            const files = event.target.files;
            var file = files[0];
            document.getElementById("file_stewartinvoice").value = files[0].name;

            const fd = new FormData();

            // add all selected files
            fd.append(event.target.name, file, "StewartInvoice_" + file.name);
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
            document.getElementById("dropzonecanopy").classList.add("dz-max-files-reached");
            document.getElementById("conentdivcanopy").style.display = '';
            document.getElementById("filesdivcanopy").innerHTML = file.name;
            //alert(document.getElementById("filep").value);
        }

        const getFileNamestewartexcel = (event) => {
            const files = event.target.files;
            var file = files[0];
            document.getElementById("file_stewartexcel").value = files[0].name;

            const fd = new FormData();

            // add all selected files
            fd.append(event.target.name, file, "StewartExcel_" + file.name);
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
            document.getElementById("dropzonecanopyexcel").classList.add("dz-max-files-reached");
            document.getElementById("conentdivcanopyexcel").style.display = '';
            document.getElementById("filesdivcanopyexcel").innerHTML = file.name;
            //alert(document.getElementById("filep").value);
        }


        $(document).ready(function () {
            addinvoice_BindYear();
            addinvoice_BindYear_canopy();

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <input id="filep" style="display: none;" />
    <input id="file_sciennadoc" style="display: none;" />
    <input id="file_sciennaexcel" style="display: none;" />
    <input id="file_laborcharges" style="display: none;" />
    <input id="file_loandetails" style="display: none;" />
    <input id="file_compinvoice" style="display: none;" />
    <input id="file_compexcel" style="display: none;" />
    <input id="file_remoteuw" style="display: none;" />
    <input id="file_stewartinvoice" style="display: none;" />
    <input id="file_stewartexcel" style="display: none;" />

    <div class="loading" id="load1">
        <img src="../images/Load_1.gif" />
        <div style="font-size: 12px; font-weight: bold;">One moment, please . . . .</div>
    </div>
    <div class="content-header">
        <div class="container">
            <div class="row mb-2 callout callout-info">
                <div class="col-sm-6">
                    <h6 class="m-0"><i class="fas fa-copy"></i>&nbsp;&nbsp;<b>Add Invoice</b></h6>
                </div>
            </div>
        </div>
        <!-- /.container-fluid -->
    </div>
    <div class="col-lg-12">

        <div class="card">
            <div class="card-body">
                <div class="card card-tabs">
                    <div class="card-header p-0 pt-1">
                        <ul class="nav nav-tabs" id="custom-tabs-one-tab_addinvoice" role="tablist">
                            <li class="nav-item">
                                <a class="nav-link active" id="custom-tabs-one-home-tab_infinity" data-toggle="pill" href="#custom-tabs-one-home_infinity" role="tab" aria-controls="custom-tabs-one-home_infinity" aria-selected="true"><b>Infinity IPS</b></a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" id="custom-tabs-one-profile-tab_canopy" data-toggle="pill" href="#custom-tabs-one-profile_canopy" role="tab" aria-controls="custom-tabs-one-profile_canopy" aria-selected="false"><b>Canopy</b></a>
                            </li>

                        </ul>
                    </div>
                    <div class="card-body">
                        <div class="tab-content" id="custom-tabs-one-tabContent_addinvocie">
                            <div class="tab-pane fade show active" id="custom-tabs-one-home_infinity" role="tabpanel" aria-labelledby="custom-tabs-one-home-tab_infinity">
                                <table class="table">
                                    <tr>
                                        <td><b>Invoice Month:</b></td>
                                        <td>
                                            <select id="addinvoice_month" name="addinvoice_month" class="form-control" style="width: 250px;">
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
                                        <td><b>Invoice Year:</b></td>
                                        <td>
                                            <select id="addinvoice_year" name="addinvoice_year" class="form-control" style="width: 250px;">
                                                <option value="">Select</option>
                                            </select>
                                        </td>
                                        <td><b>Invoice Type:</b></td>
                                        <td>
                                            <select id="addinvoice_invoicetype" name="addinvoice_invoicetype" class="form-control" style="width: 250px;" onchange="return getprojectenabledisable(this);">
                                                <option value="">Select</option>
                                                <option value="Compliance">Compliance</option>
                                                <option value="Scienna">Scienna</option>
                                                <option value="Remote UW">Remote UW</option>
                                                <%--                                <option value="Attorney">Attorney</option>
                                <option value="Abstractor">Abstractor</option>                                
                                <option value="Other">Other</option>--%>
                                            </select>
                                        </td>
                                    </tr>
                                    <tr id="trScienna1" style="display: none; box-shadow: inset 0 3px 6px rgba(0,0,0,0.16), 0 4px 6px rgba(0,0,0,0.45); border-radius: 10px;">
                                        <td colspan="6">
                                            <table style="width: 100%; border: none!important;">
                                                <tr>
                                                    <td colspan="2" rowspan="2" style="vertical-align: middle;">
                                                        <b>Please upload required documents for Scienna</b>
                                                    </td>
                                                    <td><b>Scienna Invoice:</b></td>
                                                    <td>
                                                        <input type="file" id="addinvoice_sciennadocument" name="addinvoice_sciennadocument" class="form-control" style="width: 250px;" />
                                                        <div class="dropzone dropzone-multiple p-0 dz-clickable dz-file-processing dz-file-complete" id="dropzonesciennadoc" style="display: none;">
                                                            <div class="dz-preview dz-preview-multiple m-0 d-flex flex-column" id="conentdivsciennadoc" style="display: none!important;">
                                                                <div class="flex-1 d-flex flex-between-center">
                                                                    <div id="filesdivsciennadoc" style="margin-top: 10px; margin-bottom: 10px;"></div>
                                                                    <div class="dropdown font-sans-serif">
                                                                        <button class="btn btn-link text-600 btn-sm dropdown-toggle btn-reveal dropdown-caret-none" type="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                                                        </button>
                                                                        <div class="dropdown-menu dropdown-menu-end border py-2"><a class="dropdown-item" href="#!" data-dz-remove="data-dz-remove">Remove File</a></div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td><b>Scienna Excel:</b></td>
                                                    <td>
                                                        <input type="file" id="addinvoice_sciennaexcel" name="addinvoice_sciennaexcel" class="form-control" style="width: 250px;" />
                                                        <div class="dropzone dropzone-multiple p-0 dz-clickable dz-file-processing dz-file-complete" id="dropzonesciennaexcel" style="display: none;">
                                                            <div class="dz-preview dz-preview-multiple m-0 d-flex flex-column" id="conentdivsciennaexcel" style="display: none!important;">
                                                                <div class="flex-1 d-flex flex-between-center">
                                                                    <div id="filesdivsciennaexcel" style="margin-top: 10px; margin-bottom: 10px;"></div>
                                                                    <div class="dropdown font-sans-serif">
                                                                        <button class="btn btn-link text-600 btn-sm dropdown-toggle btn-reveal dropdown-caret-none" type="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                                        </button>
                                                                        <div class="dropdown-menu dropdown-menu-end border py-2"><a class="dropdown-item" href="#!" data-dz-remove="data-dz-remove">Remove File</a></div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td><b>Labor Charges:</b></td>
                                                    <td>
                                                        <input type="file" id="addinvoice_laborexcel" name="addinvoice_laborexcel" class="form-control" style="width: 250px;" />
                                                        <div class="dropzone dropzone-multiple p-0 dz-clickable dz-file-processing dz-file-complete" id="dropzonelaborexcel" style="display: none;">
                                                            <div class="dz-preview dz-preview-multiple m-0 d-flex flex-column" id="conentdivlaborexcel" style="display: none!important;">
                                                                <div class="flex-1 d-flex flex-between-center">
                                                                    <div id="filesdivlaborexcel" style="margin-top: 10px; margin-bottom: 10px;"></div>
                                                                    <div class="dropdown font-sans-serif">
                                                                        <button class="btn btn-link text-600 btn-sm dropdown-toggle btn-reveal dropdown-caret-none" type="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                                        </button>
                                                                        <div class="dropdown-menu dropdown-menu-end border py-2"><a class="dropdown-item" href="#!" data-dz-remove="data-dz-remove">Remove File</a></div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td><b>Loan Details:</b></td>
                                                    <td>
                                                        <input type="file" id="addinvoice_loandetails" name="addinvoice_loandetails" class="form-control" style="width: 250px;" />
                                                        <div class="dropzone dropzone-multiple p-0 dz-clickable dz-file-processing dz-file-complete" id="dropzoneloandetails" style="display: none;">
                                                            <div class="dz-preview dz-preview-multiple m-0 d-flex flex-column" id="conentdivloandetails" style="display: none!important;">
                                                                <div class="flex-1 d-flex flex-between-center">
                                                                    <div id="filesdivloandetails" style="margin-top: 10px; margin-bottom: 10px;"></div>
                                                                    <div class="dropdown font-sans-serif">
                                                                        <button class="btn btn-link text-600 btn-sm dropdown-toggle btn-reveal dropdown-caret-none" type="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                                        </button>
                                                                        <div class="dropdown-menu dropdown-menu-end border py-2"><a class="dropdown-item" href="#!" data-dz-remove="data-dz-remove">Remove File</a></div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="trCompliance" style="display: none; box-shadow: inset 0 3px 6px rgba(0,0,0,0.16), 0 4px 6px rgba(0,0,0,0.45); border-radius: 10px;">
                                        <td colspan="6">
                                            <table style="width: 100%; border: none!important;">
                                                <tr>
                                                    <td colspan="2"><b>Please upload required documents for Compliance</b></td>
                                                    <td><b>Compliance Invoice:</b></td>
                                                    <td>
                                                        <input type="file" id="addinvoice_complianceinvoice" name="addinvoice_complianceinvoice" class="form-control" style="width: 250px;" />
                                                        <div class="dropzone dropzone-multiple p-0 dz-clickable dz-file-processing dz-file-complete" id="dropzonecomplianceinvoice" style="display: none;">
                                                            <div class="dz-preview dz-preview-multiple m-0 d-flex flex-column" id="conentdivcomplianceinvoice" style="display: none!important;">
                                                                <div class="flex-1 d-flex flex-between-center">
                                                                    <div id="filesdivcomplianceinvoice" style="margin-top: 10px; margin-bottom: 10px;"></div>
                                                                    <div class="dropdown font-sans-serif">
                                                                        <button class="btn btn-link text-600 btn-sm dropdown-toggle btn-reveal dropdown-caret-none" type="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                                            <svg class="svg-inline--fa fa-ellipsis" style="display: none!important" aria-hidden="true" focusable="false" data-prefix="fas" data-icon="ellipsis" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512" data-fa-i2svg="">
                                                                                <path fill="currentColor" d="M120 256C120 286.9 94.93 312 64 312C33.07 312 8 286.9 8 256C8 225.1 33.07 200 64 200C94.93 200 120 225.1 120 256zM280 256C280 286.9 254.9 312 224 312C193.1 312 168 286.9 168 256C168 225.1 193.1 200 224 200C254.9 200 280 225.1 280 256zM328 256C328 225.1 353.1 200 384 200C414.9 200 440 225.1 440 256C440 286.9 414.9 312 384 312C353.1 312 328 286.9 328 256z"></path></svg><!-- <span class="fas fa-ellipsis-h"></span> Font Awesome fontawesome.com --></button>
                                                                        <div class="dropdown-menu dropdown-menu-end border py-2"><a class="dropdown-item" href="#!" data-dz-remove="data-dz-remove">Remove File</a></div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td><b>Compliance Excel:</b></td>
                                                    <td>
                                                        <input type="file" id="addinvoice_complianceexcel" name="addinvoice_complianceexcel" class="form-control" style="width: 250px;" />
                                                        <div class="dropzone dropzone-multiple p-0 dz-clickable dz-file-processing dz-file-complete" id="dropzonecomplianceexcel" style="display: none;">
                                                            <div class="dz-preview dz-preview-multiple m-0 d-flex flex-column" id="conentdivcomplianceexcel" style="display: none!important;">
                                                                <div class="flex-1 d-flex flex-between-center">
                                                                    <div id="filesdivcomplianceexcel" style="margin-top: 10px; margin-bottom: 10px;"></div>
                                                                    <div class="dropdown font-sans-serif">
                                                                        <button class="btn btn-link text-600 btn-sm dropdown-toggle btn-reveal dropdown-caret-none" type="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                                            <svg class="svg-inline--fa fa-ellipsis" style="display: none!important" aria-hidden="true" focusable="false" data-prefix="fas" data-icon="ellipsis" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512" data-fa-i2svg="">
                                                                                <path fill="currentColor" d="M120 256C120 286.9 94.93 312 64 312C33.07 312 8 286.9 8 256C8 225.1 33.07 200 64 200C94.93 200 120 225.1 120 256zM280 256C280 286.9 254.9 312 224 312C193.1 312 168 286.9 168 256C168 225.1 193.1 200 224 200C254.9 200 280 225.1 280 256zM328 256C328 225.1 353.1 200 384 200C414.9 200 440 225.1 440 256C440 286.9 414.9 312 384 312C353.1 312 328 286.9 328 256z"></path></svg><!-- <span class="fas fa-ellipsis-h"></span> Font Awesome fontawesome.com --></button>
                                                                        <div class="dropdown-menu dropdown-menu-end border py-2"><a class="dropdown-item" href="#!" data-dz-remove="data-dz-remove">Remove File</a></div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="trRemoteUW" style="display: none; box-shadow: inset 0 3px 6px rgba(0,0,0,0.16), 0 4px 6px rgba(0,0,0,0.45); border-radius: 10px;">
                                        <td colspan="6">
                                            <table style="width: 100%; border: none!important;">
                                                <tr>
                                                    <td colspan="2"><b>Please upload required documents for Remote Underwriters</b></td>
                                                    <td><b>Invoice Attachment:</b></td>
                                                    <td>
                                                        <input type="file" id="addinvoice_remoteuw" name="addinvoice_remoteuw" class="form-control" style="width: 250px;" />
                                                        <div class="dropzone dropzone-multiple p-0 dz-clickable dz-file-processing dz-file-complete" id="dropzoneremoteuw" style="display: none;">
                                                            <div class="dz-preview dz-preview-multiple m-0 d-flex flex-column" id="conentdivremoteuw" style="display: none!important;">
                                                                <div class="flex-1 d-flex flex-between-center">
                                                                    <div id="filesdivremoteuw" style="margin-top: 10px; margin-bottom: 10px;"></div>
                                                                    <div class="dropdown font-sans-serif">
                                                                        <button class="btn btn-link text-600 btn-sm dropdown-toggle btn-reveal dropdown-caret-none" type="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                                            <svg class="svg-inline--fa fa-ellipsis" style="display: none!important" aria-hidden="true" focusable="false" data-prefix="fas" data-icon="ellipsis" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512" data-fa-i2svg="">
                                                                                <path fill="currentColor" d="M120 256C120 286.9 94.93 312 64 312C33.07 312 8 286.9 8 256C8 225.1 33.07 200 64 200C94.93 200 120 225.1 120 256zM280 256C280 286.9 254.9 312 224 312C193.1 312 168 286.9 168 256C168 225.1 193.1 200 224 200C254.9 200 280 225.1 280 256zM328 256C328 225.1 353.1 200 384 200C414.9 200 440 225.1 440 256C440 286.9 414.9 312 384 312C353.1 312 328 286.9 328 256z"></path></svg><!-- <span class="fas fa-ellipsis-h"></span> Font Awesome fontawesome.com --></button>
                                                                        <div class="dropdown-menu dropdown-menu-end border py-2"><a class="dropdown-item" href="#!" data-dz-remove="data-dz-remove">Remove File</a></div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>

                                        <td>
                                            <br />
                                            <b>Bill To:</b></td>
                                        <td>
                                            <br />
                                            <input type="text" id="addinvoice_billto" name="addinvoice_billto" class="form-control" style="width: 250px;" />
                                        </td>
                                        <td>
                                            <br />
                                            <b>Invoice #:</b></td>
                                        <td>
                                            <br />
                                            <input type="text" id="addinvoice_invoicenumber" name="addinvoice_invoicenumber" class="form-control" style="width: 250px;" />
                                        </td>

                                        <td>
                                            <br />
                                            <b id="tdloanorder">Loan Count:</b></td>
                                        <td>
                                            <br />
                                            <input type="text" id="addinvoice_loancount" name="addinvoice_loancount" class="form-control" style="width: 250px;" />
                                        </td>


                                    </tr>
                                    <tr>
                                        <td style="display: none;"><b>Domain:</b></td>
                                        <td style="display: none;">
                                            <select id="addinvoice_domain" name="addinvoice_domain" class="form-control" style="width: 250px;">
                                                <option value="">Select</option>
                                                <option value="Underwriting">Underwriting</option>
                                                <option value="Other">Other</option>
                                            </select>
                                        </td>
                                        <td style="display: none;"><b>Other Invoice Type:</b></td>
                                        <td style="display: none;">
                                            <input type="text" id="addinvoice_othertype" name="addinvoice_othertype" class="form-control" style="width: 250px;" disabled="disabled" />
                                        </td>
                                        <%-- <td><b>Project #:</b></td>
                        <td>
                            <select id="addinvoice_projectno" name="addinvoice_projectno" class="form-control" style="width: 250px;" disabled="disabled"></select>
                        </td>--%>
                                        <td style="display: none;"><b>Invoice Attachment:</b></td>
                                        <td style="display: none;">
                                            <input type="file" id="addinvoice_file" name="addinvoice_file" class="form-control" style="width: 250px;" />
                                            <div class="dropzone dropzone-multiple p-0 dz-clickable dz-file-processing dz-file-complete" id="dropzone">
                                                <div class="dz-preview dz-preview-multiple m-0 d-flex flex-column" id="conentdiv" style="display: none!important;">
                                                    <div class="flex-1 d-flex flex-between-center">
                                                        <div id="filesdiv" style="margin-top: 10px; margin-bottom: 10px;"></div>
                                                        <div class="dropdown font-sans-serif">
                                                            <button class="btn btn-link text-600 btn-sm dropdown-toggle btn-reveal dropdown-caret-none" type="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                                <svg class="svg-inline--fa fa-ellipsis" style="display: none!important" aria-hidden="true" focusable="false" data-prefix="fas" data-icon="ellipsis" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 448 512" data-fa-i2svg="">
                                                                    <path fill="currentColor" d="M120 256C120 286.9 94.93 312 64 312C33.07 312 8 286.9 8 256C8 225.1 33.07 200 64 200C94.93 200 120 225.1 120 256zM280 256C280 286.9 254.9 312 224 312C193.1 312 168 286.9 168 256C168 225.1 193.1 200 224 200C254.9 200 280 225.1 280 256zM328 256C328 225.1 353.1 200 384 200C414.9 200 440 225.1 440 256C440 286.9 414.9 312 384 312C353.1 312 328 286.9 328 256z"></path></svg><!-- <span class="fas fa-ellipsis-h"></span> Font Awesome fontawesome.com --></button>
                                                            <div class="dropdown-menu dropdown-menu-end border py-2"><a class="dropdown-item" href="#!" data-dz-remove="data-dz-remove">Remove File</a></div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><b>Invoice Date:</b></td>
                                        <td>
                                            <input type="date" id="addinvoice_invoicedate" name="addinvoice_invoicedate" class="form-control" style="width: 250px;" />
                                        </td>
                                        <td><b>Due Date:</b></td>
                                        <td>
                                            <input type="date" id="addinvoice_duedate" name="addinvoice_duedate" class="form-control" style="width: 250px;" />
                                        </td>

                                        <td><b>Invoice Amount</b></td>
                                        <td>
                                            <input type="text" id="addinvoice_invoiceamount" name="addinvoice_invoiceamount" class="form-control" style="width: 250px;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><b>Currency:</b></td>
                                        <td>
                                            <select id="addinvoice_currency" name="addinvoice_currency" class="form-control" style="width: 250px;">
                                                <option value="">Select</option>
                                                <option value="USD">USD</option>
                                                <option value="INR">INR</option>
                                            </select>
                                        </td>
                                        <td><b>Delay Remark:</b></td>
                                        <td>
                                            <textarea id="addinvoice_delayremark" name="addinvoice_delayremark" class="form-control" style="width: 250px;"></textarea>
                                        </td>
                                        <td><b>Verification Remark:</b></td>
                                        <td>
                                            <textarea id="addinvoice_verificationremark" name="addinvoice_verificationremark" class="form-control" style="width: 250px;"></textarea>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6" style="text-align: center;">
                                            <button id="addinvoice_btnsubmit" name="addinvoice_btnsubmit" class="btn btn-primary" onclick="return addinvoice_submit();">Submit</button>
                                        </td>
                                    </tr>
                                </table>
                                <hr />
                                <div id="dvScienna" style="display: none;">
                                    <div class="card card-tabs">
                                        <div class="card-header p-0 pt-1">
                                            <ul class="nav nav-tabs" id="custom-tabs-one-tab" role="tablist">
                                                <li class="nav-item">
                                                    <a class="nav-link active" id="custom-tabs-one-home-tab" data-toggle="pill" href="#custom-tabs-one-home" role="tab" aria-controls="custom-tabs-one-home" aria-selected="true">Scienna Invoice</a>
                                                </li>
                                                <li class="nav-item" onclick="onsecondclick();" id="nav2" runat="server">
                                                    <a class="nav-link" id="custom-tabs-one-profile-tab" data-toggle="pill" href="#custom-tabs-one-profile" role="tab" aria-controls="custom-tabs-one-profile" aria-selected="false">Labor Charges</a>
                                                </li>
                                                <li class="nav-item" onclick="onthirdclick();" id="nav3" runat="server">
                                                    <a class="nav-link" id="custom-tabs-one-messages-tab" data-toggle="pill" href="#custom-tabs-one-messages" role="tab" aria-controls="custom-tabs-one-messages" aria-selected="false">Loan Details</a>
                                                </li>

                                            </ul>
                                        </div>
                                        <div class="card-body">
                                            <div class="tab-content" id="custom-tabs-one-tabContent">
                                                <div class="tab-pane fade show active" id="custom-tabs-one-home" role="tabpanel" aria-labelledby="custom-tabs-one-home-tab">
                                                    <table class="table" id="addinvoice_sciennadetails" style="width: 100%;">
                                                        <thead>
                                                            <tr>
                                                                <th class="sort border-top ps-3" style="text-wrap: nowrap; text-align: center;">Sr. #</th>
                                                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Month</th>
                                                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Year</th>
                                                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Client</th>
                                                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Project</th>
                                                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Period Ending</th>
                                                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Loans Reviewed</th>
                                                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Per Loan Usage Fees</th>
                                                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Usage Fees</th>
                                                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Labor</th>
                                                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">Total Fees</th>
                                                                <th class="sort border-top ps-3" style="text-wrap: nowrap;">System Verification Status</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody></tbody>
                                                    </table>
                                                </div>
                                                <div class="tab-pane fade" id="custom-tabs-one-profile" role="tabpanel" aria-labelledby="custom-tabs-one-profile-tab">
                                                    <div style="width: 100%; overflow: scroll;">
                                                        <table class="table" id="addinvoice_laborcharges" style="width: 100%;">
                                                            <thead>
                                                                <tr>
                                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap; text-align: center;">Sr. #</th>
                                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Month</th>
                                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Year</th>
                                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Personnel</th>
                                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Client</th>
                                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Project</th>
                                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Date</th>
                                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Begin Time</th>
                                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">End Time</th>
                                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Hours</th>
                                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Rate</th>
                                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Preliminary Fee</th>
                                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Charged At</th>
                                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Reason</th>
                                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Fee</th>
                                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Activity</th>
                                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Description Of Session Activities</th>
                                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Duplicate</th>
                                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">System Verification Status</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody></tbody>
                                                        </table>
                                                    </div>
                                                </div>
                                                <div class="tab-pane fade" id="custom-tabs-one-messages" role="tabpanel" aria-labelledby="custom-tabs-one-messages-tab">
                                                    <div style="width: 100%; overflow: scroll;">
                                                        <table class="table" id="addinvoice_loandetailstable" style="width: 100%;">
                                                            <thead>
                                                                <tr>
                                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap; text-align: center;">Sr. #</th>
                                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Project</th>
                                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Deal #</th>
                                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Loan #</th>
                                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Scienna ID</th>
                                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Start Date</th>
                                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Sign Off Date</th>
                                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">RawBillable</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody></tbody>
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div style="display: none;">
                                        <table class="table" id="addinvoice_table" style="width: 100%; display: none;">
                                            <thead>
                                                <tr>
                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Download</th>
                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap; text-align: center;">Sr. #</th>
                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Month</th>
                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Year</th>
                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Invoice Type</th>
                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Invoice #</th>
                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Invoice Date</th>
                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Due Date</th>
                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Bill To?</th>
                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Balance Due</th>
                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Delay Cause</th>
                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Remark</th>
                                                    <th class="sort border-top ps-3" style="text-wrap: nowrap;">Added Date</th>
                                                </tr>
                                            </thead>
                                            <tbody></tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                            <div class="tab-pane fade" id="custom-tabs-one-profile_canopy" role="tabpanel" aria-labelledby="custom-tabs-one-profile-tab_canopy">
                                <table class="table">
                                    <tr>
                                        <td><b>Invoice Month:</b></td>
                                        <td>
                                            <select id="addinvoice_month_canopy" name="addinvoice_month_canopy" class="form-control" style="width: 250px;">
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
                                        <td><b>Invoice Year:</b></td>
                                        <td>
                                            <select id="addinvoice_year_canopy" name="addinvoice_year_canopy" class="form-control" style="width: 250px;">
                                                <option value="">Select</option>
                                            </select>
                                        </td>
                                        <td><b>Invoice Type:</b></td>
                                        <td>
                                            <select id="addinvoice_invoicetype_canopy" name="addinvoice_invoicetype_canopy" class="form-control" style="width: 250px;" onchange="return getprojectenabledisable(this);">
                                                <option value="">Select</option>
                                                <option value="Compliance">Compliance</option>
                                                <option value="LauraMac">LauraMac</option>
                                                <option value="Remote UW">Remote UW</option>
                                                <option value="Stewart">Stewart</option>
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><b>Invoice:</b></td>
                                        <td>
                                            <input type="file" id="addinvoice_invoice_canopy" name="addinvoice_invoice_canopy" class="form-control" style="width: 250px;" />
                                            <div class="dropzone dropzone-multiple p-0 dz-clickable dz-file-processing dz-file-complete" id="dropzonecanopy" style="display: none;">
                                                <div class="dz-preview dz-preview-multiple m-0 d-flex flex-column" id="conentdivcanopy" style="display: none!important;">
                                                    <div class="flex-1 d-flex flex-between-center">
                                                        <div id="filesdivcanopy" style="margin-top: 10px; margin-bottom: 10px;"></div>
                                                        <div class="dropdown font-sans-serif">
                                                            <button class="btn btn-link text-600 btn-sm dropdown-toggle btn-reveal dropdown-caret-none" type="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                                            </button>
                                                            <div class="dropdown-menu dropdown-menu-end border py-2"><a class="dropdown-item" href="#!" data-dz-remove="data-dz-remove">Remove File</a></div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                        <td><b>Excel:</b></td>
                                        <td>
                                            <input type="file" id="addinvoice_excel_canopy" name="addinvoice_excel_canopy" class="form-control" style="width: 250px;" />
                                            <div class="dropzone dropzone-multiple p-0 dz-clickable dz-file-processing dz-file-complete" id="dropzonecanopyexcel" style="display: none;">
                                                <div class="dz-preview dz-preview-multiple m-0 d-flex flex-column" id="conentdivcanopyexcel" style="display: none!important;">
                                                    <div class="flex-1 d-flex flex-between-center">
                                                        <div id="filesdivcanopyexcel" style="margin-top: 10px; margin-bottom: 10px;"></div>
                                                        <div class="dropdown font-sans-serif">
                                                            <button class="btn btn-link text-600 btn-sm dropdown-toggle btn-reveal dropdown-caret-none" type="button" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                                            </button>
                                                            <div class="dropdown-menu dropdown-menu-end border py-2"><a class="dropdown-item" href="#!" data-dz-remove="data-dz-remove">Remove File</a></div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                        <td><b>Invoice #:</b></td>
                                        <td>
                                            <input type="text" id="addinvoice_invoiceno_canopy" name="addinvoice_invoiceno_canopy" class="form-control" style="width: 250px;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><b>Invoice Date:</b></td>
                                        <td>
                                            <input type="date" id="addinvoice_invoicedate_canopy" name="addinvoice_invoicedate_canopy" class="form-control" style="width: 250px;" />
                                        </td>
                                        <td><b>Bill To:</b></td>
                                        <td>
                                            <input type="text" id="addinvoice_billto_canopy" name="addinvoice_billto_canopy" class="form-control" style="width: 250px;" />
                                        </td>
                                        <td><b>Invoice Amount($):</b></td>
                                        <td>
                                            <input type="text" id="addinvoice_invoiceamount_canopy" name="addinvoice_invoiceamount_canopy" class="form-control" style="width: 250px;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6" style="text-align: center;">
                                            <button id="addinvoice_btnsubmit_canopy" name="addinvoice_btnsubmit_canopy" class="btn btn-primary" onclick="return addinvoice_submit_canopy();">Import Data</button>
                                        </td>
                                    </tr>
                                </table>
                                <hr />
                                <table class="table" id="addinvoice_stewartgrid" style="width: 100%;">
                                    <thead>
                                        <tr>
                                            <th class="sort border-top ps-3" style="text-wrap: nowrap; text-align: center;">Sr. #</th>
                                            <th class="sort border-top ps-3" style="text-wrap: nowrap;">Month</th>
                                            <th class="sort border-top ps-3" style="text-wrap: nowrap;">Year</th>
                                            <th class="sort border-top ps-3" style="text-wrap: nowrap;">Loan #</th>
                                            <th class="sort border-top ps-3" style="text-wrap: nowrap;">Case #</th>
                                            <th class="sort border-top ps-3" style="text-wrap: nowrap;">Invoice #</th>
                                            <th class="sort border-top ps-3" style="text-wrap: nowrap;">Invoice Date</th>
                                            <th class="sort border-top ps-3" style="text-wrap: nowrap;">Order Date</th>
                                            <th class="sort border-top ps-3" style="text-wrap: nowrap;">Fee</th>
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
    <div class="modal fade" id="addinvoice_dverror">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <h6 class="modal-title" id="addinvoice_errmsg"></h6>
                </div>
                <div class="modal-footer align-content-center">
                    <button class="btn btn-primary" type="button" id="addinvoice_btnMessage" onclick="return addinvoice_closepopup();">Okay</button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
</asp:Content>
