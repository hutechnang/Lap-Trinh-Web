<?php include 'app/views/shares/header.php'; ?>

<div class="container mt-5">
    <h1 class="text-center mb-4 text-primary">Giỏ hàng của bạn</h1>

    <?php if (!empty($_SESSION['cart'])): ?>
        <div class="cart-summary bg-light p-4 rounded shadow-sm">
            <ul class="list-group">
                <?php foreach ($_SESSION['cart'] as $id => $item): ?>
                    <li class="list-group-item d-flex justify-content-between align-items-center">
                        <div>
                            <h2 class="text-secondary"><?php echo htmlspecialchars($item['name'], ENT_QUOTES, 'UTF-8'); ?></h2>
                            <p class="text-success">Giá: <?php echo number_format(htmlspecialchars($item['price'], ENT_QUOTES, 'UTF-8')); ?> VND</p>
                            <p>
                                Số lượng: 
                                <form method="POST" action="" class="d-inline">
                                    <!-- Nút Trừ -->
                                    <input type="hidden" name="product_id" value="<?php echo $id; ?>">
                                    <input type="hidden" name="action" value="decrease">
                                    <button type="submit" class="btn btn-sm btn-warning">-</button>
                                </form>
                                <span class="text-warning"><?php echo htmlspecialchars($item['quantity'], ENT_QUOTES, 'UTF-8'); ?></span>
                                <form method="POST" action="" class="d-inline">
                                    <!-- Nút Cộng -->
                                    <input type="hidden" name="product_id" value="<?php echo $id; ?>">
                                    <input type="hidden" name="action" value="increase">
                                    <button type="submit" class="btn btn-sm btn-success">+</button>
                                </form>
                            </p>
                        </div>
                        <?php if (!empty($item['image'])): ?>
                            <img src="/webbanhang/<?php echo $item['image']; ?>" alt="Hình ảnh sản phẩm" class="img-thumbnail" style="max-width: 100px;">
                        <?php endif; ?>
                        <div>
                            <!-- Nút Xóa sản phẩm -->
                            <form method="POST" action="" onsubmit="return confirmDelete();">
                                <input type="hidden" name="product_id" value="<?php echo $id; ?>">
                                <button type="submit" class="btn btn-danger btn-sm">Xóa</button>
                            </form>
                        </div>
                    </li>
                <?php endforeach; ?>
            </ul>
        </div>
    <?php else: ?>
        <div class="alert alert-warning text-center">
            <p>Giỏ hàng của bạn đang trống. Hãy thêm sản phẩm để trải nghiệm mua sắm!</p>
        </div>
    <?php endif; ?>

    <div class="mt-4 text-center">
        <a href="/webbanhang/Product" class="btn btn-primary me-3">Tiếp tục mua sắm</a>
        <a href="/webbanhang/Product/checkout" class="btn btn-success">Thanh toán</a>
    </div>
</div>

<?php include 'app/views/shares/footer.php'; ?>

<script>
// Hàm xác nhận xóa
function confirmDelete() {
    return confirm('Bạn có chắc chắn muốn xóa sản phẩm này khỏi giỏ hàng?');
}
</script>
