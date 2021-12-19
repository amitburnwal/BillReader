using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillReader.DTO
{
    public class MeterReadingDTO
    {
        public string AccountId { get; set; }
        public string ReadingValue { get; set; }
        public DateTime ReadingDt { get; set; }

        
    }
}
