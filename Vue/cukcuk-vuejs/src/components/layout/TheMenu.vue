<template>
    <div class="navbar">
      <div class="title">
        <img class="title__toggle" src="../../assets/img/toggle.png" alt="">
        <img class="title__logo" src="../../assets/img/cukcuk-logo.png" alt="">
      </div>
      <div class="menu-left">
        <div class="menu-left__icon">
          <img src="../../assets/icon/dashboard.png" alt="">
        </div>
        <p class="menu-left__text">Tổng quan</p>
      </div>
      <div class="menu-left">
        <div class="menu-left__icon">
          <img src="../../assets/icon/report.png" alt="">
        </div>
        <p class="menu-left__text">Báo cáo</p>
      </div>
      <div class="menu-left">
        <div class="menu-left__icon">
          <img src="../../assets/icon/dashboard.png" alt="">
        </div>
        <p class="menu-left__text">Mua hàng</p>
      </div>
      <div class="menu-left menu-active">
        <div class="menu-left__icon">
          <img src="../../assets/icon/dic-employee.png" alt="">
        </div>
        <p class="menu-left__text menu-active">Danh mục nhân viên</p>
      </div>
      <div class="menu-left">
        <div class="menu-left__icon">
          <img src="../../assets/icon/setting.png" alt="">
        </div>
        <p class="menu-left__text">Thiết lập hệ thống</p>
      </div>
    </div>
</template>

<script>
export default {
    name: 'TheMenu',
}
</script>

<style>

</style>