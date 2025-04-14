using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public float Cash;
    public float Budget;
    public float Energy;
    public float Happiness;

    public int Days;
    public int Hours;
    public List<BankCard> BankCards;
    public List<WorkProgressEntry> WorkProgressList;
}
[System.Serializable]
public class WorkProgressEntry
{
    public string JobId;
    public float Progress;
}
