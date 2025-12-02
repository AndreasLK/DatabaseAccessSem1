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
            var sessionRepo = new SessionRepository(dbFactory);
            var instructorRepo = new InstructorRepository(dbFactory);
            var memberGroupRepo = new MemberGroupRepository(dbFactory);
            var instructorGroupRepo = new InstructorGroupRepository(dbFactory);


            //Test af hver funktion
            var testCreateMember = memberRepo.Create(new Member
            {
                FirstName = "John",
                LastName = "Johnsen",
                MemberType = 1,
                Active = true
            });
            Console.WriteLine("testCreateMember  " + testCreateMember.ToString());


            var testUpdateMember = memberRepo.Update(
                testCreateMember with
                {
                    DateOfBirth = new DateTime(1994, 12, 2)
                });

            Console.WriteLine("testUpdateMember  " + testUpdateMember.ToString());

            var testGetMember = memberRepo.GetID(firstName: testCreateMember.FirstName, lastName: testCreateMember.LastName);

            Console.WriteLine("testGetIDMember " + "Første: " + testGetMember.First().ToString() + "ResultatMængde: " + testGetMember.Count());

            var test = memberRepo.GetByID(testCreateMember.MemberID ?? -1);




        }
    }
}
