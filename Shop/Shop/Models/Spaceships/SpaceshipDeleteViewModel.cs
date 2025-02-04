﻿namespace Shop.Models.Spaceships
{
    public class SpaceshipDeleteViewModel
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Typename { get; set; }
        public string? SpaceshipModel { get; set; }
        public DateTime BuiltDate { get; set; }
        public int Crew { get; set; }
        public int EnginePower { get; set; }
        public List<ImageViewModel> Image { get; set; } = new List<ImageViewModel>();

        //only in database
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
