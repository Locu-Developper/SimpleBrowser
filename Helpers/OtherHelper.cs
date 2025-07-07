using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MemoBrowser.Helpers;

internal partial class OtherHelper
{
    [GeneratedRegex(@"^https?://.+$")]
    private static partial Regex HasHttpRegex();

    [GeneratedRegex(@"^(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?\.)+[a-zA-Z]{2,}(?:/.*)?$")]
    private static partial Regex IsDomainRegex();

    [GeneratedRegex(@"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(?::\d+)?(?:/.*)?$")]
    private static partial Regex IsIPAddressRegex();
    public static Uri ParseAddressBarInput(string input)
    {
        // 既にプロトコルがある場合
        if (HasHttpRegex().IsMatch(input))
            return new Uri(input);

        // ドメイン形式の判定（より厳密）
        if (IsDomainRegex().IsMatch(input))
        {
            return new Uri("https://" + input);
        }

        // IPアドレスの場合
        if (IsIPAddressRegex().IsMatch(input))
        {
            return new Uri("http://" + input); // IPアドレスはhttpをデフォルト
        }

        var selectedEngineUrl = AppSettingsManager.Get("DefaultSearchEngineUrl", "google.com/search?q=");

        // URIじゃなかった場合
        return new Uri($"https://{selectedEngineUrl}{Uri.EscapeDataString(input)}");
    }


    public static bool IsInternalUrl(string? url)
    {
        if(string.IsNullOrWhiteSpace(url)) return false;
        return url.StartsWith("app://");
    }

}
