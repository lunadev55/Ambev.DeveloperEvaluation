using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class CreateSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly CreateSaleHandler _handler;

        public CreateSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _handler = new CreateSaleHandler(_saleRepository);
        }

        [Fact(DisplayName = "Given valid command When handling Then adds sale and returns Id")]
        public async Task Handle_ValidCommand_AddsSaleAndReturnsId()
        {
            // Arrange
            var command = CreateSaleHandlerTestData.ValidCommand();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Id.Should().NotBe(Guid.Empty);
            await _saleRepository.Received(1).AddAsync(
                Arg.Is<Sale>(s =>
                    s.SaleNumber == command.SaleNumber &&
                    s.Items.Count == command.Items.Count),
                CancellationToken.None);
            await _saleRepository.Received(1).SaveChangesAsync(CancellationToken.None);
        }

        //[Fact(DisplayName = "Given too many items When handling Then throws DomainException")]
        //public async Task Handle_TooManyItems_ThrowsDomainException()
        //{
        //    // Arrange
        //    var command = CreateSaleHandlerTestData.TooManyItemsCommand();

        //    // Act
        //    Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        //    // Assert
        //    var ex = await Assert.ThrowsAsync<DomainException>(act);
        //    ex.Message.Should().Be("Cannot sell more than 20 identical items.");
        //    await _saleRepository.DidNotReceiveWithAnyArgs().AddAsync(default, default);
        //}
    }
}
