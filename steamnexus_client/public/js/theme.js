const userMode = localStorage.getItem('coreui-free-vue-admin-template-theme')
const systemDarkMode =
  window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches
if (userMode === 'dark' || (userMode !== 'light' && systemDarkMode)) {
  document.documentElement.dataset.coreuiTheme = 'dark'
}
