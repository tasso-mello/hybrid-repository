namespace core.hybrid.repository.Contracts.Business
{
    using core.hybrid.repository.Contracts.Base;
    using core.hybrid.repository.Models;
    using System.Threading.Tasks;

    public interface ITestRepositoryBusiness : IGenericReadBusiness, IGenericPersistBusiness<Test> { }
}
