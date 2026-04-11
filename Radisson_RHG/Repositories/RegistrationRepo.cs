using Microsoft.EntityFrameworkCore;
using Radisson_RHG.Controllers;
using System.Collections.Generic;
using System.Linq;

namespace Radisson_RHG.Repositories
{
    public class RegistrationRepo : IRegistrationRepository
    {
        private readonly ApplicationDbContext _context;

        public RegistrationRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Registration> Getall() => _context.registrations.ToList();

        public Registration Getbyid(int id)
        {
            return _context.registrations.Find(id);
        }

        public void Create(Registration registraction)
        {
             _context.registrations.Add(registraction);
            _context.SaveChanges();
        }

        public void Modify(Registration re)
        {
            _context.registrations.Update(re);
            _context.SaveChanges();

        }

        public void Remove(Registration rg)
        {
            _context.registrations.Remove(rg);
            _context.SaveChanges();
        }

        
    }
}
