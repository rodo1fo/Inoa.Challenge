using FluentAssertions;
using Inoa.Challenge.Console.Application.Models;
using Inoa.Challenge.Console.Application.UseCases.StockMonitorUseCase;
using Inoa.Challenge.Console.Application.UseCases.StockMonitorUseCase.Gateways.YahooFinanceGateway;
using Inoa.Challenge.Console.Application.UseCases.StockMonitorUseCase.Models;
using Inoa.Challenge.Console.Application.UseCases.StockMonitorUseCase.Output;
using Microsoft.Extensions.Logging;
using Moq;

namespace Inoa.Challenge.Console.UnitTest;

public class StockMonitorUseCaseTest
{
    Mock<ILogger<StockMonitorUseCase>> loggerMock;
    Mock<IYahooFinanceApi> yahooApiMock;
    StockMonitorUseCase stockMonitorUseCase;

    public StockMonitorUseCaseTest()
    {
        loggerMock = new Mock<ILogger<StockMonitorUseCase>>();
        yahooApiMock = new Mock<IYahooFinanceApi>();
        stockMonitorUseCase = new StockMonitorUseCase(loggerMock.Object, yahooApiMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldNotNotify_When_PriceIsInTheMiddle()
    {
        // Arrange
        var args = new string[] { "-a", "PETR4", "-s", "30.00", "-b", "10.00" };
        var model = StockMonitorModel.FromArgs(args);

        var apiResponse = new YahooFinanceResponse
        {
            QuoteResponse = new QuoteResponse
            {
                Result = new List<Quote> { new Quote { regularMarketPrice = 25 } }
            }
        };

        yahooApiMock.Setup(api => api.GetStockAsync(It.IsAny<string>()))
                    .ReturnsAsync(apiResponse);

        // Act
        var result = await stockMonitorUseCase.ExecuteAsync(model);

        // Assert
        result.Should().NotBeNull().And.BeOfType<StockMonitorOutput>();
        result.ShouldNotify.Should().BeFalse();
    }

    [Fact]
    public async Task ExecuteAsync_ShouldNotifySell_When_MarketPriceIsAbove()
    {
        // Arrange
        var args = new string[] { "-a", "PETR4", "-s", "30.00", "-b", "10.00" };
        var model = StockMonitorModel.FromArgs(args);

        var apiResponse = new YahooFinanceResponse
        {
            QuoteResponse = new QuoteResponse
            {
                Result = new List<Quote> { new Quote { regularMarketPrice = 40 } }
            }
        };

        yahooApiMock.Setup(api => api.GetStockAsync(It.IsAny<string>()))
                    .ReturnsAsync(apiResponse);

        // Act
        var result = await stockMonitorUseCase.ExecuteAsync(model);

        // Assert
        result.Should().NotBeNull().And.BeOfType<StockMonitorOutput>();
        result.ShouldNotify.Should().BeTrue();
        result.Operation.Should().Be(OperationType.Sell);
    }


    [Fact]
    public async Task ExecuteAsync_ShouldNotifyBuy_When_MarketPriceIsUnder()
    {
        // Arrange
        var args = new string[] { "-a", "PETR4", "-s", "30.00", "-b", "10.00" };
        var model = StockMonitorModel.FromArgs(args);


        var apiResponse = new YahooFinanceResponse
        {
            QuoteResponse = new QuoteResponse
            {
                Result = new List<Quote> { new Quote { regularMarketPrice = 10 } }
            }
        };

        yahooApiMock.Setup(api => api.GetStockAsync(It.IsAny<string>()))
                    .ReturnsAsync(apiResponse);

        // Act
        var result = await stockMonitorUseCase.ExecuteAsync(model);

        // Assert
        result.Should().NotBeNull().And.BeOfType<StockMonitorOutput>();
        result.ShouldNotify.Should().BeTrue();
        result.Operation.Should().Be(OperationType.Buy);
    }
}