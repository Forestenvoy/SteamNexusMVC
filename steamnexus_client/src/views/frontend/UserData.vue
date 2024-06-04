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
          <form @submit.prevent="editUserSubmit" autocomplete="off">
            <div class="row">
              <!-- 左邊部分：大頭貼 -->
              <div class="col-md-4">
                <div class="form-group">
                  <!-- <label for="avatar">大頭貼</label> -->
                  <img
                    v-if="profile.photo || profile.previewPhoto"
                    :src="profile.previewPhoto || profile.photo"
                    alt="大頭貼"
                    class="img-thumbnail mt-2"
                  />
                  <input
                    type="file"
                    class="form-control mt-2"
                    id="avatar"
                    @change="onAvatarChange"
                  />
                </div>
              </div>
              <!-- 右邊部分：其他資料 -->
              <div class="col-md-8">
                <div class="form-group">
                  <label for="name">姓名</label>
                  <input type="text" class="form-control" id="name" v-model="profile.name" />
                </div>
                <div class="form-group">
                  <label class="mt-2">性別</label>
                  <div>
                    <div class="form-check form-check-inline">
                      <input
                        class="form-check-input"
                        type="radio"
                        id="genderMale"
                        value="true"
                        v-model="profile.gender"
                      />
                      <label class="form-check-label" for="genderMale">男</label>
                    </div>
                    <div class="form-check form-check-inline">
                      <input
                        class="form-check-input"
                        type="radio"
                        id="genderFemale"
                        value="false"
                        v-model="profile.gender"
                      />
                      <label class="form-check-label" for="genderFemale">女</label>
                    </div>
                  </div>
                </div>
                <div class="form-group mt-2">
                  <label for="phone">電話</label>
                  <input
                    type="text"
                    class="form-control"
                    id="phone"
                    v-model="profile.phone"
                    maxlength="10"
                    pattern="\d*"
                    @input="validatePhone"
                  />
                  <div id="phoneFeedback" class="invalid-feedback">請輸入數字</div>
                </div>
                <div class="form-group mt-2">
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
        <!-- <div v-else-if="activeItem === 'password'">
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
        </div> -->
        <div v-else-if="activeItem === 'password'">
          <h2>密碼修改</h2>
          <form @submit.prevent="changePassword" autocomplete="off">
            <div class="form-group mb-3">
              <label for="oldPassword">舊密碼</label>
              <div class="input-group">
                <input
                  :type="oldPasswordVisible ? 'text' : 'password'"
                  class="form-control"
                  id="oldPassword"
                  v-model="oldPassword"
                  required
                />
                <button
                  class="btn btn-outline-secondary"
                  type="button"
                  @click="toggleOldPasswordVisibility"
                >
                  <i :class="oldPasswordVisible ? 'fa fa-eye' : 'fa fa-eye-slash'"></i>
                </button>
              </div>
            </div>
            <!-- <div class="form-group mb-3">
              <label for="confirmOldPassword">確認舊密碼</label>
              <div class="input-group">
                <input
                  :type="confirmOldPasswordVisible ? 'text' : 'password'"
                  class="form-control"
                  id="confirmOldPassword"
                  v-model="confirmOldPassword"
                  required
                  @input="validatePasswords"
                />
                <button
                  class="btn btn-outline-secondary"
                  type="button"
                  @click="toggleConfirmOldPasswordVisibility"
                >
                  <i :class="confirmOldPasswordVisible ? 'fa fa-eye' : 'fa fa-eye-slash'"></i>
                </button>
              </div>
              <div id="oldPasswordFeedback" class="invalid-feedback">密碼與確認密碼不一致</div>
            </div> -->
            <div class="form-group mb-3">
              <label for="newPassword">新密碼</label>
              <div class="input-group">
                <input
                  :type="newPasswordVisible ? 'text' : 'password'"
                  class="form-control"
                  id="newPassword"
                  v-model="newPassword"
                  required
                />
                <button
                  class="btn btn-outline-secondary"
                  type="button"
                  @click="toggleNewPasswordVisibility"
                >
                  <i :class="newPasswordVisible ? 'fa fa-eye' : 'fa fa-eye-slash'"></i>
                </button>
              </div>
            </div>
            <div class="form-group mb-3">
              <label for="confirmNewPassword">確認新密碼</label>
              <div class="input-group">
                <input
                  :type="confirmNewPasswordVisible ? 'text' : 'password'"
                  class="form-control"
                  id="confirmNewPassword"
                  v-model="confirmNewPassword"
                  required
                  @input="validateNewPasswords"
                />
                <button
                  class="btn btn-outline-secondary"
                  type="button"
                  @click="toggleConfirmNewPasswordVisibility"
                >
                  <i :class="confirmNewPasswordVisible ? 'fa fa-eye' : 'fa fa-eye-slash'"></i>
                </button>
              </div>
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

// 特殊吐司
import { toast } from 'vue3-toastify'
import 'vue3-toastify/dist/index.css'

// 從環境變數取得 API BASE URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

const token = ''

const activeItem = ref('profile')
const profile = ref({
  name: '',
  phone: '',
  mobile: '',
  birthday: '',
  avatar: null,
  photo: null,
  previewPhoto: null, // 新增預覽照片屬性
  originalPhoto: null // 存放原本的圖片路徑
})
const oldPassword = ref('')
// const confirmOldPassword = ref('')
const newPassword = ref('')
const confirmNewPassword = ref('')

const oldPasswordVisible = ref(false)
// const confirmOldPasswordVisible = ref(false)
const newPasswordVisible = ref(false)
const confirmNewPasswordVisible = ref(false)

// 切換密碼可見性
const toggleOldPasswordVisibility = () => {
  oldPasswordVisible.value = !oldPasswordVisible.value
}

// const toggleConfirmOldPasswordVisibility = () => {
//   confirmOldPasswordVisible.value = !confirmOldPasswordVisible.value
// }

const toggleNewPasswordVisibility = () => {
  newPasswordVisible.value = !newPasswordVisible.value
}

const toggleConfirmNewPasswordVisibility = () => {
  confirmNewPasswordVisible.value = !confirmNewPasswordVisible.value
}

// 切換顯示的表單
const showProfile = () => {
  activeItem.value = 'profile'
}

const showPassword = () => {
  activeItem.value = 'password'
}

// 確認大頭貼是否有進行變更
const onAvatarChange = (event) => {
  const file = event.target.files[0]
  if (file) {
    profile.value.avatar = file
    const reader = new FileReader()
    reader.onload = (e) => {
      profile.value.previewPhoto = e.target.result
    }
    reader.readAsDataURL(file)
  }
}

// 檢查舊密碼是否一致
// const validatePasswords = () => {
//   const passwordValue = $('#oldPassword').val()
//   const confirmPasswordValue = $('#confirmOldPassword').val()
//   if (passwordValue !== confirmPasswordValue) {
//     $('#oldPasswordFeedback').show()
//     $('#confirmOldPassword').addClass('is-invalid')
//   } else {
//     $('#oldPasswordFeedback').hide()
//     $('#confirmOldPassword').removeClass('is-invalid').addClass('is-valid')
//   }
// }

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

//確認電話號碼是否為數字
const validatePhone = () => {
  const phoneValue = profile.value.phone
  const phonePattern = /^\d*$/
  if (!phonePattern.test(phoneValue)) {
    $('#phoneFeedback').show()
    $('#phone').addClass('is-invalid')
  } else {
    $('#phoneFeedback').hide()
    $('#phone').removeClass('is-invalid').addClass('is-valid')
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
      // alert(response.data)
      toast.success('密碼修改成功', {
        theme: 'colored',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.BOTTOM_CENTER
      })
    })
    .catch((error) => {
      console.error('Error:', error.response) // 輸出錯誤信息
      // alert('密碼修改失敗: ' + error.response.data)
      toast.error('密碼修改失敗: ' + error.response.data.message, {
        theme: 'colored',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.BOTTOM_CENTER
      })
    })
}

// 加載用戶資料
const loadUserProfile = async () => {
  const token = store.getToken
  try {
    const response = await axios.get(`${apiUrl}/api/UserIdentity/GetUserInfoFromToken`, {
      headers: {
        Authorization: `Bearer ${token}`
      }
    })
    const userData = response.data

    const formatDateForInput = (dateString) => {
      if (!dateString) return ''
      const date = new Date(dateString)
      const year = date.getFullYear()
      const month = (date.getMonth() + 1).toString().padStart(2, '0')
      const day = date.getDate().toString().padStart(2, '0')
      return `${year}-${month}-${day}`
    }

    profile.value.name = userData.name
    profile.value.gender = userData.gender ? 'true' : 'false' // 確保性別以布林值形式處理
    profile.value.phone = userData.phone
    profile.value.birthday = formatDateForInput(userData.birthday) // 格式化日期為 yyyy-mm-dd
    profile.value.photo = userData.photo ? `${apiUrl}/images/headshots/${userData.photo}` : null
    profile.value.originalPhoto = userData.photo // 存放原本的圖片路徑
  } catch (error) {
    console.error('Error loading user profile:', error)
    // alert('加載用戶資料失敗')
    toast.error('讀取用戶資料失敗', {
        theme: 'colored',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.BOTTOM_CENTER
    })
  }
}

// 修改基本資料
const editUserSubmit = async () => {
  const formData = new FormData()
  formData.append('Name', profile.value.name)
  formData.append('Gender', profile.value.gender)
  formData.append('Phone', profile.value.phone)
  formData.append('Birthday', profile.value.birthday)
  if (profile.value.avatar) {
    formData.append('Photo', profile.value.avatar)
  }
  formData.append('OriginalPhoto', profile.value.originalPhoto) // 傳遞原本的圖片信息

  const token = store.getToken

  try {
    const response = await axios.put(`${apiUrl}/api/UserIdentity/EditMember`, formData, {
      headers: {
        Authorization: `Bearer ${token}`,
        'Content-Type': 'multipart/form-data'
      }
    })
    // alert(response.data)
    // toast.success(response.data.message, {
      toast.success('基本資料修改成功', {
        theme: 'colored',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.BOTTOM_CENTER
    })
  } catch (error) {
    console.error('Error:', error.response)
    // alert('修改失敗: ' + error.response.data)
    toast.error('基本資料修改失敗', {
        theme: 'colored',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.BOTTOM_CENTER
    })
  }
}

onMounted(() => {
  loadUserProfile()
})
</script>

<style scoped>
.list-group-container {
  margin-right: 50px; /* 可以根據需要調整數值 */
}

.img-thumbnail {
  border-radius: 50%;
  width: 200px; /* 可以根據需要調整大小 */
  height: 200px; /* 確保寬高相等，形成正方形 */
  object-fit: cover; /* 確保圖片能夠完整顯示在圓形內 */
}
</style>
