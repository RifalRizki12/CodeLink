﻿$(document).ready(function () {
    var allData = []; // Data awal

    // Fungsi untuk menampilkan semua data
    function displayAllData(data) {
        var employeeGrid = $("#employeeGrid");
        employeeGrid.empty();

        // Fungsi untuk memotong teks
        function truncateText(text, maxLength) {
            if (text.length > maxLength) {
                return text.substring(0, maxLength);
            } else {
                return text;
            }
        }

        // Fungsi untuk menangani klik "Read More" dan "Hide"
        $(document).on('click', '.read-more', function () {
            var textContainer = $(this).closest('.text-container');
            var fullText = textContainer.attr('data-fulltext');
            var truncatedText = textContainer.attr('data-truncatedtext');
            var maxLength = parseInt(textContainer.attr('data-maxlength'));

            if (textContainer.hasClass('truncated')) {
                // Jika teks sudah dipotong, tampilkan seluruh teks
                textContainer.text(fullText + ' ').append('<span class="read-more">Read Less</span>');
                textContainer.removeClass('truncated');
            } else {
                // Jika teks belum dipotong, potong sesuai panjang maksimum
                textContainer.text(truncatedText + '... ').append('<span class="read-more">Read More</span>');
                textContainer.addClass('truncated');
            }
        });

        $.each(data, function (key, val) {
            if (val.statusEmployee === "idle" && val.foto != null && val.cv != null) {
                const baseURL = "https://localhost:7051/";
                const photoURL = `${baseURL}ProfilePictures/${val.foto}`;
                const cvURL = `${baseURL}Cv/${val.cv}`;

                var maxLength = 10; // Sesuaikan sesuai kebutuhan
                var skillText = val.skill.join(', ');
                var truncatedSkill = truncateText(skillText, maxLength);

                var showReadMore = skillText.length > maxLength;

                const employeeItem = `
                    <div class="col-lg-1-5 col-md-4 col-12 col-sm-6">
                      <div class="product-cart-wrap mb-30 wow animate__animated animate__fadeIn" data-wow-delay=".1s" style="box-shadow: 0px 4px 8px rgba(0, 0, 0, 0.15);">
                        <div class="product-img-action-wrap" style="margin-bottom: 0;">
                          <div class="product-img product-img-zoom" style="height: 200px; overflow: hidden; text-align: center;">
                            <a href="shop-product-right.html">
                              <img class="img-fluid" src="${photoURL}" alt="Employee Photo">
                            </a>
                          </div>
                        </div>
                            <div class="product-content-wrap" style="padding: 16px; text-align: center;">
                          <div class="product-category">
                            <span style="color: #6c757d; font-size: 0.9rem;">${val.statusEmployee}</span>
                          </div>
                          <h2 style="font-size: 1.2rem; font-weight: 600; margin-top: 8px;"><a href="" style="color: #333; text-decoration: none;">${val.fullName}</a></h2>
                          <div class="product-rate-cover" style="margin: 8px 0;">
                            <div class="product-rate d-inline-block">
                              <div class="product-rating" style="width: ${val.averageRating * 20}%;"></div>
                            </div>
                            <span class="font-small ml-5 text-muted"> (${val.averageRating})</span>
                          </div>
                          <div>
                            <span class="font-small text-muted">
                                <div class="text-container" data-fulltext="${skillText}" data-truncatedtext="${truncatedSkill}" data-maxlength="${maxLength}">
                                    ${showReadMore ? truncatedSkill + '... <span class="read-more">Read More</span>' : truncatedSkill}
                                </div>
                            </span>
                          </div>
                          <div class="product-card-bottom" style="margin-top: 12px;">
                            <div class="add-cart">
                              <button type="button" class="btn btn-outline-success btn-detail" data-guid="${val.guid}" data-bs-toggle="modal" data-bs-target="#modalDetail" style="margin-right: 8px;">Detail</button>
                            </div>
                            <a href="${cvURL}" target="_blank">
                              <button type="button" class="btn btn-outline-info">Show CV</button> 
                            </a>
                          </div>
                        </div>
                      </div>
                    </div>

                    `;
                employeeGrid.append(employeeItem);
            }
        });
    }

    $.ajax({
        url: "/Employee/GetEmployeeData",
        method: "GET",
        dataType: 'json',
        dataSrc: 'data',
    }).done(function (result) {
        allData = result.data; // Simpan semua data dalam variabel allData
        displayAllData(allData); // Tampilkan semua data

    }).fail(function (error) {
        console.log(error);
    });


    // Logika pencarian
    $("#search-input").on("input", function () {
        var searchKeyword = $(this).val().toLowerCase();
        var filteredData = allData.filter(function (val) {
            return val.statusEmployee === "idle" && (
                val.fullName.toLowerCase().includes(searchKeyword) || // Cari berdasarkan nama
                val.skill.some(skill => skill.toLowerCase().includes(searchKeyword)) // Cari berdasarkan skill
            );
        });
        displayAllData(filteredData); // Tampilkan data yang difilter
    });


    var guid
    $('#employeeGrid').on('click', '.btn-detail', function () {
        guid = $(this).data('guid');// Mengambil GUID dari tombol "Update" yang diklik
        getIdleByGuid(guid);
    });

    $('#modalDetail').on('click', '.btn-hire', function () {
        var guidHire = guid; // Mengambil GUID dari tombol "Update" yang diklik
        $('#modalInterview').modal('show');

    });

    function getIdleByGuid(guid) {
        $.ajax({
            url: "/Employee/GetGuidEmployee/" + guid,
            method: "GET",
            dataType: 'json',
            dataSrc: 'data',
        }).done((result) => {
            const baseURL = "https://localhost:7051/"; // Gantilah URL dasar sesuai dengan kebutuhan Anda
            const photoURL = `${baseURL}ProfilePictures/${result.foto}`;
            // var employeeModal = $("#employeeModal");
            var employeeValue = ` 
                <div style="max-width: 600px; margin: 20px auto; border: 1px solid #eaeaea; border-radius: 8px; overflow: hidden; box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);">
                    <div style="text-align: center; padding: 20px;">
                        <img src="${photoURL}" alt="Employee Photo" style="width: 100%; height: 300px; object-fit: cover; border-radius: 4px; box-shadow: 0 2px 8px rgba(0, 0, 0, 0.2);" />
                    </div>
                    </div>
                    <h2 style="font-size: 24px; color: #333; margin-top: 0; text-align: center; padding: 0 20px;">${result.fullName}</h2>
                    <div style="padding: 10px 20px; background-color: #f9f9f9; text-align: center;">
                        <span style="font-weight: bold; color: #666;">Rating Kinerja:</span>
                        <div style="display: inline-block; margin-left: 10px;">
                            <div style="position: relative; display: inline-block;">
                                <div style="position: absolute; top: 0; left: 0; white-space: nowrap; overflow: hidden; width: ${result.averageRating * 20}%; color: #ffc107; font-family: 'Font Awesome 5 Free'; font-weight: 900;">★★★★★</div>
                                <div style="color: #ccc; font-family: 'Font Awesome 5 Free'; font-weight: 900;">★★★★★</div>
                            </div>
                        </div>
                    </div>
                    <div style="padding: 20px; border-top: 1px solid #eaeaea;">
                        <div style="font-weight: bold; color: #333; font-size: 22px;">Grade: <span style="color: #007bff;">${result.grade}</span></div>
                        <h4 style="font-weight: bold; margin-top: 16px;">Skills:</h4>
                        <p style="color: #666; line-height: 1.6;">${result.skill.join(', ')}</p>
                        <h4 style="font-weight: bold; margin-top: 16px;">Description:</h4>
                        <p style="color: #666; line-height: 1.6;">
                            Calon partner yang akan bergabung dengan perusahaan Anda gendernya ${result.gender}. 
                            No Handphone yang dapat dihubungi adalah ${result.phoneNumber}. Kontak email yang tersedia adalah 
                            <a href="mailto:${result.email}" style="color: #007bff; text-decoration: none;">${result.email}</a>. CV dapat diakses melalui 
                            <a href="${result.cv}" target="_blank" style="color: #007bff; text-decoration: none;">lihat CV</a>.
                        </p>
                    </div>
                </div>
                `;
            $(".modalBodyDetail").html(employeeValue)
        }).fail((error) => {
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Failed to retrieve employee data.'
            });
        });
    };

    //Tombol detail
    $(document).on('click', '.btn-detail', function () {
        var guid = $(this).data('guid');
        $('#modalDetail').data('employee-guid', guid);
    });

    // Handler ketika form disubmit
    $('#formAddSchedule').submit(function (event) {
        event.preventDefault();

        var guid = $('#modalDetail').data('employee-guid');
        var cmpGuid = sessionStorage.getItem('employeeGuid'); // Mengambil company GUID dari sessionStorage

        $('.btn-save').attr('disabled', true);
        $('.btn-save').text('Create...');

        addScheduleInterview(guid, cmpGuid);

        setTimeout(function () {
            $('.btn-save').attr('disabled', false);
            $('.btn-save').text('Save');
        }, 5000);

    });

    function canSubmit(guid, cmpGuid) {
        var lastSubmitTimestamp = localStorage.getItem('lastSubmit_' + guid + '_' + cmpGuid);
        if (lastSubmitTimestamp) {
            var lastSubmitDate = new Date(parseInt(lastSubmitTimestamp));
            var currentDate = new Date();
            if (lastSubmitDate.toDateString() === currentDate.toDateString()) {
                return false; // Sudah disubmit oleh company ini hari ini
            }
        }
        return true; // Belum disubmit atau disubmit oleh company lain
    }

    function addScheduleInterview() {
        var compGuid = sessionStorage.getItem('employeeGuid');
        var nameInput = $("#nameInput").val();
        var dateInput = $("#dateInput").val();

        // Validasi input
        if (nameInput === "" || dateInput === "") {
            Swal.fire({
                text: 'Data Input Tidak Boleh Kosong',
                icon: 'info',
                showCloseButton: false,
                focusConfirm: false,
                customClass: {
                    confirmButton: 'btn btn-primary'
                },
                buttonsStyling: false
            });
            return;
        }

        var inputDate = new Date(dateInput);
        var today = new Date();
        today.setHours(0, 0, 0, 0);
        inputDate.setHours(0, 0, 0, 0);

        if (inputDate <= today) {
            Swal.fire({
                text: 'Tanggal interview tidak boleh kurang atau sama dengan dari hari ini',
                icon: 'info',
                showCloseButton: true,
                focusConfirm: false,
                confirmButtonText: '<i class="fa fa-thumbs-up"></i> Great!',
                confirmButtonAriaLabel: 'Thumbs up, great!',
            });
            return;
        }

        var guid = $('#modalDetail').data('employee-guid');

        // Cek apakah pengguna sudah submit hari ini
        if (canSubmit(guid, compGuid)) {
            var obj = {
                name: nameInput,
                date: dateInput,
                employeeGuid: guid,
                ownerGuid: compGuid
            };

            $.ajax({
                url: "/Interview/AddSchedule",
                method: "POST",
                data: JSON.stringify(obj),
                contentType: 'application/json',
                success: function (response) {
                    $('#modalInterview').modal('hide');
                    Swal.fire({
                        icon: 'success',
                        title: 'Pembaruan berhasil',
                        text: 'Data schedule berhasil ditambahkan!!.'
                    }).then(function () {
                        location.reload(); // Reload halaman setelah pengguna menekan tombol "OK"
                    });

                    // Set timestamp di localStorage
                    localStorage.setItem('lastSubmit_' + guid + '_' + compGuid, new Date().getTime());
                },
                error: function (response) {
                    $('#modalInterview').modal('hide');
                    Swal.fire({
                        icon: 'error',
                        title: 'Gagal',
                        text: 'Terjadi kesalahan saat mencoba menambahkan data schedule!!.'
                    });
                }
            });
        } else {
            Swal.fire({
                icon: 'error',
                title: 'Gagal Menambahkan Jadwal',
                text: 'Anda hanya dapat mengirimkan sekali dalam sehari per idle !!'
            });
        }
    }

});

$(document).ready(function () {

    var guid = sessionStorage.getItem('employeeGuid');

    var sesRole = sessionStorage.getItem('role');

    const baseURL = "https://localhost:7051/";
    var companyGuid;

    if (sesRole == "idle") {
        $.ajax({
            url: '/Employee/GetGuidEmployee/' + guid,
            type: 'GET',
            dataType: 'json',
            dataSrc: 'data',
            success: function (data) {

                if (data) {

                    const imageUrl1 = `${baseURL}ProfilePictures/${data.foto}`;
                    var imageUrl2 = `${baseURL}Cv/${data.cv}`;

                    employeeGuid = data.guid;
                    $('#editFirstName').val(data.firstName);
                    $("#editLastName").val(data.lastName);
                    $("#editGender").val(data.gender);
                    $("#editEmail").val(data.email);
                    $("#editPhoneNumber").val(data.phoneNumber);
                    $("#editGrade").text(data.grade);

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

                    // Jika ingin menandai beberapa skill tertentu sebagai dipilih (misalnya dari data lain)
                    if (data && data.selectedSkills) {
                        skillSelect.val(data.selectedSkills).trigger('change');
                    }
                    if (data.foto) {
                        $("#profilePictureInput").text(data.foto);
                        $("#editProfilePicPreview").attr("src", imageUrl1);
                    }
                    if (data.cv) {
                        $("#cvInput").text(data.cv);
                        $("#cvPreview").attr("href", imageUrl2); // Mengatur tautan CV
                    }

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
    } if (sesRole == "client") {
        $.ajax({
            url: '/Employee/GetGuidClient/' + guid,
            type: 'GET',
            dataType: 'json',
            dataSrc: 'data',
            success: function (response) {
                const imageUrl = `${baseURL}ProfilePictures/${response.fotoEmployee}`;
                employeeGuid = response.employeeGuid;
                companyGuid = response.companyGuid;

                $("#editFirstName").val(response.firstName);
                $("#editLastName").val(response.lastName);
                $("#editGender").val(response.gender);
                $("#editEmail").val(response.email);
                $("#editPhoneNumber").val(response.phoneNumber);
                $("#companyName").val(response.nameCompany);
                $("#addressCompany").val(response.address);
                $("#description").val(response.description);

                if (response.fotoEmployee) {
                    // Tampilkan nama file gambar sebelum diupdate
                    $("#profilePictureInput").text(response.fotoEmployee);

                    $("#editProfilePicPreview").attr("src", imageUrl);
                }

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

    $('#updateIdleForm').submit(function (event) {
        event.preventDefault();
        // Pastikan employeeGuid dan companyGuid telah di-set
        if (!employeeGuid) {
            console.error("employeeGuid belum di-set.");
            return;
        }
        if (sesRole == "idle") {

            updateEmployeeDetails(employeeGuid);
        }
        if (sesRole == "client") {

            updateClientDetails(employeeGuid, companyGuid);
        }
    });


    // Function untuk mengirim pembaruan data ke server
    function updateEmployeeDetails(employeeGuid) {
        // Ambil data dari elemen-elemen formulir
        var firstName = $('#editFirstName').val();
        var lastName = $('#editLastName').val();
        var phoneNumber = $('#editPhoneNumber').val();
        var email = $('#editEmail').val();
        var gender = ($('#editGender').val() === 'Male') ? 1 : 0;
        var skills = $('#editSkills').val();

        // Ambil file gambar dari input
        var profilePictureInput = document.getElementById('profilePictureInput');
        var profilePictureFile = profilePictureInput.files[0];

        var cvInput = document.getElementById('cvInput');
        var cvFile = cvInput.files[0];

        // Validasi gambar profil
        if (profilePictureFile && !isValidImageFormat(profilePictureFile.name)) {
            Swal.fire({
                title: 'Format Gambar Profil Salah!',
                icon: 'info',
                html: 'Hanya format JPEG, JPG, PNG, dan GIF yang diperbolehkan.',
                showCloseButton: true,
                focusConfirm: false,
                confirmButtonText: '<i class="fa fa-thumbs-up"></i> OK!',
                confirmButtonAriaLabel: 'OK!',
            });
            return; // Berhenti jika format gambar salah
        }

        // Buat objek FormData dan tambahkan data
        var dataToUpdate = new FormData();
        dataToUpdate.append('guid', employeeGuid);
        dataToUpdate.append('firstName', firstName);
        dataToUpdate.append('lastName', lastName);
        dataToUpdate.append('phoneNumber', phoneNumber);
        dataToUpdate.append('email', email);
        dataToUpdate.append('gender', gender);

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
                title: 'Format Skill Salah!',
                icon: 'info',
                html: 'Skills harus berupa string atau array',
                showCloseButton: true,
                focusConfirm: false,
                confirmButtonText: '<i class="fa fa-thumbs-up"></i> Great!',
                confirmButtonAriaLabel: 'Thumbs up, great!',
            });
            return; // Berhenti jika format skill salah
        }

        if (profilePictureFile) {
            dataToUpdate.append('ProfilePictureFile', profilePictureFile);
        }

        if (cvFile) {
            dataToUpdate.append('cvFile', cvFile);
        }

        $.ajax({
            url: '/Employee/updateIdle',
            type: 'PUT',
            data: dataToUpdate,
            contentType: false,
            processData: false, // Diperlukan untuk FormData
            success: function (response) {
                if (response.status === "OK") {
                    Swal.fire({
                        icon: 'success',
                        title: 'Pembaruan berhasil',
                        text: 'Data Idle berhasil diperbarui.'
                    }).then(function () {
                        location.reload();
                    });
                } else if (response.status === "Error") {
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
                Swal.fire({
                    icon: 'error',
                    title: 'Pembaruan gagal',
                    text: 'Terjadi kesalahan saat mencoba update data Idle.'
                });
            }
        });
    }

    // Function untuk Update Client
    function updateClientDetails(employeeGuid, companyGuid) {

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

        // Validasi gambar profil
        if (profilePictureFile && !isValidImageFormat(profilePictureFile.name)) {
            Swal.fire({
                title: 'Format Gambar Profil Salah!',
                icon: 'info',
                html: 'Hanya format JPEG, JPG, PNG, dan GIF yang diperbolehkan.',
                showCloseButton: true,
                focusConfirm: false,
                confirmButtonText: '<i class="fa fa-thumbs-up"></i> OK!',
                confirmButtonAriaLabel: 'OK!',
            });
            return; // Berhenti jika format gambar salah
        }

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
                if (response.status == "OK") {
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
                    }).then(function () {
                        location.reload();
                    });
                }
                else if (response.status === "Error") {
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
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'An error occurred while updating employee data.'
                });
            }
        });

    }

    // Fungsi untuk validasi format gambar
    function isValidImageFormat(fileName) {
        var allowedExtensions = ["jpg", "jpeg", "png", "gif"];
        var fileExtension = fileName.split('.').pop().toLowerCase();

        return allowedExtensions.includes(fileExtension);
    }

});