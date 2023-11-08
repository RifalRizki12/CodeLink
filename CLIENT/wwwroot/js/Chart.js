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
    var tanggalHariIni = new Date();
    var options = { year: 'numeric', month: '2-digit', day: '2-digit' };
    var tanggalFormatted = tanggalHariIni.toLocaleDateString('id-ID', options); // Format tanggal sesuai preferensi Anda
    $(".d-flex1 h6").text(tanggalFormatted);

    // Menambahkan jam hari ini
    var jamHariIni = tanggalHariIni.toLocaleTimeString('id-ID');
    $(".d-flex2 h6").text(jamHariIni);
    // Contoh: mengupdate kotak pertama dengan jumlah "Hired Employees"
    $(".card-border-shadow-primary h4").text(data.idleEmployeesCount);

    // Mengupdate kotak kedua dengan jumlah "Idle Employees"
    $(".card-border-shadow-warning h4").text(data.hiredEmployeesCount);
    $(".card-border-shadow-danger h4").text(data.companiesCount);
    $(".card-border-shadow-info h4").text(data.adminEmployeesCount);
    $(".user-progress1 h6").text(data.requestedAccountCount);
    $(".user-progress2 h6").text(data.approvedAccountCount);
    $(".user-progress3 h6").text(data.rejectedAccountCount);
    $(".user-progress4 h6").text(data.canceledAccountCount);
    $(".user-progress5 h6").text(data.nonAktifAccountCount);
    $(".user-progress11 h6").text(data.tidakLolosInterviewCount);
    $(".user-progress12 h6").text(data.lolosInterviewCount);
    $(".user-progress13 h6").text(data.contarctTerminationInterviewCount);
    $(".user-progress14 h6").text(data.endContractInterviewCount);



    
    // $(".card-border-shadow-danger h4").text(data.someOtherDataCount);
    // $(".card-border-shadow-info h4").text(data.anotherDataCount);
}

