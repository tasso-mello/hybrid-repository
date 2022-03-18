namespace api.hybrid.repository.Controllers.Business
{
    using api.hybrid.repository.Controllers.Base;
    using core.hybrid.repository.Contracts.Business;
    using core.hybrid.repository.Models;
    using core.hybrid.repository.Utilities;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [ApiController]
    [Route("[controller]")]
	public class TestHybridController : BaseController, IBaseReadBasicController
    {
        #region Attributes

        private readonly ITestRepositoryBusiness _testBusiness;
        private readonly ITestDapperRepositoryBusiness _testDapperBusiness;

        #endregion

        #region Constructor

        public TestHybridController(ITestRepositoryBusiness packageBusiness, ITestDapperRepositoryBusiness testDapperBusiness)
        {
            _testBusiness = packageBusiness;
            _testDapperBusiness = testDapperBusiness;
        }

        #endregion Constructor

        #region Read

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
            => ToResponse(await _testBusiness.GetById(id), Request.Method);

        [HttpGet("take")]
        public async Task<IActionResult> Get(int skip = 1, int take = 10)
            => ToResponse(await _testBusiness.Get(skip, take), Request.Method);

        [HttpGet("take/bydapper")]
        public async Task<IActionResult> GetByDapper(int skip = 1, int take = 10)
            => ToResponse(await _testDapperBusiness.GetByDapper(skip, take), Request.Method);

        #endregion Read

        #region Persist

        [HttpPost]
        public async Task<IActionResult> Post(Test test)
            => ToResponse(await _testBusiness.Save(test.ToModelPackage()), Request.Method);
        [HttpPut]
        public async Task<IActionResult> Put(Test test)
            => ToResponse(await _testBusiness.Update(test), Request.Method);
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
            => ToResponse(await _testBusiness.Delete(id), Request.Method);

        #endregion Persist
    }
}
