using HealthSync.Services.Identity.Api.Dto;

namespace HealthSync.Services.Identity.Api.Services;

public interface ILoginService<T>
{
    Task<LoginResponse> LoginAsync( 
        LoginRequest request,
        CancellationToken cancellationToken = default);
}