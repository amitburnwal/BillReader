using System;
using System.Collections.Generic;
using System.Text;

namespace BillService
{
    interface IAccountReadingService
    {
        public string ReadBill();
        public bool IsReadingValid();
    }
    class AccountReadingService : IAccountReadingService
    {
        public bool IsReadingValid()
        {
            throw new NotImplementedException();
        }

        public string ReadBill()
        {
            throw new NotImplementedException();
        }
    }
}
