$(document).ready(function () {
    $('.table - wishlist').DataTable({
        ajax: {
            url: '/Interview/ListHireIdle/' + guid,
            type: 'GET',
            dataType: 'json',
            dataSrc: 'data',
        },
        columns: [

            {
                data: 'foto',
                render: function (data, type, row) {
                    if (type === 'display' && data) {
                        const baseURL = "https://localhost:7051/"; // Gantilah URL dasar sesuai dengan kebutuhan Anda
                        const photoURL = `${baseURL}ProfilePictures/${data}`; // Gabungkan baseURL dengan path gambar
                        return `<img src="${photoURL}" alt="Employee Photo" style="max-width: 100px; max-height: 100px;">`;
                    }
                    return 'N/A'; // Pesan jika URL gambar tidak tersedia
                }
            },
            { data: 'idle' },
            { data: 'date' },          
            {
                data: null,
                render: function (data, type, row, meta) {

                    return `<button type="button" class="btn btn-primary btn-schedule" data-guid="${data.guid}" data-bs-toggle="modal" data-bs-target="#updateInterview">Schedule</button> `;
                }
            },
            {
                data: null,
            },
        ]
    });
});
