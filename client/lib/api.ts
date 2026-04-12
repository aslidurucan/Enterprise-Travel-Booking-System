import { AuthResponse, PagedResult, RegisterResponse, Vehicle } from '@/types'

const API_BASE = process.env.NEXT_PUBLIC_API_URL ?? 'http://localhost:5100'

async function request<T>(path: string, options?: RequestInit): Promise<T> {
  const res = await fetch(`${API_BASE}${path}`, {
    headers: { 'Content-Type': 'application/json' },
    ...options,
  })

  if (!res.ok) {
    const error = await res.json().catch(() => ({ title: 'Bir hata oluştu.' }))
    throw new Error(error.title ?? 'Bir hata oluştu.')
  }

  return res.json()
}

function authHeader(token: string) {
  return { Authorization: `Bearer ${token}` }
}

export const api = {
  auth: {
    login: (username: string, password: string) =>
      request<AuthResponse>('/auth/login', {
        method: 'POST',
        body: JSON.stringify({ username, password }),
      }),

    register: (username: string, email: string, password: string) =>
      request<RegisterResponse>('/auth/register', {
        method: 'POST',
        body: JSON.stringify({ username, email, password }),
      }),
  },

  vehicles: {
    getAll: (token: string, page = 1, pageSize = 12) =>
      request<PagedResult<Vehicle>>(
        `/catalog/vehicles/paged?pageIndex=${page}&pageSize=${pageSize}`,
        { headers: { 'Content-Type': 'application/json', ...authHeader(token) } }
      ),
  },
}
