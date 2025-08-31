using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.WebApi;
using AutoMapper;
using FluentAssertions;
using Xunit;

public class AutoMapperProfilesTests
{
    public static IEnumerable<object[]> ProfilesData()
    {
        var assemblies = new[]
        {
            typeof(Program).Assembly,          
            typeof(ApplicationLayer).Assembly     
        };

        return assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(Profile).IsAssignableFrom(t) && !t.IsAbstract)
            .Select(t => new object[] { t });
    }

    
    [Theory(Skip = "Ignore mappging test")]
    [MemberData(nameof(ProfilesData))]
    public void Profile_Should_BeValid(Type profileType)
    {
        // Arrange
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(profileType);
        });

        // Act
        Action act = () => config.AssertConfigurationIsValid();

        // Assert
        act.Should().NotThrow($"because the profile {profileType.Name} must be configured correctly");
    }
    
}
