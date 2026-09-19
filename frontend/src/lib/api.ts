import type { Coffee, CoffeeInput, CoffeeOptionInput } from '../types'

const apiBaseUrl = (import.meta.env.VITE_API_URL || '/api').replace(/\/$/, '')

export class ApiError extends Error {
  constructor(message: string, public readonly status: number) {
    super(message)
    this.name = 'ApiError'
  }
}

async function request<T>(path: string, accessToken?: string, init?: RequestInit): Promise<T> {
  const response = await fetch(`${apiBaseUrl}${path}`, {
    ...init,
    headers: {
      ...(init?.body ? { 'Content-Type': 'application/json' } : {}),
      ...(accessToken ? { Authorization: `Bearer ${accessToken}` } : {}),
      ...init?.headers,
    },
  })

  if (!response.ok) {
    let message = 'Something went wrong. Please try again.'
    try {
      const rawBody = await response.text()
      try {
        const body = JSON.parse(rawBody) as { error?: string; title?: string; errors?: Array<{ message: string }> }
        message = body.error || body.errors?.map((item) => item.message).join(' ') || body.title || message
      } catch {
        if (rawBody) message = rawBody
      }
    } catch {
      // Keep the friendly fallback when a response has no readable body.
    }
    throw new ApiError(message, response.status)
  }

  if (response.status === 204) return undefined as T

  // ASP.NET's CreatedAtAction response for coffee creation has no payload and
  // does not always send a Content-Length header. Reading the body first keeps
  // successful creates from being misreported as JSON parsing failures.
  const rawBody = await response.text()
  return rawBody ? JSON.parse(rawBody) as T : undefined as T
}

export const coffeeApi = {
  list: (signal?: AbortSignal) => request<Coffee[]>('/v1/coffees', undefined, { signal }),
  create: (input: CoffeeInput, token: string) => request<void>('/v1/coffees', token, { method: 'POST', body: JSON.stringify(input) }),
  update: (id: string, input: CoffeeInput, token: string) => request<void>(`/v1/coffees/${id}`, token, { method: 'PUT', body: JSON.stringify(input) }),
  remove: (id: string, token: string) => request<void>(`/v1/coffees/${id}`, token, { method: 'DELETE' }),
  createOption: (coffeeId: string, input: CoffeeOptionInput, token: string) => request<void>(`/v1/coffees/${coffeeId}/options`, token, { method: 'POST', body: JSON.stringify(input) }),
  updateOption: (coffeeId: string, optionId: string, input: CoffeeOptionInput, token: string) => request<void>(`/v1/coffees/${coffeeId}/options/${optionId}`, token, { method: 'PUT', body: JSON.stringify(input) }),
  removeOption: (coffeeId: string, optionId: string, token: string) => request<void>(`/v1/coffees/${coffeeId}/options/${optionId}`, token, { method: 'DELETE' }),
}
