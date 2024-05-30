<template>
  <CModalHeader>
    <CModalTitle id="MenuModal" class="me-5">Create Menu</CModalTitle>
    <CModalTitle class="error-message">{{ errorMessage }}</CModalTitle>
  </CModalHeader>
  <CModalBody>
    <CRow class="mb-3">
      <CCol
        xs="12"
        md="2"
        class="d-flex align-items-center justify-content-center justify-content-md-start mb-3 mb-md-0"
      >
        <label class="m-0 h5">Menu Name</label>
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
      v-for="product in Products"
      :key="product.id"
      ref="selectLists"
      :type="product.id"
      :type-name="product.name"
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
        <CButton color="primary" class="me-4" @click="onMenuCreate"> 建立 </CButton>
        <CButton color="secondary" @click="modalClose"> 關閉 </CButton>
      </CCol>
    </CRow>
  </CModalFooter>
</template>

<script setup>
import { CModalHeader, CModalTitle, CModalBody, CModalFooter } from '@coreui/vue'
import { CRow, CCol, CButton } from '@coreui/vue'
import { ref } from 'vue'

import SelectList from '@/components/backend/hardware/menu/create/SelectList.vue'

// 身份驗證
import { useIdentityStore } from '@/stores/identity.js'
const authStore = useIdentityStore()

// 從環境變數取得 API BASE URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

const selectLists = ref([])
let menuName = ref('')
let totalPrice = ref(0)
let totalWattage = ref(0)
let errorMessage = ref('')

const emit = defineEmits(['modal-close', 'create-result'])

// 宣告產品分類清單
let Products = [
  {
    id: 10000,
    name: 'CPU'
  },
  {
    id: 10001,
    name: 'GPU'
  },
  {
    id: 10002,
    name: 'RAM'
  },
  {
    id: 10003,
    name: 'MotherBoard'
  },
  {
    id: 10004,
    name: 'SSD'
  },
  {
    id: 10005,
    name: 'HDD'
  },
  {
    id: 10006,
    name: 'AirCooler'
  },
  {
    id: 10007,
    name: 'LiquidCooler'
  },
  {
    id: 10008,
    name: 'CASE'
  },
  {
    id: 10009,
    name: 'PSU'
  },
  {
    id: 10010,
    name: 'OS'
  }
]

// 互動視窗 關閉事件
function modalClose() {
  emit('modal-close')
}

// 產品選擇事件
function onProductSelected(price, wattage, oriPrice, oriWattage) {
  // 價格加總
  totalPrice.value = totalPrice.value - oriPrice + price
  // 瓦數加總
  totalWattage.value = totalWattage.value - oriWattage + wattage
}

// 建立菜單至資料庫
function createMenuData() {
  fetch(`${apiUrl}/api/HardwareManage/CreateMenu`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${authStore.getToken}`
    },
    body: JSON.stringify({
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
      createMenuDetails(data)
    })
    .catch((error) => {
      console.error('Error:', error.message)
    })
}

// MenuDetails 建立至資料庫
function createMenuDetails(id) {
  // 迭代選擇清單
  for (let i = 0; i < selectLists.value.length; i++) {
    const selectList = selectLists.value[i]
    const productInformationId = Number(selectList.$el.querySelector('select').value)
    // 如果沒有選擇產品則跳過
    if (productInformationId === 0) {
      continue
    }
    const menuId = Number(id)
    console.log(productInformationId, menuId)
    console.log(typeof productInformationId, typeof menuId)
    fetch(`${apiUrl}/api/HardwareManage/CreateMenuDetail`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${authStore.getToken}`
      },
      body: JSON.stringify({
        MenuId: menuId,
        ProductInformationId: productInformationId
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
        // 最後一筆資料建立完成後關閉視窗
        if (i === selectLists.value.length - 1) {
          emit('create-result', `${menuName.value} 建立成功`, true, menuId)
          emit('modal-close')
        }
      })
      .catch((error) => {
        console.error('Error:', error.message)
      })
  }
}

// 菜單建立事件
function onMenuCreate() {
  errorMessage.value = ''
  // 驗證
  if (menuName.value === '') {
    errorMessage.value = '請輸入菜單名稱'
    return
  }

  let SSDValue = 0
  let HDDValue = 0
  let AirCoolerValue = 0
  let LiquidCoolerValue = 0
  let isLoopFinish = true

  for (let i = 0; i < selectLists.value.length; i++) {
    // 獲取 select-list 元素
    const selectList = selectLists.value[i]
    // 獲取元件裡的 select 元素
    const selectElement = selectList.$el.querySelector('select') // 获取 select-list 组件内部的 select 元素
    const selectedValue = selectElement.value // 获取 select 元素的值
    const selectName = selectElement.getAttribute('name')

    if (
      selectName !== 'SSD' &&
      selectName !== 'HDD' &&
      selectName !== 'AirCooler' &&
      selectName !== 'LiquidCooler' &&
      selectedValue === '0'
    ) {
      errorMessage.value = `請選擇 ${selectName}`
      isLoopFinish = false
      break
    }

    if (selectName === 'SSD') {
      SSDValue = Number(selectedValue)
    }
    if (selectName === 'HDD') {
      HDDValue = Number(selectedValue)
    }

    if (selectName === 'AirCooler') {
      AirCoolerValue = Number(selectedValue)
    }

    if (selectName === 'LiquidCooler') {
      LiquidCoolerValue = Number(selectedValue)
    }

    errorMessage.value = ''
  }

  if (SSDValue === 0 && HDDValue === 0 && isLoopFinish) {
    errorMessage.value = 'SSD 和 HDD 請至少選擇一項'
    return
  }

  if (AirCoolerValue === 0 && LiquidCoolerValue === 0 && isLoopFinish) {
    errorMessage.value = 'AirCooler 和 LiquidCooler 請至少選擇一項'
    return
  }

  // 驗證錯誤 中斷事件
  if (errorMessage.value !== '') {
    return
  }

  // 建立菜單
  createMenuData()
}
</script>

<style scoped>
.error-message {
  color: red;
}
</style>
