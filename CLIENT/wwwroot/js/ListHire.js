//INTERVIEW LIST MASING-MASING PERUSAHAAN DAN TOMBOL LOLOS/TIDAK LOLOS
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

        var obj = {
            guid: guidInterview,
            employeeGuid: guidEmp,
            ownerGuid: guidCompny,
            statusIntervew: 0,
            startContract: $("#startContractInp").val(),
            endContract: $("#endContractInp").val(),
            
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
                    text: 'Terimakasih Orang Baik!!!.'
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

        var obj = {
            guid: guidInterview,
            employeeGuid: guidEmp,
            ownerGuid: guidCompny,
            statusIntervew: 1,
            feedBack: $("#feedback").val()

        };

        console.log(obj);
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

//EMPLOYEE ONSITE DAN  END CONTRACT 
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
                   <button class="btn-danger btn-sm btn-endContract" data-guid1="${item.interviewGuid}" data-guid2="${item.employeGuid}" data-bs-toggle="modal" data-bs-target="#endContract">End Contract</button>
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



    //FUNCTION UNTUK TOMBOL END CONTRACT
    $("#tableListOnsite").find("tbody").on('click', '.btn-endContract', function () {
        var btn = $(this); // Tombol yang diklik
        guidInterview = btn.data('guid1');
        guidEmp = btn.data('guid2');
    });

    $("#endContractForm").submit(function (event) {
        event.preventDefault();
        endContract(guidInterview);
    });
    /*var remarksInput = */
    /* var status = 2;*/
    function endContract(guidInterview) {
        console.log("ini guid di update", guidInterview);

     
        var today = new Date(); // Membuat objek Date saat ini
        var formattedDate = today.toISOString().split('T')[0];
        var obj = {
            guid: guidInterview,
            employeeGuid: guidEmp,
            ownerGuid: guid,
            endContract: formattedDate,
            statusIntervew: 3,
            remarks: $("#remarks").val(),
        };

        console.log(obj);
        $.ajax({
            url: '/Interview/Announcement/' + guidInterview,
            type: 'PUT',
            data: JSON.stringify(obj),
            contentType: 'application/json',
            success: function (response) {
                console.log(response);
                $('#endContract').modal('hide');
                //$('#tableListHire').DataTable().ajax.reload();
                Swal.fire({
                    icon: 'success',
                    title: 'End Contract Berhasil Dilakukan',
                    text: 'Semoga cepat menemukan orang baru!.'
                });
            },
            error: function (response) {
                $('#endContract').modal('hide');
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

        const btnGuid = item.employeeGuid; // Asumsikan employeeGuid adalah unik
        const isRated = item.hasRated || localStorage.getItem(btnGuid) === 'true'; // Tambahkan pemeriksaan localStorage di sini
        const ratedStyle = isRated ? 'background-color: grey; border-color: grey; color: white; cursor: not-allowed;' : '';
        const disabledAttribute = isRated ? 'disabled' : ''; // Tambahkan atribut "disabled" jika sudah dinilai

        return `
       <tr class="pt-30" style="text-align:center;">
          <td class="image product-thumbnail pt-40">
               <img src="${photoURL}" alt="${item.idle}">
          </td>
          <td class="product-des product-name">
          </td>
          <td class="product-des product-name" data-title="date">
              <h6 class="text-brand">${item.idle}</h6>
          </td>
          <td class="product-des product-name" data-title="date">
              <h6 class="text-brand">${item.endContract}</h6>
          </td>
          <td class="text-right" data-title="Lolos">
           <button ${disabledAttribute} style="${ratedStyle}" class="btn-danger btn-sm btn-rating" data-rated="${isRated ? 'true' : 'false'}" data-guid1="${item.interviewGuid}" data-guid2="${btnGuid}" data-bs-toggle="modal" data-bs-target="#ratingInterview" >Rating</button>
               </td>
            </tr>
        `;
    }
    document.body.addEventListener('click', function (e) {
        if (e.target && e.target.classList.contains('btn-rating')) {
            const button = e.target;
            button.setAttribute('data-active', "true");  // Tandai tombol sebagai yang sedang aktif
        }
    });

    // Saat halaman dimuat
    document.addEventListener('DOMContentLoaded', function () {
        let ratingButtons = document.querySelectorAll('.btn-rating');
        ratingButtons.forEach(function (button) {
            const btnGuid = button.getAttribute('data-guid2');
            if (localStorage.getItem(btnGuid) === 'true') {
                button.disabled = true;
                button.style.backgroundColor = "grey";
                button.style.borderColor = "grey";
                button.style.color = "white";
                button.style.cursor = "not-allowed";
                button.setAttribute('data-rated', "true");
            }
        });
    });

    document.getElementById('rating').addEventListener('click', function () {
        // Ambil tombol rating yang saat ini sedang aktif
        const activeRatingButton = document.querySelector('.btn-rating[data-active="true"]');

        if (activeRatingButton) {
            const btnGuid = activeRatingButton.getAttribute('data-guid2');

            // Ubah style tombol rating menjadi tidak aktif
            activeRatingButton.disabled = true;
            activeRatingButton.style.backgroundColor = "grey";
            activeRatingButton.style.borderColor = "grey";
            activeRatingButton.style.color = "white";
            activeRatingButton.style.cursor = "not-allowed";

            // Set atribut data-rated menjadi true, hapus atribut data-active, dan simpan status ke localStorage
            activeRatingButton.setAttribute('data-rated', "true");
            activeRatingButton.removeAttribute('data-active');
            localStorage.setItem(btnGuid, 'true');
        }
    });

    


    // Mengambil data dari server
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

    $("#tableGetIdleHistory").find("tbody").on('click', '.btn-rating', function () {
        var btn = $(this); // Tombol yang diklik
        guidInterview = btn.data('guid1');
        guidEmp = btn.data('guid2');
        console.log("guid emp:", guidEmp)
    });

    $("#ratingForm").submit(function (event) {
        event.preventDefault();
        rateInterview(guidInterview);
    });
    function rateInterview(guidInterview) {
        console.log("ini guid di update", guidInterview);

        var feedbackInput = $("#feedback").val();
        var rateInput;
        if ($('input[name="rating"]:checked').length > 0) {
            rateInput = parseInt($('input[name="rating"]:checked').val());
        } else {
            // Anda bisa menetapkan nilai default jika tidak ada bintang yang dipilih
            rateInput = 0;
        }
        console.log(rateInput);



        var obj = {
            guid: guidInterview,
            employeeGuid: guidEmp,
            ownerGuid: guid,
            feedBack: feedbackInput,
            rate: rateInput,

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
                $('#ratingInterview').modal('hide');
                //$('#tableListHire').DataTable().ajax.reload();
                Swal.fire({
                    icon: 'success',
                    title: 'Terimakasih Atas Kepercayaanya',
                    text: 'Orang yang Anda pilih tidak lolos dalam rekrutmen.'
                });
            },
            error: function (response) {
                $('#ratingInterview').modal('hide');
                Swal.fire({
                    icon: 'error',
                    title: 'Pembaruan data tidak lolos gagal',
                    text: 'Terjadi kesalahan saat anda mencoba menolak partner.'
                });
            }
        });

    };

});

document.addEventListener('DOMContentLoaded', function () {
    let labels = document.querySelectorAll('.rating label');
    labels.forEach(function (label) {
        label.addEventListener('click', function () {
            this.style.color = 'gold';
            let prev = this.previousElementSibling;
            while (prev) {
                if (prev.tagName.toLowerCase() === 'label') {
                    prev.style.color = 'gold';
                }
                prev = prev.previousElementSibling;
            }
            let next = this.nextElementSibling;
            while (next) {
                if (next.tagName.toLowerCase() === 'label') {
                    next.style.color = 'gray';
                }
                next = next.nextElementSibling;
            }
        });
    });
});