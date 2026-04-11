using Microsoft.AspNetCore.Mvc;
using Radisson_RHG.Repositories;

namespace Radisson_RHG.Services
{
    public class Registractionservices : IRegistrationInterface
    {
        private readonly IRegistrationRepository _reg;

        public Registractionservices(IRegistrationRepository reg)
        {
            _reg = reg;
        }

        public IEnumerable<Registration> Getall() => _reg.Getall();
        public Registration Getbyid(int id)
        {
            return _reg.Getbyid(id);
        }

        public Registration Create(Registration regi)
        {
             _reg.Create(regi);
            return regi;

        }

        public Registration Modify(int id, Registration regist)
        {
            var existing=_reg.Getbyid(id);

            if (existing == null)
                return null;

            existing.Name = regist.Name;
            existing.Email = regist.Email;
            existing.Mobile = regist.Mobile;
            existing.Gender = regist.Gender;
            existing.Age = regist.Age;
            existing.CreatedOn = regist.CreatedOn;
            _reg.Modify(existing);
            return regist;

        }

        public bool Remove(int id)
        {
            var del = _reg.Getbyid(id);
                if (del == null)
                return false;
            _reg.Remove(del);
            return true;
            

        }


    }
}
