<script setup lang="ts">
import { ref, onMounted, watch, computed, onBeforeUnmount } from 'vue'
import { useAuth } from '@/stores/auth'
import { searchContacts, type Contact } from '@/services/contacts'

const auth = useAuth()
auth.restore()

const rows = ref<Contact[]>([])
const total = ref(0)
const term = ref('')
const loading = ref(false)

const page = ref(1)
const pageSize = 10
const totalPages = computed(() => Math.max(1, Math.ceil(total.value / pageSize)))

async function load(p = page.value) {
  loading.value = true
  try {
    const res = await searchContacts(term.value, p, pageSize)
    rows.value = res.items
    total.value = res.total
    page.value = p
  } finally {
    loading.value = false
  }
}

function next() { if (page.value < totalPages.value) load(page.value + 1) }
function prev() { if (page.value > 1) load(page.value - 1) }

// debounce da busca ao digitar
let t: number | undefined
watch(term, () => {
  window.clearTimeout(t)
  t = window.setTimeout(() => load(1), 300)
})

onBeforeUnmount(() => { if (t) window.clearTimeout(t) })
onMounted(() => load())
</script>

<template>
  <section class="contacts">
    <header class="header">
      <div>
        <h2 class="title">Contatos</h2>
        <p class="subtitle" v-if="!loading && total">Total: {{ total }}</p>
      </div>

      <div class="toolbar">
        <div class="search">
          <svg viewBox="0 0 24 24" class="icon"><path d="M10.5 3a7.5 7.5 0 0 1 6.004 12.06l4.218 4.217-1.414 1.415-4.217-4.218A7.5 7.5 0 1 1 10.5 3Zm0 2a5.5 5.5 0 1 0 0 11 5.5 5.5 0 0 0 0-11Z"/></svg>
          <input
            class="input"
            v-model="term"
            placeholder="Buscar por nome, email ou telefone…"
            :disabled="loading"
          />
        </div>
        <button class="btn" :disabled="loading" @click="load(1)">
          <span v-if="!loading">Buscar</span>
          <span v-else>Carregando…</span>
        </button>
      </div>
    </header>

    <div class="card">
      <!-- Tabela -->
      <div class="table-wrap" :class="{ loading }" v-if="!loading && rows.length">
        <table class="table">
          <thead>
            <tr>
              <th>Nome</th>
              <th>Email</th>
              <th>Telefone</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="c in rows" :key="c.id">
              <td>{{ c.name }}</td>
              <td class="mono">{{ c.email }}</td>
              <td class="mono">{{ c.phone }}</td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Skeleton de carregamento -->
      <div v-if="loading" class="skeleton">
        <div class="srow" v-for="i in 6" :key="i"></div>
      </div>

      <!-- Empty state -->
      <div v-if="!loading && rows.length === 0" class="empty">
        <svg viewBox="0 0 24 24" class="empty-icon">
          <path d="M3 6h18v12H3z" fill="none" stroke="currentColor" stroke-width="1.5"/>
          <path d="M7 9h10M7 12h7M7 15h5" stroke="currentColor" stroke-width="1.5"/>
        </svg>
        <p class="empty-title">Nenhum contato encontrado</p>
        <p class="empty-sub">Tente ajustar a busca ou limpar o termo digitado.</p>
      </div>

      <!-- Paginação -->
      <footer v-if="!loading && totalPages > 1" class="pagination">
        <button class="btn ghost" :disabled="page === 1" @click="prev">← Anterior</button>
        <span class="page-indicator">Página {{ page }} de {{ totalPages }}</span>
        <button class="btn ghost" :disabled="page === totalPages" @click="next">Próxima →</button>
      </footer>
    </div>
  </section>
</template>

<style scoped>
:root {
  --bg: #ffffff;
  --fg: #111827;
  --muted: #6b7280;
  --primary: #2563eb;
  --border: #e5e7eb;
  --ring: rgba(37,99,235,0.15);
  --card: #ffffff;
  --zebra: #fafafa;
}

.contacts { display: grid; gap: 14px; }

.header {
  display: flex; align-items: end; justify-content: space-between; gap: 12px;
}
.title { margin: 0; font-size: 24px; }
.subtitle { margin: 4px 0 0; color: var(--muted); font-size: 13px; }

.toolbar { display: flex; gap: 8px; align-items: center; }

.search { position: relative; }
.search .icon {
  position: absolute; left: 10px; top: 50%; transform: translateY(-50%);
  width: 18px; height: 18px; color: var(--muted);
}
.input {
  width: 320px; max-width: 60vw;
  padding: 10px 12px 10px 36px;
  border: 1px solid var(--border); border-radius: 10px; outline: none; font-size: 14px;
  background: #fff;
}
.input:focus { border-color: var(--primary); box-shadow: 0 0 0 4px var(--ring); }

.btn {
  appearance: none; border: 1px solid var(--border); background: #f8fafc;
  padding: 10px 14px; border-radius: 10px; cursor: pointer; font-weight: 600; font-size: 14px;
}
.btn:hover { background: #f3f4f6; }
.btn:disabled { opacity: .6; cursor: not-allowed; }
.btn.ghost { background: transparent; }

.card {
  border: 1px solid var(--border); border-radius: 14px; background: var(--card);
  overflow: hidden;
}

/* Tabela */
.table-wrap { overflow: auto; }
.table {
  width: 100%; border-collapse: collapse; font-size: 14px;
}
.table thead tr { background: #f8fafc; }
.table th, .table td { padding: 12px 14px; text-align: left; border-bottom: 1px solid var(--border); }
.table tbody tr:nth-child(odd) { background: var(--zebra); }
.table tbody tr:hover { background: #eef2ff; }
.mono { font-family: ui-monospace, SFMono-Regular, Menlo, Monaco, Consolas, "Liberation Mono", monospace; }

/* Skeleton */
.skeleton { padding: 12px; }
.srow {
  height: 44px; border-radius: 8px; margin-bottom: 10px;
  background: linear-gradient(90deg, #f3f4f6 25%, #e5e7eb 37%, #f3f4f6 63%);
  background-size: 400% 100%; animation: shimmer 1.2s infinite linear;
}
@keyframes shimmer { 0% { background-position: 100% 0; } 100% { background-position: -100% 0; } }

/* Empty state */
.empty { text-align: center; padding: 40px 16px; color: var(--muted); }
.empty-icon { width: 44px; height: 44px; margin-bottom: 10px; }
.empty-title { margin: 0 0 4px; color: var(--fg); font-weight: 600; }
.empty-sub { margin: 0; }

/* Paginação */
.pagination {
  display: flex; justify-content: space-between; align-items: center; gap: 10px;
  padding: 12px 14px; background: #f8fafc; border-top: 1px solid var(--border);
}
.page-indicator { color: var(--muted); font-size: 13px; }

/* Responsivo */
@media (max-width: 640px) {
  .header { flex-direction: column; align-items: stretch; }
  .toolbar { justify-content: space-between; }
  .input { width: 100%; max-width: none; }
}
</style>
