
using Shop.Core.Dto;
using Shop.Core.ServiceInterface;

namespace Shop.RealEstateTest
{
    public class RealEstateTest
    {
        [Fact]
        public async Task ShouldNot_AddEmptyRealEstate_WhenReturnResult()
        {
            //Arrange
            RealEstateDto dto = new();
            
            dto.Size = 100;
            dto.Location = "asd";
            dto.RoomNumber = 2;
            dto.BuildingType = "asd";
            dto.CreatedAt = DateTime.Now;
            dto.UpdatedAt = DateTime.Now;

            var result = await Svc<IRealEstatesServices>().Create(dto);

            Assert.NotNull(result);
        }
    }
}