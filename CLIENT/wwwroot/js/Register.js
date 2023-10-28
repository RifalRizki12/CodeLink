$(document).ready(function () {
    $('#registerForm').on('submit', function (e) {
        e.preventDefault();

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
});
