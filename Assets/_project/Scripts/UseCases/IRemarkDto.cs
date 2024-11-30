using _project.Scripts.Entities;

namespace _project.Scripts.UseCases
{
    public interface IRemarkDto
    {
        public enum RemarkType
        {
            Issue,Remark
        }
        string[] GetMessages();
        RemarkType[] GetRemarkType();
    }
}