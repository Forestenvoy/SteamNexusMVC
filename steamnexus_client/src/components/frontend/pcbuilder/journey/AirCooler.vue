<template>
  <!-- 過濾 -->
  <!-- Filter 品牌 -->
  <CRow>
    <CCol xs="12" class="text-center mb-3">
      <h3>品牌</h3>
    </CCol>
    <CCol xs="12" class="d-flex justify-content-center">
      <select
        name="brand"
        class="form-select"
        style="width: 150px"
        v-model="brand"
        @change="filter"
      >
        <option value="All" selected>全部</option>
        <option :value="item.value" v-for="item in brandList" :key="item.value">
          {{ item.name }}
        </option>
      </select>
    </CCol>
  </CRow>
  <!-- Filter 價格 -->
  <CRow class="mb-3">
    <CCol xs="12" class="mb-4">
      <CForm @input="filter">
        <CRow>
          <CCol xs="5" class="d-flex justify-content-end align-items-center">
            <div class="form__group field">
              <input type="number" class="form__field" placeholder="最低價格" v-model="min" />
              <label for="min" class="form__label">最低價格</label>
            </div>
          </CCol>
          <CCol xs="2" class="d-flex justify-content-center align-items-end">
            <label style="font-size: 30px">~</label>
          </CCol>
          <CCol xs="5" class="d-flex justify-content-start align-items-center">
            <div class="form__group field">
              <input type="number" class="form__field" placeholder="最低價格" v-model="max" />
              <label for="max" class="form__label">最高價格</label>
            </div>
          </CCol>
        </CRow>
      </CForm>
    </CCol>
  </CRow>
  <!-- Filter 關鍵字 -->
  <CRow class="mb-4">
    <CCol
      xs="5"
      md="6"
      class="d-flex justify-content-center justify-content-md-end align-items-center"
    >
      <h3 class="m-0 pe-0 pe-md-2">產品選擇</h3>
    </CCol>
    <CCol xs="7" md="6" class="d-flex justify-content-start">
      <div class="input-wrapper">
        <input
          class="input-box text-center"
          type="text"
          placeholder="&#x1F50D;&#xFE0E;  請輸入關鍵字"
          v-model="keyword"
          @input="filter"
        />
        <span class="underline"></span>
      </div>
    </CCol>
  </CRow>
  <!-- 選單 -->
  <CRow class="mb-5">
    <CCol xs="12" class="d-flex justify-content-center align-items-center">
      <select
        name="AirCooler"
        class="form-select product"
        v-model="selectedAirCooler"
        @change="selectedProduct"
      >
        <option :value="0" disabled selected hidden>---- 請選擇硬體 ----</option>
        <optgroup
          :label="groupName"
          v-for="(group, groupName) in SortGroups"
          :key="groupName"
          style="color: #f3ae0b"
        >
          <template v-for="item in group" :key="item.id">
            <option
              :value="item.id"
              :data-price="item.price"
              :data-wattage="item.wattage"
              :data-recommend="item.recommend"
              style="color: #fff"
            >
              {{ item.name }} {{ item.specification }}
            </option>
            <!-- 在每个项目的选项下方添加价格选项 -->
            <option disabled style="color: #00ff00">${{ item.price }}</option>
          </template>
        </optgroup>
      </select>
    </CCol>
  </CRow>
</template>

<script setup>
// vue
import { ref, onMounted, watch } from 'vue'

// core UI
import { CRow, CCol, CForm } from '@coreui/vue'

// pinia
import { useBuilderStore } from '@/stores/builder.js'
const builderStore = useBuilderStore()

// API URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

// Original Data : AirCooler Products (Classified)
const AirCoolerGroups = ref({})

// Sort Data
const SortGroups = ref({})

// props
const props = defineProps(['defaultSelected'])

// emits
const emits = defineEmits(['selected'])

// Selected Product
const selectedAirCooler = ref(props.defaultSelected)

// 品牌清單
const brandList = [
  { name: '君主', value: '君主' },
  { name: 'ID-COOLING', value: 'ID-COOLING' },
  { name: 'be quiet!', value: 'be quiet!' },
  { name: '美洲獅', value: '美洲獅' },
  { name: '利民', value: '利民' },
  { name: '九州風神', value: '九州風神' },
  { name: '銀欣', value: '銀欣' },
  { name: '酷碼', value: '酷碼' },
  { name: '鎌刀', value: '鎌刀' },
  { name: '喬思伯', value: '喬思伯' },
  { name: '貓頭鷹', value: '貓頭鷹' },
  { name: '快睿', value: '快睿' }
]
// filter ref
const brand = ref('All')
const min = ref('')
const max = ref('')
const keyword = ref('')

// (Async) get AirCooler Data
const getData = async () => {
  console.log('Get AirCooler Data...')
  await fetch(`${apiUrl}/api/PcBuilder?type=10006`, {
    method: 'GET'
  })
    .then((response) => {
      if (!response.ok) {
        return response.text().then((errorMessage) => {
          throw new Error(errorMessage)
        })
      }
      return response.json()
    })
    .then((data) => {
      // 分類
      classification(data)
    })
    .catch((error) => {
      console.error('Error:', error.message)
    })
}

// Classification
const classification = (data) => {
  data.forEach((item) => {
    const className = item.classification
    if (!AirCoolerGroups.value[className]) {
      AirCoolerGroups.value[className] = []
    }
    AirCoolerGroups.value[className].push(item)
  })
  // 將資料存入 session storage
  sessionStorage.setItem('AirCoolerList', JSON.stringify(AirCoolerGroups.value))
  SortGroups.value = AirCoolerGroups.value
}

// Filter
const filter = () => {
  filterByBrand()
  filterByPrice()
  filterByKeyword()
}

// Filter By Brand
const filterByBrand = () => {
  if (brand.value === 'All') {
    SortGroups.value = AirCoolerGroups.value
  } else {
    // 過濾名稱內含有該品牌的資料
    const filteredData = Object.keys(AirCoolerGroups.value)
      .filter((key) => key.includes(brand.value))
      .reduce((obj, key) => {
        obj[key] = AirCoolerGroups.value[key]
        return obj
      }, {})
    SortGroups.value = filteredData
  }
}

// Filter By Price
const filterByPrice = () => {
  // 過濾最低價格
  if (min.value !== '') {
    const filteredGroups = Object.keys(SortGroups.value).reduce((result, key) => {
      const filteredProducts = SortGroups.value[key].filter((product) => product.price >= min.value)
      if (filteredProducts.length > 0) {
        result[key] = filteredProducts
      }
      return result
    }, {})
    SortGroups.value = filteredGroups
  }
  // 過濾最高價格
  if (max.value !== '') {
    const filteredGroups = Object.keys(SortGroups.value).reduce((result, key) => {
      const filteredProducts = SortGroups.value[key].filter((product) => product.price <= max.value)
      if (filteredProducts.length > 0) {
        result[key] = filteredProducts
      }
      return result
    }, {})
    SortGroups.value = filteredGroups
  }
}

// Filter By Keyword
const filterByKeyword = () => {
  // 過濾關鍵字
  if (keyword.value !== '') {
    const filteredGroups = Object.keys(SortGroups.value).reduce((result, key) => {
      const filteredProducts = SortGroups.value[key].filter((product) =>
        product.name.includes(keyword.value)
      )
      if (filteredProducts.length > 0) {
        result[key] = filteredProducts
      }
      return result
    }, {})
    SortGroups.value = filteredGroups
  }
}

// 產品選擇事件
const selectedProduct = (event) => {
  const Id = event.target.value
  const Name = event.target.options[event.target.selectedIndex].text
  const Price = event.target.options[event.target.selectedIndex].getAttribute('data-price')
  const Wattage = event.target.options[event.target.selectedIndex].getAttribute('data-wattage')
  const Product = {
    type: 'AirCooler',
    id: Id,
    name: Name,
    price: Price,
    wattage: Wattage
  }
  // 加入至產品清單
  builderStore.addProduct('AirCooler', Product)
  emits('selected', selectedAirCooler.value)
}

// 監看 props.defaultSelected 是否有變動
watch(
  () => props.defaultSelected,
  (newValue) => {
    selectedAirCooler.value = newValue
  }
)

onMounted(() => {
  // 檢查 session storage 是否有資料
  const storedData = sessionStorage.getItem('AirCoolerList')
  if (storedData !== null && storedData !== undefined) {
    AirCoolerGroups.value = JSON.parse(storedData)
    SortGroups.value = AirCoolerGroups.value
  } else {
    getData()
  }
})
</script>

<style scoped>
/* 隱藏數字增減按鈕 */
input::-webkit-outer-spin-button,
input::-webkit-inner-spin-button {
  -webkit-appearance: none;
  margin: 0;
}

input[type='number'] {
  -webkit-appearance: textfield; /* Safari */
  -moz-appearance: textfield; /* Firefox */
  appearance: textfield;
}

.product {
  max-width: 500px;
}

/* 製造商 */
.radio-button-container {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 24px;
}

.radio-button {
  display: inline-block;
  position: relative;
  cursor: pointer;
}

.radio-button__input {
  position: absolute;
  opacity: 0;
  width: 0;
  height: 0;
}

.radio-button__label {
  display: inline-block;
  padding-left: 30px;
  margin-bottom: 10px;
  position: relative;
  font-size: 15px;
  color: #f2f2f2;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
}

.radio-button__custom {
  position: absolute;
  top: 0;
  left: 0;
  width: 20px;
  height: 20px;
  border-radius: 50%;
  border: 2px solid #555;
  transition: all 0.3s ease;
}

.radio-button__input:checked + .radio-button__label .radio-button__custom {
  background-color: #f3ae0b;
  border-color: transparent;
  transform: scale(0.8);
  box-shadow: 0 0 20px #a47609;
}

.radio-button__input:checked + .radio-button__label {
  color: #f3ae0b;
}

.radio-button__label:hover .radio-button__custom {
  transform: scale(1.2);
  border-color: #f3ae0b;
  box-shadow: 0 0 20px #a47609;
}

/* 價格樣式 */
.form__group {
  position: relative;
  padding: 20px 0 0;
  width: 100%;
  max-width: 130px;
}

.form__field {
  font-family: inherit;
  text-align: center;
  width: 100%;
  border: none;
  border-bottom: 2px solid #9b9b9b;
  outline: 0;
  font-size: 17px;
  color: #fff;
  padding: 7px 0;
  background: transparent;
  transition: border-color 0.2s;
}

.form__field::placeholder {
  color: transparent;
}

.form__field:placeholder-shown ~ .form__label {
  font-size: 17px;
  cursor: text;
  top: 25px;
}

.form__label {
  position: absolute;
  top: 0;
  left: 50%;
  transform: translateX(-50%);
  display: block;
  transition: 0.2s;
  font-size: 17px;
  color: #9b9b9b;
  pointer-events: none;
}

.form__field:focus {
  padding-bottom: 6px;
  font-weight: 700;
  border-width: 3px;
  border-image: linear-gradient(to right, #a47609, #f3ae0b);
  border-image-slice: 1;
}

.form__field:focus ~ .form__label {
  position: absolute;
  top: 0;
  display: block;
  transition: 0.2s;
  font-size: 17px;
  color: #f3ae0b;
  font-weight: 700;
}

/* 關鍵字 */
.input-wrapper {
  position: relative;
  width: 180px;
}

.input-box {
  font-size: 16px;
  padding: 5px 0;
  border: none;
  border-bottom: 2px solid #ccc;
  color: #fff;
  width: 100%;
  background-color: transparent;
  transition: border-color 0.3s ease-in-out;
}

.underline {
  position: absolute;
  bottom: 0;
  left: 0;
  width: 180px;
  height: 2px;
  background-color: #f3ae0b;
  transform: scaleX(0);
  transition: transform 0.3s ease-in-out;
}

.input-box:focus {
  border-color: #a47609;
  outline: none;
}

.input-box:focus + .underline {
  transform: scaleX(1);
}
</style>
