import { defineConfig, loadEnv } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '')

  return {
    plugins: [react()],
    server: {
      port: 5173,
      proxy: {
        '/api': {
          target: env.VITE_API_PROXY_TARGET || 'http://localhost:5231',
          changeOrigin: true,
        },
        '/keycloak': {
          target: env.VITE_KEYCLOAK_URL || 'http://localhost:8080',
          changeOrigin: true,
          rewrite: (path) => path.replace(/^\/keycloak/, ''),
          configure: (proxy) => {
            // Keycloak rejects browser token requests when the client's Web
            // Origins are not configured. This development-only same-origin
            // proxy keeps the exchange local without weakening the client.
            proxy.on('proxyReq', (proxyRequest) => proxyRequest.removeHeader('origin'))
          },
        },
      },
    },
  }
})
