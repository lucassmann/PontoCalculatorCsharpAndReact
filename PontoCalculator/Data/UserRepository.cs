using PontoCalculator.Models;
using System.Security.Cryptography;

namespace PontoCalculator.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;
        public UserRepository(DatabaseContext context) { 
            _context = context;
        } 
        public User Create(User user)
        {
            _context.Users.Add(user);
            user.Id = _context.SaveChanges();
            return user;
        }

        public User GetByEmail(string email) 
        {
            return _context.Users.FirstOrDefault(x => x.Email == email);
        }
        public User GetById(int Id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == Id);
        }

        public User GetByPasswordResetToken(string passwordResetToken)
        {
            return _context.Users.FirstOrDefault(x => x.PasswordResetToken == passwordResetToken);
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

    }
}
