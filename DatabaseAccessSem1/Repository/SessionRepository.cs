using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DatabaseAccessSem1.Repository
{
    public class SessionRepository
    {
        private readonly IDbConnectionFactory _dbFactory;
        public SessionRepository(IDbConnectionFactory dbFactory) { _dbFactory = dbFactory; }



        public Session Create(Session session)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = @"INSERT INTO Sessions 
                        (SessionType, DateTime, SessionDuration, MaxMembers, Location) Values 
                        (@SessionType, @DateTime, @SessionDuration, @MaxMembers, @Location) RETURNING *;";

            return connection.QuerySingle<Session>(sql, session);
        }

        public IEnumerable<int> GetID(
            string? sessionType = null,
            DateTime? dateTimeStart = null,
            DateTime? dateTimeEnd = null,
            int? sessionDurationStart = null,
            int? sessionDurationEnd = null,
            int? maxMembers = null,
            int? minMembers = null,
            string? location = null)
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
                sqlBuilder.Append(" AND DateTime <= @DateTimeEnd");
                parameters.Add("DateTimeEnd", dateTimeEnd);
            }
            if (sessionDurationStart.HasValue)
            {
                sqlBuilder.Append(" AND SessionDuration >= @SessionDurationStart");
                parameters.Add("SessionDurationStart", sessionDurationStart);
            }
            if (sessionDurationEnd.HasValue)
            {
                sqlBuilder.Append(" AND SessionDuration < @SessionDurationEnd");
                parameters.Add("SessionDurationEnd", sessionDurationEnd);
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
            if (!string.IsNullOrEmpty(location))
            {
                sqlBuilder.Append(" AND Location LIKE @Location");
                parameters.Add("Location", location);
            }

            return connection.Query<int>(sqlBuilder.ToString(), parameters); // Selve forespørgsel til database

        }

        public IEnumerable<Session> GetAll()
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = "SELECT * FROM Sessions";
            return connection.Query<Session>(sql); // Selve forespørgsel til database
        }

        public Session GetByID(int sessionID)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = "SELECT * FROM Sessions WHERE SessionID = @SessionID;";

            return connection.QuerySingle<Session>(sql, new { SessionID = sessionID });
        }

        public int GetSlotsAvailable(int sessionID)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = @"
                        SELECT (
                            s.MaxMembers - (SELECT COUNT(*) FROM MemberGroups 
                            WHERE SessionID = s.SessionID)
                            ) 
                        FROM Sessions s
                        WHERE s.SessionID = @SessionID";

            return connection.ExecuteScalar<int>(sql, new { SessionID = sessionID });
        }

        public int Update(Session session)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = @" UPDATE Sessions
                        SET
                            SessionType = @SessionType,
                            DateTime = @DateTime,
                            SessionDuration = @SessionDuration,
                            MaxMembers = @MaxMembers,
                            Location = @Location
                        WHERE SessionID = @SessionID";
            return connection.Execute(sql, session); //Returnere mængden af rækker opdateret (forhåbeligt 1)
        }

        public int Delete(int sessionID)
        {
            using var connection = _dbFactory.CreateConnection(); //med using lukkes forbindelse automatisk efter metoden er kørt

            string sql = @"DELETE FROM Sessions
                        WHERE SessionID = @SessionID";

            return connection.Execute(sql, new { SessionID = sessionID });
        }

            //Metode til tælle hvor mange hold et medlem har tilmeldt sig i den nuværrende uge. - Med hjælp fra chatten.
            public int GetWeeklySessionCount(int memberId)
            {
                using var connection = _dbFactory.CreateConnection();               //  Med using lukkes forbindelse automatisk efter metoden er kørt

			    string sql = @"
                SELECT COUNT(*) 
                FROM MemberGroups mg                                              
                INNER JOIN Sessions s ON mg.SessionID = s.SessionID                 //Joiner MemberGroups med Sessions for at få adgang til Session datoer
                WHERE mg.MemberID = @MemberID                                       //Sikrer at det er det rigtige medlem
                AND DATEPART(week, s.DateTime) = DATEPART(week, GETDATE())        //Sikrer at det er den aktuelle uge
                AND DATEPART(year, s.DateTime) = DATEPART(year, GETDATE())";      //Sikrer at det er det aktuelle år

			    return connection.ExecuteScalar<int>(sql, new { MemberID = memberId });

            }
        }
}