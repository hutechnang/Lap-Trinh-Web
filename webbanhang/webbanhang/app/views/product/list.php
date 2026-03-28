<?php include 'app/views/shares/header.php'; ?>

<div class="container py-5" style="background-color: #f5f7fa;"> <!-- Nền mềm mại và nhẹ nhàng -->
    <h1 class="text-center text-uppercase fw-bold mb-5" style="color: #3c4043;">Danh Sách Sản Phẩm</h1>

    <div class="d-flex justify-content-between align-items-center mb-4">
        <h5 class="text-muted">Tổng số sản phẩm: <strong><?php echo count($products ?? []); ?></strong></h5>
        <a href="/webbanhang/Product/add" class="btn btn-success btn-lg shadow-sm" style="background-color: #4CAF50; border-color: #4CAF50;">
            <i class="bi bi-plus-circle"></i> Thêm sản phẩm mới
        </a>
    </div>

    <?php if (!empty($products)): ?>
        <div class="row g-4">
            <?php foreach ($products as $product): ?>
                <div class="col-12 col-sm-6 col-md-4 col-lg-3">
                    <div class="card h-100 border-0 shadow-sm" style="border-radius: 12px; background-color: #ffffff;">
                        <!-- Hình ảnh sản phẩm -->
                        <?php if (!empty($product->image)): ?>
                            <img src="/webbanhang/<?php echo htmlspecialchars($product->image, ENT_QUOTES, 'UTF-8'); ?>" 
                                 class="card-img-top img-fluid" 
                                 alt="Hình ảnh sản phẩm" 
                                 style="max-height: 200px; object-fit: cover; border-top-left-radius: 12px; border-top-right-radius: 12px;">
                        <?php else: ?>
                            <div class="d-flex align-items-center justify-content-center bg-light text-muted" style="height: 200px; border-top-left-radius: 12px; border-top-right-radius: 12px;">
                                <em>Không có hình ảnh</em>
                            </div>
                        <?php endif; ?>

                        <!-- Nội dung sản phẩm -->
                        <div class="card-body text-center">
                            <h5 class="card-title text-truncate fw-bold">
                                <a href="/webbanhang/Product/show/<?php echo htmlspecialchars($product->id, ENT_QUOTES, 'UTF-8'); ?>" 
                                   class="text-dark text-decoration-none">
                                   <?php echo htmlspecialchars($product->name ?? 'Không có tên', ENT_QUOTES, 'UTF-8'); ?>
                                </a>
                            </h5>
                            <p class="text-success fw-bold mb-2">
                                <?php echo number_format(htmlspecialchars($product->price ?? 0, ENT_QUOTES, 'UTF-8')); ?> VND
                            </p>
                            <p class="text-muted small">
                                <?php echo htmlspecialchars($product->description ?? 'Không có mô tả.', ENT_QUOTES, 'UTF-8'); ?>
                            </p>
                            <p>
                                <span class="badge rounded-pill" style="background-color: #e8f5e9; color: #388e3c;">
                                    <?php echo htmlspecialchars($product->category_name ?? 'Không có danh mục.', ENT_QUOTES, 'UTF-8'); ?>
                                </span>
                            </p>
                        </div>

                        <!-- Nút thao tác -->
                        <div class="card-footer bg-light d-flex justify-content-around" style="border-top: 1px solid #e0e0e0;">
                            <a href="/webbanhang/Product/edit/<?php echo htmlspecialchars($product->id, ENT_QUOTES, 'UTF-8'); ?>" 
                               class="btn btn-warning btn-sm text-white" style="background-color: #FFC107; border-color: #FFC107;">
                               <i class="bi bi-pencil-square"></i> Sửa
                            </a>
                            <a href="/webbanhang/Product/delete/<?php echo htmlspecialchars($product->id, ENT_QUOTES, 'UTF-8'); ?>" 
                               class="btn btn-danger btn-sm text-white" style="background-color: #F44336; border-color: #F44336;"
                               onclick="return confirm('Bạn có chắc chắn muốn xóa sản phẩm này?');">
                               <i class="bi bi-trash"></i> Xóa
                            </a>
                            <a href="/webbanhang/Product/addToCart/<?php echo htmlspecialchars($product->id, ENT_QUOTES, 'UTF-8'); ?>" 
                               class="btn btn-primary btn-sm text-white" style="background-color: #2196F3; border-color: #2196F3;">
                               <i class="bi bi-cart-plus"></i> Giỏ hàng
                            </a>
                        </div>
                    </div>
                </div>
            <?php endforeach; ?>
        </div>
    <?php else: ?>
        <div class="alert alert-info text-center py-5 rounded" style="background-color: #e3f2fd; border: 1px solid #90caf9;">
            <h5 class="text-dark">Hiện không có sản phẩm nào trong danh sách.</h5>
            <a href="/webbanhang/Product/add" class="btn btn-primary">Thêm sản phẩm mới</a>
        </div>
    <?php endif; ?>
</div>

<?php include 'app/views/shares/footer.php'; ?>
