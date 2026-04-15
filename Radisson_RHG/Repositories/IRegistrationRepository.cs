using Radisson_RHG.Services;

namespace Radisson_RHG.Repositories
{
    public interface IRegistrationRepository
    {

        public IEnumerable<Registration> Getall();
        public Registration Getbyid(int id);

        public void Create(Registration regist);

        public void Modify(Registration regi);

        public void Remove(Registration re);

        public IEnumerable<Registration> GetByDateRange(DateTime from, DateTime to);
    }
}
