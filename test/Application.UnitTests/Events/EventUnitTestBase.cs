using Application.UnitTests.Mocks;
using AutoMapper;
using Moq;
using Template.Application.Contrats.Persistence;
using Template.Application.Profiles;

namespace Application.UnitTests.Events
{
    public class EventUnitTestBase
    {
        protected readonly IMapper _mapper;
        protected readonly Mock<IEventRepository> _mockEventRepository;
        protected readonly Mock<ICategoryRepository> _mockCategoryRepository;

        public EventUnitTestBase()
        {
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = configurationProvider.CreateMapper();
            _mockCategoryRepository = new MockCategoryRepository().GetEntityRepository();
            _mockEventRepository = new MockEventRepository().GetEntityRepository();
        }
    }
}
