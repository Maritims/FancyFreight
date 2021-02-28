namespace FancyFreight.Models
{
    public class Shipment
    {
        public int WeightInGrams { get; set; }
        public int LengthInMillimeters { get; set; }
        public int WidthInMillimeters { get; set; }
        public int HeightInMillimeters { get; set; }
    }
}