<template>
  <CRow class="mb-3">
    <CCol
      xs="12"
      md="2"
      class="d-flex align-items-center justify-content-center justify-content-md-start mb-3 mb-md-0"
    >
      <label :for="props.typeName" class="m-0 h5">{{ props.typeName }}</label>
    </CCol>
    <CCol xs="12" md="10">
      <select
        :name="props.typeName"
        class="form-select"
        @change="onSelectChange"
        v-model="selectedId"
        ref="selectElement"
      >
        <option value="-1" disabled hidden>null</option>
        <option value="0" disabled hidden>---- 請選擇硬體 ----</option>
        <optgroup :label="groupName" v-for="(group, groupName) in typeGroups" :key="groupName">
          <option
            v-for="item in group"
            :value="item.productId"
            :key="item.productId"
            :data-price="item.price"
            :data-wattage="item.wattage"
            :data-recommend="item.recommend"
            :id="item.productId"
          >
            {{ item.productName }} {{ item.specification }} ,${{ item.price }}
          </option>
        </optgroup>
      </select>
    </CCol>
  </CRow>
</template>
<script setup>
import { CRow, CCol } from '@coreui/vue'
import { ref, onMounted } from 'vue'
// 身份驗證
import { useIdentityStore } from '@/stores/identity.js'
const authStore = useIdentityStore()

// 從環境變數取得 API BASE URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

const props = defineProps({
  typeName: String,
  type: Number,
  selectedId: Number,
  oriPrice: Number,
  oriWattage: Number
})

let selectedId = ref(props.selectedId)

const emit = defineEmits(['updateInfo', 'productSelected'])

let selectElement = ref(null)
let oriPrice = ref(props.oriPrice)
let oriWattage = ref(props.oriWattage)

// 宣告 產品分類集合
let typeGroups = ref({})

// 取得全部產品資料
function getProducts(type) {
  fetch(`${apiUrl}/api/HardwareManage/GetProductData?Type=${type}`, {
    method: 'GET',
    headers: {
      Authorization: `Bearer ${authStore.getToken}`
    }
  })
    .then((response) => {
      return response.json()
    })
    .then((data) => {
      productClassify(data)
    })
    .catch((error) => {
      console.error('Error:', error)
    })
}

// 對產品進行分類
function productClassify(data) {
  data.forEach((item) => {
    // 讀取 產品分類
    const Classification = item.componentClassificationName
    // 如果分類數組不存在，則建立
    if (!typeGroups.value[Classification]) {
      typeGroups.value[Classification] = []
    }
    // 將產品資料加入到對應的分類中
    typeGroups.value[Classification].push(item)
  })
  // 將數據存入 sessionStorage
  sessionStorage.setItem(`${props.type}Groups`, JSON.stringify(typeGroups.value))
}

// 產品選擇事件
function onSelectChange(event) {
  // 更新價格、瓦數
  const price = event.target.options[event.target.options.selectedIndex].dataset.price
  const wattage = event.target.options[event.target.options.selectedIndex].dataset.wattage
  emit('updateInfo', Number(price), Number(wattage), oriPrice.value, oriWattage.value)
  oriPrice.value = Number(price)
  oriWattage.value = Number(wattage)
  // 產品變更
  emit('productSelected', props.type, selectedId.value)
}

// 找到 被選擇的產品選項

onMounted(() => {
  // 從 sessionStorage 取得產品分類
  const storedTypeGroups = sessionStorage.getItem(`${props.type}Groups`)
  if (storedTypeGroups) {
    typeGroups.value = JSON.parse(storedTypeGroups)
  } else {
    // 如果 SessionStorage 中沒有數據，則請求數據
    getProducts(props.type)
  }
})
</script>

<style></style>
