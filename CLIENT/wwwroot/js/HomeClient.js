$(document).ready(function () {
    $.ajax({
        url: "/Employee/GetEmployeeData", // Ganti dengan URL yang sesuai
        method: "GET",
        dataType: 'json',
        dataSrc: 'data',
    }).done((result) => {
        console.log("Data dari server:", result); // Tambahkan ini

        const employeeGrid = $("#employeeGrid");

        $.each(result.data, (key, val) => {
            const baseURL = "https://localhost:7051/"; // Gantilah URL dasar sesuai dengan kebutuhan Anda
            const photoURL = `${baseURL}ProfilePictures/${val.foto}`; // Gabungkan baseURL dengan path gambar
            const cvURL = `${baseURL}Cv/${val.cv}`;

            // Membuat tampilan grid untuk setiap karyawan
            const employeeItem = `
        <div class="col-lg-1-5 col-md-4 col-12 col-sm-6">
            <div class="product-cart-wrap mb-30 wow animate__animated animate__fadeIn" data-wow-delay=".1s">
                <div class="product-img-action-wrap">
                    <div class="product-img product-img-zoom">
                        <a href="shop-product-right.html">
                            <img src="${photoURL}" alt="Employee Photo" style="max-width: 500px; max-height: 500px;">
                        </a>
                    </div>
                    <div class="product-action-1">
                        <a aria-label="Add To Wishlist" class="action-btn" href="shop-wishlist.html"><i class="fi-rs-heart"></i></a>
                        <a aria-label="Compare" class "action-btn" href="shop-compare.html"><i class="fi-rs-shuffle"></i></a>
                        <a aria-label="Quick view" class="action-btn" data-bs-toggle="modal" data-bs-target="#quickViewModal"><i class="fi-rs-eye"></i></a>
                    </div>
                </div>
                <div class="product-content-wrap">
                    <div class="product-category">
                        <span>${val.statusEmployee}</span>
                    </div>
                    <h2><a href="">${val.fullName}</a></h2>
                    <div class="product-rate-cover">
                        <div class="product-rate d-inline-block">
                            <div class="product-rating" style="width: ${val.averageRating * 10}%"></div>
                        </div>
                        <span class="font-small ml-5 text-muted"> (${val.averageRating})</span>
                    </div>
                    <div>
                        <span class="font-small text-muted">${val.skill.join(', ')}</span>
                    </div>
                    <div class="product-card-bottom">
                        <div class="add-cart">
                            <button type="button" class=" btn btn-outline-success btn-detail" data-guid="${val.guid}" data-bs-toggle="modal" data-bs-target="#modalDetail" ><i class="mr-1 btn-detail" data-guid="${val.guid}"></i>Detail</button>
                        </div>
                        <a href="${cvURL}" target="_blank">
                            <button type="button" class="btn btn-outline-info" data-guid="${val.guid}"> Show CV </button> 
                        </a>
                    </div>
                </div>
            </div>
        </div>
        `;
            // Menambahkan kartu karyawan ke dalam grid
            employeeGrid.append(employeeItem);
        });
    }).fail((error) => {
        console.log(error);

    });
    var guid
    $('#employeeGrid').on('click', '.btn-detail', function () {
        guid = $(this).data('guid');// Mengambil GUID dari tombol "Update" yang diklik
        console.log('ini tombol yang atas', guid);
        getIdleByGuid(guid);
    });

    $('#modalDetail').on('click', '.btn-hire', function () {
        var guidHire = guid; // Mengambil GUID dari tombol "Update" yang diklik
        console.log('ini tombol yang hire', guidHire);
        $('#modalInterview').modal('show');

    });

    function getIdleByGuid(guid) {
        console.log("ini guid di paramter getby guid", guid);
        $.ajax({
            url: "/Employee/GetGuidEmployee/" + guid, // Ganti dengan URL yang sesuai
            method: "GET",
            dataType: 'json',
            dataSrc: 'data',
        }).done((result) => {
            console.log("Data dari server:", result);
            const baseURL = "https://localhost:7051/"; // Gantilah URL dasar sesuai dengan kebutuhan Anda
            const photoURL = `${baseURL}ProfilePictures/${result.foto}`;
            // var employeeModal = $("#employeeModal");
            var employeeValue = ` 
                <div class="product-img product-img-zoom">
                    <a href="shop-product-right.html">
                        <img class="hover-img" src="${photoURL}" alt="Employee Photo" style="max-width: 470px; max-height: 500px;"/>
                    </a>
                </div>
                <h2 class="title-detail" >${result.fullName}</h2>
                <div class="product-detail-rating">
                    <div class="product-rate-cover text-end">
                        <span class="font-small mr-4 text-muted"> Rating Kinerja </span>

                        <div class="product-rate d-inline-block">
                            <div class="product-rating" style="width: 90%">${result.averageRating}</div>
                        </div>
                    </div>
                </div>
                <div class="clearfix product-price-cover">
                    <div class="product-price primary-color float-left">
                        <h4 class="current-price text-brand title-detail" id="grade">Grade: ${result.grade}</h4>
                    </div>
                </div>
                <div class="clearfix product-price-cover">
                    <div class="product-price primary-color float-left">
                        <h4 class="current-price text-brand little-detail">Skills</h4>
                    </div>
                </div>
                <div class="short-desc mb-30">
                    <p class="font-lg skillData">${result.skill.join(', ')}</p>
                </div>
                <div class="clearfix product-price-cover">
                    <div class="product-price primary-color float-left">
                        <h4 class="current-price text-brand little-detail">Description</h4>
                    </div>
                </div>
                <div class="short-desc mb-30">
                    <p class="font-lg skillData">Calon partner yang akan bergabung dengan perusahaan anda gendernya ${result.gender}
                        No Handphone yang dapat dihubungi adalah ${result.phoneNumber} kontak email yang tersedia adalah ${result.email} 
                        cv dapat diakses melalui <a href="${result.cv}">lihat CV</a> 
                    </p>
                </div>
                `;
            $(".modalBodyDetail").html(employeeValue)
        }).fail((error) => {
            console.log(error);
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Failed to retrieve employee data.'
            });
        });
    };

    $('#formAddSchedule').submit(function (event) {
        event.preventDefault();
        addScheduleInterview();
    });

    function addScheduleInterview() {

        var nameInput = $("#nameInput").val();
        var dateInput = $("#dateInput").val();
        // Mengambil nilai string dari input
        var compGuidString = $("#companyGuid").val();

        // Membuat tipe GUID dari string
        var compGuid = compGuidString ? compGuidString.toLowerCase() : null; // Mengonversi menjadi lowercase

        // Buat objek dengan EmployeeGuid dari session
        var obj = {
            name: nameInput,
            date: dateInput,
            employeeGuid: guid,
            ownerGuid: compGuid
        }
        console.log(obj);

        $.ajax({
            url: "/Interview/AddSchedule", // Ganti dengan URL yang sesuai
            method: "POST",
            data: JSON.stringify(obj),
            contentType: 'application/json',

            success: function (response) {
                console.log(response);
                $('#modalInterview').modal('hide');
                Swal.fire({
                    icon: 'success',
                    title: 'Pembaruan berhasil',
                    text: 'Data schedule berhasil ditambahkan!!.'
                });
            },
            error: function (response) {
                $('#modalInterview').modal('hide');
                Swal.fire({
                    icon: 'error',
                    title: 'Pembaruan gagal',
                    text: 'Terjadi kesalahan saat mencoba menambahkan data schedule!!.'
                });
            }
        });

    };

});

$(document).ready(function () {

    var guid = sessionStorage.getItem('employeeGuid');
    console.log(guid);

    var sesRole = sessionStorage.getItem('role');

    console.log("Role : ", sesRole);
    const baseURL = "https://localhost:7051/";
    var companyGuid;

    if (sesRole == "idle") {
        $.ajax({
            url: '/Employee/GetGuidEmployee/' + guid,
            type: 'GET',
            dataType: 'json',
            dataSrc: 'data',
            success: function (data) {
                console.log(data);
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

                    // Jika Anda ingin menandai beberapa skill tertentu sebagai dipilih (misalnya dari data lain), Anda dapat menggunakan bagian kode ini
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
    }if (sesRole == "client") {
        $.ajax({
            url: '/Employee/GetGuidClient/' + guid,
            type: 'GET',
            dataType: 'json',
            dataSrc: 'data',
            success: function (response) {
                const imageUrl = `${baseURL}ProfilePictures/${response.fotoEmployee}`;
                employeeGuid = response.guid;
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
        } if (sesRole == "idle") {

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


