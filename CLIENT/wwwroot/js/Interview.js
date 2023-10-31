$(document).ready(function () {
    var guidUpdate;
    var intGuid
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
            { data: 'guid' },
            { data: 'idle' },
            { data: 'interviewer' },
            { data: 'name' },
            { data: 'date' },
            { data: 'statusIdle' },
            { data: 'type' },
            { data: 'location' },

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

        if (!intGuid || !empGuid || !ownGuid) {
            console.error("employeeGuid atau companyGuid belum di-set.");
            return;
        }

        updateInterview(intGuid, empGuid, ownGuid);
    });
    // ... (your existing code)

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

                // Set the value of location from the response
                $("#location").val(response.location);
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

    function updateInterview(intGuid, empGuid, ownGuid) {
        console.log("ini guid di update", intGuid, empGuid, ownGuid);

        var type = parseInt($("#updateType").val());
        var location = $("#location").val();
        var remarks = $("#remarks").val();

        // Check if intGuid, empGuid, and ownGuid are not null
        if (intGuid && empGuid && ownGuid) {
            var obj = {
                guid: intGuid,
                employeeGuid: empGuid,
                ownerGuid: ownGuid,
                type: type,
                location: location,
                remarks: remarks
            };

            console.log(obj);
            $.ajax({
                url: '/Interview/ScheduleUpdate',
                type: 'PUT',
                data: JSON.stringify(obj), // Set the correct content type
                contentType: 'application/json',
                success: function (response) {
                    if (response.success) {
                        $('#updateInterview').modal('hide');
                        $('#tableClient').DataTable().ajax.reload();
                        Swal.fire({
                            icon: 'success',
                            title: 'Pembaruan berhasil',
                            text: 'Data client berhasil diperbarui.'
                        });
                    } else {
                        $('#updateInterview').modal('hide');
                        Swal.fire({
                            icon: 'error',
                            title: 'Pembaruan gagal',
                            text: 'Terjadi kesalahan saat mencoba update data client: ' + response.message
                        });
                    }
                },
                error: function (response) {
                    $('#updateInterview').modal('hide');
                    Swal.fire({
                        icon: 'error',
                        title: 'Pembaruan gagal',
                        text: 'Terjadi kesalahan saat mencoba update data client.'
                    });
                }
            });
        } else {
            // Handle the case where intGuid, empGuid, or ownGuid is null
            Swal.fire({
                icon: 'error',
                title: 'Pembaruan gagal',
                text: 'intGuid, empGuid, or ownGuid is null.'
            });
        }
    }

// ... (the rest of your code)


});
