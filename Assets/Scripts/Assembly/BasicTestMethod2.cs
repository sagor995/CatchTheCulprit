

using System;
using UnityEngine;

public class BasicTestMethod2
{
    public Transform transform;

    public BasicTestMethod2() {
        
    }


    public int GameModesCondition(int value, int mode) {

        if (value == 0)
            return 0;

        switch (mode)
        {
            case 1:
                if (value < 2) {
                    return 2;
                } else if (value>60) {
                    return 3;
                }
                else
                {
                    return 1;
                }
            case 2:
                if (value < 200)
                {
                    return 2;
                }
                else if (value > 5000)
                {
                    return 3;
                }
                else
                {
                    return 1;
                }
            case 3:
                if (value < 2)
                {
                    return 2;
                }
                else if (value > 50)
                {
                    return 3;
                }
                else
                {
                    return 1;
                }
        }
        return 0;
    }


    public bool AreEqual(int[] first, int[] second)
    {

        if (first.Length != second.Length)
            return false;

        Array.Sort(first);
        Array.Sort(second);

        for (int i = 0; i < first.Length; i++)
            if (first[i] != second[i])
                return false;
        return true;
    }
}
