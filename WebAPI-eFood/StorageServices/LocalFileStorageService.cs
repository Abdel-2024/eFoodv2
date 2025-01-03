﻿using eFood.Infra.Storage.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace eFood.WebAPI.StorageServices
{
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocalFileStorageService(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task DeleteFile(string fileRoute, string containerName)
        {
            if (string.IsNullOrEmpty(fileRoute))
            {
                return Task.CompletedTask;
            }

            var fileName=Path.GetFileName(fileRoute);
            var fileDirectory = Path.Combine(_env.WebRootPath,containerName,fileName);

            if (!Directory.Exists(fileDirectory))
                File.Delete(fileDirectory);

            return Task.CompletedTask;
        }

        public async Task<string> EditFile(string containerName, IFormFile file, string fileRoute)
        {
            await DeleteFile(fileRoute, containerName);

            return await SaveFile(containerName, file);
        }

        public async Task<string> SaveFile(string containerName, IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";

            string routeForDB = string.Empty;

            try
            {
                string folder = Path.Combine(_env.WebRootPath, containerName);

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string route = Path.Combine(folder, fileName);
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);

                    var content = ms.ToArray();

                    await File.WriteAllBytesAsync(route, content);
                }

                var request = _httpContextAccessor.HttpContext.Request;

                var url = $"{request.Scheme}://{request.Host}";
                routeForDB = Path.Combine(url, containerName, fileName).Replace("\\", "/");
            }
            catch (Exception ex)
            {
                
            }
            
            return routeForDB;
        }
    }
}
