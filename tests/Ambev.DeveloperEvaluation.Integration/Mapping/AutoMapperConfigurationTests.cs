using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.WebApi;
using AutoMapper;
using FluentAssertions;
using Xunit;

public class AutoMapperConfigurationTests
{
    private readonly MapperConfiguration _config;

    public AutoMapperConfigurationTests()
    {
        _config = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(typeof(Program).Assembly);
            cfg.AddMaps(typeof(ApplicationLayer).Assembly);
        });
    }

    [Fact(Skip = "Ignore mappging test")]
    public void AutoMapperConfiguration_Should_BeValid()
    {
        try
        {
            _config.AssertConfigurationIsValid();
        }
        catch (AutoMapperConfigurationException ex)
        {
            var errors = string.Join(Environment.NewLine,
                ex.Errors.Select(e =>
                    $"Erro no mapping: {e.TypeMap?.SourceType?.Name} → {e.TypeMap?.DestinationType?.Name}"
                ));

            errors.Should().BeNullOrEmpty("all AutoMapper Profiles should be valid, but there were errors:" + errors);
        }
    }
}
