namespace data.hybrid.repository.Repository
{
    using data.hybrid.repository.Context;
    using data.hybrid.repository.Entities;
    using data.hybrid.repository.Infrastructure;

    public interface ITestRepository : IRepository<Test> { }

    public class TestRepository : RepositoryBase<Test>, ITestRepository
    {
        public TestRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
