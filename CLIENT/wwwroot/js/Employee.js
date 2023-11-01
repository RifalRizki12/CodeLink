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
            {
                data: 'foto',
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
                    return `<button type="button" class="btn btn-primary edit-button" data-guid="${data.guid}" data-bs-toggle="modal" data-bs-target="#modalEditEmployee">Update</button>
                            <button type="button" class="btn btn-warning detail-button" data-guid="${row.guid}">-</button>`;
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
                    var imageUrl1 = 'https://localhost:7051/Utilities/File/ProfilePictures/' + data.foto;
                    var imageUrl2 = 'https://localhost:7051/Utilities/File/Cv/' + data.cv;
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

                    $("#profilePictureInput").text(data.foto);

                    $("#editProfilePicPreview").attr("src", imageUrl1);

                    $("#cvInput").text(data.cv);

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
    $('#updateIdleForm').submit(function (event) {
        event.preventDefault();
        // Pastikan employeeGuid dan companyGuid telah di-set
        if (!employeeGuid) {
            console.error("employeeGuid belum di-set.");
            return;
        }
        updateIdleDetails(employeeGuid);
    });


    // function update data Client
    function updateIdleDetails(employeeGuid) {
        console.log("ini di parameter update ");

        // Ambil semua data dari elemen-elemen formulir
        var firstName = $('#editFirstName').val();
        var lastName = $('#editLastName').val();
        var phoneNumber = $('#editPhoneNumber').val();
        var email = $('#editEmail').val();
        var gender = ($('#editGender').val() === 'Male') ? 1 : 0;
        var grade = ($("#editGrade").val() === 'B') ? 1 : 0;
        var statusEmployee = ($("#editStatusEmploye").val() === 'owner') ? 1 : 0;
        var skills = $('#editSkills').val(); // Menambahkan data skill ke variabel

        // Ambil file gambar dari input
        var profilePictureInput = document.getElementById('profilePictureInput');
        console.log(profilePictureInput.files);
        var profilePictureFile = profilePictureInput.files[0];

        var cvInput = document.getElementById('cvInput');
        var cvFile = cvInput.files[0];

        console.log(firstName);
        console.log(lastName);
        console.log(phoneNumber);
        console.log(email);
        console.log(gender);
        console.log(grade);
        console.log(statusEmployee);
        console.log(skills);


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
                showCloseButton: true,
                focusConfirm: false,
                confirmButtonText: '<i class="fa fa-thumbs-up"></i> Great!',
                confirmButtonAriaLabel: 'Thumbs up, great!',
            });
        }

        if (profilePictureFile) {
            dataToUpdate.append('ProfilePictureFile', profilePictureFile);
        } else {
            console.log("No profile picture file selected");
        }
        if (cvFile) {
            dataToUpdate.append('cvFile', cvFile);
        } else {
            console.log("No CV file selected");
        }

        $.ajax({
            url: '/Employee/updateIdle',
            type: 'PUT',
            data: dataToUpdate,
            contentType: false,
            processData: false, // Diperlukan untuk FormData
            success: function (response) {
                $('#modalEditEmployee').modal('hide');
                $('#tableEmployee').DataTable().ajax.reload();
                Swal.fire({
                    icon: 'success',
                    title: 'Pembaruan berhasil',
                    text: 'Data Idle berhasil diperbarui.'
                });
            },
            error: function (response) {
                Swal.fire({
                    icon: 'error',
                    title: 'Pembaruan gagal',
                    text: 'Terjadi kesalahan saat mencoba update data Idle.'
                });
            }
        });
    }
});
$(document).ready(function () {

    //MEMBUAT REGISTER IDLE
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
                showCloseButton: true,
                focusConfirm: false,
                confirmButtonText: '<i class="fa fa-thumbs-up"></i> Great!',
                confirmButtonAriaLabel: 'Thumbs up, great!',
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
                $('#modalCenter').modal('hide');
                if (response.redirectTo) {
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
                $('#modalCenter').modal('hide');
                Swal.fire({
                    icon: 'error',
                    title: 'Pembaruan gagal',
                    text: 'Terjadi kesalahan saat register data.'
                });
            }
        });
    });
    // Tambahkan event listener untuk tombol "Save"
    /*  $('#editEmployeeButton').on('click', function () {
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
                  console.log(data);
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
      });*/


    //Menampilkan data Client (Update Status client, Update Data Client)

});

$(document).ready(function () {
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
                         <button type="button" class="btn btn-primary btn-update" data-guid="${data.companyGuid}" data-bs-toggle="modal" data-bs-target="#modalUpdateClient">Update</button> `;
                }
            },
        ]
    });

    //Action tombol dropdown
    $('#tableClient').on('click', '.dropdown-item', function () {
        var guid = $(this).data('guid');
        var status = $(this).data('status');
        updateAccountStatus(guid, status);
    });


    // function update Account status
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

    //inisiasi variabel untuk nampung data sebelumnya

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
        updateClientDetails(employeeGuid, companyGuid);
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

        // Ambil file gambar dari input
        var profilePictureInput = document.getElementById('profilePictureInput');
        var profilePictureFile = profilePictureInput.files[0];
        console.log(profilePictureFile);

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

        // Cek apakah gambar sudah dipilih sebelum menambahkannya ke FormData
        if (profilePictureFile) {
            dataToUpdate.append('ProfilePictureFile', profilePictureFile);
        } else {
            alert('Harap pilih gambar profil sebelum mengirimkan permintaan.');
            return;
        }

        $.ajax({
            url: '/Employee/UpdateClient',
            type: 'PUT',
            data: dataToUpdate,
            contentType: false,
            processData: false, // Diperlukan untuk FormData
            success: function (response) {
                $('#modalUpdateClient').modal('hide');
                $('#tableClient').DataTable().ajax.reload();
                Swal.fire({
                    icon: 'success',
                    title: 'Pembaruan berhasil',
                    text: 'Data client berhasil diperbarui.'
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

    }
});