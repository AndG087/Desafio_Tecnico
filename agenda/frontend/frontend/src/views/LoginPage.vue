<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuth } from '@/stores/auth'

const email = ref('admin@local')
const password = ref('Admin@123')
const loading = ref(false)
const errorMsg = ref<string | null>(null)
const successMsg = ref<string | null>(null)

const auth = useAuth()
const router = useRouter()

async function doLogin() {
  loading.value = true
  errorMsg.value = null
  successMsg.value = null
  try {
    await auth.login(email.value, password.value)
    successMsg.value = 'Logado com sucesso.'
    setTimeout(() => router.push('/contacts'), 200)
  } catch (e: any) {
    errorMsg.value = e?.response?.data?.message ?? 'Credenciais inválidas'
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <section class="card card-sm center">
    <h2 class="card-title">Login</h2>

    <p v-if="errorMsg" class="alert alert-error">{{ errorMsg }}</p>
    <p v-if="successMsg" class="alert alert-success">{{ successMsg }}</p>

    <label class="field">
      <span class="label">Email</span>
      <input class="input" type="email" v-model="email" placeholder="admin@local" />
    </label>

    <label class="field">
      <span class="label">Senha</span>
      <input class="input" type="password" v-model="password" placeholder="••••••••" />
    </label>

    <button class="btn primary" :disabled="loading" @click="doLogin">
      <span v-if="!loading">Entrar</span>
      <span v-else>Entrando…</span>
    </button>
  </section>
</template>

<style scoped>
/* Usa as variáveis globais definidas no App.vue (dark theme) */
.card{
  background: var(--panel);
  border: 1px solid var(--border);
  border-radius: 16px;
  padding: clamp(16px, 2.8vw, 22px);
  max-width: 420px; width: 100%; margin: 0 auto;
  display: grid; gap: 12px;
  box-shadow: var(--shadow);
  color: var(--fg);
}
.card-title{ margin: 0 0 8px; font-size: 20px; letter-spacing: .2px; }

/* Alerts */
.alert{
  padding: 10px 12px; border-radius: 10px; font-size: 14px;
  border: 1px solid var(--border);
}
.alert-error{
  background: linear-gradient(180deg, rgba(255,43,72,.12), rgba(255,43,72,.06));
  color: #ff96b0; border-color: #5c1930;
}
.alert-success{
  background: linear-gradient(180deg, rgba(16,185,129,.14), rgba(16,185,129,.08));
  color: #9be4c8; border-color: #1a3e34;
}

/* Campos */
.field{ display: grid; gap: 6px; }
.label{ font-size: 13px; color: var(--muted); }

.input{
  width: 100%;
  padding: 10px 12px;
  border: 1px solid var(--border);
  border-radius: 10px;
  outline: none;
  font-size: 14px;
  color: var(--fg);
  background: var(--panel-2);
}
.input::placeholder{
  color: color-mix(in oklab, var(--muted) 80%, var(--fg));
}
.input:focus{
  border-color: color-mix(in oklab, var(--brand) 70%, var(--border));
  box-shadow: 0 0 0 4px var(--ring);
}

/* Botões */
.btn{
  appearance: none; cursor: pointer;
  border: 1px solid var(--border);
  background: linear-gradient(180deg, rgba(255,255,255,.06), rgba(255,255,255,.02));
  color: var(--fg);
  padding: 10px 14px; border-radius: 10px;
  font-weight: 800; font-size: 14px;
  transition: transform .04s ease, filter .2s ease, border-color .2s ease, background .2s ease;
}
.btn:hover{
  filter: brightness(1.08);
  border-color: color-mix(in oklab, var(--brand) 40%, var(--border));
}
.btn:active{ transform: translateY(1px); }
.btn:disabled{ opacity: .6; cursor: not-allowed; }

.btn.primary{
  background: linear-gradient(
    180deg,
    color-mix(in oklab, var(--brand) 92%, #000),
    color-mix(in oklab, var(--brand) 70%, #000)
  );
  border-color: color-mix(in oklab, var(--brand) 80%, #000);
  color: #fff;
}
.btn.primary:hover{ filter: brightness(1.05); }
</style>
