using System;

public class MessageEvents
{
    public event Action<MessageTextType> onShowMessage;

    public void ShowMessage(MessageTextType messageType)
    {
        if (onShowMessage != null)
        {
            onShowMessage(messageType);
        }
    }
}