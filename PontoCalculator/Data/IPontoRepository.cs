using PontoCalculator.Models;

namespace PontoCalculator.Data
{
    public interface IPontoRepository
    {
        Ponto Create(Ponto ponto);
        void Update (Ponto ponto);
        List<Ponto> Get(int userId, int? pontoId = null, bool? today = null);
    }
}
