<template>
  <CModalHeader>
    <CModalTitle id="MenuModal" class="me-5">Edit Menu</CModalTitle>
    <CModalTitle class="error-message">{{ errorMessage }}</CModalTitle>
  </CModalHeader>
  <CModalBody>
    <CRow class="mb-3">
      <CCol
        xs="12"
        md="2"
        class="d-flex align-items-center justify-content-center justify-content-md-start mb-3 mb-md-0"
      >
        <label class="m-0 h5">ID</label>
      </CCol>
      <CCol xs="12" md="10">
        <label class="m-0 h5">{{ props.menuId }}</label>
      </CCol>
    </CRow>
    <CRow class="mb-3">
      <CCol
        xs="12"
        md="2"
        class="d-flex align-items-center justify-content-center justify-content-md-start mb-3 mb-md-0"
      >
        <label class="m-0 h5">Name</label>
      </CCol>
      <CCol xs="12" md="10">
        <input
          type="text"
          class="form-control"
          id="menuName"
          v-model="menuName"
          placeholder="請輸入菜單名稱"
          pattern="^[A-Za-z0-9\u4e00-\u9fa5]+$"
        />
      </CCol>
    </CRow>
    <select-list
      v-for="product in props.products"
      :key="product.id"
      ref="selectLists"
      :type="product.id"
      :type-name="product.name"
      :selected-id="product.selectedId"
      :ori-price="product.price"
      :ori-wattage="product.wattage"
      @update-info="onUpdateInfo"
      @product-selected="onProductSelected"
    ></select-list>
  </CModalBody>
  <CModalFooter>
    <CRow class="w-100">
      <CCol xs="6" class="d-flex align-items-center">
        <label class="h3 m-0 me-5">
          {{ totalPrice.toLocaleString('zh-TW', { style: 'currency', currency: 'TWD' }) }}
        </label>
        <label class="h3 m-0">{{ totalWattage }} 瓦 </label>
      </CCol>
      <CCol xs="6" class="text-end">
        <CButton color="primary" class="me-4" @click="onMenuUpdate"> 更新 </CButton>
        <CButton color="secondary" @click="modalClose"> 關閉 </CButton>
      </CCol>
    </CRow>
  </CModalFooter>
</template>

<script setup>
import { CModalHeader, CModalTitle, CModalBody, CModalFooter } from '@coreui/vue'
import { CRow, CCol, CButton } from '@coreui/vue'
import { ref } from 'vue'

import { toast } from 'vue3-toastify'
import 'vue3-toastify/dist/index.css'

import SelectList from '@/components/backend/hardware/menu/edit/SelectList.vue'

// 身份驗證
import { useIdentityStore } from '@/stores/identity.js'
const authStore = useIdentityStore()

// 從環境變數取得 API BASE URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

const selectLists = ref([])

let errorMessage = ref('')

// 宣告變更的產品陣列
let changedProducts = ref([])

let props = defineProps({
  menuId: Number,
  menuName: String,
  menuPrice: Number,
  menuWattage: Number,
  products: Array
})

let oriProducts = ref(props.products)

let totalPrice = ref(props.menuPrice)
let totalWattage = ref(props.menuWattage)

let menuName = ref(props.menuName)

const emit = defineEmits(['modal-close', 'menu-update'])

// 互動視窗 關閉事件
function modalClose() {
  emit('modal-close')
}

// 更新總價錢、瓦數
function onUpdateInfo(price, wattage, oriPrice, oriWattage) {
  // 價格加總
  totalPrice.value = totalPrice.value - oriPrice + price
  // 瓦數加總
  totalWattage.value = totalWattage.value - oriWattage + wattage
}

// 產品選擇事件
function onProductSelected(typeId, selectedId) {
  // 將修改的產品資訊緩存至陣列
  // 先檢查有沒有重複的 type
  let index = changedProducts.value.findIndex((item) => item.typeId === typeId)
  if (index !== -1) {
    // 有重複的 type
    changedProducts.value[index].selectedId = selectedId
  } else {
    changedProducts.value.push({
      typeId: typeId,
      selectedId: selectedId
    })
  }
}

// 取出實際變更的產品陣列
function getChangedProducts() {
  let finalList = []
  for (let i = 0; i < changedProducts.value.length; i++) {
    let index = oriProducts.value.findIndex((item) => item.id === changedProducts.value[i].typeId)
    if (index !== -1) {
      // 有重複的 type
      // 比對原始數據、變更後的數據
      if (oriProducts.value[index].selectedId !== changedProducts.value[i].selectedId) {
        finalList.push({
          menuId: props.menuId,
          oriProductId: oriProducts.value[index].selectedId,
          newProductId: changedProducts.value[i].selectedId
        })
      }
    } else {
      // 沒有重複的 type
      finalList.push({
        menuId: props.menuId,
        oriProductId: 0,
        newProductId: changedProducts.value[i].selectedId
      })
    }
  }
  return finalList
}

// 更新 MenuDetails
async function updateMenuDetails() {
  // 取出實際變更的產品陣列
  let finalList = getChangedProducts()

  for (let i = 0; i < finalList.length; i++) {
    let fetchState = true
    await fetch(`${apiUrl}/api/HardwareManage/UpdateMenuDetail`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${authStore.getToken}`
      },
      body: JSON.stringify({
        MenuId: finalList[i].menuId,
        OriProductId: finalList[i].oriProductId,
        NewProductId: finalList[i].newProductId
      })
    })
      .then((response) => {
        if (!response.ok) {
          return response.text().then((errorMessage) => {
            throw new Error(errorMessage)
          })
        }
        return response.text()
      })
      .then((data) => {
        console.log(data)
        if (i === finalList.length - 1) {
          updateMenuData()
        }
      })
      .catch((error) => {
        fetchState = false
        console.error('Error:', error.message)
      })

    if (!fetchState) {
      break
    }
  }
}

// 更新 MenuData
function updateMenuData() {
  fetch(`${apiUrl}/api/HardwareManage/UpdateMenu`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${authStore.getToken}`
    },
    body: JSON.stringify({
      MenuId: props.menuId,
      Name: menuName.value,
      TotalPrice: totalPrice.value,
      TotalWattage: totalWattage.value
    })
  })
    .then((response) => {
      if (!response.ok) {
        return response.text().then((errorMessage) => {
          throw new Error(errorMessage)
        })
      }
      return response.text()
    })
    .then((data) => {
      toast.success(data, {
        theme: 'dark',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.TOP_CENTER
      })
      emit('menu-update', props.menuId)
      emit('modal-close')
    })
    .catch((error) => {
      toast.error(error.message, {
        theme: 'dark',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.TOP_CENTER
      })
    })
}

// 提供前台變更資訊

// 菜單資料更新
function onMenuUpdate() {
  updateMenuDetails()
}
</script>

<style scoped>
.error-message {
  color: red;
}
</style>
