using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Domain.Exceptions.NotFound
{
    public class UserNotFoundException(string email) : NotFoundException($"User Associated With Email {email} Does Not Exist!")
    {
    }
}
