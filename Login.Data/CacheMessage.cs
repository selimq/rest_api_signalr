using System;
[Serializable]
public class CacheMessage
{
    public int id { get; set; }
    public string Sender { get; set; }
    public string ToUser { get; set; }
    public string Message { get; set; }
    public DateTime Time { get; set; }
    public int TypeId {get;set;}
    /*
    public bool IsRead { get; set; }
    public bool IsSend { get; set; }
    
    public ChatMessage(){}
    public ChatMessage(string Sender,string ToUser,string Message,bool IsSend){
            this.Sender  = Sender;
            this.ToUser = ToUser;
            this.Message = Message;
            this.IsSend = IsSend;

    }*/


}

