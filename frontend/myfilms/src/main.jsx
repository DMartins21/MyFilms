import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import RoutesApp from './routes/route'
import './index.css'
// import App from './App.jsx'

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <RoutesApp />
  </StrictMode>,
)