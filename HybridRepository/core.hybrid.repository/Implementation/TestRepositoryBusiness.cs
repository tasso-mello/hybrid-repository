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

    public class TestRepositoryBusiness : ITestRepositoryBusiness
    {
        #region Attributes

        private readonly ITestRepository _testRepository;
        #endregion Attributes

        #region Constructor

        public TestRepositoryBusiness(IMapper mapper, ITestRepository testRepository)
        {
            ModelExtensions._mapper = mapper;
            _testRepository = testRepository;
        }

        #endregion Constructor

        #region Read

        public async Task<object> Get(int skip, int take)
        {
            var tests = (await _testRepository.GetAll(skip, take)).ToListViewModelTest();

            return Messages.GenerateGenericSuccessObjectMessage("Test", 
                                                                tests, 
                                                                200, 
                                                                skip, 
                                                                take,
                                                                await _testRepository.Count(),
                                                                tests.Count);
        }

        public async Task<object> GetById(Guid id)
            => Messages.GenerateGenericSuccessObjectMessage("Test", (await _testRepository.Get(p => p.Id == id)).ToViewModelTest(), 200);

        public async Task<object> GetByString(string filter)
            => Messages.GenerateGenericSuccessObjectMessage("Test", (await _testRepository.GetMany(GetUserFilterWhereClause(filter))).ToListViewModelTest(), 200);

        #endregion Read

        #region Persist

        public async Task<object> Save(Test test)
        {
            if(await _testRepository.Exists(t => t.Name.ToLower() == test.Name.ToLower()))
                return Messages.GenerateGenericErrorMessage($"Exists the register with name {test.Name}.");
            
            await _testRepository.Add(test.ToEntityTest());
            await _testRepository.SaveChanges();

            return Messages.GenerateGenericSuccessCreatedMessage("Registro salvo com sucesso.");
        }

        public async Task<object> Update(Test test)
        {
            _testRepository.Update(test.ToEntityTest());
            await _testRepository.SaveChanges();

            return Messages.GenerateGenericSuccessMessage("Registro atualizado com sucesso");
        }

        public async Task<object> Delete(Guid id)
        {
            var toDelete = await _testRepository.Get(c => c.Id == id);
            _testRepository.Update(toDelete);
            await _testRepository.SaveChanges();
            
            return Messages.GenerateGenericSuccessMessage("Registro removido com sucesso");
        }

        #endregion Persist

        #region Private Methods

        private Expression<Func<data.hybrid.repository.Entities.Test, bool>> GetUserFilterWhereClause(string filter)
            => p => p.Name.Contains(filter);

        private List<string> GetIncludes()
            => new List<string> { "" };

        #endregion
    }
}
