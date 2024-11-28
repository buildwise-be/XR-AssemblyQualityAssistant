using System;
using System.Collections.Generic;
using UnityEngine;

namespace _project.Scripts.Controllers
{
    [Serializable]
    public class AssemblyStep
    {
        public int StepID;
        [SerializeField] LocalizedStringData[] Title;
        public List<Indication> Indications;
        public Sprite StepIllustration;

        public string GetTitle(string language)
        {
            foreach (var VARIABLE in Title)
            {
                if (VARIABLE.Language == language) return VARIABLE.TextData;
            }

            return "NO DATA FOUND FOR THIS LANGUAGE - " + language;
        }
    }
}