<template>
  <div class="row">
    <div class="col-12 col-md-6 order-md-1 d-flex justify-content-center justify-content-md-start">
      <h2 id="SystemName" style="margin-top: 8px;">公告管理系統</h2>
    </div>
    <div class="col-12 col-md-6 order-md-2 d-flex justify-content-center justify-content-md-end" id="SystemMenu">
    </div>
  </div>
  <section class="section">
    <table id="AdvertiseManageTable" class="display" style="width:100%">
      <thead>
        <tr>
          <th>ID</th>
          <th>來源</th>
          <th>Url</th>
          <th>圖片</th>
          <th>說明</th>
          <th>狀態</th>
        </tr>
      </thead>
    </table>
  </section>

  <!-- 刪除廣告的浮動式窗 -->
  <CModal alignment="center" :visible="DeleteAdModal" @close="() => {
    DeleteAdModal = false
  }
    " aria-labelledby="DeleteAdModal">
    <CModalHeader>
      <CModalTitle id="DeleteAdModal">刪除公告</CModalTitle>
    </CModalHeader>
    <CModalBody>
      <div class="container">
        <div class="row">
          <span class="col-3 text-end">ID:</span>
          <span class="col-9">{{ deleteId }}</span><br /><br />
          <span class="col-3 text-end">來源:</span>
          <span class="col-9">{{ deleteTitle }}</span><br /><br />
          <span class="col-3 text-end">Url:</span>
          <span class="col-9" style="word-wrap: break-word;">{{ deleteUrl }}</span><br /><br /><br />
          <span class="col-3 text-end">圖片:</span>
          <div class="col-9">
            <img class="" v-bind:src="deleteImage" style="width:80%" /><br /><br />
          </div>
          <span class="col-3 text-end">說明:</span>
          <span class="col-9">{{ deleteDescription }}</span>
        </div>
      </div>
    </CModalBody>
    <CModalFooter>
      <CButton color="secondary" @click="() => {
        DeleteAdModal = false
      }
        ">
        取消
      </CButton>
      <CButton color="primary" @click="DeleteAdBtn">刪除</CButton>
    </CModalFooter>
  </CModal>

  <!-- 新增廣告的浮動式窗 -->
  <Form @submit.prevent="CreateAdBtn">
    <CModal alignment="center" :visible="CreateAdModal" @close="() => {
      CreateAdModal = false
      createTitle = ''
      createAdUrl = ''
      createImageUrl = ''
      createDescription = ''
      showImageUrl = '/img/noImage.png'
    }
      " aria-labelledby="CreateAdModal">
      <CModalHeader>
        <CModalTitle id="CreateAdModal">新增公告</CModalTitle>
      </CModalHeader>
      <CModalBody>
        <div class="mb-3">
          <label for="createTitle">來源</label>
          <Field id="createTitle" name="createTitle" v-model="createTitle" class="form-control"
            rules="required|max:50" />
          <ErrorMessage class="text-danger" name="createTitle" />
        </div>
        <div class="mb-3">
          <label for="createAdUrl">網址</label>
          <Field id="createAdUrl" name="createAdUrl" v-model="createAdUrl" class="form-control" rules="required|url"
            autocomplete="off" />
          <ErrorMessage class="text-danger" name="createAdUrl" />
        </div>
        <div class="mb-3">
          <label for="createImageUrl">圖片網址</label>
          <Field id="createImageUrl" name="createImageUrl" v-model="createImageUrl" @change="ImageChange"
            class="form-control mb-3" rules="required|url" autocomplete="off" />
          <img :src="showImageUrl" alt="圖片預覽" style="max-width: 100%; height: auto;">
          <ErrorMessage class="text-danger" name="createImageUrl" />
        </div>
        <div class="mb-3">
          <label for="createDescription">說明</label>
          <textarea id="createDescription" v-model="createDescription" class="form-control" maxlength="100"></textarea>
        </div>
        <!-- <div class="text-center">
          <CButton color="secondary" @click="closeModal">取消</CButton>
          <CButton type="submit" color="primary">新增</CButton>
        </div> -->
      </CModalBody>
      <CModalFooter>
        <CButton color="secondary" @click="() => {
          CreateAdModal = false
          createTitle = ''
          createAdUrl = ''
          createImageUrl = ''
          createDescription = ''
          showImageUrl = '/img/noImage.png'
        }
          ">
          取消
        </CButton>
        <CButton color="primary" @click="CreateAdBtn">新增</CButton>
      </CModalFooter>
    </CModal>
  </Form>

  <!-- 修改廣告的浮動式窗 -->
  <Form @submit.prevent="EditAdSaveBtn">
    <CModal alignment="center" :visible="EditAdModal" @close="() => {
      EditAdModal = false
    }
      " aria-labelledby="EditAdModal">
      <CModalHeader>
        <CModalTitle id="EditAdModal">公告編輯</CModalTitle>
      </CModalHeader>
      <CModalBody>
        <div class="mb-3">
          <label for="editTitle">來源</label>
          <Field id="editTitle" name="editTitle" v-model="editTitle" class="form-control" rules="required|max:50" />
          <ErrorMessage class="text-danger" name="editTitle" />
        </div>
        <div class="mb-3">
          <label for="editAdUrl">網址</label>
          <Field id="editAdUrl" name="editAdUrl" v-model="editAdUrl" class="form-control" rules="required|url" />
          <ErrorMessage class="text-danger" name="editAdUrl" />
        </div>
        <div class="mb-3">
          <label for="editImageUrl">圖片網址</label>
          <Field id="editImageUrl" name="editImageUrl" v-model="editImageUrl" class="form-control mb-3"
            rules="required|url" @change="editImageChange" />
          <img :src="showEditImageUrl" alt="圖片預覽" style="max-width: 100%; height: auto;">
          <ErrorMessage class="text-danger" name="editImageUrl" />
        </div>
        <div class="mb-3">
          <label for="editDescription">說明</label>
          <textarea id="editDescription" v-model="editDescription" class="form-control" maxlength="100"></textarea>
        </div>
        <!-- <div class="text-center">
          <CButton color="secondary" @click="closeModal">取消</CButton>
          <CButton type="submit" color="primary">新增</CButton>
        </div> -->
      </CModalBody>
      <CModalFooter>
        <CButton color="secondary" @click="() => {
          EditAdModal = false
        }
          ">
          取消
        </CButton>
        <CButton color="primary" @click="EditAdSaveBtn">儲存</CButton>
      </CModalFooter>
    </CModal>
  </Form>
</template>
<script setup>
// 核心模組 import
import $ from 'jquery'
import dataTableLanguage from '@/components/backend/hardware/dataTableLanguage.js'
import DataTable from 'datatables.net-dt'
import 'datatables.net-fixedheader-dt'
import 'datatables.net-buttons-dt'
import 'datatables.net-responsive-dt'


import '@/assets/css/dataTables.datatables.min.css';
import '@/assets/css/fixedHeader.dataTables.min.css';
import '@/assets/css/buttons.dataTables.min.css';
import '@/assets/css/responsive.dataTables.min.css';

//Form驗證製作
import { defineRule, Form, Field, ErrorMessage, configure } from 'vee-validate';
import { required, url, max } from '@vee-validate/rules';
import { localize } from '@vee-validate/i18n';
import validateErrorMsg from '@/components/backend/Ad/validateErrorMsg.json';

// 特殊吐司
import { toast } from 'vue3-toastify'
import 'vue3-toastify/dist/index.css'

import { ref, onMounted } from 'vue'
import { CModal, CModalHeader, CModalTitle, CModalBody, CModalFooter, CButton } from '@coreui/vue'

import { onBeforeRouteLeave } from 'vue-router'

defineRule('required', required)
defineRule('url', url)
defineRule('max', max)

// 從環境變數取得 API BASE URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

var dataTable = null
let DeleteAdModal = ref(false)
let deleteId = ref(0)
let deleteTitle = ref('')
let deleteUrl = ref('')
let deleteImage = ref('')
let deleteDescription = ref('')
let CreateAdModal = ref(false)
let createTitle = ref('')
let createAdUrl = ref('')
let createImageUrl = ref('')
let createDescription = ref('')
let showImageUrl = ref('/img/noImage.png')
let EditAdModal = ref(false)
let editId = ref(0)
let editTitle = ref('')
let editAdUrl = ref('')
let editImageUrl = ref('')
let editDescription = ref('')
let showEditImageUrl = ref('')


// 產生表格
function fetchDatatable() {
  fetch(`${apiUrl}/api/Advertisement/AdvertiseData`, {
    method: 'GET',
  })
    .then(response => {
      if (!response.ok) {
        throw new Error('Network response was not ok');
      }
      return response.json();
    })
    .then(jsonData => {
      // 清空表格
      dataTable.clear().draw();
      // 添加新的資料
      dataTable.rows.add(jsonData).draw();
    })
    .catch(error => {
      console.error('Fetch error:', error);
    });
};

// 刪除廣告
function DeleteAdBtn() {
  // console.log(deleteId.value)
  fetch(`${apiUrl}/api/Advertisement/DeleteAd`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      id: deleteId.value
    })
  })
    .then(response => {
      if (!response.ok) {
        return response.text().then(errorMessage => {
          throw new Error(errorMessage);
        });
      }
      return response.text();
    })
    .then(result => {
      // 成功處理回應
      toast.success(result, {
        theme: 'dark',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.TOP_CENTER
      });
      DeleteAdModal.value = false;
      fetchDatatable();
    })
    .catch(error => {
      // 處理錯誤
      toast.error(error.message, {
        theme: 'dark',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.TOP_CENTER
      });
    });
}

//設定驗證功能之{field}名稱(紅色的字的名稱預設為英文)
configure({
  generateMessage: localize('zh_TW', {
    names: {
      createTitle: '標題',
      createImageUrl: '圖片網址',
      createAdUrl: '廣告網址',
      editTitle: '標題',
      editImageUrl: '圖片網址',
      editAdUrl: '廣告網址'
    },
    messages: validateErrorMsg.messages
  })
})

//新增照片更新事件
function ImageChange() {
  if (createImageUrl.value != '') {
    var img = new Image()
    img.src = createImageUrl.value
    img.onload = function () {
      showImageUrl.value = createImageUrl.value
    }
    img.onerror = function () {
      // 如果網址有效但沒有圖片，顯示預設圖片
      showImageUrl.value = '/img/noImage.png'
    }
  } else {
    showImageUrl.value = '/img/noImage.png'
  }
}

// 新增廣告
function CreateAdBtn() {
  fetch(`${apiUrl}/api/Advertisement/CreateAd`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      title: createTitle.value,
      url: createAdUrl.value,
      imagePath: createImageUrl.value,
      description: createDescription.value
    })
  })
    .then(response => {
      if (!response.ok) {
        return response.text().then(errorMessage => {
          throw new Error(errorMessage);
        });
      }
      return response.text();
    })
    .then(result => {
      // 成功處理回應
      toast.success(result, {
        theme: 'dark',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.TOP_CENTER
      })
      CreateAdModal.value = false;
      createTitle.value = '';
      createAdUrl.value = '';
      createImageUrl.value = '';
      showImageUrl.value = '/img/noImage.png';
      createDescription.value = '';
      fetchDatatable();
    })
    .catch(error => {
      // 處理錯誤
      toast.error(error.message, {
        theme: 'dark',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.TOP_CENTER
      });
    });
}

// 編輯照片更新事件
function editImageChange() {
  if (editImageUrl.value != '') {
    var img = new Image()
    img.src = editImageUrl.value
    img.onload = function () {
      showEditImageUrl.value = editImageUrl.value
    }
    img.onerror = function () {
      // 如果網址有效但沒有圖片，顯示預設圖片
      showEditImageUrl.value = '/img/noImage.png'
    }
  } else {
    showEditImageUrl.value = '/img/noImage.png'
  }
}

// 編輯廣告儲存
function EditAdSaveBtn() {
  fetch(`${apiUrl}/api/Advertisement/EditAd`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      id: editId.value,
      title: editTitle.value,
      url: editAdUrl.value,
      imagePath: editImageUrl.value,
      description: editDescription.value
    })
  })
    .then(response => {
      if (!response.ok) {
        return response.text().then(errorMessage => {
          throw new Error(errorMessage);
        });
      }
      return response.text();
    })
    .then(result => {
      // 成功處理回應
      toast.success(result, {
        theme: 'dark',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.TOP_CENTER
      })
      EditAdModal.value = false;
      fetchDatatable();
    })
    .catch(error => {
      // 處理錯誤
      toast.error(error.message, {
        theme: 'dark',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.TOP_CENTER
      });
    });
}

onMounted(() => {
  //重設排序
  $.fn.dataTable.ext.order['dom-radio'] = function(settings, col) {
    return this.api().column(col, {order: 'index'}).nodes().map(function(td,) {
      return $('input[type="radio"]:checked', td).val() === "true" ? 0 : 1;
    });
  };
  dataTable = new DataTable('#AdvertiseManageTable', {
    columns: [
      { "data": "advertisementId", "width": "5%" },
      { "data": "title", "width": "8%", },
      { "data": "url", "width": "25%", "orderable": false, },
      {
        "data": "imagePath", "width": "15%",
        "className": "text-center",
        "orderable": false,
        "render": function (data) {
          return `<img src="${data}" alt="Image" style="width:80%" />`;
        }, responsivePriority: 1
      },
      { "data": "description", "width": "17%", "orderable": false, },
      {
        "data": "isShow",
        "width": "15%",
        "className": "text-center",
        "orderable": true, // 確保該欄位可以排序
        "orderDataType": "dom-radio",
        "render": function (data, type, row) {
          // data: 欄位的資料值
          // type: render 的呼叫類型，例如 'display', 'filter', 'sort', 'type'
          // row: 包含整列資料的物件
          let AdId = row.advertisementId;
          const Show = data ? 'checked' : '';
          const UnShow = data ? '' : 'checked';
          let showEle = `<input type="radio" class="radio-isShow" name = "${AdId}" value = "true" data-AdId="${AdId}" id = "${AdId}Show" ${Show}><label for= "${AdId}Show" style="margin-right: 10px">上架</label>`;
          let unShowEle = `<input type="radio" class="radio-isShow" name="${AdId}" value="false" data-AdId="${AdId}" id="${AdId}UnShow" ${UnShow}><label for="${AdId}UnShow">下架</label>`;
          return `${showEle}${unShowEle}`;
        }, responsivePriority: 1,
      },
      {
        "data": null,
        "orderable": false,
        "width": "15%",
        "className": "text-center",
        "orderDataType": "dom-radio",
        // 按鈕 自定義
        "render": function (data, type, row) {
          // 取得 productId
          let AdId = row.advertisementId;
          // 編輯按鈕
          let editEle = `<button class="btn btn-primary edit-btn" data-AdId="${AdId}"><i class="bi bi-pencil-square"></i></button>`;
          // 刪除按鈕
          let deletEle = `<button class="btn btn-danger del-btn" data-AdId="${AdId}"><i class="bi bi-trash3"></i></button>`;
          if (type === 'display') {
            return `${editEle}${deletEle}`;
          }
          return data;
        }, responsivePriority: 2
      }
    ],
    fixedHeader: {
      // 固定 header
      header: true
    },
    // 響應式設計
    responsive: true,
    // 語言設定
    language: dataTableLanguage,
    // 預設排序 ~ 根據第一個欄位 升冪排序
    order: [[0, 'asc']],
    // 自動寬度 開啟
    autoWidth: true,
    layout: {
      topMiddile: {
        //Create按鈕建立
        buttons: [
          {
            text: '新增公告',
            className: 'btn btn-primary',
            action: function () {
              CreateAdModal.value = true;
            }
          }
        ]
      },
    },
  });

  fetchDatatable();

  // 監聽上下架按鈕的 change 事件
  $(document).on('change', '.radio-isShow', function () {
    // alert("change");
    const adId = $(this).data('adid');
    const isShow = $(this).val() === 'true';
    // // alert(`change ${adId} ${isShow}`);
    fetch(`${apiUrl}/api/Advertisement/UpdateIsShow?adId=${adId}&isShow=${isShow}`, {
      method: 'PUT',
    }).then(response => {
      if (!response.ok) {
        return response.text().then((errorMessage) => {
          throw new Error(errorMessage)
        })
      }
      return response.text()
    }).then(result => {
      // alert(result); // 處理成功回應的操作
      toast.success(result, {
        theme: 'dark',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.TOP_CENTER
      })
    }).catch(error => {
      // alert(error); // 處理錯誤
      toast.error(error.message, {
        theme: 'dark',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.TOP_CENTER
      })
    });
  });

  //主畫面刪除按鈕
  $(document).on('click', '.del-btn', function () {
    // alert("del-btn");
    const adId = $(this).data('adid');
    fetch(`${apiUrl}/api/Advertisement/GetOneAdData?AdId=${adId}`, {
      method: 'GET',
    }).then(response => {
      if (!response.ok) {
        return response.text().then((errorMessage) => {
          throw new Error(errorMessage)
        })
      }
      return response.json()
    }).then(result => {
      deleteId.value = result.id;
      deleteTitle.value = result.title;
      deleteUrl.value = result.url;
      deleteImage.value = result.image;
      deleteDescription.value = result.description;

      DeleteAdModal.value = true;
    }).catch(error => {
      toast.error(error.message, {
        theme: 'dark',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.TOP_CENTER
      })
    });
  })

  // 主畫面編輯按鈕
  $(document).on('click', '.edit-btn', function () {
    // alert("edit-btn");
    const adId = $(this).data('adid');
    fetch(`${apiUrl}/api/Advertisement/GetOneAdData?AdId=${adId}`, {
      method: 'GET',
    }).then(response => {
      if (!response.ok) {
        return response.text().then((errorMessage) => {
          throw new Error(errorMessage)
        })
      }
      return response.json()
    }).then(result => {
      editId.value = result.id;
      editTitle.value = result.title;
      editAdUrl.value = result.url;
      editImageUrl.value = result.image;
      showEditImageUrl.value = result.image;
      editDescription.value = result.description;

      EditAdModal.value = true;
    }).catch(error => {
      toast.error(error.message, {
        theme: 'dark',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.TOP_CENTER
      })
    });
  })

})

// 路由離開時觸發
onBeforeRouteLeave(() => {
  // 銷毀 DataTable
  dataTable.clear().draw()
  dataTable.destroy()
  dataTable = null
  // 事件監聽器移除
  $(document).off('change', '.radio-isShow')
  $(document).off('click', '.del-btn')
  $(document).off('click', '.edit-btn')
  // $(document).off('click', '.Enter-btn')
  // $(document).off('click', '.Cancel-btn')
  // $(document).off('click', '.Edit-btn')
})
</script>


<style>
/***** datatables 自訂樣式 *****/

.dt-end .dt-search {
  text-align: center !important;
  /* 將搜索框的文字內容設置為居中對齊 */
}

.dt-middle {
  text-align: end !important;
}

body {
  table-layout: fixed;
  /* 將表格的列寬設置為固定值，以確保列寬不會根據內容而自動調整 */
}
</style>
