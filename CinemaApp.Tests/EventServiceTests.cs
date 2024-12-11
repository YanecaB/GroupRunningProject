using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Data;
using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.ViewModels.Event;
using MockQueryable;
using Moq;
using static CinemaApp.Common.EntityValidationMessages;

namespace CinemaApp.Tests;

[TestFixture]
public class Tests
{
    private IEnumerable<Event> eventsData = new List<Event>()
    {
        new Event()
        {
            Id = Guid.Parse("8a2d4c27-39bb-4231-8566-1bb97c3b85a7"),
            Title = "Novogodishen jogging",
            Description = "We will jog at the start of 2025",
            Date = DateTime.Parse("2025-01-01 09:00:00.0000000"),
            Location = "Plovdiv/Bulgaria",
            OrganizerId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
            Distance = 20,
            GroupId = Guid.Parse("089d31fe-18ff-4f24-8d76-cbab61c7c61a"),
            IsDeleted = false
        },
        new Event()
        {
            Id = Guid.Parse("e11c9139-da91-49de-8a0c-8da3ec220a96"),
            Title = "Weekend 10K Run",
            Description = "A friendly 10K race to get your weekend started with some fitness and fun.",
            Date = DateTime.Parse("2024-12-23 10:00:00.0000000"),
            Location = "Golden Gate Park, San Francisco",
            OrganizerId = Guid.Parse("7b242b58-cd6b-4fd9-a3de-29c795527093"),
            Distance = 10,
            GroupId = Guid.Parse("95865a98-7985-40cf-80c1-7ee4cb61f24e"),
            IsDeleted = false
        }
    };

    private Mock<IRepository<Event, Guid>> eventRepository;    
    private Mock<IRepository<ApplicationUserEvent, object>> userEventRepository;

    [SetUp]
    public void Setup()
    {
        this.eventRepository = new Mock<IRepository<Event, Guid>>();
        this.userEventRepository = new Mock<IRepository<ApplicationUserEvent, object>>();
    }

    [Test]
    public async Task GetAllEventsNullFilterNegative()
    {
        IQueryable<Event> moviesMockQueryable = eventsData.BuildMock();
        this.eventRepository
            .Setup(r => r.GetAllAttached())
            .Returns(moviesMockQueryable);

        IEventService eventService = new EventService(eventRepository.Object, userEventRepository.Object);

        Assert.ThrowsAsync<NullReferenceException>(async () =>
        {
            (IEnumerable<EventIndexViewModel> allMoviesActual, int j) = await eventService
                .IndexGetAllAsync(null, null);
        });
    }
}
