import React from 'react'
import { createRoot } from 'react-dom/client'
// Update the import path and extension if the file is named 'TemplatesPage.tsx'
import { TemplatesPage } from './pages/TemplatesPage'
import './styles/global.css'

createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <TemplatesPage />
  </React.StrictMode>
)
