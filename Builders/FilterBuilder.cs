using FancyFreight.Builders.Interfaces;
using FancyFreight.Models;
using System;

namespace FancyFreight.Builders
{
    public class FilterBuilder : IBuilder<Filter>
    {
        private readonly Filter Filter = new Filter();

        private FilterBuilder() { }

        public static FilterBuilder CreateFilter() => new FilterBuilder();

        public FilterBuilder WithCriteria(string name, Func<IFreightProduct, bool> expression)
        {
            if (Filter.Criteria.ContainsKey(name)) throw new ArgumentException($"Criteria names must be unique. A criteria by the name {name} already exists", nameof(name));

            Filter.Criteria.Add(name, expression);
            return this;
        }

        public Filter Build() => Filter;
    }
}
