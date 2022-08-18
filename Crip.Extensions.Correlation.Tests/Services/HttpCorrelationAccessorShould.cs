using Crip.Extensions.Correlation.Services;
using Microsoft.AspNetCore.Http;

namespace Crip.Extensions.Correlation.Tests.Services;

public class HttpCorrelationAccessorShould
{
    private readonly Mock<ICorrelationService> _correlation = new();
    private readonly Mock<IHttpContextAccessor> _httpContextAccessor = new();

    [Fact, Trait("Category", "Unit")]
    public void Constructor_DoesNotFail()
    {
        var act = () => new HttpCorrelationAccessor(_correlation.Object, _httpContextAccessor.Object);

        act.Should().NotThrow();
    }

    [Fact, Trait("Category", "Unit")]
    public void Constructor_FailsIfCorrelationNotProvided()
    {
        var act = () => new HttpCorrelationAccessor(null!, _httpContextAccessor.Object);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'correlation')");
    }

    [Fact, Trait("Category", "Unit")]
    public void Constructor_FailsIfHttpContextAccessorNotProvided()
    {
        var act = () => new HttpCorrelationAccessor(_correlation.Object, null!);

        act.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'httpContextAccessor')");
    }

    [Fact, Trait("Category", "Unit")]
    public void Get_ProperlyCallsUnderlyingService()
    {
        var accessor = Accessor();
        var httpContext = new DefaultHttpContext();
        MockHttpContext(httpContext);

        accessor.Get();

        _correlation.Verify(correlation => correlation.Get(httpContext), Times.Once);
    }

    [Fact, Trait("Category", "Unit")]
    public void Get_ReturnsUnderlyingServiceValue()
    {
        var accessor = Accessor();
        var httpContext = new DefaultHttpContext();
        MockHttpContext(httpContext);
        MockCorrelationGet("value");

        var result = accessor.Get();

        result.Should().Be("value");
    }

    private void MockCorrelationGet(string result) =>
        _correlation
            .Setup(correlation => correlation.Get(It.IsAny<HttpContext>()))
            .Returns(result);

    private void MockHttpContext(HttpContext httpContext) =>
        _httpContextAccessor
            .Setup(httpAccessor => httpAccessor.HttpContext)
            .Returns(httpContext);

    private HttpCorrelationAccessor Accessor() => new(_correlation.Object, _httpContextAccessor.Object);
}