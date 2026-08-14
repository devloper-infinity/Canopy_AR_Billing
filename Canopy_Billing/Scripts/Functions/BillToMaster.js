function blankForNull(s) {
    return s == "null" || s == null ? "" : s;
}

function clearClientForm() {
    $("#txtBillTo").val('');
    $("#txtsellerName").val('');
    $("#txtbuyerName").val('');
    $("#txttransactionIdentifier").val('');
}

function saveBillToMaster_1() {
    let BillTo = $("#txtBillTo").val();
    let Seller = $("#txtsellerName").val();
    let Buyer = $(`#txtbuyerName`).val().trim();
    let Transaction = $(`#txttransactionIdentifier`).val().trim();

    alert(BillTo);
    alert(Seller);
    alert(Buyer);
    alert(Transaction);
    

    // 🔴 VALIDATION
    if (!BillTo || !Seller || !Buyer || !Transaction ) {
        Swal.fire({
            icon: 'warning',
            title: 'Validation Error',
            text: 'All fields (Bill To, Seller Name, Buyer Name, Transaction) are required.'
        });
        return false;
    }

    $.ajax({
        type: "POST",
        url: "BillToMaster.aspx/SaveBillToMaster",
        contentType: "application/json",
        data: JSON.stringify({
            transactionIdentifier: Transaction,
            buyerName: Buyer,
            sellerName: Seller,
            BillTo: BillTo

        }),
        success: function () {
            clearClientForm();
            Swal.fire({
                icon: 'success',
                title: 'Bill To Master Saved',
                text: 'Bill To added successfully.',
                confirmButtonColor: '#3085d6'
            });

            $('#BillToMaster_table').DataTable().ajax.reload();
        }
    });
    return false;
}

function saveBillToMaster() {
    let BillTo = $("#txtBillTo").val().trim();
    let Seller = $("#txtsellerName").val().trim();
    let Buyer = $("#txtbuyerName").val().trim();
    let Transaction = $("#txttransactionIdentifier").val().trim();

    //alert(BillTo);
    //alert(Seller);
    //alert(Buyer);
    //alert(Transaction);

    if (BillTo.length === 0 || Seller.length === 0 || Buyer.length === 0 || Transaction.length === 0) {
        alert("All fields(Bill To, Seller Name, Buyer Name, Transaction) are required.");
        //Swal.fire({
        //    icon: 'warning',
        //    title: 'Validation Error',
        //    text: 'All fields (Bill To, Seller Name, Buyer Name, Transaction) are required.'
      //        );
        return false;
    }

    $.ajax({
        type: "POST",
        url: "BillToMaster.aspx/SaveBillToMaster",
        contentType: "application/json",
        data: JSON.stringify({
            transactionIdentifier: Transaction,
            buyerName: Buyer,
            sellerName: Seller,
            BillTo: BillTo
        }),
        success: function () {
            clearClientForm();

            alert("Bill To added successfully.");
            //Swal.fire({
            //    icon: 'success',
            //    title: 'Bill To Master Saved',
            //    text: 'Bill To added successfully.',
            //    confirmButtonColor: '#3085d6'
            //});

            $('#BillToMaster_table').DataTable().ajax.reload();
        }
    });

    return false;
}

function BindBillToMaster() {
    $('#load1').show();
    $.ajax({
        url: "BillToMaster.aspx/GetBillToMaster",
        type: "POST",
        dataType: "json",
        /*data: "{Month:'" + month + "',Year:'" + year + "'}",*/
        contentType: "application/json; charset=utf-8",

        success: function (data) {
            if ($.fn.dataTable.isDataTable('#BillToMaster_table')) {
                $('#BillToMaster_table').DataTable().destroy();
            }
            dataArray = JSON.parse(data.d);
            $('#BillToMaster_table').DataTable({
                dom: 'Bftp',
                scrollX: true,
                destroy: true,
                paging: true,
                "autoWidth": true,
                select: true,
                fixedHeader: true,
                processing: true,
                "aaSorting": [],
                'select': {
                    'style': 'single'
                },
                "data": dataArray,
                columns: [
                    { data: 'MasterID' },
                    { data: 'BillTo' },
                    {  data: 'sellerName' },
                    { data: 'buyerName' },
                    { data: 'transactionIdentifier' },
                    
                ],
                fnCreatedRow: function (nRow, aData, iDataIndex) {

                    $(nRow).children("td").css("text-align", "center");
                },

                initComplete: function () {
                    $('#load1').hide();
                },
                buttons: [
                    {
                        extend: 'excelHtml5', title: 'Bill To Master', autoFilter: true,
                    },
                ],

            });

        }
    });

    return false;

}

