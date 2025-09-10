import axios from './http'

export interface Contact { id: string; name: string; email: string; phone: string }
export interface Paged<T> { items: T[]; total: number }

export async function searchContacts(term = '', page = 1, size = 10) {
  const { data } = await axios.get<Paged<Contact>>('/api/contacts', {
    params: { term, page, size }
  })
  return data
}
