using System.Collections.Concurrent;

namespace Application.Services;

public class TempTokenService
{
    private static readonly ConcurrentDictionary<string, TempTokenInfo> _tokens = new();

    public string GenerateTempToken(int usuarioId)
    {
        var tempToken = Guid.NewGuid().ToString();
        var info = new TempTokenInfo
        {
            UsuarioId = usuarioId,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10)
        };
        _tokens.TryAdd(tempToken, info);
        return tempToken;
    }

    public TempTokenInfo? ValidateTempToken(string tempToken)
    {
        if (_tokens.TryGetValue(tempToken, out var info))
        {
            if (info.ExpiresAt > DateTime.UtcNow)
            {
                _tokens.TryRemove(tempToken, out _);
                return info;
            }
            _tokens.TryRemove(tempToken, out _);
        }
        return null;
    }
}

public class TempTokenInfo
{
    public int UsuarioId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
}