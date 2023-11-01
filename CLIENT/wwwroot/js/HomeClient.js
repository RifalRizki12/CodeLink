$(document).ready(function () {
    $.ajax({
        url: "/Employee/GetEmployeeData", // Ganti dengan URL yang sesuai
        method: "GET",
        dataType: 'json',
        dataSrc: 'data',
    }).done((result) => {
        console.log("Data dari server:", result); // Tambahkan ini

        const employeeGrid = $("#employeeGrid");

        $.each(result.data, (key, val) => {
            const baseURL = "https://localhost:7051/"; // Gantilah URL dasar sesuai dengan kebutuhan Anda
            const photoURL = `${baseURL}ProfilePictures/${val.foto}`; // Gabungkan baseURL dengan path gambar
            
            // Membuat tampilan grid untuk setiap karyawan
            const employeeItem = `
        <div class="col-lg-1-5 col-md-4 col-12 col-sm-6">
            <div class="product-cart-wrap mb-30 wow animate__animated animate__fadeIn" data-wow-delay=".1s">
                <div class="product-img-action-wrap">
                    <div class="product-img product-img-zoom">
                        <a href="shop-product-right.html">
                            <img src="${photoURL}" alt="Employee Photo" style="max-width: 500px; max-height: 500px;">
                        </a>
                    </div>
                    <div class="product-action-1">
                        <a aria-label="Add To Wishlist" class="action-btn" href="shop-wishlist.html"><i class="fi-rs-heart"></i></a>
                        <a aria-label="Compare" class "action-btn" href="shop-compare.html"><i class="fi-rs-shuffle"></i></a>
                        <a aria-label="Quick view" class="action-btn" data-bs-toggle="modal" data-bs-target="#quickViewModal"><i class="fi-rs-eye"></i></a>
                    </div>
                </div>
                <div class="product-content-wrap">
                    <div class="product-category">
                        <span>${val.statusEmployee}</span>
                    </div>
                    <h2><a href="">${val.fullName}</a></h2>
                    <div class="product-rate-cover">
                        <div class="product-rate d-inline-block">
                            <div class="product-rating" style="width: ${val.averageRating * 10}%"></div>
                        </div>
                        <span class="font-small ml-5 text-muted"> (${val.averageRating})</span>
                    </div>
                    <div>
                        <span class="font-small text-muted">${val.skill.join(', ')}</span>
                    </div>
                    <div class="product-card-bottom">
                        <div class="add-cart">
                            <button type="button" class=" btn btn-outline-success btn-detail" data-guid="${val.guid}" data-bs-toggle="modal" data-bs-target="#modalDetail" ><i class="mr-1 btn-detail" data-guid="${val.guid}"></i>Detail</button>
                        </div>
                        <button type="button" class="btn btn-outline-info" data-guid="${val.guid}" data-bs-toggle="modal" data-bs-target="#modalDetail"> Show CV </button> 

                    </div>
                </div>
            </div>
        </div>
        `;
            // Menambahkan kartu karyawan ke dalam grid
            employeeGrid.append(employeeItem);
        });
    }).fail((error) => {
        console.log(error);

    });
    var guid
    $('#employeeGrid').on('click', '.btn-detail', function () {
        guid = $(this).data('guid');// Mengambil GUID dari tombol "Update" yang diklik
        console.log('ini tombol yang atas', guid);
        getIdleByGuid(guid);
    });

    $('#modalDetail').on('click', '.btn-hire', function () {
        var guidHire = guid; // Mengambil GUID dari tombol "Update" yang diklik
        console.log('ini tombol yang hire', guidHire);
        $('#modalInterview').modal('show');

    });
   
    function getIdleByGuid(guid) {
        console.log("ini guid di paramter getby guid", guid);
        $.ajax({
            url: "/Employee/GetGuidEmployee/" + guid, // Ganti dengan URL yang sesuai
            method: "GET",
            dataType: 'json',
            dataSrc: 'data',
        }).done((result) => {
            console.log("Data dari server:", result);
            const baseURL = "https://localhost:7051/"; // Gantilah URL dasar sesuai dengan kebutuhan Anda
            const photoURL = `${baseURL}ProfilePictures/${result.foto}`;
           // var employeeModal = $("#employeeModal");
                var employeeValue = ` 
                <div class="product-img product-img-zoom">
                    <a href="shop-product-right.html">
                        <img class="hover-img" src="${photoURL}" alt="Employee Photo" style="max-width: 470px; max-height: 500px;"/>
                    </a>
                </div>
                <h2 class="title-detail" >${result.fullName}</h2>
                <div class="product-detail-rating">
                    <div class="product-rate-cover text-end">
                        <span class="font-small mr-4 text-muted"> Rating Kinerja </span>

                        <div class="product-rate d-inline-block">
                            <div class="product-rating" style="width: 90%">${result.averageRating}</div>
                        </div>
                    </div>
                </div>
                <div class="clearfix product-price-cover">
                    <div class="product-price primary-color float-left">
                        <h4 class="current-price text-brand title-detail" id="grade">Grade: ${result.grade}</h4>
                    </div>
                </div>
                <div class="clearfix product-price-cover">
                    <div class="product-price primary-color float-left">
                        <h4 class="current-price text-brand little-detail">Skills</h4>
                    </div>
                </div>
                <div class="short-desc mb-30">
                    <p class="font-lg skillData">${result.skill.join(', ')}</p>
                </div>
                <div class="clearfix product-price-cover">
                    <div class="product-price primary-color float-left">
                        <h4 class="current-price text-brand little-detail">Description</h4>
                    </div>
                </div>
                <div class="short-desc mb-30">
                    <p class="font-lg skillData">Calon partner yang akan bergabung dengan perusahaan anda gendernya ${result.gender}
                        No Handphone yang dapat dihubungi adalah ${result.phoneNumber} kontak email yang tersedia adalah ${result.email} 
                        cv dapat diakses melalui <a href="${result.cv}">lihat CV</a> 
                    </p>
                </div>
                `;
                 $(".modalBodyDetail").html(employeeValue)
            }).fail((error) => {
            console.log(error);
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Failed to retrieve employee data.'
            });




        });
    };
});
