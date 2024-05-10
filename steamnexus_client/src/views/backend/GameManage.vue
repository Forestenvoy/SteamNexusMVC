<template>
  <h1>全部遊戲<img src="/public/img/loading-block-white.gif" style="margin:20px;width:30px;height:30px;" id="GameIndexLoading" ></h1>
  <DataTable id="yourDataTableId" :columns="columns" :ajax="ajax" :options="options" :layout="layout"  class="display">
      <thead>
          <tr>
            <th></th>
            <th>代碼</th>
            <th>名稱</th>
            <th>原始價格</th>
            <th>現在價格</th> 
            <th>遊戲分級</th>
            <th>評價</th>
            <th>評價數量</th>
            <th>發行商</th>
          </tr>
      </thead>
  </DataTable>
</template>

<script setup >
import $ from 'jquery'
import DataTable from 'datatables.net-vue3';
import DataTablesCore from 'datatables.net';
import 'datatables.net-buttons';
import 'datatables.net-buttons/js/buttons.html5';
import 'datatables.net-responsive';
import 'datatables.net-select';

DataTable.use(DataTablesCore);

const columns = [
  {data: 'imagePath',title: '',"render": function (data, type, row) {
    return '<img src="' + data + '" alt="圖片" style="width:150px">';
}, responsivePriority: 1},
  {data: 'appId',title: '代碼',},
  {data: 'name',title: '名稱',},
  {data: 'originalPrice',title: '原始價格',},
  {data: 'currentPrice',title: '現在價格',},
  {data: 'ageRating',title: '遊戲分級',},
  {data: 'comment',title: '評價',},
  {data: 'commentNum',title: '評價數量',},
  {data: 'publisher',title: '發行商',},
];

const options = {
  responsive: true,
  select: true,
  ordering: true,
  paging: true,
};

// 從環境變數取得 API BASE URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL
//拿取datatable的資料
const ajax = {
        url: `${apiUrl}/api/GamesManagement/IndexJson`,
        type: 'GET',
        dataType: 'json',
        success: function (data) {
      if (data && data.length > 0) {
        console.log(data);

        // 將取得的資料添加至DataTable
        const table = $('#yourDataTableId').DataTable(); // 取得DataTable的實例
        table.clear().rows.add(data).draw(); // 清空表格並添加資料後重新繪製
      }
    },
    error: function (error) {
      console.log(error);
    },
    };

</script>

<style>



</style>
