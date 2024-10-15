using Microsoft.EntityFrameworkCore;
using Shop.Core.Domain;
using Shop.Core.Dto;
using Shop.Core.ServiceInterface;
using Shop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.ApplicationServices.Services
{
    public class RealEstatesServices : IRealEstatesServices
    {
        private readonly ShopContext _context;
        private readonly IFileServices _fileServices;

        public RealEstatesServices
            (
            ShopContext context,
            IFileServices fileServices
            )
        {
            _context = context;
            _fileServices = fileServices;
        }

        public async Task<RealEstate> Create(RealEstateDto dto)
        {
            RealEstate realestate = new RealEstate();

            realestate.Id = Guid.NewGuid();
            
            realestate.Size = (double)dto.Size;
            realestate.Location = dto.Location;
            realestate.RoomNumber = (int)dto.RoomNumber;
            realestate.BuildingType = dto.BuildingType;
            realestate.CreatedAt = DateTime.Now;
            realestate.UpdatedAt = DateTime.Now;

            if (dto.Files != null)
            {
                _fileServices.UploadFilesToDatabase(dto, realestate);
            }


            await _context.RealEstates.AddAsync(realestate);
            await _context.SaveChangesAsync();

            return realestate;
        }
        public async Task<RealEstate> DetailsAsync(Guid id)
        {
            var result = await _context.RealEstates
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<RealEstate> Update(RealEstateDto dto)
        {
            RealEstate realestate = new();

            realestate.Id = dto.Id;
            realestate.Location = dto.Location;
            realestate.Size = (double)dto.Size;
            realestate.RoomNumber = (int)dto.RoomNumber;
            realestate.BuildingType = dto.BuildingType;
            realestate.CreatedAt = dto.CreatedAt;
            realestate.UpdatedAt = DateTime.Now;

            if (dto.Files != null)
            {
                _fileServices.UploadFilesToDatabase(dto, realestate);
            }


            _context.RealEstates.Update(realestate);
            await _context.SaveChangesAsync();

            return realestate;
        }

        public async Task<RealEstate> Delete(Guid id)
        {
            var realestateId = await _context.RealEstates
                .FirstOrDefaultAsync(x => x.Id == id);

            var images = await _context.FileToDatabases
                .Where(x => x.RealEstateId == id)
                .Select(y => new FileToDatabaseDto
                {
                    Id = y.Id,
                    ImageTitle = y.ImageTitle,
                    RealEstateId = y.RealEstateId
                }
                ).ToArrayAsync();
                
            await _fileServices.RemoveFilesFromDatabase(images);

            _context.RealEstates.Remove(realestateId);
            await _context.SaveChangesAsync();


            return realestateId;
        }

    }
}
