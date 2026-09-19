import { Navigate, Route, Routes } from 'react-router-dom'
import { AuthCallback } from './pages/AuthCallback'
import { HomePage } from './pages/HomePage'
import { ManagerPage } from './pages/ManagerPage'

export default function App() {
  return <Routes><Route path="/" element={<HomePage />} /><Route path="/manage" element={<ManagerPage />} /><Route path="/auth/callback" element={<AuthCallback />} /><Route path="*" element={<Navigate to="/" replace />} /></Routes>
}
