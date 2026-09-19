import { useCallback, useEffect, useState } from 'react'
import { coffeeApi } from '../lib/api'
import type { Coffee } from '../types'

export function useCoffees() {
  const [coffees, setCoffees] = useState<Coffee[]>([])
  const [isLoading, setIsLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const refresh = useCallback(async (signal?: AbortSignal) => {
    setIsLoading(true)
    setError(null)
    try {
      setCoffees(await coffeeApi.list(signal))
    } catch (reason) {
      if (reason instanceof DOMException && reason.name === 'AbortError') return
      setError(reason instanceof Error ? reason.message : 'Could not load the coffee collection.')
    } finally {
      if (!signal?.aborted) setIsLoading(false)
    }
  }, [])

  useEffect(() => {
    const controller = new AbortController()
    // Initial data fetching is the external synchronization owned by this hook.
    // eslint-disable-next-line react-hooks/set-state-in-effect
    void refresh(controller.signal)
    return () => controller.abort()
  }, [refresh])

  return { coffees, isLoading, error, refresh }
}
