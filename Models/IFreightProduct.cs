namespace FancyFreight.Models
{
    public interface IFreightProduct
    {
        string Id { get; }
        int MaxWeightInGrams { get; }
        int MaxLengthInMillimeters { get; }
        int MaxWidthInMillimeters { get; }
        int MaxHeightInMillimeters { get; }
        double PriceWithoutVat { get; set; }
    }
}
