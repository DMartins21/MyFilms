import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import RoutesApp from './routes/route'
import { ToastContainer } from 'react-toastify'
import { ChakraProvider, defaultSystem } from "@chakra-ui/react"
import { ThemeProvider } from "next-themes"
import 'react-toastify/dist/ReactToastify.css'
import './index.css'
import App from './App.jsx'

createRoot(document.getElementById('root')).render(
  <StrictMode>
    <ChakraProvider value={defaultSystem}>
      <ThemeProvider attribute='class' disableTransitionOnChange>
      <ToastContainer />
      <RoutesApp />
      <App />
      </ThemeProvider>
    </ChakraProvider>
  </StrictMode>,
)