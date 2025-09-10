<script setup lang="ts">
import { RouterView, RouterLink, useRouter } from 'vue-router'
import { useAuth } from './stores/auth'

const auth = useAuth()
const router = useRouter()

function doLogout() {
  auth.logout()
  router.replace('/login') // garante sair da rota protegida
}
</script>

<template>
  <div class="container">
    <!-- Header (brand + ação de conta) -->
    <header class="header">
      <h1 class="title">Agenda</h1>
      <nav class="account">
        <RouterLink v-if="!auth.token" to="/login" class="btn ghost">Login</RouterLink>
        <button v-else class="btn danger" @click="doLogout">
          Sair ({{ auth.userEmail || 'usuário' }})
        </button>
      </nav>
    </header>

    <!-- Box de abas (separa navegação das páginas) -->
    <nav class="tabs-box">
      <RouterLink v-if="auth.token" to="/contacts" class="tab">Contatos</RouterLink>
      <RouterLink v-if="!auth.token" to="/login" class="tab">Login</RouterLink>
    </nav>

    <!-- Conteúdo -->
    <main class="main">
      <RouterView />
    </main>
  </div>
</template>

<style>
/* ---------- DARK THEME GLOBAL ---------- */
:root{
  --bg: #0b1020;
  --fg: #e6e6f0;
  --muted:#9aa3b2;
  --brand:#6ea8ff;
  --brand-weak: rgba(110,168,255,.16);
  --border:#1c2333;
  --panel:#0f1629;
  --panel-2:#0c1222;
  --ring: rgba(110,168,255,.22);
  --shadow: 0 10px 30px rgba(0,0,0,.35);
}

html, body, #app {
  height: 100%;
  background: radial-gradient(1200px 500px at -10% -20%, #0e1a33, transparent 55%),
              radial-gradient(900px 400px at 120% -40%, #0e1a33, transparent 55%),
              var(--bg);
  color: var(--fg);
  font-family: ui-sans-serif, system-ui, -apple-system, Segoe UI, Roboto, Inter, "Helvetica Neue", Arial;
}

/* ---------- Layout ---------- */
.container {
  max-width: 1040px;
  margin: 0 auto;
  padding: 22px clamp(16px, 4vw, 28px);
}

/* Header “glass” */
.header{
  position: sticky; top: 12px; z-index: 10;
  display: flex; align-items: center; justify-content: space-between; gap: 16px;
  padding: 14px 18px;
  background: linear-gradient(180deg, rgba(255,255,255,.04), rgba(255,255,255,.02));
  background-color: var(--panel);
  border: 1px solid var(--border);
  border-radius: 16px;
  box-shadow: var(--shadow);
  backdrop-filter: saturate(150%) blur(8px);
}
.title{ margin: 0; font-size: clamp(22px, 2.2vw, 30px); letter-spacing:.2px; }
.account{ display:flex; gap:10px; }

/* ---------- Tabs box (separa navegação) ---------- */
.tabs-box{
  margin: 16px 0 22px;
  padding: 10px;
  border: 1px solid var(--border);
  border-radius: 14px;
  background: var(--panel-2);
  display: flex; gap: 8px; flex-wrap: wrap;
  box-shadow: var(--shadow);
}
.tab{
  position: relative;
  display: inline-flex; align-items: center; justify-content: center;
  padding: 9px 14px;
  border-radius: 10px;
  text-decoration: none;
  color: var(--fg);
  font-weight: 700; font-size: 14px;
  border: 1px solid transparent;
  transition: background .2s ease, color .2s ease, border-color .2s ease;
}
.tab:hover{ background: var(--brand-weak); color: var(--brand); border-color: var(--border); }
/* ativa por padrão do vue-router */
.tab.router-link-active{
  background: var(--brand-weak);
  color: var(--brand);
  border-color: color-mix(in oklab, var(--brand) 40%, var(--border));
  box-shadow: 0 0 0 3px var(--ring) inset;
}

/* ---------- Main (card grande) ---------- */
.main{
  min-height: 60vh;
  border: 1px solid var(--border);
  border-radius: 16px;
  background: linear-gradient(180deg, rgba(255,255,255,.02), rgba(255,255,255,.01));
  background-color: var(--panel);
  padding: clamp(16px, 3.2vw, 24px);
  box-shadow: var(--shadow);
}

/* ---------- Botões ---------- */
.btn{
  appearance:none; cursor:pointer;
  font-weight: 800; font-size: 14px;
  padding: 9px 12px; border-radius: 10px;
  border: 1px solid var(--border);
  color: var(--fg);
  background: linear-gradient(180deg, rgba(255,255,255,.06), rgba(255,255,255,.02));
  transition: transform .04s ease, filter .2s ease, background .2s ease, border-color .2s ease;
}
.btn:hover{ filter: brightness(1.08); border-color: color-mix(in oklab, var(--brand) 40%, var(--border)); }
.btn:active{ transform: translateY(1px); }
.btn:disabled{ opacity:.6; cursor:not-allowed; }

/* variantes */
.btn.ghost{
  background: transparent;
  border-color: var(--border);
}
.btn.danger{
  border-color: #3a1020;
  color: #ff96b0;
  background: linear-gradient(180deg, rgba(255,43,72,.12), rgba(255,43,72,.06));
}
.btn.danger:hover{
  filter: none;
  border-color: #5c1930;
  background: linear-gradient(180deg, rgba(255,43,72,.18), rgba(255,43,72,.10));
}

/* ---------- Responsivo ---------- */
@media (max-width: 640px){
  .header{ padding: 12px 14px; border-radius: 14px; top: 8px; }
  .tabs-box{ padding: 8px; border-radius: 12px; }
}
</style>
