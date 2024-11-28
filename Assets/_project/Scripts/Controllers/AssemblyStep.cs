using System;
using System.Collections.Generic;
using UnityEngine;

namespace _project.Scripts.Controllers
{
    [Serializable]
    public class AssemblyStep
    {
        public int StepID;
        public string Title;
        public List<Indication> Indications;
        public Sprite StepIllustration;
    }
}