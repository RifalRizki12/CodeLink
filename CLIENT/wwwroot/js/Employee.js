$(document).ready(function () {
    $('#tableEmployee').DataTable({
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'copy',
                text: 'Copy',
                className: 'btn btn-dark btn-sm',
                exportOptions: {
                    columns: ':visible:not(.no-export)'
                }
            },
            {
                extend: 'excel',
                text: 'Export to Excel',
                className: 'btn btn-success btn-sm',
                exportOptions: {
                    columns: ':visible:not(.no-export)'
                },
            },
            {
                extend: 'pdf',
                text: 'Export to PDF',
                className: 'btn btn-danger btn-sm',
                exportOptions: {
                    columns: ':visible:not(.no-export)'
                },
                customize: function (doc) {
                    doc.pageOrientation = 'landscape';
                    doc.pageSize = 'A3';
                }
            },
            {
                extend: 'print',
                text: 'Print',
                className: 'btn btn-info btn-sm',
                exportOptions: {
                    columns: ':visible:not(.no-export)'
                },
                customize: function (win) {
                    $(win.document.body).css('font-size', '12px');
                    $(win.document.body).find('table').addClass('compact').css('font-size', 'inherit');
                }
            },
            {
                extend: 'colvis',
                className: 'btn btn-primary btn-sm',
                postfixButtons: ['colvisRestore']
            }
        ],
        scrollX: true,
        columnDefs: [
            {
                visible: false
            }
        ],
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
            {
                data: 'foto',
                render: function (data, type, row) {
                    if (type === 'display' && data) {
                        const baseURL = "https://localhost:7051/"; // Gantilah URL dasar sesuai dengan kebutuhan Anda
                        const photoURL = `${baseURL}ProfilePictures/${data}`; // Gabungkan baseURL dengan path gambar
                        return `
                        <div class="text-center">
                            <img src="${photoURL}" alt="Employee Photo" style="max-width: 100px; max-height: 100px;">
                        </div>`;
                    }
                    return 'N/A'; // Pesan jika URL gambar tidak tersedia
                }
            },
            { data: 'fullName' },
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
            {
                data: 'statusEmployee',
                render: function (data, type, row) {
                    var statusText = data;
                    var badgeClass = "bg-success";

                    if (data === "idle") {
                        statusText = "Idle";
                    } else if (data === "onsite") {
                        statusText = "On Site";
                        badgeClass = "bg-warning";
                    }

                    return `
                        <div class="text-center">
                            <span class="badge bg-glow ${badgeClass}">${statusText}</span>
                        </div>`;
                }

            },
            {
                data: 'cv',
                render: function (data, type, row) {
                    if (type === 'display' && data) {
                        const baseURL = "https://localhost:7051/";
                        const cvURL = `${baseURL}Cv/${data}`; // Gabungkan baseURL dengan path cv
                        return `
                        <div class="text-center">
                            <a href="${cvURL}" target="_blank"><button class="btn btn-dark btn-sm"><i class="fa-solid fa-eye"></i></button></a>
                        </div>`;
                    }
                    return `
                    <div class="text-center">
                        <span class="badge bg-glow bg-secondary">N/A</span>
                    </div>`; // Pesan jika URL gambar tidak tersedia
                }
            },
            { data: 'statusAccount' },
            {
                data: null,
                render: function (data, type, row) {
                    return `
                     <div class="text-center">
                        <button type="button" class="btn btn-danger nonAktif" data-guid="${data.guid}" data-bs-toggle="modal" data-bs-target="#modalEditEmployee">NonAktif</button>
                    </div> <br/> 
                    <div class="text-center">
                        <button type="button" class="btn btn-warning edit-button" data-guid="${data.guid}" data-bs-toggle="modal" data-bs-target="#modalEditEmployee">Update</button>
                    </div>


                    `;
                }
            },

        ]
    });
    $('.dt-buttons').removeClass('dt-buttons');

    var updateIdleGuid; //menyimpan guid di tombol save
    var employeeGuid;

    $('#tableEmployee').on('click', '.edit-button', function () {
        updateIdleGuid = $(this).data('guid'); // Mengambil GUID dari tombol "Update" yang diklik
        console.log('Employee Guid update', updateIdleGuid);
        getIdleByGuid(updateIdleGuid);
    });

    $('#tableEmployee').on('click', '.nonAktif', function () {
        var guid = $(this).data('guid');
        updateAccountStatus(guid);
    });

    // Tambahkan event listener untuk tombol "Edit"

    function getIdleByGuid(guid) {
        $.ajax({
            url: '/Employee/GetGuidEmployee/' + guid,
            type: 'GET',
            dataType: 'json',
            dataSrc: 'data',
            success: function (data) {
                console.log(data);
                if (data) {
                    const baseURL = "https://localhost:7051/";
                    const imageUrl1 = `${baseURL}ProfilePictures/${data.foto}`;
                    var imageUrl2 = `${baseURL}Cv/${data.cv}`;

                    // Isi formulir modal dengan data karyawan
                    employeeGuid = data.guid;
                    $('#editFirstName').val(data.firstName);
                    $("#editLastName").val(data.lastName);
                    $("#editGender").val(data.gender);
                    $("#editEmail").val(data.email);
                    $("#editPhoneNumber").val(data.phoneNumber);
                    $("#editGrade").val(data.grade);
                    $("#editStatusEmploye").val(data.statusEmployee);
                    // Inisialisasi select2

                    // Saat menerima data dari AJAX:
                    if (!$('#editSkills').data('select2')) {
                        $('#editSkills').select2({
                            tags: true,
                            tokenSeparators: [',', ' '],
                            placeholder: "Select or add skills"
                        });
                    }

                    var skillSelect = $('#editSkills');
                    skillSelect.empty(); // Bersihkan opsi yang ada

                    // Menambahkan skill ke dropdown dari data AJAX
                    if (data && data.skill) {
                        var selectedSkills = [];

                        data.skill.forEach(function (skill) {
                            var newOption = new Option(skill, skill, false, true); // Parameter ke-4 diset true agar option tersebut otomatis terpilih
                            skillSelect.append(newOption);
                            selectedSkills.push(skill);
                        });

                        skillSelect.val(selectedSkills).trigger('change'); // Set skill yang telah dipilih
                    }

                    // Jika Anda ingin menandai beberapa skill tertentu sebagai dipilih (misalnya dari data lain), Anda dapat menggunakan bagian kode ini
                    if (data && data.selectedSkills) {
                        skillSelect.val(data.selectedSkills).trigger('change');
                    }

                    $("#InputFoto").text(data.foto);

                    $("#editProfilePicPreview").attr("src", imageUrl1);

                    $("#AddCv").text(data.cv);

                    $("#cvPreview").attr("src", imageUrl2);

                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Failed to retrieve employee data.'
                    });
                }
            },
            error: function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Failed to retrieve employee data.'
                });
            }
        });
    }

    $('.spinner-border').hide();
    $('.btn-save').show();

    $('#updateIdleForm').submit(function (event) {
        event.preventDefault();
        // Pastikan employeeGuid dan companyGuid telah di-set
        if (!employeeGuid) {
            console.error("employeeGuid belum di-set.");
            return;
        }
        $('.btn-save').attr('disabled', true);
        $('.btn-save').text('Create...');

        $('.spinner-border').show();

        updateIdleDetails(employeeGuid);
        setTimeout(function () {
            $('.btn-save').attr('disabled', false);
            $('.btn-save').text('Save');
            $('.spinner-border').hide();
        }, 5000);
    });


    // function update data idle
    function updateIdleDetails(employeeGuid) {
        // Ambil semua data dari elemen-elemen formulir
        var firstName = $('#editFirstName').val();
        var lastName = $('#editLastName').val();
        var phoneNumber = $('#editPhoneNumber').val();
        var email = $('#editEmail').val();
        var gender = ($('#editGender').val() === 'Male') ? 1 : 0;
        var grade = ($("#editGrade").val() === 'B') ? 1 : 0;
        var statusEmployee = ($("#editStatusEmploye").val() === 'owner') ? 1 : 0;
        var skills = $('#editSkills').val();

        var profilePictureInput = document.getElementById('InputFoto');
        var profilePictureFile = profilePictureInput.files[0];

        var cvInput = document.getElementById('AddCv');
        var cvFile = cvInput.files[0];

        // Buat objek FormData dan tambahkan data
        var dataToUpdate = new FormData();
        dataToUpdate.append('guid', employeeGuid);
        dataToUpdate.append('firstName', firstName);
        dataToUpdate.append('lastName', lastName);
        dataToUpdate.append('phoneNumber', phoneNumber);
        dataToUpdate.append('email', email);
        dataToUpdate.append('gender', gender);
        dataToUpdate.append('grade', grade);
        dataToUpdate.append('statusEmployee', statusEmployee);
        dataToUpdate.append('ProfilePictureFile', profilePictureFile);
        dataToUpdate.append('cvFile', cvFile);

        if (typeof skills === 'string') {
            skills.split(',').forEach((skill, index) => {
                dataToUpdate.append('skills[' + index + ']', skill.trim());
            });
        } else if (Array.isArray(skills)) {
            skills.forEach((skill, index) => {
                dataToUpdate.append('skills[' + index + ']', skill.trim());
            });
        } else {
            Swal.fire({
                title: 'Format Skill Salah !',
                icon: 'info',
                html: 'Skills harus berupa string atau array',
                showCloseButton: false,
                focusConfirm: false,
                customClass: {
                    confirmButton: 'btn btn-primary'
                },
                buttonsStyling: false
            });
        }

        $.ajax({
            url: '/Employee/updateIdle',
            type: 'PUT',
            data: dataToUpdate,
            contentType: false,
            processData: false, // Diperlukan untuk FormData
            success: function (response) {
                $('spinner-border').hide();
                $('#tableEmployee').DataTable().ajax.reload();
                if (response.status == "OK") {
                    $('#modalEditEmployee').modal('hide');
                    Swal.fire({
                        icon: 'success',
                        title: 'Pembaruan berhasil',
                        text: 'Data idle berhasil diperbarui.',
                        showCloseButton: false,
                        focusConfirm: false,
                        customClass: {
                            confirmButton: 'btn btn-primary'
                        },
                        buttonsStyling: false,
                    });
                }
                else if (response.status === "Error") {
                    $('spinner-border').hide();
                    Swal.fire({
                        icon: 'error',
                        title: 'Gagal Update!',
                        text: response.message.error || response.message.message,
                        showCloseButton: false,
                        focusConfirm: false,
                        customClass: {
                            confirmButton: 'btn btn-primary'
                        },
                        buttonsStyling: false,
                    });
                }
            },
            error: function (response) {
                $('spinner-border').hide();
                Swal.fire({
                    icon: 'error',
                    title: 'Pembaruan gagal',
                    text: 'Terjadi kesalahan saat mencoba update data Idle.',
                    showCloseButton: false,
                    focusConfirm: false,
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false,
                });
            }
        });
    }


    function updateAccountStatus(guid) {
        $.ajax({
            url: '/Account/GuidAccount/' + guid,
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
                        status: 4,
                    };

                    $.ajax({
                        url: '/Account/UpdateAccount/' + guid,
                        type: 'PUT',
                        data: JSON.stringify(dataToUpdate),
                        contentType: 'application/json',

                        success: function (response) {
                            $('#tableClient').DataTable().ajax.reload();
                            Swal.fire({
                                icon: 'success',
                                title: 'Pembaruan berhasil',
                                text: 'Status akun klien telah diubah !',
                                showCloseButton: false,
                                focusConfirm: false,
                                customClass: {
                                    confirmButton: 'btn btn-primary'
                                },
                                buttonsStyling: false,
                            });
                        },
                        error: function () {
                            Swal.fire({
                                icon: 'error',
                                title: 'Pembaruan Gagal',
                                text: 'Terjadi kesalahan saat mencoba mengubah status akun klien !',
                                showCloseButton: false,
                                focusConfirm: false,
                                customClass: {
                                    confirmButton: 'btn btn-primary'
                                },
                                buttonsStyling: false,
                            });
                        }
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Data tidak ditemukan',
                        text: 'Data akun klien tidak ditemukan !',
                        showCloseButton: false,
                        focusConfirm: false,
                        customClass: {
                            confirmButton: 'btn btn-primary'
                        },
                        buttonsStyling: false,
                    });
                }
            },
            error: function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Kesalahan',
                    text: 'Terjadi kesalahan saat mencoba mendapatkan data akun klien.',
                    showCloseButton: false,
                    focusConfirm: false,
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false,
                });
            }
        });
    }

});
$(document).ready(function () {

    //MEMBUAT REGISTER IDLE
    $('#createEmployeeForm').on('submit', function (event) {
        event.preventDefault();

        var profilePictureFile = $('#profilePictureInput').prop('files')[0];
        var cvFile = $('#cvInput').prop('files')[0];

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
        formData.append('profilePictureFile', profilePictureFile);

        var cvFile = $('#cvInput').prop('files')[0];

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
            Swal.fire({
                title: 'Format Skill Salah !',
                icon: 'info',
                html: 'Skills harus berupa string atau array',
                showCloseButton: false,
                focusConfirm: false,
                customClass: {
                    confirmButton: 'btn btn-primary'
                },
                buttonsStyling: false
            });
        }

        // Kirim data dengan metode AJAX ke server
        $.ajax({
            url: '/Employee/RegisterIdle',
            type: 'POST',
            data: formData,
            processData: false,  // penting, jangan proses data
            contentType: false,  // penting, biarkan jQuery mengatur ini
            success: function (response) {
                if (response.status == "OK") {
                    $('#modalCenter').modal('hide');
                    $('#tableEmployee').DataTable().ajax.reload;
                    Swal.fire({
                        icon: 'success',
                        title: 'Register Berhasil!',
                        text: response.message.message,
                        showCloseButton: false,
                        focusConfirm: false,
                        customClass: {
                            confirmButton: 'btn btn-primary'
                        },
                        buttonsStyling: false,
                    });
                }
                else if (response.status === "Error") {
                    Swal.fire({
                        icon: 'error',
                        title: 'Gagal Register!',
                        text: response.message.error || response.message.message,
                        showCloseButton: false,
                        focusConfirm: false,
                        customClass: {
                            confirmButton: 'btn btn-primary'
                        },
                        buttonsStyling: false,
                    });
                }
            },
            error: function () {
                $('#modalCenter').modal('hide');
                Swal.fire({
                    icon: 'error',
                    title: 'Pembaruan gagal',
                    text: 'Terjadi kesalahan saat register data.'
                });
            }
        });
    });

});

$(document).ready(function () {
    $('#tableClient').DataTable({
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'copy',
                text: 'Copy',
                className: 'btn btn-dark btn-sm',
                exportOptions: {
                    columns: ':visible:not(.no-export)'
                }
            },
            {
                extend: 'excel',
                text: 'Export to Excel',
                className: 'btn btn-success btn-sm',
                exportOptions: {
                    columns: ':visible:not(.no-export)'
                },
            },
            {
                extend: 'pdf',
                text: 'Export to PDF',
                className: 'btn btn-danger btn-sm',
                exportOptions: {
                    columns: ':visible:not(.no-export)'
                },
                customize: function (doc) {
                    doc.pageOrientation = 'landscape';
                    doc.pageSize = 'A3';
                }
            },
            {
                extend: 'print',
                text: 'Print',
                className: 'btn btn-info btn-sm',
                exportOptions: {
                    columns: ':visible:not(.no-export)'
                },
                customize: function (win) {
                    $(win.document.body).css('font-size', '12px');
                    $(win.document.body).find('table').addClass('compact').css('font-size', 'inherit');
                }
            },
            {
                extend: 'colvis',
                className: 'btn btn-primary btn-sm',
                postfixButtons: ['colvisRestore']
            }
        ],
        scrollX: true,
        columnDefs: [
            {
                visible: false
            }
        ],

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
            {
                data: 'fotoEmployee',
                render: function (data, type, row) {
                    if (type === 'display' && data) {
                        const baseURL = "https://localhost:7051/"; // Gantilah URL dasar sesuai dengan kebutuhan Anda
                        const photoURL = `${baseURL}ProfilePictures/${data}`; // Gabungkan baseURL dengan path gambar
                        return `<img src="${photoURL}" alt="Employee Photo" style="max-width: 100px; max-height: 100px;">`;
                    }
                    return 'N/A'; // Pesan jika URL gambar tidak tersedia
                }
            },
            { data: 'fullName' },
            { data: 'gender' },
            { data: 'email' },
            {
                data: 'statusAccount',
                render: function (data, type, row) {
                    var statusText = data;
                    var badgeClass = "bg-success";

                    if (data === "Approved") {
                        statusText = "Approved";
                    } else if (data === "Requested") {
                        statusText = "Requested";
                        badgeClass = "bg-dark"; 
                    } else if (data === "Rejected") {
                        statusText = "Reject";
                        badgeClass = "bg-danger";
                    } else if (data === "NonAktif") {
                        statusText = "NonAktif";
                        badgeClass = "bg-secondary";
                    } 

                    return `
                        <div class="text-center">
                            <span class="badge bg-glow ${badgeClass}">${statusText}</span>
                        </div>`;
                }
            },
            { data: 'phoneNumber' },
            /*{ data: 'statusEmployee' },*/
            { data: 'nameCompany' },
            { data: 'address' },
            {
                data: null,
                render: function (data, type, row) {

                    return `<div class="btn-group">
                            <button type="button" class="btn btn-primary waves-effect waves-light btn-sm">Actions</button>
                            <button type="button" class="btn btn-primary dropdown-toggle dropdown-toggle-split waves-effect waves-light" data-bs-toggle="dropdown" aria-expanded="false">
                              <span class="visually-hidden">Toggle Dropdown</span>
                            </button>
                            <ul class="dropdown-menu" style="">
                                 <a class="dropdown-item" data-guid="${data.employeeGuid}" data-status="1">Approve</a>
                                 <a class="dropdown-item" data-guid="${data.employeeGuid}" data-status="2">Reject</a>
                                 <a class="dropdown-item" data-guid="${data.employeeGuid}" data-status="4">Non Active</a>
                            </ul>
                          </div><br/><br/>
                         <button type="button" class="btn btn-warning btn-sm btn-update" data-guid="${data.employeeGuid}" data-bs-toggle="modal" data-bs-target="#modalUpdateClient">Update</button> `;
                }
            },
        ]
    });
    $('.dt-buttons').removeClass('dt-buttons');

    //Action tombol dropdown
    $('#tableClient').on('click', '.dropdown-item', function () {
        var guid = $(this).data('guid');
        var status = $(this).data('status');
        updateAccountStatus(guid, status);
    });


    // function update Account status
    function updateAccountStatus(guid, status) {
        $.ajax({
            url: '/Account/GuidAccount/' + guid,
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
                        url: '/Account/UpdateAccount/' + guid,
                        type: 'PUT',
                        data: JSON.stringify(dataToUpdate),
                        contentType: 'application/json',

                        success: function (response) {
                            $('#tableClient').DataTable().ajax.reload();
                            Swal.fire({
                                icon: 'success',
                                title: 'Pembaruan berhasil',
                                text: 'Status akun klien telah diubah !',
                                showCloseButton: false,
                                focusConfirm: false,
                                customClass: {
                                    confirmButton: 'btn btn-primary'
                                },
                                buttonsStyling: false,
                            });
                        },
                        error: function () {
                            Swal.fire({
                                icon: 'error',
                                title: 'Pembaruan Gagal',
                                text: 'Terjadi kesalahan saat mencoba mengubah status akun klien !',
                                showCloseButton: false,
                                focusConfirm: false,
                                customClass: {
                                    confirmButton: 'btn btn-primary'
                                },
                                buttonsStyling: false,
                            });
                        }
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Data tidak ditemukan',
                        text: 'Data akun klien tidak ditemukan !',
                        showCloseButton: false,
                        focusConfirm: false,
                        customClass: {
                            confirmButton: 'btn btn-primary'
                        },
                        buttonsStyling: false,
                    });
                }
            },
            error: function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Kesalahan',
                    text: 'Terjadi kesalahan saat mencoba mendapatkan data akun klien.',
                    showCloseButton: false,
                    focusConfirm: false,
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false,
                });
            }
        });
    }

    var updateGuid; //menyimpan guid di tombol save
    var employeeGuid;
    var companyGuid;
    //  //Action tombol Update
    $('#tableClient').on('click', '.btn-update', function () {
        updateGuid = $(this).data('guid'); // Mengambil GUID dari tombol "Update" yang diklik
        console.log('Company Guid update', updateGuid);
        getClientByGuid(updateGuid);
    });
    //function untuk menampilkan data client di form modal update
    function getClientByGuid(guid) {
        console.log("ini parameter guid " + guid);

        $.ajax({
            url: '/Employee/GetGuidClient/' + guid,
            type: 'GET',
            dataType: 'json',
            dataSrc: 'data',
            success: function (response) {
                var imageUrl = 'https://localhost:7051/Utilities/File/ProfilePictures/' + response.fotoEmployee;
                console.log(response);
                employeeGuid = response.employeeGuid;
                companyGuid = response.companyGuid;
                console.log("ini employee guid " + employeeGuid);
                console.log("ini companyGuid " + companyGuid);
                $("#editFirstName").val(response.firstName);
                $("#editLastName").val(response.lastName);
                $("#editGender").val(response.gender);
                $("#editEmail").val(response.email);
                $("#editPhoneNumber").val(response.phoneNumber);
                $("#companyName").val(response.nameCompany);
                $("#addressCompany").val(response.address);
                $("#description").val(response.description);

                // Tampilkan nama file gambar sebelum diupdate
                $("#profilePictureInput").text(response.fotoEmployee);

                $("#editProfilePicPreview").attr("src", imageUrl);

            },
            error: function (response) {
                Swal.fire({
                    icon: 'error',
                    title: 'Kesalahan',
                    text: 'Terjadi kesalahan saat mencoba mendapatkan data klien.'
                });
            }
        });
    }

    // Event handler untuk tombol "Save"
    $('#updateClientForm').submit(function (event) {
        event.preventDefault();
        // Pastikan employeeGuid dan companyGuid telah di-set
        if (!employeeGuid || !companyGuid) {
            console.error("employeeGuid atau companyGuid belum di-set.");
            return;
        }
        $('.btn-save').attr('disabled', true);
        $('.btn-save').text('Saving...');
        $('.spinner-border').show();

        updateClientDetails(employeeGuid, companyGuid);
        setTimeout(function () {
            $('.btn-save').attr('disabled', false);
            $('.btn-save').text('Save');
            $('.spinner-border').hide();
        }, 5000);
    });


    // function update data Client
    function updateClientDetails(employeeGuid, companyGuid) {
        console.log("ini di parameter update ");

        // Ambil semua data dari elemen-elemen formulir
        var firstName = $('#editFirstName').val();
        var lastName = $('#editLastName').val();
        var phoneNumber = $('#editPhoneNumber').val();
        var email = $('#editEmail').val();
        var gender = ($('#editGender').val() === 'Male') ? 1 : 0;
        var nameCompany = $('#companyName').val();
        var addressCompany = $('#addressCompany').val();
        var description = $('#description').val();

        var profilePictureFile = $('#profilePictureInput').prop('files')[0];

        var profilePictureInput = document.getElementById('profilePictureInput');  // Ambil file gambar dari input
        var profilePictureFile = profilePictureInput.files[0];

        // Buat objek FormData dan tambahkan data
        var dataToUpdate = new FormData();
        dataToUpdate.append('employeeGuid', employeeGuid);
        dataToUpdate.append('companyGuid', companyGuid);
        dataToUpdate.append('firstName', firstName);
        dataToUpdate.append('lastName', lastName);
        dataToUpdate.append('phoneNumber', phoneNumber);
        dataToUpdate.append('email', email);
        dataToUpdate.append('gender', gender);
        dataToUpdate.append('nameCompany', nameCompany);
        dataToUpdate.append('addressCompany', addressCompany);
        dataToUpdate.append('description', description);
        dataToUpdate.append('ProfilePictureFile', profilePictureFile);

        $.ajax({
            url: '/Employee/UpdateClient',
            type: 'PUT',
            data: dataToUpdate,
            contentType: false,
            processData: false, // Diperlukan untuk FormData
            success: function (response) {
                $('spinner-border').hide();
                $('#tableClient').DataTable().ajax.reload();
                if (response.status == "OK") {
                    $('#modalUpdateClient').modal('hide');
                    Swal.fire({
                        icon: 'success',
                        title: 'Pembaruan berhasil',
                        text: 'Data client berhasil diperbarui.',
                        showCloseButton: false,
                        focusConfirm: false,
                        customClass: {
                            confirmButton: 'btn btn-primary'
                        },
                        buttonsStyling: false,
                    });
                }
                else if (response.status === "Error") {
                    $('spinner-border').hide();
                    Swal.fire({
                        icon: 'error',
                        title: 'Gagal Update!',
                        text: response.message.error || response.message.message,
                        showCloseButton: false,
                        focusConfirm: false,
                        customClass: {
                            confirmButton: 'btn btn-primary'
                        },
                        buttonsStyling: false,
                    });
                }

            },
            error: function (xhr, status, error) {
                $('spinner-border').hide();
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'An error occurred while updating employee data.'
                });
            }
        });

    }
});