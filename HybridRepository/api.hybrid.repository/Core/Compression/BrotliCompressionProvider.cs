namespace api.hybrid.repository.Core.Compression
{
    using Microsoft.AspNetCore.ResponseCompression;
    using System.IO;
    using System.IO.Compression;

    /// <summary>
    /// 
    /// </summary>
    public class BrotliCompressionProvider: ICompressionProvider
	{
		/// <summary>
		/// 
		/// </summary>
		public string EncodingName => "br";
		
		/// <summary>
		/// 
		/// </summary>
		public bool SupportsFlush => true;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="outputStream"></param>
		/// <returns></returns>
		public Stream CreateStream(Stream outputStream) => new BrotliStream(outputStream, CompressionLevel.Optimal, true);
	}
}
