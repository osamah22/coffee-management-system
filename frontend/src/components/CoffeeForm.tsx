import { useState, type FormEvent } from 'react'
import type { Coffee, CoffeeInput } from '../types'

export function CoffeeForm({ coffee, onSubmit, onCancel }: { coffee?: Coffee; onSubmit: (input: CoffeeInput) => Promise<void>; onCancel: () => void }) {
  const [name, setName] = useState(coffee?.name || '')
  const [description, setDescription] = useState(coffee?.description || '')
  const [error, setError] = useState<string | null>(null)
  const [saving, setSaving] = useState(false)

  async function submit(event: FormEvent) {
    event.preventDefault()
    if (!name.trim()) { setError('Give this coffee a name before saving it.'); return }
    setSaving(true); setError(null)
    try { await onSubmit({ name: name.trim(), description: description.trim() }) }
    catch (reason) { setError(reason instanceof Error ? reason.message : 'Could not save this coffee.'); setSaving(false) }
  }

  return (
    <form className="form" onSubmit={(event) => void submit(event)}>
      <label><span>Coffee name</span><input autoFocus value={name} onChange={(event) => setName(event.target.value)} maxLength={120} placeholder="e.g. Honeycomb Flat White" /><small>{name.length}/120</small></label>
      <label><span>Tasting notes</span><textarea value={description} onChange={(event) => setDescription(event.target.value)} maxLength={400} rows={5} placeholder="Tell guests what makes this cup memorable…" /><small>{description.length}/400</small></label>
      {error && <div className="form-error" role="alert">{error}</div>}
      <div className="form__actions"><button type="button" className="button button--ghost" onClick={onCancel}>Cancel</button><button className="button button--accent" disabled={saving}>{saving ? 'Saving…' : coffee ? 'Save changes' : 'Add to collection'}</button></div>
    </form>
  )
}
