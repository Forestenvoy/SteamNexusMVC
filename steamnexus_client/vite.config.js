import { fileURLToPath, URL } from 'node:url'

import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import VueDevTools from 'vite-plugin-vue-devtools'
// import path from 'path'
// import { dirname } from 'path'
// const __dirname = dirname(fileURLToPath(import.meta.url))

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue(), VueDevTools()],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
      // 'datatables.net-dt': path.resolve(__dirname, 'node_modules/datatables.net-dt'),
      // 'datatables.net-fixedheader-dt': path.resolve(
      //   __dirname,
      //   'node_modules/datatables.net-fixedheader-dt'
      // ),
      // 'datatables.net-rowgroup-dt': path.resolve(
      //   __dirname,
      //   'node_modules/datatables.net-rowgroup-dt'
      // ),
      // 'datatables.net-buttons-dt': path.resolve(
      //   __dirname,
      //   'node_modules/datatables.net-buttons-dt'
      // ),
      // 'datatables.net-responsive-dt': path.resolve(
      //   __dirname,
      //   'node_modules/datatables.net-responsive-dt'
      // )
    }
  },
  build: {
    // 小於 4KB 的資源將內聯為 base64
    assetsInlineLimit: 4096,
    chunkSizeWarningLimit: 1500
    // rollupOptions: {
    //   external: [
    //     'datatables.net-dt/css/dataTables.datatables.min.css',
    //     'datatables.net-fixedheader-dt/css/fixedHeader.dataTables.min.css',
    //     'datatables.net-rowgroup-dt/css/rowGroup.dataTables.min.css',
    //     'datatables.net-buttons-dt/css/buttons.dataTables.min.css',
    //     'datatables.net-responsive-dt/css/responsive.dataTables.min.css'
    //   ]
    // }
  }
})
