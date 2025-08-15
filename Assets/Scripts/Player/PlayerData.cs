using UnityEngine;

public class PlayerData
{
    public ulong ClientId;
    public int Health;
    public int Gold;
    public int WinStreak;
    public int LoseStreak;
    public int Level;
    public int Exp;

    public PlayerData(ulong clientId)
    {
        ClientId = clientId;
        Health = 100;
        Gold = 0;
        WinStreak = 0;
        LoseStreak = 0;
        Level = 1;
        Exp = 0;
    }
}
