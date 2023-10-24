$(document).ready(function () {
    $.ajax({
        url: '/Employee/GetEmployeeData', // Ganti dengan URL API yang sesuai
        type: 'GET',
        dataType: 'json',
        success: function (data) {
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
                    {
                        data: null,
                        render: function (data, type, row) {
                            return row.firstName + ' ' + row.lastName;
                        }
                    },
                    { data: 'gender' },
                    { data: 'email' },
                    { data: 'phoneNumber' },
                    { data: 'statusEmployee' },
                    { data: 'hireDate' },
                    { data: 'expiredDate' },
                    { data: 'nameCompany' },
                    { data: 'employeeOwner' },
                    { data: 'address' },
                    { data: 'experience' },
                    { data: 'position' },
                    { data: 'companyExperience' },
                    { data: 'hardSkill' },
                    { data: 'softSkill' },
                    { data: 'averageRating' },
                    {
                        data: null,
                        render: function (data, type, row) {
                            return `<button type="button" class="btn btn-warning edit-button" data-guid="${data.guid}">Edit</button>
                            <button type="button" class="btn btn-danger delete-button" data-guid="${data.guid}">Delete</button>`;
                        }
                    }
                ]
            });
        },
        error: function () {
            // Handle kesalahan jika ada
        }
    });
});