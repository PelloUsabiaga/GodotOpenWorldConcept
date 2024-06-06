using Godot;
using System;
using System.Collections.Generic;

public struct SpeakResponse
{
    public string Response;
    public List<string> SuggestedMessages;
    public bool AllowDifferentMessages;
    public bool EndConversation;
    public string EndConversationMessage;


    public SpeakResponse(string response, List<string> suggestedMessages=null,
                        bool allowDifferentMessages=false, bool endConversation=false, 
                        string endConversationMessage="Goodbye")
    {
        if (suggestedMessages == null)
        {
            suggestedMessages = new List<string>();
        }
        Response = response;
        SuggestedMessages = suggestedMessages;
        AllowDifferentMessages = allowDifferentMessages;
        EndConversation = endConversation;
        EndConversationMessage = endConversationMessage;
    }
    public override string ToString()
    {
        return string.Format("Response: {0}, SuggestedMessages: {1}", Response, 
                                                                    string.Join( ", ", SuggestedMessages));
    }
}