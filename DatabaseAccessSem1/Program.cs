using DatabaseAccessSem1.Repository;
using System.Runtime.InteropServices;

namespace DatabaseAccessSem1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string _runningPath = AppDomain.CurrentDomain.BaseDirectory;
            string _projectPath = Path.GetFullPath(Path.Combine(_runningPath, @"..\..\..\"));
            string _dbPath = Path.Combine(_projectPath, "Data", "EksamenSem1.db"); //Fulde path doneret af Gemini
            string sqliteConnString = $"Data Source={_dbPath}"; //Alt dette er for at sikre der ændres i den rigtige database. Slipper vi for med MSSQL serveren

            IDbConnectionFactory dbFactory = new SqliteConnectionFactory(sqliteConnString);

            var memberRepo = new MemberRepository(dbFactory);

            var _temp = new Member
            {
                MemberID = 5001,
                FirstName = "Test 3",
                LastName = "Også Test",
                DateOfBirth = new DateTime(1995, 5, 12),
                Email = "test@jegErEnTe.st",
                PhoneNumber = 88888888,
                MemberType = 1,
                Active = true
            };
            var test = memberRepo.Remove(5001);

            Console.WriteLine(test.ToString());
        }
    }
}
