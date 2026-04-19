using Microsoft.AspNetCore.Http.HttpResults;
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

        public IEnumerable<Registration> GetByDateRange(DateTime from, DateTime to)
        {
            return _context.registrations.AsNoTracking()
                .Where(r => r.CreatedOn >= from && r.CreatedOn <= to)
                .OrderBy(r => r.CreatedOn)
                .ToList();
        }

        public Registration GetByMobileOrEmail(string? mobile,string? email)
        {

            if(string.IsNullOrEmpty(mobile) && string.IsNullOrEmpty(email))
            
                return null;


            // If BOTH are provided, match record that satisfies both
            if (!string.IsNullOrEmpty(mobile) && !string.IsNullOrEmpty(email))
            {
                return _context.registrations.FirstOrDefault(x => x.Mobile == mobile && x.Email == email);
            }

            // If only one is provided, match by whichever is given
            return _context.registrations.FirstOrDefault(x =>
            (!string.IsNullOrEmpty(mobile) && x.Mobile == mobile) || (!string.IsNullOrEmpty(email) && x.Email == email));

        }

    }
}
