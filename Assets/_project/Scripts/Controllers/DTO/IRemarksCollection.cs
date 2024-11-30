namespace _project.Scripts.Controllers.DTO
{
    public interface IRemarksCollection
    {
        public enum RemarksType
        {
            Remark,Issue
        }
        string[] GetRemarkMessages();
        RemarksType[] GetRemarkType();
    }
}