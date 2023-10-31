$.ajax({
    url: "/Employee/GetEmployeeData", // Ganti dengan URL yang sesuai
    method: "GET",
    dataType: 'json',
    dataSrc: 'data',
}).done((result) => {
    console.log("Data dari server:", result); // Tambahkan ini

    const employeeGrid = $("#employeeGrid");

    $.each(result.data, (key, val) => {
        // Membuat tampilan grid untuk setiap karyawan
        const employeeItem = `
        <div class="col-lg-1-5 col-md-4 col-12 col-sm-6">
            <div class="product-cart-wrap mb-30 wow animate__animated animate__fadeIn" data-wow-delay=".1s">
                <div class="product-img-action-wrap">
                    <div class="product-img product-img-zoom">
                        <a href="shop-product-right.html">
                            <img class="default-img" src="${val.foto}" alt="${val.fullName}" />
                            <img class="hover-img" src="${val.foto}" alt="${val.fullName}" />
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
                            <a class="add" href=""><i class="fi-rs-shopping-cart mr-5"></i>Add</a>
                        </div>
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
