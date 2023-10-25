namespace MyProfile.Extensions
{
	public static class FileHelper
	{
		public static async Task<string> ProcessUploadedFile(this IFormFile photoFile, IWebHostEnvironment hostEnvironment)
		{
			var uploadsFolder = Path.Combine(hostEnvironment.WebRootPath, "images");
			var uniqueFileName = Guid.NewGuid().ToString() + '_' + photoFile.FileName;
			var filePath = Path.Combine(uploadsFolder, uniqueFileName);

			await using (var fileStream = new FileStream(filePath, FileMode.Create))
			{
				await photoFile.CopyToAsync(fileStream);
			}

			return uniqueFileName;
		}

		public static async Task DeleteImageFile(this string filePath, IWebHostEnvironment hostEnvironment)
		{
			filePath = Path.Combine(hostEnvironment.WebRootPath, "images", filePath);

			try
			{
				GC.Collect();
				GC.WaitForPendingFinalizers();
				await Task.Run(() => File.Delete(filePath));
			}
			catch (Exception)
			{
				// ignored
			}
		}
	}
}
