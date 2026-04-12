namespace Radisson_RHG.Services
{
    public interface IRepositoryUserInterface
    {
        User? GetByUserName(string userName);
        User? GetById(int id);
        void Create(User user);
        void Savechanges();
    }
}
