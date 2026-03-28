<?php include 'app/views/shares/header.php'; ?>

<div class="container mt-5">
    <h1 class="text-center text-primary mb-4">Đăng ký tài khoản</h1>

    <!-- Hiển thị lỗi nếu có -->
    <?php if (isset($errors)): ?>
        <div class="alert alert-danger">
            <ul>
                <?php foreach ($errors as $err): ?>
                    <li><?php echo htmlspecialchars($err, ENT_QUOTES, 'UTF-8'); ?></li>
                <?php endforeach; ?>
            </ul>
        </div>
    <?php endif; ?>

    <!-- Form đăng ký -->
    <div class="card shadow p-4">
        <form class="user" action="/webbanhang/account/save" method="post">
            <div class="form-group row">
                <!-- Username -->
                <div class="col-sm-6 mb-3">
                    <input type="text" class="form-control form-control-user" 
                           id="username" name="username" placeholder="Tên đăng nhập" required>
                </div>
                <!-- Fullname -->
                <div class="col-sm-6">
                    <input type="text" class="form-control form-control-user" 
                           id="fullname" name="fullname" placeholder="Họ và tên" required>
                </div>
            </div>
            <div class="form-group row">
                <!-- Password -->
                <div class="col-sm-6 mb-3">
                    <input type="password" class="form-control form-control-user" 
                           id="password" name="password" placeholder="Mật khẩu" required>
                </div>
                <!-- Confirm Password -->
                <div class="col-sm-6">
                    <input type="password" class="form-control form-control-user" 
                           id="confirmpassword" name="confirmpassword" placeholder="Xác nhận mật khẩu" required>
                </div>
            </div>
            <div class="form-group row">
                <!-- Email -->
                <div class="col-sm-6 mb-3">
                    <input type="email" class="form-control form-control-user" 
                           id="email" name="email" placeholder="Email" required>
                </div>
                <!-- Phone -->
                <div class="col-sm-6">
                    <input type="text" class="form-control form-control-user" 
                           id="phone" name="phone" placeholder="Số điện thoại" required>
                </div>
            </div>
            <div class="form-group">
                <!-- Address -->
                <textarea class="form-control form-control-user" 
                          id="address" name="address" rows="3" placeholder="Địa chỉ (tùy chọn)"></textarea>
            </div>
            <div class="form-group text-center">
                <!-- Submit Button -->
                <button class="btn btn-primary btn-icon-split p-3">Đăng ký</button>
            </div>
        </form>
    </div>
</div>

<?php include 'app/views/shares/footer.php'; ?>
