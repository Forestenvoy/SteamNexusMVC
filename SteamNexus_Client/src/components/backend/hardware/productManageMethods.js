/* 硬體產品管理 */

import { ref } from 'vue'

// 從環境變數取得 API BASE URL
const apiUrl = import.meta.env.VITE_APP_API_BASE_URL

// 宣告硬體選單陣列
const HardwareSelect = ref([])
// 取得硬體選單資料
export function getHardwareSelect() {
  // 發送非同步 GET 請求
  fetch(`${apiUrl}/api/HardwareManage/GetComputerParts`, { method: 'GET' })
    .then((response) => {
      if (!response.ok) {
        throw new Error('Network response was not ok')
      }
      return response.json()
    })
    .then((data) => {
      HardwareSelect.value = data
    })
    .catch((error) => {
      console.error('There was a problem with the fetch operation:', error)
    })
}

// Toast 彈出式通知 元素初始化
export function ToastInitialization(id) {
  // loading
  let Toast_Status = `<div class="spinner-border spinner-border-sm animate__animated me-2" role="status" id="${id}_Toast_Status"></div>`
  // 成功 svg
  let ToastIcon_success = `<div class="ToastIcon-success d-flex justify-content-center align-items-center animate__animated me-2 d-none" id="${id}_ToastIcon_success"><i class="fa-solid fa-check"></i></div>`
  // 失敗 svg
  let ToastIcon_fail = `<div class="ToastIcon-fail d-flex justify-content-center align-items-center animate__animated me-2 d-none" id="${id}_ToastIcon_fail"><i class="fa-solid fa-xmark"></i></div>`
  // 吐司文字
  let ToastText = `<span style="margin-top: 2px" id="${id}_ToastText">請求傳送中</span>`
  // 吐司進度條
  let ToastProgress = `<svg width="250px" height="2px" class="position-absolute bottom-0 start-0" style="display: none" id="${id}_ToastProgress"><rect width="250px" height="2px" style="fill: #00FF00" id="${id}_ToastProgress_rect"></rect></svg>`
  // 吐司容器
  let ToastElement = `<div class="toast align-items-center mb-3 animate__animated animate__fadeInLeft defaultToast" role="alert" aria-live="assertive" aria-atomic="true" data-bs-autohide="false" id="${id}_Toast"><div class="d-flex align-items-center"><div class="toast-body d-flex align-items-center position-relative">${Toast_Status}${ToastIcon_success}${ToastIcon_fail}${ToastText}${ToastProgress}</div></div></div>`
  // 回傳 吐司元素
  return ToastElement
}

export { HardwareSelect }
