using System.ComponentModel;
using System.IO;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Sentryer;

public class Config {
    public static Config? Instance;
    static readonly JsonSerializerOptions Options = new JsonSerializerOptions { WriteIndented = true };
    public Config(string configPath) {
        _workingPath = Path.Combine(configPath,"config.json");
        if (!File.Exists(_workingPath)) {
            File.WriteAllText(_workingPath, JsonSerializer.Serialize(new ConfigData(), Options));
        }
        ConfigData? data = JsonSerializer.Deserialize<ConfigData>(File.ReadAllText(_workingPath));
        Data = data ?? throw new NullReferenceException();
        Data.PropertyChanged += Save;
    }
    void Save(object? sender,PropertyChangedEventArgs e) {
        Save();
    }
    void Save() {
        Console.WriteLine($"{DateTime.Now:G} | info | Sentryer | 写入配置文件");
        File.WriteAllText(_workingPath, JsonSerializer.Serialize(Data, Options));
    }

    readonly string _workingPath;
    public readonly ConfigData Data;
    
    public class ConfigData : ObservableObject {
        string _dsn = string.Empty;
        public string Dsn {
            get => _dsn;
            set {
                if (value == _dsn) return;
                _dsn = value;
                OnPropertyChanged();
            }
        }
    
        string _release = "{1}";
        public string Release {
            get => _release;
            set {
                if (value == _release) return;
                _release = value;
                OnPropertyChanged();
            }
        }
    
        string _distribution = "{0}";
        public string Distribution {
            get => _distribution;
            set {
                if (value == _distribution) return;
                _distribution = value;
                OnPropertyChanged();
            }
        }
    
        bool _isSettingsPageShowed = true;
        public bool IsSettingsPageShowed {
            get => _isSettingsPageShowed;
            set {
                if (value == _isSettingsPageShowed) return;
                _isSettingsPageShowed = value;
                OnPropertyChanged();
            }
        }   
    }
}