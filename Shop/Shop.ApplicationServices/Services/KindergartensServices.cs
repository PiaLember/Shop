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
        private readonly IFileServices _fileServices;
        public KindergartensServices
            (ShopContext context, IFileServices fileServices)
        {
            _context = context;
            _fileServices = fileServices;
        }
        public async Task<Kindergarten> Create(KindergartenDto dto)
        {
            Kindergarten kindergarten = new Kindergarten();
            kindergarten.Id = Guid.NewGuid();
            kindergarten.KindergartenName = dto.KindergartenName;
            kindergarten.GroupName = dto.GroupName;
            kindergarten.ChildrenCount = (int)dto.ChildrenCount;
            kindergarten.Teacher = dto.Teacher;
            kindergarten.CreatedAt = DateTime.Now;
            kindergarten.UpdatedAt = DateTime.Now;

            if (dto.Files != null)
            {
                _fileServices.UploadFilesToDatabase(dto, kindergarten);
            }

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
            kindergarten.ChildrenCount = (int)dto.ChildrenCount;
            kindergarten.Teacher = dto.Teacher;
            kindergarten.CreatedAt = dto.CreatedAt;
            kindergarten.UpdatedAt = DateTime.Now;
            if (dto.Files != null)
            {
                _fileServices.UploadFilesToDatabase(dto, kindergarten);
            }

            _context.Kindergartens.Update(kindergarten);
            await _context.SaveChangesAsync();
            return kindergarten;
        }
        public async Task<Kindergarten> Delete(Guid id)
        {
            var kindergartenId = await _context.Kindergartens
                .FirstOrDefaultAsync(x=> x.Id == id);

            var images = await _context.FileToDatabases
                .Where(x => x.KindergartenId == id)
                .Select(y => new FileToDatabaseDto
                {
                    Id = y.Id,
                    ImageTitle = y.ImageTitle,
                    KindergartenId = y.KindergartenId
                }
                ).ToArrayAsync();
            await _fileServices.RemoveFilesFromDatabase(images);

            _context.Kindergartens.Remove(kindergartenId);
            await _context.SaveChangesAsync();
            return kindergartenId;

        }
    }
}
