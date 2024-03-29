
namespace scanapp {
    internal class Constants {
        public readonly static int UNASSIGNED = -1;

        internal enum DeveolpmentEnvironment
        {
            PRODUCTION_NAS,
            RASPBERRY_PI_5,
            LOCALHOST,
        }

        internal enum AddNewArticleAction
        {
            DUPLICATE_ARTICLE,
            INSERT_NEW_ARTICLE
        };

        internal const string DatabaseName = "Warehouse";
    }
}