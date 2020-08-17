using Chai.WorkflowManagment.CoreDomain.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Inventory
{
    public partial class Receive : IEntity
    {
        public Receive()
        {
            ReceiveDetails = new List<ReceiveDetail>();
        }
        public int Id { get; set; }        
        public string ReceiveNo { get; set; }
        public string InvoiceNo { get; set; }      
        public Nullable<DateTime> ReceiveDate { get; set; }
        public int Receiver { get; set; }
        public int DeliveredBy { get; set; }
        public virtual Program Program { get; set; }
        public virtual Project Project { get; set; }
        public virtual Grant Grant { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual Store Store { get; set; }
        public IList<ReceiveDetail> ReceiveDetails { get; set; }
        public virtual ReceiveDetail GetReceiveDetail(int Id)
        {
            foreach (ReceiveDetail rd in ReceiveDetails)
            {
                if (rd.Id == Id)
                    return rd;
            }
            return null;
        }
        public virtual void RemoveReceiveDetail(int Id)
        {
            foreach (ReceiveDetail rd in ReceiveDetails)
            {
                if (rd.Id == Id)
                    ReceiveDetails.Remove(rd);
                break;
            }
        }
    }
}
