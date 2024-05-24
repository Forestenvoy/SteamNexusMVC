<template>
  <div class="container mt-4">
    <div class="d-flex flex-row">
      <div class="col-12 col-md-3 list-group-container">
        <CListGroup>
          <CListGroupItem active>會員基本資料修改</CListGroupItem>
          <CListGroupItem>密碼修改</CListGroupItem>
        </CListGroup>
      </div>
      <div class="content-container">
        <div class="col-12 col-md-9">
          <form @submit.prevent="submitdeitPassword">
            <h2 class="col-12">密碼修改</h2>
            <div class="mt-3">
              <label class="mb-1" for="">原始密碼</label>
              <input type="password" name="" id="" v-model="oldPassword" readonly />
            </div>
            <div class="mt-3">
              <label class="mb-1" for="">新密碼</label>
              <input type="password" name="" id="" v-model="Password" required />
            </div>
            <div class="mt-3">
              <label class="mb-1" for="">新密碼確認</label>
              <input type="password" name="" id="" v-model="confirmPassword" required />
            </div>
            <button class="btn btn-primary mt-4">儲存修改</button>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>
<script setup>
// 使用 Pinia，利用 store 去訪問狀態
import { useIdentityStore } from '@/stores/identity.js'
const store = useIdentityStore()
// export default {}
import { onMounted, ref } from 'vue'
const oldPassword = ref('')
const Password = ref('')
const confirmPassword = ref('')

const token = ''

const submitdeitPassword = () => {
  // 處理密碼變更表單提交
}

onMounted(() => {})
</script>

<style scoped>
.list-group-container {
  margin-right: 50px; /* 可以根據需要調整數值 */
}
</style>
