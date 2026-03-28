<?php include 'app/views/shares/header.php'; ?>
<h1>Xác nhận đơn hàng</h1>
<p>Cảm ơn bạn đã đặt hàng. Đơn hàng của bạn đã được xử lý thành công.</p>

<!-- Thêm CSS trực tiếp -->
<style>
    .btn-secondary {
        background-color: #007bff; /* Màu xanh dương */
        color: white; /* Màu chữ trắng */
        border-radius: 5px; /* Bo tròn các góc */
        padding: 10px 20px; /* Đệm cho nút */
        text-decoration: none; /* Bỏ gạch chân */
    }

    .btn-secondary:hover {
        background-color: #45a049; /* Màu khi hover (di chuột vào) */
    }
</style>

<a href="/webbanhang/Product" class="btn btn-secondary mt-2">Tiếp tục mua sắm</a>

<?php include 'app/views/shares/footer.php'; ?>