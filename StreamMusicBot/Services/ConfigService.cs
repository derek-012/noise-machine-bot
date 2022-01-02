using Newtonsoft.Json;
using NoiseMachine.Entities;
using System.IO;

namespace NoiseMachine.Services
{
    public class ConfigService
    {
        public Config GetConfig()
        {
            var file = "Config.json";
            var data = File.ReadAllText(file);
            return JsonConvert.DeserializeObject<Config>(data);
        }
    }
}
