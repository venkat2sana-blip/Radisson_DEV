namespace Radisson_RHG.Services
{
    public interface IRegistrationInterface
    {
        public IEnumerable<Registration> Getall();

        public Registration Getbyid(int id);

        public Registration Create(Registration registration);

        public Registration Modify(int id, Registration reg);

        public bool Remove(int id);

        public IEnumerable<Registration> GetByDateRange(DateTime from, DateTime to);

        public Registration GetByMobileOrEmail(string mobile, string email);

  
    }
}
