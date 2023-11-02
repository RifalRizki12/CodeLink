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
                   <h6 class="text-brand">${item.date}</h6>
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
        url: '/Interview/ListHireIdle/' + guid,
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
});
