using System;
using System.Collections.Generic;
using System.Text;
using DatabaseAccessSem1.Repository;
using static System.Collections.Specialized.BitVector32;

namespace DatabaseAccessSem1.Services
{
	public class BookingService
	{
		private readonly SessionRepository _sessionRepository;
		private readonly MemberGroupRepository _memberGroupRepository;

		public BookingService(SessionRepository sessionRepository, MemberGroupRepository memberGroupRepository)
		{
			_sessionRepository = sessionRepository;
			_memberGroupRepository = memberGroupRepository;
		}

		public string TryBookSession(int memberId, Membership membership, int sessionId)
		{
			// 1) Tjek om medlemmet kan booke flere hold denne uge
			if (!CanBook(memberId, membership))
				return "Du har nået dit maksimale antal hold for denne uge." +
					"\n Vil du opgradere dit medlemsskab for 200kr ekstra pr mdr.";


			// 2) Tjek om der er plads på holdet 
			int slotsAvailable = _sessionRepository.GetSlotsAvailable(sessionId);
			if (slotsAvailable <= 0)
				return "Der er ikke flere pladser tilbage på holdet";

			// 3) Booker medlemmet på holdet
			var memberGroup = new MemberGroup
			{
				MemberID = memberId,
				SessionID = sessionId
			};
			_memberGroupRepository.Create(memberGroup);

			var session = _sessionRepository.GetByID(sessionId);
			return $"Booking gennemført\nHold: {session.SessionType}\nTidspunkt: {session.DateTime}";
		}

		public bool CanBook(int memberId, Membership membership)
		{
			int weeklyCount = _sessionRepository.GetWeeklySessionCount(memberId);
			return weeklyCount < membership.GetWeeklyVisit();
		}
	}
}