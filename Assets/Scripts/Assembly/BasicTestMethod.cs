using System;
using System.Collections;

public class BasicTestMethod
{
    public string[] cardAssignIndex = new string[4];

    public BasicTestMethod(string[] cards)
    {
        playerTypes = cards;
    }

    public string[] getCardValues()
    {
        ArrayList ints = new ArrayList();
        ints.Add(playerTypes[0]);
        ints.Add(playerTypes[1]);
        ints.Add(playerTypes[2]);
        ints.Add(playerTypes[3]);

        System.Random rand = new System.Random();

        for (int i = 0; (i < 4) && (ints.Count > 0); i++)
        {
                int randomIndex = rand.Next(ints.Count);
                cardAssignIndex[i] = (string)ints[randomIndex];
                ints.RemoveAt(randomIndex);
        }

        return cardAssignIndex;
    }

    public bool AreEqual(string[] first, string[] second){

        if (first.Length != second.Length)
            return false;

        Array.Sort(first);
        Array.Sort(second);

        for (int i = 0; i < first.Length; i++)
            if (first[i] != second[i])
                return false;
        return true;
    }


    public string[] playerTypes { get; private set; }
}
