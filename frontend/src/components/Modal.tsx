import { X } from 'lucide-react'
import { useEffect, type ReactNode } from 'react'

export function Modal({ title, description, children, onClose }: { title: string; description?: string; children: ReactNode; onClose: () => void }) {
  useEffect(() => {
    const onKeyDown = (event: KeyboardEvent) => event.key === 'Escape' && onClose()
    document.addEventListener('keydown', onKeyDown)
    document.body.classList.add('modal-open')
    return () => { document.removeEventListener('keydown', onKeyDown); document.body.classList.remove('modal-open') }
  }, [onClose])

  return (
    <div className="modal-backdrop" role="presentation" onMouseDown={(event) => event.target === event.currentTarget && onClose()}>
      <section className="modal" role="dialog" aria-modal="true" aria-labelledby="modal-title">
        <button className="icon-button modal__close" onClick={onClose} aria-label="Close dialog"><X /></button>
        <span className="eyebrow">Coffee studio</span><h2 id="modal-title">{title}</h2>
        {description && <p className="modal__description">{description}</p>}{children}
      </section>
    </div>
  )
}
