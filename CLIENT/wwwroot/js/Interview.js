$(document).ready(function () {
    var guidUpdate;
    var intGuid;
    var empGuid;
    var ownGuid;

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
                        return ""; // Jika type adalah null, kembalikan teks kosong
                    } else {
                        return row.type == "0" ? "Online" : "Offline";
                    }
                }
            },
            { data: 'location' },
            {
                data: 'statusInterview',
                render: function (data, type, row) {
                    if (row.statusInterview === null) {
                        return ""; // Jika type adalah null, kembalikan teks kosong
                    } else if (row.statusInterview === 0) {
                        return "Lolos";
                    } else if (row.statusInterview === 1) {
                        return "Tidak Lolos";
                    } else if (row.statusInterview === 2) {
                        return "Contract Terminated";
                    } else
                    {
                        return "Contract Finish";
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

    $('#tableInterview').on('click', '.btn-schedule', function () {
        guidUpdate = $(this).data('guid');
        console.log(guidUpdate);
        getInterviewByGuid(guidUpdate);
    });

    $('#scheduleUpdateForm').submit(function (event) {
        event.preventDefault();
        updateInterview(intGuid);
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
