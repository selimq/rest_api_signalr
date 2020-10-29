using System;
using System.Collections.Generic;
using System.Text;

namespace Login.Data
{
    public class Giris
    {  
        public int Id { get; set; }
        public String Ad { get; set; }
        public String Soyad { get; set; }
        public String Mail { get; set; }
        public String Sifre { get; set; }
        public String Unvan { get; set; }

        public static implicit operator List<object>(Giris v)
        {
            throw new NotImplementedException();
        }
    }
}
