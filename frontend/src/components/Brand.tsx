import { Coffee } from 'lucide-react'
import { Link } from 'react-router-dom'

export function Brand({ light = false }: { light?: boolean }) {
  return (
    <Link className={`brand ${light ? 'brand--light' : ''}`} to="/" aria-label="Roast and Ritual home">
      <span className="brand__mark"><Coffee size={21} strokeWidth={1.8} /></span>
      <span><strong>Roast <i>&</i> Ritual</strong><small>COFFEE COLLECTION</small></span>
    </Link>
  )
}
