using BillReader.DTO;
using BillReader.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BillReader.Services
{
    public interface IAccountReadingService
    {
        public string ProcessMeterReading(IFormFile file);
    }
    public class AccountReadingService : IAccountReadingService
    {
        private testContext context;
        public AccountReadingService(testContext dbContext)
        {
            context = dbContext;
        }

        public string ProcessMeterReading(IFormFile file)
        {
            string returnMessage = string.Empty;
            List<MeterReadingDTO> values = new List<MeterReadingDTO>();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                reader.ReadLine();//skipping the first line
                while (reader.Peek() >= 0)
                {
                    var meterReading = FromCSV(reader.ReadLine());
                    if (meterReading != null)
                        values.Add(meterReading);
                }
            }
            var validValues = ValidationReading(values);

            if(validValues.Count > 0)
            {
                context.MeterReadings.AddRange(validValues.Select( x=> new MeterReading {AccountId= x.AccountId, MeterReadDt = x.ReadingDt, MeterReadValue = x.ReadingValue}));
                try
                {
                    context.SaveChanges();
                    returnMessage = "Total Reading: " + values.Count + "; Total Succeed: " + validValues.Count + "; Total Failed: " + (values.Count - validValues.Count).ToString();
                    
                }
                catch (Exception)
                {
                    // log exceptions
                    
                }              
                
            }
            return returnMessage;
        }

        private List<MeterReadingDTO> ValidationReading(List<MeterReadingDTO> values)
        {
            List<MeterReadingDTO> val = new List<MeterReadingDTO>();
            Regex regex = new Regex(@"^[0-9]{5}$");            
            bool flag = true;
            if (values.Count > 0)
            {
                foreach (var item in values)
                {
                    //Valid account entry only
                    if (!context.TestAccounts.Any(x => x.AccountId == item.AccountId))
                        flag = false;
                    //reading value match 5 digit constraint
                    if (!regex.IsMatch(item.ReadingValue))
                        flag = false;
                    //No duplicate meter reading entry
                    if (val.Any(x => (x.AccountId == item.AccountId && x.ReadingValue == item.ReadingValue)))
                    {
                        flag = false;
                    }
                    //new entry should be of latest.
                    if (val.Any(x => (x.AccountId == item.AccountId && x.ReadingDt > item.ReadingDt)))
                    {
                        flag = false;
                    }
                    if (flag)
                    {
                        val.Add(item);
                    }
                    flag = true;
                }
            }
            return val;
        }
        public MeterReadingDTO FromCSV(string csvLine)
        {
            string[] values = csvLine.Split(',');
            MeterReadingDTO meterReadingDTO = null;
            if (values.Length > 2)
            {
                meterReadingDTO = new MeterReadingDTO();
                meterReadingDTO.AccountId = Convert.ToString(values[0]);
                meterReadingDTO.ReadingValue = Convert.ToString(values[2]);
                meterReadingDTO.ReadingDt = DateTime.ParseExact(values[1], "dd/MM/yyyy HH:mm", null);
            }
            return meterReadingDTO;
        }
        //private async Task SaveFile(IFormFile file)
        //{
        //    string uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
        //    if (file.Length > 0)
        //    {
        //        string filePath = Path.Combine(uploads, file.FileName);
        //        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await file.CopyToAsync(fileStream);
        //        }
        //    }
        //}
    }
}
