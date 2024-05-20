<template>
  <!-- 過濾 -->
  <CRow>
    <CCol xs="12" md="6">
      <h3>CPU</h3>
    </CCol>
    <CCol xs="12" md="6">
      <select name="CPUList" class="form-select"></select>
    </CCol>
  </CRow>
</template>

<script setup>
// vue
import { ref, onMounted } from 'vue'

// core UI
import { CRow, CCol } from '@coreui/vue'

// APIURL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

// (Async) get CPU Data
const getData = async () => {
  await fetch(`${apiUrl}/api/PcBuilder`, {
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
      // 將資料存入 session storage
      sessionStorage.setItem('CPUList', JSON.stringify(data))
    })
    .catch((error) => {
      console.error('Error:', error.message)
    })
}

onMounted(() => {
  // 檢查 session storage 是否有資料
  if (sessionStorage.getItem('CPUList')) {
    const CPUList = JSON.parse(sessionStorage.getItem('CPUList'))
  } else {
    getData()
  }
})
</script>

<style scoped></style>
