using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using DatabaseAccessSem1; // ændret af sandra

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

        // ændret af sandra - Gemini - metode til at få information fra databasen om hvilke hold er mest populære.  
        public IEnumerable<SessionPopularityData> GetSessionPopularity() // IEnumerable = returtype.
        {
            using var connection = _dbFactory.CreateConnection();
        string sql = @"SELECT S.SessionType, 
                        COUNT(MG.MemberID) AS ParticipantCount,
                        ROUND(
                            (COUNT(MG.MemberID) * 100.0) / 
                            (SELECT SUM(MaxMembers)
                            FROM Sessions s2 WHERE s2.SessionType = S.SessionType), 
                        1) As ParticipantPercentage
                       FROM MemberGroups mg
                       INNER JOIN Sessions s ON mg.SessionID = s.SessionID
                       GROUP BY S.SessionType
                       ORDER BY ParticipantPercentage DESC"; //s2 istedet for S for at kende forskel på specifik Session frem for alle
            return connection.Query<SessionPopularityData>(sql); }


        // Metode til at finde de mest travle dage på ugen - ændret af sandra - ved hjælp fra gemini.
        public IEnumerable<SessionDayData> GetBusiestDayOfWeek()
        {
            using var connection = _dbFactory.CreateConnection();

            // SQL-forespørgslen bruger SQLite-funktionen strftime('%w') til at finde ugedagen (0=Søndag, 1=Mandag...)
            string sql = @"
        SELECT 
            CASE CAST(STRFTIME('%w', S.DateTime) AS INT) 
                WHEN 0 THEN 'Søndag' 
                WHEN 1 THEN 'Mandag' 
                WHEN 2 THEN 'Tirsdag' 
                WHEN 3 THEN 'Onsdag' 
                WHEN 4 THEN 'Torsdag' 
                WHEN 5 THEN 'Fredag' 
                ELSE 'Lørdag' 
            END AS DayOfWeek,
            S.SessionType, 
            COUNT(MG.MemberID) AS ParticipantCount
        FROM MemberGroups mg
        INNER JOIN Sessions s ON mg.SessionID = s.SessionID
        GROUP BY S.SessionType, DayOfWeek 
        ORDER BY ParticipantCount DESC";

            return connection.Query<SessionDayData>(sql);
        } // CASE...END: dette oversætter det nummerede resultat (0-6) til læselige danske ugedage.
          // GROUP BY S.SessionType, DayOfWeek: sikrer at deltagerne tælles separat for hvert hold på hver ugedag.
    }
}
