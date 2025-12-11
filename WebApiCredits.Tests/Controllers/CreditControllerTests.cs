using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApiCredits.Controllers;
using WebApiCredits.DAL;
using WebApiCredits.Models;

namespace WebApiCredits.Tests.Controllers
{
    public class CreditControllerTests
    {
        private readonly Mock<ICreditRepository> _mockRepository;
        private readonly CreditController _controller;

        public CreditControllerTests()
        {
            _mockRepository = new Mock<ICreditRepository>();
            _controller = new CreditController(_mockRepository.Object);
        }

        [Fact]
        public async Task GetAllCredits_ReturnsOkResult_WithCredits()
        {
            List<Credit> credits =
            [
                new() { Id = 1, CreditNumber = "CRD001", CustomerName = "Test Customer" }
            ];

            _mockRepository.Setup(x => x.GetAllCreditsWithInvoicesAsync())
                .ReturnsAsync(credits);

            var result = await _controller.GetAllCredits();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Credit>>(okResult.Value);

            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetCreditSummary_ReturnsOkResult_WithSummary()
        {
            CreditSummary summary = new()
            {
                TotalPaidAmount = 1000,
                TotalAwaitingPaymentAmount = 2000,
                PaidPercentage = 33.33m,
                AwaitingPaymentPercentage = 66.67m
            };

            _mockRepository.Setup(x => x.GetCreditSummaryAsync())
                .ReturnsAsync(summary);

            var result = await _controller.GetCreditSummary();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<CreditSummary>(okResult.Value);

            Assert.Equal(1000, returnValue.TotalPaidAmount);
            Assert.Equal(33.33m, returnValue.PaidPercentage);
        }
    }
}