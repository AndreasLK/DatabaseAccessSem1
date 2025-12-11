using DatabaseAccessSem1.Repository;
using System.Runtime.InteropServices;
using DatabaseAccessSem1.Reporting; // ændret af sandra. importering af klasser og typer fra namespacet/filmappen Reporting.
using System.IO; // Til at bygge database path - et namespace (samling af klasser), som lader dit program arbejde med filer og mapper. system,IO = kommunikation mellem dit program og din computers harddisk.
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

            var reporter = new ReportingStatistics(memberRepo, memberGroupRepo); // oprettelse af objektet (ReportingStatestics) der generer rapporten.

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); // returnerer streng, som repræsenterer stien til skrivebordet.
            string fileName = $"FitnessRapport_TEST_{DateTime.Now:yyyyMMdd_HHmmss}.txt"; 
            string fullFilePath = Path.Combine(desktopPath, fileName); // kombinering af filename og desktoppath som skaber den endelige sti.
            // "Path" er en indbygget klasse i c# (system.IO) der indeholder metoder til at håndtere fil- og mappestier.
            // "combine" er en metode til at sammensætte mappe-sti med filnavn, og som sikrer, at man får den korrekte sti. 


            // Generer rapporten - sandra - ved hjælp af gemini
            reporter.GenerateReport(fullFilePath); // kalder metoden, der starter rapportgenerering, og sender stien (fullfilepath) til den tekst fil der skal gemmes på skrivebordet.

            Console.WriteLine("--- Test Afsluttet. Tjek dit skrivebord. ---");
        }
    }
}
