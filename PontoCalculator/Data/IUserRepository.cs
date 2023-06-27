using PontoCalculator.Models;

namespace PontoCalculator.Data
{
    public interface IUserRepository
    {
        User Create(User user);
        User GetByEmail(string email);
        User GetById(int Id);
        User GetByPasswordResetToken(string passwordResetToken);

        void Update(User user);
    }
}
