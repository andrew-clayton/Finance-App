using Finance.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace FinanceAppTests
{
    public class TransactionServiceTests
    {
        [Fact]
        public async Task DeleteTransactionAsync_DeletesExpectedTransaction()
        {
            // Arrange
            var mockSet = new Mock<DbSet<ATransaction>>();
            var mockContext = new Mock<FinanceContext>();
            mockContext.Setup(m => m.Transactions).Returns(mockSet.Object);

            var service = new TransactionService(mockContext.Object);
            var transaction = new ATransaction
            {
                Id = 1,
                Value = -20,
                TimeStamp = DateTime.Now,
                IsReoccuring = false,
                Title = "Test Transaction",
                Description = string.Empty,
                Budget = Category.Grocery
            };

            // Act
            await service.DeleteTransaction(transaction.Id);

            // Assert
            mockSet.Verify(m => m.Remove(It.IsAny<ATransaction>()), Times.Once());
            mockContext.Verify(m => m.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once());


        }

    }
}