using System;

namespace DomainServices.OfferService.Abstraction.Exceptions
{
    public class SimulationServiceFatalException : CIS.Core.Exceptions.BaseCisException
    {
        public SimulationServiceFatalException(int code, string message)
            : base(code, message) 
        { }
    }
}
