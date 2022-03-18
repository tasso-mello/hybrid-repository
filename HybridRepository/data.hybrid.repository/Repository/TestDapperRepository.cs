namespace data.hybrid.repository.Repository
{
    using data.hybrid.repository.Context;
    using data.hybrid.repository.Entities;
    using data.hybrid.repository.Infrastructure;
    using data.hybrid.repository.Service;

    public interface ITestDapperRepository : IRepository<Test> { }

    public class TestDapperRepository : RepositoryBase<Test>, ITestDapperRepository
    {
        public TestDapperRepository(ApplicationDbContext dbContext, IDapperDbService dapperDbService) : base(dbContext, dapperDbService) { }
    }
}
