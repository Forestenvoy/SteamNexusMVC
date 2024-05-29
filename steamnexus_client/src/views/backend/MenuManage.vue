<template>
  <section class="mt-1 mt-lg-3 mx-3">
    <!-- 菜單管理 UI -->
    <CRow>
      <CCol sm="12" md="6" lg="5">
        <h2>Menu Manage System</h2>
        <div>
          <label style="color: grey !important">
            新增、編輯、刪除菜單，且可以自由上下架，並偵測異常菜單
          </label>
        </div>
      </CCol>
      <CCol sm="12" md="6" lg="4" class="d-flex justify-content-center align-items-end">
        <CFormInput
          type="text"
          placeholder="請輸入菜單關鍵字"
          class="me-2"
          style="width: 250px; height: 40px"
        />
        <!-- 搜尋按鈕 -->
        <CButton color="light" class="text-center p-0" style="width: 40px; height: 40px">
          <CIcon icon="cilSearch" style="width: 24px; height: 24px" />
        </CButton>
      </CCol>
      <CCol sm="12" md="6" lg="3" class="d-flex justify-content-end align-items-end">
        <CButton color="light" shape="rounded-0" class="border border-black" @click="Menu_Create()"
          >新增菜單</CButton
        >
      </CCol>
    </CRow>
    <CRow>
      <transition-group name="list">
        <menu-card
          v-for="menu in menuLists"
          :key="menu.id"
          :menuId="menu.id"
          :menuName="menu.name"
          :menuPrice="menu.totalPrice"
          :menuWattage="menu.totalWattage"
          :menuCount="menu.count"
          :menuStatus="menu.status"
          :menuActive="menu.active"
          @menu-edit="Menu_Edit"
          @menu-delete="deleteCard"
        ></menu-card>
      </transition-group>
    </CRow>
  </section>
  <!-- Modal Start -->
  <!-- 新增、編輯 -->
  <!-- alignment="center" 置中 -->
  <!-- backdrop="static" 靜態 -->
  <CModal
    size="xl"
    alignment="center"
    backdrop="static"
    :visible="isModalVisible"
    @close="
      () => {
        isModalVisible = false
      }
    "
    aria-labelledby="MenuModal"
  >
    <menu-modal-body-c
      v-if="Mode === 'create'"
      @create-result="presentResult"
      @modal-close="isModalVisible = false"
    ></menu-modal-body-c>
    <menu-modal-body-e
      v-if="Mode === 'edit'"
      :menuId="editId"
      :menuName="editName"
      :menuPrice="editPrice"
      :menuWattage="editWattage"
      :products="Products"
      @modal-close="isModalVisible = false"
      @menu-update="cardInfoUpdate"
    ></menu-modal-body-e>
  </CModal>
  <!-- Modal End -->
</template>
<script setup>
// CCol 可以考慮 手機 sm 平板 md 桌機 lg
import { CRow, CCol } from '@coreui/vue'
import { CButton, CFormInput } from '@coreui/vue'
import { ref, onMounted } from 'vue'
import { toast } from 'vue3-toastify'
import 'vue3-toastify/dist/index.css'
import MenuModalBodyC from '@/components/backend/hardware/menu/create/MenuModalBodyC.vue'
import MenuModalBodyE from '@/components/backend/hardware/menu/edit/MenuModalBodyE.vue'
import MenuCard from '@/components/backend/hardware/MenuCard.vue'

// 身份驗證
import { useIdentityStore } from '@/stores/identity.js'
const authStore = useIdentityStore()

// 從環境變數取得 API BASE URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

let isModalVisible = ref(false)

let menuLists = ref([])

let Mode = ref('')
let editId = ref(0)
let editName = ref('')
let editPrice = ref(0)
let editWattage = ref(0)

// 宣告產品分類清單
let Products = ref([])

// 新增菜單 Modal 開啟
function Menu_Create() {
  Mode.value = 'create'
  isModalVisible.value = true
}

// 編輯菜單 Modal 開啟
function Menu_Edit(menuId, menuName, menuPrice, menuWattage) {
  editId.value = menuId
  editName.value = menuName
  editPrice.value = menuPrice
  editWattage.value = menuWattage
  Products.value = [
    {
      id: 10000,
      name: 'CPU',
      selectedId: 0,
      price: 0,
      wattage: 0
    },
    {
      id: 10001,
      name: 'GPU',
      selectedId: 0,
      price: 0,
      wattage: 0
    },
    {
      id: 10002,
      name: 'RAM',
      selectedId: 0,
      price: 0,
      wattage: 0
    },
    {
      id: 10003,
      name: 'MotherBoard',
      selectedId: 0,
      price: 0,
      wattage: 0
    },
    {
      id: 10004,
      name: 'SSD',
      selectedId: 0,
      price: 0,
      wattage: 0
    },
    {
      id: 10005,
      name: 'HDD',
      selectedId: 0,
      price: 0,
      wattage: 0
    },
    {
      id: 10006,
      name: 'AirCooler',
      selectedId: 0,
      price: 0,
      wattage: 0
    },
    {
      id: 10007,
      name: 'LiquidCooler',
      selectedId: 0,
      price: 0,
      wattage: 0
    },
    {
      id: 10008,
      name: 'CASE',
      selectedId: 0,
      price: 0,
      wattage: 0
    },
    {
      id: 10009,
      name: 'PSU',
      selectedId: 0,
      price: 0,
      wattage: 0
    },
    {
      id: 10010,
      name: 'OS',
      selectedId: 0,
      price: 0,
      wattage: 0
    }
  ]
  GetAllMenuDetails(menuId)
}

// Get MenuDetails
function GetAllMenuDetails(menuId) {
  fetch(`${apiUrl}/api/HardwareManage/GetMenuDetail?MenuId=${menuId}`, {
    method: 'GET',
    headers: {
      Authorization: `Bearer ${authStore.getToken}`
    }
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error('Network response was not ok')
      }
      return response.json()
    })
    .then((data) => {
      if (data !== null) {
        for (let item of data) {
          const product = Products.value.find((product) => product.id === item.typeId)
          product.selectedId = item.productId
          product.price = item.price
          product.wattage = item.wattage
        }
        Mode.value = 'edit'
        isModalVisible.value = true
      } else {
        console.log('沒有資料')
      }
    })
    .catch((error) => {
      console.error('There was a problem with the fetch operation:', error)
    })
}

// 訊息結果
function presentResult(result, isSuccess, menuId) {
  if (isSuccess) {
    insertMenu(menuId)
  }

  toast.success(result, {
    theme: 'dark',
    autoClose: 2000,
    transition: toast.TRANSITIONS.ZOOM,
    position: toast.POSITION.TOP_CENTER
  })
}

// 成功的話 插入列表
function insertMenu(menuId) {
  getMenu(menuId)
}

// 獲取單一菜單資料
function getMenu(id) {
  fetch(`${apiUrl}/api/HardwareManage/GetMenu?MenuId=${id}`, {
    method: 'GET',
    headers: {
      Authorization: `Bearer ${authStore.getToken}`
    }
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error('NetworkError')
      }
      return response.json()
    })
    .then((data) => {
      menuLists.value.push(data)
    })
    .catch((error) => {
      console.error(error.message)
    })
}
// 獲取菜單列表
function getMenuList() {
  fetch(`${apiUrl}/api/HardwareManage/GetMenuList`, {
    method: 'GET',
    headers: {
      Authorization: `Bearer ${authStore.getToken}`
    }
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error('NetworkError')
      }
      return response.json()
    })
    .then((data) => {
      menuLists.value = data
    })
    .catch((error) => {
      console.error(error.message)
    })
}

// 刪除菜單(畫面)
function deleteCard(id) {
  menuLists.value = menuLists.value.filter((item) => item.id !== id)
}

// 更新卡片菜單資訊
function cardInfoUpdate(menuId) {
  fetch(`${apiUrl}/api/HardwareManage/GetMenu?MenuId=${menuId}`, {
    method: 'GET',
    headers: {
      Authorization: `Bearer ${authStore.getToken}`
    }
  })
    .then((response) => {
      if (!response.ok) {
        throw new Error('NetworkError')
      }
      return response.json()
    })
    .then((data) => {
      // 抓出菜單資訊
      let index = menuLists.value.findIndex((item) => item.id === data.id)
      menuLists.value[index] = data
    })
    .catch((error) => {
      console.error(error.message)
    })
}

onMounted(() => {
  // 獲取菜單列表
  getMenuList()
})
</script>
<style scoped>
.list-move, /* 对移动中的元素应用的过渡 */
.list-enter-active,
.list-leave-active {
  transition: all 0.5s cubic-bezier(0.55, 0, 0.1, 1);
}

.list-enter-from,
.list-leave-to {
  opacity: 0;
  transform: scaleY(0.01) translate(30px, 0);
}

/* 确保将离开的元素从布局流中删除
  以便能够正确地计算移动的动画。 */
.list-leave-active {
  position: absolute;
}
</style>
