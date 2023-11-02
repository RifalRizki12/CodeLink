$(document).ready(function () {
    var guidCompny = document.getElementById("employeeGuid").getAttribute("data-guid");
    console.log(guidCompny);

    // Fungsi untuk membangun tampilan setiap karyawan
    function buildEmployeeItem(item) {
        const baseURL = "https://localhost:7051/";
        const photoURL = `${baseURL}ProfilePictures/${item.foto}`;

        return `
            <tr class="pt-30" style="text-align:center;">
               <td class="custome-checkbox pl-30">
                   <input class="form-check-input" type="checkbox" name="checkbox" id="exampleCheckbox1" value="">
                   <label class="form-check-label" for="exampleCheckbox1"></label>
               </td>
               <td class="image product-thumbnail pt-40">
                    <img src="${photoURL}" alt="${item.idle}">
                    <h6><a class="product-name mb-10" href="">${item.idle}</a></h6>
               
               </td>
               <td class="product-des product-name">
               </td>
               <td class="product-des product-name" data-title="date">
                   <h6 class="text-brand">${item.date}</h6>
               </td>
               <td class="text-right" data-title="Lolos">
                   <button class="btn btn-sm btn-lolos" data-guid1="${item.employeGuid}" data-guid2="${item.idle}" data-bs-toggle="modal" data-bs-target="#lolosInterview">Lolos</button>
               </td>
               <td class="text-right" data-title="tidakLolos">
                   <button class="btn-danger btn-sm btn-ditolak" data-guid="${item.employeGuid}">Tidak Lolos</button>
               </td>
            </tr>
        `;
    }

    // Mengambil data dari server
    $.ajax({
        url: '/Interview/ListHireIdle/' + guidCompny,
        type: 'GET',
        dataType: 'json',
        dataSrc: '',
    }).done(function (result) {
        console.log("Data dari server:", result);

        if (result && result.length > 0) {
            const tableBody = $("#tableListHire").find("tbody");
            tableBody.empty(); // Mengosongkan isi tabel

            $.each(result, function (index, item) {
                const employeeItem = buildEmployeeItem(item);
                tableBody.append(employeeItem);
            });
        } else {
            console.log("Tidak ada data yang ditemukan.");
        }
    }).fail(function (xhr, status, error) {
        console.error("Gagal mengambil data: " + error);
    });

    var guidInterview
    var guidEmp

    $(document).on('click', '.btn-lolos', function () {
        var btn = $(this); // Tombol yang diklik
        guidInterview = btn.data('guid1');
        guidEmp = btn.data('guid2');
        console.log("ini guid interview", guidInterview);
        console.log("ini guid employee", guidEmp);
    });


    function lolosInterview(intGuid) {
        console.log("ini guid di update", intGuid);

        var updateType = parseInt($("#updateType").val());
        var locUpdate = $("#location").val();
        var remarksUpate = $("#remarks").val();


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

    };
});
