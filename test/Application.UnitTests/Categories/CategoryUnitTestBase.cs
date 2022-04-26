using Application.UnitTests.Mocks;
using AutoMapper;
using Moq;
using Template.Application.Contrats.Persistence;
using Template.Application.Profiles;

namespace Application.UnitTests.Categories
{
    public class CategoryUnitTestBase
    {
        protected readonly Mock<ICategoryRepository> _mockCategoryRepository;
        protected readonly IMapper _mapper;

        public CategoryUnitTestBase()
        {
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = configurationProvider.CreateMapper();
            _mockCategoryRepository = new MockCategoryRepository().GetEntityRepository();
        }
    }
}
