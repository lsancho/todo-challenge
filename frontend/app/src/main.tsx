import React from 'react'
import ErrorFallback from '@/components/ErrorFallback.tsx'
import ReactDOM from 'react-dom/client'
import { ErrorBoundary } from 'react-error-boundary'
import App from './App.tsx'
import './main.css'
import { SWRConfig } from 'swr'

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <ErrorBoundary FallbackComponent={ErrorFallback}>
      <SWRConfig
        value={{
          onErrorRetry: (error, _key, _config, revalidate, { retryCount }) => {
            // Never retry on 404.
            if (error.status === 404) return

            // Only retry up to 10 times.
            if (retryCount >= 5) return

            // Retry after 5 seconds.
            setTimeout(() => revalidate({ retryCount }), 5000)
          }
        }}
      >
        <App />
      </SWRConfig>
    </ErrorBoundary>
  </React.StrictMode>
)
