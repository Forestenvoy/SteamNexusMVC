<template>
  <CRow>
    <!-- 左側 -->
    <CCol xs="12" md="6" class="mb-5 mb-md-0 d-flex flex-column justify-content-center">
      <!-- 標題 -->
      <h3 class="title text-center">顯示卡</h3>
      <!-- 圖片 -->
      <div class="d-flex justify-content-center align-items-center">
        <img src="@/assets/images/builder/gpu.png" alt="GPU" class="image" />
      </div>
    </CCol>
    <!-- 右側 -->
    <CCol xs="12" md="6">
      <!-- Filter 製造商 -->
      <CRow class="mb-md-3">
        <CCol xs="12" class="text-center mb-3">
          <h3>製造商</h3>
        </CCol>
        <CCol xs="12">
          <div class="radio-button-container">
            <CRow>
              <CCol
                xs="4"
                md="3"
                v-for="item in manufacturerList"
                :key="item.value"
                class="d-flex justify-content-center"
              >
                <div class="radio-button">
                  <input
                    type="radio"
                    name="manufacturer"
                    class="radio-button__input"
                    :id="item.value"
                    :value="item.value"
                    v-model="manufacturer"
                    @change="filter"
                  />
                  <label class="radio-button__label" :for="item.value">
                    <span class="radio-button__custom"></span>
                    {{ item.name }}
                  </label>
                </div>
              </CCol>
            </CRow>
          </div>
        </CCol>
      </CRow>
      <!-- Filter 品牌 -->
      <CRow>
        <CCol xs="12" class="text-center mb-3">
          <h3>品牌</h3>
        </CCol>
        <CCol xs="12">
          <div class="radio-button-container">
            <CRow>
              <CCol
                xs="4"
                md="3"
                xl="2"
                v-for="item in brandList"
                :key="item.id"
                class="d-flex justify-content-center"
              >
                <div class="radio-button">
                  <input
                    type="radio"
                    name="brand"
                    class="radio-button__input"
                    :id="item.id"
                    :value="item.value"
                    v-model="brand"
                    @change="filter"
                  />
                  <label class="radio-button__label" :for="item.id">
                    <span class="radio-button__custom"></span>
                    {{ item.name }}
                  </label>
                </div>
              </CCol>
            </CRow>
          </div>
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
            name="GPU"
            class="form-select product"
            v-model="selectedGPU"
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
      <!-- 換頁 -->
      <CRow class="mb-3">
        <CCol xs="6" class="d-flex justify-content-center">
          <!-- 上一頁 -->
          <button class="back-btn" @click="builderStore.prev()">
            <svg
              width="34"
              height="34"
              viewBox="0 0 74 74"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
            >
              <circle cx="37" cy="37" r="35.5" stroke="black" stroke-width="3"></circle>
              <path
                d="M49 37C49 36.1716 48.3284 35.5 47.5 35.5V38.5C48.3284 38.5 49 37.8284 49 37ZM24.9393 35.9393C24.3536 36.5251 24.3536 37.4749 24.9393 38.0607L34.4853 47.6066C35.0711 48.1924 36.0208 48.1924 36.6066 47.6066C37.1924 47.0208 37.1924 46.0711 36.6066 45.4853L28.1213 37L36.6066 28.5147C37.1924 27.9289 37.1924 26.9792 36.6066 26.3934C36.0208 25.8076 35.0711 25.8076 34.4853 26.3934L24.9393 35.9393ZM49 38.5H26V35.5H49V38.5Z"
                fill="black"
              ></path>
            </svg>
            <span>Back</span>
          </button>
        </CCol>
        <CCol xs="6" class="d-flex justify-content-center">
          <!-- 下一頁 -->
          <button class="next-btn" @click="builderStore.next()">
            <span>Next</span>
            <svg
              width="34"
              height="34"
              viewBox="0 0 74 74"
              fill="none"
              xmlns="http://www.w3.org/2000/svg"
            >
              <circle cx="37" cy="37" r="35.5" stroke="black" stroke-width="3"></circle>
              <path
                d="M25 35.5C24.1716 35.5 23.5 36.1716 23.5 37C23.5 37.8284 24.1716 38.5 25 38.5V35.5ZM49.0607 38.0607C49.6464 37.4749 49.6464 36.5251 49.0607 35.9393L39.5147 26.3934C38.9289 25.8076 37.9792 25.8076 37.3934 26.3934C36.8076 26.9792 36.8076 27.9289 37.3934 28.5147L45.8787 37L37.3934 45.4853C36.8076 46.0711 36.8076 47.0208 37.3934 47.6066C37.9792 48.1924 38.9289 48.1924 39.5147 47.6066L49.0607 38.0607ZM25 38.5L48 38.5V35.5L25 35.5V38.5Z"
                fill="black"
              ></path>
            </svg>
          </button>
        </CCol>
      </CRow>
    </CCol>
  </CRow>
</template>

<script setup>
// vue
import { ref, onMounted } from 'vue'

// core UI
import { CRow, CCol, CForm } from '@coreui/vue'

// pinia
import { useBuilderStore } from '@/stores/builder.js'
const builderStore = useBuilderStore()

// API URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

// Original Data : CPU Products (Classified)
const GPUGroups = ref({})

// Sort Data
const SortGroups = ref({})

// Selected Product
const selectedGPU = ref(0)

// 製造商清單
const manufacturerList = [
  { name: '全部', value: 'All' },
  { name: 'AMD', value: 'AMD' },
  { name: 'Intel', value: 'INTEL' },
  { name: 'NVIDIA', value: 'NVIDIA' }
]

// 品牌清單
const brandList = [
  { name: '全部', value: 'All', id: 'brand_all' },
  { name: 'Intel', value: 'INTEL', id: 'brand_intel' },
  { name: '宏碁', value: '宏碁', id: 'brand_acer' },
  { name: '華碩', value: '華碩', id: 'brand_asus' },
  { name: '技嘉', value: '技嘉', id: 'brand_gigabyte' },
  { name: '微星', value: '微星', id: 'brand_msi' },
  { name: '華擎', value: '華擎', id: 'brand_asrock' },
  { name: '索泰', value: 'ZOTAC', id: 'brand_zotac' },
  { name: '映眾', value: 'INNO3D', id: 'brand_inno3d' }
]

// filter ref
const manufacturer = ref('All')
const brand = ref('All')
const min = ref('')
const max = ref('')
const keyword = ref('')

// (Async) get GPU Data
const getData = async () => {
  console.log('Get GPU Data...')
  await fetch(`${apiUrl}/api/PcBuilder?type=10001`, {
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
    if (!GPUGroups.value[className]) {
      GPUGroups.value[className] = []
    }
    GPUGroups.value[className].push(item)
  })
  // 將資料存入 session storage
  sessionStorage.setItem('GPUList', JSON.stringify(GPUGroups.value))
  SortGroups.value = GPUGroups.value
}

// Filter
const filter = () => {
  filterByManufacturer()
  filterByBrand()
  filterByPrice()
  filterByKeyword()
}

// Filter By Manufacturer
const filterByManufacturer = () => {
  if (manufacturer.value === 'All') {
    SortGroups.value = GPUGroups.value
  } else {
    // 過濾名稱內含有該品牌的資料
    const filteredData = Object.keys(GPUGroups.value)
      .filter((key) => key.includes(manufacturer.value))
      .reduce((obj, key) => {
        obj[key] = GPUGroups.value[key]
        return obj
      }, {})
    SortGroups.value = filteredData
  }
}

// Filter By Brand
const filterByBrand = () => {
  if (brand.value !== 'All') {
    // 過濾名稱內含有該品牌的資料
    const filteredGroups = Object.keys(SortGroups.value).reduce((result, key) => {
      const filteredProducts = SortGroups.value[key].filter((product) =>
        product.name.includes(brand.value)
      )
      if (filteredProducts.length > 0) {
        result[key] = filteredProducts
      }
      return result
    }, {})
    SortGroups.value = filteredGroups
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
    type: 'GPU',
    id: Id,
    name: Name,
    price: Price,
    wattage: Wattage
  }
  // 加入至產品清單
  builderStore.addProduct('GPU', Product)
}

onMounted(() => {
  // 檢查 session storage 是否有資料
  const storedData = sessionStorage.getItem('GPUList')
  if (storedData !== null && storedData !== undefined) {
    GPUGroups.value = JSON.parse(storedData)
    SortGroups.value = GPUGroups.value
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

/* 標題 */
.title {
  font-size: 2.5rem;
  font-weight: 700;
  letter-spacing: 2px;
  margin-bottom: 40px;
}

/* 圖片 */
.image {
  width: 350px;
  height: 300px;
}

@media screen and (max-width: 576px) {
  .image {
    width: 250px;
    height: 200px;
  }
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

/* 上一頁、下一頁 */
button {
  cursor: pointer;
  font-weight: 700;
  font-family: Helvetica, 'sans-serif';
  transition: all 0.2s;
  padding: 10px 20px;
  border-radius: 100px;
  color: #000;
  background: #f3ae0b;
  border: 1px solid transparent;
  display: flex;
  align-items: center;
  font-size: 15px;
}

button:hover {
  background: #f3ae0b;
}

.back-btn > svg {
  width: 34px;
  margin-right: 10px;
  transition: transform 0.3s ease-in-out;
}

.next-btn > svg {
  width: 34px;
  margin-left: 10px;
  transition: transform 0.3s ease-in-out;
}

.back-btn:hover svg {
  transform: translateX(-5px);
}

.next-btn:hover svg {
  transform: translateX(5px);
}

button:active {
  transform: scale(0.95);
}
</style>
