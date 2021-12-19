using System;
using System.Collections.Generic;

#nullable disable

namespace BillReader.Entities
{
    public partial class MeterReading
    {
        public int Id { get; set; }
        public string AccountId { get; set; }
        public string MeterReadValue { get; set; }
        public DateTime? MeterReadDt { get; set; }
    }
}
