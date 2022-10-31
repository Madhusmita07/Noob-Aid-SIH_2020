using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Root
{
    public string type { get; set; }
    public From from = new From();
    public string text { get; set; }
    public List<Activity> activities  { get; set; }
    public string watermark;
    public string conversationId;
    public string token;
    public string streamUrl;
    public string id;

}
public class From
{
    public string id { get; set; }
    public string name { get; set; }

}


public class Activity
{
    public string type { get; set; }
    public string id { get; set; }
    public DateTime timestamp { get; set; }
    public string channelId { get; set; }
    public From from { get; set; }
    public string text { get; set; }
    public string replyToId { get; set; }

}


