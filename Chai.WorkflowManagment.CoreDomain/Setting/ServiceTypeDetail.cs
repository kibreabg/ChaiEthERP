using Chai.WorkflowManagment.CoreDomain.Requests;
using System;
using System.ComponentModel.DataAnnotations;

namespace Chai.WorkflowManagment.CoreDomain.Setting
{
    [Table("ServiceTypeDetails")]
    public partial class ServiceTypeDetail : IEntity
    {
        public ServiceTypeDetail()
        {
        }
        public int Id { get; set; }
        public string Description { get; set; }        
        public bool Status { get; set; }
        public virtual ServiceType ServiceType { get; set; }
       

    }
}
