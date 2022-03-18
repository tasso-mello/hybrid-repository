namespace core.hybrid.repository.Implementation
{
    using AutoMapper;
    using core.hybrid.repository.Properties;
    using core.hybrid.repository.Utilities;
    using core.hybrid.repository.Contracts.Business;
    using core.hybrid.repository.Models;
    using data.hybrid.repository.Repository;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public class TestDapperRepositoryBusiness : ITestDapperRepositoryBusiness
    {
        #region Attributes

        private readonly ITestDapperRepository _testDapperRepository;

        #endregion Attributes

        #region Constructor

        public TestDapperRepositoryBusiness(IMapper mapper, ITestDapperRepository testDapperRepository)
        {
            ModelExtensions._mapper = mapper;
            _testDapperRepository = testDapperRepository;
        }

        #endregion Constructor

        #region Read

        public async Task<object> GetByDapper(int skip, int take)
        {
            var tests = (await _testDapperRepository.GetAll(Queries.SelectTests(), skip, take)).ToListViewModelTest();

            return Messages.GenerateGenericSuccessObjectMessage("Test",
                                                                tests,
                                                                200,
                                                                skip,
                                                                take,
                                                                await _testDapperRepository.Count(Queries.SelectTests(true ,false)),
                                                                tests.Count,
                                                                Queries.SelectTests());
        }

        #endregion Read

        #region Persist

        #endregion Persist

        #region Private Methods

        #endregion
    }
}
