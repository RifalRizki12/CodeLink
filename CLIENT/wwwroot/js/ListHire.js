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
                   <button class="btn btn-sm btn-lolos" data-guid1="${item.interviewGuid}" data-guid2="${item.employeGuid}" data-bs-toggle="modal" data-bs-target="#lolosInterview">Lolos</button>
               </td>
               <td class="text-right" data-title="tidakLolos">
                   <button class="btn-danger btn-sm btn-tdkLolos" data-guid1="${item.interviewGuid}" data-guid2="${item.employeGuid}" data-bs-toggle="modal" data-bs-target="#tdkLolosInterview" >Tidak Lolos</button>
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

    $("#tableListHire").find("tbody").on('click', '.btn-lolos', function () {
        var btn = $(this); // Tombol yang diklik
        guidInterview = btn.data('guid1');
        guidEmp = btn.data('guid2');
    });

    $("#lolosForm").submit(function (event) {
        event.preventDefault();
        lolosInterview(guidInterview);
    });

    function lolosInterview(guidInterview) {
        console.log("ini guid di update", guidInterview);

        var contractStart = $("#startContractInp").val();
        var contractEnd = $("#endContractInp").val();
        var status = 0;

        var obj = {
            guid: guidInterview,
            employeeGuid: guidEmp,
            ownerGuid: guidCompny,
            statusIntervew: status,
            startContract: contractStart,
            endContract: contractEnd,
            
        };

        console.log(obj);
        // lnjut disini, belum bikin controller dan repo
        $.ajax({
            url: '/Interview/Announcement/' + guidInterview,
            type: 'PUT',
            data: JSON.stringify(obj),
            contentType: 'application/json',
            success: function (response) {
                console.log(response);
                $('#lolosInterview').modal('hide');
                //$('#tableListHire').DataTable().ajax.reload();
                Swal.fire({
                    icon: 'success',
                    title: 'Proses Rekrutmen Berhasil',
                    text: 'Orang yang Anda pilih telah berhasil direkrut!.'
                });
            },
            error: function (response) {
                $('#lolosInterview').modal('hide');
                Swal.fire({
                    icon: 'error',
                    title: 'Perekrutan gagal',
                    text: 'Terjadi kesalahan saat mencoba merekrut partner.'
                });
            }
        });

    };

    $("#tableListHire").find("tbody").on('click', '.btn-tdkLolos', function () {
        var btn = $(this); // Tombol yang diklik
        guidInterview = btn.data('guid1');
        guidEmp = btn.data('guid2');
    });

    $("#tdkLolosForm").submit(function (event) {
        event.preventDefault();
        tidakLolosInterview(guidInterview);
    });
    function tidakLolosInterview(guidInterview) {
        console.log("ini guid di update", guidInterview);

        var feedbackInput = $("#feedback").val();
        var status = 1;

        var obj = {
            guid: guidInterview,
            employeeGuid: guidEmp,
            ownerGuid: guidCompny,
            statusIntervew: status,
            feedBack: feedbackInput

        };

        console.log(obj);
        // lnjut disini, belum bikin controller dan repo
        $.ajax({
            url: '/Interview/Announcement/' + guidInterview,
            type: 'PUT',
            data: JSON.stringify(obj),
            contentType: 'application/json',
            success: function (response) {
                console.log(response);
                $('#tdkLolosInterview').modal('hide');
                //$('#tableListHire').DataTable().ajax.reload();
                Swal.fire({
                    icon: 'success',
                    title: 'Rekrutmen Tidak Lolos',
                    text: 'Orang yang Anda pilih tidak lolos dalam rekrutmen.'
                });
            },
            error: function (response) {
                $('#tdkLolosInterview').modal('hide');
                Swal.fire({
                    icon: 'error',
                    title: 'Pembaruan data tidak lolos gagal',
                    text: 'Terjadi kesalahan saat anda mencoba menolak partner.'
                });
            }
        });

    };
   
});

$(document).ready(function () {
    var guid = document.getElementById("employeeGuid").getAttribute("data-guid");
    console.log(guid);

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
                   <h6 class="text-brand">${item.startContract}</h6>
               </td>
               <td class="product-des product-name" data-title="date">
                   <h6 class="text-brand">${item.endContract}</h6>
               </td>
               <td class="text-right" data-title="Lolos">
                   <button class="btn btn-sm">Lolos</button>
               </td>
               <td class="text-right" data-title="tidakLolos">
                   <button class="btn-danger btn-sm">Tidak Lolos</button>
               </td>
            </tr>
        `;
    }

    // Mengambil data dari server
    $.ajax({
        url: '/Interview/GetOnsite/' + guid,
        type: 'GET',
        dataType: 'json',
        dataSrc: '',
    }).done(function (result) {
        console.log("Data dari server:", result);

        if (result && result.length > 0) {
            const tableBody = $("#tableListOnsite").find("tbody");
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
});

$(document).ready(function () {
    var guid = document.getElementById("employeeGuid").getAttribute("data-guid");
    console.log(guid);

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
                   <h6 class="text-brand">${item.startContract}</h6>
               </td>
               <td class="product-des product-name" data-title="date">
                   <h6 class="text-brand">${item.endContract}</h6>
               </td>
               <td class="text-right" data-title="Lolos">
                   <button class="btn btn-sm">Lolos</button>
               </td>
            </tr>
        `;
    }

    // Mengambil data dari server
    $.ajax({
        url: '/Interview/GetIdleHistory/' + guid,
        type: 'GET',
        dataType: 'json',
        dataSrc: '',
    }).done(function (result) {
        console.log("Data dari server:", result);

        if (result && result.length > 0) {
            const tableBody = $("#tableGetIdleHistory").find("tbody");
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
});

