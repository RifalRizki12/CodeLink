$(document).ready(function () {
     // Ganti #someElement dengan selector yang sesuai
    var guid = $('#empGuid').val();

    var compGuid = guid ? guid.toLowerCase() : null;

    if (typeof guid === 'undefined' || guid === null) {
        console.error('guid is not defined or null');
        return;
    }

    console.log(compGuid);

    $('#tableListHire').DataTable({
        ajax: {
            url: '/Interview/ListHireIdle/' + compGuid,
            type: 'GET',
            dataType: 'json',
            dataSrc: 'data',
        },
        columns: [
            {
                data: 'employeGuid',
            },
            { data: 'date' },
            { data: 'idle' },
            { data: 'foto' },
            {
                data: null,
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
          
        ]

    }); 
});
