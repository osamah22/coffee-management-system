import { useState, type FormEvent } from 'react'
import type { CoffeeOption, CoffeeOptionInput, CoffeeSize, CoffeeType } from '../types'

export function OptionForm({ option, existingOptions = [], onSubmit, onCancel }: { option?: CoffeeOption; existingOptions?: CoffeeOption[]; onSubmit: (input: CoffeeOptionInput) => Promise<void>; onCancel: () => void }) {
  const [type, setType] = useState<CoffeeType>(option?.type || 'Hot')
  const [size, setSize] = useState<CoffeeSize>(option?.size || 'Medium')
  const [price, setPrice] = useState(option ? (option.priceInCents / 100).toFixed(2) : '')
  const [error, setError] = useState<string | null>(null)
  const [saving, setSaving] = useState(false)

  async function submit(event: FormEvent) {
    event.preventDefault()
    const amount = Number(price)
    if (!Number.isFinite(amount) || amount < 0) { setError('Enter a valid price of zero or more.'); return }
    const duplicate = existingOptions.some((existing) => existing.id !== option?.id && existing.type === type && existing.size === size)
    if (duplicate) { setError(`${type} / ${size} is already available for this coffee.`); return }
    setSaving(true); setError(null)
    try { await onSubmit({ type, size, priceInCents: Math.round(amount * 100) }) }
    catch (reason) { setError(reason instanceof Error ? reason.message : 'Could not save this option.'); setSaving(false) }
  }

  return (
    <form className="form" onSubmit={(event) => void submit(event)}>
      <div className="segmented-field"><span>Serve</span><div className="segmented">{(['Hot', 'Cold'] as CoffeeType[]).map((value) => <button type="button" key={value} className={type === value ? 'active' : ''} onClick={() => setType(value)}>{value}</button>)}</div></div>
      <label><span>Cup size</span><select value={size} onChange={(event) => setSize(event.target.value as CoffeeSize)}><option>Small</option><option>Medium</option><option>Large</option></select></label>
      <label><span>Price</span><div className="price-input"><span>$</span><input inputMode="decimal" value={price} onChange={(event) => setPrice(event.target.value)} placeholder="4.50" /></div></label>
      {error && <div className="form-error" role="alert">{error}</div>}
      <div className="form__actions"><button type="button" className="button button--ghost" onClick={onCancel}>Cancel</button><button className="button button--accent" disabled={saving}>{saving ? 'Saving…' : option ? 'Save option' : 'Add option'}</button></div>
    </form>
  )
}
