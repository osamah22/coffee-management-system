import { UserManager, WebStorageStateStore } from 'oidc-client-ts'

const keycloakUrl = (import.meta.env.VITE_KEYCLOAK_URL || 'http://localhost:8080').replace(/\/$/, '')
const realm = import.meta.env.VITE_KEYCLOAK_REALM || 'coffee_management_system'
const realmUrl = `${keycloakUrl}/realms/${realm}`
const oidcPath = `/realms/${realm}/protocol/openid-connect`
const browserOidcUrl = import.meta.env.DEV ? `${window.location.origin}/keycloak${oidcPath}` : `${keycloakUrl}${oidcPath}`

export const userManager = new UserManager({
  authority: realmUrl,
  client_id: import.meta.env.VITE_KEYCLOAK_CLIENT_ID || 'public',
  redirect_uri: `${window.location.origin}/auth/callback`,
  post_logout_redirect_uri: window.location.origin,
  response_type: 'code',
  scope: 'openid profile email',
  automaticSilentRenew: true,
  userStore: new WebStorageStateStore({ store: window.localStorage }),
  metadata: {
    issuer: realmUrl,
    authorization_endpoint: `${keycloakUrl}${oidcPath}/auth`,
    token_endpoint: `${browserOidcUrl}/token`,
    userinfo_endpoint: `${browserOidcUrl}/userinfo`,
    jwks_uri: `${browserOidcUrl}/certs`,
    end_session_endpoint: `${keycloakUrl}${oidcPath}/logout`,
  },
})

function decodeJwtPayload(accessToken?: string): Record<string, unknown> {
  if (!accessToken) return {}
  try {
    const payload = accessToken.split('.')[1].replace(/-/g, '+').replace(/_/g, '/')
    return JSON.parse(atob(payload)) as Record<string, unknown>
  } catch {
    return {}
  }
}

function rolesFromClaim(value: unknown): string[] {
  if (Array.isArray(value)) return value.filter((role): role is string => typeof role === 'string')

  if (value && typeof value === 'object') {
    return Object.entries(value as Record<string, unknown>).flatMap(([key, nestedValue]) => {
      if (nestedValue === true) return [key]
      if (typeof nestedValue === 'string') return [nestedValue]
      return rolesFromClaim(nestedValue)
    })
  }

  return typeof value === 'string' ? [value] : []
}

export function getRoles(profile: Record<string, unknown>, accessToken?: string): string[] {
  const tokenClaims = decodeJwtPayload(accessToken)
  const realmAccess = profile.realm_access as { roles?: string[] } | undefined
  const resourceAccess = profile.resource_access as Record<string, { roles?: string[] }> | undefined
  const tokenRealmAccess = tokenClaims.realm_access as { roles?: string[] } | undefined
  const tokenResourceAccess = tokenClaims.resource_access as Record<string, { roles?: string[] }> | undefined
  const clientId = import.meta.env.VITE_KEYCLOAK_CLIENT_ID || 'public'
  return [...new Set([
    ...(realmAccess?.roles || []),
    ...(resourceAccess?.[clientId]?.roles || []),
    ...(tokenRealmAccess?.roles || []),
    ...(tokenResourceAccess?.[clientId]?.roles || []),
    ...rolesFromClaim(profile.roles),
    ...rolesFromClaim(tokenClaims.roles),
  ])]
}
