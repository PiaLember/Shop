namespace Shop.Models.Spaceships
{
    public class SpaceshipsIndexViewModel
    {
        public Guid? Id { get; set; }

        public String Name { get; set; }

        public String Typename { get; set; }

        public int Crew { get; set; }

        public DateTime BuiltDate { get; set; }
    }
}
