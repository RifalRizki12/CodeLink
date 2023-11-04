$(document).ready(function () {
    $('#registerForm').on('submit', function (e) {
        e.preventDefault();

       

        var profilePictureFile = $('#profilePictureInput').prop('files')[0];
        if (!profilePictureFile) {
            Swal.fire({
                title: 'Gambar Profil Harus Dipilih',
                icon: 'info',
                showCloseButton: true,
                focusConfirm: false,
                confirmButtonText: '<i class="fa fa-thumbs-up"></i> Great!',
                confirmButtonAriaLabel: 'Thumbs up, great!',
            });
            return;
        }

        if ($('#firstNameInput').val() === "" || parseInt($('#genderInput').val()) === null ||
            $('#emailInput').val() === "" || $('#phoneNumberInput').val() === "" || $('#nameCompanyInput').val() === ""||
            $('#addressCompanyInput').val() === "" || $('#passwordInput').val() === "" || $('#confirmPasswordInput').val() === "") {
            Swal.fire({
                title: 'Data Inputan Tidak Boleh Kosong',
                icon: 'info',
                showCloseButton: true,
                focusConfirm: false,
                confirmButtonText: '<i class="fa fa-thumbs-up"></i> Great!',
                confirmButtonAriaLabel: 'Thumbs up, great!',
            })
            return;
        }

        var formData = new FormData();

        // Tambahkan data teks ke formData
        formData.append('firstName', $('#firstNameInput').val());
        formData.append('lastName', $('#lastNameInput').val()); // Optional, bisa kosong
        formData.append('gender', parseInt($('#genderInput').val())); // Pastikan ini sesuai dengan enum GenderLevel pada server
        formData.append('email', $('#emailInput').val());
        formData.append('phoneNumber', $('#phoneNumberInput').val());
        formData.append('nameCompany', $('#nameCompanyInput').val());
        formData.append('addressCompany', $('#addressCompanyInput').val());
        formData.append('description', $('#descriptionInput').val());
        formData.append('password', $('#passwordInput').val());
        formData.append('confirmPassword', $('#confirmPasswordInput').val());
        formData.append('profilePictureFile', profilePictureFile);

        // Kirim data dengan metode AJAX ke server
        $.ajax({
            url: '/Employee/RegisterClient',
            type: 'POST',
            data: formData,
            processData: false,  // penting, jangan proses data
            contentType: false,  // penting, biarkan jQuery mengatur ini
            success: function (response) {
                Swal.fire({
                    icon: 'success',
                    title: 'Register Berhasil!',
                    text: 'Anda akan diarahkan ke halaman yang dituju.',
                }).then(function () {
                    // Ganti dengan URL yang diinginkan
                    window.location.href = '/path/to/redirect';
                });
            },
            error: function (xhr, status, error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Terjadi Kesalahan!',
                    text: 'Koneksi gagal atau server mengalami masalah. Silakan coba lagi nanti.',
                });
                console.error('Error:', error);
            }
        });
    });
});
