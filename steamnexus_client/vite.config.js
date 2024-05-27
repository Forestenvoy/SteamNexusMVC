import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import VueDevTools from 'vite-plugin-vue-devtools'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue(), VueDevTools()],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  },
  build: {
    // 小於 4KB 的資源將內聯為 base64
    assetsInlineLimit: 4096,
    chunkSizeWarningLimit: 1500
  },
  esbuild: {
    pure: ['console.log'],
    drop: ['debugger']
  }
})
