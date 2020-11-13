using System;

public class ChatMessage
{
    public int id { get; set; }
    public string Sender { get; set; }
    public string ToUser { get; set; }
    public string Message { get; set; }
    public DateTime Time { get; set; }
    public bool IsRead { get; set; }
    public bool IsSend { get; set; }

}