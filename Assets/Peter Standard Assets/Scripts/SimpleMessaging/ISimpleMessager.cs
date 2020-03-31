namespace EnliStandardAssets.SimpleMessaging
{
    public interface ISimpleMessager
    {
        void ShowMessage(string message, ToastDuration duration);
        void ShowMessage(string message, float duration);
    }

    public enum ToastDuration
    {
        Short,
        Long
    }
}