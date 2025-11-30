using Dapper;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection;
using System.Text;

namespace DatabaseAccessSem1
{
    public class DbHandler
    {
        private readonly IDbConnectionFactory _dbFactory; //Hvilken database factory den skal bruge (sqlite eller microsoft sql)

        public DbHandler(IDbConnectionFactory dbFactory) //INIT af klassen
        {
            _dbFactory = dbFactory;
        }

        public IEnumerable<Member> GetAllMembers()
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt
            
            string sql = "SELECT * FROM Customers"; 
            return connection.Query<Member>(sql); // Selve forespørgsel til database
        }

        public IEnumerable<int> FindMemberIds(
            string? FirstName = null, 
            string? LastName = null, 
            DateTime? DateOfBirth = null, 
            string? Email = null, 
            int? PhoneNumber = null,
            int? MemberType = null,
            bool? Active = null)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt
                                                                  // 1. Start with a basic query that selects ALL columns so the Member object can be filled
            var sqlBuilder = new StringBuilder("SELECT MemberID FROM Customers WHERE 1=1");

            // 2. Create a container for your safe parameters
            var parameters = new DynamicParameters();

            // 3. Add filters only if they are provided
            if (!string.IsNullOrEmpty(FirstName))
            {
                sqlBuilder.Append(" AND FirstName LIKE @FirstName");
                parameters.Add("FirstName", $"{FirstName}%");
            }

            if (!string.IsNullOrEmpty(LastName))
            {
                sqlBuilder.Append(" AND LastName LIKE @LastName");
                parameters.Add("LastName", $"%{LastName}%");
            }

            if (DateOfBirth.HasValue)
            {
                sqlBuilder.Append(" AND DateOfBirth = @DateOfBirth");
                parameters.Add("DateOfBirth", DateOfBirth);
            }

            if (!string.IsNullOrEmpty(Email))
            {
                sqlBuilder.Append(" AND Email LIKE @Email");
                parameters.Add("Email", $"%{Email}%");
            }

            if (PhoneNumber.HasValue)
            {
                sqlBuilder.Append(" AND PhoneNumber = @PhoneNumber");
                parameters.Add("PhoneNumber", PhoneNumber);
            }

            if (MemberType.HasValue)
            {
                sqlBuilder.Append(" AND MemberType = @MemberType");
                parameters.Add("MemberType", MemberType);
            }

            if (Active.HasValue)
            {
                sqlBuilder.Append(" AND Active = @Active");
                parameters.Add("Active", Active); 
            }


            return connection.Query<int>(sqlBuilder.ToString(), parameters); // Selve forespørgsel til database
        }

        public IEnumerable<int> FindSessionIds(
            string? SessionType = null,
            DateTime? DateTimeStart = null,
            DateTime? DateTimeEnd = null,
            int? MaxMembers = null,
            int? MinMembers = null)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt
                                                                  // 1. Start with a basic query that selects ALL columns so the Member object can be filled
            var sqlBuilder = new StringBuilder("SELECT SessionID FROM Sessions WHERE 1=1"); //Søger efter alle linjer hvor 1=1 (som er alle) og tilføjer senere mere præcise instruktioner

            // 2. Create a container for your safe parameters
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(SessionType))
            {
                sqlBuilder.Append(" AND SessionType = @SessionType");
                parameters.Add("SessionType", SessionType);
            }
            if (DateTimeStart.HasValue)
            {
                sqlBuilder.Append(" AND DateTime >= @DateTimeStart");
                parameters.Add("DateTimeStart", DateTimeStart);
            }
            if (DateTimeEnd.HasValue)
            {
                sqlBuilder.Append(" AND DateTime < @DateTimeEnd");
                parameters.Add("DateTimeEnd", DateTimeEnd);
            }
            if (MaxMembers.HasValue)
            {
                sqlBuilder.Append(" AND MaxMembers <= @MaxMembers");
                parameters.Add("MaxMembers", MaxMembers);
            }
            if (MinMembers.HasValue)
            {
                sqlBuilder.Append(" AND MaxMembers >= @MinMembers");
                parameters.Add("MinMembers", MinMembers);
            }

            return connection.Query<int>(sqlBuilder.ToString(), parameters); // Selve forespørgsel til database

        }

        public Member GetMemberById(int MemberID)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = "SELECT * FROM Customers WHERE MemberID = @MemberID";

            return connection.QuerySingle<Member>(sql, new { MemberID = MemberID });
        }

        public Instructor GetInstructorById(int InstructorID)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = "SELECT * FROM Instructors WHERE InstructorID = @InstructorID";

            return connection.QuerySingle<Instructor>(sql, new { InstructorID = InstructorID });
        }

        public Session GetSessionById(int SessionID)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = "SELECT * FROM Sessions WHERE SessionID = @SessionID";

            return connection.QuerySingle<Session>(sql, new { SessionID = SessionID });
        }

        public IEnumerable<int> GetMemberIDsInSession(int SessionID)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = "SELECT MemberID FROM MemberGroups WHERE SessionID = @SessionID";

            return connection.Query<int>(sql, new { SessionID = SessionID });
        }

        public IEnumerable<int> GetInstructorIDsInSession(int SessionID)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = "SELECT InstructorID FROM InstructorGroups WHERE SessionID = @SessionID";

            return connection.Query<int>(sql, new { SessionID = SessionID });
        }



    }
}
