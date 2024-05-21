// vite.config.js
import { fileURLToPath, URL } from "node:url";
import { defineConfig } from "file:///C:/Users/iSpan/Desktop/SteamNexus/steamnexus_client/node_modules/vite/dist/node/index.js";
import vue from "file:///C:/Users/iSpan/Desktop/SteamNexus/steamnexus_client/node_modules/@vitejs/plugin-vue/dist/index.mjs";
import VueDevTools from "file:///C:/Users/iSpan/Desktop/SteamNexus/steamnexus_client/node_modules/vite-plugin-vue-devtools/dist/vite.mjs";
import path from "path";
import { dirname } from "path";
var __vite_injected_original_import_meta_url = "file:///C:/Users/iSpan/Desktop/SteamNexus/steamnexus_client/vite.config.js";
var __dirname = dirname(fileURLToPath(__vite_injected_original_import_meta_url));
var vite_config_default = defineConfig({
  plugins: [vue(), VueDevTools()],
  resolve: {
    alias: {
      "@": fileURLToPath(new URL("./src", __vite_injected_original_import_meta_url)),
      "datatables.net-dt": path.resolve(__dirname, "node_modules/datatables.net-dt"),
      "datatables.net-fixedheader-dt": path.resolve(
        __dirname,
        "node_modules/datatables.net-fixedheader-dt"
      ),
      "datatables.net-rowgroup-dt": path.resolve(
        __dirname,
        "node_modules/datatables.net-rowgroup-dt"
      ),
      "datatables.net-buttons-dt": path.resolve(
        __dirname,
        "node_modules/datatables.net-buttons-dt"
      ),
      "datatables.net-responsive-dt": path.resolve(
        __dirname,
        "node_modules/datatables.net-responsive-dt"
      )
    }
  },
  build: {
    rollupOptions: {
      external: [
        "datatables.net-dt/css/dataTables.datatables.min.css",
        "datatables.net-fixedheader-dt/css/fixedHeader.dataTables.min.css",
        "datatables.net-rowgroup-dt/css/rowGroup.dataTables.min.css",
        "datatables.net-buttons-dt/css/buttons.dataTables.min.css",
        "datatables.net-responsive-dt/css/responsive.dataTables.min.css"
      ]
    }
  }
});
export {
  vite_config_default as default
};
//# sourceMappingURL=data:application/json;base64,ewogICJ2ZXJzaW9uIjogMywKICAic291cmNlcyI6IFsidml0ZS5jb25maWcuanMiXSwKICAic291cmNlc0NvbnRlbnQiOiBbImNvbnN0IF9fdml0ZV9pbmplY3RlZF9vcmlnaW5hbF9kaXJuYW1lID0gXCJDOlxcXFxVc2Vyc1xcXFxpU3BhblxcXFxEZXNrdG9wXFxcXFN0ZWFtTmV4dXNcXFxcc3RlYW1uZXh1c19jbGllbnRcIjtjb25zdCBfX3ZpdGVfaW5qZWN0ZWRfb3JpZ2luYWxfZmlsZW5hbWUgPSBcIkM6XFxcXFVzZXJzXFxcXGlTcGFuXFxcXERlc2t0b3BcXFxcU3RlYW1OZXh1c1xcXFxzdGVhbW5leHVzX2NsaWVudFxcXFx2aXRlLmNvbmZpZy5qc1wiO2NvbnN0IF9fdml0ZV9pbmplY3RlZF9vcmlnaW5hbF9pbXBvcnRfbWV0YV91cmwgPSBcImZpbGU6Ly8vQzovVXNlcnMvaVNwYW4vRGVza3RvcC9TdGVhbU5leHVzL3N0ZWFtbmV4dXNfY2xpZW50L3ZpdGUuY29uZmlnLmpzXCI7aW1wb3J0IHsgZmlsZVVSTFRvUGF0aCwgVVJMIH0gZnJvbSAnbm9kZTp1cmwnXHJcblxyXG5pbXBvcnQgeyBkZWZpbmVDb25maWcgfSBmcm9tICd2aXRlJ1xyXG5pbXBvcnQgdnVlIGZyb20gJ0B2aXRlanMvcGx1Z2luLXZ1ZSdcclxuaW1wb3J0IFZ1ZURldlRvb2xzIGZyb20gJ3ZpdGUtcGx1Z2luLXZ1ZS1kZXZ0b29scydcclxuaW1wb3J0IHBhdGggZnJvbSAncGF0aCdcclxuaW1wb3J0IHsgZGlybmFtZSB9IGZyb20gJ3BhdGgnXHJcbmNvbnN0IF9fZGlybmFtZSA9IGRpcm5hbWUoZmlsZVVSTFRvUGF0aChpbXBvcnQubWV0YS51cmwpKVxyXG5cclxuLy8gaHR0cHM6Ly92aXRlanMuZGV2L2NvbmZpZy9cclxuZXhwb3J0IGRlZmF1bHQgZGVmaW5lQ29uZmlnKHtcclxuICBwbHVnaW5zOiBbdnVlKCksIFZ1ZURldlRvb2xzKCldLFxyXG4gIHJlc29sdmU6IHtcclxuICAgIGFsaWFzOiB7XHJcbiAgICAgICdAJzogZmlsZVVSTFRvUGF0aChuZXcgVVJMKCcuL3NyYycsIGltcG9ydC5tZXRhLnVybCkpLFxyXG4gICAgICAnZGF0YXRhYmxlcy5uZXQtZHQnOiBwYXRoLnJlc29sdmUoX19kaXJuYW1lLCAnbm9kZV9tb2R1bGVzL2RhdGF0YWJsZXMubmV0LWR0JyksXHJcbiAgICAgICdkYXRhdGFibGVzLm5ldC1maXhlZGhlYWRlci1kdCc6IHBhdGgucmVzb2x2ZShcclxuICAgICAgICBfX2Rpcm5hbWUsXHJcbiAgICAgICAgJ25vZGVfbW9kdWxlcy9kYXRhdGFibGVzLm5ldC1maXhlZGhlYWRlci1kdCdcclxuICAgICAgKSxcclxuICAgICAgJ2RhdGF0YWJsZXMubmV0LXJvd2dyb3VwLWR0JzogcGF0aC5yZXNvbHZlKFxyXG4gICAgICAgIF9fZGlybmFtZSxcclxuICAgICAgICAnbm9kZV9tb2R1bGVzL2RhdGF0YWJsZXMubmV0LXJvd2dyb3VwLWR0J1xyXG4gICAgICApLFxyXG4gICAgICAnZGF0YXRhYmxlcy5uZXQtYnV0dG9ucy1kdCc6IHBhdGgucmVzb2x2ZShcclxuICAgICAgICBfX2Rpcm5hbWUsXHJcbiAgICAgICAgJ25vZGVfbW9kdWxlcy9kYXRhdGFibGVzLm5ldC1idXR0b25zLWR0J1xyXG4gICAgICApLFxyXG4gICAgICAnZGF0YXRhYmxlcy5uZXQtcmVzcG9uc2l2ZS1kdCc6IHBhdGgucmVzb2x2ZShcclxuICAgICAgICBfX2Rpcm5hbWUsXHJcbiAgICAgICAgJ25vZGVfbW9kdWxlcy9kYXRhdGFibGVzLm5ldC1yZXNwb25zaXZlLWR0J1xyXG4gICAgICApXHJcbiAgICB9XHJcbiAgfSxcclxuICBidWlsZDoge1xyXG4gICAgcm9sbHVwT3B0aW9uczoge1xyXG4gICAgICBleHRlcm5hbDogW1xyXG4gICAgICAgICdkYXRhdGFibGVzLm5ldC1kdC9jc3MvZGF0YVRhYmxlcy5kYXRhdGFibGVzLm1pbi5jc3MnLFxyXG4gICAgICAgICdkYXRhdGFibGVzLm5ldC1maXhlZGhlYWRlci1kdC9jc3MvZml4ZWRIZWFkZXIuZGF0YVRhYmxlcy5taW4uY3NzJyxcclxuICAgICAgICAnZGF0YXRhYmxlcy5uZXQtcm93Z3JvdXAtZHQvY3NzL3Jvd0dyb3VwLmRhdGFUYWJsZXMubWluLmNzcycsXHJcbiAgICAgICAgJ2RhdGF0YWJsZXMubmV0LWJ1dHRvbnMtZHQvY3NzL2J1dHRvbnMuZGF0YVRhYmxlcy5taW4uY3NzJyxcclxuICAgICAgICAnZGF0YXRhYmxlcy5uZXQtcmVzcG9uc2l2ZS1kdC9jc3MvcmVzcG9uc2l2ZS5kYXRhVGFibGVzLm1pbi5jc3MnXHJcbiAgICAgIF1cclxuICAgIH1cclxuICB9XHJcbn0pXHJcbiJdLAogICJtYXBwaW5ncyI6ICI7QUFBdVYsU0FBUyxlQUFlLFdBQVc7QUFFMVgsU0FBUyxvQkFBb0I7QUFDN0IsT0FBTyxTQUFTO0FBQ2hCLE9BQU8saUJBQWlCO0FBQ3hCLE9BQU8sVUFBVTtBQUNqQixTQUFTLGVBQWU7QUFOaU0sSUFBTSwyQ0FBMkM7QUFPMVEsSUFBTSxZQUFZLFFBQVEsY0FBYyx3Q0FBZSxDQUFDO0FBR3hELElBQU8sc0JBQVEsYUFBYTtBQUFBLEVBQzFCLFNBQVMsQ0FBQyxJQUFJLEdBQUcsWUFBWSxDQUFDO0FBQUEsRUFDOUIsU0FBUztBQUFBLElBQ1AsT0FBTztBQUFBLE1BQ0wsS0FBSyxjQUFjLElBQUksSUFBSSxTQUFTLHdDQUFlLENBQUM7QUFBQSxNQUNwRCxxQkFBcUIsS0FBSyxRQUFRLFdBQVcsZ0NBQWdDO0FBQUEsTUFDN0UsaUNBQWlDLEtBQUs7QUFBQSxRQUNwQztBQUFBLFFBQ0E7QUFBQSxNQUNGO0FBQUEsTUFDQSw4QkFBOEIsS0FBSztBQUFBLFFBQ2pDO0FBQUEsUUFDQTtBQUFBLE1BQ0Y7QUFBQSxNQUNBLDZCQUE2QixLQUFLO0FBQUEsUUFDaEM7QUFBQSxRQUNBO0FBQUEsTUFDRjtBQUFBLE1BQ0EsZ0NBQWdDLEtBQUs7QUFBQSxRQUNuQztBQUFBLFFBQ0E7QUFBQSxNQUNGO0FBQUEsSUFDRjtBQUFBLEVBQ0Y7QUFBQSxFQUNBLE9BQU87QUFBQSxJQUNMLGVBQWU7QUFBQSxNQUNiLFVBQVU7QUFBQSxRQUNSO0FBQUEsUUFDQTtBQUFBLFFBQ0E7QUFBQSxRQUNBO0FBQUEsUUFDQTtBQUFBLE1BQ0Y7QUFBQSxJQUNGO0FBQUEsRUFDRjtBQUNGLENBQUM7IiwKICAibmFtZXMiOiBbXQp9Cg==
