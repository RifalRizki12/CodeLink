$(document).ready(function () {
    var guidUpdate;
    var intGuid;
    var empGuid;
    var ownGuid;

    $('#tableInterview').DataTable({
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'copy',
                text: 'Copy',
                className: 'btn btn-dark btn-sm',
                exportOptions: {
                    columns: ':visible:not(.no-export)'
                }
            },
            {
                extend: 'excel',
                text: 'Export to Excel',
                className: 'btn btn-success btn-sm',
                exportOptions: {
                    columns: ':visible:not(.no-export)'
                },
            },
            {
                extend: 'pdf',
                text: 'Export to PDF',
                className: 'btn btn-danger btn-sm',
                exportOptions: {
                    columns: ':visible:not(.no-export)'
                },
                customize: function (doc) {
                    doc.pageOrientation = 'landscape';
                    doc.pageSize = 'A3';
                }
            },
            {
                extend: 'print',
                text: 'Print',
                className: 'btn btn-info btn-sm',
                exportOptions: {
                    columns: ':visible:not(.no-export)'
                },
                customize: function (win) {
                    $(win.document.body).css('font-size', '12px');
                    $(win.document.body).find('table').addClass('compact').css('font-size', 'inherit');
                }
            },
            {
                extend: 'colvis',
                className: 'btn btn-primary btn-sm',
                postfixButtons: ['colvisRestore']
            }
        ],
        scrollX: true,
        columnDefs: [
            {
                visible: false
            }
        ],

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
            {
                data: 'date',
                render: function (data, type, row) {
                    if (type === 'display') {
                        var date = new Date(data);
                        var formattedDate = date.getFullYear() + '-' +
                            String(date.getMonth() + 1).padStart(2, '0') + '-' +
                            String(date.getDate()).padStart(2, '0') + ' ' +
                            String(date.getHours()).padStart(2, '0') + ':' +
                            String(date.getMinutes()).padStart(2, '0');
                        return formattedDate;
                    }
                    return data; // Mengembalikan data asli untuk jenis lainnya
                }
            },
            { data: 'statusIdle' },
            {
                data: "type",
                render: function (data, type, row) {
                    if (row.type === null) {
                        return '<div class="text-center /"><span class="badge bg-secondary bg-glow">N/A</span>'; // Jika type adalah null, kembalikan teks kosong
                    } else {
                        return row.type == "0" ? '<div class="text-center /"><span class="badge bg-success bg-glow">Online</span>' : '<div class="text-center /"><span class="badge bg-warning bg-glow">Offline</span>';
                    }
                }
            },
            { data: 'location' },
            {
                data: 'statusInterview',
                render: function (data, type, row) {
                    if (row.statusInterview === null) {
                        return `
                            <div class="text-center">
                                <span class="badge bg-glow bg-secondary">N/A</span>
                            </div>`;
                    } else if (row.statusInterview === 0) {
                        return `
                            <div class="text-center">
                                <span class="badge bg-glow bg-success">Lolos</span>
                            </div>`;
                    } else if (row.statusInterview === 1) {
                        return `
                            <div class="text-center">
                                <span class="badge bg-glow bg-danger">Tidak Lolos</span>
                            </div>`;
                    } else if (row.statusInterview === 2) {
                        return `
                            <div class="text-center">
                                <span class="badge bg-glow bg-danger">Contract Terminated</span>
                            </div>`;
                    } else {
                        return `
                            <div class="text-center">
                                <span class="badge bg-glow bg-success">Contract Finish</span>
                            </div>`;
                    }
                }
            },

            {
                data: null,
                render: function (data, type, row, meta) {

                    return `<button type="button" class="btn btn-primary btn-schedule" data-guid="${data.guid}" data-bs-toggle="modal" data-bs-target="#updateInterview">Schedule</button> `;
                }
            },
        ]
    });

    $('.dt-buttons').removeClass('dt-buttons');

    $('#tableInterview').on('click', '.btn-schedule', function () {
        guidUpdate = $(this).data('guid');
        console.log(guidUpdate);
        getInterviewByGuid(guidUpdate);
    });

    $('.spinner-border').hide();
    $('.btn-save').show();
    $('#scheduleUpdateForm').submit(function (event) {
        event.preventDefault();

        // Sembunyikan tombol "Save" dan tampilkan spinner
        $('.btn-save').attr('disabled', true);
        $('.btn-save').text('Saving...');

        $('.spinner-border').show();
        updateInterview(intGuid);
        setTimeout(function () {
            $('.btn-save').attr('disabled', false);
            $('.btn-save').text('Save');
            $('.spinner-border').hide();
        }, 5000);
    });

    function getInterviewByGuid(guid) {
        console.log(guid);
        $.ajax({
            url: '/Interview/InterviewById/' + guid,
            type: 'GET',
            dataType: 'json',
            dataSrc: 'data',
            success: function (response) {
                console.log(response);
                empGuid = response.employeeGuid;
                ownGuid = response.ownerGuid;
                intGuid = response.guid;

                $("#location").val(response.location);

                $("#remarks").val(response.remarks);
            },
            error: function (response) {
                Swal.fire({
                    icon: 'error',
                    title: 'Kesalahan',
                    text: 'Terjadi kesalahan saat mencoba mendapatkan data klien.'
                });
            }
        });
    }
    function updateInterview(intGuid) {
        var typeInpt = parseInt($("#updateType").val());
        var locInput = $("#location").val();

        if (typeInpt === "" || locInput === "") {
            $('.spinner-border').hide();
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
        } if (typeInpt === 0) {
            // Gunakan ekspresi reguler untuk memeriksa apakah locInput adalah tautan
            var linkPattern = /^(https?:\/\/[^\s]+)/;
            if (!linkPattern.test(locInput)) {
                $('.spinner-border').hide();
                Swal.fire({
                    text: 'Location harus berupa tautan (misal: https://zoom.com)',
                    icon: 'info',
                    showCloseButton: false,
                    focusConfirm: false,
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
                return;
            }
        };

        var obj = {
            guid: intGuid,
            employeeGuid: empGuid,
            ownerGuid: ownGuid,
            type: typeInpt,
            location: locInput,
            remarks: $("#remarks").val()
        }

        $.ajax({
            url: '/Interview/ScheduleUpdate/' + intGuid,
            type: 'PUT',
            data: JSON.stringify(obj),
            contentType: 'application/json',
            success: function (response) {
                $('.spinner-border').hide();
                $('#updateInterview').modal('hide');
                $('#tableInterview').DataTable().ajax.reload();
                Swal.fire({
                    icon: 'success',
                    title: 'Success!',
                    text: 'Detail Schedule Berhasil Ditambahkan!.',
                    showCloseButton: false,
                    focusConfirm: false,
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false
                });
            },
            error: function (response) {
                $('.spinner-border').hide();
                $('#updateInterview').modal('hide');
                Swal.fire({
                    icon: 'error',
                    title: 'Pembaruan gagal',
                    text: 'Terjadi kesalahan saat mencoba update data client.',
                    showCloseButton: false,
                    focusConfirm: false,
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false

                });
            }
        })

    }

});
