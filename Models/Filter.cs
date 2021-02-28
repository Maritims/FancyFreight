using System;
using System.Collections.Generic;
using System.Linq;

namespace FancyFreight.Models
{
    public class Filter
    {
        public Dictionary<string, Func<IFreightProduct, bool>> Criteria = new Dictionary<string, Func<IFreightProduct, bool>>();

        public List<IFreightProduct> Apply(List<IFreightProduct> freightProducts)
        {
            if (freightProducts == null) return null;
            foreach(var criteria in Criteria)
            {
                freightProducts = freightProducts.Where(criteria.Value).ToList();
            }
            return freightProducts;
        }
    }
}
