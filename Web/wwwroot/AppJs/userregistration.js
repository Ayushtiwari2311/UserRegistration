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
            $.get(`${hostUrl}/api/Masters/cities/${stateId}`, function (data) {
                $.each(data, function (i, item) {
                    $('[name=City]').append(`<option value="${item.id}">${item.name}</option>`);
                });
            });
        }
    });

    $('#filterState').change(function () {
        const stateId = $(this).val();
        $('#filterCity').html('<option value="" disabled selected>Select City</option>');
        if (stateId) {
            $.get(`${hostUrl}/api/Masters/cities/${stateId}`, function (data) {
                $.each(data, function (i, item) {
                    $('#filterCity').append(`<option value="${item.id}">${item.name}</option>`);
                });
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
            url: `${hostUrl}/api/UserRegistration`,
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (data) {
                console.log(data);
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
        ajax: function (data, callback) {
            const params = {
                start: data.start,
                length: data.length,
                searchValue: $('#searchName').val(),
                stateId: $('#filterState').val(),
                cityId: $('#filterCity').val()
            };

            $.get(`${hostUrl}/api/UserRegistration`, params, function (res) {
                callback({
                    recordsTotal: res.recordsTotal,
                    recordsFiltered: res.recordsFiltered,
                    data: res.data
                });
            });
        },
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
    $.get(`${hostUrl}/api/Masters/genders`, g => {
        $('[name=Gender]').html('<option value="" disabled selected>Select Gender</option>');
        $.each(g, function (i, item) {
            $('[name=Gender]').append(`<option value="${item.id}">${item.name}</option>`);
        });
    });

    $.get(`${hostUrl}/api/Masters/states`, s => {
        $('[name=State]').html('<option value="" disabled selected>Select State</option>');
        $.each(s, function (i, item) {
            $('[name=State]').append(`<option value="${item.id}">${item.name}</option>`);
        });

        $('#filterState').html('<option value="" disabled selected>Select State</option>');
        $.each(s, function (i, item) {
            $('#filterState').append(`<option value="${item.id}">${item.name}</option>`);
        });
    });

    $.get(`${hostUrl}/api/Masters/hobbies`, h => {
        $('#hobbyList').html(h.map(o =>
            `<div class="form-check">
                    <input type="checkbox" name="HobbyIds" class="form-check-input" value="${o.id}"/> ${o.name}
                </div>`
        ));
    });
}