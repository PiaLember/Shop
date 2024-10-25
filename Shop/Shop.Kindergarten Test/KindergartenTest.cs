using Shop.Core.Dto;
using Shop.Core.ServiceInterface;

namespace Shop.Kindergarten_Test
{
    public class KindergartenTest : TestBase
    {
        [Fact]
        public async Task Should_DeleteByIdKindergarten_WhenDeleteKindergarten()
        {
            KindergartenDto kindergarten = MockkindergartenData();
            var addData = await Svc<IKindergartensServices>().Create(kindergarten);
            var result = await Svc<IKindergartensServices>().Delete((Guid)addData.Id);
            Assert.Equal(result, addData);
        }
        [Fact]
        public async Task ShouldNot_DeleteByIdKindergarten_WhenDidNotDeleteKindergarten()
        {
            KindergartenDto kindergarten = MockkindergartenData();
            var addKindergarten1 = await Svc<IKindergartensServices>().Create(kindergarten);
            var addKindergarten2 = await Svc<IKindergartensServices>().Create(kindergarten);

            var result = await Svc<IKindergartensServices>().Delete((Guid)addKindergarten2.Id);

            Assert.NotEqual(result.Id, addKindergarten1.Id);
        }

        [Fact]
        public async Task Should_UpdateKindergarten_WhenUpdateData()
        {
            var guid = new Guid("0c7b218b-2f8c-45d1-bea8-3c0d843a33c7");
            KindergartenDto dto = MockkindergartenData();

            KindergartenDto domain = new();

            domain.Id = Guid.Parse("0c7b218b-2f8c-45d1-bea8-3c0d843a33c7");
            domain.GroupName = "asdf";
            domain.ChildrenCount = 3;
            domain.KindergartenName = "qwerty";
            domain.Teacher = "äölk";
            domain.CreatedAt = DateTime.UtcNow;
            domain.UpdatedAt = DateTime.UtcNow;

            await Svc<IKindergartensServices>().Update(dto);

            Assert.Equal(domain.Id, guid);
            Assert.NotEqual(domain.ChildrenCount, dto.ChildrenCount);
            Assert.DoesNotMatch(domain.Teacher, dto.Teacher);
        }

        [Fact]
        public async Task ShouldNot_UpdateKindergarten_WhenNotUpdateData()
        {
            KindergartenDto dto = MockkindergartenData();
            var createKindergarten = await Svc<IKindergartensServices>().Create(dto);

            KindergartenDto nullUpdate = MockNullKindergartenData();
            var result = await Svc<IKindergartensServices>().Update(nullUpdate);

            Assert.NotEqual(createKindergarten.Id, result.Id);

        }

        private KindergartenDto MockkindergartenData()
        {
            KindergartenDto kindergarten = new()
            {
                GroupName = "Test",
                ChildrenCount = 1,
                KindergartenName = "Test",
                Teacher = "Test",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            return kindergarten;
        }

        private KindergartenDto MockNullKindergartenData()
        {
            KindergartenDto nullDto = new()
            {
                Id = null,
                GroupName = "Qwerty",
                ChildrenCount = 1,
                KindergartenName = "Qwerty",
                Teacher = "Qwerty",
                CreatedAt = DateTime.Now.AddYears(1),
                UpdatedAt = DateTime.Now.AddYears(1)
            };
            return nullDto;
        }
    }
}