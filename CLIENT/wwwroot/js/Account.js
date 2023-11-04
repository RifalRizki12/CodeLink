//Login
$(document).ready(function () {
    $('#loginButton').click(function () {
        var email = $('#emailInput').val();
        var password = $('#passwordInput').val();

        var data = {
            Email: email,
            Password: password
        };

        $.ajax({
            url: '/Account/Logins', // Ganti dengan URL API yang sesuai
            type: 'POST',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: 'application/json',
            success: function (response) {
                if (response.redirectTo) {
                    // SweetAlert untuk login berhasil
                    Swal.fire({
                        icon: 'success',
                        title: 'Login Berhasil!',
                        text: 'Anda akan diarahkan ke halaman yang dituju.',
                        showCloseButton: false,
                        focusConfirm: false,
                        customClass: {
                            confirmButton: 'btn btn-primary'
                        },
                        buttonsStyling: false
                    }).then(function () {
                        window.location.href = response.redirectTo;
                    });

                }
            },
            error: function (xhr, status, error) {
                if (xhr.status === 500) {
                    // SweetAlert untuk kesalahan server
                    Swal.fire({
                        icon: 'error',
                        title: 'Login Gagal!',
                        text: 'Pastikan email dan password Anda benar.',
                        showCloseButton: false,
                        focusConfirm: false,
                        customClass: {
                            confirmButton: 'btn btn-primary'
                        },
                        buttonsStyling: false
                    });

                }
            }
        });
    });
});

//Forgot Password
$(document).ready(function () {
    $('#forgotPsswd').click(function () {
        var email = $('#emailInput').val();

        if (email === "" ) {
            Swal.fire({
                text: 'Email Tidak Boleh Kosong',
                icon: 'info',
                showCloseButton: false,
                focusConfirm: false,
                customClass: {
                    confirmButton: 'btn btn-primary'
                },
                buttonsStyling: false
            })
            return;
        }

    
        var data = {
            Email: email,
        };

        $.ajax({
            url: '/Account/ForgotPassword/' + email, 
            type: 'PUT',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: 'application/json',
            success: function (response) {
                Swal.fire({
                    icon: 'success',
                    title: 'Forgot Password Berhasil!',
                    text: 'Silahkan Cek Email Anda!!!.',
                }).then(function () {
                    window.location.href = response.redirectTo;
                });
            },
            error: function (xhr, status, error) {
                if (xhr.status === 500) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Forgot Password Gagal!',
                        text: 'Pastikan email  Anda benar.',
                    });

                }
            }
        });
    });
});

//CHANGE PASSWORD
$(document).ready(function () {
    $('#changePasswd').click(function () {
        var email = $('#emailInput').val();
        var otp = $('#otpInput').val();
        var password = $('#passwordInput').val();
        var confirmPassword = $('#confirmPsswdInput').val();

        if (email == " " || otp === "" || password === "" || confirmPassword === "") {
            Swal.fire({
                text: 'Data Input Tidak Boleh Kosong',
                icon: 'info',
                showCloseButton: false,
                focusConfirm: false,
                customClass: {
                    confirmButton: 'btn btn-primary'
                },
                buttonsStyling: false
            })
            return;
        }

        var data = {
            Email: email,
            Otp: otp,
            NewPassword: password,
            ConfirmPassword: confirmPassword
        };

        $.ajax({
            url: '/Account/PasswordChange/' + email,
            type: 'PUT',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: 'application/json',
            success: function (response) {
                Swal.fire({
                    icon: 'success',
                    title: 'Change Password Berhasil!',
                    text: 'Silahkan Login Kembali!!!.',
                }).then(function () {
                    window.location.href = response.redirectTo;
                });
            },
            error: function (xhr, status, error) {
                if (xhr.status === 500) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Change Password Gagal!',
                        text: 'Pastikan email  Anda benar.',
                    });

                }
            }
        });
    });
});