using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public class ProAgilRepository : IProAgilRepository
    {
        private readonly ProAgilContext _proAgilContext;
        public ProAgilRepository(ProAgilContext proAgilContext)
        {
            _proAgilContext = proAgilContext;
            _proAgilContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        }


        //Métodos gerais
        public void Add<T>(T entity) where T : class
        {
            _proAgilContext.Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _proAgilContext.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _proAgilContext.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _proAgilContext.SaveChangesAsync()) > 0;
        }

        //EVENTOS
        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _proAgilContext.Eventos
                .Include(c => c.Lotes)
                .Include(c => c.RedesSociais);

                if(includePalestrantes)
                {
                    query = query
                                .Include(pe => pe.PalestrantesEventos)
                                .ThenInclude(p => p.Palestrante);
                }

            query = query.OrderByDescending(c => c.DataEvento);

            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventosAsyncByTema(string tema, bool includePalestrantes)
        {
             IQueryable<Evento> query = _proAgilContext.Eventos
                .Include(c => c.Lotes)
                .Include(c => c.RedesSociais);

                if(includePalestrantes)
                {
                    query = query
                                .Include(pe => pe.PalestrantesEventos)
                                .ThenInclude(p => p.Palestrante);
                }

            query = query.OrderByDescending(c => c.DataEvento)
                    .Where(c => c.Tema.ToLower().Contains(tema.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Evento> GetEventoAsyncByid(int EventoId, bool includePalestrantes)
        {
             IQueryable<Evento> query = _proAgilContext.Eventos
                .Include(c => c.Lotes)
                .Include(c => c.RedesSociais);

                if(includePalestrantes)
                {
                    query = query
                                .Include(pe => pe.PalestrantesEventos)
                                .ThenInclude(p => p.Palestrante);
                }

            query = query.OrderByDescending(c => c.DataEvento)
                    .Where(c => c.Id == EventoId);

            return await query.FirstOrDefaultAsync();
        }

        //PALESTRANTES

        public async Task<Palestrante> GetPalestranteAsync(int PalestranteId, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _proAgilContext.Palestrantes
                .Include(c => c.RedesSociais);

                if(includeEventos)
                {
                    query = query
                                .Include(pe => pe.PalestrantesEventos)
                                .ThenInclude(p => p.Evento);
                }

            query = query.OrderBy(c => c.Nome)
                    .Where(p => p.Id == PalestranteId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesAsyncByName(string name, bool includeEventos = false)
        {
             IQueryable<Palestrante> query = _proAgilContext.Palestrantes
                .Include(c => c.RedesSociais);

                if(includeEventos)
                {
                    query = query
                                .Include(pe => pe.PalestrantesEventos)
                                .ThenInclude(p => p.Evento);
                }

            query = query.Where(p => p.Nome.ToLower().Contains(name.ToLower()));

            return await query.ToArrayAsync();
        }
    }
}