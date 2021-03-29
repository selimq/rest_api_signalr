using System;

public class WebRTCMessage
{
    public String To { get; set; }
    public String From { get; set; }
    public String Type { get; set; }//can be offer, answer,candidate,hung-up
    public Object Sdp { get; set; }
    public Object Candidate { get; set; }

}