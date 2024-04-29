using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace Task.Application.Services;

internal class CsvService : ICsvService
{
    public async Task<List<T>> ReadDataAsync<T>(IFormFile formFile)
    {
        using var streamReader = new StreamReader(formFile.OpenReadStream());

        // ignore the firt line
        await streamReader.ReadLineAsync();
        
        PropertyInfo[] propertyInfo = typeof(T).GetProperties();
        
        // Get type properties
        propertyInfo = propertyInfo.TakeLast(propertyInfo.Length - 1).ToArray();
        
        List<T> result = new List<T>();
        
        while (!streamReader.EndOfStream)
        {
            string? row = await streamReader.ReadLineAsync();

            if (row is null)
                continue;

            T entity = Activator.CreateInstance<T>();

            var splittedContent = row.Split(',');

            for (int i = 0; i <= propertyInfo.Count() && i < splittedContent.Length; i++)
                propertyInfo[i].SetValue(entity, splittedContent[i]);

            result.Add(entity);
        }
        return result;
    }
}