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
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public DateTime? DateOfBirth { get; init; }
        public string? Email { get; init; }
        public int? PhoneNumber { get; init; }
        public int? MemberType { get; init; }
        public bool? Active { get; init; }
    }

    public record Instructor
    {
        public int? InstructorID { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public bool? CertifiedForTrailRunning { get; init; }
        public bool? CertifiedForSkovYoga { get; init; }
    }

    public record Session
    {
        public int? SessionID { get; init; }
        public string? SessionType { get; init; }
        public DateTime? DateTime { get; init; }
        public int? SessionDuration { get; init; }
        public int? MaxMembers { get; init; }
    }

    public record MemberGroup
    {
        public int? GroupingID { get; init; }
        public int? MemberID { get; init; }
        public int? SessionID { get; init; }
    }

    public record InstructorGroup
    {
        public int? GroupingID { get; init; }
        public int? InstructorID { get; init; }
        public int? SessionID { get; init; }
    }
}
