$(document).ready(function () {
    $('#tableEmployee').DataTable({
        ajax: {
            url: '/Employee/GetEmployeeData',
            type: 'GET',
            dataType: 'json',
            dataSrc: 'data',

        },
        columns: [
            {
                data: null,
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            { data: 'fullName'},
            {
                data: "gender",
                render: function (data) {
                    return data === 0 ? "Male" : "Female";
                }
            },
            { data: 'email' },
            { data: 'phoneNumber' },
            {
                data: 'skill', // Menggunakan atribut 'skill'
                render: function (data, type, row) {
                    // 'data' sekarang berisi array 'Skill'
                    if (Array.isArray(data) && data.length > 0) {
                        return data.join(', '); // Menggabungkan elemen dalam array dengan koma
                    } else {
                        return 'N/A'; // Pesan jika tidak ada 'Skill'
                    }
                }
            },
            { data: 'statusEmployee' },
            {
                data: null,
                render: function (data, type, row) {
                    return `<button type="button" class="btn btn-warning edit-button" data-guid="${row.guid}">Edit</button>
                            <button type="button" class="btn btn-danger delete-button" data-guid="${row.guid}">-</button>`;
                }
            },
        ]
    });
    $('.dt-buttons').removeClass('dt-buttons');

    // Tambahkan event listener untuk tombol "Edit"
    $(document).on("click", ".edit-button", function () {
        var employeeGuid = $(this).data("guid");
        
        $.ajax({
            url: '/Employee/GetEmployee/' + employeeGuid,
            type: 'GET',
            success: function (data) {
                if (data) {
                    // Isi formulir modal dengan data karyawan yang diambil dari API
                    $('#editEmployeeId').val(data.guid);
                    $('#editFirstName').val(data.firstName);
                    $('#editLastName').val(data.lastName);
                    $('#editGender').val(data.gender);
                    $('#editEmail').val(data.email);
                    $('#editPhoneNumber').val(data.phoneNumber);
                    $('#editGrade').val(data.grade);
                    $('#editStatusEmployee').val(data.statusEmployee);
                    $('#editCompanyGuid').val(data.companyGuid);

                    // Tampilkan modal edit
                    $('#modalEditEmployee').modal('show');
                } else {
                    console.error('Error: Unable to retrieve employee data.');
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Failed to retrieve employee data.'
                    });
                }
            },
            error: function (xhr) {
                console.error('Error: AJAX request failed.', xhr);
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Failed to retrieve employee data.'
                });
            }
        });
    });

    $('#createEmployeeForm').on('submit', function (event) {
        event.preventDefault();


        var formData = new FormData();

        // Tambahkan data teks ke formData
        formData.append('firstName', $('#firstNameInput').val());
        formData.append('lastName', $('#lastNameInput').val());
        formData.append('gender', parseInt($('#genderInput').val()));
        formData.append('phoneNumber', $('#phoneNumberInput').val());
        formData.append('email', $('#emailInput').val());
        formData.append('grade', parseInt($('#gradeInput').val()));
        formData.append('statusEmployee', parseInt($('#statusEmployeeInput').val()));
        formData.append('password', $('#passwordInput').val());
        formData.append('confirmPassword', $('#confirmPasswordInput').val());

        // Tambahkan file ke formData
        var profilePictureFile = $('#profilePictureInput').prop('files')[0];
        var cvFile = $('#cvInput').prop('files')[0];
        formData.append('profilePictureFile', profilePictureFile);
        formData.append('cvFile', cvFile);

        // Tambahkan skills (diasumsikan sebagai array string)

        var skills = $('#skillsInput').val();
        if (typeof skills === 'string') {
            skills.split(',').forEach((skill, index) => {
                formData.append('skills[' + index + ']', skill.trim());
            });
        } else if (Array.isArray(skills)) {
            skills.forEach((skill, index) => {
                formData.append('skills[' + index + ']', skill.trim());
            });
        } else {
            console.error('Skills harus berupa string atau array');
        }

        // Kirim data dengan metode AJAX ke server
        $.ajax({
            url: '/Employee/RegisterIdle',
            type: 'POST',
            data: formData,
            processData: false,  // penting, jangan proses data
            contentType: false,  // penting, biarkan jQuery mengatur ini
            success: function (response) {
                // Handle respons sukses atau pesan kesalahan
                if (response.redirectTo) {
                    // SweetAlert untuk login berhasil
                    Swal.fire({
                        icon: 'success',
                        title: 'Register Berhasil!',
                        text: 'Anda akan diarahkan ke halaman yang dituju.',
                    }).then(function () {
                        window.location.href = response.redirectTo;
                    });

                }
            },
            error: function () {
                alert('Terjadi kesalahan koneksi.');
            }
        });
    });
    // Tambahkan event listener untuk tombol "Save"
    $('#editEmployeeButton').on('click', function () {
        var updatedEmployee = {
            Guid: $('#editEmployeeId').val(),
            FirstName: $('#editFirstName').val(),
            LastName: $('#editLastName').val(),
            Gender: $('#editGender').val(),
            Email: $('#editEmail').val(),
            PhoneNumber: $('#editPhoneNumber').val(),
            Grade: $('#editGrade').val(),
            StatusEmployee: $('#editStatusEmployee').val(),
            CompanyGuid: $('#editCompanyGuid').val()
        };

        // Lakukan permintaan PUT ke API untuk memperbarui data karyawan
        $.ajax({
            url: '/Employee/UpdateIdle',
            type: 'PUT',
            dataType: 'json',
            data: JSON.stringify(updatedEmployee),
            contentType: 'application/json',
            success: function (data) {
                $('#modalEditEmployee').modal('hide');
                // Tambahkan logika lainnya seperti memperbarui tampilan tabel atau memberikan notifikasi
                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: 'Employee data updated successfully.'
                });
            },
            error: function (xhr, status, error) {
                if (xhr.status === 400) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Validation Error',
                        text: xhr.responseText
                    });
                } else if (xhr.status === 404) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Employee data not found.'
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'An error occurred while updating employee data.'
                    });
                }
                console.error(xhr.responseText);
            }
        });
    });


    //Action Account (Approve, non aktif, reject)
    $('#tableClient').DataTable({
        ajax: {
            url: '/Employee/GetClientData',
            type: 'GET',
            dataType: 'json',
            dataSrc: 'data',
        },
        columns: [
            {
                data: null,
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            { data: 'fullName' },
            { data: 'gender' },
            { data: 'email' },
            { data: 'statusAccount' },
            { data: 'phoneNumber' },
            { data: 'statusEmployee' },
            { data: 'nameCompany' },
            { data: 'address' },
            {
                data: null,
                render: function (data, type, row, meta) {

                    return `<div class="btn-group">
                                <button type="button" class="btn btn-danger waves-effect waves-light">Actions</button>
                                <button type="button" class="btn btn-danger dropdown-toggle dropdown-toggle-split waves-effect waves-light" data-bs-toggle="dropdown" aria-expanded="false">
                                  <span class="visually-hidden">Toggle Dropdown</span>
                                </button>
                                <ul class="dropdown-menu" style="">
                                     <a class="dropdown-item" data-guid="${data.employeeGuid}" data-status="1">Approve</a>
                                     <a class="dropdown-item" data-guid="${data.employeeGuid}" data-status="2">Reject</a>
                                     <a class="dropdown-item" data-guid="${data.employeeGuid}" data-status="4">Non Active</a>
                                </ul>
                              </div>
                             <button type="button" class="btn btn-primary" data-guid="${data.companyGuid}" data-bs-toggle="modal" data-bs-target="#modalUpdateClient">Update</button> `;
                }
            },
        ]
    });
    $('.dropdown-toggle').dropdown();
    // Tambahkan event handler untuk tombol "Approve"
    $('#tableClient').on('click', '.dropdown-item', function () {
        var guid = $(this).data('guid');
        var status = $(this).data('status');
        updateAccountStatus(guid, status);
    });

});


function updateClientDetails(companyGuid) {
    $.ajax({
        url: '/Account/UpdateClient/' + companyGuid,
        type: 'PUT',
        data: JSON.stringify(dataToUpdate),
        contentType: 'application/json',

        success: function (response) {
            $('#tableClient').DataTable().ajax.reload();
            Swal.fire({
                icon: 'success',
                title: 'Pembaruan berhasil',
                text: 'Status akun klien telah diubah.'
            });
        },
        error: function () {
            Swal.fire({
                icon: 'error',
                title: 'Pembaruan gagal',
                text: 'Terjadi kesalahan saat mencoba mengubah status akun klien.'
            });
        }
    });
}
        

function updateAccountStatus(guid, status) {
    $.ajax({
        url: '/Account/GuidClient/' + guid,
        type: 'GET',
        dataType: 'json',
        dataSrc: 'data',
        success: function (data) {
            if (data) {
                var dataToUpdate = {
                    guid: data.guid,
                    expiredTime: data.expiredTime,
                    isUsed: data.isUsed,
                    otp: data.otp,
                    password: data.password,
                    roleGuid: data.roleGuid,
                    status: status,
                };

                $.ajax({
                    url: '/Account/UpdateClient/' + guid,
                    type: 'PUT',
                    data: JSON.stringify(dataToUpdate),
                    contentType: 'application/json',

                    success: function (response) {
                        $('#tableClient').DataTable().ajax.reload();
                        Swal.fire({
                            icon: 'success',
                            title: 'Pembaruan berhasil',
                            text: 'Status akun klien telah diubah.'
                        });
                    },
                    error: function () {
                        Swal.fire({
                            icon: 'error',
                            title: 'Pembaruan gagal',
                            text: 'Terjadi kesalahan saat mencoba mengubah status akun klien.'
                        });
                    }
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Data tidak ditemukan',
                    text: 'Data akun klien tidak ditemukan.'
                });
            }
        },
        error: function () {
            Swal.fire({
                icon: 'error',
                title: 'Kesalahan',
                text: 'Terjadi kesalahan saat mencoba mendapatkan data akun klien.'
            });
        }
    });
}






