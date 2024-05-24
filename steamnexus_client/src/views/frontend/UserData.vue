<template>
  <div class="container mt-4">
    <div class="d-flex flex-row">
      <div class="col-12 col-md-3 list-group-container">
        <!-- 左側導航選單 -->
        <CListGroup>
          <CListGroupItem :active="activeItem === 'profile'" @click="showProfile"
            >會員基本資料修改</CListGroupItem
          >
          <CListGroupItem :active="activeItem === 'password'" @click="showPassword"
            >密碼修改</CListGroupItem
          >
        </CListGroup>
      </div>
      <div class="col-12 col-md-9">
        <!-- 基本資料修改表單 -->
        <div v-if="activeItem === 'profile'">
          <h2>會員基本資料修改</h2>
          <form autocomplete="off">
            <div class="row">
              <!-- 左邊部分：大頭貼 -->
              <div class="col-md-4">
                <div class="form-group">
                  <label for="avatar">大頭貼</label>
                  <input type="file" class="form-control" id="avatar" @change="onAvatarChange" />
                </div>
              </div>
              <!-- 右邊部分：其他資料 -->
              <div class="col-md-8">
                <div class="form-group">
                  <label for="name">姓名</label>
                  <input type="text" class="form-control" id="name" v-model="profile.name" />
                </div>
                <div class="form-group">
                  <label for="phone">電話</label>
                  <input type="text" class="form-control" id="phone" v-model="profile.phone" />
                </div>
                <div class="form-group">
                  <label for="mobile">手機</label>
                  <input type="text" class="form-control" id="mobile" v-model="profile.mobile" />
                </div>
                <div class="form-group">
                  <label for="birthday">生日</label>
                  <input
                    type="date"
                    class="form-control"
                    id="birthday"
                    v-model="profile.birthday"
                  />
                </div>
                <div class="form-group text-end">
                  <button type="editUserSubmit" class="btn btn-primary mt-3">儲存修改</button>
                </div>
              </div>
            </div>
          </form>
        </div>
        <!-- 密碼修改表單 -->
        <div v-else-if="activeItem === 'password'">
          <h2>密碼修改</h2>
          <form @submit.prevent="changePassword" autocomplete="off">
            <div class="form-group mb-3">
              <label for="oldPassword">舊密碼</label>
              <input
                type="password"
                class="form-control"
                id="oldPassword"
                v-model="oldPassword"
                required
              />
            </div>
            <div class="form-group mb-3">
              <label for="confirmOldPassword">確認舊密碼</label>
              <input
                type="password"
                class="form-control"
                id="confirmOldPassword"
                v-model="confirmOldPassword"
                required
                @input="validatePasswords"
              />
              <div id="oldPasswordFeedback" class="invalid-feedback">密碼與確認密碼不一致</div>
            </div>
            <div class="form-group mb-3">
              <label for="newPassword">新密碼</label>
              <input
                type="password"
                class="form-control"
                id="newPassword"
                v-model="newPassword"
                required
              />
            </div>
            <div class="form-group mb-3">
              <label for="confirmNewPassword">確認新密碼</label>
              <input
                type="password"
                class="form-control"
                id="confirmNewPassword"
                v-model="confirmNewPassword"
                required
                @input="validateNewPasswords"
              />
              <div id="newPasswordFeedback" class="invalid-feedback">密碼與確認密碼不一致</div>
            </div>
            <button type="submit" class="btn btn-primary mt-2">儲存修改</button>
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
import { onMounted, ref, computed } from 'vue'
import axios from 'axios'

// 從環境變數取得 API BASE URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

const token = ''

const activeItem = ref('profile')
const profile = ref({
  name: '',
  phone: '',
  mobile: '',
  birthday: '',
  avatar: null
})
const oldPassword = ref('')
const confirmOldPassword = ref('')
const newPassword = ref('')
const confirmNewPassword = ref('')

// 切換顯示的表單
const showProfile = () => {
  activeItem.value = 'profile'
}

const showPassword = () => {
  activeItem.value = 'password'
}

// 確認大頭貼是否有進行變更
const onAvatarChange = (event) => {
  profile.value.avatar = event.target.files[0]
}

// 檢查舊密碼是否一致
const validatePasswords = () => {
  const passwordValue = $('#oldPassword').val()
  const confirmPasswordValue = $('#confirmOldPassword').val()
  if (passwordValue !== confirmPasswordValue) {
    $('#oldPasswordFeedback').show()
    $('#confirmOldPassword').addClass('is-invalid')
  } else {
    $('#oldPasswordFeedback').hide()
    $('#confirmOldPassword').removeClass('is-invalid').addClass('is-valid')
  }
}

// 檢查新密碼是否一致
const validateNewPasswords = () => {
  const passwordValue = $('#newPassword').val()
  const confirmPasswordValue = $('#confirmNewPassword').val()
  if (passwordValue !== confirmPasswordValue) {
    $('#newPasswordFeedback').show()
    $('#confirmNewPassword').addClass('is-invalid')
  } else {
    $('#newPasswordFeedback').hide()
    $('#confirmNewPassword').removeClass('is-invalid').addClass('is-valid')
  }
}

// 密碼修改
const changePassword = () => {
  const token = store.getToken
  console.log('JWT Token:', token) // 輸出 JWT Token

  axios
    .post(
      `${apiUrl}/api/UserIdentity/ChangePassword`,
      {
        oldPassword: oldPassword.value,
        newPassword: newPassword.value
      },
      {
        headers: {
          Authorization: `Bearer ${token}` // 確保 token 正確傳遞
        }
      }
    )
    .then((response) => {
      console.log('Response:', response) // 輸出響應結果
      alert(response.data)
    })
    .catch((error) => {
      console.error('Error:', error.response) // 輸出錯誤信息
      alert('密碼修改失敗: ' + error.response.data)
    })
}

onMounted(() => {})
</script>

<style scoped>
.list-group-container {
  margin-right: 50px; /* 可以根據需要調整數值 */
}
</style>
