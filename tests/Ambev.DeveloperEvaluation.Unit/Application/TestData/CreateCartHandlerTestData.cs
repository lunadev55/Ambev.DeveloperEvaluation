using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    public static class CreateCartHandlerTestData
    {
        private static readonly Faker Faker = new Faker();

        public static CreateCartCommand ValidCommand() => new CreateCartCommand
        {
            CartNumber = Faker.Random.Replace("SN-#####"),
            Date = Faker.Date.Past().Date,
            CustomerId = Faker.Random.Guid(),
            Branch = "Downtown Branch",
            Items = new List<CreateCartItemDto>
            {
                new CreateCartItemDto
                {
                    ProductId = Faker.Random.Guid(),
                    Quantity  = Faker.Random.Int(1, 4),
                    UnitPrice = Faker.Finance.Amount(1, 100)
                }
            }
        };

        public static CreateCartCommand TooManyItemsCommand() => new CreateCartCommand
        {
            CartNumber = ValidCommand().CartNumber,
            Date = ValidCommand().Date,
            CustomerId = ValidCommand().CustomerId,
            Branch = ValidCommand().Branch,
            Items = new List<CreateCartItemDto>
            {
                new CreateCartItemDto
                {
                    ProductId = Faker.Random.Guid(),
                    Quantity  = 21,
                    UnitPrice = Faker.Finance.Amount(1, 100)
                }
            }
        };

        public static Cart ValidCartEntityFrom(CreateCartCommand command)
        {
            var cart = new Cart(
                Guid.NewGuid(),
                command.CartNumber,
                command.Date,
                new CustomerId(command.CustomerId),
                command.Branch);

            foreach (var dto in command.Items)
                cart.AddItem(dto.ProductId, dto.Quantity, dto.UnitPrice);

            return cart;
        }
    }
}
