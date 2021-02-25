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

        public string FullName { get { return FirstName + " " + LastName; } }
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



        public virtual Contract GetActiveContractForEmp()
        {

            foreach (Contract contt in Contracts)
            {
                if (contt.Status == "Active")
                    return contt;

            }
            return null;
        }

        public virtual Contract GetMyPrevContract(int Id)
        {
            Contract con = Contracts.Last(y => y.Status == "In Active");

            return con;

        }
        public virtual Contract GetPreviousContract()
        {
            if (Contracts.Count > 1)
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
                        if (CD.EmployeeDetails.Last().Program != null)
                        {
                            return CD.EmployeeDetails.Last().Program.ProgramName;
                        }
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

                        if (CD.EmployeeDetails.Last().Position != null)
                        {
                            return CD.EmployeeDetails.Last().Position.PositionName;
                        }
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

        public virtual double Leavefromhiredtonow()
        {
            DateTime DateHired = GetEmployeeHiredDate();
            double leaveEnti = 0;
            double Sumleave = 0;
            TimeSpan workingdays = DateTime.Now - DateHired;
            TimeSpan Settingdays = DateTime.Now - LeaveSettingDate.Value;
            double Totworkingdays = workingdays.Days;
            double wd = workingdays.Days;
           
            int count = 1;
            if (wd > 365)
            {
                while (wd > 365)
                {
                    if (count <= 6 && count > 1)
                        leaveEnti = leaveEnti + 2;

                  

                    count++;
                   wd = wd - 365;
                }
                if (leaveEnti < 10 && wd != 0)
                {
                    leaveEnti += 2;
                }

            }

            return (Settingdays.Days * (20 + leaveEnti) / 12) / 30;
           
        }
        public virtual double LeavefromhiredtoYE()
        {
            DateTime YE = new DateTime(DateTime.Today.Year, 12, 31);
            DateTime DateHired = GetEmployeeHiredDate();
            double leaveEnti = 0;
            double Sumleave = 0;
            TimeSpan workingdays = YE - DateHired;
            TimeSpan Settingdays = YE - LeaveSettingDate.Value;
            double Totworkingdays = workingdays.Days;
            double wd = workingdays.Days;
            int count = 1;
            if (wd > 365)
            {
                while (wd > 365)
                {
                    if (count <= 6 && count > 1)
                        leaveEnti = leaveEnti + 2;


                    count++;
                    wd = wd - 365;
                }
            
            }
            //if (leaveEnti < 10 && wd != 0)
            //{
            //    leaveEnti += 2;
            //}

            if ( (Settingdays.Days * (20 + leaveEnti) / 12) / 30 > 30 )
            return 30;
            else
            return (Settingdays.Days * (20 + leaveEnti) / 12) / 30;
     
        }
        public virtual double LeavefromhiredtoCED(DateTime CED)
        {
            DateTime ced = CED;

            DateTime DateHired = GetEmployeeHiredDate();
            double leaveEnti = 0;
            double Sumleave = 0;
            TimeSpan workingdays = ced - DateHired;
            TimeSpan Settingdays = ced - LeaveSettingDate.Value;
            double Totworkingdays = workingdays.Days;
            double wd = workingdays.Days;

            int count = 1;
            if (wd > 365)
            {
                while (wd > 365)
                {
                    if (count <= 6 && count > 1)
                        leaveEnti = leaveEnti + 2;
                                       
                    count++;
                    wd = wd - 365;
                }
              
            }
            if (leaveEnti < 10 && wd != 0)
            {
                leaveEnti += 2;
            }

            if ((Settingdays.Days * (20 + leaveEnti) / 12) / 30 > 30)
                return 30;
            else
                return (Settingdays.Days * (20 + leaveEnti) / 12) / 30;
        }
        public virtual double LeavefromhiredtoSettingDate()
        {
            double leaveEnti = 0;
            double Sumleave = 0;
            TimeSpan workingdays = LeaveSettingDate.Value - GetEmployeeHiredDate();
            double Totworkingdays = workingdays.Days;
            double wd = workingdays.Days;
            int count = 1;
            if (wd > 365)
            {
                while (wd > 365)
                {
                    if (count <= 6 && count > 1)
                        leaveEnti = leaveEnti + 2;
                                      

                    count++;
                    wd = wd - 365;
                }
               
            }
            if (leaveEnti < 10 && wd != 0)
            {
                leaveEnti += 2;
            }

            return ((Totworkingdays / 30) * ((20 + leaveEnti)/12));
      
        }
        public virtual double Leavefromhiredtolast(DateTime lastday)
        {
            // DateTime lastday = lastday;

            DateTime DateHired = GetEmployeeHiredDate();
            double leaveEnti = 0;
            double Sumleave = 0;
            TimeSpan workingdays = lastday - DateHired;
            TimeSpan Settingdays = lastday - LeaveSettingDate.Value;
            double Totworkingdays = workingdays.Days;
            double wd = workingdays.Days;

            int count = 1;
            if (wd > 365)
            {
                while (wd > 365)
                {
                    if (count <= 6 && count > 1)
                        leaveEnti = leaveEnti + 2;

                  

                    count++;
                    wd = wd - 365;
                }
                
            }
            if (leaveEnti < 10 && wd != 0)
            {
                leaveEnti += 2;
            }

            if ((Settingdays.Days * (20 + leaveEnti) / 12) / 30 > 30)
                return 30;
            else
                return (Settingdays.Days * (20 + leaveEnti) / 12) / 30;
        }

        public virtual double EmployeeLeaveBalance()
        {

            if ((Leavefromhiredtonow()) + Convert.ToDouble(SDLeaveBalance.Value) > 50)
            {
                return 50;
            }
            else
            { return ((Leavefromhiredtonow()) + Convert.ToDouble(SDLeaveBalance.Value)); }

        }
        public virtual double EmployeeLeaveBalanceCED(DateTime CED)
        {

            if (LeavefromhiredtoCED(CED)  + Convert.ToDouble(SDLeaveBalance.Value) > 50)
            {
                return 50;
            }
            else
            { return (LeavefromhiredtoCED(CED)) + Convert.ToDouble(SDLeaveBalance.Value); }
        }
        public virtual double EmployeeLeaveBalanceYE()
        {
           // return 0;
            if (LeavefromhiredtoYE() + Convert.ToDouble(SDLeaveBalance.Value) > 50)
            {
                return 50;
            }
            else
            { return LeavefromhiredtoYE()  + Convert.ToDouble(SDLeaveBalance.Value); }
        }
        public virtual double EmployeeLeaveBalanceLastDay(DateTime Lastday)
        {
           
            if ((Leavefromhiredtolast(Lastday)  + Convert.ToDouble(SDLeaveBalance.Value) > 50))
            {
                return 50;
            }
            else
            { return (Leavefromhiredtolast(Lastday) + Convert.ToDouble(SDLeaveBalance.Value)); }
        }
        #endregion

    }
}
