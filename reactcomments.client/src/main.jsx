import { StrictMode } from 'react'
import '@ant-design/v5-patch-for-react-19';
import { createRoot } from 'react-dom/client'
import { BrowserRouter } from 'react-router-dom';
import './index.css'
import App from './App.jsx'

createRoot(document.getElementById('root')).render(
  // <StrictMode>
  //   <App />
  // </StrictMode>,

  <BrowserRouter>
    <App />
  </BrowserRouter>
)
