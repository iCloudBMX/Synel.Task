using Microsoft.AspNetCore.Http;

namespace Task.Application.Services;

public interface ICsvService
{
    Task<List<T>> ReadDataAsync<T>(IFormFile formFile);
}