namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Rating
    {
        public decimal Rate { get; private set; }
        public int Count { get; private set; }

        protected Rating() { }

        public Rating(decimal rate, int count)
        {
            Rate = rate;
            Count = count;
        }
    }
}
