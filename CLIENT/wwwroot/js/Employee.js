// employee.js
$(document).ready(function () {
    // Menggunakan Ajax untuk mengambil data karyawan
    $.ajax({
        url: '/Employee/List', // Ganti dengan URL API yang sesuai
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            // Menggunakan data yang diterima untuk menginisialisasi tabel DataTables
            var table = $('#tableEmployee').DataTable({
                data: data, // Menggunakan data karyawan yang telah diterima
                columns: [
                    { data: 'FirstName'+'LastName'},
                    { data: 'Gender' },
                    { data: 'Email' },
                    { data: 'PhoneNumber' },
                    { data: 'HiringDate' },
                    // Definisikan kolom data lainnya sesuai kebutuhan Anda
                ]
            });
        },
        error: function () {
            // Handle kesalahan jika ada
        }
    });
});
