using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaMission : Mission
{
    public BananaMission(int bananaCount)
    {
        goal = bananaCount;
        Name = "Bananas";
        Description = "Get " + goal + " bananas in one run";
    }

    public override void UpdateProgress()
    {

    }
}
