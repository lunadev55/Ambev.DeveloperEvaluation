using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    public static class CreateSaleHandlerTestData
    {
        private static readonly Faker Faker = new Faker();

        public static CreateSaleCommand ValidCommand() => new CreateSaleCommand
        {
            SaleNumber = Faker.Random.Replace("SN-#####"),
            Date = Faker.Date.Past().Date,
            CustomerId = Faker.Random.Guid(),
            BranchId = Faker.Random.Guid(),
            Items = new List<CreateSaleItemDto>
            {
                new CreateSaleItemDto
                {
                    ProductId = Faker.Random.Guid(),
                    Quantity  = Faker.Random.Int(1, 4),
                    UnitPrice = Faker.Finance.Amount(1, 100)
                }
            }
        };

        public static CreateSaleCommand TooManyItemsCommand() => new CreateSaleCommand
        {
            SaleNumber = ValidCommand().SaleNumber,
            Date = ValidCommand().Date,
            CustomerId = ValidCommand().CustomerId,
            BranchId = ValidCommand().BranchId,
            Items = new List<CreateSaleItemDto>
            {
                new CreateSaleItemDto
                {
                    ProductId = Faker.Random.Guid(),
                    Quantity  = 21,
                    UnitPrice = Faker.Finance.Amount(1, 100)
                }
            }
        };

        public static Sale ValidSaleEntityFrom(CreateSaleCommand command)
        {
            var sale = new Sale(
                Guid.NewGuid(),
                command.SaleNumber,
                command.Date,
                new CustomerId(command.CustomerId),
                new BranchId(command.BranchId));

            foreach (var dto in command.Items)
                sale.AddItem(dto.ProductId, dto.Quantity, dto.UnitPrice);

            return sale;
        }
    }
}
