namespace ObjectTracingInVideoImage.App.Extensions
{
    public static class ControlExtensions
    {
        public static Task InvokeAsync(this Control control, Action action)
        {
            if (control.InvokeRequired)
                return Task.Factory.StartNew(() => control.Invoke(action), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
            else
            {
                action();
                return Task.CompletedTask;
            }
        }
    }
}
