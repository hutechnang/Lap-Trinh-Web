<?php
require_once('app/config/database.php');
require_once('app/models/AccountModel.php');
class AccountController {
    private $accountModel;
    private $db;
    public function __construct() {
    $this->db = (new Database())->getConnection();
    $this->accountModel = new AccountModel($this->db);
    }
    function register(){
    include_once 'app/views/account/register.php';
    }
    public function login() {
    include_once 'app/views/account/login.php';
    }
    function save(){
    if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    $username = $_POST['username'] ?? '';
    $fullName = $_POST['fullname'] ?? '';
    $password = $_POST['password'] ?? '';
    $confirmPassword = $_POST['confirmpassword'] ?? '';
    $errors =[];
    if(empty($username)){
    $errors['username'] = "Vui long nhap userName!";
    }
    if(empty($fullName)){
    $errors['fullname'] = "Vui long nhap fullName!";
    }
    if(empty($password)){
    $errors['password'] = "Vui long nhap password!";
    }
    if($password != $confirmPassword){
    $errors['confirmPass'] = "Mat khau va xac nhan chua dung";
    }
    //kiểm tra username đã được đăng ký chưa?
    $account = $this->accountModel->getAccountByUsername($username);
    if($account){
    $errors['account'] = "Tai khoan nay da co nguoi dang ky!";
    }
    if(count($errors) > 0){
    include_once 'app/views/account/register.php';
    }else{
    $password = password_hash($password, PASSWORD_BCRYPT, ['cost' => 12]);
    $result = $this->accountModel->save($username, $fullName, $password);
if($result){
header('Location: /webbanhang/account/login');
}
}
}
}
function logout(){
unset($_SESSION['username']);
unset($_SESSION['role']);
header('Location: /webbanhang/product');
}
public function checkLogin(){
// Kiểm tra xem liệu form đã được submit
if ($_SERVER['REQUEST_METHOD'] == 'POST') {
$username = $_POST['username'] ?? '';
$password = $_POST['password'] ?? '';
$account = $this->accountModel->getAccountByUserName($username);
if ($account) {
$pwd_hashed = $account->password;
//check mat khau
if (password_verify($password, $pwd_hashed)) {
session_start();
// $_SESSION['user_id'] = $account->id;
// $_SESSION['user_role'] = $account->role;
$_SESSION['username'] = $account->username;
header('Location: /webbanhang/product');
exit;
}
else {
echo "Password incorrect.";
}
} else {
echo "Bao loi khong tim thay tai khoan";
}
}
}
}