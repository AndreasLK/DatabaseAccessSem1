using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseAccessSem1.Repository
{
    public class MemberGroupRepository
    {
        private readonly IDbConnectionFactory _dbFactory;
        public MemberGroupRepository(IDbConnectionFactory dbFactory) { _dbFactory = dbFactory; }



        public MemberGroup Create(MemberGroup memberGroup)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = @"INSERT INTO MemberGroups 
                        (MemberID, SessionID) Values 
                        (@MemberID, @SessionID) RETURNING *;";

            return connection.QuerySingle<MemberGroup>(sql, memberGroup);
        }

        public IEnumerable<Session>GetSessions(int memberID)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = @"SELECT Sessions.* FROM MemberGroups
                        RIGHT JOIN Sessions
                        ON MemberGroups.SessionID = Sessions.SessionID
                        WHERE MemberGroups.MemberID = @MemberID";

            return connection.Query<Session>(sql, new { MemberID = memberID });

        }

        public IEnumerable<Member> GetMembers(int sessionID)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = @"SELECT Members.* FROM MemberGroups
                        RIGHT JOIN Customers
                        ON MemberGroups.MemberID = Customers.MemberID
                        WHERE MemberGroups.SessionID = @SessionID";

            return connection.Query<Member>(sql, new { SessionID = sessionID });

        }
        public int Update(MemberGroup memberGroup)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = @" UPDATE ´MemberGroups
                        SET
                            MemberID = @MemberID,
                            SessionID = @SessionID
                        WHERE GroupingID = @GroupingID";

            return connection.Execute(sql, memberGroup); //Returnere mængden af rækker opdateret (forhåbeligt 1)
        }

        public int Delete(int groupingID)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = @"DELETE FROM MemberGroups
                        WHERE GroupingID = @GroupingID";

            return connection.Execute(sql, new { GroupingID = groupingID }); //Returnere mængden af rækker opdateret (forhåbeligt 1)
        }
    }
}
