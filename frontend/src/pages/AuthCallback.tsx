import { LoaderCircle } from 'lucide-react'
import { useEffect, useRef, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { userManager } from '../auth/keycloak'

export function AuthCallback() {
  const navigate = useNavigate()
  const handled = useRef(false)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    if (handled.current) return
    handled.current = true
    void userManager.signinRedirectCallback().then((user) => {
      const state = user.state as { returnTo?: string } | undefined
      navigate(state?.returnTo || '/manage', { replace: true })
    }).catch((reason) => setError(reason instanceof Error ? reason.message : 'Could not complete sign in.'))
  }, [navigate])

  return <main className="auth-screen"><div className="auth-card">{error ? <><h1>Sign in paused</h1><p>{error}</p><a className="button button--dark" href="/">Back home</a></> : <><LoaderCircle className="spin" /><h1>Preparing your coffee studio</h1><p>Completing your secure sign in…</p></>}</div></main>
}
