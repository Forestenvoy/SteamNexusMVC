<template>
  <div class="row">
    <div class="col-12 col-md-6 order-md-1 d-flex justify-content-center justify-content-md-start">
      <h2 id="SystemName" style="margin-top: 8px">權限管理系統</h2>
    </div>
    <div
      class="col-12 col-md-6 order-md-2 d-flex justify-content-center justify-content-md-end"
      id="SystemMenu"
    >
      <button class="btn btn-danger" id="createRoles">新增權限</button>
    </div>
  </div>

  <section class="section">
    <table id="RolesManageTable" class="display" style="width: 100%">
      <thead id="HardwareThead">
        <tr>
          <th data-priority="1">編號</th>
          <th>名稱</th>
          <th>刪除</th>
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
  datatable = new DataTable('#RolesManageTable', {
    ajax: {
      type: 'GET',
      url: `${apiUrl}/api/MemberManagement/RolesData`,
      dataSrc: function (json) {
        return json
      }
    },
    // column 定義
    columns: [
      { data: 'roleId', width: '10%', className: 'text-center' },
      { data: 'roleName', width: '10%', className: 'text-center' },
      {
        data: null,
        orderable: false,
        width: '10%',
        className: 'text-center',
        render: function (data, type, row, meta) {
          // 取得 roleId
          let RoleId = row.roleId
          let RoleName = row.roleName
          // 刪除按鈕
          let resetEle = `<button class="btn btn-danger del-btn" data-roleid="${row.roleId}" data-rolename="${RoleName}" data-bs-toggle="popover" data-bs-content="nothing"><i class="bi bi-trash3-fill"></i></button>`

          if (type === 'display') {
            return `${resetEle}`
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
