using FancyFreight.Models;
using System.Collections.Generic;

namespace FancyFreight.Repositories
{
    public interface IFreightProductRepository
    {
        List<IFreightProduct> GetAll(string destinationPostCode, Shipment shipment);
    }
}
