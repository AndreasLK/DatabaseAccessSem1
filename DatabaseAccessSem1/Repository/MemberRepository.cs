using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseAccessSem1.Repository
{
    public class MemberRepository
    {
        private readonly IDbConnectionFactory _dbFactory;
        public MemberRepository(IDbConnectionFactory dbFactory) {_dbFactory = dbFactory;}

        public IEnumerable<int> GetID(
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

        public IEnumerable<Member> GetAll()
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = "SELECT * FROM Customers";
            return connection.Query<Member>(sql); // Selve forespørgsel til database
        }

        public Member GetByID(int memberID)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = "SELECT * FROM Customers WHERE MemberID = @MemberID";

            return connection.QuerySingle<Member>(sql, new { MemberID = memberID });
        }

        public Member Create(Member member) {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            var parameters = new DynamicParameters();
            string sql = "INSERT INTO Customers " +
                        "(FirstName, LastName, DateOfBirth, Email, PhoneNumber, MemberType, Active) " +
                "Values (@FirstName, @LastName, @DateOfBirth, @Email, @PhoneNumber, @MemberType, @Active)" +
                "RETURNING *;";

            parameters.Add("FirstName", member.FirstName);
            parameters.Add("LastName", member.LastName);
            parameters.Add("DateOfBirth", member.DateOfBirth);
            parameters.Add("Email", member.Email);
            parameters.Add("PhoneNumber", member.PhoneNumber);
            parameters.Add("MemberType", member.MemberType);
            parameters.Add("Active", member.Active);

            return connection.QuerySingle<Member>(sql, parameters);
        }



    }
}
