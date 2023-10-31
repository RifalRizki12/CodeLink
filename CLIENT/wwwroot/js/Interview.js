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
            { data: 'date' },
            { data: 'statusIdle' },
            {
                data: "type",
                render: function (data, type, row) {
                    return row.type == "0" ? "Online" : "Offline";
                }
            },
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

                // Set the value of location from the response
                $("#location").val(response.location);
                $("#type").val(response.type);
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
        console.log("ini guid di update", intGuid);

        var updateType = parseInt($("#updateType").val());
        var locUpdate = $("#location").val();
        var remarksUpate = $("#remarks").val();

        if (intGuid && empGuid && ownGuid) {
            var obj = {
                guid: intGuid,
                employeeGuid: empGuid,
                ownerGuid: ownGuid,
                type: updateType,
                location: locUpdate,
                remarks: remarksUpate
            };

            console.log(obj);
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
                            title: 'Pembaruan berhasil',
                            text: 'Data client berhasil diperbarui.'
                        });
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
            Swal.fire({
                icon: 'error',
                title: 'Pembaruan gagal',
                text: 'intGuid, empGuid, or ownGuid is null.'
            });
        }
    }
});
