
namespace Chai.WorkflowManagment.CoreDomain.Setting
{
    public class Program : IEntity
    {
        public int Id { get; set; }
        public string ProgramName { get; set; }
        public string ProgramCode { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}
