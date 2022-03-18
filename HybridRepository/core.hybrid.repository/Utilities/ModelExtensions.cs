namespace core.hybrid.repository.Utilities
{
    using AutoMapper;
    using core.hybrid.repository.Models;
    using System.Collections.Generic;
    using System.Linq;

    public static class ModelExtensions
    {
        public static IMapper _mapper;

        #region Test
        public static Test ToModelPackage(this Test test)
            => _mapper.Map<Test, Test>(test);

        public static data.hybrid.repository.Entities.Test ToEntityTest(this Test test)
            => _mapper.Map<Test, data.hybrid.repository.Entities.Test>(test);

        public static Test ToViewModelTest(this data.hybrid.repository.Entities.Test test)
            => new Test
            {
                Id = test.Id,
                Name = test.Name.ToUpper()
            };

        public static List<Test> ToListViewModelTest(this IEnumerable<data.hybrid.repository.Entities.Test> test)
            => test.Select(t => new Test
            {
                Id = t.Id,
                Name = t.Name.ToUpper()
            }).ToList();

        #endregion Test
    }
}
