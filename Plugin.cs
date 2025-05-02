using ClassIsland.Core;
using ClassIsland.Core.Abstractions;
using ClassIsland.Core.Attributes;
using ClassIsland.Core.Extensions.Registry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Sentryer;

[PluginEntrance]
public class Plugin : PluginBase {
    public override void Initialize(HostBuilderContext context,IServiceCollection services) {
        Config.Instance = new Config(PluginConfigFolder);
        if (Config.Instance.Data.IsSettingsPageShowed) services.AddSettingsPage<SettingsPage>();
        if (Config.Instance.Data.Dsn == string.Empty) return;
        SentrySdk.Init(o => {
            o.Dsn = Config.Instance.Data.Dsn;
            o.Release = string.Format(Config.Instance.Data.Release, AppBase.AppVersion, AppBase.AppCodeName);
            o.Distribution = string.Format(Config.Instance.Data.Distribution, AppBase.AppVersion, AppBase.AppCodeName);
        });
        AppBase.Current.DispatcherUnhandledException += (_,e) => {
            SentrySdk.CaptureException(e.Exception);
        };
    }
}