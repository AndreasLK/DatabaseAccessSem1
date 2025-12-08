using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseAccessSem1
{

    //Basically en klasse der ikke (nemt) kan ændres.

    //Navne på variabler skal matche databasen og følger derfor ikke styleguiden

    public record Member
    {
        public int? MemberID { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public DateTime? DateOfBirth { get; init; }
        public string? Email { get; init; }
        public int? PhoneNumber { get; init; }
        public required int MemberType { get; init; }
        public required bool Active { get; init; }
    }

    public record Instructor
    {
        public int? InstructorID { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required bool CertifiedForTrailRunning { get; init; }
        public required bool CertifiedForSkovYoga { get; init; }

        //ændret af sandra
        public required bool CertifiedForHIIT { get; init; }
        public required bool CertifiedForPowerwalk { get; init; }
        public required bool CertifiedForTrappeløb { get; init; }
        public required bool CertifiedForMountainbike { get; init; }
    }

    public record Session
    {
        public int? SessionID { get; init; }
        public required string SessionType { get; init; }
        public required DateTime DateTime { get; init; }
        public int? SessionDuration { get; init; }
        public required int MaxMembers { get; init; }
    }

    public record MemberGroup
    {
        public int? GroupingID { get; init; }
        public required int MemberID { get; init; }
        public required int SessionID { get; init; }
    }

    public record InstructorGroup
    {
        public int? GroupingID { get; init; }
        public required int InstructorID { get; init; }
        public required int SessionID { get; init; }
    }

    //ændret af sandra 
    public record SessionPopularityData
    {
        public required string SessionType { get; init; }
        public required int ParticipantCount { get; init; }
        public required decimal ParticipantPercentage { get; init; }
    }

}
