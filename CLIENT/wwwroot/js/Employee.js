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
            { data: 'fullName' },
            { data: 'gender' },
            { data: 'email' },
            { data: 'phoneNumber' },
            { data: 'statusEmployee' },
            { data: 'averageRating' },
        ]
    });
});


/*$(document).ready(function () {
    $.ajax({
        url: '/Employee/GetEmployeeData', // Ganti dengan URL API yang sesuai
        type: 'GET',
        dataType: 'json',
         dataSrc: 'data'
        success: function (data) {
            console.log(data);
            // Menggunakan data yang diterima untuk menginisialisasi tabel DataTables
            var table = $('#tableEmployee').DataTable({
                data: data, // Menggunakan data yang telah digabungkan
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
                    { data: 'phoneNumber' },
                    { data: 'statusEmployee' },
                   
                    { data: 'averageRating' },
                    
                ]
            });
        },
        error: function (xhr, status, error) {
            console.log("Terjadi kesalahan: " + status + " - " + error);
        }

    });
});*/


    var experiences = [];

    // Memantau klik tombol "Add Experience"
    $('#addExperienceButton').click(function () {
        var experienceRow = `
            <div class="row">
                <div class="col mb-3">
                    <label for="nameInput" class="form-label">Experience Name</label>
                    <input type="text" class="form-control name-input" placeholder="Experience Name" required />
                </div>
                <div class="col mb-3">
                    <label for="positionInput" class="form-label">Position</label>
                    <input type="text" class="form-control position-input" placeholder="Position" required />
                </div>
                <div class="col mb-3">
                    <label for="companyInput" class="form-label">Company</label>
                    <input type="text" class="form-control company-input" placeholder="Company" required />
                </div>
            </div>`;

        $('#experiencesContainer').append(experienceRow);
    });

    // Ketika tombol "Create Employee" ditekan
    $('#createEmployeeButton').click(function () {
        var skills = $('#skillsInput').val().split(',').map(skill => skill.trim());

        // Mengumpulkan data pengalaman dari input
        var experiences = [];
        $('#experiencesContainer .row').each(function () {
            var name = $(this).find('.name-input').val();
            var position = $(this).find('.position-input').val();
            var company = $(this).find('.company-input').val();

            experiences.push({
                name: name,
                position: position,
                company: company
            });
        });

        // Membuat objek data sesuai dengan Swagger
        var employeeData = {
            firstName: $('#firstNameInput').val(),
            lastName: $('#lastNameInput').val(),
            gender: $('#genderInput').val(),
            var profilePictureFile = $('#profilePictureInput').prop('files')[0],
            var cvFile = $('#cvInput').prop('files')[0],
            phoneNumber: $('#phoneNumberInput').val(),
            hireMetro: $('#hireMetroInput').val(),
            endMetro: $('#endMetroInput').val(),
            email: $('#emailInput').val(),
            grade: $('#gradeInput').val(),
            statusEmployee: $('#statusEmployeeInput').val(),
            password: $('#passwordInput').val(),
            confirmPassword: $('#confirmPasswordInput').val(),
            skills: skills,
            experiences: experiences
        };

        // Kirim data dengan metode AJAX ke server
        $.ajax({
            url: '/Employee/RegisterIdle',
            type: 'POST',
            data: JSON.stringify(employeeData),
            contentType: 'application/json',
            success: function (response) {
                console.log(response);
                // Handle respons sukses atau pesan kesalahan
                if (response.success) {
                    $('#modalCenter').modal('hide');
                    // Lakukan tindakan lain, seperti membersihkan formulir, menampilkan pesan sukses, dll.
                } else {
                    $('#errorMessage').text(response.message);
                }
            },
            error: function () {
                alert('Terjadi kesalahan koneksi.');
            }
        });
    });
});