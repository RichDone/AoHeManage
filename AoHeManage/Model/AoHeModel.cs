using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AoHeManage.Model
{
    public class AoHeModel
    {

    }
    public class Guest
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Sex { get; set; }
        public int Age { get; set; }
        public string NurseLevel { get; set; }
        public string FloorID { get; set; }
        public string RoomNo { get; set; }
        public string BedNo { get; set; }
        public string IDCardNo { get; set; }
        public DateTime? TryAdmissionDate { get; set; }
        public DateTime? BirthDay { get; set; }
        public DateTime AdmissionDate { get; set; }
        public DateTime? LeaveDate { get; set; }
        public DateTime? ChangeLevelDate { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }
    }

    public class Accident
    {
        public int AccidentID { get; set; }
        public int GuestID { get; set; }
        public List<AccidentRelatedPerson> ListAccidentRelatedPerson { get; set; }
        public DateTime CreateOn { get; set; }
        public string AccidentType { get; set; }
        public string Place { get; set; }
        public string Condition { get; set; }
        public string Remark { get; set; }

        public string RoomNo { get; set; }
        public string BedNo { get; set; }
        public string Name { get; set; }
        public int Sex { get; set; }
        public int Age { get; set; }
        public string NurseLevel { get; set; }

    }
    public class AccidentRelatedPerson
    {
        public int AccidentID { get; set; }
        public string StaffNo { get; set; }
        public string StaffName { get; set; }
    }
    public class Staff
    {
        public int ID { get; set; }
        public string StaffNo { get; set; }
        public string Name { get; set; }
        public int Sex { get; set; }
        public string IDCardNo { get; set; }
        public string StaffOtherNo { get; set; }
        public string Rank { get; set; }
        public string OldRank { get; set; }
        public string OldPost { get; set; }
        public string MasterStaffNo { get; set; }
        public int PostLevel { get; set; }
        public string PostName { get; set; }
        public int Status { get; set; }
        public DateTime? LeaveDate { get; set; }
        public DateTime? RegularDate { get; set; }
        public DateTime? HireDate { get; set; }
        public string Education { get; set; }
        public string SkillLevel { get; set; }
        public string Phone { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactPhone { get; set; }
        public DateTime? HealthCardDate { get; set; }
        public string NurseCardLevel { get; set; }
        public DateTime? NurseCardDate { get; set; }
        public DateTime? ContractDate { get; set; }
        public string NoCrimeProof { get; set; }
        public int IsMaster { get; set; }
        public string HouseholdType { get; set; }
        public string NativePlace { get; set; }
        public string RecruitmentChannel { get; set; }
        public string ContracType { get; set; }
    }
    public class DailyRecord
    {
        public int DailyRecordID { get; set; }
        public int GuestID { get; set; }
        public string DailyRecordType { get; set; }
        public DateTime CreateOn { get; set; }
        public string Remark { get; set; }
        public string ReportPerson { get; set; }
    }
    public class StaffEvaluate
    {
        public int StaffEvaluateID { get; set; }
        public string StaffNo { get; set; }
        public int EvaluateType { get; set; }
        public DateTime CreateOn { get; set; }
        public string Remark { get; set; }
    }
    public class AccidentFollow
    {
        public int AccidentID { get; set; }
        public DateTime FollowTime { get; set; }
        public string Remark { get; set; }
    }
    public class Room
    {
        public int ID { get; set; }
        public string RoomNo { get; set; }
        public string FloorID { get; set; }
        public int RoomSex { get; set; }
        public int PeopleNum { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
    }
    public class SchedualInfo
    {
        public string StaffNo { get; set; }
        public string DutyDate { get; set; }
        public string Schedual { get; set; }
        public string RoomNo { get; set; }
        public decimal Hours { get; set; }
        public string Remark { get; set; }
        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 1：夜班 0：非夜班
        /// </summary>
        public int IsNight { get; set; }
    }
    public class TemplateMapping
    {
        public int TemplateID { get; set; }
        public int SerialNo { get; set; }
        public int WhichDay { get; set; }
        public string Schedual { get; set; }
        public string Remark { get; set; }
    }
    public class SchedualTemplate
    {
        public int ID { get; set; }
        public string TemplateName { get; set; }
        public int PeopleNum { get; set; }
        public string RoomList { get; set; }
        public string TemplateRemark { get; set; }
        public int Status { get; set; }
        public List<TemplateMapping> ListTemplateMapping { get; set; }
    }
    public class OtherWorkTime
    {
        public int ID { get; set; }
        public string StaffNo { get; set; }
        public int WorkType { get; set; }
        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
        public decimal Hours { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }
    }
    public class RoomCombine
    {
        public int ID { get; set; }
        public string RoomList { get; set; }
        public string Remark { get; set; }
    }
    public class AssessTemplate
    {
        public int ID { get; set; }
        public string Rank { get; set; }
        public int Post { get; set; }
        public string AssessType { get; set; }
        public int Status { get; set; }
        public List<AssessTemplateDetail> ListAssessTemplateDetail { get; set; }
    }
    public class AssessTemplateDetail
    {
        public int DetailID { get; set; }
        public int AssessTemplateID { get; set; }
        public string AssessProjectType { get; set; }
        public string AssessProjectNo { get; set; }
        public string AssessProjectName { get; set; }
        public string Remark { get; set; }
        public decimal Score { get; set; }
    }

    public class StaffAssess
    {
        public int ID { get; set; }
        public string StaffNo { get; set; }
        public int AssessTemplateID { get; set; }
        public string Score { get; set; }
        public DateTime AssessDate { get; set; }
        public DateTime ModifyOn { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public string Name { get; set; }
        public string Rank { get; set; }
        public string AssessType { get; set; }
        public List<AssessTemplateDetail> ListAssessTemplateDetail { get; set; }
    }
    public class ParamOption
    {
        public string ParamCode { get; set; }
        public string ParamName { get; set; }
    }
    public class PostInfo
    {
        public int ID { get; set; }
        public string PostLevel { get; set; }
        public string PostName { get; set; }
        public string Remark { get; set; }
        public decimal Rate { get; set; }
    }

    public class StaffLeave
    {
        public int ID { get; set; }
        public string StaffNo { get; set; }
        public string LeaveType { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreateOn { get; set; }
        public int Status { get; set; }
        public string Remark { get; set; }
    }

    public class StaffConnection
    {
        public int ID { get; set; }
        public string StaffNo { get; set; }
        public string ConnectStaffNo { get; set; }
        public string ConnectGuestID { get; set; }
        public string OtherConnectName { get; set; }
        public string ConnectionType { get; set; }
        public DateTime ModifyOn { get; set; }
        public string Remark { get; set; }
    }

    public class ReasonableParam
    {
        public string ParamType_A { get; set; }
        public string ParamType_B { get; set; }
        public string ParamType_C { get; set; }
        public string ParamType_D { get; set; }
        public string ParamType_E { get; set; }
    }

    public class ExpireParam
    {
        public string HealthCardExpire { get; set; }
        public string ContractExpire { get; set; }
    }

    public class Floor
    {
        public int ID { get; set; }
        public string FloorID { get; set; }
        public string Remark { get; set; }
        public string Manager { get; set; }
        public int Status { get; set; }
    }
    public class GuestConnection
    {
        public int ID { get; set; }
        public string GuestID { get; set; }
        public string ConnectStaffNo { get; set; }
        public string ConnectGuestID { get; set; }
        public string OtherConnectName { get; set; }
        public string ConnectionType { get; set; }
        public DateTime ModifyOn { get; set; }
        public string Remark { get; set; }
    }

    public class EvaluateTemplate
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public List<EvaluateTemplateDetail> ListEvaluateTemplateDetail { get; set; }
    }
    public class EvaluateTemplateDetail
    {
        public int DetailID { get; set; }
        public int EvaluateTemplateID { get; set; }
        public string EvaluateProjectType { get; set; }
        public string EvaluateProjectNo { get; set; }
        public string EvaluateProjectName { get; set; }
        public string Remark { get; set; }
        public decimal Score { get; set; }
    }
    public class GuestEvaluate
    {
        public int ID { get; set; }
        public int GuestID { get; set; }
        public string EvaluateType { get; set; }
        public int EvaluateTemplateID { get; set; }
        public string Score { get; set; }
        public DateTime EvaluateDate { get; set; }
        public DateTime ModifyOn { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public string SuggestNurseLevel { get; set; }
        public string Name { get; set; }
        public string NurseLevel { get; set; }
        public int Age { get; set; }
        public List<EvaluateTemplateDetail> ListEvaluateTemplateDetail { get; set; }
    }

    public class NurseProject
    {
        public int ID { get; set; }
        public string NurseName { get; set; }
        public string Requirement { get; set; }
        public string NurseType { get; set; }
    }

    public class GuestNursePlan
    {
        public int ID { get; set; }
        public int GuestID { get; set; }
        public DateTime BeginDate { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public string Name { get; set; }
        public string NurseLevel { get; set; }
        public int Age { get; set; }
        public List<GuestNursePlanDetail> ListGuestNursePlanDetail { get; set; }
    }
    public class GuestNursePlanDetail
    {
        public int DetailID { get; set; }
        public int GuestNursePlanID { get; set; }
        public int NurseProjectID { get; set; }
        public string Remark { get; set; }
        public string NurseName { get; set; }
        public string NurseType { get; set; }
        public string Requirement { get; set; }
    }

    public class ProductType
    {
        public int ID { get; set; }
        public string ProductTypeNo { get; set; }
        public string ProductTypeName { get; set; }
        public string MainProductType { get; set; }
        public string Remark { get; set; }
    }

    public class Product
    {
        public int ID { get; set; }
        public string ProductNo { get; set; }
        public int ProductTypeID { get; set; }
        public string ProductName { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
    }

    public class ProductServiceObject
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public string NurseLevel { get; set; }
        public string Remark { get; set; }
        public string ObjectRemark { get; set; }
        public string EffectRemark { get; set; }
    }
    public class ProductSkillRequired
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public string SkillRequired { get; set; }
        public string Remark { get; set; }
        public string RequiredRemark { get; set; }
    }
    public class ProductPrice
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public decimal Amount { get; set; }
        public string Remark { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
    }
    public class ServiceReserve
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public int GuestID { get; set; }
        public string Remark { get; set; }
        public DateTime ApplyDate { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
        public int PayStatus { get; set; }
    }
    public class ServiceExecute
    {
        public int ID { get; set; }
        public int ServiceReserveID { get; set; }
        public DateTime ServiceDate { get; set; }
        public string ServiceStaff { get; set; }
        public string Remark { get; set; }
    }
    public class ServiceEvaluation
    {
        public int ID { get; set; }
        public int GuestID { get; set; }
        public int ServiceExecuteID { get; set; }
        public DateTime EvaluateDate { get; set; }
        public string Rater { get; set; }
        public string Remark { get; set; }

    }
    public class ServiceEvaluationInfo
    {
        public int ID { get; set; }
        public int GuestID { get; set; }
        public string GuestName { get; set; }
        public string RoomNo { get; set; }
        public string BedNo { get; set; }
        public int Sex { get; set; }
        public int ServiceExecuteID { get; set; }
        public string StaffName { get; set; }
        public DateTime ServiceDate { get; set; }
        public string ProductName { get; set; }
        public DateTime EvaluateDate { get; set; }
        public string Rater { get; set; }
        public string Remark { get; set; }

    }
    public class ServicePay
    {
        public int ID { get; set; }
        public int ServiceReserveID { get; set; }
        public DateTime PayDate { get; set; }
        public decimal Amount { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }

    }
    public class ActivitySchedual
    {
        public int ID { get; set; }
        public DateTime ActivityDate { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ProductID { get; set; }
        public string Place { get; set; }
        public string ActivityInfo { get; set; }
        public decimal ActivityFee { get; set; }
        public string ActivityManager { get; set; }
        public string ActivityExecuteInfo { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public DateTime ModifyOn { get; set; }
        public DateTime? PayDate { get; set; }
        public int PayStatus { get; set; }
    }
    public class ActivityReserve
    {
        public int ID { get; set; }
        public int ActivitySchedualID { get; set; }
        public int GuestID { get; set; }
        public DateTime ApplyDate { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public DateTime ModifyOn { get; set; }
        public DateTime? PayDate { get; set; }
        public int PayStatus { get; set; }
        public string Participation { get; set; }
    }
}