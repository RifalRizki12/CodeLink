$(document).ready(function () {
     // Ganti #someElement dengan selector yang sesuai



    var guid = $('#employeeGuid').data('guid');

    if (typeof guid === 'undefined' || guid === null) {
        console.error('guid is not defined or null');
        return;
    }

    console.log(guid);

    $('#tableListHire').DataTable({
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
                        const baseURL = "https://localhost:7051/";
                        const photoURL = `${baseURL}ProfilePictures/${data}`;
                        return `<img src="${photoURL}" alt="Employee Photo" style="max-width: 100px; max-height: 100px;">`;
                    }
                    return 'N/A';
                }
            },
            { data: 'idle' },
            { data: 'date' },
            {
                data: null,
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            {
                data: 'employeGuid',
            },
        ]
    });
});
