using PontoCalculator.Models;

namespace PontoCalculator.Data
{
    public class PontoRepository : IPontoRepository
    {
        private readonly DatabaseContext _context;

        public PontoRepository(DatabaseContext context) {
            _context = context;
        }
        public Ponto Create(Ponto ponto)
        {
            _context.Pontos.Add(ponto);
            ponto.Id = _context.SaveChanges();
            return ponto;
        }

        public List<Ponto> Get(int userId, int? pontoId = null, bool? today = null)
        {
            if (pontoId != null) 
            {
                return _context.Pontos.Where(x => x.Id == pontoId).ToList();
            }
            if (today == true)
            {
                return _context.Pontos.Where
                (x => x.User_id == userId
                && x.DateTime.Day == DateTime.Today.Day).ToList();
            }
            return _context.Pontos.Where(x => x.User_id  == userId).ToList();

        }


        public void Update(Ponto ponto)
        {
            _context.Pontos.Update(ponto);
            _context.SaveChanges();
        }
    }
}
