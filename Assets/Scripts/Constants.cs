using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{

    public const int MapSizeStart = 100;
    public const int MapSize_1 = 30;
    

    public const int MapSize_2 = 50;
    
    public const int MapSize_3 = 60;

    public const int CaseSize = 10;

    public const float altitude = 60f; //amplitude de l'altitude des blocks

    public const int bioCase = 100;
    
    public static int GetConstant(int level)
    {
        int size = 0;
        
        if (level == -1)
        {
            size = Constants.MapSizeStart;
        }
        
        if (level == 0)
        {
            size = Constants.MapSize_1;
        }

        if (level == 1)
        {
            size = Constants.MapSize_2;
        }

        if (level == 2)
        {
            size = Constants.MapSize_3;
        }

        return size;
    }
    
    public static void IncrementLevel(int level)
    {
        level += 1;
    }
}
