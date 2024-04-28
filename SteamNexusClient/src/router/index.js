import { createRouter, createWebHistory } from 'vue-router'
import Home from '@/views/Home.vue'
import About from '@/views/About.vue'
import Brazil from '@/views/Brazil.vue'

const routes = [
    { path: '/', name: 'Home', component: Home },
    { path: '/about', name: 'About', component: About },
    { path: '/brazil', name: 'brazil', component: Brazil },
]

const router = createRouter({ 
    history: createWebHistory(),
    routes
})

export default router