public enum MembershipType
{
	Basis,   // Max 2 hold i ugen
	Premium, // Ubegrænset hold
	VIP      // Ubegrænset hold
}

public class Membership
{
	public MembershipType Type { get; private set; }
	public string Name { get; private set; } = string.Empty;
	public Membership(MembershipType type)
	{
		Type = type;
	}

	// Returnerer max antal hold pr. uge for Basis medlemskab
	//Hvis medlemstypen er andet end Basis, returneres int.MaxValue (ubegrænset)  (Næsten, good luck med at booke 2 milliarder hold på en uge :) )
	public int GetWeeklyVisit()
	{
		if (Type == MembershipType.Basis)
			return 2;
		else
			return int.MaxValue;
	}
}


