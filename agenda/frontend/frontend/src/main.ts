import { createApp } from 'vue'
import App from './App.vue'
import { createPinia } from 'pinia'
import router from './router'

// PrimeVue
import PrimeVue from 'primevue/config'
import ToastService from 'primevue/toastservice'


// Restaura auth ANTES do router
import { useAuth } from './stores/auth'

const app = createApp(App)
const pinia = createPinia()
app.use(pinia)

// 🔑 restaura token antes do router (importante para guards/menus)
useAuth(pinia).restore()

// 🔌 registre PrimeVue e ToastService ANTES de montar
app.use(PrimeVue, { ripple: true })
app.use(ToastService)

app.use(router)
app.mount('#app')
