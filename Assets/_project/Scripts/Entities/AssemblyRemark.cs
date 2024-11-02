namespace _project.Scripts.Entities
{
    public class AssemblyRemark
    {
        public readonly string m_message;
        public readonly TYPE m_type;

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