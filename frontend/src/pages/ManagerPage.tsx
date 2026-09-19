import { ChevronDown, Coffee as CoffeeIcon, Edit3, LogOut, Plus, Snowflake, ThermometerSun, Trash2 } from 'lucide-react'
import { useState } from 'react'
import { Link } from 'react-router-dom'
import { useAuth } from '../auth/AuthContext'
import { Brand } from '../components/Brand'
import { CoffeeArtwork } from '../components/CoffeeArtwork'
import { CoffeeForm } from '../components/CoffeeForm'
import { Modal } from '../components/Modal'
import { OptionForm } from '../components/OptionForm'
import { useCoffees } from '../hooks/useCoffees'
import { coffeeApi } from '../lib/api'
import type { Coffee, CoffeeInput, CoffeeOption, CoffeeOptionInput } from '../types'

type Dialog =
  | { kind: 'coffee'; coffee?: Coffee }
  | { kind: 'option'; coffee: Coffee; option?: CoffeeOption }
  | { kind: 'delete-coffee'; coffee: Coffee }
  | { kind: 'delete-option'; coffee: Coffee; option: CoffeeOption }
  | null

const money = (cents: number) => new Intl.NumberFormat(undefined, { style: 'currency', currency: 'USD' }).format(cents / 100)

export function ManagerPage() {
  const { user, isLoading: authLoading, isAuthenticated, isManager, login, logout } = useAuth()
  const { coffees, isLoading, error, refresh } = useCoffees()
  const [expanded, setExpanded] = useState<string | null>(null)
  const [dialog, setDialog] = useState<Dialog>(null)
  const [actionError, setActionError] = useState<string | null>(null)
  const [deleting, setDeleting] = useState(false)
  const token = user?.access_token || ''

  async function saveCoffee(input: CoffeeInput, coffee?: Coffee) {
    if (coffee) await coffeeApi.update(coffee.id, input, token)
    else await coffeeApi.create(input, token)
    setDialog(null); await refresh()
  }

  async function saveOption(coffee: Coffee, input: CoffeeOptionInput, option?: CoffeeOption) {
    if (option) await coffeeApi.updateOption(coffee.id, option.id, input, token)
    else await coffeeApi.createOption(coffee.id, input, token)
    setDialog(null); setExpanded(coffee.id); await refresh()
  }

  async function confirmDelete() {
    if (!dialog || (dialog.kind !== 'delete-coffee' && dialog.kind !== 'delete-option')) return
    setDeleting(true); setActionError(null)
    try {
      if (dialog.kind === 'delete-coffee') await coffeeApi.remove(dialog.coffee.id, token)
      else await coffeeApi.removeOption(dialog.coffee.id, dialog.option.id, token)
      setDialog(null); await refresh()
    } catch (reason) { setActionError(reason instanceof Error ? reason.message : 'Could not delete this item.') }
    finally { setDeleting(false) }
  }

  if (authLoading) return <div className="full-loader"><CoffeeIcon className="pulse" /><span>Opening the studio…</span></div>
  if (!isAuthenticated) return <main className="gate"><Brand /><div className="gate__art"><CoffeeIcon /></div><span className="eyebrow">Manager access</span><h1>Your coffee studio<br />is ready.</h1><p>Sign in through Keycloak to curate coffees, serving options, and pricing.</p><button className="button button--accent" onClick={() => void login('/manage')}>Continue to sign in</button><Link to="/">Return to the collection</Link></main>
  if (!isManager) return <main className="gate"><Brand /><div className="gate__art gate__art--muted"><CoffeeIcon /></div><span className="eyebrow">Collection access only</span><h1>This studio is for managers.</h1><p>Your account is signed in, but it does not include the <strong>manager</strong> role.</p><Link className="button button--dark" to="/">Browse the collection</Link><button className="link-button" onClick={() => void logout()}>Sign in with another account</button></main>

  return (
    <div className="manager-layout">
      <aside className="manager-sidebar"><Brand light /><nav><span>Workspace</span><a className="active"><CoffeeIcon size={18} /> Coffee collection</a></nav><div className="manager-sidebar__foot"><div className="user-chip"><span>{String(user?.profile.given_name || user?.profile.preferred_username || 'M').charAt(0)}</span><div><strong>{String(user?.profile.name || user?.profile.preferred_username || 'Manager')}</strong><small>Manager</small></div></div><button onClick={() => void logout()} aria-label="Sign out"><LogOut size={18} /></button></div></aside>
      <main className="manager-main">
        <div className="manager-topbar"><Link to="/">View storefront</Link><span>•</span><span>Manager studio</span></div>
        <header className="manager-heading"><div><span className="eyebrow">Coffee collection</span><h1>Curate the menu</h1><p>Create, price, and polish every cup in your collection.</p></div><button className="button button--accent" onClick={() => setDialog({ kind: 'coffee' })}><Plus size={18} /> New coffee</button></header>
        <section className="manager-panel">
          <div className="manager-toolbar"><span>{coffees.length} {coffees.length === 1 ? 'coffee' : 'coffees'}</span></div>
          {isLoading && <div className="table-state">Loading the collection…</div>}
          {error && <div className="table-state table-state--error"><p>{error}</p><button className="button button--ghost" onClick={() => void refresh()}>Try again</button></div>}
          {!isLoading && !error && coffees.length === 0 && <div className="table-state"><CoffeeIcon /><h3>Your menu is a blank canvas</h3><p>Create your first coffee to get started.</p></div>}
          {!isLoading && !error && coffees.map((coffee) => (
            <article className={`manager-coffee ${expanded === coffee.id ? 'manager-coffee--open' : ''}`} key={coffee.id}>
              <div className="manager-coffee__row"><CoffeeArtwork coffee={coffee} /><div className="manager-coffee__details"><h3>{coffee.name}</h3><p>{coffee.description || 'No tasting notes yet.'}</p></div><div className="manager-coffee__count"><strong>{coffee.options.length}</strong><span>options</span></div><div className="manager-coffee__actions"><button className="icon-button" title="Edit coffee" onClick={() => setDialog({ kind: 'coffee', coffee })}><Edit3 size={17} /></button><button className="icon-button danger" title="Delete coffee" onClick={() => setDialog({ kind: 'delete-coffee', coffee })}><Trash2 size={17} /></button><button className="icon-button expand" title="Show options" onClick={() => setExpanded(expanded === coffee.id ? null : coffee.id)}><ChevronDown size={19} /></button></div></div>
              {expanded === coffee.id && <div className="options-panel"><div className="options-panel__heading"><div><strong>Serving options</strong><span>Configure size, temperature, and price.</span></div><button className="button button--ghost button--small" onClick={() => setDialog({ kind: 'option', coffee })}><Plus size={16} /> Add option</button></div>{coffee.options.length === 0 ? <div className="empty-options">No serving options yet.</div> : coffee.options.map((option) => <div className="option-row" key={option.id}><span className={`temp-icon temp-icon--${option.type.toLowerCase()}`}>{option.type.toLowerCase() === 'hot' ? <ThermometerSun size={17} /> : <Snowflake size={17} />}</span><strong>{option.type}</strong><span>{option.size}</span><b>{money(option.priceInCents)}</b><button onClick={() => setDialog({ kind: 'option', coffee, option })} aria-label="Edit option"><Edit3 size={16} /></button><button className="danger" onClick={() => setDialog({ kind: 'delete-option', coffee, option })} aria-label="Delete option"><Trash2 size={16} /></button></div>)}</div>}
            </article>
          ))}
        </section>
      </main>
      {dialog?.kind === 'coffee' && <Modal title={dialog.coffee ? 'Edit coffee' : 'Create a new coffee'} description="Shape the story your guests will see in the collection." onClose={() => setDialog(null)}><CoffeeForm coffee={dialog.coffee} onCancel={() => setDialog(null)} onSubmit={(input) => saveCoffee(input, dialog.coffee)} /></Modal>}
      {dialog?.kind === 'option' && <Modal title={dialog.option ? 'Edit serving option' : 'Add a serving option'} description={`Set how ${dialog.coffee.name} is served and priced.`} onClose={() => setDialog(null)}><OptionForm option={dialog.option} existingOptions={dialog.coffee.options} onCancel={() => setDialog(null)} onSubmit={(input) => saveOption(dialog.coffee, input, dialog.option)} /></Modal>}
      {(dialog?.kind === 'delete-coffee' || dialog?.kind === 'delete-option') && <Modal title={dialog.kind === 'delete-coffee' ? `Remove ${dialog.coffee.name}?` : 'Remove this serving option?'} description="This action cannot be undone." onClose={() => setDialog(null)}><div className="confirm-delete"><div className="confirm-delete__icon"><Trash2 /></div>{actionError && <div className="form-error">{actionError}</div>}<div className="form__actions"><button className="button button--ghost" onClick={() => setDialog(null)}>Keep it</button><button className="button button--danger" disabled={deleting} onClick={() => void confirmDelete()}>{deleting ? 'Removing…' : 'Yes, remove'}</button></div></div></Modal>}
    </div>
  )
}
