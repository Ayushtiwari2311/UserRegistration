﻿using Microsoft.AspNetCore.Http;

namespace Application.Helpers.Image
{
	public interface IImageHelper
	{
		Task<string> SaveImageAsync(IFormFile imageFile, string folderName);
		string GenerateImageUrl(string? folderName = null, string? fileName = null);
        Task<bool> DeleteImage(string folderName, string fileName);
		Task<string> UpdateImageAsync(IFormFile newImageFile, string folderName, string oldFileName = null);
		Task<string> GetImage(string folderName, string fileName);
	}
}
