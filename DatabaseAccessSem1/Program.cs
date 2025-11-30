using System.Runtime.InteropServices;

namespace DatabaseAccessSem1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "EksamenSem1.db");
            string sqliteConnString = $"Data Source={dbPath}";

            IDbConnectionFactory dbFactory = new SqliteConnectionFactory(sqliteConnString);

            DbHandler dbHandler = new DbHandler(dbFactory);

            IEnumerable<Member> allMembers = dbHandler.GetAllMembers();
            foreach (Member member in allMembers)
            {
                Console.WriteLine(member.FirstName);
            }

            var search = dbHandler.FindMemberIds(Active: true);

            var individual = dbHandler.GetMemberById(search.First());
            Console.WriteLine($"{individual.FirstName},{individual.LastName}");
        }
    }
}
