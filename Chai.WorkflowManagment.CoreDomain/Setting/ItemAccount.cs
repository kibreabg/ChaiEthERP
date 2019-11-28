using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Setting
{
    public partial class ItemAccount : IEntity 
    {
        public ItemAccount()
        {
            this.ItemAccountChecklists = new List<ItemAccountChecklist>();
        }
        public int Id { get; set; }
        public string AccountName { get; set; }
        public string AccountCode { get; set; }
        public string Status { get; set; }
        public virtual IList<ItemAccountChecklist> ItemAccountChecklists { get; set; }
        public virtual ItemAccountChecklist GetChecklist(int Id)
        {
            foreach (ItemAccountChecklist cl in ItemAccountChecklists)
            {
                if (cl.Id == Id)
                    return cl;
            }
            return null;
        }
        public virtual IList<ItemAccountChecklist> GetChecklistsByAccountId(int accountId)
        {
            IList<ItemAccountChecklist> Checklists = new List<ItemAccountChecklist>();
            foreach (ItemAccountChecklist cl in ItemAccountChecklists)
            {
                if (cl.ItemAccount.Id == accountId)
                    ItemAccountChecklists.Add(cl);
            }
            return Checklists;
        }
        public virtual void RemoveChecklist(int Id)
        {
            foreach (ItemAccountChecklist cl in ItemAccountChecklists)
            {
                if (cl.Id == Id)
                    ItemAccountChecklists.Remove(cl);
                break;
            }

        }

    }
}
