namespace core.hybrid.repository.Utilities
{
    using core.hybrid.repository.Properties;
    using System;
    using System.Text;

    public static class Queries
    {
        public static string CreateDatabaseQuery()
            => Resources.CreateDatabase;

        public static string AddInitialSeed()
            => Resources.InitialSeed;

        public static string AddTable()
            => Resources.CreateTable;

        public static string SelectTests(bool isCount = false, bool isPaging = true)
            => GetQueryTest(isCount, isPaging);

        private static string GetQueryTest(bool isCount = false, bool isPaging = true)
        {
            StringBuilder query;

            if (!isCount)
            {
                query = new StringBuilder();
                query.AppendLine(Resources.SelectTests);

                if (isPaging)
                    query.AppendLine(Properties.Resources.Offset);
            }
            else
            {
                query = new StringBuilder("SELECT COUNT(*) FROM ("); ;
                query.AppendLine(Resources.SelectTests);
                query.AppendLine(") AS Registers");
            }

            return query.ToString();
        }
    }
}
