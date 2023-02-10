namespace IoC.ServiceProvider
{
    public class Singleton<T>
    {
        private static T _instance;

        public static T Instance
        {
            get => _instance;
            set => _instance = value;
        }
    }
}