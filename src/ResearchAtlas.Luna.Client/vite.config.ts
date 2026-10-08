import { resolve } from 'node:path'
import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'
import { defineConfig } from 'vite'

// https://vite.dev/config/
export default defineConfig({
  plugins: [tailwindcss(), react()],
  server: {
    proxy: {
        '/api': 'http://localhost:5035',
    },
  },
  build: {
    outDir: resolve(import.meta.dirname, '../ResearchAtlas.Luna/wwwroot'),
    emptyOutDir: true,
  },
})
