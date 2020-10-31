using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Data
{
    public class Person
    {  
        public int Id { get; set; }
        public String Ad { get; set; }
        public String Soyad { get; set; }
        public String Mail { get; set; }
        public String Sifre { get; set; }
        public String Token { get; set; }
        public String Role { get; set; }
    }
}
