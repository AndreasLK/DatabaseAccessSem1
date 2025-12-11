using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseAccessSem1.Repository
{
    public enum MemberType

    { 
        Basis,        // Max 2 hold i ugen
        Premium,     // Ubegrænset hold
        VIP         // Ubegræsnset hold
 
    }
    public class Membership
    {

        public MemberType type{ get; private set; }
        public int MaxWeeklyVisit { get; private set; } 
        private int currentWeeklyVisit;
        private int currentWeeklyVisitCount;

        public Membership(MemberType type)
        {
            Type = type;
            MaxWeeklyVisit = GetMaxVisit(type);
            currentWeeklyVisit = 0;
            currentWeekNumber = GetWeekNumber(DateTime.Now);
        }

        private int GetMaxVisit(MemberType type)
        {
            switch (type)
			{
                case MemberType.Basis => 2,
                case MemberType.Premium => int.MaxValue,
                case MemberType.VIP => int.MaxValue,
                default: return 0;
            }
		}

		private int GetWeekNumber(DateTime date)
		{
			var calendar = System.Globalization.CultureInfo.CurrentCulture.Calendar;        //Med hjælp fra chatten
			return calendar.GetWeekOfYear(date,
				System.Globalization.CalendarWeekRule.FirstFourDayWeek,                     //Er lidt usikker på hvordan dato reglerne er.
				DayOfWeek.Monday);                                                          //Men den regner kun en uge for en uge hvis der mindst er 4 dage i ugen. (nytår)
		}

		public bool TryRegisterVisit()                                                      //Bool retunere True hvis det er tilladt at tilmelde sig flere hold
		{                                                                                   //og false hvis grænsen er nået
			int weekNow = GetWeekNumber(DateTime.Now);

			if (weekNow != currentWeekNumber)
			{
				currentWeekNumber = weekNow;
				CurrentWeekVisits = 0;
			}

			if (CurrentWeekVisits < (int)Type)                                            // Brug af Enum værdi
			{
				CurrentWeekVisits++;
				return true;
			}
           
            Console.WriteLine("Du kan opgradere dit medlemsskab for 200kr mere pr. mdr. for ubegrænset hold");
            return false;
		}
     
	}




}
}
