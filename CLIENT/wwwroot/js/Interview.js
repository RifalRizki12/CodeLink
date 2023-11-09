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
            dataSrc: function (data) {
                // Filter data berdasarkan statusInterview yang tidak sama dengan 2 (Contract Terminated)
                return data.data.filter(function (row) {
                    return row.statusInterview === null;
                });
            }
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
                                <span class="badge bg-glow bg-secondary">Proses</span>
                            </div>`;
                    }
                }
            },

            {
                data: null,
                render: function (data, type, row, meta) {

                    return `<button type="button" class="btn btn-primary btn-schedule" data-guid="${data.guid}" data-bs-toggle="modal" data-bs-target="#updateInterview">Schedule</button>
                    <button type="button" class="btn btn-danger btn-delete" data-guid="${data.guid}">Delete</button>
                    `;
                }
            },
        ]
    });

    $('#historyInterview').DataTable({
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
            {
                data: null,
                render: function (data, type, row) {
                    if (type === 'display') {
                        var startContract = row.startContract;
                        var endContract = row.endContract;

                        if (startContract && endContract) {
                            // Jika kedua tanggal tersedia, tampilkan rentang tanggal
                            var startDate = new Date(startContract).toLocaleDateString();
                            var endDate = new Date(endContract).toLocaleDateString();
                            return startDate + ' - ' + endDate;
                        } else if (startContract) {
                            // Jika hanya tanggal "Start Contract" yang tersedia
                            return new Date(startContract).toLocaleDateString() + ' - No End Date';
                        } else if (endContract) {
                            // Jika hanya tanggal "End Contract" yang tersedia
                            return 'No Start Date - ' + new Date(endContract).toLocaleDateString();
                        } else {
                            // Jika kedua tanggal tidak tersedia
                            return '<div class="text-center /"><span class="badge bg-secondary bg-glow" />No Date';
                        }
                    }
                    return data; // Mengembalikan data asli untuk jenis lainnya
                }
            },
            {
                data: 'statusInterview',
                render: function (data, type, row) {
                    if (row.statusInterview === null) {
                        return `
                            <div class="text-center">
                                <span class="badge bg-glow bg-secondary">Proses</span>
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
                data: 'rate',
                render: function (data) {
                    if (data !== null && data !== undefined) {
                        var stars = '';
                        for (var i = 0; i < data; i++) {
                            stars += '<i class="fa-solid fa-star" style="color: #ffea00;"></i>';
                        }
                        return stars;
                    } else {
                        return '<span class="badge bg-glow bg-secondary /">No Rating';
                    }
                }
            },
            {
                data: 'feedback',
                render: function (data) {
                    if (data !== null && data !== undefined) {
                        if (data.length > 5) {
                            return '<a class="read-more"><span class="badge bg-glow bg-primary /">Read More</a><div class="feedback-content" style="display: none;">' + data + '</div>';
                        } else {
                            return '<div class="feedback-content">' + data + '</div>';
                        }
                    } else {
                        return '<span class="badge bg-glow bg-secondary /">No Feedback';
                    }
                }
            }
        ]
    });

    // Menambahkan event listener untuk menangani klik "Read More"
    $(document).on('click', '.read-more', function () {
        var feedbackContent = $(this).next('.feedback-content');
        feedbackContent.slideToggle();
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

    //Delete
/*    $('#tableInterview').on('click', '.btn-delete', function () {
        guidDelete = $(this).data('guid');
        console.log(guidUpdate);
        deleteInterview(guidUpdate);
    });*/

    $(document).on("click", ".btn-delete", function () {
        var guidDelete = $(this).data("guid");
        $('#modalCenter').modal('hide');
        // Tampilkan SweetAlert konfirmasi
        Swal.fire({
            title: 'Konfirmasi',
            text: 'Anda yakin ingin menghapus data ini?',
            icon: 'question',
            showCloseButton: true,
            focusConfirm: true,
            customClass: {
                confirmButton: 'btn btn-danger'
            },
            buttonsStyling: false,
        }).then((result) => {
            if (result.isConfirmed) {
                // Jika pengguna mengonfirmasi penghapusan
                $.ajax({
                    url: "/Interview/deleteInterview/" + guidDelete,
                    type: "DELETE",
                    success: function (data) {
                        // Refresh tabel data employee setelah penghapusan
                        $('#tableInterview').DataTable().ajax.reload();
                        Swal.fire({
                            icon: 'success',
                            title: 'Success',
                            text: 'Data berhasil dihapus!',
                            showCloseButton: true,
                            focusConfirm: false,
                            customClass: {
                                confirmButton: 'btn btn-primary'
                            },
                            buttonsStyling: false,
                        });
                    },
                    error: function (xhr) {
                        var errorMessage = xhr.responseJSON ? xhr.responseJSON.message : 'Gagal menghapus data.';
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: errorMessage,
                            showCloseButton: true,
                            focusConfirm: true,
                            customClass: {
                                confirmButton: 'btn btn-danger'
                            },
                            buttonsStyling: false,
                        });
                    }
                });
            }
        });
    });

});
