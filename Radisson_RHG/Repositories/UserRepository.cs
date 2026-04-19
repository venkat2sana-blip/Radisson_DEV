using Radisson_RHG.Controllers;
using Radisson_RHG.Services;

namespace Radisson_RHG.Repositories
{
    public class UserRepository : IRepositoryUserInterface
    {

        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public User? GetByUserName(string userName) => _db.Users.SingleOrDefault(u => u.UserName == userName);
        public User? GetById(int id) => _db.Users.Find(id);
        public void Create(User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
        }

        public void Savechanges()
        {
            _db.SaveChanges();
        }
    }
}
