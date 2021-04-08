using System;
using System.Collections.Generic;
using System.Text;

namespace Login.Data
{
    public class POJO
    {   
        public int Id { get; set; }
        public int NumberOfRows { get; set; }
        public bool Flag { get; set; }
        public String Message { get; set; }
        public String Token{get;set;}
    }
}
