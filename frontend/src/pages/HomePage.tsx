import { ArrowDown, Coffee, Leaf, Sparkles } from 'lucide-react'
import { CoffeeCard } from '../components/CoffeeCard'
import { Header } from '../components/Header'
import { useCoffees } from '../hooks/useCoffees'

export function HomePage() {
  const { coffees, isLoading, error, refresh } = useCoffees()
  return (
    <div className="page"><Header /><main>
      <section className="hero"><div className="hero__grain" /><div className="shell hero__content"><div className="hero__copy"><span className="eyebrow eyebrow--light"><Sparkles size={14} /> Crafted slowly. Savored fully.</span><h1>Your daily ritual,<br /><em>beautifully brewed.</em></h1><p>A considered collection of comforting classics and bright seasonal cups, crafted for every kind of coffee moment.</p><a className="button button--cream" href="#collection">Explore the collection <ArrowDown size={17} /></a></div><div className="hero-cup" aria-hidden="true"><span className="steam steam--one" /><span className="steam steam--two" /><span className="hero-cup__saucer" /><span className="hero-cup__body"><i><Leaf /></i></span></div></div><div className="hero__ticker"><span>ETHICALLY SOURCED</span><i>✦</i><span>ROASTED WITH CARE</span><i>✦</i><span>MADE FOR SLOW MORNINGS</span><i>✦</i></div></section>
      <section className="collection shell" id="collection"><div className="section-heading"><div><span className="eyebrow">Our coffee menu</span><h2>Find your next <em>favorite.</em></h2></div><p>From warm, velvety comfort to crisp, refreshing pours—there’s a cup waiting for you.</p></div>
        {isLoading && <div className="coffee-grid" aria-label="Loading coffees">{[0, 1, 2].map((item) => <div className="coffee-card skeleton" key={item} />)}</div>}
        {error && <div className="state-card"><Coffee /><h3>The pot needs a moment</h3><p>{error}</p><button className="button button--dark" onClick={() => void refresh()}>Try again</button></div>}
        {!isLoading && !error && coffees.length === 0 && <div className="state-card"><Coffee /><h3>The collection is brewing</h3><p>Our first coffee will be here soon.</p></div>}
        {!isLoading && !error && coffees.length > 0 && <div className="coffee-grid">{coffees.map((coffee) => <CoffeeCard coffee={coffee} key={coffee.id} />)}</div>}
      </section>
      <section className="story" id="story"><div className="shell story__inner"><div className="story__seal"><Coffee /><span>ROAST<br />& RITUAL</span></div><div><span className="eyebrow eyebrow--light">A better cup</span><h2>Good coffee is more than a drink.<br /><em>It’s a pause.</em></h2></div><p>We celebrate simple ingredients, careful craft, and the small moments that make a day feel good.</p></div></section>
    </main><footer><div className="shell"><span>Roast & Ritual</span><span>Made for unhurried moments.</span><span>© {new Date().getFullYear()}</span></div></footer></div>
  )
}
