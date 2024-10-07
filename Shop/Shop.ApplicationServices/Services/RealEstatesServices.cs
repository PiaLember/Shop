using Shop.Core.ServiceInterface;
using Shop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.ApplicationServices.Services
{
    public class RealEstatesServices : IRealEstatesServices
    {
        private readonly ShopContext _context;

        public RealEstatesServices
            (
            ShopContext context
            )
        {
            _context = context;
        }
    }
}
