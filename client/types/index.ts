export type Vehicle = {
  id: string
  brand: string
  model: string
  year: number
  dailyPrice: number
  currency: string
  isAvailable: boolean
}

export type PagedResult<T> = {
  items: T[]
  totalCount: number
  pageIndex: number
  pageSize: number
  totalPages: number
  hasPreviousPage: boolean
  hasNextPage: boolean
}

export type AuthResponse = {
  token: string
  username: string
  role: string
  message: string
}

export type RegisterResponse = {
  userId: string
  username: string
  email: string
  role: string
  message: string
}
