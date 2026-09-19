import type { CSSProperties } from 'react'
import type { Coffee } from '../types'

const palettes = [['#a44b2a', '#e8a675', '#522014'], ['#384733', '#9ab184', '#1d271c'], ['#a77b42', '#ead2a2', '#52381e'], ['#61383a', '#be7c76', '#2d1718']]
const hash = (value: string) => [...value].reduce((total, char) => total + char.charCodeAt(0), 0)

export function CoffeeArtwork({ coffee }: { coffee: Coffee }) {
  const palette = palettes[hash(coffee.id) % palettes.length]
  const initials = coffee.name.split(/\s+/).map((part) => part[0]).join('').slice(0, 2)
  return (
    <div className="coffee-art" style={{ '--bean': palette[0], '--cream': palette[1], '--deep': palette[2] } as CSSProperties}>
      <span className="coffee-art__sun" /><span className="coffee-art__leaf coffee-art__leaf--one" /><span className="coffee-art__leaf coffee-art__leaf--two" />
      <span className="coffee-art__cup"><i>{initials}</i></span><span className="coffee-art__shadow" />
    </div>
  )
}
