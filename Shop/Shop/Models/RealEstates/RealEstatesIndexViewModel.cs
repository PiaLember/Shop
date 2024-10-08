﻿namespace Shop.Models.RealEstates
{
    public class RealEstatesIndexViewModel
    {
        public Guid? Id { get; set; }
        public string Location { get; set; }
        public int RoomNumber { get; set; }
        public string BuildingType { get; set; }
        public double Size { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
