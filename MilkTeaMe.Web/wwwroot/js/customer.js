function formatVND(value) {
    return value.toLocaleString('vi-VN') + 'đ';
}

function showCart() {
    const container = document.getElementById("cartContent");
    const localCart = localStorage.getItem("cart");

    if (!localCart) {
        container.innerHTML = "<p>Giỏ hàng trống.</p>";
        return;
    }

    const cart = JSON.parse(localCart);

    if (cart.length === 0) {
        container.innerHTML = "<p>Giỏ hàng trống.</p>";
        return;
    }

    container.innerHTML = "";

    var totalPrice = 0;

    cart.forEach((item, index) => {

        totalPrice += item.totalMoney * item.quantity;

        const toppingHtml = `
            <div class="d-flex overflow-auto gap-2 mt-1 pb-1 toppings-scrollable" style="white-space: nowrap;">
                ${item.toppings.map(t => `
                    <div class="text-center">
                        <img src="${t.ImageUrl}" width="50" height="50" class="rounded" style="object-fit: cover;" />
                        <div style="font-size: 12px;">${t.Name}</div>
                    </div>
                `).join("")}
            </div>
        `;
        const html = `
            <div class="card mb-3 shadow-sm">
                <div class="row g-0">
                    <div class="col-md-3 text-center">
                        <img src="${item.imageUrl}" class="img-fluid rounded-start p-2" style="max-height: 150px; object-fit: contain;" />
                    </div>
                    <div class="col-md-9">
                        <div class="card-body">
                        <div class="d-flex justify-content-between">
                            <h5 class="card-title">${item.name}(${item.size})</h5>     
                              <button class="btn btn-sm btn-danger" onclick="removeCartItem(${index})">
                                  <i class="bi bi-trash"></i>
                              </button>
                            </div>
                            <p class="card-text mb-1" style="color: #8a733f">${formatVND(item.totalMoney)} x ${item.quantity} = ${formatVND(item.totalMoney * item.quantity)}</p>
                            <div><strong>Toppings:</strong> ${item.toppings.length > 0 ? "" : "Không có topping"}</div>
                            <div>${toppingHtml}</div>
                        </div>
                    </div>
                </div>
            </div>
        `;

        container.innerHTML += html;
    });

    container.innerHTML += `
        <div class="d-flex justify-content-between align-items-center py-2">
                <span class="fw-bold text-danger fs-5">Tổng giá trị đơn hàng: ${formatVND(totalPrice)}</span>
                <button class="btn btn-success" onclick="proceedToOrderPage()">Đặt hàng</button>
        </div>
    `;
}

function removeCartItem(index) {
    let cart = JSON.parse(localStorage.getItem("cart")) || [];
    cart.splice(index, 1);
    localStorage.setItem("cart", JSON.stringify(cart));
    toastr.success("Đã xóa sản phẩm khỏi đơn hàng");
    showCart();
}

function proceedToOrderPage() {
    const cart = JSON.parse(localStorage.getItem("cart")) || [];

    if (cart.length > 0) {
        window.location.href = '/Orders';
    } else {
        toastr.error("Giỏ hàng đang rỗng");
    }
}