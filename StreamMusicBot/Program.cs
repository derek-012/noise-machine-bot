using System.Threading.Tasks;

namespace NoiseMachine
{
    class Program
    {
        static async Task Main(string[] args)
            => await new NoiseMachineClient().InitializeAsync();
    }
}
