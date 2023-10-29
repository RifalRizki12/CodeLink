$(document).ready(function () {
    $.ajax({
        url: '/Employee/GetChartData',
        type: 'GET',
        success: function (response) {
            if (response.data) {
                const data = response.data;
                updateCards(data);
            } else {
                console.error('Data tidak ditemukan');
            }
        },
        error: function (xhr, status, error) {
            console.error('Terjadi kesalahan saat mengambil data:', error);
        }
    });
});

function updateCards(data) {
    // Contoh: mengupdate kotak pertama dengan jumlah "Hired Employees"
    $(".card-border-shadow-primary h4").text(data.idleEmployeesCount);

    // Mengupdate kotak kedua dengan jumlah "Idle Employees"
    $(".card-border-shadow-warning h4").text(data.hiredEmployeesCount);
    $(".card-border-shadow-danger h4").text(data.companiesCount);

    
    // $(".card-border-shadow-danger h4").text(data.someOtherDataCount);
    // $(".card-border-shadow-info h4").text(data.anotherDataCount);
}
