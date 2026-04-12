'use client'

import { useState } from 'react'
import { useRouter } from 'next/navigation'
import Link from 'next/link'
import { api } from '@/lib/api'

export default function LoginPage() {
  const router = useRouter()
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    setError('')
    setLoading(true)
    try {
      const result = await api.auth.login(username, password)
      localStorage.setItem('token', result.token)
      localStorage.setItem('username', result.username)
      localStorage.setItem('role', result.role)
      router.push('/vehicles')
    } catch (err: unknown) {
      setError(err instanceof Error ? err.message : 'Giriş yapılamadı.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="flex min-h-screen bg-[#0d0e1a]">

      {/* ── Sol panel ── */}
      <div className="relative hidden lg:flex lg:w-[55%] flex-col overflow-hidden">
        {/* Aurora */}
        <div className="orb-1 pointer-events-none absolute -top-32 -left-32 h-[500px] w-[500px] rounded-full bg-violet-600/30 blur-[120px]" />
        <div className="orb-2 pointer-events-none absolute bottom-0 right-0 h-[450px] w-[450px] rounded-full bg-blue-500/25 blur-[110px]" />
        <div className="orb-3 pointer-events-none absolute top-1/2 left-1/3 h-[300px] w-[300px] rounded-full bg-cyan-500/15 blur-[90px]" />

        {/* Noise */}
        <div className="pointer-events-none absolute inset-0 opacity-[0.04]"
          style={{ backgroundImage: `url("data:image/svg+xml,%3Csvg viewBox='0 0 256 256' xmlns='http://www.w3.org/2000/svg'%3E%3Cfilter id='n'%3E%3CfeTurbulence type='fractalNoise' baseFrequency='0.9' numOctaves='4' stitchTiles='stitch'/%3E%3C/filter%3E%3Crect width='100%25' height='100%25' filter='url(%23n)'/%3E%3C/svg%3E")` }}
        />

        {/* İçerik */}
        <div className="relative z-10 flex flex-1 flex-col justify-between p-12">
          {/* Logo */}
          <Link href="/">
            <span className="bg-gradient-to-r from-violet-400 via-blue-400 to-cyan-400 bg-clip-text text-2xl font-bold text-transparent">
              WanderSync
            </span>
          </Link>

          {/* Ana içerik */}
          <div>
            <p className="mb-4 text-sm font-medium text-violet-400 uppercase tracking-widest">Araç Kiralama Platformu</p>
            <h1 className="mb-6 text-4xl font-bold leading-tight text-white xl:text-5xl">
              Gitmek istediğin
              <br />
              <span className="bg-gradient-to-r from-violet-400 to-cyan-400 bg-clip-text text-transparent">
                her yere ulaş.
              </span>
            </h1>
            <p className="mb-10 max-w-sm text-base text-white/50">
              Binlerce araç, anında kiralama, 7/24 destek. Seyahat planın ne olursa olsun yanındayız.
            </p>

            {/* Feature list */}
            <ul className="flex flex-col gap-4">
              {[
                { icon: '⚡', text: 'Saniyeler içinde araç bul ve kirala' },
                { icon: '🔒', text: 'Uçtan uca şifreli güvenli ödeme' },
                { icon: '🚗', text: '1.000+ araç, 50+ şehirde hizmet' },
              ].map(item => (
                <li key={item.text} className="flex items-center gap-3">
                  <span className="flex h-9 w-9 shrink-0 items-center justify-center rounded-xl border border-white/10 bg-white/[0.06] text-base backdrop-blur-sm">
                    {item.icon}
                  </span>
                  <span className="text-sm text-white/60">{item.text}</span>
                </li>
              ))}
            </ul>
          </div>

          {/* Mock araç kartları — dekoratif */}
          <div className="flex gap-3">
            {[
              { brand: 'BMW', model: '5 Serisi', price: '4.200', year: '2023' },
              { brand: 'Mercedes', model: 'C Serisi', price: '3.800', year: '2024' },
            ].map(car => (
              <div
                key={car.brand}
                className="flex-1 rounded-2xl border border-white/10 bg-white/[0.05] p-4 backdrop-blur-sm"
              >
                <div className="mb-3 flex h-16 items-center justify-center rounded-xl bg-white/[0.05]">
                  <svg className="h-9 w-9 text-white/20" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1} d="M9 17a2 2 0 11-4 0 2 2 0 014 0zM19 17a2 2 0 11-4 0 2 2 0 014 0z" />
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1} d="M13 16V6a1 1 0 00-1-1H4a1 1 0 00-1 1v10l2.083.694M13 16H9m4 0h2.083M13 16v-5a2 2 0 012-2h3l2 4v3h-2.083M13 11h4" />
                  </svg>
                </div>
                <p className="text-xs font-semibold text-white/80">{car.brand} {car.model}</p>
                <p className="text-xs text-white/30">{car.year}</p>
                <p className="mt-1 text-sm font-bold text-violet-400">{car.price} ₺<span className="text-xs font-normal text-white/30">/gün</span></p>
              </div>
            ))}
          </div>
        </div>
      </div>

      {/* ── Sağ panel — Form ── */}
      <div className="relative flex flex-1 flex-col items-center justify-center overflow-hidden px-6 py-12 lg:px-16">
        {/* Mobilde aurora */}
        <div className="orb-3 pointer-events-none absolute -top-20 -right-20 h-[300px] w-[300px] rounded-full bg-violet-600/20 blur-[90px] lg:hidden" />

        <div className="relative z-10 w-full max-w-sm">
          {/* Mobil logo */}
          <div className="mb-8 text-center lg:hidden">
            <Link href="/">
              <span className="bg-gradient-to-r from-violet-400 via-blue-400 to-cyan-400 bg-clip-text text-2xl font-bold text-transparent">
                WanderSync
              </span>
            </Link>
          </div>

          <div className="mb-8">
            <h2 className="text-2xl font-bold text-white">Tekrar hoş geldin</h2>
            <p className="mt-1 text-sm text-white/40">Hesabına giriş yap ve keşfe devam et</p>
          </div>

          <form onSubmit={handleSubmit} className="flex flex-col gap-4">
            <div className="flex flex-col gap-1.5">
              <label className="text-xs font-medium uppercase tracking-widest text-white/40">
                Kullanıcı Adı
              </label>
              <input
                type="text"
                value={username}
                onChange={e => setUsername(e.target.value)}
                required
                placeholder="kullaniciadiniz"
                className="rounded-xl border border-white/10 bg-white/[0.07] px-4 py-3 text-sm text-white placeholder:text-white/20 outline-none transition-all focus:border-violet-500/60 focus:bg-white/10 focus:ring-2 focus:ring-violet-500/20"
              />
            </div>

            <div className="flex flex-col gap-1.5">
              <label className="text-xs font-medium uppercase tracking-widest text-white/40">
                Şifre
              </label>
              <input
                type="password"
                value={password}
                onChange={e => setPassword(e.target.value)}
                required
                placeholder="••••••••"
                className="rounded-xl border border-white/10 bg-white/[0.07] px-4 py-3 text-sm text-white placeholder:text-white/20 outline-none transition-all focus:border-violet-500/60 focus:bg-white/10 focus:ring-2 focus:ring-violet-500/20"
              />
            </div>

            {error && (
              <div className="flex items-center gap-2 rounded-xl border border-red-500/20 bg-red-500/10 px-4 py-3 text-sm text-red-400">
                <svg className="h-4 w-4 shrink-0" fill="currentColor" viewBox="0 0 20 20">
                  <path fillRule="evenodd" d="M10 18a8 8 0 100-16 8 8 0 000 16zM8.707 7.293a1 1 0 00-1.414 1.414L8.586 10l-1.293 1.293a1 1 0 101.414 1.414L10 11.414l1.293 1.293a1 1 0 001.414-1.414L11.414 10l1.293-1.293a1 1 0 00-1.414-1.414L10 8.586 8.707 7.293z" clipRule="evenodd" />
                </svg>
                {error}
              </div>
            )}

            <button
              type="submit"
              disabled={loading}
              className="mt-2 flex items-center justify-center rounded-xl bg-gradient-to-r from-violet-600 to-blue-600 py-3 text-sm font-semibold text-white shadow-lg shadow-violet-500/25 transition-all hover:from-violet-500 hover:to-blue-500 hover:shadow-violet-500/40 disabled:opacity-50"
            >
              {loading ? (
                <>
                  <svg className="mr-2 h-4 w-4 animate-spin" fill="none" viewBox="0 0 24 24">
                    <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4" />
                    <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4z" />
                  </svg>
                  Giriş yapılıyor...
                </>
              ) : 'Giriş Yap →'}
            </button>
          </form>

          <div className="mt-6 flex items-center gap-3">
            <div className="h-px flex-1 bg-white/10" />
            <span className="text-xs text-white/30">hesabın yok mu?</span>
            <div className="h-px flex-1 bg-white/10" />
          </div>

          <Link
            href="/auth/register"
            className="mt-4 flex w-full items-center justify-center rounded-xl border border-white/10 bg-white/[0.05] py-3 text-sm font-medium text-white/70 transition-all hover:border-white/20 hover:bg-white/[0.09] hover:text-white"
          >
            Yeni hesap oluştur
          </Link>
        </div>
      </div>
    </div>
  )
}
