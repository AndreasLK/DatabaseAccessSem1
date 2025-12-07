using DatabaseAccessSem1.Repository;
using DatabaseAccessSem1;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text;


//Ændret af sandra - ved hjælp af Gemini
namespace DatabaseAccessSem1.Reporting
{
    public class ReportingStatistics
    {
        private readonly MemberRepository _memberRepository;
        private readonly MemberGroupRepository _memberGroupRepository;

        public ReportingStatistics(MemberRepository memberRepository, MemberGroupRepository memberGroupRepository)
        {
            _memberRepository = memberRepository;
            _memberGroupRepository = memberGroupRepository;
        }

        public void GenerateReport(string filePath)
        {
            int activeMembers = _memberRepository.GetActiveMemberCount();

            var sessions = _memberGroupRepository.GetSessionPopularity().ToList();

            var popularSessions = sessions.Take(3).ToList();

            var unpopularSessions = sessions.OrderBy(s => s.ParticipantCount).Take(3).ToList();

            var sb = new StringBuilder();
            sb.AppendLine("Fitness Center Rapport - statistik");
            sb.AppendLine($"Genereret denne Dato: {DateTime.Now}");
            sb.AppendLine();
            sb.AppendLine($"Antal aktive medlemmer: {activeMembers}");
            sb.AppendLine();
            sb.AppendLine($" TOP {popularSessions.Count} MEST POPULÆRE HOLD");
            foreach (var session in popularSessions)
            {
                sb.AppendLine($" - {session.SessionType}: {session.ParticipantCount} deltagere");
            }
            sb.AppendLine();

            sb.AppendLine($" TOP {unpopularSessions.Count} MINDST POPULÆRE HOLD");
            foreach (var session in unpopularSessions)
            {
                sb.AppendLine($" - {session.SessionType}: {session.ParticipantCount} deltagere");
            }

            try
            {
                File.WriteAllText(filePath, sb.ToString());
                Console.WriteLine($"Rapport genereret og gemt til:\n{filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FEJL ved lagring af rapport: {ex.Message}");
                // Optionally log to console as well
                Console.WriteLine($"Fejl ved skrivning af rapport: {ex.Message}");
            }
        }
    }
}

