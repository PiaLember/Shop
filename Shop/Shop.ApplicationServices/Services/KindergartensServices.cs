using Shop.Data;
using Shop.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Core.Domain;
using Shop.Core.ServiceInterface;

namespace Shop.ApplicationServices.Services
{
   public class KindergartensServices : IKindergartensServices
    {
        private readonly ShopContext _context;
        public KindergartensServices
            (ShopContext context)
        {
            _context = context;
        }
        public async Task<Kindergarten> Create(KindergartenDto dto)
        {
            Kindergarten kindergarten = new Kindergarten();
            kindergarten.Id = Guid.NewGuid();
            kindergarten.KindergartenName = dto.KindergartenName;
            kindergarten.GroupName = dto.GroupName;
            kindergarten.ChildrenCount = dto.ChildrenCount;
            kindergarten.Teacher = dto.Teacher;
            kindergarten.CreatedAt = DateTime.Now;
            kindergarten.UpdatedAt = DateTime.Now;

            await _context.Kindergartens.AddAsync(kindergarten);
            await _context.SaveChangesAsync();
            return kindergarten;
        }
    }
}
