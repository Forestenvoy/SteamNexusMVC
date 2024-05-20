import $ from 'jquery'
import dataTableLanguage from '@/components/backend/hardware/dataTableLanguage.js'

// 硬體產品的 dataTable 參數配置
export const dataTableConfig = {
  // column 定義
  columns: [
    { data: 'productId', width: '5%', className: 'text-center' },
    { data: 'productName', width: '34%' },
    { data: 'specification', width: '29%' },
    { data: 'componentClassificationName', visible: false },
    { data: 'price', width: '8%', className: 'text-center' },
    {
      data: 'wattage',
      width: '8%',
      className: 'text-center',
      // 瓦數 ~ 文字輸入方塊
      render: function (data, type, row) {
        // 此處 type 為 'sort' 表示 DataTables 正在進行排序
        if (type === 'sort') {
          // 返回元素的值作為排序依據
          return data
        }
        // 取得 productId
        let productId = row.productId
        // input 欄位 ~ 可編輯
        let inputEle = `<input type="text" class="${productId}_watt defaultCellType" value="${data}" style="width: 50px; text-align: center;"  disabled>`
        return inputEle
      },
      // 將此列的數據類型設置為數字 ~ 排序才會正常運作
      type: 'num'
    },
    {
      data: 'recommend',
      width: '8%',
      className: 'text-center',
      // 推薦 ~ 下拉式選單
      render: function (data, type, row) {
        // 此處 type 為 'sort' 表示 DataTables 正在進行排序
        if (type === 'sort') {
          // 返回元素的值作為排序依據
          return data
        }
        // 取得 productId
        let productId = row.productId
        const isRec1 = data == 0 ? 'selected' : ''
        const isRec2 = data == 1 ? 'selected' : ''
        const isRec3 = data == 2 ? 'selected' : ''
        const isRec4 = data == 3 ? 'selected' : ''
        const ele1 = `<option value = "0" ${isRec1}>無</option>`
        const ele2 = `<option value = "1" ${isRec2}>基本</option>`
        const ele3 = `<option value = "2" ${isRec3}>優良</option>`
        const ele4 = `<option value = "3" ${isRec4}>完美</option>`
        // select 欄位 ~ 可編輯
        let selectEle = `<select class="${productId}_recommend defaultCellType" disabled>${ele1}${ele2}${ele3}${ele4}</select>`
        return selectEle
      },
      // 將此列的數據類型設置為數字 ~ 排序才會正常運作
      type: 'num'
    },
    {
      data: null,
      orderable: false,
      width: '8%',
      // 按鈕 自定義
      render: function (data, type, row) {
        // 取得 productId
        let productId = row.productId
        // 編輯按鈕
        let editEle = `<button class="${productId}_edit btn Edit-btn d-flex justify-content-center align-items-center" style="width:30px;height:30px;"><i class="fa-solid fa-pen-to-square"></i></button>`
        let div = `<div class="d-flex justify-content-between" id="${productId}_div">${editEle}</div>`
        if (type === 'display') {
          return `${div}`
        }
        return data
      },
      type: 'html'
    }
  ],
  // 標頭固定
  // fixedHeader: {
  //   // 固定 header
  //   header: true,
  //   // 使用導覽欄的高度作為偏移量
  //   headerOffset: 64
  // },
  // 響應式設計
  responsive: true,
  // 語言設定
  language: dataTableLanguage,
  // 預設排序 ~ 根據第一個欄位 升冪排序
  order: [[1, 'asc']],
  // 行分組
  rowGroup: {
    dataSrc: 'componentClassificationName'
  },
  // 資料載入中 gif
  processing: true,
  // 自動寬度 關閉
  autoWidth: true,
  // 資料行樣式
  createdRow: function (row, data) {
    // 加上 ID
    $(row).attr('id', data.productId + '_row')
    // 為特定行加入 class ==> 不會影響 th
    $(row).find('td:eq(6)').addClass('d-flex')
    $(row).find('td:eq(6)').addClass('justify-content-center')
  }
  // // 按鈕建立
  // layout: {
  //   topMiddle: {
  //     buttons: [
  //       {
  //         text: '重新整理',
  //         // 按鈕點擊事件
  //         action: function () {
  //           // isrefresh = true
  //           // 重新整理
  //           // getdatatableData()
  //         }
  //       },
  //       {
  //         text: '單一零件更新',
  //         // 按鈕點擊事件
  //         action: function () {
  //           // 單一零件更新
  //           // UpdateOneHardware()
  //         }
  //       },
  //       {
  //         text: '全零件更新',
  //         // 按鈕點擊事件
  //         action: function () {
  //           // 全零件更新
  //           // UpdateAllHardware()
  //         }
  //       }
  //     ]
  //   }
  // }
}
