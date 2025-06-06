﻿using Commerce.Query.Domain.Abstractions.Aggregates;

namespace Commerce.Query.Domain.Entities.Ward
{
    public class Province : AggregateRoot<int>
    {
        public string? Name { get; set; }
        public string? FullName { get; set; }
        public string? LocalId { get; set; }
        public override void Validate()
        {
        }
    }
}