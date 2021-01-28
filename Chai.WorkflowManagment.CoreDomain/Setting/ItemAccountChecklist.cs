using Chai.WorkflowManagment.CoreDomain.Requests;
using System;
using System.ComponentModel.DataAnnotations;

namespace Chai.WorkflowManagment.CoreDomain.Setting
{
    [Table("ItemAccountChecklists")]
    public partial class ItemAccountChecklist : IEntity
    {
        public ItemAccountChecklist()
        {
        }
        public int Id { get; set; }
        public string ChecklistName { get; set; }        
        public string Status { get; set; }
        public virtual ItemAccount ItemAccount { get; set; }
        public virtual CPRAttachment CPRAttachment { get; set; }
        public virtual PRAttachment PRAttachment { get; set; }
        public virtual ELRAttachment ELRAttachment { get; set; }

    }
}
