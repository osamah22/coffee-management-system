import { createContext, useCallback, useContext, useEffect, useMemo, useState, type ReactNode } from 'react'
import type { User } from 'oidc-client-ts'
import { getRoles, userManager } from './keycloak'

interface AuthValue {
  user: User | null
  isLoading: boolean
  isAuthenticated: boolean
  isManager: boolean
  login: (returnTo?: string) => Promise<void>
  logout: () => Promise<void>
}

const AuthContext = createContext<AuthValue | null>(null)

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(null)
  const [isLoading, setIsLoading] = useState(true)

  useEffect(() => {
    let active = true
    void userManager.getUser().then((currentUser) => {
      if (active) setUser(currentUser?.expired ? null : currentUser)
    }).finally(() => {
      if (active) setIsLoading(false)
    })

    const onLoaded = (loadedUser: User) => setUser(loadedUser)
    const onUnloaded = () => setUser(null)
    userManager.events.addUserLoaded(onLoaded)
    userManager.events.addUserUnloaded(onUnloaded)
    return () => {
      active = false
      userManager.events.removeUserLoaded(onLoaded)
      userManager.events.removeUserUnloaded(onUnloaded)
    }
  }, [])

  const login = useCallback(async (returnTo = window.location.pathname) => {
    await userManager.signinRedirect({ state: { returnTo } })
  }, [])
  const logout = useCallback(async () => userManager.signoutRedirect(), [])

  const value = useMemo<AuthValue>(() => ({
    user,
    isLoading,
    isAuthenticated: Boolean(user && !user.expired),
    isManager: user ? getRoles(user.profile as Record<string, unknown>, user.access_token).includes('manager') : false,
    login,
    logout,
  }), [isLoading, login, logout, user])

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}

// Auth state and its hook intentionally share one module so consumers cannot import
// the context directly and bypass the provider contract.
// eslint-disable-next-line react-refresh/only-export-components
export function useAuth() {
  const context = useContext(AuthContext)
  if (!context) throw new Error('useAuth must be used within AuthProvider')
  return context
}
