<?php include 'app/views/shares/header.php'; ?>

<div class="container mt-5">
    <?php if ($product): ?>
        <div class="card shadow-sm">
            <div class="card-header text-center bg-primary text-white">
                <h1>Chi tiết sản phẩm</h1>
            </div>
            <div class="card-body">
                <h2 class="text-center text-secondary"><?php echo htmlspecialchars($product->name, ENT_QUOTES, 'UTF-8'); ?></h2>
                <div class="text-center my-3">
                    <?php if ($product->image): ?>
                        <img src="/webbanhang/<?php echo $product->image; ?>" alt="Hình ảnh sản phẩm" class="img-fluid rounded" style="max-width: 300px;">
                    <?php else: ?>
                        <p class="text-muted">Không có hình ảnh sản phẩm.</p>
                    <?php endif; ?>
                </div>
                <p><strong>Mô tả:</strong> <?php echo htmlspecialchars($product->description, ENT_QUOTES, 'UTF-8'); ?></p>
                <p><strong>Giá:</strong> <span class="text-success"><?php echo htmlspecialchars($product->price, ENT_QUOTES, 'UTF-8'); ?> VND</span></p>
                <p><strong>Danh mục:</strong> <span class="category"><?php echo htmlspecialchars($product->category_name ?? 'Chưa có danh mục', ENT_QUOTES, 'UTF-8'); ?></span></p>
                   
                </p>
                <div class="d-flex justify-content-center mt-4">
                    <a href="/webbanhang/Product/edit/<?php echo $product->id; ?>" class="btn btn-warning mx-2">Sửa</a>
                    <a href="/webbanhang/Product/delete/<?php echo $product->id; ?>" class="btn btn-danger mx-2" onclick="return confirm('Bạn có chắc chắn muốn xóa sản phẩm này?');">Xóa</a>
                    <a href="/webbanhang/Product/addToCart/<?php echo $product->id; ?>" class="btn btn-primary mx-2">Thêm vào giỏ hàng</a>
                </div>
            </div>
        </div>
    <?php else: ?>
        <div class="alert alert-warning text-center">
            <p>Không tìm thấy sản phẩm.</p>
        </div>
    <?php endif; ?>
</div>

<?php include 'app/views/shares/footer.php'; ?>
