import Link from 'next/link'

export default function Home() {
  return (
    <div className="relative min-h-screen overflow-hidden bg-[#0d0e1a] text-white">
      {/* Aurora orbs */}
      <div className="orb-1 pointer-events-none absolute -top-40 -left-40 h-[600px] w-[600px] rounded-full bg-violet-600/25 blur-[120px]" />
      <div className="orb-2 pointer-events-none absolute top-20 -right-40 h-[500px] w-[500px] rounded-full bg-blue-500/20 blur-[110px]" />
      <div className="orb-3 pointer-events-none absolute bottom-0 left-1/2 h-[400px] w-[400px] rounded-full bg-cyan-500/15 blur-[100px]" />

      {/* Noise texture */}
      <div className="pointer-events-none absolute inset-0 opacity-[0.03]"
        style={{ backgroundImage: `url("data:image/svg+xml,%3Csvg viewBox='0 0 256 256' xmlns='http://www.w3.org/2000/svg'%3E%3Cfilter id='noise'%3E%3CfeTurbulence type='fractalNoise' baseFrequency='0.9' numOctaves='4' stitchTiles='stitch'/%3E%3C/filter%3E%3Crect width='100%25' height='100%25' filter='url(%23noise)' opacity='1'/%3E%3C/svg%3E")` }}
      />

      {/* Navbar */}
      <nav className="relative z-10 flex items-center justify-between px-8 py-6">
        <span className="bg-gradient-to-r from-violet-400 via-blue-400 to-cyan-400 bg-clip-text text-xl font-bold text-transparent">
          WanderSync
        </span>
        <div className="flex items-center gap-3">
          <Link
            href="/auth/login"
            className="rounded-xl px-4 py-2 text-sm font-medium text-white/60 transition-colors hover:text-white"
          >
            Giriş Yap
          </Link>
          <Link
            href="/auth/register"
            className="rounded-xl border border-white/10 bg-white/[0.07] px-4 py-2 text-sm font-medium text-white backdrop-blur-sm transition-all hover:border-white/20 hover:bg-white/[0.12]"
          >
            Kayıt Ol
          </Link>
        </div>
      </nav>

      {/* Hero */}
      <section className="relative z-10 flex flex-col items-center px-6 pb-32 pt-24 text-center">
        <div className="mb-6 inline-flex items-center gap-2 rounded-full border border-violet-500/20 bg-violet-500/10 px-4 py-1.5 text-sm text-violet-300 backdrop-blur-sm">
          <span className="h-1.5 w-1.5 rounded-full bg-violet-400" />
          2026'nın en iyi araç kiralama platformu
        </div>

        <h1 className="mb-6 max-w-3xl text-5xl font-bold leading-tight tracking-tight sm:text-6xl lg:text-7xl">
          Seyahat etmek{' '}
          <span className="bg-gradient-to-r from-violet-400 via-blue-400 to-cyan-400 bg-clip-text text-transparent">
            hiç bu kadar
          </span>
          <br />
          kolay olmamıştı.
        </h1>

        <p className="mb-10 max-w-xl text-lg text-white/50">
          Yüzlerce araç arasından saniyeler içinde seç, güvenli şekilde kirala.
          Nereye gidersen git, WanderSync yanında.
        </p>

        <div className="flex flex-col gap-3 sm:flex-row">
          <Link
            href="/auth/register"
            className="rounded-2xl bg-gradient-to-r from-violet-600 to-blue-600 px-8 py-3.5 text-sm font-semibold text-white shadow-lg shadow-violet-500/30 transition-all hover:from-violet-500 hover:to-blue-500 hover:shadow-violet-500/50"
          >
            Ücretsiz Başla →
          </Link>
          <Link
            href="/auth/login"
            className="rounded-2xl border border-white/10 bg-white/[0.06] px-8 py-3.5 text-sm font-semibold text-white/80 backdrop-blur-sm transition-all hover:border-white/20 hover:bg-white/[0.10]"
          >
            Giriş Yap
          </Link>
        </div>

        {/* Stats */}
        <div className="mt-20 grid grid-cols-3 gap-8 sm:gap-16">
          {[
            { value: '1.000+', label: 'Araç' },
            { value: '50+', label: 'Şehir' },
            { value: '24/7', label: 'Destek' },
          ].map(stat => (
            <div key={stat.label} className="text-center">
              <p className="text-2xl font-bold text-white sm:text-3xl">{stat.value}</p>
              <p className="mt-1 text-sm text-white/40">{stat.label}</p>
            </div>
          ))}
        </div>
      </section>

      {/* Feature cards */}
      <section className="relative z-10 px-6 pb-24">
        <div className="mx-auto max-w-5xl">
          <div className="grid grid-cols-1 gap-4 sm:grid-cols-3">
            {[
              {
                icon: (
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M13 10V3L4 14h7v7l9-11h-7z" />
                ),
                color: 'from-violet-500/20 to-violet-500/5 border-violet-500/20',
                iconColor: 'text-violet-400',
                title: 'Anlık Kiralama',
                desc: 'Dakikalar içinde araç seç, onay beklemeden yola çık.',
              },
              {
                icon: (
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-8.618 3.04A12.02 12.02 0 003 9c0 5.591 3.824 10.29 9 11.622 5.176-1.332 9-6.03 9-11.622 0-1.042-.133-2.052-.382-3.016z" />
                ),
                color: 'from-blue-500/20 to-blue-500/5 border-blue-500/20',
                iconColor: 'text-blue-400',
                title: 'Güvenli & Şifreli',
                desc: 'JWT kimlik doğrulama ile her işleminiz güvence altında.',
              },
              {
                icon: (
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M19 11H5m14 0a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2m14 0V9a2 2 0 00-2-2M5 11V9a2 2 0 012-2m0 0V5a2 2 0 012-2h6a2 2 0 012 2v2M7 7h10" />
                ),
                color: 'from-cyan-500/20 to-cyan-500/5 border-cyan-500/20',
                iconColor: 'text-cyan-400',
                title: 'Geniş Filo',
                desc: 'Ekonomikten lükse, her bütçeye uygun araç seçenekleri.',
              },
            ].map(f => (
              <div
                key={f.title}
                className={`rounded-2xl border bg-gradient-to-br ${f.color} p-6 backdrop-blur-sm`}
              >
                <div className={`mb-4 ${f.iconColor}`}>
                  <svg className="h-7 w-7" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    {f.icon}
                  </svg>
                </div>
                <h3 className="mb-2 font-semibold text-white">{f.title}</h3>
                <p className="text-sm text-white/50">{f.desc}</p>
              </div>
            ))}
          </div>
        </div>
      </section>

      {/* CTA banner */}
      <section className="relative z-10 px-6 pb-24">
        <div className="mx-auto max-w-3xl rounded-3xl border border-white/10 bg-gradient-to-r from-violet-600/20 to-blue-600/20 p-12 text-center backdrop-blur-sm">
          <h2 className="mb-4 text-3xl font-bold">Hemen başla, ücretsiz.</h2>
          <p className="mb-8 text-white/50">Kredi kartı gerekmez. Saniyeler içinde hazır.</p>
          <Link
            href="/auth/register"
            className="inline-block rounded-2xl bg-gradient-to-r from-violet-600 to-blue-600 px-10 py-3.5 text-sm font-semibold text-white shadow-lg shadow-violet-500/30 transition-all hover:from-violet-500 hover:to-blue-500"
          >
            Hesap Oluştur
          </Link>
        </div>
      </section>

      <footer className="relative z-10 border-t border-white/[0.06] py-8 text-center text-sm text-white/20">
        © 2026 WanderSync — Tüm hakları saklıdır
      </footer>
    </div>
  )
}
