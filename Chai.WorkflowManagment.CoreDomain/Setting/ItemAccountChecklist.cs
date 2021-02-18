using Chai.WorkflowManagment.CoreDomain.Requests;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Chai.WorkflowManagment.CoreDomain.Setting
{
    [Table("ItemAccountChecklists")]
    public partial class ItemAccountChecklist : IEntity
    {
        public ItemAccountChecklist()
        {
            this.CPRAttachments = new List<CPRAttachment>();
            this.ELRAttachments = new List<ELRAttachment>();
        }
        public int Id { get; set; }
        public string ChecklistName { get; set; }
        public string Status { get; set; }
        public virtual ItemAccount ItemAccount { get; set; }
        public virtual PRAttachment PRAttachment { get; set; }
        public virtual IList<ELRAttachment> ELRAttachments { get; set; }
        public virtual IList<CPRAttachment> CPRAttachments { get; set; }

    }
}
