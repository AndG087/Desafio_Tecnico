import axios from 'axios'

axios.defaults.baseURL = '/'
const token = localStorage.getItem('token')
if (token) {
  axios.defaults.headers.common['Authorization'] = `Bearer ${token}`
}

const instance = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:3000'
})

export default instance
export { axios }  // ✅ exportação nomeada correta
