using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AssemblyStep
{
    public int StepID;
    public string Title;
    public List<Indication> Indications = new List<Indication>();
    public Sprite StepIllustration;
}