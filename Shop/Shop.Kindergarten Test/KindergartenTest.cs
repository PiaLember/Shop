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
    }
}