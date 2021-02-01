using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentTask.Domain.Exceptions
{
    public class UnsupportedFlightIdException : Exception
    {
        public UnsupportedFlightIdException(string id)
            : base($"Flight id \"{id}\" is unsupported.")
        {
        }
    }
}
