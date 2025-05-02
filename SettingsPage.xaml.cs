using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ClassIsland.Core.Attributes;
using MaterialDesignThemes.Wpf;

namespace Sentryer;

[SettingsPageInfo("sentryer.main","Sentryer配置",PackIconKind.Satellite,PackIconKind.Satellite)]
public partial class SettingsPage {
    public SettingsPage() {
        Settings = Config.Instance!.Data;
        InitializeComponent();
        Settings.PropertyChanged += Record;
    }
    bool _isChanged;
    void Record(object? sender,PropertyChangedEventArgs e) {
        _isChanged = true;
    }
    
    public Config.ConfigData Settings { get; set; }
    void SettingsPage_OnUnloaded(object sender,RoutedEventArgs e) {
        Settings.PropertyChanged -= Record;
        if (_isChanged) {
            RequestRestart();
        }
    }
}