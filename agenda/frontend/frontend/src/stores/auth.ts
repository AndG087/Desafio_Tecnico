import { defineStore } from 'pinia'
import axios from '@/services/http'

const DEV_BYPASS = true // true no `vite dev`

export const useAuth = defineStore('auth', {
  state: () => ({
    token: '' as string,
    userEmail: '' as string,
  }),
  actions: {
    async login(email: string, password: string) {
      if (DEV_BYPASS) {
        // bypass de desenvolvimento
        this.token = 'dev-token'
        this.userEmail = email
        axios.defaults.headers.common['Authorization'] = `Bearer ${this.token}`
        localStorage.setItem('token', this.token)
        localStorage.setItem('userEmail', email)
        return
      }
      // fluxo real
      const { data } = await axios.post('/api/auth/login', { email, password })
      this.token = data.token
      this.userEmail = email
      axios.defaults.headers.common['Authorization'] = `Bearer ${this.token}`
      localStorage.setItem('token', this.token)
      localStorage.setItem('userEmail', email)
    },
    restore() {
      const t = localStorage.getItem('token')
      const e = localStorage.getItem('userEmail') || ''
      if (t) {
        this.token = t
        axios.defaults.headers.common['Authorization'] = `Bearer ${t}`
      }
      this.userEmail = e
    },
    logout() {
      this.token = ''
      this.userEmail = ''
      localStorage.removeItem('token')
      localStorage.removeItem('userEmail')
      delete axios.defaults.headers.common['Authorization']
    }
  }
})
