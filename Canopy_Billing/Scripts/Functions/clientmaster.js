function blankForNull(s) {
    return s == "null" || s == null ? "" : s;
}

function saveClient() {

    let clientName = $("#txtClientName").val();
    let address = $("#txtAddress").val();
    let person = $(`#txtPrimaryPerson`).val().trim();
    let phone = $(`#txtPrimaryPhone`).val().trim();
    let email = $(`#txtPrimaryEmail`).val().trim();

    // 🔴 VALIDATION
    if (!clientName || !address || !person || !phone || !email) {
        Swal.fire({
            icon: 'warning',
            title: 'Validation Error',
            text: 'All fields (Client Name, Contact Person, Phone, Email) are required.'
        });
        return false;
    }

    // 🔴 OPTIONAL: Phone validation (only digits)
    let phoneRegex = /^[0-9]{7,15}$/;
    if (!phoneRegex.test(phone)) {
        Swal.fire({
            icon: 'warning',
            title: 'Invalid Phone',
            text: 'Phone must be numeric and 7–15 digits.'
        });
        return false;
    }

    // 🔴 OPTIONAL: Email format validation
    let emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
        Swal.fire({
            icon: 'warning',
            title: 'Invalid Email',
            text: 'Please enter a valid email address.'
        });
        return false;
    }

    let contacts = [{
        ContactPerson: $("#txtPrimaryPerson").val(),
        ContactNo: $("#txtPrimaryPhone").val(),
        Email: $("#txtPrimaryEmail").val()
    }];

    $.ajax({
        type: "POST",
        url: "ClientMaster.aspx/SaveClient",
        contentType: "application/json",
        data: JSON.stringify({
            clientName: clientName,
            Address: address,
            contacts: contacts
        }),
        success: function () {
            clearClientForm();
            Swal.fire({
                icon: 'success',
                title: 'Client Saved',
                text: 'Client and primary contact added successfully.',
                confirmButtonColor: '#3085d6'
            });

            $('#clm_table').DataTable().ajax.reload();
        }
    });
    return false;
}
function clearClientForm() {
    $("#txtClientName").val('');
    $("#txtAddress").val('');
    $("#txtPrimaryPerson").val('');
    $("#txtPrimaryPhone").val('');
    $("#txtPrimaryEmail").val('');
}

function clm_loadTable() {

    let table = $('#clm_table').DataTable({
        ajax: {
            url: 'ClientMaster.aspx/GetClientsGrouped',
            type: 'POST',
            contentType: 'application/json',
            data: function () { return JSON.stringify({}); },
            dataSrc: 'd'
        },

        columns: [
            {
                className: 'details-control',
                orderable: false,
                data: null,
                defaultContent: '<i class="fa fa-plus"></i>'
            },
            { data: 'ClientName' },
            { data: 'Address' },
            {
                data: 'PrimaryContact',
                render: d => d ? d.ContactPerson : ''
            },
            {
                data: 'PrimaryContact',
                render: d => d ? d.ContactNo : ''
            },
            {
                data: 'PrimaryContact',
                render: d => d ? d.Email : ''
            }
        ]

    });

    // Expand row
    $('#clm_table tbody').on('click', 'td.details-control', function () {

        let tr = $(this).closest('tr');
        let row = table.row(tr);
        let icon = $(this).find('i'); // get icon

        if (row.child.isShown()) {
            row.child.hide();
            tr.removeClass('shown');
            // 🔽 change to PLUS
            icon.removeClass('fa-minus').addClass('fa-plus');
        }
        else {
            row.child(format(row.data())).show();
            tr.addClass('shown');
            icon.removeClass('fa-plus').addClass('fa-minus');
        }
    });
    return false;
}



function format(data) {

    let html = `<div style="padding:10px;">`;

    html += '<table class="table table-bordered">';
    html += '<tr><th>Contact</th><th>Phone</th><th>Email</th><th>Action</th></tr>';

    data.OtherContacts.forEach(c => {

        html += `<tr id="row_${c.ContactID}">
            <td>
                <span id="name_${c.ContactID}">${c.ContactPerson}</span>
                <input type="text" id="edit_name_${c.ContactID}" value="${c.ContactPerson}" style="display:none;" />
            </td>
            <td>
                <span id="phone_${c.ContactID}">${c.ContactNo}</span>
                <input type="text" id="edit_phone_${c.ContactID}" value="${c.ContactNo}" style="display:none;" />
            </td>
            <td>
                <span id="email_${c.ContactID}">${c.Email}</span>
                <input type="text" id="edit_email_${c.ContactID}" value="${c.Email}" style="display:none;" />
            </td>
            <td>
                <button onclick="return editContact(${c.ContactID});" class="action-btn btn btn-default">Edit</button>
                <button onclick="return deleteContact(${c.ContactID});" class="action-btn btn btn-danger">Delete</button>
                <button onclick="return makePrimary(${data.ClientID}, ${c.ContactID});" class="action-btn btn btn-success">Make as Primary</button>
                <button onclick="return updateContact(${data.ClientID}, ${c.ContactID});" class="action-btn btn btn-info" style="display:none;" id="save_${c.ContactID}">Save</button>
                <button id="cancel_${c.ContactID}" style="display:none;" onclick="return cancelEdit(${c.ContactID})" class="action-btn btn btn-secondary">Cancel</button>
            </td>
        </tr>`;
    });

    html += '</table>';

    // Add new contact row
    html += `
    <div class="contact-row">
        <input type="text" placeholder="Contact Person" id="person_${data.ClientID}" class="form-control" />
        <input type="text" placeholder="Phone" id="phone_new_${data.ClientID}" class="form-control" />
        <input type="text" placeholder="Email" id="email_new_${data.ClientID}" class="form-control" />

        <button onclick="return addContactInline(${data.ClientID});" class="btn btn-warning btn-sm">
            Add Contact
        </button>
    </div>
`;

    return html;
}

function makePrimary(clientId, contactId) {

    $.ajax({
        type: "POST",
        url: "ClientMaster.aspx/SetPrimaryContact",
        contentType: "application/json",
        data: JSON.stringify({
            clientId: clientId,
            contactId: contactId
        }),
        success: function () {

            Swal.fire({
                icon: 'success',
                title: 'Primary Updated',
                text: 'Primary contact changed successfully.'
            });

            $('#clm_table').DataTable().ajax.reload();
        },
        error: function () {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Something went wrong. Please try again.'
            });
        }
    });
    return false;
}

function cancelEdit(contactId) {

    // Restore text view
    $(`#name_${contactId}`).show();
    $(`#phone_${contactId}`).show();
    $(`#email_${contactId}`).show();

    // Hide inputs
    $(`#edit_name_${contactId}`).hide();
    $(`#edit_phone_${contactId}`).hide();
    $(`#edit_email_${contactId}`).hide();

    // Hide Save/Cancel
    $(`#save_${contactId}`).hide();
    $(`#cancel_${contactId}`).hide();

    // 🔥 Show all buttons again
    $(".action-btn").show();
    $(`#save_${contactId}`).hide();
    $(`#cancel_${contactId}`).hide();
    return false;
}

function addContactInline(clientId) {
    let person = $(`#person_${clientId}`).val().trim();
    let phone = $(`#phone_new_${clientId}`).val().trim();
    let email = $(`#email_new_${clientId}`).val().trim();

    // 🔴 VALIDATION
    if (!person || !phone || !email) {
        Swal.fire({
            icon: 'warning',
            title: 'Validation Error',
            text: 'All fields (Contact Person, Phone, Email) are required.'
        });
        return false;
    }

    // 🔴 OPTIONAL: Email format validation
    let emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(email)) {
        Swal.fire({
            icon: 'warning',
            title: 'Invalid Email',
            text: 'Please enter a valid email address.'
        });
        return false;
    }

    // 🔴 OPTIONAL: Phone validation (only digits)
    let phoneRegex = /^[0-9]{7,15}$/;
    if (!phoneRegex.test(phone)) {
        Swal.fire({
            icon: 'warning',
            title: 'Invalid Phone',
            text: 'Phone must be numeric and 7–15 digits.'
        });
        return false;
    }

    let obj = {
        ClientID: clientId,
        ContactPerson: $(`#person_${clientId}`).val(),
        ContactNo: $(`#phone_new_${clientId}`).val(),
        Email: $(`#email_new_${clientId}`).val()
    };

    $.ajax({
        type: "POST",
        url: "ClientMaster.aspx/SaveSingleContact",
        contentType: "application/json",
        data: JSON.stringify({ model: obj }),
        success: function () {

            Swal.fire({
                icon: 'success',
                title: 'Contact Added',
                text: 'New contact has been added successfully.'
            });

            $('#clm_table').DataTable().ajax.reload();
        },
        error: function () {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Something went wrong. Please try again.'
            });
        }
    });

    return false;
}

function editContact(contactId) {
    $(".action-btn").hide();
    $(`#name_${contactId}`).hide();
    $(`#phone_${contactId}`).hide();
    $(`#email_${contactId}`).hide();

    $(`#edit_name_${contactId}`).show();
    $(`#edit_phone_${contactId}`).show();
    $(`#edit_email_${contactId}`).show();

    $(`#save_${contactId}`).show();
    $(`#cancel_${contactId}`).show();
    return false;
}

function updateContact(clientId, contactId) {

    let obj = {
        ContactID: contactId,
        ClientID: clientId,
        ContactPerson: $(`#edit_name_${contactId}`).val(),
        ContactNo: $(`#edit_phone_${contactId}`).val(),
        Email: $(`#edit_email_${contactId}`).val()
    };

    $.ajax({
        type: "POST",
        url: "ClientMaster.aspx/UpdateContact",
        contentType: "application/json",
        data: JSON.stringify({ model: obj }),
        success: function () {

            Swal.fire({
                icon: 'success',
                title: 'Updated',
                text: 'Contact details updated successfully.'
            });
            $('#clm_table').DataTable().ajax.reload();
        },
        error: function () {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Something went wrong. Please try again.'
            });
        }
    });
    return false;
}
function deleteContact(contactId) {

    Swal.fire({
        title: 'Are you sure?',
        text: 'This contact will be permanently deleted!',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#6c757d',
        confirmButtonText: 'Yes, delete it'
    }).then((result) => {

        if (result.isConfirmed) {

            $.ajax({
                type: "POST",
                url: "ClientMaster.aspx/DeleteContact",
                contentType: "application/json",
                data: JSON.stringify({ contactId: contactId }),
                success: function () {

                    Swal.fire({
                        icon: 'success',
                        title: 'Deleted',
                        text: 'Contact deleted successfully.'
                    });

                    $('#clm_table').DataTable().ajax.reload();
                },
                error: function () {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Something went wrong. Please try again.'
                    });
                }
            });
        }
    });
    return false;
}