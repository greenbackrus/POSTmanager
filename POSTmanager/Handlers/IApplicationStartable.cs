namespace POSTmanager.handlers
{
    internal interface IApplicationStartable
    {
        void Start();
        void BeforeStart();
        void AfterStart();
    }
}
