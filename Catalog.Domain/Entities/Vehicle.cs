using Catalog.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Entities
{
    public class Vehicle : BaseEntity, ISoftDeletable
    {
        public string Brand { get; set; }      
        public string Model { get; set; }      
        public int Year { get; set; }          
        public decimal DailyPrice { get; set; } 
        public string Currency { get; set; }   
        public bool IsAvailable { get; set; }
        public bool IsDeleted { get; set; } = false;

        public Vehicle()
        {
            IsAvailable = true;
        }
    }
}
