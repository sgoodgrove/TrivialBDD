namespace TrivialBDD
{
    public static class BDD
    {
        public static T Given<T>(this T t) => t;
        public static T When<T>(this T t) => t;
        public static T Then<T>(this T t) => t;
        public static T And<T>(this T t) => t;
    }
}
