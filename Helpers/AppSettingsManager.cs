using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace MemoBrowser.Helpers;
internal class AppSettingsManager
{
    private static readonly string _settingsFileName = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "AppSettings.json");

    public static void Set(string key, string value)
    {
        
        try
        {
            if (!File.Exists(_settingsFileName))
            {
                // ファイルが存在しない場合は新規作成
                File.WriteAllText(_settingsFileName, "{}");
            }

            var jsonRaw = File.ReadAllText(_settingsFileName);
            var json = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonRaw) ?? [];

            json[key] = value;
            var jsonSerialized = JsonSerializer.Serialize(json, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_settingsFileName, jsonSerialized);
        }catch(Exception ex)
        {
            Debug.WriteLine($"Error writing settings: {ex.Message}");
        }
    }

    public static string Get(string key, string defaultValue)
    {
        try
        {
            if (!File.Exists(_settingsFileName))
            {
                // ファイルが存在しない場合はデフォルト値を返す
                return defaultValue;
            }

            var jsonRaw = File.ReadAllText(_settingsFileName);
            var settings = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(jsonRaw);

            return settings?.GetValueOrDefault(key, defaultValue) ?? defaultValue;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error reading settings: {ex.Message}");
            return defaultValue; // エラーが発生した場合はデフォルト値を返す
        }
    }


    public static string DefaultSearchEngine
    {
        get => Get("DefaultSearchEngine", "Google"); // デフォルト値を設定
        set => Set("DefaultSearchEngine", value);
    }
}
