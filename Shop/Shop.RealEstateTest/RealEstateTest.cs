
using Shop.ApplicationServices.Services;
using Shop.Core.Domain;
using Shop.Core.Dto;
using Shop.Core.ServiceInterface;

namespace Shop.RealEstateTest
{
    public class RealEstateTest :TestBase
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
            //Act
            var result = await Svc<IRealEstatesServices>().Create(dto);
            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task ShouldNot_GetByIdRealEstate_WhereReturnsNotEqual()
        {
            //Arrange
            //Ask for a nonexisting realestate
            Guid wrongGuid = Guid.Parse(Guid.NewGuid().ToString());
            Guid guid = Guid.Parse("0c7b218b-2f8c-45d1-bea8-3c0d843a33c7");


            //Act
            //call a method that is from the realEstateService class
            await Svc<IRealEstatesServices>().DetailsAsync(guid);


            //Assert
            //assertimise võrdlus
            Assert.NotEqual(wrongGuid, guid);
        }
        [Fact]
        public async Task Should_CreateRealEstate_WithFutureDates()
        {
            // Arrange
            RealEstateDto futureDatedRealEstate = new()
            {
                Size = 150,
                Location = "kdkd",
                RoomNumber = 3,
                BuildingType = "öölkjk",
                CreatedAt = DateTime.Now.AddDays(1),
                UpdatedAt = DateTime.Now.AddDays(1)
            };

            // Act
            var result = await Svc<IRealEstatesServices>().Create(futureDatedRealEstate);
            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Should_GetByIdRealEstate_WhenReturnsEqual()
        {
            // Arrange
            Guid dbGuid = Guid.Parse("0c7b218b-2f8c-45d1-bea8-3c0d843a33c7");
            Guid guid = Guid.Parse("0c7b218b-2f8c-45d1-bea8-3c0d843a33c7");


            // Act
            await Svc<IRealEstatesServices>().DetailsAsync(guid);

            // Assert
            Assert.Equal(dbGuid, guid);
        }

        [Fact]
        public async Task Should_DeleteByIdRealEstate_WhenDeleteRealEstate()
        {
            RealEstateDto realEstate = MockRealEstateData();
            //enter data
            var addData = await Svc<IRealEstatesServices>().Create(realEstate);
            //delete data
            var result = await Svc<IRealEstatesServices>().Delete((Guid)addData.Id);

            Assert.Equal(result, addData);

        }

        [Fact]
        public async Task ShouldNot_DeleteByIdRealEstate_WhenDidNotDeleteREalEstate()
        {
            RealEstateDto realEstate = MockRealEstateData();

            var addRealEstate1 = await Svc<IRealEstatesServices>().Create(realEstate);
            var addRealEstate2 = await Svc<IRealEstatesServices>().Create(realEstate);

            var result = await Svc<IRealEstatesServices>().Delete((Guid)addRealEstate2.Id);

            Assert.NotEqual(result.Id, addRealEstate1.Id);

        }

        [Fact]
        public async Task Should_UpdateRealEstate_WhenUpdateData()
        {
            //vaja luua guid, mida hakkame kasutama update puhul

            var guid = new Guid("0c7b218b-2f8c-45d1-bea8-3c0d843a33c7");

            RealEstateDto dto = MockRealEstateData();

            //vaja saada domainist andmed kätte
            //kasutam domaini andmeid
            RealEstate domain = new();

            domain.Id = Guid.Parse("0c7b218b-2f8c-45d1-bea8-3c0d843a33c7");
            domain.Location = "Address123";
            domain.Size = 220;
            domain.RoomNumber = 5;
            domain.BuildingType = "qwerty";
            domain.CreatedAt = DateTime.UtcNow;
            domain.UpdatedAt = DateTime.UtcNow;

            await Svc<IRealEstatesServices>().Update(dto);

            Assert.Equal(domain.Id, guid);
            Assert.DoesNotMatch(domain.Location, dto.Location);
            Assert.DoesNotMatch(domain.RoomNumber.ToString(), dto.RoomNumber.ToString());
            Assert.NotEqual(domain.Size, dto.Size);
            Assert.NotEqual(domain.UpdatedAt, dto.UpdatedAt);
            Assert.NotEqual(domain.CreatedAt, dto.CreatedAt);
            
        }

        [Fact]
        public async Task Should_UpdateRealEstate_WhenUpdateDataVersion2()
        { 
            //kasutame kahte mock andmebaasi ja võrdleme neid omavahel
            RealEstateDto dto = MockRealEstateData();
            await Svc<IRealEstatesServices>().Create(dto);

            RealEstateDto update = MockUpdateRealEstateData();
            await Svc<IRealEstatesServices>().Update(update);

            Assert.DoesNotMatch(update.Location, dto.Location);
            Assert.NotEqual(update.UpdatedAt, dto.UpdatedAt);
            //Assert.Equal(result.CreatedAt, createRealEstate.CreatedAt);
        }

        [Fact]
        public async Task ShouldNot_UpdateRealEstate_WhenNotUpdateData()
        {
            RealEstateDto dto = MockRealEstateData();
            var createRealestate = await Svc<IRealEstatesServices>().Create(dto);

            RealEstateDto nullUpdate = MockNullRealEstate();
            var result = await Svc<IRealEstatesServices>().Update(nullUpdate);

            Assert.NotEqual(createRealestate.Id, result.Id);

        }


        private RealEstateDto MockNullRealEstate()
        {
            RealEstateDto nullDto = new()
            {
                Id = null,
                Size = 200,
                Location = "qwerty",
                RoomNumber = 5,
                BuildingType = "poiu",
                CreatedAt = DateTime.Now.AddYears(1),
                UpdatedAt = DateTime.Now.AddYears(1)
            };
            return nullDto;
        }

        private RealEstateDto MockRealEstateData()
        {
            RealEstateDto realEstate = new()
            {
                Size = 100,
                Location = "asd",
                RoomNumber = 4,
                BuildingType = "qwe",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            return realEstate;
        }

        private RealEstateDto MockUpdateRealEstateData()
        {
            RealEstateDto realEstate = new()
            {
                Size = 200,
                Location = "qwerty",
                RoomNumber = 5,
                BuildingType = "poiu",
                CreatedAt = DateTime.Now.AddYears(1),
                UpdatedAt = DateTime.Now.AddYears(1)            
            };
            return realEstate;
        }
    }
}