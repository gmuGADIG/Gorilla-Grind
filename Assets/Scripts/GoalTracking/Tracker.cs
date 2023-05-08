using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tracker 
{
    public abstract float GetCount();
    public abstract int GetCount(string type);
}
