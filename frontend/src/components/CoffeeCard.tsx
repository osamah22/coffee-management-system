import { ArrowUpRight, Snowflake, ThermometerSun } from 'lucide-react'
import type { Coffee } from '../types'
import { CoffeeArtwork } from './CoffeeArtwork'

const formatPrice = (cents: number) => new Intl.NumberFormat(undefined, { style: 'currency', currency: 'USD' }).format(cents / 100)

export function CoffeeCard({ coffee }: { coffee: Coffee }) {
  const prices = coffee.options.map((option) => option.priceInCents)
  const fromPrice = prices.length ? Math.min(...prices) : null
  const hasHot = coffee.options.some((option) => option.type.toLowerCase() === 'hot')
  const hasCold = coffee.options.some((option) => option.type.toLowerCase() === 'cold')
  return (
    <article className="coffee-card">
      <CoffeeArtwork coffee={coffee} />
      <div className="coffee-card__body">
        <div className="eyebrow-row"><span className="eyebrow">House selection</span><span className="serve-icons">{hasHot && <ThermometerSun size={15} />}{hasCold && <Snowflake size={15} />}</span></div>
        <h3>{coffee.name}</h3><p>{coffee.description}</p>
        <div className="coffee-card__footer"><span>{fromPrice === null ? 'Coming soon' : <>From <strong>{formatPrice(fromPrice)}</strong></>}</span><span className="round-arrow"><ArrowUpRight size={17} /></span></div>
      </div>
    </article>
  )
}
