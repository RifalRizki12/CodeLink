$(document).ready(function () {
    $('#tableInterview').DataTable({
        ajax: {
            url: '/Interview/InterviewData',
            type: 'GET',
            dataType: 'json',
            dataSrc: 'data',
        },
        columns: [
            {
                data: null,
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            { data: 'idle' },
            { data: 'interviewer' },
            { data: 'name' },
            { data: 'date' },
            {
                data: null,
                render: function (data, type, row, meta) {

                    return `<button type="button" class="btn btn-primary btn-update" data-guid="${data.guid}" data-bs-toggle="modal" data-bs-target="#modalUpdateClient">Shedule</button> `;
                }
            },
        ]
    });

});