using System;

namespace _project.Scripts.Entities
{
    [Serializable]
    public class AssemblyRemark
    {
        public string m_message;
        public TYPE m_type;

        public enum TYPE
        {
            REMARK,
            ISSUE
        }

        public AssemblyRemark(TYPE type, string message)
        {
            m_type = type;
            m_message = message;
        }
    }
}