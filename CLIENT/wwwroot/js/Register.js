$(document).ready(function () {
    $('#registerForm').on('submit', function (e) {
        e.preventDefault();

        var profilePictureFile = $('#profilePictureInput').prop('files')[0];
        
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

                if (response.status == "OK") {
                    Swal.fire({
                        icon: 'success',
                        title: 'Register Berhasil!',
                        text: 'Anda akan diarahkan ke halaman yang dituju !',
                        showCloseButton: false,
                        focusConfirm: false,
                        customClass: {
                            confirmButton: 'btn btn-primary'
                        },
                        buttonsStyling: false,
                    }).then(function () {
                        // Ganti dengan URL yang diinginkan
                        window.location.href = '/Account/Logins';
                    });
                }
                else if (response.status === "Error") {
                    Swal.fire({
                        icon: 'error',
                        title: 'Gagal Register !',
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
                    title: 'Terjadi Kesalahan!',
                    text: 'Koneksi gagal atau server mengalami masalah. Silakan coba lagi nanti.',
                });
                console.error('Error:', error);
            }
        });
    });
});
