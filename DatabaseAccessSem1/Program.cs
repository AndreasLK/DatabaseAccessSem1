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
            //::::: DONT MESS WITH THIS ::::::::: 
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


            //TODO // WARNING MIGRATE ALL CREATE STATEMENTS TO MSSQL. (RETURNING * IS SQLITE SPECIFIC)
            //::::: DONT MESS WITH THIS :::::::::

            // test af rapport - sandra - ved hjælp af Gemini
            Console.WriteLine("--- Starter Rapport Test ---");

            var reporter = new ReportingStatistics(memberRepo, memberGroupRepo);

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"FitnessRapport_TEST_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            string fullFilePath = Path.Combine(desktopPath, fileName);

            // Generer rapporten - sandra - ved hjælp af gemini
            reporter.GenerateReport(fullFilePath);

            Console.WriteLine("--- Test Afsluttet. Tjek dit skrivebord. ---");
        }
    }
}
