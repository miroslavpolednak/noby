namespace CIS.Core.Data
{
    public interface IActual
    {
        bool IsActual { get; set; }
    }

    public class BaseIsActual : IActual
    {
        public bool IsActual { get; set; }
    }
}
