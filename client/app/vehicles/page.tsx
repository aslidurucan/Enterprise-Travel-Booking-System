'use client'

import { useEffect, useState, useCallback } from 'react'
import { useRouter } from 'next/navigation'
import { api } from '@/lib/api'
import { Vehicle, PagedResult } from '@/types'

const PAGE_SIZE = 12

export default function VehiclesPage() {
  const router = useRouter()
  const [result, setResult] = useState<PagedResult<Vehicle> | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [username, setUsername] = useState('')
  const [page, setPage] = useState(1)
  const [search, setSearch] = useState('')
  const [filterAvailable, setFilterAvailable] = useState<'all' | 'available' | 'unavailable'>('all')

  const fetchVehicles = useCallback((token: string, pageNum: number) => {
    setLoading(true)
    api.vehicles
      .getAll(token, pageNum, PAGE_SIZE)
      .then(data => setResult(data))
      .catch(err => setError(err.message))
      .finally(() => setLoading(false))
  }, [])

  useEffect(() => {
    const token = localStorage.getItem('token')
    const savedUsername = localStorage.getItem('username')

    if (!token) {
      router.push('/auth/login')
      return
    }

    setUsername(savedUsername ?? '')
    fetchVehicles(token, page)
  }, [router, page, fetchVehicles])

  function handleLogout() {
    localStorage.clear()
    router.push('/auth/login')
  }

  function handlePageChange(newPage: number) {
    window.scrollTo({ top: 0, behavior: 'smooth' })
    setPage(newPage)
  }

  const vehicles = result?.items ?? []

  const filtered = vehicles.filter(v => {
    const matchSearch = search === '' ||
      v.brand.toLowerCase().includes(search.toLowerCase()) ||
      v.model.toLowerCase().includes(search.toLowerCase())
    const matchAvail =
      filterAvailable === 'all' ||
      (filterAvailable === 'available' && v.isAvailable) ||
      (filterAvailable === 'unavailable' && !v.isAvailable)
    return matchSearch && matchAvail
  })

  const totalPages = result?.totalPages ?? 1

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Navbar */}
      <header className="sticky top-0 z-10 border-b border-gray-200 bg-white/80 px-6 py-4 backdrop-blur-sm shadow-sm">
        <div className="mx-auto flex max-w-6xl items-center justify-between">
          <span className="text-xl font-bold text-blue-600">WanderSync</span>
          <div className="flex items-center gap-4">
            <div className="flex items-center gap-2">
              <div className="flex h-8 w-8 items-center justify-center rounded-full bg-blue-100 text-sm font-semibold text-blue-700">
                {username.charAt(0).toUpperCase()}
              </div>
              <span className="hidden text-sm font-medium text-gray-700 sm:block">{username}</span>
            </div>
            <button
              onClick={handleLogout}
              className="rounded-lg border border-gray-300 px-4 py-1.5 text-sm font-medium text-gray-600 hover:bg-gray-100 transition-colors"
            >
              Çıkış Yap
            </button>
          </div>
        </div>
      </header>

      <main className="mx-auto max-w-6xl px-6 py-8">
        {/* Başlık + istatistik */}
        <div className="mb-6">
          <h2 className="text-2xl font-bold text-gray-900">Araçlar</h2>
          {result && (
            <p className="mt-1 text-sm text-gray-500">
              Toplam {result.totalCount.toLocaleString('tr-TR')} araç —{' '}
              Sayfa {result.pageIndex} / {result.totalPages}
            </p>
          )}
        </div>

        {/* Arama + Filtre */}
        <div className="mb-6 flex flex-col gap-3 sm:flex-row sm:items-center">
          <div className="relative flex-1">
            <svg className="absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
            </svg>
            <input
              type="text"
              value={search}
              onChange={e => setSearch(e.target.value)}
              placeholder="Marka veya model ara..."
              className="w-full rounded-xl border border-gray-200 bg-white py-2.5 pl-10 pr-4 text-sm shadow-sm focus:border-blue-500 focus:outline-none focus:ring-2 focus:ring-blue-500/20"
            />
          </div>
          <div className="flex gap-2">
            {(['all', 'available', 'unavailable'] as const).map(f => (
              <button
                key={f}
                onClick={() => setFilterAvailable(f)}
                className={`rounded-lg px-4 py-2 text-sm font-medium transition-colors ${
                  filterAvailable === f
                    ? 'bg-blue-600 text-white'
                    : 'border border-gray-200 bg-white text-gray-600 hover:bg-gray-50'
                }`}
              >
                {f === 'all' ? 'Tümü' : f === 'available' ? 'Müsait' : 'Dolu'}
              </button>
            ))}
          </div>
        </div>

        {/* Hata */}
        {error && (
          <div className="mb-6 flex items-center gap-2 rounded-xl border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-600">
            <svg className="h-4 w-4 shrink-0" fill="currentColor" viewBox="0 0 20 20">
              <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clipRule="evenodd" />
            </svg>
            {error}
          </div>
        )}

        {/* Skeleton yüklenme */}
        {loading ? (
          <div className="grid grid-cols-1 gap-5 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
            {Array.from({ length: PAGE_SIZE }).map((_, i) => (
              <div key={i} className="rounded-2xl border border-gray-200 bg-white p-5">
                <div className="mb-4 h-28 animate-pulse rounded-xl bg-gray-100" />
                <div className="mb-2 h-4 animate-pulse rounded bg-gray-100" />
                <div className="mb-4 h-3 w-2/3 animate-pulse rounded bg-gray-100" />
                <div className="h-8 animate-pulse rounded-lg bg-gray-100" />
              </div>
            ))}
          </div>
        ) : filtered.length === 0 ? (
          <div className="flex flex-col items-center justify-center py-24 text-center">
            <div className="mb-4 flex h-16 w-16 items-center justify-center rounded-full bg-gray-100">
              <svg className="h-8 w-8 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
              </svg>
            </div>
            <p className="font-medium text-gray-700">Araç bulunamadı</p>
            <p className="mt-1 text-sm text-gray-400">Farklı bir arama dene veya filtreyi kaldır</p>
          </div>
        ) : (
          <div className="grid grid-cols-1 gap-5 sm:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4">
            {filtered.map(vehicle => (
              <VehicleCard key={vehicle.id} vehicle={vehicle} />
            ))}
          </div>
        )}

        {/* Pagination */}
        {!loading && totalPages > 1 && (
          <div className="mt-10 flex items-center justify-center gap-2">
            <button
              onClick={() => handlePageChange(page - 1)}
              disabled={!result?.hasPreviousPage}
              className="flex h-9 w-9 items-center justify-center rounded-lg border border-gray-200 bg-white text-gray-600 shadow-sm hover:bg-gray-50 disabled:opacity-40 disabled:cursor-not-allowed transition-colors"
            >
              <svg className="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 19l-7-7 7-7" />
              </svg>
            </button>

            {getPageNumbers(page, totalPages).map((p, i) =>
              p === '...' ? (
                <span key={`dots-${i}`} className="px-1 text-gray-400">···</span>
              ) : (
                <button
                  key={p}
                  onClick={() => handlePageChange(p as number)}
                  className={`h-9 w-9 rounded-lg text-sm font-medium transition-colors ${
                    p === page
                      ? 'bg-blue-600 text-white shadow-sm'
                      : 'border border-gray-200 bg-white text-gray-600 hover:bg-gray-50'
                  }`}
                >
                  {p}
                </button>
              )
            )}

            <button
              onClick={() => handlePageChange(page + 1)}
              disabled={!result?.hasNextPage}
              className="flex h-9 w-9 items-center justify-center rounded-lg border border-gray-200 bg-white text-gray-600 shadow-sm hover:bg-gray-50 disabled:opacity-40 disabled:cursor-not-allowed transition-colors"
            >
              <svg className="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 5l7 7-7 7" />
              </svg>
            </button>
          </div>
        )}
      </main>
    </div>
  )
}

function VehicleCard({ vehicle }: { vehicle: Vehicle }) {
  return (
    <div className="group rounded-2xl border border-gray-200 bg-white p-5 shadow-sm transition-all hover:shadow-md hover:-translate-y-0.5">
      <div className="mb-4 flex h-28 items-center justify-center rounded-xl bg-gradient-to-br from-blue-50 to-indigo-100">
        <svg className="h-14 w-14 text-blue-300" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1} d="M9 17a2 2 0 11-4 0 2 2 0 014 0zM19 17a2 2 0 11-4 0 2 2 0 014 0z" />
          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1} d="M13 16V6a1 1 0 00-1-1H4a1 1 0 00-1 1v10l2.083.694M13 16H9m4 0h2.083M13 16v-5a2 2 0 012-2h3l2 4v3h-2.083M13 11h4" />
        </svg>
      </div>

      <div className="mb-3 flex items-start justify-between gap-2">
        <div className="min-w-0">
          <h3 className="truncate font-semibold text-gray-900">{vehicle.brand} {vehicle.model}</h3>
          <p className="text-sm text-gray-500">{vehicle.year}</p>
        </div>
        <span className={`shrink-0 rounded-full px-2.5 py-0.5 text-xs font-semibold ${
          vehicle.isAvailable ? 'bg-green-100 text-green-700' : 'bg-red-100 text-red-700'
        }`}>
          {vehicle.isAvailable ? 'Müsait' : 'Dolu'}
        </span>
      </div>

      <div className="mb-4 border-t border-gray-100 pt-3">
        <p className="text-xl font-bold text-blue-600">
          {vehicle.dailyPrice.toLocaleString('tr-TR', { maximumFractionDigits: 0 })}
          <span className="text-sm font-medium text-gray-400"> {vehicle.currency}</span>
        </p>
        <p className="text-xs text-gray-400">günlük</p>
      </div>

      <button
        disabled={!vehicle.isAvailable}
        className={`w-full rounded-lg py-2 text-sm font-semibold transition-colors ${
          vehicle.isAvailable
            ? 'bg-blue-600 text-white hover:bg-blue-700'
            : 'cursor-not-allowed bg-gray-100 text-gray-400'
        }`}
      >
        {vehicle.isAvailable ? 'Kirala' : 'Müsait Değil'}
      </button>
    </div>
  )
}

function getPageNumbers(current: number, total: number): (number | '...')[] {
  if (total <= 7) return Array.from({ length: total }, (_, i) => i + 1)

  const pages: (number | '...')[] = [1]

  if (current > 3) pages.push('...')

  const start = Math.max(2, current - 1)
  const end = Math.min(total - 1, current + 1)
  for (let i = start; i <= end; i++) pages.push(i)

  if (current < total - 2) pages.push('...')
  pages.push(total)

  return pages
}
