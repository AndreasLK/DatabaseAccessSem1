using DatabaseAccessSem1.Repository;
using System.Runtime.InteropServices;
using DatabaseAccessSem1.Reporting; // ændret af sandra
using System.IO; // Til at bygge database path,
using System;

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
            var sessionRepo = new SessionRepository(dbFactory);
            var instructorRepo = new InstructorRepository(dbFactory);
            var memberGroupRepo = new MemberGroupRepository(dbFactory);
            var instructorGroupRepo = new InstructorGroupRepository(dbFactory);

        }
    }
}
