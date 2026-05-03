using AspNetApp.Dto;
using AspNetApp.Entity;
using AspNetApp.Mapping;
using AutoFixture;
using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;

namespace AspNetAppTests.Mapping;

public class AppToDbEndpointsProfileTests
{
    private IMapper _mapper = null!;
    protected Fixture _fixture;

    public AppToDbEndpointsProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AppToDbEndpointsProfile>();
        }, new NullLoggerFactory());

        config.AssertConfigurationIsValid();
        _mapper = config.CreateMapper();

        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Test]
    public async Task MoveSpaceshipRequest_To_Star_ShouldMapCorrectly()
    {
        // Arrange
        var request = _fixture.Build<MoveSpaceshipRequest>().Create();

        // Act
        var star = _mapper.Map<Star>(request);

        // Assert
        await Assert.That(star).IsNotDefault();
        await Assert.That(star.Radius).IsEqualTo(request.NewRadius);
        await Assert.That(star.AngleMilliradians).IsEqualTo(request.NewAngle);
    }

    [Test]
    public async Task MoveSpaceshipRequest_To_Star_WithZeroValues_ShouldMapCorrectly()
    {
        // Arrange
        var request = _fixture.Build<MoveSpaceshipRequest>()
            .With(r => r.NewAngle, 0)
            .With(r => r.NewRadius, 0)
            .Create();

        // Act
        var star = _mapper.Map<Star>(request);

        // Assert
        // await Assert.That(star).IsNotDefault();
        await Assert.That(star.Radius).IsEqualTo(0);
        await Assert.That(star.AngleMilliradians).IsEqualTo(0);
    }

    [Test]
    public async Task MoveSpaceshipRequest_To_Star_WithMaxValues_ShouldMapCorrectly()
    {
        // Arrange
        var request = _fixture.Build<MoveSpaceshipRequest>()
            .With(r => r.NewAngle, int.MaxValue)
            .With(r => r.NewRadius, int.MaxValue)
            .Create();

        // Act
        var star = _mapper.Map<Star>(request);

        // Assert
        await Assert.That(star).IsNotDefault();
        await Assert.That(star.Radius).IsEqualTo(request.NewRadius);
        await Assert.That(star.AngleMilliradians).IsEqualTo(request.NewAngle);
    }

    [Test]
    public async Task MoveSpaceshipRequest_To_Star_ShouldIgnoreVisitedByShips()
    {
        // Arrange
        var request = _fixture.Build<MoveSpaceshipRequest>().Create();

        // Act
        var star = _mapper.Map<Star>(request);

        // Assert
        await Assert.That(star).IsNotDefault();
        await Assert.That(star.VisitedByShips).IsNull();
    }
}