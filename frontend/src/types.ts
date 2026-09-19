export type CoffeeType = 'Hot' | 'Cold'
export type CoffeeSize = 'Small' | 'Medium' | 'Large'

export interface CoffeeOption {
  id: string
  type: CoffeeType
  size: CoffeeSize
  priceInCents: number
}

export interface Coffee {
  id: string
  name: string
  description: string
  options: CoffeeOption[]
  slug: string
}

export interface CoffeeInput { name: string; description: string }

export interface CoffeeOptionInput {
  type: CoffeeType
  size: CoffeeSize
  priceInCents: number
}
