using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseAccessSem1.Repository
{
    public class InstructorGroupRepository
    {
        private readonly IDbConnectionFactory _dbFactory;
        public InstructorGroupRepository(IDbConnectionFactory dbFactory) { _dbFactory = dbFactory; }



        public InstructorGroup Create(InstructorGroup instructorGroup)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = @"INSERT INTO InstructorGroups 
                        (InstructorID, SessionID) Values 
                        (@InstructorID, @SessionID) RETURNING *;";

            return connection.QuerySingle<InstructorGroup>(sql, instructorGroup);
        }

        public IEnumerable<Session> GetSessions(int instructorID)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = @"SELECT Sessions.* FROM InstructorGroups
                        RIGHT JOIN Sessions
                        ON InstructorGroups.SessionID = Sessions.SessionID
                        WHERE InstructorGroups.InstructorID = @InstructorID";

            return connection.Query<Session>(sql, new { InstructorID = instructorID });

        }

        public IEnumerable<Instructor> GetInstructors(int sessionID)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = @"SELECT Instructors.* FROM InstructorGroups
                        RIGHT JOIN Instructors
                        ON InstructorGroups.InstructorID = Instructors.InstructorID
                        WHERE InstructorGroups.SessionID = @SessionID";

            return connection.Query<Instructor>(sql, new { SessionID = sessionID });

        }
        public int Update(InstructorGroup instructorGroup)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = @" UPDATE ´InstructorGroups
                        SET
                            InstructorID = @InstructorID,
                            SessionID = @SessionID
                        WHERE GroupingID = @GroupingID";

            return connection.Execute(sql, instructorGroup); //Returnere mængden af rækker opdateret (forhåbeligt 1)
        }

        public int Delete(int groupingID)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = @"DELETE FROM InstructorGroups
                        WHERE GroupingID = @GroupingID";

            return connection.Execute(sql, new { GroupingID = groupingID }); //Returnere mængden af rækker opdateret (forhåbeligt 1)
        }
    }

}
