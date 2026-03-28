<?php include 'app/views/shares/header.php'; ?>

<div class="container py-5" style="background-color: #f9f9f9;">
    <h1 class="text-center text-uppercase fw-bold mb-4" style="color: #343a40;">Thanh Toán</h1>

    <div class="row justify-content-center">
        <div class="col-lg-6">
            <div class="card shadow-sm" style="border-radius: 12px;">
                <div class="card-body p-4">
                    <form method="POST" action="/webbanhang/Product/processCheckout">
                        <!-- Họ tên -->
                        <div class="form-group mb-4">
                            <label for="name" class="form-label fw-bold">Họ tên:</label>
                            <input type="text" id="name" name="name" class="form-control" placeholder="Nhập họ tên của bạn" required>
                        </div>

                        <!-- Số điện thoại -->
                        <div class="form-group mb-4">
                            <label for="phone" class="form-label fw-bold">Số điện thoại:</label>
                            <input type="text" id="phone" name="phone" class="form-control" placeholder="Nhập số điện thoại của bạn" required>
                        </div>

                        <!-- Địa chỉ -->
                        <div class="form-group mb-4">
                            <label for="address" class="form-label fw-bold">Địa chỉ:</label>
                            <textarea id="address" name="address" class="form-control" rows="4" placeholder="Nhập địa chỉ giao hàng" required></textarea>
                        </div>

                        <!-- Nút Thanh Toán -->
                        <div class="d-grid">
                            <button type="submit" class="btn btn-success btn-lg shadow-sm">Thanh toán</button>
                        </div>
                    </form>
                </div>
            </div>

            <div class="text-center mt-4">
                <a href="/webbanhang/Product/cart" class="btn btn-secondary btn-lg shadow-sm">
                    <i class="bi bi-arrow-left"></i> Quay lại giỏ hàng
                </a>
            </div>
        </div>
    </div>
</div>

<?php include 'app/views/shares/footer.php'; ?>
