namespace api.hybrid.repository.Controllers.Base
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    ///		
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBasePersistController<T>
	{
		/// <summary>
		///		
		/// </summary>
		/// <param name="jsonObject"></param>
		/// <returns></returns>
		[HttpPost()]
		Task<IActionResult> Post(T jsonObject);
		/// <summary>
		///		
		/// </summary>
		/// <param name="band"></param>
		/// <returns></returns>
		[HttpPut()]
		Task<IActionResult> Put(T jsonObject);
		/// <summary>
		///		
		/// </summary>
		/// <param name="jsonObject"></param>
		/// <returns></returns>
		[HttpDelete]
		Task<IActionResult> Delete(T jsonObject);
	}

	/// <summary>
	///		
	/// </summary>
	public interface IBaseReadBasicController
	{
		/// <summary>
		///		
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		Task<IActionResult> Get(Guid id);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="skip"></param>
		/// <param name="take"></param>
		/// <returns></returns>
		[HttpGet("take")]
		Task<IActionResult> Get(int skip = 1, int take = 10);
	}

	/// <summary>
	///		
	/// </summary>
	public interface IBaseReadFullController
	{
		/// <summary>
		///		
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		Task<IActionResult> Get(Guid id);
		/// <summary>
		///		
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		[HttpGet("{name}")]
		Task<IActionResult> GetByName(string name);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="skip"></param>
		/// <param name="take"></param>
		/// <returns></returns>
		[HttpGet("take")]
		Task<IActionResult> Get(int skip = 1, int take = 10);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="filter"></param>
		/// <param name="skip"></param>
		/// <param name="take"></param>
		/// <returns></returns>
		[HttpGet("filter/{filter}")]
		Task<IActionResult> Get(string filter, int skip = 1, int take = 10);
	}

	/// <summary>
	/// 
	/// </summary>
	public class BaseController : ControllerBase
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        protected IActionResult ToResponse(object result, string method)
			=> JsonConvert.SerializeObject(result).Contains("error") ? HttpErrorStatusCodeResult(result, method) : HttpSuccessStatusCodeResult(result, method);

		private IActionResult HttpErrorStatusCodeResult(object result, string method = null) => BadRequest(result);

		private IActionResult HttpSuccessStatusCodeResult(object result, string method)
		{
			switch (method)
			{
				case "POST":
					return Created(string.Empty, result);
				default:
					return Ok(result);
			}
		}
    }
}
