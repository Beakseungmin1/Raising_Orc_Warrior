using System;

public class MessageEvents
{
    public event Action<MessageTextType, float, int> onShowMessage;

    public void ShowMessage(MessageTextType messageType, float duration, int sortOrder)
    {
        if (onShowMessage != null)
        {
            onShowMessage(messageType, duration, sortOrder);
        }
    }
}