using AutoFixture.Xunit2;

namespace Betatalks.UnitTesting.Api.UnitTests.AutoMoq;

public class InlineAutoMoqDataAttribute : InlineAutoDataAttribute
{
    public InlineAutoMoqDataAttribute(params object[] parameters)
        : base(new AutoMoqDataAttribute(), parameters)
    { }
}
