import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import VueDevTools from 'vite-plugin-vue-devtools'
import path from 'path'
import { dirname } from 'path'
const __dirname = dirname(fileURLToPath(import.meta.url))

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue(), VueDevTools()],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url)),
      'datatables.net-dt': path.resolve(__dirname, 'node_modules/datatables.net-dt'),
      'datatables.net-fixedheader-dt': path.resolve(
        __dirname,
        'node_modules/datatables.net-fixedheader-dt'
      ),
      'datatables.net-rowgroup-dt': path.resolve(
        __dirname,
        'node_modules/datatables.net-rowgroup-dt'
      ),
      'datatables.net-buttons-dt': path.resolve(
        __dirname,
        'node_modules/datatables.net-buttons-dt'
      ),
      'datatables.net-responsive-dt': path.resolve(
        __dirname,
        'node_modules/datatables.net-responsive-dt'
      )
    }
  }
})
