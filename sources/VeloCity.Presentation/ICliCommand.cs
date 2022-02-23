using System.Threading.Tasks;

namespace DustInTheWind.VeloCity.Presentation
{
    public interface ICliCommand
    {
        Task Execute();
    }
}