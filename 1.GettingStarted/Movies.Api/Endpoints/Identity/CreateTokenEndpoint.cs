using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using Movies.Contracts.Requests;

namespace Movies.Api.Endpoints.Identity;

public static class CreateTokenEndpoint
{
    private const string TokenSecret = "ForTheLoveOfGodStoreAndLoadThisSecurely";
    private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(8);
    public const string Name = "CreateToken";

    public static IEndpointRouteBuilder MapTokenEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("token", async (TokenGenerationRequest request) =>
            {
                var tokenHandler = new JwtSecurityTokenHandler(); // Создаем экземпляр обработчика JWT-токенов
                var key = Encoding.UTF8.GetBytes(TokenSecret); // Преобразуем секретный ключ в массив байтов
                // Создаем список утверждений (claims) для JWT-токена
                var claims = new List<Claim>
                {
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Уникальный идентификатор токена
                    new(JwtRegisteredClaimNames.Sub,
                        request.Email), // Тема (subject) токена, обычно это email пользователя
                    new(JwtRegisteredClaimNames.Email, request.Email), // Email пользователя
                    new("userid", request.UserId.ToString()) // Пользовательский идентификатор (userid)
                };
                // Перебираем каждую пару "ключ-значение" в пользовательских утверждениях (custom claims)
                foreach (var claimPair in request.CustomClaims)
                {
                    var jsonElement = (JsonElement)claimPair.Value; // Преобразуем значение утверждения в JsonElement
                    var valueType =
                        jsonElement.ValueKind switch // Определяем тип значения утверждения на основе его JsonValueKind
                        {
                            // Если значение true или false, тип будет boolean
                            JsonValueKind.True => ClaimValueTypes.Boolean,
                            JsonValueKind.False => ClaimValueTypes.Boolean,
                            // Если значение число, тип будет double
                            JsonValueKind.Number => ClaimValueTypes.Double,
                            // Во всех остальных случаях тип будет string
                            _ => ClaimValueTypes.String
                        };
                    // Создаем новое утверждение с ключом, значением и типом значения
                    var claim = new Claim(claimPair.Key, claimPair.Value.ToString()!, valueType);
                    claims.Add(claim); // Добавляем утверждение в список утверждений
                }

                var tokenDescriptor = new SecurityTokenDescriptor // Создаем дескриптор токена (SecurityTokenDescriptor)
                {
                    Subject = new ClaimsIdentity(
                        claims), // Устанавливаем идентификатор субъекта (Subject) с утверждениями (claims)
                    Expires = DateTime.UtcNow.Add(TokenLifetime), // Устанавливаем время истечения токена (Expires)
                    Issuer = "https://id.nickchapsas.com", // Устанавливаем издателя токена (Issuer)
                    Audience = "https://movies.nickchapsas.com", // Устанавливаем аудиторию токена (Audience)
                    // Устанавливаем учетные данные для подписи токена (SigningCredentials)
                    // Используем симметричный ключ и алгоритм HmacSha256 для подписи
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };
                // Создаем токен на основе дескриптора
                var token = tokenHandler.CreateToken(tokenDescriptor);
                // Преобразуем токен в строку
                var jwt = tokenHandler.WriteToken(token);
                return TypedResults.Ok(jwt);
            })
            .WithName(Name);
        return app;
    }
}