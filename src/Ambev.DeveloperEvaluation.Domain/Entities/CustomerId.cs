﻿namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class CustomerId
    {
        protected CustomerId() { }
        public Guid Value { get; private set; }
        public CustomerId(Guid value) => Value = value;
    }
}
