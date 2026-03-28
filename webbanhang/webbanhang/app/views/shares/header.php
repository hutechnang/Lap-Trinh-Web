<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Quản lý sản phẩm</title>
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet">
    <style>
        .product-image {
            max-width: 100px;
            height: auto;
        }
    </style>
</head>

<body>
    <!-- Thanh điều hướng -->
    <nav class="navbar navbar-expand-lg navbar-light bg-light">
        <a class="navbar-brand" href="/webbanhang">Quản lý sản phẩm</a>
        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav"
                aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
            <ul class="navbar-nav">
                <li class="nav-item">
                    <a class="nav-link" href="/webbanhang/Product/">Danh sách sản phẩm</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="/webbanhang/Product/add">Thêm sản phẩm</a>
                    
           
                </li>
                <li class="nav-item">
                    <!-- Kiểm tra trạng thái đăng nhập -->
                    <?php 
                        if (SessionHelper::isLoggedIn()) {
                            echo "<a class='nav-link'>Chào, " . htmlspecialchars($_SESSION['username'], ENT_QUOTES, 'UTF-8') . "</a>";
                        } else {
                            echo "<a class='nav-link' href='/webbanhang/account/login'>Đăng nhập</a>";
                        }
                    ?>
                </li>
                <li class="nav-item">
                    <?php 
                        if (SessionHelper::isLoggedIn()) {
                            echo "<a class='nav-link' href='/webbanhang/account/logout'>Đăng xuất</a>";
                        }
                    ?>
                </li>
                <li class="nav-item">
                    <!-- Hiển thị giỏ hàng -->
                    <a class="nav-link btn btn-primary text-white" href="/webbanhang/Product/cart">
                        Giỏ hàng 
                       
                    </a>
                </li>
            </ul>
        </div>
    </nav>

    <!-- Nội dung chính -->
    <div class="container mt-4">
        <!-- Nội dung động sẽ được hiển thị ở đây -->
    </div>

    <!-- Tài nguyên JavaScript -->
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.5.2/dist/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
</body>

</html>