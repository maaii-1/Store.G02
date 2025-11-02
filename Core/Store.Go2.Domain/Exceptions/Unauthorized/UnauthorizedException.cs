using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Domain.Exceptions.Unauthorized
{
    public class UnauthorizedException() : Exception("Unauthorized Access: User Not Authorized!")
    {
    }
}
