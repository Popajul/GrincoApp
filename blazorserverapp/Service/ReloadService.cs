namespace blazorserverapp.Service
{
    public class ReloadService
    {
         public event Action? UserConnectionStateReloadRequested;

        public void UserConnectionStateRequestReload()
        {
            UserConnectionStateReloadRequested?.Invoke();
        }
    }
}