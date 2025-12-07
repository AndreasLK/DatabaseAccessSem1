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



        public Member Create(Member member) 
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = @"INSERT INTO Customers 
                        (FirstName, LastName, DateOfBirth, Email, PhoneNumber, MemberType, Active) Values 
                        (@FirstName, @LastName, @DateOfBirth, @Email, @PhoneNumber, @MemberType, @Active) RETURNING *;";

            return connection.QuerySingle<Member>(sql, member);
        }

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

        public int Update(Member member)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = @" UPDATE Customers
                        SET
                            FirstName = @FirstName,
                            LastName = @LastName,
                            DateOfBirth = @DateOfBirth,
                            Email = @Email,
                            PhoneNumber = @PhoneNumber,
                            MemberType = @MemberType,
                            Active = @Active
                        WHERE MemberID = @MemberID";
            return connection.Execute(sql, member); //Returnere mængden af rækker opdateret (forhåbeligt 1)
        }

        public int Delete(int memberID)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = @"DELETE FROM Customers
                        WHERE MemberID = @MemberID";

            return connection.Execute(sql, new { MemberID = memberID });
        }

        //ændret af sandra - Gemini
        public int GetActiveMemberCount()
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt
            string sql = @"SELECT COUNT(*) FROM Customers WHERE Active = @IsActive";
            return connection.QuerySingle<int>(sql, new { IsActive = true});
        }

    }
}
