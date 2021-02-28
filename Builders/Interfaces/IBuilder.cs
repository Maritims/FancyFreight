namespace FancyFreight.Builders.Interfaces
{
    public interface IBuilder<T> where T : class
    {
        T Build();
    }
}
