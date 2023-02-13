namespace IoCServiceProvider
{
    public class Singleton
    {
        static Singleton()
        {
            Singletons = new Dictionary<Type, object>();
        }

        public static IDictionary<Type, object> Singletons { get; }
    }

    public class Singleton<T> : Singleton
    {
        private static T? _instance;

        public static T? Instance
        {
            get => _instance;
            set { _instance = value; Singletons[typeof(T)] = value; }
        }
    }
}