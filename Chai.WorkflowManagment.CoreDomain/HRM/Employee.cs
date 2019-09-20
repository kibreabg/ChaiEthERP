using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;
using System.ComponentModel.DataAnnotations;

namespace Chai.WorkflowManagment.CoreDomain.HRM
{
    public partial class Employee : IEntity
    {
        public Employee()
        {

            this.Contracts = new List<Contract>();
            this.Educations = new List<Education>();
            this.EmergencyContacts = new List<EmergencyContact>();
            
            this.FamilyDetails = new List<FamilyDetail>();
            this.Terminations = new List<Termination>();
            this.WorkExperiences = new List<WorkExperience>();
            this.Warnings = new List<Warning>();
           
        }
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string MaritalStatus { get; set; }
        public string Nationality { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string CellPhone { get; set; }
        public string PersonalEmail { get; set; }
        public string ChaiEMail { get; set; }
        public string Photo { get; set; }
        public Nullable<decimal> SDLeaveBalance { get; set; }
        public Nullable<decimal> ExpiredLeave { get; set; }
        
        public DateTime? LeaveSettingDate { get; set; }
        public Nullable<Boolean> Status { get; set; }
        [Required]
        public virtual AppUser AppUser { get; set; }
        public virtual IList<Contract> Contracts { get; set; }
        public virtual IList<Education> Educations { get; set; }
        public virtual IList<EmergencyContact> EmergencyContacts { get; set; }
       // public virtual IList<EmployeeDetail> EmployeeDetails { get; set; }
        public virtual IList<FamilyDetail> FamilyDetails { get; set; }
        public virtual IList<Termination> Terminations { get; set; }
        public virtual IList<WorkExperience> WorkExperiences { get; set; }

        public virtual IList<Warning> Warnings { get; set; }


      
        #region Contracts
        public virtual Contract GetContract(int Id)
        {

            foreach (Contract CD in Contracts)
            {
                if (CD.Id == Id)
                    return CD;
            }
            return null;
        }
        public virtual Contract GetPreviousContract()
        {
            if (Contracts.Count >1)
            {
                return Contracts[Contracts.Count - 2];
            }
            
            return null;
        }
        public virtual Contract GetEmpContract(int Id)
        {

            foreach (Contract CD in Contracts)
            {
                if (CD.Employee.Id == Id)
                    return CD;
            }
            return null;
        }
        public bool GetInActiveContract()
        {
            Contract con = Contracts.Last();
            if (con.Status == "In Active")
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public virtual Contract GetLastInActiveContract()
        {
            Contract con = Contracts.Last();
            if (con != null)
            {
                return con;
            }
            else
            {
                return null;
            }

        }
        public virtual Contract GetActiveContract()
        {
            if (Contracts.Count > 0)
            {
                Contract con = Contracts.Last();
                return con;
            }
            else
                return null;
            

        }
        public virtual void RemoveContract(int Id)
        {
            foreach (Contract CD in Contracts)
            {
                if (CD.Id == Id)
                {
                    Contracts.Remove(CD);
                    break;
                }
            }
        }
        #endregion
        


        #region Education
        public virtual Education GetEducation(int Id)
        {

            foreach (Education ED in Educations)
            {
                if (ED.Id == Id)
                    return ED;
            }
            return null;
        }


        public virtual void RemoveEducation(int Id)
        {
            foreach (Education ED in Educations)
            {
                if (ED.Id == Id)
                {
                    Educations.Remove(ED);
                    break;
                }
            }
        }
        #endregion
        #region EmergencyContact
        public virtual EmergencyContact GetEmergencyContact(int Id)
        {

            foreach (EmergencyContact ED in EmergencyContacts)
            {
                if (ED.Id == Id)
                    return ED;
            }
            return null;
        }


        public virtual void RemoveEmergencyContact(int Id)
        {
            foreach (EmergencyContact ED in EmergencyContacts)
            {
                if (ED.Id == Id)
                {
                    EmergencyContacts.Remove(ED);
                    break;
                }
            }
        }
        #endregion
        #region FamilyDetail
        public virtual FamilyDetail GetFamilyDetail(int Id)
        {

            foreach (FamilyDetail FD in FamilyDetails)
            {
                if (FD.Id == Id)
                    return FD;
            }
            return null;
        }


        public virtual void RemoveFamilyDetail(int Id)
        {
            foreach (FamilyDetail FD in FamilyDetails)
            {
                if (FD.Id == Id)
                {
                    FamilyDetails.Remove(FD);
                    break;
                }
            }
        }
        #endregion
        #region Termination
        public virtual Termination GetTerminations(int Id)
        {

            foreach (Termination TD in Terminations)
            {
                if (TD.Id == Id)
                    return TD;
            }
            return null;
        }


        public virtual void RemoveTermination(int Id)
        {
            foreach (Termination TD in Terminations)
            {
                if (TD.Id == Id)
                {
                    Terminations.Remove(TD);
                    break;
                }
            }
        }
        #endregion
        #region WorkExperience
        public virtual WorkExperience GetWorkExperience(int Id)
        {

            foreach (WorkExperience WE in WorkExperiences)
            {
                if (WE.Id == Id)
                    return WE;
            }
            return null;
        }


        public virtual void RemoveWorkExperience(int Id)
        {
            foreach (WorkExperience WE in WorkExperiences)
            {
                if (WE.Id == Id)
                {
                    WorkExperiences.Remove(WE);
                    break;
                }
            }
        }
        #endregion
        #region Employee detail
        public virtual string GetEmployeeDutyStation()
        {
            foreach (Contract CD in Contracts)
            {
                if (CD.Status == "Active")
                {
                    if (CD.EmployeeDetails.Count != 0)

                        return CD.EmployeeDetails.Last().DutyStation;
                }
                
            }


            return null;        
                
           
            
        }
        public virtual string GetEmployeeProgram()
        {
            foreach (Contract CD in Contracts)
            {
                if (CD.Status == "Active")
                {
                    if (CD.EmployeeDetails.Count != 0)

                        return CD.EmployeeDetails.Last().Program.ProgramName;
                }
                
            }

            return null;
        }
        public virtual string GetEmployeePosition()
        {
            foreach (Contract CD in Contracts)
            {
                if (CD.Status == "Active")
                {
                    if (CD.EmployeeDetails.Count != 0)

                        return CD.EmployeeDetails.Last().Position.PositionName;
                }
                
            }

            return null;   
            
        }
        #endregion
        #region Leave calculation Methods
        public virtual DateTime GetEmployeeHiredDate()
        {
            if (AppUser.ReHiredDate != null)
            {
                return AppUser.ReHiredDate.Value;
            }

            else
                return AppUser.HiredDate.Value;
            
        }

        public virtual decimal Leavefromhiredtonow()
        {
            DateTime DateHired = GetEmployeeHiredDate();
           
            decimal leaveEnti = 0;
            decimal Sumleave = 0;
            TimeSpan workingdays = DateTime.Now - DateHired;
            decimal wd = workingdays.Days;


            int count = 1;
            if (wd > 365)
            {
                while (wd > 365)
                {
                    if (count > 1)
                        leaveEnti = leaveEnti + 2;
                    if (count >= 6)
                        Sumleave = Sumleave + 30;
                    else
                        if (count == 1)
                        Sumleave = 20;
                        else
                        Sumleave = Sumleave + (20 + Convert.ToInt32(leaveEnti));

                    count++;
                    wd = wd - 365;
                }
                if (wd <= 365)
                {

                   // leaveEnti = leaveEnti + Math.Round(wd / 365);
                    Sumleave = Sumleave + ((wd * (20 + Convert.ToInt32(leaveEnti))) / 365);
                    
                }
            }
            else { //leaveEnti = leaveEnti + Math.Round(wd / 365);
                   Sumleave = Sumleave + ((wd * (20 + Convert.ToInt32(leaveEnti)) / 365));
            }

            return Sumleave;
        }
        public virtual int LeavefromhiredtoYE()
        {
            DateTime YE = new DateTime(DateTime.Today.Year, 12, 31);
            int leaveEnti = 0;
            int Sumleave = 0;
            TimeSpan workingdays = YE - GetEmployeeHiredDate();
            int wd = workingdays.Days;


            int count = 1;
            if (wd > 365)
            {
                while (wd > 365)
                {
                    if (count > 1)
                        leaveEnti = leaveEnti + 2;
                    if (count >= 6)
                        Sumleave = Sumleave + 30;
                    else
                        if (count == 1)
                        Sumleave = 20;
                        else
                        Sumleave = Sumleave + (20 + Convert.ToInt32(leaveEnti));

                    count++;
                    wd = wd - 365;
                }
                if (wd <= 365)
                {
                    Sumleave = Sumleave + ((wd * (20 + Convert.ToInt32(leaveEnti))) / 365);
                }
            }
            else { Sumleave = Sumleave + ((wd * (20 + Convert.ToInt32(leaveEnti))) / 365); }

            return Sumleave;
        }
        public virtual decimal LeavefromhiredtoCED(DateTime CED)
        {
            DateTime ced = CED;

            int leaveEnti = 0;
            int Sumleave = 0;
            TimeSpan workingdays =  ced - GetEmployeeHiredDate();
            int wd = workingdays.Days;


            int count = 1;
            if (wd > 365)
            {
                while (wd > 365)
                {
                    if (count > 1)
                        leaveEnti = leaveEnti + 2;
                    if (count >= 6)
                        Sumleave = Sumleave + 30;
                    else
                       if (count == 1)
                        Sumleave = 20;
                        else
                        Sumleave = Sumleave + (20 + Convert.ToInt32(leaveEnti));

                    count++;
                    wd = wd - 365;
                }
                if (wd <= 365)
                {
                    Sumleave = Sumleave + ((wd * (20 + Convert.ToInt32(leaveEnti))) / 365);
                }
            }
            else { Sumleave = Sumleave + ((wd * (20 + Convert.ToInt32(leaveEnti))) / 365); }

            return Sumleave;
        }
        public virtual int LeavefromhiredtoSettingDate()
        {
            int leaveEnti = 0;
            int Sumleave = 0;
            TimeSpan workingdays = LeaveSettingDate.Value - GetEmployeeHiredDate();
            int wd = workingdays.Days;
            int count = 1;
            if (wd > 365)
            {
                while (wd > 365)
                 {
                    if (count > 1)
                        leaveEnti = leaveEnti + 2;
                    if (count >= 6)
                        Sumleave = Sumleave + 30;
                    else
                        if (count == 1)
                        Sumleave = 20;
                        else
                        Sumleave = Sumleave + (20 + Convert.ToInt32(leaveEnti));

                    count++;
                    wd = wd - 365;
                }
                if (wd <= 365)
                {
                    Sumleave = Sumleave + ((wd * (20 + Convert.ToInt32(leaveEnti))) / 365);
                }
            }
            else { Sumleave = Sumleave + ((wd * (20 + Convert.ToInt32(leaveEnti))) / 365); }

            return Sumleave;
        }
        public virtual decimal Leavefromhiredtolast(DateTime lastday)
        {
           // DateTime lastday = lastday;

            decimal leaveEnti = 0;
            decimal Sumleave = 0;
            TimeSpan workingdays = lastday - GetEmployeeHiredDate(); ;
            decimal wd = workingdays.Days;


            int count = 1;
            if (wd > 365)
            {
                while (wd > 365)
                {
                    if (count > 1)
                        leaveEnti = leaveEnti + 2;
                    if (count >= 6)
                        Sumleave = Sumleave + 30;
                    else
                        if (count == 1)
                        Sumleave = 20;
                    else
                        Sumleave = Sumleave + (20 + Convert.ToInt32(leaveEnti));

                    count++;
                    wd = wd - 365;
                }
                if (wd <= 365)
                {

                    // leaveEnti = leaveEnti + Math.Round(wd / 365);
                    Sumleave = Sumleave + ((wd * (20 + Convert.ToInt32(leaveEnti))) / 365);

                }
            }
            else
            { //leaveEnti = leaveEnti + Math.Round(wd / 365);
                Sumleave = Sumleave + ((wd * (20 + Convert.ToInt32(leaveEnti)) / 365));
            }

            return Sumleave;
        }

        public virtual decimal EmployeeLeaveBalance()
        {
            return (Leavefromhiredtonow() - LeavefromhiredtoSettingDate()) + SDLeaveBalance.Value;
        }
        public virtual decimal EmployeeLeaveBalanceCED(DateTime CED)
        {
            
            if ((LeavefromhiredtoCED(CED) - LeavefromhiredtoSettingDate()) + SDLeaveBalance.Value > 50)
            {
                return 50;
            }
            else
            { return  (LeavefromhiredtoCED(CED) - LeavefromhiredtoSettingDate()) + SDLeaveBalance.Value; }
        }
        public virtual decimal EmployeeLeaveBalanceYE()
        {
             if ((LeavefromhiredtoYE() - LeavefromhiredtoSettingDate()) + SDLeaveBalance.Value > 50)
            {
                return 50;
            }
             else
                { return (LeavefromhiredtoYE() - LeavefromhiredtoSettingDate()) + SDLeaveBalance.Value; }
        }
        public virtual decimal EmployeeLeaveBalanceLastDay(DateTime Lastday)
        {

            if ((Leavefromhiredtolast(Lastday) - LeavefromhiredtoSettingDate()) + SDLeaveBalance.Value > 50)
            {
                return 50;
            }
            else
            { return (Leavefromhiredtolast(Lastday) - LeavefromhiredtoSettingDate()) + SDLeaveBalance.Value; }
        }
        #endregion

    }
}
