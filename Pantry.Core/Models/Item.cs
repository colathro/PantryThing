// ReSharper disable UnusedMember.Global

using System.Collections.Generic;

namespace Pantry.Core.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public int FoodId { get; set; }
        public Food Food { get; set; }
        public double Weight { get; set; }
        public string Upc { get; set; }
        public int? UnitId { get; set; }
        public Unit Unit { get; set; }
        public ICollection<ItemReservation> ItemReservations { get; set; }
    }
}