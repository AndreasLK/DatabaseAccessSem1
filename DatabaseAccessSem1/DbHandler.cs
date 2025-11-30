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
            string? firstName = null, 
            string? lastName = null, 
            DateTime? dateOfBirth = null, 
            string? email = null, 
            int? phoneNumber = null,
            int? memberType = null,
            bool? active = null)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt
                                                                  // 1. Start with a basic query that selects ALL columns so the Member object can be filled
            var sqlBuilder = new StringBuilder("SELECT MemberID FROM Customers WHERE 1=1");

            // 2. Create a container for your safe parameters
            var parameters = new DynamicParameters();

            // 3. Add filters only if they are provided
            if (!string.IsNullOrEmpty(firstName))
            {
                sqlBuilder.Append(" AND FirstName LIKE @FirstName");
                parameters.Add("FirstName", $"{firstName}%");
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                sqlBuilder.Append(" AND LastName LIKE @LastName");
                parameters.Add("LastName", $"%{lastName}%");
            }

            if (dateOfBirth.HasValue)
            {
                sqlBuilder.Append(" AND DateOfBirth = @DateOfBirth");
                parameters.Add("DateOfBirth", dateOfBirth);
            }

            if (!string.IsNullOrEmpty(email))
            {
                sqlBuilder.Append(" AND Email LIKE @Email");
                parameters.Add("Email", $"%{email}%");
            }

            if (phoneNumber.HasValue)
            {
                sqlBuilder.Append(" AND PhoneNumber = @PhoneNumber");
                parameters.Add("PhoneNumber", phoneNumber);
            }

            if (memberType.HasValue)
            {
                sqlBuilder.Append(" AND MemberType = @MemberType");
                parameters.Add("MemberType", memberType);
            }

            if (active.HasValue)
            {
                sqlBuilder.Append(" AND Active = @Active");
                parameters.Add("Active", active); 
            }


            return connection.Query<int>(sqlBuilder.ToString(), parameters); // Selve forespørgsel til database
        }

        public IEnumerable<int> FindSessionIds(
            string? sessionType = null,
            DateTime? dateTimeStart = null,
            DateTime? dateTimeEnd = null,
            int? maxMembers = null,
            int? minMembers = null)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt
                                                                  // 1. Start with a basic query that selects ALL columns so the Member object can be filled
            var sqlBuilder = new StringBuilder("SELECT SessionID FROM Sessions WHERE 1=1"); //Søger efter alle linjer hvor 1=1 (som er alle) og tilføjer senere mere præcise instruktioner

            // 2. Create a container for your safe parameters
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(sessionType))
            {
                sqlBuilder.Append(" AND SessionType = @SessionType");
                parameters.Add("SessionType", sessionType);
            }
            if (dateTimeStart.HasValue)
            {
                sqlBuilder.Append(" AND DateTime >= @DateTimeStart");
                parameters.Add("DateTimeStart", dateTimeStart);
            }
            if (dateTimeEnd.HasValue)
            {
                sqlBuilder.Append(" AND DateTime < @DateTimeEnd");
                parameters.Add("DateTimeEnd", dateTimeEnd);
            }
            if (maxMembers.HasValue)
            {
                sqlBuilder.Append(" AND MaxMembers <= @MaxMembers");
                parameters.Add("MaxMembers", maxMembers);
            }
            if (minMembers.HasValue)
            {
                sqlBuilder.Append(" AND MaxMembers >= @MinMembers");
                parameters.Add("MinMembers", minMembers);
            }

            return connection.Query<int>(sqlBuilder.ToString(), parameters); // Selve forespørgsel til database

        }

        public IEnumerable<int> FindInstructorIds(
            string? firstName = null,
            string? lastName = null,
            bool? certifiedForTrailRunning = null,
            bool? certifiedForSkovYoga = null)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt
                                                                  // 1. Start with a basic query that selects ALL columns so the Member object can be filled
            var sqlBuilder = new StringBuilder("SELECT InstructorID FROM Instructors WHERE 1=1");

            // 2. Create a container for your safe parameters
            var parameters = new DynamicParameters();

            // 3. Add filters only if they are provided
            if (!string.IsNullOrEmpty(firstName))
            {
                sqlBuilder.Append(" AND FirstName LIKE @FirstName");
                parameters.Add("FirstName", $"{firstName}%");
            }

            if (!string.IsNullOrEmpty(lastName))
            {
                sqlBuilder.Append(" AND LastName LIKE @LastName");
                parameters.Add("LastName", $"%{lastName}%");
            }

            if (certifiedForTrailRunning.HasValue)
            {
                sqlBuilder.Append(" AND CertifiedForTrailRunning = @CertifiedForTrailRunning");
                parameters.Add("CertifiedForTrailRunning", certifiedForTrailRunning);
            }

            if (certifiedForSkovYoga.HasValue)
            {
                sqlBuilder.Append(" AND CertifiedForTrailRunning = @CertifiedForSkovYoga");
                parameters.Add("CertifiedForSkovYoga", certifiedForSkovYoga);
            }

            return connection.Query<int>(sqlBuilder.ToString(), parameters); // Selve forespørgsel til database

        }

        public Member GetMemberById(int memberID)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = "SELECT * FROM Customers WHERE MemberID = @MemberID";

            return connection.QuerySingle<Member>(sql, new { MemberID = memberID });
        }

        public Instructor GetInstructorById(int instructorID)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = "SELECT * FROM Instructors WHERE InstructorID = @InstructorID";

            return connection.QuerySingle<Instructor>(sql, new { InstructorID = instructorID });
        }

        public Session GetSessionById(int sessionID)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = "SELECT * FROM Sessions WHERE SessionID = @SessionID";

            return connection.QuerySingle<Session>(sql, new { SessionID = sessionID });
        }

        public IEnumerable<int> GetMemberIDsInSession(int sessionID)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = "SELECT MemberID FROM MemberGroups WHERE SessionID = @SessionID";

            return connection.Query<int>(sql, new { SessionID = sessionID });
        }

        public IEnumerable<int> GetInstructorIDsInSession(int sessionID)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = "SELECT InstructorID FROM InstructorGroups WHERE SessionID = @SessionID";

            return connection.Query<int>(sql, new { SessionID = sessionID });
        }



    }
}
