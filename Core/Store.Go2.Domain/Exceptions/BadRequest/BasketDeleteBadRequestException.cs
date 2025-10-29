using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Domain.Exceptions.BadRequest
{
    public class BasketDeleteBadRequestException() 
        : BadRequestException("Failed to delete basket: operation not permitted or invalid")
    {
    }
}
