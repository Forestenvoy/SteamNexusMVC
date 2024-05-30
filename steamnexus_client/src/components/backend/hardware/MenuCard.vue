<template>
  <CCol
    sm="12"
    md="6"
    lg="4"
    xl="3"
    style="height: 350px"
    class="d-flex justify-content-center align-items-center"
  >
    <div class="nft">
      <div class="main">
        <h2 class="ms-1">{{ props.menuName }}</h2>
        <p class="description h2">$ {{ props.menuPrice }}</p>
        <p class="description mb-3">總共 {{ props.menuCount }} 個零件</p>
        <div class="tokenInfo">
          <div class="status">
            <p>{{ props.menuStatus ? '正常' : '異常' }}</p>
          </div>
          <div class="duration">
            <CFormCheck
              id="ActiveCheck"
              label="上架"
              class="mb-2"
              v-model="menuActive"
              @change="onMenuActive"
            />
          </div>
        </div>
        <hr />
        <div class="creator d-flex justify-content-evenly">
          <button role="button" class="button-59" @click="onMenuEdit">編輯</button>
          <button role="button" class="button-59 button-59-del" @click="onMenuDelete">刪除</button>
        </div>
      </div>
    </div>
  </CCol>
</template>

<script setup>
import { CCol } from '@coreui/vue'
import { ref } from 'vue'
import { toast } from 'vue3-toastify'
import 'vue3-toastify/dist/index.css'

// 身份驗證
import { useIdentityStore } from '@/stores/identity.js'
const authStore = useIdentityStore()

// 從環境變數取得 API BASE URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

const props = defineProps({
  menuId: Number,
  menuName: String,
  menuPrice: Number,
  menuWattage: Number,
  menuCount: Number,
  menuStatus: Boolean,
  menuActive: Boolean
})

const emits = defineEmits(['menuDelete', 'menuEdit'])

let menuActive = ref(props.menuActive)

// 編輯資料
const onMenuEdit = () => {
  emits('menuEdit', props.menuId, props.menuName, props.menuPrice, props.menuWattage)
}

// 上下架切換
const onMenuActive = () => {
  fetch(`${apiUrl}/api/HardwareManage/MenuActive`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${authStore.getToken}`
    },
    body: JSON.stringify({
      MenuId: props.menuId
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

// 刪除資料
const onMenuDelete = () => {
  fetch(`${apiUrl}/api/HardwareManage/DeleteMenu`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${authStore.getToken}`
    },
    body: JSON.stringify({
      MenuId: props.menuId
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
      emits('menuDelete', props.menuId)
      toast.success(data, {
        theme: 'dark',
        autoClose: 1000,
        transition: toast.TRANSITIONS.ZOOM,
        position: toast.POSITION.TOP_CENTER
      })
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
</script>

<style scoped>
.button-59 {
  align-items: center;
  background-color: #fff;
  border: 2px solid #000;
  box-sizing: border-box;
  color: #000;
  cursor: pointer;
  display: inline-flex;
  fill: #000;
  font-family: Inter, sans-serif;
  font-size: 16px;
  font-weight: 600;
  height: 48px;
  justify-content: center;
  letter-spacing: -0.8px;
  line-height: 24px;
  min-width: 100px;
  outline: 0;
  padding: 0 17px;
  text-align: center;
  text-decoration: none;
  transition: all 0.3s;
  user-select: none;
  -webkit-user-select: none;
  touch-action: manipulation;
}

.button-59:focus {
  color: #171e29;
}

.button-59:hover {
  border-color: #06f;
  background-color: #06f;
  color: #fff;
  fill: #06f;
}

.button-59-del:hover {
  border-color: red;
  background-color: red;
  color: #fff;
  fill: red;
}

.button-59:active {
  border-color: #06f;
  background-color: #06f;
  color: #fff;
  fill: #06f;
}

.button-59-del:active {
  border-color: red;
  background-color: red;
  color: #fff;
  fill: red;
}
</style>

<style lang="scss" scoped>
.nft {
  user-select: none;
  width: 300px;
  height: 285px;
  margin: 5rem auto;
  border: 1px solid #ffffff22;
  background-color: #282c34;
  background: linear-gradient(0deg, rgba(40, 44, 52, 1) 0%, rgba(0, 0, 0, 0.5) 100%);
  box-shadow: 0 7px 20px 5px #00000088;
  border-radius: 0.7rem;
  backdrop-filter: blur(7px);
  -webkit-backdrop-filter: blur(7px);
  overflow: hidden;
  transition: 0.5s all;
  hr {
    width: 100%;
    border: none;
    border-bottom: 1px solid #88888855;
    margin-top: 0;
  }
  ins {
    text-decoration: none;
  }
  .main {
    display: flex;
    flex-direction: column;
    width: 100%;
    padding: 1rem;
    .tokenImage {
      border-radius: 0.5rem;
      max-width: 100%;
      height: 250px;
      object-fit: cover;
    }
    .description {
      margin: 0.5rem 0;
      color: white;
    }
    .tokenInfo {
      display: flex;
      justify-content: space-between;
      align-items: center;
      .status {
        display: flex;
        align-items: center;
        color: #4eed73;
        font-weight: 700;
        ins {
          margin-left: -0.3rem;
          margin-right: 0.5rem;
        }
      }
      .duration {
        display: flex;
        align-items: center;
        color: gray;
        margin-right: 0.2rem;
        ins {
          margin: 0.5rem;
          margin-bottom: 0.4rem;
        }
      }
    }
    .creator {
      display: flex;
      align-items: center;
      margin-top: 0.2rem;
      margin-bottom: -0.3rem;
      ins {
        color: #a89ec9;
        text-decoration: none;
      }
      .wrapper {
        display: flex;
        align-items: center;
        border: 1px solid #ffffff22;
        padding: 0.3rem;
        margin: 0;
        margin-right: 0.5rem;
        border-radius: 100%;
        box-shadow: inset 0 0 0 4px #000000aa;
        img {
          border-radius: 100%;
          border: 1px solid #ffffff22;
          width: 2rem;
          height: 2rem;
          object-fit: cover;
          margin: 0;
        }
      }
    }
  }
  ::before {
    position: fixed;
    content: '';
    box-shadow: 0 0 100px 40px #ffffff08;
    top: -10%;
    left: -100%;
    transform: rotate(-45deg);
    height: 60rem;
    transition: 0.7s all;
  }
  &:hover {
    border: 1px solid #ffffff44;
    box-shadow: 0 7px 50px 10px #000000aa;
    transform: scale(1.015);
    filter: brightness(1.3);
    ::before {
      filter: brightness(0.5);
      top: -100%;
      left: 200%;
    }
  }
}
</style>
