using Chai.WorkflowManagment.CoreDomain.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class BidderItemDetail : IEntity
    {

        public BidderItemDetail()
        {
            this.Bidders = new List<Bidder>();
        }

        public int Id { get; set; }

        public virtual BidAnalysisRequest BidAnalysisRequest { get; set; }
       // public virtual Bidder Bidder { get; set; }
        public virtual ItemAccount ItemAccount { get; set; }

        public string ItemDescription { get; set; }
        public int Qty { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        public virtual Project Project { get; set; }
        public virtual Grant Grant { get; set; }
        public virtual IList<Bidder> Bidders { get; set; }



        #region Bidders

        public virtual Bidder GetBidderbyRank()
        {

            foreach (Bidder bidder in Bidders)
            {
                if (bidder.Rank == 1)
                    return bidder;

            }
            return null;
        }
        public virtual Bidder GetTBidder(int bidId)
        {
            foreach (Bidder BID in Bidders)
            {
                if (BID.Id == bidId)
                    return BID;
            }
            return null;
        }
       
        public virtual IList<Bidder> GetBidderByItemId(int itemId)
        {
            IList<Bidder> BIDs = new List<Bidder>();
            foreach (Bidder BID in Bidders)
            {
                if (BID.BidderItemDetail.Id == itemId)
                    BIDs.Add(BID);
            }
            return BIDs;
        }
        public virtual void RemoveBidder(int Id)
        {
            foreach (Bidder BID in Bidders)
            {
                if (BID.Id == Id)
                    Bidders.Remove(BID);
                break;
            }
        }
        #endregion
    }
}
