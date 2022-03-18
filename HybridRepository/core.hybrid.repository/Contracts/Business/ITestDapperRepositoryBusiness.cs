namespace core.hybrid.repository.Contracts.Business
{
    using core.hybrid.repository.Contracts.Base;
    using core.hybrid.repository.Models;
    using System.Threading.Tasks;

    public interface ITestDapperRepositoryBusiness
    {
        Task<object> GetByDapper(int skip, int take);
    }
}
