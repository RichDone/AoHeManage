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

        public string GuestName { get; set; }
        public string RoomNo { get; set; }
        public string BedNo { get; set; }
        public int Sex { get; set; }
        public string NurseLevel { get; set; }
        public int Age { get; set; }
        public string StaffName { get; set; }
    }
    public class StaffEvaluate
    {
        public int StaffEvaluateID { get; set; }
        public string StaffNo { get; set; }
        public int EvaluateType { get; set; }
        public DateTime CreateOn { get; set; }
        public string Remark { get; set; }
        public string Name { get; set; }
        public int Sex { get; set; }
        public string PostName { get; set; }
        public string IDCardNo { get; set; }
        public string StaffOtherNo { get; set; }
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

    public class MaterielType
    {
        private int _id;
        private string _typeno;
        private string _typename;
        private string _remark;
        /// <summary>
        /// auto_increment
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Custom(Exists = true)]
        public string TypeNo
        {
            set { _typeno = value; }
            get { return _typeno; }
        }

        [Custom(Exists = true)]
        public string TypeName
        {
            set { _typename = value; }
            get { return _typename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
    }

    public class Supplier
    {
        private int _id;
        private string _shortname;
        private string _name;
        private string _contactname;
        private string _phone;
        private string _address;
        private string _remark;
        private int _status;
        /// <summary>
        /// auto_increment
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        [Custom(Exists = true)]
        public string ShortName
        {
            set { _shortname = value; }
            get { return _shortname; }
        }
        [Custom(Exists = true)]
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ContactName
        {
            set { _contactname = value; }
            get { return _contactname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Phone
        {
            set { _phone = value; }
            get { return _phone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Address
        {
            set { _address = value; }
            get { return _address; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }
    }

    public class Materiel
    {
        private int _id;
        private int _materieltypeid;
        private string _materielno;
        private int _isconsumable;
        private string _name;
        private int _supplierid;
        private decimal _price;
        private string _unit;
        private DateTime? _begindate;
        private DateTime? _enddate;
        private string _remark;
        private int _status;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int MaterielTypeID
        {
            set { _materieltypeid = value; }
            get { return _materieltypeid; }
        }
        [Custom(Exists = true)]
        public string MaterielNo
        {
            set { _materielno = value; }
            get { return _materielno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int IsConsumable
        {
            set { _isconsumable = value; }
            get { return _isconsumable; }
        }
        [Custom(Exists = true)]
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SupplierID
        {
            set { _supplierid = value; }
            get { return _supplierid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal Price
        {
            set { _price = value; }
            get { return _price; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Unit
        {
            set { _unit = value; }
            get { return _unit; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? BeginDate
        {
            set { _begindate = value; }
            get { return _begindate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? EndDate
        {
            set { _enddate = value; }
            get { return _enddate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }
    }

    public class MaterielApply
    {
        private int _id;
        private string _name;
        private string _unit;
        private int _quantity;
        private DateTime _applydate;
        private string _applydept;
        private string _applypeople;
        private int _status;
        private string _remark;
        /// <summary>
        /// auto_increment
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Custom(Exists = true)]
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Unit
        {
            set { _unit = value; }
            get { return _unit; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Quantity
        {
            set { _quantity = value; }
            get { return _quantity; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ApplyDate
        {
            set { _applydate = value; }
            get { return _applydate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ApplyDept
        {
            set { _applydept = value; }
            get { return _applydept; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ApplyPeople
        {
            set { _applypeople = value; }
            get { return _applypeople; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
    }

    public class PurchaseApply
    {
        private int _id;
        private string _applydept;
        private string _applypeople;
        private DateTime _applydate;
        private decimal _totalprice;
        private int _status;
        private string _remark;

        public List<PurchaseApplyDetail> ListPurchaseApplyDetail { get; set; }
        /// <summary>
        /// auto_increment
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ApplyDept
        {
            set { _applydept = value; }
            get { return _applydept; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ApplyPeople
        {
            set { _applypeople = value; }
            get { return _applypeople; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ApplyDate
        {
            set { _applydate = value; }
            get { return _applydate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal TotalPrice
        {
            set { _totalprice = value; }
            get { return _totalprice; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
    }

    public class PurchaseApplyDetail
    {
        private int _id;
        private int _purchaseapplyid;
        private int _materielid;
        private string _unit;
        private decimal _price;
        private int _requirequantity;
        private decimal _requireprice;
        private int _purchasequantity;
        private decimal _purchaseprice;
        private int _storedquantity;
        private int _storingquantity;
        private string _remark;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int PurchaseApplyID
        {
            set { _purchaseapplyid = value; }
            get { return _purchaseapplyid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int MaterielID
        {
            set { _materielid = value; }
            get { return _materielid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Unit
        {
            set { _unit = value; }
            get { return _unit; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal Price
        {
            set { _price = value; }
            get { return _price; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int RequireQuantity
        {
            set { _requirequantity = value; }
            get { return _requirequantity; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal RequirePrice
        {
            set { _requireprice = value; }
            get { return _requireprice; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int PurchaseQuantity
        {
            set { _purchasequantity = value; }
            get { return _purchasequantity; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal PurchasePrice
        {
            set { _purchaseprice = value; }
            get { return _purchaseprice; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int StoredQuantity
        {
            set { _storedquantity = value; }
            get { return _storedquantity; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int StoringQuantity
        {
            set { _storingquantity = value; }
            get { return _storingquantity; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }

    }

    public class StoreManage
    {
        private int _id;
        private int _purchaseapplyid;
        private int _materielid;
        private int _storequantity;
        private decimal _storeprice;
        private string _storepeople;
        private DateTime _storedate;
        private int _status;
        private string _remark;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int PurchaseApplyID
        {
            set { _purchaseapplyid = value; }
            get { return _purchaseapplyid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int MaterielID
        {
            set { _materielid = value; }
            get { return _materielid; }
        }
        public string FixedAssetNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int StoreQuantity
        {
            set { _storequantity = value; }
            get { return _storequantity; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal StorePrice
        {
            set { _storeprice = value; }
            get { return _storeprice; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string StorePeople
        {
            set { _storepeople = value; }
            get { return _storepeople; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime StoreDate
        {
            set { _storedate = value; }
            get { return _storedate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        public string MaterielName { get; set; }
        public decimal Price { get; set; }
        public int StoringQuantity { get; set; }
    }

    public class MaterielStock
    {
        private int _materielid;
        private int _totalstorequantity;
        private int _usequantity;
        private int _losequantity;
        private int _stockquantity;
        private int _borrowquantity;
        /// <summary>
        /// 
        /// </summary>
        public int MaterielID
        {
            set { _materielid = value; }
            get { return _materielid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int TotalStoreQuantity
        {
            set { _totalstorequantity = value; }
            get { return _totalstorequantity; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int UseQuantity
        {
            set { _usequantity = value; }
            get { return _usequantity; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int LoseQuantity
        {
            set { _losequantity = value; }
            get { return _losequantity; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int StockQuantity
        {
            set { _stockquantity = value; }
            get { return _stockquantity; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int BorrowQuantity
        {
            set { _borrowquantity = value; }
            get { return _borrowquantity; }
        }

    }

    public class FixedAssetStock
    {
        private int _id;
        private int _materielstockid;
        private string _fixedassetno;
        private string _borrower;
        private DateTime _borrowdate;
        private DateTime _returndate;
        private int _status;
        private string _remark;
        /// <summary>
        /// auto_increment
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int MaterielStockID
        {
            set { _materielstockid = value; }
            get { return _materielstockid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string FixedAssetNo
        {
            set { _fixedassetno = value; }
            get { return _fixedassetno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Borrower
        {
            set { _borrower = value; }
            get { return _borrower; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime BorrowDate
        {
            set { _borrowdate = value; }
            get { return _borrowdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ReturnDate
        {
            set { _returndate = value; }
            get { return _returndate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }

    }

    public class UseManage
    {
        public int ID { get; set; }
        public int MaterielID { get; set; }
        public int UseQuantity { get; set; }
        public string UsePeople { get; set; }
        public string Remark { get; set; }
        public DateTime UseDate { get; set; }

        public string MaterielName { get; set; }
        public int StockQuantity { get; set; }
    }
}