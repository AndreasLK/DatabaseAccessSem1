using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseAccessSem1.Repository
{
    public class InstructorRepository
    {
        private readonly IDbConnectionFactory _dbFactory;
        public InstructorRepository(IDbConnectionFactory dbFactory) { _dbFactory = dbFactory; }

        public Instructor Create(Instructor instructor)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = @"INSERT INTO Instructors 
                        (FirstName, LastName, CertifiedForTrailRunning, CertifiedForSkovYoga, CertifiedForHIIT, CertifiedForPowerwalk, CertifiedForTrappeløb, CertifiedForMountainbike) Values 
                        (@FirstName, @LastName, @CertifiedForTrailRunning, @CertifiedForSkovYoga, @CertifiedForHIIT, @CertifiedForPowerwalk, @CertifiedForTrappeløb, @CertifiedForMountainbike) RETURNING *;";

            return connection.QuerySingle<Instructor>(sql, instructor);
        }

        public IEnumerable<int> GetID(
    string? firstName = null,
    string? lastName = null,
    bool? CertifiedForTrailRunning = null,
    bool? CertifiedForSkovYoga = null,
    bool? CertifiedForHIIT = null,
    bool? CertifiedForTrappeløb = null,
    bool? CertifiedForPowerwalk = null,
     bool? CertifiedForMountainbike = null)
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

            if (CertifiedForTrailRunning.HasValue)
            {
                sqlBuilder.Append(" AND CertifiedForTrailRunning = @CertifiedForTrailRunning");
                parameters.Add("CertifiedForTrailRunning", CertifiedForTrailRunning);
            }

            if (CertifiedForSkovYoga.HasValue)
            {
                sqlBuilder.Append(" AND CertifiedForSkovYoga = @CertifiedForSkovYoga");
                parameters.Add("CertifiedForSkovYoga", CertifiedForSkovYoga);
            }

            if (CertifiedForHIIT.HasValue)
            {
                sqlBuilder.Append(" AND CertifiedForHIIT = @CertifiedForHIIT");
                parameters.Add("CertifiedForHIIT", CertifiedForHIIT);
            }
            if (CertifiedForPowerwalk.HasValue)
            {
                sqlBuilder.Append(" AND CertifiedForPowerwalk = @CertifiedForPowerwalk");
                parameters.Add("CertifiedForPowerwalk", CertifiedForPowerwalk);
            }
            if (CertifiedForTrappeløb.HasValue)
            {
                sqlBuilder.Append(" AND CertifiedForTrappeløb = @CertifiedForTrappeløb");
                parameters.Add("CertifiedForTrappeløb", CertifiedForTrappeløb);
            }
            if (CertifiedForMountainbike.HasValue)
            {
                sqlBuilder.Append(" AND CertifiedForMountainbike = @CertifiedForMountainbike");
                parameters.Add("CertifiedForMountainbike", CertifiedForMountainbike);
            }

            return connection.Query<int>(sqlBuilder.ToString(), parameters); // Selve forespørgsel til database
        }

        public IEnumerable<Instructor> GetAll()
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = "SELECT * FROM Instructors";
            return connection.Query<Instructor>(sql); // Selve forespørgsel til database
        }
        
        public Member GetByID(int InstructorID)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = "SELECT * FROM Instructors WHERE InstructorID = @InstructorID";

            return connection.QuerySingle<Member>(sql, new { InstructorID = InstructorID });
        }

        public int Update(Instructor instructor)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = @" UPDATE Instructors
                        SET
                            FirstName = @FirstName,
                            LastName = @LastName,
                            CertifiedForTrailRunning = @CertifiedForTrailRunning,
                            CertifiedForSkovYoga = @CertifiedForSkovYoga,
                            CertifiedForHIIT = @CertifiedForHIIT, 
                            CertifiedForPowerwalk = @CertifiedForPowerwalk,
                            CertifiedForTrappeløb = @CertifiedForTrappeløb,
                            CertifiedForMountainbike = @CertifiedForMountainbike
                        WHERE InstructorID = @InstructorID";
            return connection.Execute(sql, instructor); //Returnere mængden af rækker opdateret (forhåbeligt 1)
        } // tilføjelse af nye certifikater - sandra

        public int Delete(int instructorID)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = @"DELETE FROM Instructors
                        WHERE InstructorID = @InstructorID";

            return connection.Execute(sql, new { InstructorID = instructorID });
        }

    }
}
