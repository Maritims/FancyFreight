using System;
using System.Collections.Generic;

namespace FancyFreight.Models
{
    public class Mutation
    {
        public string Name { get; set; }
        public Action<List<IFreightProduct>> Action { get; set; }

        public List<IFreightProduct> Apply(List<IFreightProduct> freightProducts)
        {
            if (freightProducts == null) return null;
            Action(freightProducts);
            return freightProducts;
        }
    }
}
