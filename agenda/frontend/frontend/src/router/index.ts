import { createRouter, createWebHistory } from 'vue-router'
import ContactsPage from '../views/ContactPage.vue'
import LoginPage from '../views/LoginPage.vue'
import { useAuth } from '../stores/auth'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/', redirect: '/contacts' },
    { path: '/login', component: LoginPage },
    { path: '/contacts', component: ContactsPage, meta: { requiresAuth: true } },
  ],
})

router.beforeEach((to) => {
  const auth = useAuth()
  if (!auth.token) auth.restore()
  if (to.meta.requiresAuth && !auth.token) return { path: '/login' }
})

export default router
