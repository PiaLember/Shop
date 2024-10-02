using Shop.Data;
using Shop.Core.Dto;
using Shop.Core.Domain;
using Shop.Core.ServiceInterface;
using Microsoft.EntityFrameworkCore;

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
        public async Task<Kindergarten> DetailsAsync(Guid id)
        {
            var result = await _context.Kindergartens
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }
        public async Task<Kindergarten> Update(KindergartenDto dto)
        {
            Kindergarten kindergarten = new();
            kindergarten.Id = dto.Id;
            kindergarten.KindergartenName = dto.KindergartenName;
            kindergarten.GroupName = dto.GroupName;
            kindergarten.ChildrenCount = dto.ChildrenCount;
            kindergarten.Teacher = dto.Teacher;
            kindergarten.CreatedAt = dto.CreatedAt;
            kindergarten.UpdatedAt = DateTime.Now;

            _context.Kindergartens.Update(kindergarten);
            await _context.SaveChangesAsync();
            return kindergarten;
        }
    }
}
