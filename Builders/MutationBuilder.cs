using FancyFreight.Builders.Interfaces;
using FancyFreight.Models;
using System;
using System.Collections.Generic;

namespace FancyFreight.Builders
{
    public class MutationBuilder : IBuilder<Mutation>
    {
        private readonly Mutation Mutation = new Mutation();

        private MutationBuilder() { }

        public static MutationBuilder CreateMutation(string name)
        {
            var mutationBuilder = new MutationBuilder();
            mutationBuilder.Mutation.Name = name;
            return mutationBuilder;
        }

        public MutationBuilder WithAction(Action<List<IFreightProduct>> action)
        {
            Mutation.Action = action;
            return this;
        }

        public Mutation Build() => Mutation;
    }
}
