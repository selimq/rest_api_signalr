using System;

namespace Login.Data{
    public class Connection
    {
        public int ConnectionID { get; set; }
        public string UserName { get; set; }
        public char Connected { get; set; }
        public DateTime LastSeen {get;set;}
    }
}