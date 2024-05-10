<template>
  <div class="row">
    <div class="col-12 col-md-6 order-md-1 d-flex justify-content-center justify-content-md-start">
      <h2 id="SystemName" style="margin-top: 8px">會員管理系統</h2>
    </div>
    <div
      class="col-12 col-md-6 order-md-2 d-flex justify-content-center justify-content-md-end"
      id="SystemMenu"
    >
      <button class="btn btn-danger" id="createMember">新增會員</button>
    </div>
  </div>
  <section class="section">
    <table id="MemberManageTable" class="display" style="width: 100%">
      <thead id="HardwareThead">
        <tr>
          <th data-priority="1">姓名</th>
          <th>信箱</th>
          <th>性別</th>
          <th>電話</th>
          <th>生日</th>
          <th>個人照</th>
          <th>權限</th>
          <th>操作</th>
        </tr>
      </thead>
    </table>
  </section>
</template>

<script setup>
// 核心模組 import
import $ from 'jquery'
import DataTable from 'datatables.net-dt'
import 'datatables.net-fixedheader-dt'
import 'datatables.net-buttons-dt'
import 'datatables.net-responsive-dt'
import { onMounted } from 'vue'

// 從環境變數取得 API BASE URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

var datatable = null

onMounted(() => {
  // 初始化 Datatables
  datatable = new DataTable('#MemberManageTable', {
    ajax: {
      type: 'GET',
      url: `${apiUrl}/api/MemberManagement/GetUsersWithRoles`,
      dataSrc: function (json) {
        return json
      }
    },
    // column 定義
    columns: [
      // { "data": "userId", "width": "10%", "className": "text-center" },
      { data: 'name', width: '10%', className: 'text-center' },
      { data: 'email', width: '10%', className: 'text-center' },
      { data: 'gender', width: '10%', className: 'text-center' },
      { data: 'phone', width: '10%', className: 'text-center' },
      {
        data: 'birthday',
        width: '10%',
        className: 'text-center',
        render: function (data, type, row) {
          if (type === 'display' && data) {
            let date = new Date(data)
            return `${date.getFullYear()}-${('0' + (date.getMonth() + 1)).slice(-2)}-${('0' + date.getDate()).slice(-2)}`
          }
          return data
        }
      },
      {
        data: 'photo',
        width: '10%',
        className: 'text-center',
        render: function (data, type, row) {
          return data
            ? `<img src="/images/headshots/${data}" alt="Image" style="width: 100px; height: 100px;" />`
            : 'No Image'
        }
      },
      { data: 'roleName', width: '10%', className: 'text-center' },
      {
        data: null,
        orderable: false,
        width: '10%',
        className: 'text-center',
        render: function (data, type, row, meta) {
          // 取得 UserId
          let UserId = row.userId
          let Name = row.name
          let Email = row.email
          let Gender = row.gender
          let Phone = row.phone
          let Birthday = row.birthday
          let Photo = row.photo
          let RoleName = row.roleName
          // 編輯按鈕
          let enterEle = `<button class="btn btn-danger" data-userid="${UserId}" data-name="${Name}" data-email="${Email}"  data-gender="${Gender}" data-phone="${Phone}" data-birthday="${Birthday}" data-photo="${Photo}" data-rolename="${RoleName}" id="edit_btn" data-bs-toggle="popover" data-bs-content="nothing"><i class="bi bi-pencil-square"></i></button>`
          // 刪除按鈕
          // let resetEle = '<button class="btn btn-danger" data-userId="' + UserId + '"  data-name="' + Name + '" data-email="' + Email + '" id="delete_btn" data-bs-toggle="popover" data-bs-content="nothing"><i class="bi bi-trash3-fill"></i></button>';
          let resetEle = `<button class="btn btn-danger" data-userid="${UserId}" data-name="${Name}" data-email="${Email}"  data-gender="${Gender}" data-phone="${Phone}" data-birthday="${Birthday}" data-photo="${Photo}" data-rolename="${RoleName}" " id="delete_btn" data-bs-toggle="popover" data-bs-content="nothing"><i class="bi bi-trash3-fill"></i></button>`

          if (type === 'display') {
            return `${enterEle}&nbsp;${resetEle}`
          }
          return data
        },
        responsivePriority: 1
      }
    ],
    // 標頭固定
    fixedHeader: {
      // 固定 header
      header: true,
      // 使用導覽欄的高度作為偏移量
      headerOffset: $('#pageNav').outerHeight()
    },
    // 響應式設計
    responsive: true,
    // 語言設定
    language: {
      url: '//cdn.datatables.net/plug-ins/2.0.3/i18n/zh-HANT.json'
    },
    // 預設排序 ~ 根據第一個欄位 升冪排序
    order: [[1, 'asc']],
    // 自動寬度 關閉
    autoWidth: true
  })
})
</script>

<style>
/* DataTables */
@import 'datatables.net-dt/css/dataTables.datatables.min.css';
@import 'datatables.net-fixedheader-dt/css/fixedHeader.dataTables.min.css';
@import 'datatables.net-buttons-dt/css/buttons.dataTables.min.css';
@import 'datatables.net-responsive-dt/css/responsive.dataTables.min.css';
</style>
