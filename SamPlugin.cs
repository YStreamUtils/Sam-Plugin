
using Sam_Plugin;
using SamSharp;
using YStreamUtils.SDK.Plugin;

[assembly: PluginManifest(
    "Sam-Plugin", 
    "v1.0.0", 
    "SAM Integration, because it's funny",
    "YStreamUtils",
    "SAM-Plugin",
    ["network"],
    ["ysnt-yes"],
    typeof(SamPluginSettings))]

namespace Sam_Plugin;

public class SamPluginSettings
{
    public string Pitch { get; set; } = "64";
    public string Mouth { get; set; } = "128";
    public string Throat { get; set; } = "128";
    public string Speed { get; set; } = "72";
    public bool SingMode { get; set; } = false;
}
public class SamPlugin : PluginBase<SamPluginSettings>
{
    public static class OptionSamples
    {
        private static byte NextByte(int min, int max) => (byte)Random.Shared.Next(min, max + 1);

        public static Options JohnWard => new(pitch: NextByte(64, 75));
        public static Options FatherGarcia => new(pitch: 100, mouth: 100, speed: 85);
        public static Options FatherAllred => new(pitch: 50, mouth: 90, speed: 70);
        public static Options LisaPearson => new(pitch: 40, mouth: 50);
        public static Options GaryMiller => new(pitch: 110, mouth: 150, throat: 100, speed: NextByte(80, 110));
        public static Options AmyMartinPossessed => new(pitch: 150, mouth: 150, throat: 200, speed: 100);
        public static Options AmyMartinHuman => new(pitch: 30, mouth: 50, throat: 10, speed: 100);
        public static Options MichaelDaviesPossessed => new(pitch: 30, mouth: 185, speed: 100);
        public static Options MichaelDaviesHuman => new(pitch: 40, mouth: 188, throat: 245);
        public static Options TiffanyRobinson => new(pitch: 30, mouth: 75, throat: 20, speed: 85);
        public static Options PoliceOfficer => new(pitch: 90, throat: 100, speed: NextByte(60, 70));
        public static Options Thrall => new(pitch: 144, mouth: 170, speed: NextByte(40, 100));
    }
    
    private Options _defaultOptions = new();
    
    protected override Task OnSettingsUpdatedAsync(SamPluginSettings settings)
    {
        _defaultOptions = new Options(
            pitch: ParseByte(settings.Pitch, 64),
            mouth: ParseByte(settings.Mouth, 128),
            throat: ParseByte(settings.Throat, 128),
            speed: ParseByte(settings.Speed, 72),
            singMode: settings.SingMode
        );

        return Task.CompletedTask;
    }

    public byte[] SpeakToBytes(string text, Options? options = null)
    {
        var sam = new Sam(options ?? _defaultOptions);
        return sam.Speak(text);
    }

    public Task<byte[]> SpeakToBytesAsync(string text, Options? options = null)
    {
        var sam = new Sam(options ?? _defaultOptions);
        return sam.SpeakAsync(text);
    }
    
    private static byte ParseByte(string value, byte fallback)
    {
        if (string.IsNullOrWhiteSpace(value)) return fallback;

        var parts = value.Split('-');
        if (parts.Length == 2 && byte.TryParse(parts[0], out var min) && byte.TryParse(parts[1], out var max))
            return (byte)Random.Shared.Next(min, max + 1);

        return byte.TryParse(value, out var exact) ? exact : fallback;
    }

}