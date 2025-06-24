namespace API.Helpers.Image
{
	public class ImageHelper(IHttpContextAccessor _httpContextAccessor) : IImageHelper
	{
		private readonly string _path = @"wwwroot/Images";
		private readonly IHttpContextAccessor _httpContextAccessor;

		public async Task<string> SaveImageAsync(IFormFile imageFile, string folderName)
		{
			if (imageFile == null || imageFile.Length == 0)
				throw new ArgumentException("No file provided");

			var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

			var fullPath = Path.Combine(_path, folderName, fileName);

			if (!Directory.Exists(Path.Combine(_path, folderName)))
			{
				Directory.CreateDirectory(Path.Combine(_path, folderName));
			}

			using (var stream = new FileStream(fullPath, FileMode.Create))
			{
				await imageFile.CopyToAsync(stream);
			}
			return fileName;
		}

		public string GenerateImageUrl(string? folderName = null, string? fileName = null)
		{
			string imageUrl;
			var request = _httpContextAccessor.HttpContext.Request;
			var scheme = request.Scheme;
			var host = request.Host.Value;

			imageUrl = $"{scheme}://{host}/api/image/getimage?folderName={folderName}&fileName={fileName}";

			return imageUrl;
		}

		public bool DeleteImage(string folderName, string fileName)
		{
			var fullPath = Path.Combine(_path, folderName, fileName);

			if (System.IO.File.Exists(fullPath))
			{
				System.IO.File.Delete(fullPath);
				return true;
			}

			return false;
		}

		public async Task<string> UpdateImageAsync(IFormFile newImageFile, string folderName, string oldFileName = null)
		{
			if (!string.IsNullOrEmpty(oldFileName))
			{
				DeleteImage(folderName, oldFileName);
			}
			return await SaveImageAsync(newImageFile, folderName);
		}
		public async Task<string> GetImage(string folderName, string fileName)
		{
			var request = _httpContextAccessor.HttpContext.Request;
			var scheme = request.Scheme;
			var host = request.Host.Value;
			var imageUrl = $"{scheme}://{host}/{_path}/{folderName}/{fileName}";

			return imageUrl;
		}

	}
}
