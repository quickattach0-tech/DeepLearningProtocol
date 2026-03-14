import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    proxy: {
      // Proxy API calls to the ASP.NET backend
      '/chatHub': 'http://localhost:5000',
      '/api': 'http://localhost:5000'
    }
  }
});
