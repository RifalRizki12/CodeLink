$(document).ready(function () {
    $('#loginButton').click(function () {
        var email = $('#emailInput').val();
        var password = $('#passwordInput').val();

        var data = {
            Email: email,
            Password: password
        };

        $.ajax({
            url: '/Account/Login', // Ganti dengan URL API yang sesuai
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
                    });
                
                }
            }
        });
    });
});
