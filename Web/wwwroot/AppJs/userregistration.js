let table;
const hostUrl = 'https://localhost:7155';

$(document).ready(function () {
    initializeDataTable();
    loadDropdowns();

    $('#userForm').validate();

    $('#searchBtn').click(function () {
        initializeDataTable();
    });

    $('[name=State]').change(function () {
        const stateId = $(this).val();
        $('[name=City]').html('<option value="" disabled selected>Select City</option>');
        if (stateId) {
            $.get(`/Masters/dropdown?ddName=cities&parentId=${stateId}`, function (res) {
                if (res.isSuccess) {
                    $.each(res.data, function (i, item) {
                        $('[name=City]').append(`<option value="${item.id}">${item.name}</option>`);
                    });
                }
                else {
                    console.log(res.message);
                }
            });
        }
    });

    $('#filterState').change(function () {
        const stateId = $(this).val();
        $('#filterCity').html('<option value="" disabled selected>Select City</option>');
        if (stateId) {
            $.get(`/Masters/dropdown?ddName=cities&parentId=${stateId}`, function (res) {

                if (res.isSuccess) {
                    $.each(res.data, function (i, item) {
                        $('#filterCity').append(`<option value="${item.id}">${item.name}</option>`);
                    });
                }
                else {
                    console.log(res.message);
                }
            });
        }
    });

    $('#userForm').on('submit', function (e) {
        e.preventDefault();

        if (!$(this).valid()) return;

        const termsChecked = $('#termsCheckbox').is(':checked');

        if (!termsChecked) {
            Swal.fire({
                icon: 'warning',
                title: 'Terms & Conditions Required',
                text: 'Please accept the Terms and Conditions to proceed.'
            });
            return;
        }

        var formData = new FormData(this);
        $('#hobbyList input:checked').each(function () {
            formData.append("Hobbies", $(this).val());
        });

        $.ajax({
            url: `${hostUrl}/UserRegistration`,
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (data) {
                if (data.isSuccess) {
                    $('#userModal').modal('hide');
                    Swal.fire({
                        title: "Success",
                        text: data.message,
                        icon: "success"
                    });
                    $('#userForm')[0].reset();
                    initializeDataTable();
                }
                else
                {
                    Swal.fire({
                        title: "Error",
                        html: data.message,
                        icon: "error"
                    });
                    return;
                }
            },
            error: function (xhr) {
                alert("Error: " + xhr.responseText);
            }
        });
    });
});


function initializeDataTable() {
    $("#userTable").DataTable().destroy();
    table = $('#userTable').DataTable({
        processing: true,
        serverSide: true,
        searching: false,
        order: [[8, 'desc']], // Sort by CreatedDate descending
        ajax: function (data, callback) {
            const params = {
                start: data.start,
                length: data.length,
                searchValue: $('#searchName').val(),
                stateId: $('#filterState').val(),
                cityId: $('#filterCity').val(),
                sortColumn: data.columns[data.order[0].column].data,
                sortDirection: data.order[0].dir
            };

            $.get(`/User/GetAll`, params, function (res) {
                callback({
                    recordsTotal: res.data.recordsTotal,
                    recordsFiltered: res.data.recordsFiltered,
                    data: res.data.data
                });
            });
        },
        columnDefs: [
            {
                targets: [7, 8], // 0-based index: Hobbies, Photo
                orderable: false,
                className: 'no-sort' // Optional: add class to identify non-sortable columns
            }
        ],
        columns: [
            { data: 'name' },
            { data: 'gender' },
            { data: 'dateOfBirth', render: d => d.split('T')[0] },
            { data: 'email' },
            { data: 'mobile' },
            { data: 'state' },
            { data: 'city' },
            { data: 'hobbies', render: h => h.join(", ") },
            { data: 'photoPath', render: p => `<img src="${hostUrl}${p}" width="50" />` }
        ]
    });
}


function loadDropdowns() {
    $.get(`/Masters/dropdown?ddName=genders`, res => {
        $('[name=Gender]').html('<option value="" disabled selected>Select Gender</option>');
        if (res.isSuccess) {
            $.each(res.data, function (i, item) {
                $('[name=Gender]').append(`<option value="${item.id}">${item.name}</option>`);
            });
        }
        else {
            console.log(res);
            console.error(res.message);
        }
    });

    $.get(`/Masters/dropdown?ddName=states`, res => {
        if (res.isSuccess) {
            $('[name=State]').html('<option value="" disabled selected>Select State</option>');
            $('#filterState').html('<option value="" disabled selected>Select State</option>');
            $.each(res.data, function (i, item) {
                $('[name=State]').append(`<option value="${item.id}">${item.name}</option>`);
                $('#filterState').append(`<option value="${item.id}">${item.name}</option>`);
            });
        }
        else
        {
            console.error(res.message);
        }
    });

    $.get(`/Masters/dropdown?ddName=hobbies`, res => {
        if (res.isSuccess) {
            $('#hobbyList').html(res.data.map(o =>
                `<div class="form-check">
                    <input type="checkbox" name="HobbyIds" class="form-check-input" value="${o.id}"/> ${o.name}
                </div>`
            ));
        }
        else {
            console.error(res.message);
        }
    });
}