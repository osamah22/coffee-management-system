import { ArrowRight, LayoutDashboard, LogOut, Menu, X } from 'lucide-react'
import { useState } from 'react'
import { Link, NavLink } from 'react-router-dom'
import { useAuth } from '../auth/AuthContext'
import { Brand } from './Brand'

export function Header() {
  const { isAuthenticated, isManager, login, logout, user } = useAuth()
  const [open, setOpen] = useState(false)
  const firstName = user?.profile.given_name || user?.profile.preferred_username || 'Manager'

  return (
    <header className="site-header">
      <div className="shell header__inner">
        <Brand />
        <button className="icon-button nav-toggle" onClick={() => setOpen((value) => !value)} aria-label="Toggle navigation">{open ? <X /> : <Menu />}</button>
        <nav className={`main-nav ${open ? 'main-nav--open' : ''}`} onClick={() => setOpen(false)}>
          <NavLink to="/">Collection</NavLink><a href="/#story">Our story</a>
          {isManager && <NavLink to="/manage"><LayoutDashboard size={16} /> Manage</NavLink>}
          {isAuthenticated ? <div className="account-actions"><Link className="avatar" to={isManager ? '/manage' : '/'}>{String(firstName).charAt(0).toUpperCase()}</Link><button className="button button--ghost button--small" onClick={() => void logout()}><LogOut size={16} /> Sign out</button></div> : <button className="button button--dark button--small" onClick={() => void login()}>Manager login <ArrowRight size={16} /></button>}
        </nav>
      </div>
    </header>
  )
}
