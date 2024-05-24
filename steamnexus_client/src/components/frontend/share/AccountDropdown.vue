<template>
  <CDropdown placement="bottom-end" variant="nav-item">
    <CDropdownToggle class="py-0 pe-0" :caret="false">
      <CAvatar :src="avatar" size="md" />
    </CDropdownToggle>
    <CDropdownMenu class="pt-0">
      <CDropdownHeader
        component="h6"
        class="bg-body-secondary text-body-secondary fw-semibold mb-2 rounded-top text-center"
      >
        會員中心
      </CDropdownHeader>
      <CDropdownItem @click="$router.push('/userData')">
        <CIcon icon="cil-user" class="me-2" /> 會員資料變更
      </CDropdownItem>
      <CDropdownItem @click="$router.push('/trackedGames')">
        <CIcon icon="cilGamepad" class="me-2" /> 遊戲追蹤
      </CDropdownItem>
      <CDropdownDivider />
      <CDropdownItem @click="store.Logout()">
        <CIcon icon="cil-lock-locked" class="me-2" /> 登出
      </CDropdownItem>
    </CDropdownMenu>
  </CDropdown>
</template>

<script setup>
// 使用 Pinia，利用 store 去訪問狀態
import { useIdentityStore } from '@/stores/identity.js'
const store = useIdentityStore()

import avatar from '@/assets/images/avatars/2.jpg'



</script>
