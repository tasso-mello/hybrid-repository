namespace core.hybrid.repository.Utilities
{
    using core.hybrid.repository.Enums;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;

    public static class Communication
	{
		/// <summary>
		///		Connect to simple service
		/// </summary>
		/// <param name="url">string url</param>
		/// <param name="requestType">type of request</param>
		/// <param name="body">request body</param>
		/// <param name="bodyType">type of body</param>
		/// <param name="parameters">parameters of request</param>
		/// <returns></returns>
		public static object ConnectToService(string url, RequestType requestType, string body = null, string bodyType = null)
		{
			HttpClient client = new HttpClient();

			try
			{
				return ManagerCall(requestType, client, url, body, bodyType);
			}
			catch (AggregateException e)
			{
				throw new AggregateException($"The service is turn off. {e.Message.Split(',')[0] ?? e.InnerException.Message.Split(',')[0]}");
			}
			catch (Exception e)
			{
				throw new Exception($"Requisição inválida. Detalhes: {e.Message ?? e.InnerException.Message}");
			}
			finally
			{
				client.Dispose();
			}
		}

		/// <summary>
		///		Connect to service without credentials
		/// </summary>
		/// <param name="url">string url</param>
		/// <param name="requestType">type of request</param>
		/// <param name="objectResult">expected success object result</param>
		/// <param name="objectErrorResult">expected error object result</param>
		/// <param name="objectErrorResultDescription">expected error object description</param>
		/// <param name="body">request body</param>
		/// <param name="bodyType">type of body</param>
		/// <param name="parameters">parameters of request</param>
		/// <returns></returns>
		public static object ConnectToService(string url, string model, RequestType requestType, string objectResult, string objectErrorResult,
											  string objectErrorResultDescription, string body = null, string bodyType = null,
											  string parameters = null)
		{
			HttpClient client = new HttpClient();

			try
			{				
				string tokenEndpoint = url;
				string result = ManagerCall(requestType, client, tokenEndpoint, body, bodyType);

				return GetResult(result, objectResult, objectErrorResult, objectErrorResultDescription, model);
			}
			catch (AggregateException e)
			{
				throw new AggregateException($"{objectResult} - The service is turn off. {e.Message.Split(',')[0] ?? e.InnerException.Message.Split(',')[0]}");
			}
			catch (Exception e)
			{
				throw new Exception($"{objectResult} - Requisição inválida. Detalhes: {e.Message ?? e.InnerException.Message}");
			}
			finally
			{
				client.Dispose();
			}
		}

		/// <summary>
		///		Connect to service with headers
		/// </summary>
		/// <param name="url">string url</param>
		/// <param name="requestType">type of request</param>
		/// <param name="objectResult">expected success object result</param>
		/// <param name="objectErrorResult">expected error object result</param>
		/// <param name="objectErrorResultDescription">expected error object description</param>
		/// <param name="body">request body</param>
		/// <param name="bodyType">type of body</param>
		/// <param name="parameters">parameters of request</param>
		/// <returns></returns>
		public static object ConnectToService(string url, string model, RequestType requestType, Dictionary<string, string> headers, string objectResult, string objectErrorResult,
											  string objectErrorResultDescription, string body = null, string bodyType = null, string parameters = null)
		{
			HttpClient client = new HttpClient();

			try
			{
				foreach (var header in headers)
					client.DefaultRequestHeaders.Add(header.Key, header.Value);

				string tokenEndpoint = url;
				string result = ManagerCall(requestType, client, tokenEndpoint, body, bodyType);

				return GetResult(result, objectResult, objectErrorResult, objectErrorResultDescription, model);
			}
			catch (AggregateException e)
			{
				throw new AggregateException($"{model} - Requisição inválida. {e.Message.Split(',')[0] ?? e.InnerException.Message.Split(',')[0]}");
			}
			catch (Exception e)
			{
				throw new Exception($"{model} - Requisição inválida. Detalhes: {e.Message ?? e.InnerException.Message}");
			}
			finally
			{
				client.Dispose();
			}
		}

		/// <summary>
		///		Connect to service with handler credentials
		/// </summary>
		/// <param name="url">string url</param>
		/// <param name="requestType">type of request</param>
		/// <param name="handler">credentials</param>
		/// <param name="objectResult">expected success object result</param>
		/// <param name="objectErrorResult">expected error object result</param>
		/// <param name="objectErrorResultDescription">expected error object description</param>
		/// <param name="body">request body</param>
		/// <param name="bodyType">type of body</param>
		/// <param name="parameters">parameters of request</param>
		/// <returns></returns>
		public static object ConnectToService(string url, string model, RequestType requestType, HttpClientHandler handler, string objectResult,
											  string objectErrorResult, string objectErrorResultDescription, string body = null, string bodyType = null,
											  string parameters = null)
		{
			HttpClient client = new HttpClient(handler);

			try
			{				
				string tokenEndpoint = url;
				string result = ManagerCall(requestType, client, tokenEndpoint, body, bodyType);

				return GetResult(result, objectResult, objectErrorResult, objectErrorResultDescription, model);
			}
			catch (AggregateException e)
			{
				throw new AggregateException($"{model} - Requisição inválida. {e.Message.Split(',')[0] ?? e.InnerException.Message.Split(',')[0]}");
			}
			catch (Exception e)
			{
				throw new Exception($"{model} - Requisição inválida. {e.Message.Split(',')[0] ?? e.InnerException.Message.Split(',')[0]}");
			}
			finally
			{
				client.Dispose();
			}
		}
	

		private static object GetResult(string result, string objectResult, string objectErrorResult, string objectErrorResultDescription, string model)
		{
			
			if (!string.IsNullOrEmpty(result) )
			{
				result = result.Trim();
				if (result.StartsWith("{") && result.EndsWith("}") ||
				   (result.StartsWith("[") && result.EndsWith("]")))
				{
					JObject jobject = JObject.Parse(result);

					var obj = (jobject != null) ? GetJobjectResultValue(jobject, objectResult, model) : null;

					return (obj == null && jobject?.ToString() == null && jobject[objectResult]?.ToString() == null)
								? throw new Exception(($"{jobject[objectErrorResult]?.ToString()} - {jobject[objectErrorResultDescription]?.ToString()}")
													   ?? (new { Error = new { Code = 400, Message = $"{model} - Requisição inválida." } }).ToString())
								: (obj ?? (object)jobject[objectResult]?.ToString()) ?? jobject;
				}
			}

			return result;
		}

		private static object GetJobjectResultValue(JObject jobject, string objectResult, string model)
		{
			if (model != "Auth")
				return (jobject[objectResult]?.ToList()?.Count > 0 ? jobject[objectResult]?.ToList() : null);
			else
				return jobject[objectResult]?.ToString();
		}

		private static string ManagerCall(RequestType requestType, HttpClient client, string tokenEndpoint, string body = null, string bodyType = null)
		{
			string result = string.Empty;
			StringContent stringContent;

			switch (requestType)
			{
				case RequestType.Get:
					{
						var returnRequest = client.GetAsync(tokenEndpoint).Result;
						result = returnRequest.Content.ReadAsStringAsync().Result;

						break;
					}
				case RequestType.Post:
					{
						stringContent = new StringContent(body, Encoding.UTF8, bodyType);

						var returnRequest = client.PostAsync(tokenEndpoint, stringContent).Result;
						result = returnRequest.Content.ReadAsStringAsync().Result;

						break;
					}
				case RequestType.Put:
					{
						stringContent = new StringContent(body, Encoding.UTF8, bodyType);

						var returnRequest = client.PutAsync(tokenEndpoint, stringContent).Result;
						result = returnRequest.Content.ReadAsStringAsync().Result;

						break;
					}
				case RequestType.Delete:
					{
						var returnRequest = client.DeleteAsync(tokenEndpoint).Result;
						result = returnRequest.Content.ReadAsStringAsync().Result;

						break;
					}
				case RequestType.Patch:
					{
						stringContent = new StringContent(body, Encoding.UTF8, bodyType);

						var returnRequest = client.PatchAsync(tokenEndpoint, stringContent).Result;
						result = returnRequest.Content.ReadAsStringAsync().Result;

						break;

						break;
					}

				default:
					break;
			}

			return result;
		}
	}
}
