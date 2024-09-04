using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Movies.Api.Swagger;

/// <summary>
/// класс ConfigureSwaggerOptions автоматически настраивает Swagger для отображения документации по всем доступным версиям API в приложении, используя информацию о среде выполнения и версионировании API.
/// Это упрощает процесс документирования API и обеспечивает клиентам актуальную информацию о доступных версиях и методах API.
/// </summary>
public class ConfigureSwaggerOptions:IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;
    private readonly IHostEnvironment _environment;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IHostEnvironment environment)
    {
        _provider = provider;
        _environment = environment;
    }

    /// <summary>
    /// Метод Configure принимает объект SwaggerGenOptions, который используется для настройки генерации Swagger документации.
    ///Внутри метода происходит итерация по всем описаниям версий API, предоставленным _provider.ApiVersionDescriptions.
    /// Для каждого описания версии API создается соответствующая документация Swagger:description.GroupName используется как имя документа.
    /// OpenApiInfo содержит метаданные документации, включая название приложения (_environment.ApplicationName) и версию API (description.ApiVersion.ToString()).
    /// </summary>
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName,
                new OpenApiInfo
                {
                    Title = _environment.ApplicationName,
                    Version = description.ApiVersion.ToString()
                });
        }
    }
}