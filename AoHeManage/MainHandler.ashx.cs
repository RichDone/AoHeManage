using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using AoHeManage.Dal;
using AoHeManage.Common;
using AoHeManage.Model;
using System.IO;
using System.Net;
using System.Web.UI;

namespace AoHeManage
{
    /// <summary>
    /// MainHandler 的摘要说明
    /// </summary>
    public class MainHandler : IHttpHandler
    {
        //test svn
        AoHeDal dal = new AoHeDal();
        AoHeDalGlobal dalGlobal = new AoHeDalGlobal();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ClearContent();
            context.Response.Charset = "utf-8";
            context.Response.ContentType = "text/plain";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            string result = "";
            string action = context.Request.Params["action"];
            string currentpage = context.Request.Params["currentpage"];
            string pagesize = context.Request.Params["pagesize"];
            string sortfield = context.Request.Params["sortfield"];
            string sorttype = context.Request.Params["sorttype"];
            //sortfield = sortfield.Replace("sort_", "");

            #region 获取意外事故列表
            if (action == "getRecordPage_AccidentInfo")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                string beginDate = context.Request.Params["beginDate"];
                string endDate = context.Request.Params["endDate"];
                string name = context.Request.Params["name"];
                string accidentType = context.Request.Params["accidentType"];
                StringBuilder strWhere = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    strWhere.AppendFormat(" and b.Name like '%{0}%' ", name);
                }
                if (!string.IsNullOrWhiteSpace(accidentType) && accidentType.ToUpper() != "NULL")
                {
                    strWhere.AppendFormat(" and a.AccidentType = '{0}' ", accidentType);
                }
                if (!string.IsNullOrWhiteSpace(beginDate))
                {
                    strWhere.AppendFormat(" and CreateOn>= '{0}' ", beginDate);
                }
                if (!string.IsNullOrWhiteSpace(endDate))
                {
                    endDate = Convert.ToDateTime(endDate).AddDays(1).ToString("yyyy-MM-dd");
                    strWhere.AppendFormat(" and CreateOn< '{0}' ", endDate);
                }
                result = getRecordPage_AccidentInfo(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 根据姓名查找客人
            if (action == "FindGuest")
            {
                string chooseGuest = context.Request.Params["chooseGuest"];
                var listGuest = dal.GetGuestInfoByName(chooseGuest);
                result = CommTools.ObjectToJson(listGuest);
            }
            #endregion

            #region 绑定参数码表下拉框
            if (action == "InitSelect_ParamOption")
            {
                DataSet ds = new DataSet();
                var paramType = context.Request.Params["paramType"];
                ds = dal.GetParamOption(paramType);
                StringBuilder strOpt = new StringBuilder();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    strOpt.Append("<option value=''>请选择</option>");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        strOpt.AppendFormat("<option value='{0}'>{1}</option>", ds.Tables[0].Rows[i]["ParamOptionCode"], ds.Tables[0].Rows[i]["ParamOptionName"]);
                    }
                }
                result = strOpt.ToString();
            }
            #endregion

            #region 绑定职位下拉框
            if (action == "InitSelect_PostLevel")
            {
                DataSet ds = new DataSet();
                ds = dal.GetPostInfo();
                StringBuilder strOpt = new StringBuilder();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    strOpt.Append("<option value=''>请选择</option>");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        strOpt.AppendFormat("<option value='{0}'>{1}</option>", ds.Tables[0].Rows[i]["PostLevel"], ds.Tables[0].Rows[i]["PostName"]);
                    }
                }
                result = strOpt.ToString();
            }
            #endregion

            #region 绑定楼层下拉框
            if (action == "InitSelect_Floor")
            {
                DataSet ds = new DataSet();
                ds = dal.GetFloorInfo();
                StringBuilder strOpt = new StringBuilder();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    strOpt.Append("<option value=''>请选择</option>");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        strOpt.AppendFormat("<option value='{0}'>{1}</option>", ds.Tables[0].Rows[i]["FloorID"], ds.Tables[0].Rows[i]["FloorID"]);
                    }
                }
                result = strOpt.ToString();
            }
            #endregion

            #region 绑定房间下拉框
            if (action == "InitSelect_Room")
            {
                DataSet ds = new DataSet();
                ds = dal.GetRoomInfo();
                StringBuilder strOpt = new StringBuilder();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    strOpt.Append("<option value=''>请选择</option>");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        strOpt.AppendFormat("<option value='{0}'>{1}</option>", ds.Tables[0].Rows[i]["RoomNo"], ds.Tables[0].Rows[i]["RoomNo"]);
                    }
                }
                result = strOpt.ToString();
            }
            #endregion

            #region 绑定员工下拉框
            if (action == "InitSelect_StaffInfo")
            {
                DataSet ds = new DataSet();
                ds = dal.GetStaffInfo();
                StringBuilder strOpt = new StringBuilder();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    strOpt.Append("<option value=''>请选择</option>");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        strOpt.AppendFormat("<option value='{0}'>{1}</option>", ds.Tables[0].Rows[i]["StaffNo"], ds.Tables[0].Rows[i]["Name"]);
                    }
                }
                result = strOpt.ToString();
            }
            #endregion

            #region 绑定带教师傅员工下拉框
            if (action == "InitSelect_MasterStaffInfo")
            {
                DataSet ds = new DataSet();
                ds = dal.GetStaffInfo();
                StringBuilder strOpt = new StringBuilder();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    strOpt.Append("<option value=''>请选择</option>");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        var isMaster = ds.Tables[0].Rows[i]["IsMaster"].ToString();
                        if (isMaster == "1")
                        {
                            strOpt.AppendFormat("<option value='{0}'>{1}</option>", ds.Tables[0].Rows[i]["StaffNo"], ds.Tables[0].Rows[i]["Name"]);
                        }
                    }
                }
                result = strOpt.ToString();
            }
            #endregion

            #region 绑定客人下拉框
            if (action == "InitSelect_GuestInfo")
            {
                DataSet ds = new DataSet();
                ds = dal.GetGuestInfo();
                StringBuilder strOpt = new StringBuilder();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    strOpt.Append("<option value=''>请选择</option>");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        strOpt.AppendFormat("<option value='{0}' floorID='{2}' roomNo='{3}'>{1}</option>",
                            ds.Tables[0].Rows[i]["ID"], ds.Tables[0].Rows[i]["Name"],
                            ds.Tables[0].Rows[i]["FloorID"], ds.Tables[0].Rows[i]["RoomNo"]);
                    }
                }
                result = strOpt.ToString();
            }
            #endregion

            #region 添加意外事故
            if (action == "AddAccident")
            {
                var addOrUpdate = context.Request.Params["addOrUpdate"];
                var accidentID = context.Request.Params["accidentID"];
                var guestID = context.Request.Params["guestID"];
                var createDate = context.Request.Params["createDate"];
                var createTime = context.Request.Params["createTime"];
                var place = context.Request.Params["place"];
                var condition = context.Request.Params["condition"];
                var remark = context.Request.Params["remark"];
                var accidentType = context.Request.Params["accidentType"];
                var relatedPerson = context.Request.Params["relatedPerson"];
                List<AccidentRelatedPerson> list = new List<AccidentRelatedPerson>();
                if (!string.IsNullOrWhiteSpace(relatedPerson))
                {
                    list = CommTools.JsonToObject(relatedPerson, typeof(List<AccidentRelatedPerson>)) as List<AccidentRelatedPerson>;
                }
                Accident model = new Accident();
                model.ListAccidentRelatedPerson = list;
                model.GuestID = Convert.ToInt16(guestID);
                model.CreateOn = Convert.ToDateTime(createDate + " " + createTime);
                model.AccidentType = accidentType;
                model.Place = place;
                model.Condition = condition;
                model.Remark = remark;
                string excuteResult = string.Empty;
                if (addOrUpdate == "add")
                {
                    excuteResult = dal.AddAccident(model).ToString();
                }
                else
                {
                    model.AccidentID = Convert.ToInt16(accidentID);
                    excuteResult = dal.UpdateAccident(model).ToString();
                }

                result = excuteResult.ToString();
            }
            #endregion

            #region 根据ID获取意外事故信息
            if (action == "GetAccidentModelByID")
            {
                var accidentID = context.Request.Params["accidentID"];
                var model = dal.GetAccidentModelByID(accidentID);
                result = CommTools.ObjectToJson(model);
            }
            #endregion

            #region 删除意外事件
            if (action == "DeleteAccident")
            {
                var ID = context.Request.Params["ID"];
                var deleteResult = dal.DeleteAccident(Convert.ToInt16(ID));
                result = deleteResult.ToString();
            }
            #endregion

            #region 到期时间设置
            if (action == "SaveExpireParam")
            {
                var healthCardExpire = context.Request.Params["healthCardExpire"];
                var contractExpire = context.Request.Params["contractExpire"];
                ExpireParam model = new ExpireParam();
                model.HealthCardExpire = healthCardExpire;
                model.ContractExpire = contractExpire;
                var saveResult = dal.SaveExpireParam(model);
                result = saveResult.ToString();
            }
            #endregion

            #region 获取到期时间
            if (action == "GetExpireParam")
            {
                var model = dal.GetExpireParam();
                result = CommTools.ObjectToJson(model);
            }
            #endregion

            #region 获取员工信息列表
            if (action == "getRecordPage_StaffInfo")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string queryName = context.Request.Params["queryName"];
                string queryStaffOtherNo = context.Request.Params["queryStaffOtherNo"];
                string querySex = context.Request.Params["querySex"];
                string queryPostLevel = context.Request.Params["queryPostLevel"];
                string querySkillLevel = context.Request.Params["querySkillLevel"];
                string queryStatus = context.Request.Params["queryStatus"];
                string queryExpire = context.Request.Params["queryExpire"];

                if (!string.IsNullOrWhiteSpace(queryName))
                {
                    strWhere.AppendFormat(" and a.Name like '%{0}%' ", queryName);
                }
                if (!string.IsNullOrWhiteSpace(queryStaffOtherNo))
                {
                    strWhere.AppendFormat(" and a.StaffOtherNo = '{0}' ", queryStaffOtherNo);
                }
                if (!string.IsNullOrWhiteSpace(querySex))
                {
                    strWhere.AppendFormat(" and a.Sex = '{0}' ", querySex);
                }
                if (!string.IsNullOrWhiteSpace(queryPostLevel) && queryPostLevel != "null")
                {
                    strWhere.AppendFormat(" and a.PostLevel = '{0}' ", queryPostLevel);
                }
                if (!string.IsNullOrWhiteSpace(querySkillLevel) && querySkillLevel != "null")
                {
                    strWhere.AppendFormat(" and a.SkillLevel = '{0}' ", querySkillLevel);
                }
                if (!string.IsNullOrWhiteSpace(queryStatus))
                {
                    strWhere.AppendFormat(" and a.Status = '{0}' ", queryStatus);
                }
                if (!string.IsNullOrWhiteSpace(queryExpire) && queryExpire == "contractDate")
                {
                    strWhere.Clear();
                    var contractExpire = context.Request.Params["contractExpire"];
                    strWhere.AppendFormat(" and (a.ContractDate is not null and a.ContractDate<ADDDATE(NOW(),{0})) ", contractExpire);
                }
                if (!string.IsNullOrWhiteSpace(queryExpire) && queryExpire == "healthCardDate")
                {
                    strWhere.Clear();
                    var healthCardExpire = context.Request.Params["healthCardExpire"];
                    strWhere.AppendFormat(" and (a.HealthCardDate is not null and a.HealthCardDate<ADDDATE(NOW(),{0})) ", healthCardExpire);
                }
                result = getRecordPage_StaffInfo(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 保存员工
            if (action == "SaveStaff")
            {
                var name = context.Request.Params["name"];
                var sex = context.Request.Params["sex"];
                var IDCardNo = context.Request.Params["IDCardNo"];
                var staffNo = context.Request.Params["staffNo"];
                var staffOtherNo = context.Request.Params["staffOtherNo"];
                var postLevel = context.Request.Params["postLevel"];
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var status = context.Request.Params["status"];
                var rank = context.Request.Params["rank"];
                var leaveDate = context.Request.Params["leaveDate"];
                var masterStaffNo = context.Request.Params["masterStaffNo"];
                var hireDate = context.Request.Params["hireDate"];
                var regularDate = context.Request.Params["regularDate"];

                var education = context.Request.Params["education"];
                var skillLevel = context.Request.Params["skillLevel"];
                var phone = context.Request.Params["phone"];
                var emergencyContactName = context.Request.Params["emergencyContactName"];
                var emergencyContactPhone = context.Request.Params["emergencyContactPhone"];
                var healthCardDate = context.Request.Params["healthCardDate"];
                var nurseCardLevel = context.Request.Params["nurseCardLevel"];
                var nurseCardDate = context.Request.Params["nurseCardDate"];
                var contractDate = context.Request.Params["contractDate"];
                var noCrimeProof = context.Request.Params["noCrimeProof"];
                var oldPost = context.Request.Params["oldPost"];
                var oldRank = context.Request.Params["oldRank"];

                var isMaster = context.Request.Params["isMaster"];
                var householdType = context.Request.Params["householdType"];
                var nativePlace = context.Request.Params["nativePlace"];
                var recruitmentChannel = context.Request.Params["recruitmentChannel"];
                var contracType = context.Request.Params["contracType"];
                //保存图片，和图片地址
                string fileName = string.Empty;
                if (!string.IsNullOrWhiteSpace(noCrimeProof))
                {
                    string path = HttpContext.Current.Server.MapPath("/Pictures");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    if (noCrimeProof.LastIndexOf("\\") > -1)
                    {
                        fileName = noCrimeProof.Substring(noCrimeProof.LastIndexOf("\\") + 1, noCrimeProof.Length - noCrimeProof.LastIndexOf("\\") - 1);
                    }
                    //File.Copy(noCrimeProof, path + "\\" + fileName, true);
                    var getFile = context.Request.Files[0];
                    getFile.SaveAs(path + "/" + fileName);
                }
                Staff model = new Staff();
                model.Name = name;
                model.Sex = Convert.ToInt16(sex);
                model.IDCardNo = IDCardNo;
                model.StaffOtherNo = staffOtherNo;
                model.PostLevel = Convert.ToInt16(postLevel);
                model.Status = Convert.ToInt16(status);
                model.MasterStaffNo = masterStaffNo;
                model.Rank = rank;

                model.Education = education;
                model.SkillLevel = skillLevel;
                model.Phone = phone;
                model.EmergencyContactName = emergencyContactName;
                model.EmergencyContactPhone = emergencyContactPhone;

                model.IsMaster = Convert.ToInt16(isMaster);
                model.HouseholdType = householdType;
                model.NativePlace = nativePlace;
                model.RecruitmentChannel = recruitmentChannel;
                model.ContracType = contracType;

                if (!string.IsNullOrWhiteSpace(healthCardDate))
                {
                    model.HealthCardDate = Convert.ToDateTime(healthCardDate);
                }
                model.NurseCardLevel = nurseCardLevel;
                if (!string.IsNullOrWhiteSpace(nurseCardDate))
                {
                    model.NurseCardDate = Convert.ToDateTime(nurseCardDate);
                }
                if (!string.IsNullOrWhiteSpace(contractDate))
                {
                    model.ContractDate = Convert.ToDateTime(contractDate);
                }
                model.NoCrimeProof = fileName;
                if (!string.IsNullOrWhiteSpace(leaveDate))
                {
                    model.LeaveDate = Convert.ToDateTime(leaveDate);
                }
                if (!string.IsNullOrWhiteSpace(hireDate))
                {
                    model.HireDate = Convert.ToDateTime(hireDate);
                }
                if (!string.IsNullOrWhiteSpace(regularDate))
                {
                    model.RegularDate = Convert.ToDateTime(regularDate);
                }
                int excuteResult = 0;
                if (saveflag == "add")
                {
                    excuteResult = dal.AddStaff(model);
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    model.StaffNo = staffNo;
                    model.OldPost = oldPost;
                    model.OldRank = oldRank;
                    excuteResult = dal.UpdateStaff(model);
                }
                result = excuteResult.ToString();
                //当使用一般处理程序向前端注册js时，一定要将ContentType改为text/html
                context.Response.ContentType = "text/html";
                context.Response.Write("<script type=\"text/javascript\">alert('保存成功！');window.parent.ChangeIframe('StaffManage');</script>");
                context.Response.Flush();
                context.Response.End();
            }
            #endregion

            #region 根据ID获取员工信息
            if (action == "GetStaffInfoByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dal.GetStaffInfoByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 根据ID获取客人信息
            if (action == "GetGuestInfoByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dal.GetGuestInfoByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 保存客人
            if (action == "SaveGuest")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var name = context.Request.Params["name"];
                var IDCardNo = context.Request.Params["IDCardNo"];
                var birthDay = context.Request.Params["birthDay"];
                var tryAdmissionDate = context.Request.Params["tryAdmissionDate"];
                var floorID = context.Request.Params["floorID"];
                var sex = context.Request.Params["sex"];
                var roomNo = context.Request.Params["roomNo"];
                var bedNo = context.Request.Params["bedNo"];
                var nurseLevel = context.Request.Params["nurseLevel"];
                var admissionDate = context.Request.Params["admissionDate"];
                var changeLevelDate = context.Request.Params["changeLevelDate"];
                var status = context.Request.Params["status"];
                var leaveDate = context.Request.Params["leaveDate"];
                var remark = context.Request.Params["remark"];
                Guest model = new Guest();
                model.Name = name;
                model.Sex = Convert.ToInt16(sex);
                if (!string.IsNullOrWhiteSpace(birthDay))
                {
                    model.Age = DateTime.Now.Date.Year - Convert.ToDateTime(birthDay).Year;
                }
                model.FloorID = floorID;
                model.RoomNo = roomNo;
                model.BedNo = bedNo;
                model.NurseLevel = nurseLevel;
                model.IDCardNo = IDCardNo;
                model.AdmissionDate = Convert.ToDateTime(admissionDate);
                if (!string.IsNullOrWhiteSpace(birthDay))
                {
                    model.BirthDay = Convert.ToDateTime(birthDay);
                }
                if (!string.IsNullOrWhiteSpace(changeLevelDate))
                {
                    model.ChangeLevelDate = Convert.ToDateTime(changeLevelDate);
                }
                if (!string.IsNullOrWhiteSpace(tryAdmissionDate))
                {
                    model.TryAdmissionDate = Convert.ToDateTime(tryAdmissionDate);
                }
                if (!string.IsNullOrWhiteSpace(leaveDate))
                {
                    model.LeaveDate = Convert.ToDateTime(leaveDate);
                }
                model.Status = Convert.ToInt16(status);
                model.Remark = remark;
                int excuteResult = 0;
                if (saveflag == "add")
                {
                    excuteResult = dal.AddGuest(model);
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    excuteResult = dal.UpdateGuest(model);
                }

                result = excuteResult.ToString();
            }
            #endregion

            #region 获取客人信息列表
            if (action == "getRecordPage_GuestInfo")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string queryName = context.Request.Params["queryName"];
                string queryStatus = context.Request.Params["queryStatus"];
                string queryFloorID = context.Request.Params["queryFloorID"];
                string queryRoomNo = context.Request.Params["queryRoomNo"];
                string queryNurseLevel = context.Request.Params["queryNurseLevel"];
                string queryAge1 = context.Request.Params["queryAge1"];
                string queryAge2 = context.Request.Params["queryAge2"];
                string queryAdmissionDate1 = context.Request.Params["queryAdmissionDate1"];
                string queryAdmissionDate2 = context.Request.Params["queryAdmissionDate2"];
                if (!string.IsNullOrWhiteSpace(queryName))
                {
                    strWhere.AppendFormat(" and Name like '%{0}%' ", queryName);
                }
                if (!string.IsNullOrWhiteSpace(queryStatus))
                {
                    if (queryStatus== "ShowDefault")
                    {
                        strWhere.Append(" and `Status` in (0,1) ");
                    }
                    else
                    {
                        strWhere.AppendFormat(" and `Status` = '{0}' ", queryStatus);
                    }
                }
                if (!string.IsNullOrWhiteSpace(queryFloorID) && queryFloorID != "null")
                {
                    strWhere.AppendFormat(" and FloorID = '{0}' ", queryFloorID);
                }
                if (!string.IsNullOrWhiteSpace(queryRoomNo) && queryRoomNo != "null")
                {
                    strWhere.AppendFormat(" and RoomNo = '{0}' ", queryRoomNo);
                }
                if (!string.IsNullOrWhiteSpace(queryNurseLevel))
                {
                    strWhere.AppendFormat(" and NurseLevel = '{0}' ", queryNurseLevel);
                }
                if (!string.IsNullOrWhiteSpace(queryAge1))
                {
                    strWhere.AppendFormat(" and Age>= '{0}' ", queryAge1);
                }
                if (!string.IsNullOrWhiteSpace(queryAge2))
                {
                    strWhere.AppendFormat(" and Age<= '{0}' ", queryAge2);
                }
                if (!string.IsNullOrWhiteSpace(queryAdmissionDate1))
                {
                    strWhere.AppendFormat(" and AdmissionDate>= '{0}' ", queryAdmissionDate1);
                }
                if (!string.IsNullOrWhiteSpace(queryAdmissionDate2))
                {
                    strWhere.AppendFormat(" and AdmissionDate<= '{0}' ", queryAdmissionDate2);
                }
                result = getRecordPage_GuestInfo(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 添加日常记录
            if (action == "AddDailyRecord")
            {
                var saveflag = context.Request.Params["saveflag"];
                var dailyRecordID = context.Request.Params["dailyRecordID"];

                var guestID = context.Request.Params["guestID"];
                var dailyRecordType = context.Request.Params["dailyRecordType"];
                var createOn = context.Request.Params["createOn"];
                var remark = context.Request.Params["remark"];
                var staffNo = context.Request.Params["staffNo"];
                DailyRecord model = new DailyRecord();
                model.GuestID = Convert.ToInt16(guestID);
                model.CreateOn = Convert.ToDateTime(createOn);
                model.DailyRecordType = dailyRecordType;
                model.Remark = remark;
                model.ReportPerson = staffNo;

                int excuteResult = 0;
                if (saveflag == "add")
                {
                    excuteResult = dal.AddDailyRecord(model);
                }
                if (saveflag == "edit")
                {
                    model.DailyRecordID = Convert.ToInt16(dailyRecordID);
                    excuteResult = dal.UpdateDailyRecord(model);
                }
                result = excuteResult.ToString();


                //var insertResult = dal.AddDailyRecord(model);
                //result = insertResult.ToString();
            }
            #endregion

            #region 获取日常记录列表
            if (action == "getRecordPage_DailyRecordInfo")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                string beginDate = context.Request.Params["beginDate"];
                string endDate = context.Request.Params["endDate"];
                string name = context.Request.Params["name"];
                string keyword = context.Request.Params["keyword"];
                StringBuilder strWhere = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    strWhere.AppendFormat(" and b.Name like '%{0}%' ", name);
                }
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    strWhere.AppendFormat(" and a.Remark like '%{0}%' ", keyword);
                }
                if (!string.IsNullOrWhiteSpace(beginDate))
                {
                    strWhere.AppendFormat(" and CreateOn>= '{0}' ", beginDate);
                }
                if (!string.IsNullOrWhiteSpace(endDate))
                {
                    endDate = Convert.ToDateTime(endDate).AddDays(1).ToString("yyyy-MM-dd");
                    strWhere.AppendFormat(" and CreateOn< '{0}' ", endDate);
                }
                result = getRecordPage_DailyRecordInfo(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 删除日常记录
            if (action == "DeleteDailyRecord")
            {
                var ID = context.Request.Params["ID"];
                var deleteResult = dal.DeleteDailyRecord(Convert.ToInt16(ID));
                result = deleteResult.ToString();
            }
            #endregion

            #region 根据ID获取活动预约
            if (action == "GetDailyRecordByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dal.GetDailyRecordByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 添加员工考评记录
            if (action == "AddStaffEvaluate")
            {
                var saveflag = context.Request.Params["saveflag"];
                var staffEvaluateID = context.Request.Params["staffEvaluateID"];

                var staffNo = context.Request.Params["staffNo"];
                var evaluateType = context.Request.Params["evaluateType"];
                var createOn = context.Request.Params["createOn"];
                var remark = context.Request.Params["remark"];
                StaffEvaluate model = new StaffEvaluate();
                model.StaffNo = staffNo;
                model.CreateOn = Convert.ToDateTime(createOn);
                model.EvaluateType = Convert.ToInt16(evaluateType);
                model.Remark = remark;
                //have a try for git ignore
                int excuteResult = 0;
                if (saveflag == "add")
                {
                    excuteResult = dal.AddStaffEvaluate(model);
                }
                if (saveflag == "edit")
                {
                    model.StaffEvaluateID = Convert.ToInt16(staffEvaluateID);
                    excuteResult = dal.UpdateStaffEvaluate(model);
                }
                result = excuteResult.ToString();
            }
            #endregion

            #region 获取员工考评记录列表
            if (action == "getRecordPage_StaffEvaluateInfo")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                string beginDate = context.Request.Params["beginDate"];
                string endDate = context.Request.Params["endDate"];
                string name = context.Request.Params["name"];
                StringBuilder strWhere = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    strWhere.AppendFormat(" and b.Name like '%{0}%' ", name);
                }
                if (!string.IsNullOrWhiteSpace(beginDate))
                {
                    strWhere.AppendFormat(" and CreateOn>= '{0}' ", beginDate);
                }
                if (!string.IsNullOrWhiteSpace(endDate))
                {
                    endDate = Convert.ToDateTime(endDate).AddDays(1).ToString("yyyy-MM-dd");
                    strWhere.AppendFormat(" and CreateOn< '{0}' ", endDate);
                }
                result = getRecordPage_StaffEvaluateInfo(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 删除员工考评
            if (action == "DeleteStaffEvaluate")
            {
                var ID = context.Request.Params["ID"];
                var deleteResult = dal.DeleteStaffEvaluate(Convert.ToInt16(ID));
                result = deleteResult.ToString();
            }
            #endregion

            #region 根据ID获取员工考评
            if (action == "GetStaffEvaluateByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dal.GetStaffEvaluateByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 根据姓名查找员工
            if (action == "FindStaff")
            {
                string chooseStaff = context.Request.Params["chooseStaff"];
                var listStaff = dal.GetStaffInfoByName(chooseStaff);
                result = CommTools.ObjectToJson(listStaff);
            }
            #endregion

            #region 获取意外事件跟踪信息
            if (action == "getRecordPage_AccidentFollow")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                var accidentID = context.Request.Params["accidentID"];
                StringBuilder strWhere = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(accidentID))
                {
                    strWhere.AppendFormat(" and AccidentID = '{0}' ", accidentID);
                }
                result = getRecordPage_AccidentFollow(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 添加意外事件跟踪信息
            if (action == "AddAccidentFollow")
            {
                var accidentID = context.Request.Params["accidentID"];
                var followTime = context.Request.Params["followTime"];
                var remark = context.Request.Params["remark"];
                AccidentFollow model = new AccidentFollow();
                model.AccidentID = Convert.ToInt16(accidentID);
                model.FollowTime = Convert.ToDateTime(followTime);
                model.Remark = remark;
                var insertResult = dal.AddAccidentFollow(model);
                result = insertResult.ToString();
            }
            #endregion

            #region 获取意外事件详情
            if (action == "getAccidentInfoByWhere")
            {
                string accidentID = context.Request.Params["accidentID"];
                StringBuilder strWhere = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(accidentID))
                {
                    strWhere.AppendFormat(" and a.AccidentID = '{0}' ", accidentID);
                }
                result = getAccidentInfoHtml(strWhere.ToString());
                strWhere = null;
            }
            #endregion

            #region 意外事故统计
            if (action == "getRecordPage_AccidentStats")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                string beginDate = context.Request.Params["beginDate"];
                string endDate = context.Request.Params["endDate"];
                StringBuilder strWhere = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(beginDate))
                {
                    strWhere.AppendFormat(" and CreateOn>= '{0}' ", beginDate);
                }
                if (!string.IsNullOrWhiteSpace(endDate))
                {
                    endDate = Convert.ToDateTime(endDate).AddDays(1).ToString("yyyy-MM-dd");
                    strWhere.AppendFormat(" and CreateOn< '{0}' ", endDate);
                }
                result = getRecordPage_AccidentStats(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 日常记录统计
            if (action == "getRecordPage_DailyRecordStats")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                string beginDate = context.Request.Params["beginDate"];
                string endDate = context.Request.Params["endDate"];
                string name = context.Request.Params["name"];
                string dailyRecordType = context.Request.Params["dailyRecordType"];
                StringBuilder strWhere = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    strWhere.AppendFormat(" and c.Name like '%{0}%' ", name);
                }
                if (!string.IsNullOrWhiteSpace(dailyRecordType) && dailyRecordType != "null")
                {
                    strWhere.AppendFormat(" and a.DailyRecordType = '{0}' ", dailyRecordType);
                }
                if (!string.IsNullOrWhiteSpace(beginDate))
                {
                    strWhere.AppendFormat(" and CreateOn>= '{0}' ", beginDate);
                }
                if (!string.IsNullOrWhiteSpace(endDate))
                {
                    endDate = Convert.ToDateTime(endDate).AddDays(1).ToString("yyyy-MM-dd");
                    strWhere.AppendFormat(" and CreateOn< '{0}' ", endDate);
                }
                result = getRecordPage_DailyRecordStats(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 排班列表
            if (action == "getRecordPage_Schedual")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                string beginDate = context.Request.Params["beginDate"];
                string endDate = context.Request.Params["endDate"];
                string name = context.Request.Params["name"];
                string accidentType = context.Request.Params["accidentType"];
                StringBuilder strWhere = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    strWhere.AppendFormat(" and b.Name like '%{0}%' ", name);
                }
                if (!string.IsNullOrWhiteSpace(accidentType) && accidentType.ToUpper() != "NULL")
                {
                    strWhere.AppendFormat(" and a.AccidentType = '{0}' ", accidentType);
                }
                if (!string.IsNullOrWhiteSpace(beginDate))
                {
                    strWhere.AppendFormat(" and CreateOn>= '{0}' ", beginDate);
                }
                if (!string.IsNullOrWhiteSpace(endDate))
                {
                    endDate = Convert.ToDateTime(endDate).AddDays(1).ToString("yyyy-MM-dd");
                    strWhere.AppendFormat(" and CreateOn< '{0}' ", endDate);
                }
                result = getRecordPage_AccidentInfo(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 获取当前在住房间
            if (action == "GetAllRoom")
            {
                DataSet ds = new DataSet();
                ds = dal.GetAllRoom();
                List<Room> listRoom = new List<Room>();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Room model = new Room();
                        model.RoomNo = ds.Tables[0].Rows[i]["RoomNo"].ToString();
                        model.FloorID = ds.Tables[0].Rows[i]["FloorID"].ToString();
                        model.PeopleNum = Convert.ToInt16(ds.Tables[0].Rows[i]["PeopleNum"]);
                        listRoom.Add(model);
                    }
                }
                context.Response.Write(CommTools.ObjectToJson(listRoom));
                context.Response.Flush();
                context.Response.End();
            }
            #endregion

            #region 绑定护理员下拉框
            if (action == "GetStaffInfoByLevel")
            {
                DataSet ds = new DataSet();
                ds = dal.GetStaffInfoByLevel("");//暂时不限制只有护理员
                StringBuilder strOpt = new StringBuilder();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    strOpt.Append("<option value=''>请选择</option>");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        strOpt.AppendFormat("<option value='{0}' masterStaffNo='{2}' status='{3}'>{1}</option>", ds.Tables[0].Rows[i]["StaffNo"], ds.Tables[0].Rows[i]["Name"], ds.Tables[0].Rows[i]["MasterStaffNo"], ds.Tables[0].Rows[i]["Status"]);
                    }
                }
                result = strOpt.ToString();
            }
            #endregion

            #region 绑定排班查询护理员下拉框
            if (action == "GetSchedualStaffInfo")
            {
                DataSet ds = new DataSet();
                ds = dal.GetStaffInfoByLevel("");//暂时不限制只有护理员
                StringBuilder strOpt = new StringBuilder();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    strOpt.Append("<option value=''>所有人</option>");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        strOpt.AppendFormat("<option value='{0}' masterStaffNo='{2}' status='{3}'>{1}</option>", ds.Tables[0].Rows[i]["StaffNo"], ds.Tables[0].Rows[i]["Name"], ds.Tables[0].Rows[i]["MasterStaffNo"], ds.Tables[0].Rows[i]["Status"]);
                    }
                }
                result = strOpt.ToString();
            }
            #endregion

            #region 根据排班规则获取界面
            if (action == "GetSchedualTemp")
            {
                var rule = context.Request.Params["rule"];
                var isCustom = context.Request.Params["isCustom"];
                string strChooseMonth = context.Request.Params["chooseMonth"];
                string strRoomCombine = context.Request.Params["roomCombine"];
                var addMore = context.Request.Params["addMore"];
                var paramPeopleNum = context.Request.Params["peopleNum"];
                var beginDate = Convert.ToDateTime(strChooseMonth + "-01");
                var endDate = beginDate.AddMonths(1).AddDays(-1);
                string[] weekdays = { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };
                StringBuilder tblHtml = new StringBuilder();
                tblHtml.Append("<table cellspacing='0' cellpadding='0' class='list_tb tb_fix'>");
                #region 处理页面布局问题
                StringBuilder tr_cols = new StringBuilder();
                tr_cols.Append("<col style='width:50px'/>");
                tr_cols.Append("<col style='width:100px'/>");
                for (int i = 0; i <= (endDate - beginDate).Days; i++)
                {
                    tr_cols.Append("<col style='width:130px'/>");
                }
                tblHtml.Append(tr_cols);
                #endregion
                StringBuilder tr_First = new StringBuilder();
                tr_First.Append("<tr><th colspan='2'>日期</th>");
                StringBuilder tr_Second = new StringBuilder();
                tr_Second.Append("<tr><th colspan='2'>姓名</th>");
                for (int i = 0; i <= (endDate - beginDate).Days; i++)
                {
                    var date = beginDate.AddDays(i).ToString("MM-dd");
                    string week = weekdays[Convert.ToInt32(beginDate.AddDays(i).DayOfWeek)];
                    tr_First.AppendFormat("<th>{0}</th>", date);
                    tr_Second.AppendFormat("<th>{0}</th>", week);
                }
                tr_First.Append("</tr>");
                tr_Second.Append("</tr>");

                tblHtml.Append(tr_First);
                tblHtml.Append(tr_Second);
                if (addMore == "true")
                {
                    tblHtml.Clear();
                }

                if (isCustom == "false")
                {
                    var schedualTemplate = dal.GetSchedualTemplateByID(rule);
                    schedualTemplate.ListTemplateMapping = schedualTemplate.ListTemplateMapping.OrderBy(p => p.SerialNo).ThenBy(p => p.WhichDay).ToList();
                    var strPeopleNum = schedualTemplate.PeopleNum;
                    string strRooms = strRoomCombine;
                    var rooms = "";
                    for (int n = 0; n < strRooms.Split(',').Length; n++)
                    {
                        rooms += strRooms.Split(',')[n] + "</br>";
                    }
                    int peopleNum = Convert.ToInt16(strPeopleNum);
                    for (int i = 0; i < peopleNum; i++)
                    {
                        StringBuilder tr = new StringBuilder();
                        tr.Append("<tr>");
                        if (i == 0)
                        {

                            tr.AppendFormat("<td rowspan='{0}'><span roomNo='{2}'>{1}</span></td>", peopleNum, rooms, strRooms);
                        }
                        tr.Append("<td><select class='staff smallerSelect'><option>请选择</option></select></td>");
                        var _listTemplate = schedualTemplate.ListTemplateMapping.FindAll(p => p.SerialNo == (i + 1));
                        for (int j = 0; j <= (endDate - beginDate).Days; j++)
                        {
                            var _template = _listTemplate.Find(p => p.WhichDay == (j + 1));
                            string schedual = _template.Schedual;
                            string remark = _template.Remark;
                            tr.AppendFormat("<td ondblclick='ChangeSchedual(this)'><span class='period'>{0}</span></br><span class='remark'>{1}</span></td>", schedual, remark);
                        }
                        tr.Append("</tr>");
                        tblHtml.Append(tr);
                    }
                }
                else
                {
                    StringBuilder tr = new StringBuilder();
                    int peopleNum = Convert.ToInt16(paramPeopleNum);
                    for (int i = 0; i < peopleNum; i++)
                    {
                        tr.Append("<tr>");
                        tr.Append("<td><span roomNo=''></span></td>");
                        tr.Append("<td><select class='staff smallerSelect' onchange='GetMasterSchedual(this)'><option>请选择</option></select></td>");
                        for (int j = 0; j <= (endDate - beginDate).Days; j++)
                        {
                            tr.Append("<td ondblclick='ChangeSchedual(this)'><span class='period'></span></br><span class='remark'></span></td>");
                        }
                        tr.Append("</tr>");
                    }
                    tblHtml.Append(tr);
                }
                if (addMore == "false")
                {
                    tblHtml.Append("</table>");
                }
                result = tblHtml.ToString();
            }
            #endregion

            #region 添加排班信息
            if (action == "AddSchedualInfo")
            {
                var listSchedual = context.Request.Params["listSchedual"];
                var saveflag = context.Request.Params["saveflag"];
                List<SchedualInfo> list = new List<SchedualInfo>();
                if (!string.IsNullOrWhiteSpace(listSchedual))
                {
                    list = CommTools.JsonToObject(listSchedual, typeof(List<SchedualInfo>)) as List<SchedualInfo>;
                }
                #region 校验试用期员工上班时，其带教师傅都在上班时间
                var staffInfo = dal.GetStaffInfoByLevel("");
                var staffList = new List<string>();
                var staffWithMasterList = new List<string>();
                var masterSchedual = new DataSet();
                foreach (var item in list)
                {
                    if (!staffList.Contains(item.StaffNo))
                    {
                        staffList.Add(item.StaffNo);
                    }
                }
                var dataRows = staffInfo.Tables[0].Select(" StaffNo in ( " + CommTools.GetSqlInStr(staffList) + " ) ");
                if (dataRows.Length > 0)
                {
                    var masterList = new List<string>();
                    for (int i = 0; i < dataRows.Length; i++)
                    {
                        if (dataRows[i]["Status"].ToString() == "2" && !string.IsNullOrWhiteSpace(dataRows[i]["MasterStaffNo"].ToString()))
                        {
                            //带教师傅工号
                            var masterStaffNo = dataRows[i]["MasterStaffNo"].ToString();
                            if (!masterList.Contains(masterStaffNo))
                            {
                                masterList.Add(masterStaffNo);
                                staffWithMasterList.Add(dataRows[i]["StaffNo"].ToString());
                            }
                        }
                    }
                    if (masterList.Count > 0)
                    {
                        StringBuilder strWhere = new StringBuilder();
                        var maxDate = list.Max(p => Convert.ToDateTime(p.DutyDate));
                        var minDate = list.Min(p => Convert.ToDateTime(p.DutyDate));
                        strWhere.AppendFormat(" and a.DutyDate>= '{0}' ", minDate.ToString("yyyy-MM-dd"));
                        strWhere.AppendFormat(" and a.DutyDate<= '{0}' ", maxDate.ToString("yyyy-MM-dd"));
                        strWhere.AppendFormat(" and a.StaffNo in ({0}) ", CommTools.GetSqlInStr(masterList));
                        masterSchedual = dal.GetSchedualInfo(strWhere.ToString());
                    }
                }
                #endregion
                foreach (var item in list)
                {
                    //有带教师傅的
                    if (staffWithMasterList.Contains(item.StaffNo))
                    {
                        if (!string.IsNullOrWhiteSpace(item.Schedual) && item.Schedual != "休")
                        {
                            var _masterRow = staffInfo.Tables[0].Select(" StaffNo ='" + item.StaffNo + "' ")[0];
                            var _masterStaffNo = _masterRow["MasterStaffNo"].ToString();
                            var _staffName = _masterRow["Name"].ToString();
                            //验证师傅也在班
                            if (masterSchedual.Tables.Count > 0 && masterSchedual.Tables[0].Rows.Count > 0)
                            {
                                var _rows = masterSchedual.Tables[0].Select(" StaffNo='" + _masterStaffNo + "' and DutyDate='" + item.DutyDate + "' ");
                                if (_rows.Length <= 0
                                    || string.IsNullOrWhiteSpace(_rows[0]["Schedual"].ToString())
                                    || _rows[0]["Schedual"].ToString() == "休")
                                {
                                    result = "保存失败，原因：" + "员工【" + _staffName + "】试用期上班时，其带教师傅不在上班！";
                                    context.Response.Write(result);
                                    context.Response.Flush();
                                    context.Response.End();
                                    break;
                                }
                            }
                        }
                    }
                    var period = item.Schedual;
                    if (period.IndexOf("/") > -1)
                    {
                        //有两个时间段
                        var period_1 = period.Split('/')[0];
                        var period_2 = period.Split('/')[1];
                        var period_1_1 = "2017-01-01 " + period_1.Split('-')[0] + ":00";
                        var period_1_2 = "2017-01-01 " + period_1.Split('-')[1] + ":00";
                        var period_2_1 = "2017-01-01 " + period_2.Split('-')[0] + ":00";
                        var period_2_2 = "2017-01-01 " + period_2.Split('-')[1] + ":00";
                        var hours1 = (Convert.ToDateTime(period_1_2) - Convert.ToDateTime(period_1_1)).TotalHours;
                        var hours2 = (Convert.ToDateTime(period_2_2) - Convert.ToDateTime(period_2_1)).TotalHours;
                        item.Hours = Convert.ToDecimal(Math.Abs(hours1) + Math.Abs(hours2));
                        //赋值开始时间，结束时间，是否夜班
                        item.BeginTime = Convert.ToDateTime(item.DutyDate + " " + period_1.Split('-')[0] + ":00");
                        if ((Convert.ToInt32(period_1.Split('-')[1].Replace(":", "")) > 1200
                            || Convert.ToInt32(period_2.Split('-')[0].Replace(":", "")) > 1200)
                            && Convert.ToInt32(period_2.Split('-')[1].Replace(":", "")) < 1000)
                        {
                            item.EndTime = Convert.ToDateTime(item.DutyDate + " " + period_2.Split('-')[1] + ":00").AddDays(1);
                            item.IsNight = 1;
                        }
                        else
                        {
                            item.EndTime = Convert.ToDateTime(item.DutyDate + " " + period_2.Split('-')[1] + ":00");
                            item.IsNight = 0;
                        }
                    }
                    else if (period.IndexOf("/") < 0 && period.IndexOf("-") > -1)
                    {
                        //有两个时间段
                        var period_1 = "2017-01-01 " + period.Split('-')[0] + ":00";
                        var period_2 = "2017-01-01 " + period.Split('-')[1] + ":00";
                        var hours = (Convert.ToDateTime(period_2) - Convert.ToDateTime(period_1)).TotalHours;
                        item.Hours = Convert.ToDecimal(Math.Abs(hours));
                        //赋值开始时间，结束时间，是否夜班
                        item.BeginTime = Convert.ToDateTime(item.DutyDate + " " + period.Split('-')[0] + ":00");
                        if (Convert.ToInt32(period.Split('-')[0].Replace(":", "")) > 1200
                            && Convert.ToInt32(period.Split('-')[1].Replace(":", "")) < 1000)
                        {
                            item.EndTime = Convert.ToDateTime(item.DutyDate + " " + period.Split('-')[1] + ":00").AddDays(1);
                            item.IsNight = 1;
                        }
                        else
                        {
                            item.EndTime = Convert.ToDateTime(item.DutyDate + " " + period.Split('-')[1] + ":00");
                            item.IsNight = 0;
                        }
                    }
                }
                if (saveflag == "add")
                {
                    var verifyResult = dal.VerifySchedualInfo(list);
                    if (verifyResult == false)
                    {
                        result = "请勿添加重复房间，请重新选择房间";
                    }
                    else
                    {
                        var insertResult = dal.AddSchedualInfo(list);
                        result = insertResult.ToString();
                    }
                }
                else
                {
                    var insertResult = dal.AddSchedualInfo(list);
                    result = insertResult.ToString();
                }
            }
            #endregion

            #region 根据排班信息展现界面
            if (action == "GetSchedualInfo")
            {
                DataSet ds = new DataSet();
                StringBuilder roomCombineInfo = new StringBuilder();
                DataSet dsRoomCombine = new DataSet();
                dsRoomCombine = dal.GetRoomCombine();
                string begin_ChooseMonth = context.Request.Params["begin_ChooseMonth"];
                string end_ChooseMonth = context.Request.Params["end_ChooseMonth"];
                var where_beginDate = Convert.ToDateTime(begin_ChooseMonth);
                var where_endDate = Convert.ToDateTime(end_ChooseMonth);

                string paramStaffNo = context.Request.Params["staffNo"];
                string paramRooms = context.Request.Params["rooms"];
                if (!string.IsNullOrWhiteSpace(paramRooms))
                {
                    paramRooms = paramRooms.TrimEnd(',');
                }
                StringBuilder strWhere = new StringBuilder();
                strWhere.AppendFormat(" and a.DutyDate>= '{0}' ", where_beginDate.ToString("yyyy-MM-dd"));
                strWhere.AppendFormat(" and a.DutyDate<= '{0}' ", where_endDate.ToString("yyyy-MM-dd"));
                if (!string.IsNullOrWhiteSpace(paramStaffNo))
                {
                    strWhere.AppendFormat(" and a.StaffNo = '{0}' ", paramStaffNo);
                }
                if (!string.IsNullOrWhiteSpace(paramRooms))
                {
                    if (paramRooms.Split(',').Length > 0)
                    {
                        for (int i = 0; i < paramRooms.Split(',').Length; i++)
                        {
                            var tempRoomNo = paramRooms.Split(',')[i];
                            if (i == 0)
                            {
                                strWhere.AppendFormat(" and a.RoomNo in ('{0}' ", tempRoomNo);
                            }
                            else
                            {
                                strWhere.AppendFormat(" ,'{0}' ", tempRoomNo);
                            }
                        }
                        strWhere.Append(" ) ");
                    }
                }
                ds = dal.GetSchedualInfo(strWhere.ToString());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var dt = ds.Tables[0];
                    string strBeginDate = dt.Select().Min(p => p["DutyDate"]).ToString();
                    string strEndDate = dt.Select().Max(p => p["DutyDate"]).ToString();
                    var beginDate = Convert.ToDateTime(strBeginDate);
                    var endDate = Convert.ToDateTime(strEndDate);
                    string[] weekdays = { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };
                    StringBuilder tblHtml = new StringBuilder();
                    tblHtml.Append("<table cellspacing='0' cellpadding='0' class='list_tb tb_fix' id='testTB'>");
                    #region 处理页面布局问题
                    StringBuilder tr_cols = new StringBuilder();
                    tr_cols.Append("<col style='width:50px'/>");
                    tr_cols.Append("<col style='width:100px'/>");
                    for (int i = 0; i <= (endDate - beginDate).Days; i++)
                    {
                        tr_cols.Append("<col style='width:130px'/>");
                    }
                    tblHtml.Append(tr_cols);
                    #endregion

                    #region 眉头
                    StringBuilder tr_First = new StringBuilder();
                    tr_First.Append("<tr><th colspan='2'>日期</th>");
                    StringBuilder tr_Second = new StringBuilder();
                    tr_Second.Append("<tr><th colspan='2'>姓名</th>");
                    for (int i = 0; i <= (endDate - beginDate).Days; i++)
                    {
                        var date = beginDate.AddDays(i).ToString("MM-dd");
                        string week = weekdays[Convert.ToInt32(beginDate.AddDays(i).DayOfWeek)];
                        tr_First.AppendFormat("<th>{0}</th>", date);
                        tr_Second.AppendFormat("<th>{0}</th>", week);
                    }
                    tr_First.Append("</tr>");
                    tr_Second.Append("</tr>");
                    tblHtml.Append(tr_First);
                    tblHtml.Append(tr_Second);
                    #endregion

                    #region 正文
                    var rows = dt.Select("", "DutyDate").GroupBy(p => (p["StaffNo"] + "|" + p["RoomNo"]));
                    List<string> containRoom = new List<string>();
                    foreach (var item in rows)
                    {
                        string key = item.Key.ToString();
                        string staffNo = key.Split('|')[0];
                        string roomNo = key.Split('|')[1];
                        string staffName = item.First()["Name"].ToString();
                        StringBuilder tr = new StringBuilder();
                        tr.Append("<tr>");
                        //确定下该房间需要几个人护理
                        int rowspan = 0;
                        rowspan = dt.Select("RoomNo='" + roomNo + "'").GroupBy(p => p["StaffNo"]).Count();
                        if (!containRoom.Contains(roomNo))
                        {
                            containRoom.Add(roomNo);
                            var strRooms = "";
                            for (int n = 0; n < roomNo.Split(',').Length; n++)
                            {
                                strRooms += roomNo.Split(',')[n] + "\r\n";
                            }
                            tr.AppendFormat("<td rowspan='{0}'><span roomNo='{1}' class='IsRoom'>{2}</span></td>", rowspan, roomNo, strRooms);
                            #region 获取房间组合备注
                            string roomRemark = string.Empty;
                            if (!string.IsNullOrWhiteSpace(roomNo) && dsRoomCombine.Tables.Count > 0 && dsRoomCombine.Tables[0].Select("RoomList = '" + roomNo + "'").Length > 0)
                            {
                                roomRemark = dsRoomCombine.Tables[0].Select("RoomList = '" + roomNo + "'")[0]["Remark"].ToString();
                                if (!string.IsNullOrWhiteSpace(roomRemark))
                                {
                                    roomCombineInfo.Append("<div>" + roomNo + "：" + roomRemark + "</div>");
                                }
                            }
                            #endregion
                        }
                        tr.AppendFormat("<td><span staffNo='{1}'>{0}</span></td>", staffName, staffNo);

                        for (int j = 0; j <= (endDate - beginDate).Days; j++)
                        {
                            string schedualInfo = string.Empty;
                            string remark = string.Empty;
                            var getScheInfo = item.ToList().Find(p => Convert.ToDateTime(p["DutyDate"]) == beginDate.AddDays(j));
                            if (getScheInfo != null)
                            {
                                schedualInfo = item.ToList().First(p => Convert.ToDateTime(p["DutyDate"]) == beginDate.AddDays(j))["Schedual"].ToString();
                                remark = item.ToList().First(p => Convert.ToDateTime(p["DutyDate"]) == beginDate.AddDays(j))["Remark"].ToString();
                            }
                            var date = beginDate.AddDays(j).ToString("MM-dd");
                            string week = weekdays[Convert.ToInt32(beginDate.AddDays(j).DayOfWeek)];
                            tr.AppendFormat("<td bizDate='" + date + "【" + week + "】' staffName='" + staffName + "' ondblclick='ChangeSchedual(this)'><span class='period'>{0}</span><br style='mso-data-placement:same-cell;'/><span class='remark'>{1}</span></td>", schedualInfo, remark);
                        }
                        tr.Append("</tr>");
                        tblHtml.Append(tr);
                    }
                    #endregion

                    tblHtml.Append("</table>");
                    #region 房间组合详情
                    tblHtml.Append("<div class='mt_20 ftsize12' id=\"roomCombineInfo\">" + roomCombineInfo + "</div>");
                    #endregion
                    result = tblHtml.ToString();
                }
            }
            #endregion

            #region 获取排班模板列表
            if (action == "getRecordPage_SchedualTemplate")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string queryTemplateName = context.Request.Params["queryTemplateName"];
                if (!string.IsNullOrWhiteSpace(queryTemplateName))
                {
                    strWhere.AppendFormat(" and TemplateName like '%{0}%' ", queryTemplateName);
                }
                result = getRecordPage_SchedualTemplate(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 添加排班模板信息
            if (action == "AddSchedualTemplate")
            {
                var ID = context.Request.Params["ID"];
                var templateName = context.Request.Params["templateName"];
                var peopleNum = context.Request.Params["peopleNum"];
                var status = context.Request.Params["status"];
                var templateRemark = context.Request.Params["templateRemark"];
                var saveflag = context.Request.Params["saveflag"];
                string roomList = context.Request.Params["roomList"];
                var listTemplateMapping = context.Request.Params["listTemplateMapping"];
                List<TemplateMapping> list = new List<TemplateMapping>();
                if (!string.IsNullOrWhiteSpace(listTemplateMapping))
                {
                    list = CommTools.JsonToObject(listTemplateMapping, typeof(List<TemplateMapping>)) as List<TemplateMapping>;
                }
                SchedualTemplate model = new SchedualTemplate();
                model.TemplateName = templateName;
                model.PeopleNum = Convert.ToInt16(peopleNum);
                model.RoomList = roomList;
                model.TemplateRemark = templateRemark;
                model.Status = Convert.ToInt16(status);
                model.ListTemplateMapping = list;
                if (saveflag == "add")
                {
                    var verifyResult = dal.VerifySchedualTemplate(model);
                    if (verifyResult == false)
                    {
                        result = "排班模板名称重复，请重新填写！";
                    }
                    else
                    {
                        var insertResult = dal.AddSchedualTemplate(model);
                        result = insertResult.ToString();
                    }
                }
                else
                {
                    model.ID = Convert.ToInt16(ID);
                    var updateResult = dal.UpdateSchedualTemplate(model);
                    result = updateResult.ToString();
                }
            }
            #endregion

            #region 根据模板编号获取模板信息
            if (action == "GetSchedualTemplateByID")
            {
                var ID = context.Request.Params["ID"];
                var schedualTemplate = dal.GetSchedualTemplateByID(ID);
                schedualTemplate.ListTemplateMapping = schedualTemplate.ListTemplateMapping.OrderBy(p => p.SerialNo).ThenBy(p => p.WhichDay).ToList();
                result = CommTools.ObjectToJson(schedualTemplate);
            }
            #endregion

            #region 绑定排班模板下拉框
            if (action == "InitSelect_SchedualTemplate")
            {
                DataSet ds = new DataSet();
                ds = dal.GetSchedualTemplateInfo();
                StringBuilder strOpt = new StringBuilder();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    strOpt.Append("<option value=''>请选择模板</option>");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        strOpt.AppendFormat("<option value='{0}' title='人数：{2} &#10;房间：{3}'>{1}</option>", ds.Tables[0].Rows[i]["ID"], ds.Tables[0].Rows[i]["TemplateName"],
                            ds.Tables[0].Rows[i]["PeopleNum"], ds.Tables[0].Rows[i]["RoomList"]);
                    }
                }
                result = strOpt.ToString();
            }
            #endregion

            #region 绑定房间组合下拉框
            if (action == "InitSelect_RoomCombine")
            {
                DataSet ds = new DataSet();
                ds = dal.GetRoomCombine();
                StringBuilder strOpt = new StringBuilder();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    strOpt.Append("<option value=''>请选择房间组合</option>");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        strOpt.AppendFormat("<option value='{0}' title='{1}'>{0}</option>", ds.Tables[0].Rows[i]["RoomList"], ds.Tables[0].Rows[i]["Remark"]);
                    }
                }
                result = strOpt.ToString();
            }
            #endregion

            #region 根据员工号获取班次
            if (action == "GetSchedualInfoByStaffNo")
            {
                List<SchedualInfo> listModel = new List<SchedualInfo>();
                DataSet ds = new DataSet();
                string strChooseMonth = context.Request.Params["chooseMonth"];
                var where_beginDate = Convert.ToDateTime(strChooseMonth + "-01");
                var where_endDate = where_beginDate.AddMonths(1).AddDays(-1);
                string paramStaffNo = context.Request.Params["staffNo"];
                StringBuilder strWhere = new StringBuilder();
                strWhere.AppendFormat(" and a.DutyDate>= '{0}' ", where_beginDate.ToString("yyyy-MM-dd"));
                strWhere.AppendFormat(" and a.DutyDate<= '{0}' ", where_endDate.ToString("yyyy-MM-dd"));
                if (!string.IsNullOrWhiteSpace(paramStaffNo))
                {
                    strWhere.AppendFormat(" and a.StaffNo = '{0}' ", paramStaffNo);
                }
                ds = dal.GetSchedualInfo(strWhere.ToString());
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var dt = ds.Tables[0];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var dutyDate = dt.Rows[i]["DutyDate"].ToString();
                        var schedual = dt.Rows[i]["Schedual"].ToString();
                        var remark = dt.Rows[i]["Remark"].ToString();
                        listModel.Add(new SchedualInfo()
                        {
                            DutyDate = dutyDate,
                            Schedual = schedual,
                            Remark = remark
                        });
                    }
                    listModel = listModel.OrderBy(p => Convert.ToDateTime(p.DutyDate)).ToList();

                }
                result = CommTools.ObjectToJson(listModel);
            }
            #endregion

            #region 导出排班到Excel
            if (action == "PrintSchedualInfo")
            {
                string strChooseMonth = context.Request.Params["chooseMonth"];
                string paramStaffNo = context.Request.Params["staffNo"];
                string paramRooms = context.Request.Params["rooms"];
                string tbHtml = GetSchedualInfoHtml(strChooseMonth, paramStaffNo, paramRooms);
                context.Response.Clear();
                context.Response.Buffer = true;
                context.Response.ContentType = "application/force-download";
                context.Response.AddHeader("content-disposition",
                                "attachment; filename=" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                context.Response.Write("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
                context.Response.Write("<head>");
                context.Response.Write("<META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                context.Response.Write("<!--[if gte mso 9]><xml>");//添加下面这一段就会有excel的线条
                context.Response.Write("<x:ExcelWorkbook>");
                context.Response.Write("<x:ExcelWorksheets>");
                context.Response.Write("<x:ExcelWorksheet>");
                context.Response.Write("<x:Name>Report Data</x:Name>");
                context.Response.Write("<x:WorksheetOptions>");
                context.Response.Write("<x:Print>");
                context.Response.Write("<x:ValidPrinterInfo/>");
                context.Response.Write("</x:Print>");
                context.Response.Write("</x:WorksheetOptions>");
                context.Response.Write("</x:ExcelWorksheet>");
                context.Response.Write("</x:ExcelWorksheets>");
                context.Response.Write("</x:ExcelWorkbook>");
                context.Response.Write("</xml>");
                context.Response.Write("<![endif]--> ");
                context.Response.Write(tbHtml);//HTML
                context.Response.Flush();
                context.Response.End();
            }
            #endregion

            #region 获取其他工时列表
            if (action == "getRecordPage_OtherWorkTime")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string staffName = context.Request.Params["staffName"];
                if (!string.IsNullOrWhiteSpace(staffName))
                {
                    strWhere.AppendFormat(" and Name like '%{0}%' ", staffName);
                }
                result = getRecordPage_OtherWorkTime(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 保存其他工时
            if (action == "SaveOtherWorkTime")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var staffNo = context.Request.Params["staffNo"];
                var workType = context.Request.Params["workType"];
                var workDate = context.Request.Params["workDate"];
                var beginTime = context.Request.Params["beginTime"];
                var endTime = context.Request.Params["endTime"];
                var status = context.Request.Params["status"];
                var remark = context.Request.Params["remark"];
                OtherWorkTime model = new OtherWorkTime();
                model.StaffNo = staffNo;
                model.WorkType = Convert.ToInt16(workType);
                model.BeginTime = Convert.ToDateTime(workDate + " " + beginTime);
                model.EndTime = Convert.ToDateTime(workDate + " " + endTime);
                model.Hours = Convert.ToDecimal((Convert.ToDateTime(endTime) - Convert.ToDateTime(beginTime)).TotalHours);
                model.Status = Convert.ToInt16(status);
                model.Remark = remark;
                int excuteResult = 0;
                if (saveflag == "add")
                {
                    excuteResult = dal.AddOtherWorkTime(model);
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    excuteResult = dal.UpdateOtherWorkTime(model);
                }
                result = excuteResult.ToString();
            }
            #endregion

            #region 根据ID获取其他工时
            if (action == "GetOtherWorkTimeByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dal.GetOtherWorkTimeByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 获取工时统计
            if (action == "getRecordPage_SchedualCount")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere_a = new StringBuilder();
                StringBuilder strWhere_b = new StringBuilder();
                string staffName = context.Request.Params["staffName"];
                string beginDate = context.Request.Params["beginDate"];
                string endDate = context.Request.Params["endDate"];
                if (!string.IsNullOrWhiteSpace(staffName))
                {
                    strWhere_a.AppendFormat(" and b.Name like '%{0}%' ", staffName);
                    strWhere_b.AppendFormat(" and b.Name like '%{0}%' ", staffName);
                }
                if (!string.IsNullOrWhiteSpace(beginDate) && string.IsNullOrWhiteSpace(endDate))
                {
                    strWhere_a.AppendFormat(" and a.DutyDate >='{0}' ", beginDate);
                    strWhere_b.AppendFormat(" and a.BeginTime >='{0}' ", beginDate);
                }
                if (string.IsNullOrWhiteSpace(beginDate) && !string.IsNullOrWhiteSpace(endDate))
                {
                    endDate = Convert.ToDateTime(endDate).AddDays(1).ToString("yyyy-MM-dd");
                    strWhere_a.AppendFormat(" and a.DutyDate< '{0}' ", endDate);
                    strWhere_b.AppendFormat(" and a.EndTime<'{0}' ", endDate);
                }
                if (!string.IsNullOrWhiteSpace(beginDate) && !string.IsNullOrWhiteSpace(endDate))
                {
                    endDate = Convert.ToDateTime(endDate).AddDays(1).ToString("yyyy-MM-dd");
                    strWhere_a.AppendFormat(" and a.DutyDate >='{0}' and a.DutyDate< '{1}' ", beginDate, endDate);
                    strWhere_b.AppendFormat(" and a.BeginTime<='{0}' and a.EndTime>='{1}' ", endDate, beginDate);
                }
                result = getRecordPage_SchedualCount(m_currentpage, m_pagesize, strWhere_a.ToString(), strWhere_b.ToString(), sortfield, sorttype);
                strWhere_a = null;
                strWhere_b = null;
            }
            #endregion

            #region 获取班次时间列表
            if (action == "getRecordPage_SchedualTime")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();

                result = getRecordPage_SchedualTime(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 添加班次时间
            if (action == "AddSchedualTime")
            {
                var schedualName = context.Request.Params["schedualName"];
                var schedual = context.Request.Params["schedual"];
                var existsResult = dal.ExistsSchedualTime(schedualName);
                if (existsResult)
                {
                    result = "exists";
                }
                else
                {
                    var insertResult = dal.AddSchedualTime(schedualName, schedual);
                    result = insertResult.ToString();
                }
            }
            #endregion

            #region 删除班次时间
            if (action == "DeleteSchedualTime")
            {
                var ID = context.Request.Params["ID"];
                var deleteResult = dal.DeleteSchedualTime(Convert.ToInt16(ID));
                result = deleteResult.ToString();
            }
            #endregion

            #region 绑定班次选择下拉框
            if (action == "GetSchedualTime")
            {
                DataSet ds = new DataSet();
                ds = dal.GetSchedualTime();
                StringBuilder strOpt = new StringBuilder();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    strOpt.Append("<option value=''>请选择</option>");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        strOpt.AppendFormat("<option value='{0}' title='{0}'>{1}</option>", ds.Tables[0].Rows[i]["Schedual"], ds.Tables[0].Rows[i]["SchedualName"]);
                    }
                }
                result = strOpt.ToString();
            }
            #endregion

            #region 获取房间组合列表
            if (action == "getRecordPage_RoomCombine")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string queryRoom = context.Request.Params["queryRoom"];
                if (!string.IsNullOrWhiteSpace(queryRoom))
                {
                    strWhere.AppendFormat(" and RoomList like '%{0}%' ", queryRoom);
                }
                result = getRecordPage_RoomCombine(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 添加房间组合
            if (action == "SaveRoomCombine")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var remark = context.Request.Params["remark"];
                var roomList = context.Request.Params["roomList"];
                if (!string.IsNullOrWhiteSpace(roomList))
                {
                    roomList = roomList.TrimEnd(',');
                }
                RoomCombine model = new RoomCombine();
                model.RoomList = roomList;
                model.Remark = remark;
                string excuteResult = string.Empty;
                if (saveflag == "add")
                {
                    var existsResult = dal.ExistsRoomCombine(roomList);
                    if (existsResult)
                    {
                        excuteResult = "exists";
                    }
                    else
                    {
                        excuteResult = dal.AddRoomCombine(model).ToString();
                    }
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    excuteResult = dal.UpdateRoomCombine(model).ToString();
                }
                result = excuteResult.ToString();
            }
            #endregion

            #region 根据ID获取房间组合
            if (action == "GetRoomCombineInfoByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dal.GetRoomCombineInfoByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 删除房间组合
            if (action == "DeleteRoomCombine")
            {
                var ID = context.Request.Params["ID"];
                var deleteResult = dal.DeleteRoomCombine(Convert.ToInt16(ID));
                result = deleteResult.ToString();
            }
            #endregion

            #region 根据ID获取考核模板信息
            if (action == "GetAssessTemplateByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dal.GetAssessTemplateByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 根据条件获取考核模板信息
            if (action == "GetAssessTemplateByWhere")
            {
                var rank = context.Request.Params["rank"];
                var assessType = context.Request.Params["assessType"];
                string strWhere = string.Empty;
                strWhere = string.Format(" and Rank='{0}' and AssessType='{1}' and Status=1 ", rank, assessType);
                var staff = dal.GetAssessTemplateByWhere(strWhere);
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 保存考核模板
            if (action == "SaveAssessTemplate")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var rank = context.Request.Params["rank"];
                var post = context.Request.Params["post"];
                var assessType = context.Request.Params["assessType"];
                var status = context.Request.Params["status"];
                var details = context.Request.Params["details"];
                List<AssessTemplateDetail> listAssessTemplateDetail = new List<AssessTemplateDetail>();
                if (!string.IsNullOrWhiteSpace(details))
                {
                    listAssessTemplateDetail = CommTools.JsonToObject(details, typeof(List<AssessTemplateDetail>)) as List<AssessTemplateDetail>;
                }
                AssessTemplate model = new AssessTemplate();
                model.Rank = rank;
                model.Post = Convert.ToInt16(post);
                model.AssessType = assessType;
                model.Status = Convert.ToInt16(status);
                model.ListAssessTemplateDetail = listAssessTemplateDetail;
                string excuteResult = string.Empty;
                if (saveflag == "add")
                {
                    var existsResult = dal.ExistsAssessTemplate(model);
                    if (existsResult)
                    {
                        excuteResult = "exists";
                    }
                    else
                    {
                        excuteResult = dal.AddAssessTemplate(model).ToString();
                    }
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    var existsResult = dal.ExistsAssessTemplate(model);
                    if (existsResult)
                    {
                        excuteResult = "exists";
                    }
                    else if (dal.ExistsHasStaffAssess(model))
                    {
                        excuteResult = "existsUsing";
                    }
                    else
                    {
                        excuteResult = dal.UpdateAssessTemplate(model).ToString();
                    }
                }

                result = excuteResult;
            }
            #endregion

            #region 获取考核模板列表
            if (action == "getRecordPage_AssessTemplate")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string rank = context.Request.Params["queryRank"];
                if (!string.IsNullOrWhiteSpace(rank))
                {
                    strWhere.AppendFormat(" and Rank like '%{0}%' ", rank);
                }
                result = getRecordPage_AssessTemplate(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 删除考核模板
            if (action == "DeleteAssessTemplate")
            {
                var excuteResult = "";
                var ID = context.Request.Params["ID"];
                var model = new AssessTemplate();
                model.ID = Convert.ToInt16(ID);
                if (dal.ExistsHasStaffAssess(model))
                {
                    excuteResult = "existsUsing";
                }
                else
                {
                    excuteResult = dal.DeleteAssessTemplate(Convert.ToInt16(ID)).ToString();
                }
                result = excuteResult;
            }
            #endregion
            
            #region 获取员工绩效考核列表
            if (action == "getRecordPage_StaffAssess")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string name = context.Request.Params["name"];
                string assessDate = context.Request.Params["assessDate"];
                if (!string.IsNullOrWhiteSpace(name))
                {
                    strWhere.AppendFormat(" and c.`Name` like '%{0}%' ", name);
                }
                if (!string.IsNullOrWhiteSpace(assessDate))
                {
                    var where_beginDate = Convert.ToDateTime(assessDate + "-01");
                    var where_endDate = where_beginDate.AddMonths(1).AddDays(-1);
                    strWhere.AppendFormat(" and a.AssessDate >='{0}' and a.AssessDate <='{1}' ", where_beginDate, where_endDate);
                }
                result = getRecordPage_StaffAssess(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 保存员工绩效考核
            if (action == "SaveStaffAssess")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var staffNo = context.Request.Params["staffNo"];
                var assessTemplateID = context.Request.Params["assessTemplateID"];
                var assessDate = context.Request.Params["assessDate"];
                var score = context.Request.Params["score"].TrimEnd(',');
                var status = context.Request.Params["status"];
                var remark = context.Request.Params["remark"];
                StaffAssess model = new StaffAssess();
                model.StaffNo = staffNo;
                model.AssessTemplateID = Convert.ToInt16(assessTemplateID);
                model.AssessDate = Convert.ToDateTime(assessDate);
                model.Score = score;
                model.Status = Convert.ToInt16(status);
                model.Remark = remark;
                string excuteResult = string.Empty;
                if (saveflag == "add")
                {
                    excuteResult = dal.AddStaffAssess(model).ToString();
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    excuteResult = dal.UpdateStaffAssess(model).ToString();
                }
                result = excuteResult;
            }
            #endregion

            #region 根据ID获取员工绩效考核
            if (action == "GetStaffAssessByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dal.GetStaffAssessByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 删除员工绩效考核
            if (action == "DeleteStaffAssess")
            {
                var ID = context.Request.Params["ID"];
                var deleteResult = dal.DeleteStaffAssess(Convert.ToInt16(ID));
                result = deleteResult.ToString();
            }
            #endregion

            #region 导出员工考核列表
            if (action == "ExportStaffAssess")
            {
                StringBuilder strWhere = new StringBuilder();
                string name = context.Request.Params["name"];
                string assessDate = context.Request.Params["assessDate"];
                if (!string.IsNullOrWhiteSpace(name))
                {
                    strWhere.AppendFormat(" and c.`Name` like '%{0}%' ", name);
                }
                if (!string.IsNullOrWhiteSpace(assessDate))
                {
                    var where_beginDate = Convert.ToDateTime(assessDate + "-01");
                    var where_endDate = where_beginDate.AddMonths(1).AddDays(-1);
                    strWhere.AppendFormat(" and a.AssessDate >='{0}' and a.AssessDate <='{1}' ", where_beginDate, where_endDate);
                }
                DataSet ds = dal.GetStaffAssessForExport(strWhere.ToString());
                StringBuilder tblHtml = new StringBuilder();
                #region 拼接成table
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
                      + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
                      + "<tr class=\"\" >");
                    tblHtml.Append("  <th style='width:10%'>序号</th>"
                                   + "<th style='width:15%'>姓名</th>"
                                   + "<th style='width:10%'>职级</th>"
                                   + "<th style='width:20%'>考核时间</th>"
                                   + "<th style='width:10%'>考核类型</th>"
                                   + "<th style='width:10%'>总分</th>"
                                   + "<th style='width:10%'>状态</th>"
                                   );
                    tblHtml.Append("</tr><tbody id=wjtbl>");
                    var dt = ds.Tables[0];
                    if (dt != null)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            tblHtml.Append("<tr>");
                            tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["Name"] + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["Rank"] + "</td>");
                            tblHtml.Append("<td style=\"vnd.ms-excel.numberformat:@\">" + (Convert.ToDateTime(dt.Rows[i]["AssessDate"]).ToString("yyyy-MM")) + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["AssessType"] + "</td>");
                            var score = dt.Rows[i]["Score"].ToString();
                            decimal sumScore = 0;
                            if (!string.IsNullOrWhiteSpace(score))
                            {
                                for (int m = 0; m < score.Split(',').Length; m++)
                                {
                                    sumScore += Convert.ToDecimal(score.Split(',')[m]);
                                }
                            }
                            tblHtml.Append("<td>" + sumScore + "</td>");
                            var status = ((dt.Rows[i]["Status"].ToString() == "0") ? "考核中" : "确认");
                            tblHtml.Append("<td>" + status + "</td>");
                            tblHtml.Append("</tr>");
                        }
                    }
                    tblHtml.Append("</tbody></table></div>");
                }
                #endregion
                context.Response.Clear();
                context.Response.Buffer = true;
                context.Response.ContentType = "application/force-download";
                context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                context.Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                context.Response.Write("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
                context.Response.Write("<head>");
                context.Response.Write("<META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                context.Response.Write("<!--[if gte mso 9]><xml>");//添加下面这一段就会又excel的线条
                context.Response.Write("<x:ExcelWorkbook>");
                context.Response.Write("<x:ExcelWorksheets>");
                context.Response.Write("<x:ExcelWorksheet>");
                context.Response.Write("<x:Name>Report Data</x:Name>");
                context.Response.Write("<x:WorksheetOptions>");
                context.Response.Write("<x:Print>");
                context.Response.Write("<x:ValidPrinterInfo/>");
                context.Response.Write("</x:Print>");
                context.Response.Write("</x:WorksheetOptions>");
                context.Response.Write("</x:ExcelWorksheet>");
                context.Response.Write("</x:ExcelWorksheets>");
                context.Response.Write("</x:ExcelWorkbook>");
                context.Response.Write("</xml>");
                context.Response.Write("<![endif]--> ");
                context.Response.Write(tblHtml.ToString());//HTML
                context.Response.Flush();
                context.Response.End();
            }
            #endregion

            #region 获取基础信息列表
            if (action == "getRecordPage_BasicInfo")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                var chooseType = context.Request.Params["chooseType"];
                if (!string.IsNullOrWhiteSpace(chooseType))
                {
                    strWhere.AppendFormat(" and ParamType='{0}' ", chooseType);
                }
                result = getRecordPage_BasicInfo(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 添加基础信息
            if (action == "AddBasicInfo")
            {
                var chooseType = context.Request.Params["chooseType"];
                var name = context.Request.Params["name"];
                var existsResult = dal.ExistsBasicInfo(chooseType, name);
                if (existsResult)
                {
                    result = "exists";
                }
                else
                {
                    var insertResult = dal.AddBasicInfo(chooseType, name);
                    result = insertResult.ToString();
                }
            }
            #endregion

            #region 删除基础信息
            if (action == "DeleteBasicInfo")
            {
                var ID = context.Request.Params["ID"];
                var deleteResult = dal.DeleteBasicInfo(Convert.ToInt16(ID));
                result = deleteResult.ToString();
            }
            #endregion

            #region 获取岗位列表
            if (action == "getRecordPage_PostInfo")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string name = context.Request.Params["name"];
                if (!string.IsNullOrWhiteSpace(name))
                {
                    strWhere.AppendFormat(" and PostName like '%{0}%' ", name);
                }
                result = getRecordPage_PostInfo(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 保存岗位
            if (action == "SavePostInfo")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var postLevel = context.Request.Params["postLevel"];
                var postName = context.Request.Params["postName"];
                var rate = context.Request.Params["rate"];
                var remark = context.Request.Params["remark"];
                PostInfo model = new PostInfo();
                model.PostLevel = postLevel;
                model.PostName = postName;
                model.Remark = remark;
                if (!string.IsNullOrWhiteSpace(rate))
                {
                    model.Rate = Convert.ToDecimal(rate);
                }
                else
                {
                    model.Rate = 0;
                }
                string excuteResult = string.Empty;
                if (saveflag == "add")
                {
                    var existsResult1 = dal.ExistsPostLevel(model);
                    var existsResult2 = dal.ExistsPostName(model);
                    if (existsResult1 || existsResult2)
                    {
                        excuteResult = "exists";
                    }
                    else
                    {
                        excuteResult = dal.AddPostInfo(model).ToString();
                    }
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    var existsResult1 = dal.ExistsPostLevel(model);
                    var existsResult2 = dal.ExistsPostName(model);
                    if (existsResult1 || existsResult2)
                    {
                        excuteResult = "exists";
                    }
                    else
                    {
                        excuteResult = dal.UpdatePostInfo(model).ToString();
                    }
                }
                result = excuteResult.ToString();
            }
            #endregion

            #region 根据ID获取岗位
            if (action == "GetPostInfoByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dal.GetPostInfoByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 导出员工信息
            if (action == "ExportStaffInfo")
            {
                StringBuilder strWhere = new StringBuilder();
                string queryName = context.Request.Params["queryName"];
                string querySex = context.Request.Params["querySex"];
                string queryPostLevel = context.Request.Params["queryPostLevel"];
                string querySkillLevel = context.Request.Params["querySkillLevel"];
                string queryStatus = context.Request.Params["queryStatus"];
                if (!string.IsNullOrWhiteSpace(queryName))
                {
                    strWhere.AppendFormat(" and a.Name like '%{0}%' ", queryName);
                }
                if (!string.IsNullOrWhiteSpace(querySex))
                {
                    strWhere.AppendFormat(" and a.Sex = '{0}' ", querySex);
                }
                if (!string.IsNullOrWhiteSpace(queryPostLevel) && queryPostLevel != "null")
                {
                    strWhere.AppendFormat(" and a.PostLevel = '{0}' ", queryPostLevel);
                }
                if (!string.IsNullOrWhiteSpace(querySkillLevel) && querySkillLevel != "null")
                {
                    strWhere.AppendFormat(" and a.SkillLevel = '{0}' ", querySkillLevel);
                }
                if (!string.IsNullOrWhiteSpace(queryStatus))
                {
                    strWhere.AppendFormat(" and a.Status = '{0}' ", queryStatus);
                }
                DataSet ds = dal.GetStaffInfoForExport(strWhere.ToString());
                StringBuilder tblHtml = new StringBuilder();
                #region 拼接成table
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
                      + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
                      + "<tr class=\"\" >");
                    tblHtml.Append("  <th>序号</th>"
                           + "<th>姓名</th>"
                           + "<th>性别</th>"
                           + "<th>身份证号</th>"
                           + "<th>岗位</th>"
                           + "<th>职级</th>"
                           + "<th>员工编号</th>"
                           + "<th>带教师傅</th>"
                           + "<th>状态</th>"
                           + "<th>入职日期</th>"
                           + "<th>转正日期</th>"
                           + "<th>离职日期</th>"
                           + "<th>学历</th>"
                           + "<th>技能等级</th>"
                           + "<th>联系电话</th>"
                           + "<th>紧急联络人姓名</th>"
                           + "<th>紧急联络人电话</th>"
                           + "<th>护理证级别</th>"
                           + "<th>护理证日期</th>"
                           + "<th>健康证日期</th>"
                           + "<th>员工合同日期</th>"
                           );
                    tblHtml.Append("</tr><tbody id=wjtbl>");
                    var dt = ds.Tables[0];
                    if (dt != null)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            tblHtml.Append("<tr>");
                            tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["Name"] + "</td>");
                            var sexName = dt.Rows[i]["Sex"].ToString() == "1" ? "女" : "男";
                            tblHtml.Append("<td>" + sexName + "</td>");
                            tblHtml.Append("<td style=\"vnd.ms-excel.numberformat:@\">" + dt.Rows[i]["IDCardNo"] + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["PostName"] + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["Rank"] + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["StaffOtherNo"] + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["MasterStaffName"] + "</td>");
                            var status = (dt.Rows[i]["Status"].ToString() == "0") ? "试用" : (dt.Rows[i]["Status"].ToString() == "1") ? "转正" : "离职";
                            tblHtml.Append("<td>" + status + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["HireDate"] + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["RegularDate"] + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["LeaveDate"] + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["Education"] + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["SkillLevel"] + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["Phone"] + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["EmergencyContactName"] + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["EmergencyContactPhone"] + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["NurseCardLevel"] + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["NurseCardDate"] + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["HealthCardDate"] + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["ContractDate"] + "</td>");
                            tblHtml.Append("</tr>");
                        }
                    }
                    tblHtml.Append("</tbody></table></div>");
                }
                #endregion
                context.Response.Clear();
                context.Response.Buffer = true;
                context.Response.ContentType = "application/force-download";
                context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                context.Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                context.Response.Write("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
                context.Response.Write("<head>");
                context.Response.Write("<META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                context.Response.Write("<!--[if gte mso 9]><xml>");//添加下面这一段就会又excel的线条
                context.Response.Write("<x:ExcelWorkbook>");
                context.Response.Write("<x:ExcelWorksheets>");
                context.Response.Write("<x:ExcelWorksheet>");
                context.Response.Write("<x:Name>Report Data</x:Name>");
                context.Response.Write("<x:WorksheetOptions>");
                context.Response.Write("<x:Print>");
                context.Response.Write("<x:ValidPrinterInfo/>");
                context.Response.Write("</x:Print>");
                context.Response.Write("</x:WorksheetOptions>");
                context.Response.Write("</x:ExcelWorksheet>");
                context.Response.Write("</x:ExcelWorksheets>");
                context.Response.Write("</x:ExcelWorkbook>");
                context.Response.Write("</xml>");
                context.Response.Write("<![endif]--> ");
                context.Response.Write(tblHtml.ToString());//HTML
                context.Response.Flush();
                context.Response.End();
            }
            #endregion

            #region 合理性检测
            if (action == "CheckReasonableSchedualInfo")
            {
                var type = context.Request.Params["type"];
                var paramType_a = context.Request.Params["paramType_a"];
                var paramType_b = context.Request.Params["paramType_b"];
                var paramType_c = context.Request.Params["paramType_c"];
                var paramType_d = context.Request.Params["paramType_d"];
                var paramType_e = context.Request.Params["paramType_e"];
                var where_beginDate = Convert.ToDateTime(context.Request.Params["begin_ChooseMonth"]);
                var where_endDate = Convert.ToDateTime(context.Request.Params["end_ChooseMonth"]);
                StringBuilder strWhere = new StringBuilder();
                DataSet ds = new DataSet();
                StringBuilder tblHtml = new StringBuilder();
                switch (type)
                {
                    case "a":
                        ds = dal.GetReasonableSchedualInfo(type, where_beginDate, where_endDate, paramType_a);
                        tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist' >");
                        tblHtml.Append("<table cellspacing='0' cellpadding='0' class='list_tb'><tr class=\"\" >");
                        tblHtml.Append("  <th style='width:10%'>序号</th>"
                                       + "<th style='width:10%'>姓名</th>"
                                       + "<th style='width:20%'>日期</th>"
                                       + "<th style='width:20%'>班次</th>"
                                       + "<th style='width:20%'>上一班次结束时间</th>"
                                       + "<th style='width:20%'>班次间隔时间</th>"
                                       );
                        tblHtml.Append("</tr><tbody id=wjtbl>");
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                var row = ds.Tables[0].Rows[i];
                                tblHtml.Append("<tr>");
                                tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                                tblHtml.Append("<td>" + row["Name"] + "</td>");
                                tblHtml.Append("<td>" + row["DutyDate"] + "</td>");
                                tblHtml.Append("<td>" + row["Schedual"] + "</td>");
                                tblHtml.Append("<td>" + row["LastEndTime"] + "</td>");
                                var period = (Convert.ToDateTime(row["BeginTime"]) - Convert.ToDateTime(row["LastEndTime"])).TotalHours;
                                tblHtml.Append("<td>" + period.ToString("0.0") + "</td>");
                                tblHtml.Append("</tr>");
                            }
                        }
                        tblHtml.Append("</tbody></table>");
                        tblHtml.Append("</div>");
                        break;
                    case "b":
                        ds = dal.GetReasonableSchedualInfo(type, where_beginDate, where_endDate, paramType_b);
                        tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist' >");
                        tblHtml.Append("<table cellspacing='0' cellpadding='0' class='list_tb'><tr class=\"\" >");
                        tblHtml.Append("  <th style='width:10%'>序号</th>"
                                       + "<th style='width:20%'>姓名</th>"
                                       + "<th style='width:30%'>开始日期</th>"
                                       + "<th style='width:30%'>结束日期</th>"
                                       + "<th style='width:10%'>连续天数</th>"
                                       );
                        tblHtml.Append("</tr><tbody id=wjtbl>");
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                var row = ds.Tables[0].Rows[i];
                                tblHtml.Append("<tr>");
                                tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                                tblHtml.Append("<td>" + row["Name"] + "</td>");
                                tblHtml.Append("<td>" + row["BeginDate"] + "</td>");
                                tblHtml.Append("<td>" + row["EndDate"] + "</td>");
                                tblHtml.Append("<td>" + row["Days"] + "</td>");
                                tblHtml.Append("</tr>");
                            }
                        }
                        tblHtml.Append("</tbody></table>");
                        tblHtml.Append("</div>");
                        break;
                    case "c":
                        ds = dal.GetReasonableSchedualInfo(type, where_beginDate, where_endDate, paramType_c);
                        tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist' >");
                        tblHtml.Append("<table cellspacing='0' cellpadding='0' class='list_tb'><tr class=\"\" >");
                        tblHtml.Append("  <th style='width:10%'>序号</th>"
                                       + "<th style='width:20%'>姓名</th>"
                                       + "<th style='width:30%'>开始日期</th>"
                                       + "<th style='width:30%'>结束日期</th>"
                                       + "<th style='width:10%'>连续天数</th>"
                                       );
                        tblHtml.Append("</tr><tbody id=wjtbl>");
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                var row = ds.Tables[0].Rows[i];
                                tblHtml.Append("<tr>");
                                tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                                tblHtml.Append("<td>" + row["Name"] + "</td>");
                                tblHtml.Append("<td>" + row["BeginDate"] + "</td>");
                                tblHtml.Append("<td>" + row["EndDate"] + "</td>");
                                tblHtml.Append("<td>" + row["Days"] + "</td>");
                                tblHtml.Append("</tr>");
                            }
                        }
                        tblHtml.Append("</tbody></table>");
                        tblHtml.Append("</div>");
                        break;
                    case "d":
                        ds = dal.GetReasonableSchedualInfo(type, where_beginDate, where_endDate, paramType_d);
                        tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist' >");
                        tblHtml.Append("<table cellspacing='0' cellpadding='0' class='list_tb'><tr class=\"\" >");
                        tblHtml.Append("  <th style='width:5%'>序号</th>"
                                       + "<th style='width:20%'>姓名</th>"
                                       + "<th style='width:20%'>累积工时数</th>"
                                       + "<th style='width:20%'>折算工时数</th>"
                                       + "<th style='width:15%'>最小工时数</th>"
                                       + "<th style='width:20%'>最小工时数持有人</th>"
                                       );
                        tblHtml.Append("</tr><tbody id=wjtbl>");
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                var row = ds.Tables[0].Rows[i];
                                tblHtml.Append("<tr>");
                                tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                                tblHtml.Append("<td>" + row["Name"] + "</td>");
                                tblHtml.Append("<td>" + row["SumHours"] + "</td>");
                                tblHtml.Append("<td>" + row["ActSumHours"] + "</td>");
                                tblHtml.Append("<td>" + row["MinSumHours"] + "</td>");
                                tblHtml.Append("<td>" + row["MinStaffName"] + "</td>");
                                tblHtml.Append("</tr>");
                            }
                        }
                        tblHtml.Append("</tbody></table>");
                        tblHtml.Append("</div>");
                        break;
                    case "e":
                        ds = dal.GetReasonableSchedualInfo(type, where_beginDate, where_endDate, paramType_e);
                        tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist' >");
                        tblHtml.Append("<table cellspacing='0' cellpadding='0' class='list_tb'><tr class=\"\" >");
                        tblHtml.Append("  <th style='width:5%'>序号</th>"
                                       + "<th style='width:20%'>姓名</th>"
                                       + "<th style='width:20%'>累积夜班数</th>"
                                       + "<th style='width:20%'>折算夜班数</th>"
                                       + "<th style='width:15%'>最少夜班数</th>"
                                       + "<th style='width:20%'>最少夜班数持有人</th>"
                                       );
                        tblHtml.Append("</tr><tbody id=wjtbl>");
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                var row = ds.Tables[0].Rows[i];
                                tblHtml.Append("<tr>");
                                tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                                tblHtml.Append("<td>" + row["Name"] + "</td>");
                                tblHtml.Append("<td>" + row["SumNights"] + "</td>");
                                tblHtml.Append("<td>" + row["ActSumNights"] + "</td>");
                                tblHtml.Append("<td>" + row["MinSumNights"] + "</td>");
                                tblHtml.Append("<td>" + row["MinStaffName"] + "</td>");
                                tblHtml.Append("</tr>");
                            }
                        }
                        tblHtml.Append("</tbody></table>");
                        tblHtml.Append("</div>");
                        break;
                    default:
                        break;
                }
                result = tblHtml.ToString();
            }
            #endregion

            #region 获取员工请假列表
            if (action == "getRecordPage_StaffLeave")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string staffName = context.Request.Params["staffName"];
                if (!string.IsNullOrWhiteSpace(staffName))
                {
                    strWhere.AppendFormat(" and Name like '%{0}%' ", staffName);
                }
                result = getRecordPage_StaffLeave(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 保存员工请假
            if (action == "SaveStaffLeave")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var staffNo = context.Request.Params["staffNo"];
                var leaveType = context.Request.Params["leaveType"];
                var beginDate = context.Request.Params["beginDate"];
                var endDate = context.Request.Params["endDate"];
                var status = context.Request.Params["status"];
                var remark = context.Request.Params["remark"];
                StaffLeave model = new StaffLeave();
                model.StaffNo = staffNo;
                model.LeaveType = leaveType;
                model.BeginDate = Convert.ToDateTime(beginDate);
                model.EndDate = Convert.ToDateTime(endDate);
                model.Status = Convert.ToInt16(status);
                model.Remark = remark;
                int excuteResult = 0;
                if (saveflag == "add")
                {
                    excuteResult = dal.AddStaffLeave(model);
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    excuteResult = dal.UpdateStaffLeave(model);
                }
                result = excuteResult.ToString();
            }
            #endregion

            #region 根据ID获取员工请假
            if (action == "GetStaffLeaveByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dal.GetStaffLeaveByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 获取员工人脉列表
            if (action == "getRecordPage_StaffConnection")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string staffName = context.Request.Params["staffName"];
                if (!string.IsNullOrWhiteSpace(staffName))
                {
                    strWhere.AppendFormat(" and b.Name like '%{0}%' ", staffName);
                }
                result = getRecordPage_StaffConnection(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 保存员工人脉
            if (action == "SaveStaffConnection")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var staffNo = context.Request.Params["staffNo"];
                var connectStaffNo = context.Request.Params["connectStaffNo"];
                var connectGuestID = context.Request.Params["connectGuestID"];
                var otherConnectName = context.Request.Params["otherConnectName"];
                var connectionType = context.Request.Params["connectionType"];
                var remark = context.Request.Params["remark"];
                StaffConnection model = new StaffConnection();
                model.StaffNo = staffNo;
                model.ConnectStaffNo = connectStaffNo;
                model.ConnectGuestID = connectGuestID;
                model.OtherConnectName = otherConnectName;
                model.ConnectionType = connectionType;
                model.Remark = remark;
                int excuteResult = 0;
                if (saveflag == "add")
                {
                    excuteResult = dal.AddStaffConnection(model);
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    excuteResult = dal.UpdateStaffConnection(model);
                }
                result = excuteResult.ToString();
            }
            #endregion

            #region 根据ID获取员工人脉
            if (action == "GetStaffConnectionByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dal.GetStaffConnectionByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 删除员工人脉
            if (action == "DeleteStaffConnection")
            {
                var ID = context.Request.Params["ID"];
                var deleteResult = dal.DeleteStaffConnection(Convert.ToInt16(ID));
                result = deleteResult.ToString();
            }
            #endregion

            #region 合理性检测参数设置
            if (action== "SaveReasonableParam")
            {
                var paramType_a = context.Request.Params["paramType_a"];
                var paramType_b = context.Request.Params["paramType_b"];
                var paramType_c = context.Request.Params["paramType_c"];
                var paramType_d = context.Request.Params["paramType_d"];
                var paramType_e = context.Request.Params["paramType_e"];
                ReasonableParam model = new ReasonableParam();
                model.ParamType_A = paramType_a;
                model.ParamType_B = paramType_b;
                model.ParamType_C = paramType_c;
                model.ParamType_D = paramType_d;
                model.ParamType_E = paramType_e;
                var saveResult = dal.SaveReasonableParam(model);
                result = saveResult.ToString();
            }
            #endregion

            #region 获取合理性检测参数
            if (action == "GetReasonableParam")
            {
                var model = dal.GetReasonableParam();
                result = CommTools.ObjectToJson(model);
            }
            #endregion

            #region 获取楼层列表
            if (action == "getRecordPage_Floor")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string floorID = context.Request.Params["floorID"];
                if (!string.IsNullOrWhiteSpace(floorID))
                {
                    strWhere.AppendFormat(" and FloorID = '{0}' ", floorID);
                }
                result = getRecordPage_Floor(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 保存楼层
            if (action == "SaveFloor")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var floorID = context.Request.Params["floorID"];
                var remark = context.Request.Params["remark"];
                var manager = context.Request.Params["manager"];
                var status = context.Request.Params["status"];
                Floor model = new Floor();
                model.FloorID = floorID;
                model.Remark = remark;
                model.Manager = manager;
                model.Status = Convert.ToInt16(status);
                string excuteResult = string.Empty;
                if (saveflag == "add")
                {
                    var existsResult = dal.ExistsFloor(model);
                    if (existsResult)
                    {
                        excuteResult = "exists";
                    }
                    else
                    {
                        excuteResult = dal.AddFloor(model).ToString();
                    }
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    var existsResult = dal.ExistsFloor(model);
                    if (existsResult)
                    {
                        excuteResult = "exists";
                    }
                    else
                    {
                        excuteResult = dal.UpdateFloor(model).ToString();
                    }
                }
                result = excuteResult.ToString();
            }
            #endregion

            #region 根据ID获取楼层
            if (action == "GetFloorByID")
            {
                var ID = context.Request.Params["ID"];
                var floor = dal.GetFloorByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(floor);
            }
            #endregion

            #region 获取房间列表
            if (action == "getRecordPage_Room")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string roomNo = context.Request.Params["roomNo"];
                if (!string.IsNullOrWhiteSpace(roomNo))
                {
                    strWhere.AppendFormat(" and RoomNo = '{0}' ", roomNo);
                }
                result = getRecordPage_Room(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 保存房间
            if (action == "SaveRoom")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var floorID = context.Request.Params["floorID"];
                var roomNo = context.Request.Params["roomNo"];
                var remark = context.Request.Params["remark"];
                var roomSex = context.Request.Params["roomSex"];
                var peopleNum = context.Request.Params["peopleNum"];
                var status = context.Request.Params["status"];
                Room model = new Room();
                model.RoomNo = roomNo;
                model.FloorID = floorID;
                model.Remark = remark;
                model.RoomSex = Convert.ToInt16(roomSex);
                model.PeopleNum = Convert.ToInt16(peopleNum);
                model.Status = Convert.ToInt16(status);
                string excuteResult = string.Empty;
                if (saveflag == "add")
                {
                    var existsResult = dal.ExistsRoom(model);
                    if (existsResult)
                    {
                        excuteResult = "exists";
                    }
                    else
                    {
                        excuteResult = dal.AddRoom(model).ToString();
                    }
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    var existsResult = dal.ExistsRoom(model);
                    if (existsResult)
                    {
                        excuteResult = "exists";
                    }
                    else
                    {
                        excuteResult = dal.UpdateRoom(model).ToString();
                    }
                }
                result = excuteResult.ToString();
            }
            #endregion

            #region 根据ID获取房间
            if (action == "GetRoomByID")
            {
                var ID = context.Request.Params["ID"];
                var room = dal.GetRoomByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(room);
            }
            #endregion

            #region 导出客人信息
            if (action == "ExportGuestInfo")
            {
                StringBuilder strWhere = new StringBuilder();
                string queryName = context.Request.Params["queryName"];
                string queryStatus = context.Request.Params["queryStatus"];
                string queryFloorID = context.Request.Params["queryFloorID"];
                string queryRoomNo = context.Request.Params["queryRoomNo"];
                string queryNurseLevel = context.Request.Params["queryNurseLevel"];
                string queryAge1 = context.Request.Params["queryAge1"];
                string queryAge2 = context.Request.Params["queryAge2"];
                string queryAdmissionDate1 = context.Request.Params["queryAdmissionDate1"];
                string queryAdmissionDate2 = context.Request.Params["queryAdmissionDate2"];
                if (!string.IsNullOrWhiteSpace(queryName))
                {
                    strWhere.AppendFormat(" and Name like '%{0}%' ", queryName);
                }
                if (!string.IsNullOrWhiteSpace(queryStatus))
                {
                    if (queryStatus == "ShowDefault")
                    {
                        strWhere.Append(" and `Status` in (0,1) ");
                    }
                    else
                    {
                        strWhere.AppendFormat(" and `Status` = '{0}' ", queryStatus);
                    }
                }
                if (!string.IsNullOrWhiteSpace(queryFloorID) && queryFloorID != "null")
                {
                    strWhere.AppendFormat(" and FloorID = '{0}' ", queryFloorID);
                }
                if (!string.IsNullOrWhiteSpace(queryRoomNo) && queryRoomNo != "null")
                {
                    strWhere.AppendFormat(" and RoomNo = '{0}' ", queryRoomNo);
                }
                if (!string.IsNullOrWhiteSpace(queryNurseLevel))
                {
                    strWhere.AppendFormat(" and NurseLevel = '{0}' ", queryNurseLevel);
                }
                if (!string.IsNullOrWhiteSpace(queryAge1))
                {
                    strWhere.AppendFormat(" and Age>= '{0}' ", queryAge1);
                }
                if (!string.IsNullOrWhiteSpace(queryAge2))
                {
                    strWhere.AppendFormat(" and Age<= '{0}' ", queryAge2);
                }
                if (!string.IsNullOrWhiteSpace(queryAdmissionDate1))
                {
                    strWhere.AppendFormat(" and AdmissionDate>= '{0}' ", queryAdmissionDate1);
                }
                if (!string.IsNullOrWhiteSpace(queryAdmissionDate2))
                {
                    strWhere.AppendFormat(" and AdmissionDate<= '{0}' ", queryAdmissionDate2);
                }
                DataSet ds = dal.GetGuestInfoForExport(strWhere.ToString());
                StringBuilder tblHtml = new StringBuilder();
                #region 拼接成table
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
                      + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
                      + "<tr class=\"\" >");
                    tblHtml.Append("  <th>序号</th>"
                           + "<th>姓名</th>"
                           + "<th>性别</th>"
                           + "<th>年龄</th>"
                           + "<th>出生日期</th>"
                           + "<th>楼层</th>"
                           + "<th>房间号</th>"
                           + "<th>床位号</th>"
                           + "<th>护理等级</th>"
                           + "<th>身份证号</th>"
                           + "<th>状态</th>"
                           + "<th>试住日期</th>"
                           + "<th>入住日期</th>"
                           + "<th>离院日期</th>"
                           + "<th>调整护理等级日期</th>"
                           + "<th>备注</th>"
                           );
                    tblHtml.Append("</tr><tbody id=wjtbl>");
                    var dt = ds.Tables[0];
                    if (dt != null)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            tblHtml.Append("<tr>");
                            tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["Name"] + "</td>");
                            var sexName = dt.Rows[i]["Sex"].ToString() == "1" ? "女" : "男";
                            tblHtml.Append("<td>" + sexName + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["Age"] + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["BirthDay"] + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["FloorID"] + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["RoomNo"] + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["BedNo"] + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["NurseLevel"] + "</td>");
                            tblHtml.Append("<td style=\"vnd.ms-excel.numberformat:@\">" + dt.Rows[i]["IDCardNo"] + "</td>");
                            var status = (dt.Rows[i]["Status"].ToString() == "0") ? "入住" : (dt.Rows[i]["Status"].ToString() == "1") ? "试住" : "离院";
                            tblHtml.Append("<td>" + status + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["TryAdmissionDate"] + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["AdmissionDate"] + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["LeaveDate"] + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["ChangeLevelDate"] + "</td>");
                            tblHtml.Append("<td>" + dt.Rows[i]["Remark"] + "</td>");
                            tblHtml.Append("</tr>");
                        }
                    }
                    tblHtml.Append("</tbody></table></div>");
                }
                #endregion
                context.Response.Clear();
                context.Response.Buffer = true;
                context.Response.ContentType = "application/force-download";
                context.Response.ContentEncoding = System.Text.Encoding.UTF8;
                context.Response.AddHeader("content-disposition", "attachment; filename=" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
                context.Response.Write("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
                context.Response.Write("<head>");
                context.Response.Write("<META http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                context.Response.Write("<!--[if gte mso 9]><xml>");//添加下面这一段就会又excel的线条
                context.Response.Write("<x:ExcelWorkbook>");
                context.Response.Write("<x:ExcelWorksheets>");
                context.Response.Write("<x:ExcelWorksheet>");
                context.Response.Write("<x:Name>Report Data</x:Name>");
                context.Response.Write("<x:WorksheetOptions>");
                context.Response.Write("<x:Print>");
                context.Response.Write("<x:ValidPrinterInfo/>");
                context.Response.Write("</x:Print>");
                context.Response.Write("</x:WorksheetOptions>");
                context.Response.Write("</x:ExcelWorksheet>");
                context.Response.Write("</x:ExcelWorksheets>");
                context.Response.Write("</x:ExcelWorkbook>");
                context.Response.Write("</xml>");
                context.Response.Write("<![endif]--> ");
                context.Response.Write(tblHtml.ToString());//HTML
                context.Response.Flush();
                context.Response.End();
            }
            #endregion

            #region 客人信息变更记录
            if (action == "getRecordPage_GuestChangeLog")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                string beginDate = context.Request.Params["beginDate"];
                string endDate = context.Request.Params["endDate"];
                string name = context.Request.Params["name"];
                string changeType = context.Request.Params["changeType"];
                StringBuilder strWhere = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    strWhere.AppendFormat(" and b.Name like '%{0}%' ", name);
                }
                if (!string.IsNullOrWhiteSpace(changeType) && changeType != "null")
                {
                    strWhere.AppendFormat(" and a.ChangeType = '{0}' ", changeType);
                }
                if (!string.IsNullOrWhiteSpace(beginDate))
                {
                    strWhere.AppendFormat(" and ModifyOn>= '{0}' ", beginDate);
                }
                if (!string.IsNullOrWhiteSpace(endDate))
                {
                    endDate = Convert.ToDateTime(endDate).AddDays(1).ToString("yyyy-MM-dd");
                    strWhere.AppendFormat(" and ModifyOn< '{0}' ", endDate);
                }
                result = getRecordPage_GuestChangeLog(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 员工异动记录
            if (action == "getRecordPage_StaffTransfer")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                string beginDate = context.Request.Params["beginDate"];
                string endDate = context.Request.Params["endDate"];
                string name = context.Request.Params["name"];
                StringBuilder strWhere = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    strWhere.AppendFormat(" and b.Name like '%{0}%' ", name);
                }
                if (!string.IsNullOrWhiteSpace(beginDate))
                {
                    strWhere.AppendFormat(" and ModifyOn>= '{0}' ", beginDate);
                }
                if (!string.IsNullOrWhiteSpace(endDate))
                {
                    endDate = Convert.ToDateTime(endDate).AddDays(1).ToString("yyyy-MM-dd");
                    strWhere.AppendFormat(" and ModifyOn< '{0}' ", endDate);
                }
                result = getRecordPage_StaffTransfer(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 获取客人关系列表
            if (action == "getRecordPage_GuestConnection")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string guestName = context.Request.Params["guestName"];
                if (!string.IsNullOrWhiteSpace(guestName))
                {
                    strWhere.AppendFormat(" and b.Name like '%{0}%' ", guestName);
                }
                result = getRecordPage_GuestConnection(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 保存客人关系
            if (action == "SaveGuestConnection")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var guestID = context.Request.Params["guestID"];
                var connectStaffNo = context.Request.Params["connectStaffNo"];
                var connectGuestID = context.Request.Params["connectGuestID"];
                var otherConnectName = context.Request.Params["otherConnectName"];
                var connectionType = context.Request.Params["connectionType"];
                var remark = context.Request.Params["remark"];
                GuestConnection model = new GuestConnection();
                model.GuestID = guestID;
                model.ConnectStaffNo = connectStaffNo;
                model.ConnectGuestID = connectGuestID;
                model.OtherConnectName = otherConnectName;
                model.ConnectionType = connectionType;
                model.Remark = remark;
                int excuteResult = 0;
                if (saveflag == "add")
                {
                    excuteResult = dal.AddGuestConnection(model);
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    excuteResult = dal.UpdateGuestConnection(model);
                }
                result = excuteResult.ToString();
            }
            #endregion

            #region 根据ID获取客人关系
            if (action == "GetGuestConnectionByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dal.GetGuestConnectionByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 删除客人关系
            if (action == "DeleteGuestConnection")
            {
                var ID = context.Request.Params["ID"];
                var deleteResult = dal.DeleteGuestConnection(Convert.ToInt16(ID));
                result = deleteResult.ToString();
            }
            #endregion

            #region 根据ID获取客人评估模板信息
            if (action == "GetEvaluateTemplateByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dal.GetEvaluateTemplateByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 根据条件获取客人评估模板信息
            if (action == "GetEvaluateTemplateByWhere")
            {
                string strWhere = string.Empty;
                strWhere = " and Status=1 ";
                DataSet ds = new DataSet();
                ds = dal.GetEvaluateTemplateByWhere(strWhere);
                StringBuilder strOpt = new StringBuilder();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    strOpt.Append("<option value=''>请选择</option>");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        strOpt.AppendFormat("<option value='{0}'>{1}</option>", ds.Tables[0].Rows[i]["ID"], ds.Tables[0].Rows[i]["Name"]);
                    }
                }
                result = strOpt.ToString();
            }
            #endregion

            #region 保存客人评估模板
            if (action == "SaveEvaluateTemplate")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var name = context.Request.Params["name"];
                var remark = context.Request.Params["remark"];
                var status = context.Request.Params["status"];
                var details = context.Request.Params["details"];
                List<EvaluateTemplateDetail> listEvaluateTemplateDetail = new List<EvaluateTemplateDetail>();
                if (!string.IsNullOrWhiteSpace(details))
                {
                    listEvaluateTemplateDetail = CommTools.JsonToObject(details, typeof(List<EvaluateTemplateDetail>)) as List<EvaluateTemplateDetail>;
                }
                EvaluateTemplate model = new EvaluateTemplate();
                model.Name = name;
                model.Remark = remark;
                model.Status = Convert.ToInt16(status);
                model.ListEvaluateTemplateDetail = listEvaluateTemplateDetail;
                string excuteResult = string.Empty;
                if (saveflag == "add")
                {
                    var existsResult = dal.ExistsEvaluateTemplate(model);
                    if (existsResult)
                    {
                        excuteResult = "exists";
                    }
                    else
                    {
                        excuteResult = dal.AddEvaluateTemplate(model).ToString();
                    }
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    var existsResult = dal.ExistsEvaluateTemplate(model);
                    if (existsResult)
                    {
                        excuteResult = "exists";
                    }
                    else if (dal.ExistsHasGuestEvaluate(model))
                    {
                        excuteResult = "existsUsing";
                    }
                    else
                    {
                        excuteResult = dal.UpdateEvaluateTemplate(model).ToString();
                    }
                }

                result = excuteResult;
            }
            #endregion

            #region 获取客人评估模板列表
            if (action == "getRecordPage_EvaluateTemplate")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string queryName = context.Request.Params["queryName"];
                if (!string.IsNullOrWhiteSpace(queryName))
                {
                    strWhere.AppendFormat(" and Name like '%{0}%' ", queryName);
                }
                result = getRecordPage_EvaluateTemplate(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 删除客人评估模板
            if (action == "DeleteEvaluateTemplate")
            {
                var excuteResult = "";
                var ID = context.Request.Params["ID"];
                var model = new EvaluateTemplate();
                model.ID = Convert.ToInt16(ID);
                if (dal.ExistsHasGuestEvaluate(model))
                {
                    excuteResult = "existsUsing";
                }
                else
                {
                    excuteResult = dal.DeleteEvaluateTemplate(Convert.ToInt16(ID)).ToString();
                }
                result = excuteResult;
            }
            #endregion

            #region 获取客人照护需求评估列表
            if (action == "getRecordPage_GuestEvaluate")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string queryName = context.Request.Params["queryName"];
                string evaluateDate = context.Request.Params["evaluateDate"];
                if (!string.IsNullOrWhiteSpace(queryName))
                {
                    strWhere.AppendFormat(" and b.`Name` like '%{0}%' ", queryName);
                }
                if (!string.IsNullOrWhiteSpace(evaluateDate))
                {
                    var where_beginDate = Convert.ToDateTime(evaluateDate + "-01");
                    var where_endDate = where_beginDate.AddMonths(1).AddDays(-1);
                    strWhere.AppendFormat(" and a.EvaluateDate >='{0}' and a.EvaluateDate <='{1}' ", where_beginDate, where_endDate);
                }
                result = getRecordPage_GuestEvaluate(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 保存客人照护需求评估
            if (action == "SaveGuestEvaluate")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var guestID = context.Request.Params["guestID"];
                var evaluateType = context.Request.Params["evaluateType"];
                var evaluateTemplateID = context.Request.Params["evaluateTemplateID"];
                var evaluateDate = context.Request.Params["evaluateDate"];
                var score = context.Request.Params["score"].TrimEnd(',');
                var status = context.Request.Params["status"];
                var remark = context.Request.Params["remark"];
                var suggestNurseLevel = context.Request.Params["suggestNurseLevel"];
                GuestEvaluate model = new GuestEvaluate();
                model.GuestID = Convert.ToInt16(guestID);
                model.EvaluateType = evaluateType;
                model.EvaluateTemplateID = Convert.ToInt16(evaluateTemplateID);
                model.EvaluateDate = Convert.ToDateTime(evaluateDate);
                model.Score = score;
                model.Status = Convert.ToInt16(status);
                model.Remark = remark;
                model.SuggestNurseLevel = suggestNurseLevel;
                string excuteResult = string.Empty;
                if (saveflag == "add")
                {
                    excuteResult = dal.AddGuestEvaluate(model).ToString();
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    excuteResult = dal.UpdateGuestEvaluate(model).ToString();
                }
                result = excuteResult;
            }
            #endregion

            #region 根据ID获取客人照护需求评估
            if (action == "GetGuestEvaluateByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dal.GetGuestEvaluateByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 删除客人照护需求评估
            if (action == "DeleteGuestEvaluate")
            {
                var ID = context.Request.Params["ID"];
                var deleteResult = dal.DeleteGuestEvaluate(Convert.ToInt16(ID));
                result = deleteResult.ToString();
            }
            #endregion

            #region 获取客人照护项目列表
            if (action == "getRecordPage_NurseProject")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string queryNurseName = context.Request.Params["queryNurseName"];
                if (!string.IsNullOrWhiteSpace(queryNurseName))
                {
                    strWhere.AppendFormat(" and NurseName like '%{0}%' ", queryNurseName);
                }
                result = getRecordPage_NurseProject(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 保存客人照护项目
            if (action == "SaveNurseProject")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var nurseName = context.Request.Params["nurseName"];
                var nurseType = context.Request.Params["nurseType"];
                var requirement = context.Request.Params["requirement"];
                NurseProject model = new NurseProject();
                model.NurseName = nurseName;
                model.NurseType = nurseType;
                model.Requirement = requirement;
                string excuteResult = string.Empty;
                if (saveflag == "add")
                {
                    var existsResult = dal.ExistsNurseProject(model);
                    if (existsResult)
                    {
                        excuteResult = "exists";
                    }
                    else
                    {
                        excuteResult = dal.AddNurseProject(model).ToString();
                    }
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    var existsResult = dal.ExistsNurseProject(model);
                    if (existsResult)
                    {
                        excuteResult = "exists";
                    }
                    else
                    {
                        excuteResult = dal.UpdateNurseProject(model).ToString();
                    }
                }
                result = excuteResult.ToString();
            }
            #endregion

            #region 根据ID获取客人照护项目
            if (action == "GetNurseProjectByID")
            {
                var ID = context.Request.Params["ID"];
                var floor = dal.GetNurseProjectByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(floor);
            }
            #endregion

            #region 删除客人照护项目
            if (action == "DeleteNurseProject")
            {
                var ID = context.Request.Params["ID"];
                var floor = dal.DeleteNurseProject(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(floor);
            }
            #endregion

            #region 根据条件获取客人照护项目
            if (action == "GetNurseProjectInfo")
            {
                DataSet ds = new DataSet();
                ds = dal.GetNurseProjectInfo();
                StringBuilder strOpt = new StringBuilder();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    strOpt.Append("<option value=''>请选择</option>");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        strOpt.AppendFormat("<option value='{0}' requirement='{3}' nurseName='{2}' nurseType='{1}'>{2}</option>", ds.Tables[0].Rows[i]["ID"], ds.Tables[0].Rows[i]["NurseType"], ds.Tables[0].Rows[i]["NurseName"], ds.Tables[0].Rows[i]["Requirement"]);
                    }
                }
                result = strOpt.ToString();
            }
            #endregion

            #region 获取客人照护计划列表
            if (action == "getRecordPage_GuestNursePlan")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string queryName = context.Request.Params["queryName"];
                if (!string.IsNullOrWhiteSpace(queryName))
                {
                    strWhere.AppendFormat(" and b.`Name` like '%{0}%' ", queryName);
                }
                result = getRecordPage_GuestNursePlan(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 保存客人计划评估
            if (action == "SaveGuestNursePlan")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var guestID = context.Request.Params["guestID"];
                var beginDate = context.Request.Params["beginDate"];
                var status = context.Request.Params["status"];
                var remark = context.Request.Params["remark"];
                var detailNurseProjectID = context.Request.Params["detailNurseProjectID"].TrimEnd('~');
                var detailRemark = context.Request.Params["detailRemark"].TrimEnd('~');
                GuestNursePlan model = new GuestNursePlan();
                model.GuestID = Convert.ToInt16(guestID);
                model.BeginDate = Convert.ToDateTime(beginDate);
                model.Remark = remark;
                model.Status = Convert.ToInt16(status);
                List<GuestNursePlanDetail> details = new List<GuestNursePlanDetail>();
                for (int i = 0; i < detailNurseProjectID.Split('~').Length; i++)
                {
                    details.Add(new GuestNursePlanDetail()
                    {
                        NurseProjectID = Convert.ToInt16(detailNurseProjectID.Split('~')[i]),
                        Remark = detailRemark.Split('~')[i]
                    });
                }
                model.ListGuestNursePlanDetail = details;
                string excuteResult = string.Empty;
                if (saveflag == "add")
                {
                    excuteResult = dal.AddGuestNursePlan(model).ToString();
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    excuteResult = dal.UpdateGuestNursePlan(model).ToString();
                }
                result = excuteResult;
            }
            #endregion

            #region 根据ID获取客人计划
            if (action == "GetGuestNursePlanByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dal.GetGuestNursePlanByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 删除客人计划
            if (action == "DeleteGuestNursePlan")
            {
                var ID = context.Request.Params["ID"];
                var deleteResult = dal.DeleteGuestNursePlan(Convert.ToInt16(ID));
                result = deleteResult.ToString();
            }
            #endregion

            #region 获取产品类型列表
            if (action == "getRecordPage_ProductType")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string queryName = context.Request.Params["queryName"];
                if (!string.IsNullOrWhiteSpace(queryName))
                {
                    strWhere.AppendFormat(" and ProductTypeName like '%{0}%' ", queryName);
                }
                result = getRecordPage_ProductType(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 保存产品类型
            if (action == "SaveProductType")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var productTypeNo = context.Request.Params["productTypeNo"];
                var productTypeName = context.Request.Params["productTypeName"];
                var mainProductType = context.Request.Params["mainProductType"];
                var remark = context.Request.Params["remark"];
                ProductType model = new ProductType();
                model.ProductTypeNo = productTypeNo;
                model.ProductTypeName = productTypeName;
                model.MainProductType = mainProductType;
                model.Remark = remark;
                int excuteResult = 0;
                if (saveflag == "add")
                {
                    excuteResult = dal.AddProductType(model);
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    excuteResult = dal.UpdateProductType(model);
                }
                result = excuteResult.ToString();
            }
            #endregion

            #region 根据ID获取产品类型
            if (action == "GetProductTypeByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dal.GetProductTypeByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 绑定产品类型下拉框
            if (action == "InitSelect_ProductType")
            {
                DataSet ds = new DataSet();
                ds = dal.GetProductType();
                StringBuilder strOpt = new StringBuilder();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    strOpt.Append("<option value=''>请选择</option>");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        strOpt.AppendFormat("<option value='{0}' MainProductType='{2}'>{1}</option>", ds.Tables[0].Rows[i]["ID"],
                            ds.Tables[0].Rows[i]["ProductTypeName"], ds.Tables[0].Rows[i]["MainProductType"]);
                    }
                }
                result = strOpt.ToString();
            }
            #endregion

            #region 获取产品列表
            if (action == "getRecordPage_Product")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string queryName = context.Request.Params["queryName"];
                if (!string.IsNullOrWhiteSpace(queryName))
                {
                    strWhere.AppendFormat(" and a.ProductName like '%{0}%' ", queryName);
                }
                result = getRecordPage_Product(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 保存产品
            if (action == "SaveProduct")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var productNo = context.Request.Params["productNo"];
                var productTypeID = context.Request.Params["productTypeID"];
                var productName = context.Request.Params["productName"];
                var status = context.Request.Params["status"];
                var remark = context.Request.Params["remark"];
                Product model = new Product();
                model.ProductNo = productNo;
                model.ProductTypeID = Convert.ToInt16(productTypeID);
                model.ProductName = productName;
                model.Status = Convert.ToInt16(status);
                model.Remark = remark;
                int excuteResult = 0;
                if (saveflag == "add")
                {
                    //excuteResult = dal.AddProduct(model);

                    excuteResult = new AoHeDalGlobal().Insert_Factory(model);

                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    excuteResult = dal.UpdateProduct(model);
                }
                result = excuteResult.ToString();
            }
            #endregion

            #region 根据ID获取产品
            if (action == "GetProductByID")
            {
                var ID = context.Request.Params["ID"];
                //var staff = dal.GetProductByID(Convert.ToInt16(ID));

                var staff = new AoHeDalGlobal().GetModel_Factory<Product>(Convert.ToInt16(ID));

                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 删除产品
            if (action == "DeleteProduct")
            {
                var ID = context.Request.Params["ID"];
                var deleteResult = dal.DeleteProduct(Convert.ToInt16(ID));
                result = deleteResult.ToString();
            }
            #endregion

            #region 绑定产品下拉框
            if (action == "InitSelect_Product")
            {
                DataSet ds = new DataSet();
                ds = dal.GetProduct();
                StringBuilder strOpt = new StringBuilder();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    var mainProductType = context.Request.Params["mainProductType"];
                    DataRow[] rows;
                    if (!string.IsNullOrWhiteSpace(mainProductType))
                    {
                        rows = ds.Tables[0].Select(" MainProductType='" + mainProductType + "' ");
                    }
                    else
                    {
                        rows = ds.Tables[0].Select();
                    }
                    strOpt.Append("<option value=''>请选择</option>");
                    for (int i = 0; i < rows.Length; i++)
                    {
                        strOpt.AppendFormat("<option value='{0}' ProductNo='{3}' ProductTypeName='{2}'>{1}</option>", rows[i]["ID"],
                            rows[i]["ProductName"], rows[i]["ProductTypeName"], rows[i]["ProductNo"]);
                    }
                }
                result = strOpt.ToString();
            }
            #endregion

            #region 获取产品服务对象列表
            if (action == "getRecordPage_ProductServiceObject")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string queryName = context.Request.Params["queryName"];
                if (!string.IsNullOrWhiteSpace(queryName))
                {
                    strWhere.AppendFormat(" and b.ProductName like '%{0}%' ", queryName);
                }
                result = getRecordPage_ProductServiceObject(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 保存产品服务对象
            if (action == "SaveProductServiceObject")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var productID = context.Request.Params["productID"];
                var nurseLevel = context.Request.Params["nurseLevel"];
                var objectRemark = context.Request.Params["objectRemark"];
                var effectRemark = context.Request.Params["effectRemark"];
                var remark = context.Request.Params["remark"];
                ProductServiceObject model = new ProductServiceObject();
                model.ProductID = Convert.ToInt16(productID);
                model.NurseLevel = nurseLevel;
                model.ObjectRemark = objectRemark;
                model.EffectRemark = effectRemark;
                model.Remark = remark;
                int excuteResult = 0;
                if (saveflag == "add")
                {
                    excuteResult = dal.AddProductServiceObject(model);
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    excuteResult = dal.UpdateProductServiceObject(model);
                }
                result = excuteResult.ToString();
            }
            #endregion

            #region 根据ID获取产品服务对象
            if (action == "GetProductServiceObjectByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dal.GetProductServiceObjectByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 删除产品服务对象
            if (action == "DeleteProductServiceObject")
            {
                var ID = context.Request.Params["ID"];
                var deleteResult = dal.DeleteProductServiceObject(Convert.ToInt16(ID));
                result = deleteResult.ToString();
            }
            #endregion

            #region 获取产品技能要求列表
            if (action == "getRecordPage_ProductSkillRequired")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string queryName = context.Request.Params["queryName"];
                if (!string.IsNullOrWhiteSpace(queryName))
                {
                    strWhere.AppendFormat(" and b.ProductName like '%{0}%' ", queryName);
                }
                result = getRecordPage_ProductSkillRequired(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 保存产品技能要求
            if (action == "SaveProductSkillRequired")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var productID = context.Request.Params["productID"];
                var skillRequired = context.Request.Params["skillRequired"];
                var requiredRemark = context.Request.Params["requiredRemark"];
                var remark = context.Request.Params["remark"];
                ProductSkillRequired model = new ProductSkillRequired();
                model.ProductID = Convert.ToInt16(productID);
                model.SkillRequired = skillRequired;
                model.RequiredRemark = requiredRemark;
                model.Remark = remark;
                int excuteResult = 0;
                if (saveflag == "add")
                {
                    excuteResult = dal.AddProductSkillRequired(model);
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    excuteResult = dal.UpdateProductSkillRequired(model);
                }
                result = excuteResult.ToString();
            }
            #endregion

            #region 根据ID获取产品技能要求
            if (action == "GetProductSkillRequiredByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dal.GetProductSkillRequiredByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 删除产品技能要求
            if (action == "DeleteProductSkillRequired")
            {
                var ID = context.Request.Params["ID"];
                var deleteResult = dal.DeleteProductSkillRequired(Convert.ToInt16(ID));
                result = deleteResult.ToString();
            }
            #endregion

            #region 获取产品价格列表
            if (action == "getRecordPage_ProductPrice")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string queryName = context.Request.Params["queryName"];
                if (!string.IsNullOrWhiteSpace(queryName))
                {
                    strWhere.AppendFormat(" and b.ProductName like '%{0}%' ", queryName);
                }
                result = getRecordPage_ProductPrice(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 保存产品价格
            if (action == "SaveProductPrice")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var productID = context.Request.Params["productID"];
                var amount = context.Request.Params["amount"];
                var beginDate = context.Request.Params["beginDate"];
                var endDate = context.Request.Params["endDate"];
                var status = context.Request.Params["status"];
                var remark = context.Request.Params["remark"];
                ProductPrice model = new ProductPrice();
                model.ProductID = Convert.ToInt16(productID);
                model.Amount = Convert.ToDecimal(amount);
                model.BeginDate = Convert.ToDateTime(beginDate);
                model.EndDate = Convert.ToDateTime(endDate);
                model.Status = Convert.ToInt16(status);
                model.Remark = remark;
                int excuteResult = 0;
                if (saveflag == "add")
                {
                    excuteResult = dal.AddProductPrice(model);
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    excuteResult = dal.UpdateProductPrice(model);
                }
                result = excuteResult.ToString();
            }
            #endregion

            #region 根据ID获取产品价格
            if (action == "GetProductPriceByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dal.GetProductPriceByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 删除产品价格
            if (action == "DeleteProductPrice")
            {
                var ID = context.Request.Params["ID"];
                var deleteResult = dal.DeleteProductPrice(Convert.ToInt16(ID));
                result = deleteResult.ToString();
            }
            #endregion

            #region 获取服务预订列表
            if (action == "getRecordPage_ServiceReserve")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string queryName = context.Request.Params["queryName"];
                if (!string.IsNullOrWhiteSpace(queryName))
                {
                    strWhere.AppendFormat(" and b.ProductName like '%{0}%' ", queryName);
                }
                result = getRecordPage_ServiceReserve(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 保存服务预订
            if (action == "SaveServiceReserve")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var productID = context.Request.Params["productID"];
                var guestID = context.Request.Params["guestID"];
                var applyDate = context.Request.Params["applyDate"];
                var beginDate = context.Request.Params["beginDate"];
                var endDate = context.Request.Params["endDate"];
                var status = context.Request.Params["status"];
                //var payStatus = context.Request.Params["payStatus"];
                var remark = context.Request.Params["remark"];
                ServiceReserve model = new ServiceReserve();
                model.ProductID = Convert.ToInt16(productID);
                model.GuestID = Convert.ToInt16(guestID);
                model.ApplyDate = Convert.ToDateTime(applyDate);
                model.BeginDate = Convert.ToDateTime(beginDate);
                model.EndDate = Convert.ToDateTime(endDate);
                model.Status = Convert.ToInt16(status);
                model.PayStatus = 1;//缴费状态默认为正常
                model.Remark = remark;
                int excuteResult = 0;
                if (saveflag == "add")
                {
                    excuteResult = dal.AddServiceReserve(model);
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    excuteResult = dal.UpdateServiceReserve(model);
                }
                result = excuteResult.ToString();
            }
            #endregion

            #region 根据ID获取服务预订
            if (action == "GetServiceReserveByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dal.GetServiceReserveByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 删除服务预订
            if (action == "DeleteServiceReserve")
            {
                var ID = context.Request.Params["ID"];
                var deleteResult = dal.DeleteServiceReserve(Convert.ToInt16(ID));
                result = deleteResult.ToString();
            }
            #endregion

            #region 获取服务执行列表
            if (action == "getRecordPage_ServiceExecute")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string queryName = context.Request.Params["queryName"];
                string serviceReserveID = context.Request.Params["serviceReserveID"];
                if (!string.IsNullOrWhiteSpace(serviceReserveID))
                {
                    strWhere.AppendFormat(" and ServiceReserveID ='{0}' ", serviceReserveID);
                }
                if (!string.IsNullOrWhiteSpace(queryName))
                {
                    strWhere.AppendFormat(" and b.Name like '%{0}%' ", queryName);
                }
                result = getRecordPage_ServiceExecute(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 保存服务执行
            if (action == "SaveServiceExecute")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var serviceReserveID = context.Request.Params["serviceReserveID"];
                var serviceDate = context.Request.Params["serviceDate"];
                var serviceStaff = context.Request.Params["serviceStaff"];
                var remark = context.Request.Params["remark"];
                ServiceExecute model = new ServiceExecute();
                model.ServiceReserveID = Convert.ToInt16(serviceReserveID);
                model.ServiceDate = Convert.ToDateTime(serviceDate);
                model.ServiceStaff = serviceStaff;
                model.Remark = remark;
                int excuteResult = 0;
                if (saveflag == "add")
                {
                    excuteResult = dal.AddServiceExecute(model);
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    excuteResult = dal.UpdateServiceExecute(model);
                }
                result = excuteResult.ToString();
            }
            #endregion

            #region 根据ID获取服务执行
            if (action == "GetServiceExecuteByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dal.GetServiceExecuteByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 删除服务执行
            if (action == "DeleteServiceExecute")
            {
                var ID = context.Request.Params["ID"];
                var deleteResult = dal.DeleteServiceExecute(Convert.ToInt16(ID));
                result = deleteResult.ToString();
            }
            #endregion

            #region 根据条件获取服务预订详情
            if (action == "getServiceReserveByWhere")
            {
                string serviceReserveID = context.Request.Params["serviceReserveID"];
                StringBuilder strWhere = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(serviceReserveID))
                {
                    strWhere.AppendFormat(" and a.ID = '{0}' ", serviceReserveID);
                }
                result = getServiceReserveInfoHtml(strWhere.ToString());
                strWhere = null;
            }
            #endregion

            #region 获取服务评价列表
            if (action == "getRecordPage_ServiceEvaluation")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string queryName = context.Request.Params["queryName"];
                if (!string.IsNullOrWhiteSpace(queryName))
                {
                    strWhere.AppendFormat(" and b.Name like '%{0}%' ", queryName);
                }
                result = getRecordPage_ServiceEvaluation(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 保存服务评价
            if (action == "SaveServiceEvaluation")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var guestID = context.Request.Params["guestID"];
                var serviceExecuteID = context.Request.Params["serviceExecuteID"];
                var rater = context.Request.Params["rater"];
                var remark = context.Request.Params["remark"];
                ServiceEvaluation model = new ServiceEvaluation();
                model.GuestID = Convert.ToInt16(guestID);
                model.ServiceExecuteID = Convert.ToInt16(serviceExecuteID);
                model.EvaluateDate = DateTime.Now;
                model.Rater = rater;
                model.Remark = remark;
                int excuteResult = 0;
                if (saveflag == "add")
                {
                    excuteResult = dal.AddServiceEvaluation(model);
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    excuteResult = dal.UpdateServiceEvaluation(model);
                }
                result = excuteResult.ToString();
            }
            #endregion

            #region 根据ID获取服务评价
            if (action == "GetServiceEvaluationByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dal.GetServiceEvaluationInfoByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 删除服务评价
            if (action == "DeleteServiceEvaluation")
            {
                var ID = context.Request.Params["ID"];
                var deleteResult = dal.DeleteServiceEvaluation(Convert.ToInt16(ID));
                result = deleteResult.ToString();
            }
            #endregion

            #region 获取服务收费列表
            if (action == "getRecordPage_ServicePay")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string queryDate = context.Request.Params["queryDate"];
                if (!string.IsNullOrWhiteSpace(queryDate))
                {
                    strWhere.AppendFormat(" and a.PayDate = '{0}' ", Convert.ToDateTime(queryDate).ToString("yyyy-MM-dd"));
                }
                result = getRecordPage_ServicePay(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 保存服务收费
            if (action == "SaveServicePay")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var serviceReserveID = context.Request.Params["serviceReserveID"];
                var amount = context.Request.Params["amount"];
                var payDate = context.Request.Params["payDate"];
                var beginDate = context.Request.Params["beginDate"];
                var endDate = context.Request.Params["endDate"];
                ServicePay model = new ServicePay();
                model.ServiceReserveID = Convert.ToInt16(serviceReserveID);
                model.Amount = Convert.ToDecimal(amount);
                model.PayDate = Convert.ToDateTime(payDate);
                model.BeginDate = Convert.ToDateTime(beginDate);
                model.EndDate = Convert.ToDateTime(endDate);
                int excuteResult = 0;
                if (saveflag == "add")
                {
                    excuteResult = dal.AddServicePay(model);
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    excuteResult = dal.UpdateServicePay(model);
                }
                result = excuteResult.ToString();
            }
            #endregion

            #region 根据ID获取服务收费
            if (action == "GetServicePayByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dal.GetServicePayByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 删除服务收费
            if (action == "DeleteServicePay")
            {
                var ID = context.Request.Params["ID"];
                var deleteResult = dal.DeleteServicePay(Convert.ToInt16(ID));
                result = deleteResult.ToString();
            }
            #endregion

            #region 获取客人所有服务执行
            if (action == "GetServiceExecuteByGuestID")
            {
                var guestID = context.Request.Params["guestID"];
                DataSet ds = new DataSet();
                ds = dal.GetServiceExecuteByGuestID(Convert.ToInt16(guestID));
                StringBuilder strOpt = new StringBuilder();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    strOpt.Append("<option value=''>请选择</option>");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        var row = ds.Tables[0].Rows[i];
                        strOpt.AppendFormat("<option value='{0}' ServiceStaff='{3}' ProductName='{2}'>{1}</option>", row["ID"],
                          Convert.ToDateTime(row["ServiceDate"]).ToString("yyyy-MM-dd"), row["ProductName"], row["Name"]);
                    }
                }
                result = strOpt.ToString();
            }
            #endregion

            #region 获取活动排班列表
            if (action == "getRecordPage_ActivitySchedual")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string queryName = context.Request.Params["queryName"];
                if (!string.IsNullOrWhiteSpace(queryName))
                {
                    strWhere.AppendFormat(" and b.ProductName like '%{0}%' ", queryName);
                }
                result = getRecordPage_ActivitySchedual(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 保存活动排班
            if (action == "SaveActivitySchedual")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var productID = context.Request.Params["productID"];
                var activityDate = context.Request.Params["activityDate"];
                var beginDate = context.Request.Params["beginDate"];
                var endDate = context.Request.Params["endDate"];
                var place = context.Request.Params["place"];
                var activityFee = context.Request.Params["activityFee"];
                var activityManager = context.Request.Params["activityManager"];
                var activityInfo = context.Request.Params["activityInfo"];
                var activityExecuteInfo = context.Request.Params["activityExecuteInfo"];
                var status = context.Request.Params["status"];
                var remark = context.Request.Params["remark"];
                ActivitySchedual model = new ActivitySchedual();
                model.ProductID = Convert.ToInt16(productID);
                model.ActivityDate = Convert.ToDateTime(activityDate);
                model.BeginDate = Convert.ToDateTime(beginDate);
                model.EndDate = Convert.ToDateTime(endDate);
                model.Place = place;
                model.ActivityFee = string.IsNullOrWhiteSpace(activityFee) ? 0 : Convert.ToDecimal(activityFee);
                model.ActivityManager = activityManager;
                model.ActivityInfo = activityInfo;
                model.ActivityExecuteInfo = activityExecuteInfo;

                model.Status = Convert.ToInt16(status);
                model.PayStatus = 0;//缴费状态默认为未完成
                model.ModifyOn = DateTime.Now;
                model.Remark = remark;
                int excuteResult = 0;
                if (saveflag == "add")
                {
                    excuteResult = dal.AddActivitySchedual(model);
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    excuteResult = dal.UpdateActivitySchedual(model);
                }
                result = excuteResult.ToString();
            }
            #endregion

            #region 根据ID获取活动排班
            if (action == "GetActivitySchedualByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dal.GetActivitySchedualByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 删除活动排班
            if (action == "DeleteActivitySchedual")
            {
                var ID = context.Request.Params["ID"];
                var deleteResult = dal.DeleteActivitySchedual(Convert.ToInt16(ID));
                result = deleteResult.ToString();
            }
            #endregion

            #region 获取活动预约列表
            if (action == "getRecordPage_ActivityReserve")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string queryName = context.Request.Params["queryName"];
                string activitySchedualID = context.Request.Params["activitySchedualID"];
                if (!string.IsNullOrWhiteSpace(activitySchedualID))
                {
                    strWhere.AppendFormat(" and ActivitySchedualID ='{0}' ", activitySchedualID);
                }
                if (!string.IsNullOrWhiteSpace(queryName))
                {
                    strWhere.AppendFormat(" and b.Name like '%{0}%' ", queryName);
                }
                result = getRecordPage_ActivityReserve(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 保存活动预约
            if (action == "SaveActivityReserve")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var activitySchedualID = context.Request.Params["activitySchedualID"];
                var guestID = context.Request.Params["guestID"];
                var applyDate = context.Request.Params["applyDate"];
                var remark = context.Request.Params["remark"];

                ActivityReserve model = new ActivityReserve();
                model.ActivitySchedualID = Convert.ToInt16(activitySchedualID);
                model.GuestID = Convert.ToInt16(guestID);
                model.ApplyDate = Convert.ToDateTime(applyDate);
                model.Status = 0;
                model.Remark = remark;
                model.ModifyOn = DateTime.Now;
                model.PayStatus = 0;

                int excuteResult = 0;
                if (saveflag == "add")
                {
                    excuteResult = dal.AddActivityReserve(model);
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    excuteResult = dal.UpdateActivityReserve(model);
                }
                result = excuteResult.ToString();
            }
            #endregion

            #region 根据ID获取活动预约
            if (action == "GetActivityReserveByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dal.GetActivityReserveByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 删除活动预约
            if (action == "DeleteActivityReserve")
            {
                var ID = context.Request.Params["ID"];
                var deleteResult = dal.DeleteActivityReserve(Convert.ToInt16(ID));
                result = deleteResult.ToString();
            }
            #endregion

            #region 根据条件获取活动排班详情
            if (action == "getActivitySchedualByWhere")
            {
                string activitySchedualID = context.Request.Params["activitySchedualID"];
                StringBuilder strWhere = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(activitySchedualID))
                {
                    strWhere.AppendFormat(" and a.ID = '{0}' ", activitySchedualID);
                }
                result = getActivitySchedualInfoHtml(strWhere.ToString());
                strWhere = null;
            }
            #endregion

            #region 活动执行
            if (action == "SaveActivityExecute")
            {
                var ID = context.Request.Params["ID"];
                var status = context.Request.Params["status"];
                var participation = context.Request.Params["participation"];

                ActivityReserve model = new ActivityReserve();
                model.Status = Convert.ToInt16(status);
                model.Participation = participation;
                model.ModifyOn = DateTime.Now;

                int excuteResult = 0;
                model.ID = Convert.ToInt16(ID);
                excuteResult = dal.UpdateActivityReserveExecute(model);

                result = excuteResult.ToString();
            }
            #endregion

            #region 活动缴费
            if (action == "SaveActivityPay")
            {
                var ID = context.Request.Params["ID"];
                var payStatus = context.Request.Params["payStatus"];
                var payDate = context.Request.Params["payDate"];

                ActivityReserve model = new ActivityReserve();
                model.PayStatus = Convert.ToInt16(payStatus);
                model.PayDate = Convert.ToDateTime(payDate);
                model.ModifyOn = DateTime.Now;

                int excuteResult = 0;
                model.ID = Convert.ToInt16(ID);
                excuteResult = dal.UpdateActivityReservePay(model);

                result = excuteResult.ToString();
            }
            #endregion

            #region 获取物料类型列表
            if (action == "getRecordPage_MaterielType")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string queryName = context.Request.Params["queryName"];
                if (!string.IsNullOrWhiteSpace(queryName))
                {
                    strWhere.AppendFormat(" and TypeName like '%{0}%' ", queryName);
                }
                result = getRecordPage_MaterielType(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 保存物料类型
            if (action == "SaveMaterielType")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var typeNo = context.Request.Params["typeNo"];
                var typeName = context.Request.Params["typeName"];
                var remark = context.Request.Params["remark"];
                MaterielType model = new MaterielType();
                model.TypeNo = typeNo;
                model.TypeName = typeName;
                model.Remark = remark;   
                string excuteResult = string.Empty;
                if (saveflag == "add")
                {
                    var existsResult = dalGlobal.Exists_Factory<MaterielType>(model);
                    if (existsResult)
                    {
                        excuteResult = "exists";
                    }
                    else
                    {
                        excuteResult = dalGlobal.Insert_Factory<MaterielType>(model).ToString();
                    }
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    var existsResult = dalGlobal.Exists_Factory<MaterielType>(model);
                    if (existsResult)
                    {
                        excuteResult = "exists";
                    }
                    else
                    {
                        excuteResult = dalGlobal.Update_Factory<MaterielType>(model).ToString();
                    }
                }
                result = excuteResult.ToString();
            }
            #endregion

            #region 根据ID获取物料类型
            if (action == "GetMaterielTypeByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dalGlobal.GetModel_Factory<MaterielType>(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 获取供应商列表
            if (action == "getRecordPage_Supplier")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string queryName = context.Request.Params["queryName"];
                if (!string.IsNullOrWhiteSpace(queryName))
                {
                    strWhere.AppendFormat(" and Name like '%{0}%' ", queryName);
                }
                result = getRecordPage_Supplier(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 保存供应商
            if (action == "SaveSupplier")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var shortName = context.Request.Params["shortName"];
                var name = context.Request.Params["name"];
                var contactName = context.Request.Params["contactName"];
                var phone = context.Request.Params["phone"];
                var status = context.Request.Params["status"];
                var address = context.Request.Params["address"];
                var remark = context.Request.Params["remark"];
                Supplier model = new Supplier();
                model.ShortName = shortName;
                model.Name = name;
                model.ContactName = contactName;
                model.Phone = phone;
                model.Status = Convert.ToInt16(status);
                model.Address = address;
                model.Remark = remark;
                string excuteResult = string.Empty;
                if (saveflag == "add")
                {
                    var existsResult = dalGlobal.Exists_Factory<Supplier>(model);
                    if (existsResult)
                    {
                        excuteResult = "exists";
                    }
                    else
                    {
                        excuteResult = dalGlobal.Insert_Factory<Supplier>(model).ToString();
                    }
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    var existsResult = dalGlobal.Exists_Factory<Supplier>(model);
                    if (existsResult)
                    {
                        excuteResult = "exists";
                    }
                    else
                    {
                        excuteResult = dalGlobal.Update_Factory<Supplier>(model).ToString();
                    }
                }
                result = excuteResult.ToString();
            }
            #endregion

            #region 根据ID获取供应商
            if (action == "GetSupplierByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dalGlobal.GetModel_Factory<Supplier>(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 绑定物料类型下拉框
            if (action == "InitSelect_MaterielType")
            {
                DataSet ds = new DataSet();
                ds = dalGlobal.GetRecord<MaterielType>("");
                StringBuilder strOpt = new StringBuilder();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    strOpt.Append("<option value=''>请选择</option>");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        strOpt.AppendFormat("<option value='{0}'>{1}</option>", ds.Tables[0].Rows[i]["ID"],
                            ds.Tables[0].Rows[i]["TypeName"]);
                    }
                }
                result = strOpt.ToString();
            }
            #endregion

            #region 绑定供应商下拉框
            if (action == "InitSelect_Supplier")
            {
                DataSet ds = new DataSet();
                ds = dalGlobal.GetRecord<Supplier>("");
                StringBuilder strOpt = new StringBuilder();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    strOpt.Append("<option value=''>请选择</option>");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        strOpt.AppendFormat("<option value='{0}'>{1}</option>", ds.Tables[0].Rows[i]["ID"],
                            ds.Tables[0].Rows[i]["Name"]);
                    }
                }
                result = strOpt.ToString();
            }
            #endregion

            #region 获取物料列表
            if (action == "getRecordPage_Materiel")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string queryName = context.Request.Params["queryName"];
                if (!string.IsNullOrWhiteSpace(queryName))
                {
                    strWhere.AppendFormat(" and a.Name like '%{0}%' ", queryName);
                }
                result = getRecordPage_Materiel(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 保存物料
            if (action == "SaveMateriel")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var materielTypeID = context.Request.Params["materielTypeID"];
                var materielNo = context.Request.Params["materielNo"];
                var isConsumable = context.Request.Params["isConsumable"];
                var name = context.Request.Params["name"];
                var status = context.Request.Params["status"];
                var supplierID = context.Request.Params["supplierID"];
                var price = context.Request.Params["price"];
                var unit = context.Request.Params["unit"];
                var beginDate = context.Request.Params["beginDate"];
                var endDate = context.Request.Params["endDate"];
                var remark = context.Request.Params["remark"];
                Materiel model = new Materiel();
                model.MaterielTypeID = Convert.ToInt16(materielTypeID); ;
                model.MaterielNo = materielNo;
                model.IsConsumable = Convert.ToInt16(isConsumable);
                model.Name = name;
                model.SupplierID = Convert.ToInt16(supplierID);
                model.Price = Convert.ToDecimal(price);
                model.Unit = unit;
                if (!string.IsNullOrWhiteSpace(beginDate))
                {
                    model.BeginDate = Convert.ToDateTime(beginDate);
                }
                if (!string.IsNullOrWhiteSpace(endDate))
                {
                    model.EndDate = Convert.ToDateTime(endDate);
                }
                
                model.Status = Convert.ToInt16(status);
                model.Remark = remark;
                string excuteResult = string.Empty;
                if (saveflag == "add")
                {
                    var existsResult = dalGlobal.Exists_Factory<Materiel>(model);
                    if (existsResult)
                    {
                        excuteResult = "exists";
                    }
                    else
                    {
                        excuteResult = dalGlobal.Insert_Factory<Materiel>(model).ToString();
                    }
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    var existsResult = dalGlobal.Exists_Factory<Materiel>(model);
                    if (existsResult)
                    {
                        excuteResult = "exists";
                    }
                    else
                    {
                        excuteResult = dalGlobal.Update_Factory<Materiel>(model).ToString();
                    }
                }
                result = excuteResult.ToString();
            }
            #endregion

            #region 根据ID获取物料
            if (action == "GetMaterielByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dalGlobal.GetModel_Factory<Materiel>(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 获取申请新物料列表
            if (action == "getRecordPage_MaterielApply")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string queryName = context.Request.Params["queryName"];
                if (!string.IsNullOrWhiteSpace(queryName))
                {
                    strWhere.AppendFormat(" and a.Name like '%{0}%' ", queryName);
                }
                result = getRecordPage_MaterielApply(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 保存申请新物料
            if (action == "SaveMaterielApply")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var name = context.Request.Params["name"];
                var quantity = context.Request.Params["quantity"];
                var unit = context.Request.Params["unit"];

                var applyDate = context.Request.Params["applyDate"];
                var applyDept = context.Request.Params["applyDept"];
                var applyPeople = context.Request.Params["applyPeople"];

                var status = context.Request.Params["status"];
                var remark = context.Request.Params["remark"];
                MaterielApply model = new MaterielApply();
                model.Quantity = Convert.ToInt16(quantity); ;
                model.Name = name;
                model.Unit =unit;
                model.ApplyDate = Convert.ToDateTime(applyDate);
                model.ApplyDept = applyDept;
                model.ApplyPeople = applyPeople;
                model.Status = Convert.ToInt16(status);
                model.Remark = remark;
                string excuteResult = string.Empty;
                if (saveflag == "add")
                {
                    var existsResult = dalGlobal.Exists_Factory<MaterielApply>(model);
                    if (existsResult)
                    {
                        excuteResult = "exists";
                    }
                    else
                    {
                        excuteResult = dalGlobal.Insert_Factory<MaterielApply>(model).ToString();
                    }
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    var existsResult = dalGlobal.Exists_Factory<MaterielApply>(model);
                    if (existsResult)
                    {
                        excuteResult = "exists";
                    }
                    else
                    {
                        excuteResult = dalGlobal.Update_Factory<MaterielApply>(model).ToString();
                    }
                }
                result = excuteResult.ToString();
            }
            #endregion

            #region 根据ID获取申请新物料
            if (action == "GetMaterielApplyByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dalGlobal.GetModel_Factory<MaterielApply>(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 绑定物料下拉框
            if (action == "InitSelect_Materiel")
            {
                DataSet ds = new DataSet();
                ds = dalGlobal.GetRecord<Materiel>("");
                StringBuilder strOpt = new StringBuilder();
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    strOpt.Append("<option value=''>请选择</option>");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        strOpt.AppendFormat("<option Unit='{2}' Price='{3}' value='{0}'>{1}</option>", ds.Tables[0].Rows[i]["ID"],
                            ds.Tables[0].Rows[i]["Name"], ds.Tables[0].Rows[i]["Unit"], ds.Tables[0].Rows[i]["Price"]);
                    }
                }
                result = strOpt.ToString();
            }
            #endregion
            
            #region 获取采购申请列表
            if (action == "getRecordPage_PurchaseApply")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string quaryName = context.Request.Params["quaryName"];
                if (!string.IsNullOrWhiteSpace(quaryName))
                {
                    strWhere.AppendFormat(" and a.ApplyPeople like '%{0}%' ", quaryName);
                }
                result = getRecordPage_PurchaseApply(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 根据ID获取采购申请
            if (action == "GetPurchaseApplyByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dal.GetPurchaseApplyByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 保存采购申请
            if (action == "SavePurchaseApply")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var applyDept = context.Request.Params["applyDept"];
                var applyPeople = context.Request.Params["applyPeople"];
                var applyDate = context.Request.Params["applyDate"];
                var status = context.Request.Params["status"];
                var remark = context.Request.Params["remark"];
                var details = context.Request.Params["details"];
                List<PurchaseApplyDetail> listPurchaseApplyDetail = new List<PurchaseApplyDetail>();
                if (!string.IsNullOrWhiteSpace(details))
                {
                    listPurchaseApplyDetail = CommTools.JsonToObject(details, typeof(List<PurchaseApplyDetail>)) as List<PurchaseApplyDetail>;
                }
                PurchaseApply model = new PurchaseApply();
                model.ApplyDept = applyDept;
                model.ApplyPeople = applyPeople;
                model.ApplyDate = Convert.ToDateTime(applyDate);
                model.Remark = remark;
                model.Status = Convert.ToInt16(status);
                model.ListPurchaseApplyDetail = listPurchaseApplyDetail;
                model.TotalPrice = listPurchaseApplyDetail.Sum(p => p.RequirePrice);
                string excuteResult = string.Empty;
                if (saveflag == "add")
                {
                    excuteResult = dal.AddPurchaseApply(model).ToString();
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);
                    excuteResult = dal.UpdatePurchaseApply(model).ToString();
                }
                result = excuteResult;
            }
            #endregion

            #region 获取物料入库列表
            if (action == "getRecordPage_StoreManage")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string queryName = context.Request.Params["queryName"];
                if (!string.IsNullOrWhiteSpace(queryName))
                {
                    strWhere.AppendFormat(" and b.Name like '%{0}%' ", queryName);
                }
                result = getRecordPage_StoreManage(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 保存物料入库
            if (action == "SaveStoreManage")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var materielID = context.Request.Params["materielID"];
                var purchaseApplyID = context.Request.Params["purchaseApplyID"];
                var fixedAssetNo = context.Request.Params["fixedAssetNo"];
                var storeQuantity = context.Request.Params["storeQuantity"];
                var storePrice = context.Request.Params["storePrice"];
                var storePeople = context.Request.Params["storePeople"];
                var storeDate = context.Request.Params["storeDate"];
                var status = context.Request.Params["status"];
                var remark = context.Request.Params["remark"];
                StoreManage model = new StoreManage();
                model.MaterielID = Convert.ToInt16(materielID);
                model.PurchaseApplyID = Convert.ToInt16(purchaseApplyID);
                model.FixedAssetNo = fixedAssetNo;
                model.StoreQuantity = Convert.ToInt16(storeQuantity);
                model.StorePrice = Convert.ToDecimal(storePrice);
                model.StorePeople = storePeople;
                model.StoreDate = Convert.ToDateTime(storeDate);
                model.Status = Convert.ToInt16(status);
                model.Remark = remark;
                string excuteResult = string.Empty;
                if (saveflag == "add")
                {
                    excuteResult = dal.AddStoreManage(model).ToString();
                }
                if (saveflag == "edit")
                {
                    model.ID = Convert.ToInt16(ID);

                    excuteResult = dal.UpdateStoreManage(model).ToString();
                }
                result = excuteResult.ToString();
            }
            #endregion

            #region 根据ID获取物料入库
            if (action == "GetStoreManageByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dal.GetStoreManageByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 获取申请完成物料采购明细
            if (action == "getRecordPage_PurchaseApplyDetail")
            {
                //StringBuilder strWhere = new StringBuilder();
                //string queryName = context.Request.Params["queryName"];
                //if (!string.IsNullOrWhiteSpace(queryName))
                //{
                //    strWhere.AppendFormat(" and b.Name like '%{0}%' ", queryName);
                //}
                result = getRecordPage_PurchaseApplyDetail("");
                //strWhere = null;
            }
            #endregion

            #region 确认物料入库
            if (action == "ConfirmStored")
            {
                var ID = context.Request.Params["ID"];
                var staff = dal.ConfirmStored(ID);
                result = staff.ToString();
            }
            #endregion

            #region 获取物料领用列表
            if (action == "getRecordPage_UseManage")
            {
                int m_currentpage = 1;
                int m_pagesize = 15;
                if (currentpage != null && currentpage != "" && currentpage != "undefined") { m_currentpage = int.Parse(currentpage); }
                if (pagesize != null && pagesize != "" && pagesize != "undefined") { m_pagesize = int.Parse(pagesize); }
                StringBuilder strWhere = new StringBuilder();
                string queryName = context.Request.Params["queryName"];
                if (!string.IsNullOrWhiteSpace(queryName))
                {
                    strWhere.AppendFormat(" and b.Name like '%{0}%' ", queryName);
                }
                result = getRecordPage_UseManage(m_currentpage, m_pagesize, strWhere.ToString(), sortfield, sorttype);
                strWhere = null;
            }
            #endregion

            #region 保存物料领用
            if (action == "SaveUseManage")
            {
                var saveflag = context.Request.Params["saveflag"];
                var ID = context.Request.Params["ID"];
                var materielID = context.Request.Params["materielID"];
                var useQuantity = context.Request.Params["useQuantity"];
                var usePeople = context.Request.Params["usePeople"];
                var useDate = context.Request.Params["useDate"];
                var remark = context.Request.Params["remark"];
                UseManage model = new UseManage();
                model.MaterielID = Convert.ToInt16(materielID);
                model.UseQuantity = Convert.ToInt16(useQuantity);
                model.UsePeople = usePeople;
                model.UseDate = Convert.ToDateTime(useDate);
                model.Remark = remark;
                string excuteResult = string.Empty;
                if (saveflag == "add")
                {
                    excuteResult = dal.AddUseManage(model).ToString();
                }
                result = excuteResult.ToString();
            }
            #endregion

            #region 根据ID获取物料领用
            if (action == "GetUseManageByID")
            {
                var ID = context.Request.Params["ID"];
                var staff = dal.GetUseManageByID(Convert.ToInt16(ID));
                result = CommTools.ObjectToJson(staff);
            }
            #endregion

            #region 获取物料库存明细
            if (action == "getRecordPage_MeterielStock")
            {
                StringBuilder strWhere = new StringBuilder();
                strWhere.AppendFormat(" and c.IsConsumable=1 ");
                result = getRecordPage_MaterielStock(strWhere.ToString());
                strWhere = null;
            }
            #endregion

            context.Response.Write(result);
            context.Response.Flush();
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #region 意外事故列表
        private string getRecordPage_AccidentInfo(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();

            string sortname = " a.CreateOn desc ";
            DataSet ds = dal.GetAccidentInfoList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_AccidentInfo(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_AccidentInfo(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:3%'>序号</th>"
                           + "<th style='width:6%'>床位号</th>"
                           + "<th style='width:6%'>姓名</th>"
                           + "<th style='width:3%'>性别</th>"
                           + "<th style='width:3%'>年龄</th>"
                           + "<th style='width:6%'>护理等级</th>"
                           + "<th style='width:9%'>发生日期</th>"
                           + "<th style='width:4%'>发生时间</th>"
                           + "<th style='width:6%'>事件类型</th>"
                           + "<th style='width:6%'>发生地点</th>"
                           + "<th style='width:13%'>伤情</th>"
                           + "<th style='width:20%'>备注</th>"
                           + "<th style='width:15%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    var room_bed_no = dt.Rows[i]["RoomNo"].ToString() + "-" + dt.Rows[i]["BedNo"].ToString();
                    tblHtml.Append("<td>" + room_bed_no + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Name"] + "</td>");
                    var sexName = dt.Rows[i]["Sex"].ToString() == "1" ? "女" : "男";
                    tblHtml.Append("<td>" + sexName + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Age"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["NurseLevel"] + "</td>");
                    var createOnDate = Convert.ToDateTime(dt.Rows[i]["CreateOn"]).ToString("yyyy-MM-dd");
                    var createOnTime = Convert.ToDateTime(dt.Rows[i]["CreateOn"]).ToString("HH:mm");
                    tblHtml.Append("<td>" + createOnDate + "</td>");
                    tblHtml.Append("<td>" + createOnTime + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["AccidentType"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Place"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Condition"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Remark"] + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"UpdateAccident(" + dt.Rows[i]["AccidentID"] + ")\">编辑</a><a class='ml_20' href='javascript:void(0)' onclick=\"FollowAccident(" + dt.Rows[i]["AccidentID"] + ")\">跟进</a><a class='ml_20' href='javascript:void(0)' onclick=\"DeleteAccident(" + dt.Rows[i]["AccidentID"] + ")\">删除</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 日常记录列表
        private string getRecordPage_DailyRecordInfo(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();

            string sortname = " a.CreateOn desc ";
            DataSet ds = dal.GetDailyRecordInfoList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_DailyRecordInfo(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_DailyRecordInfo(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:5%'>序号</th>"
                           + "<th style='width:5%'>床位号</th>"
                           + "<th style='width:8%'>姓名</th>"
                            + "<th style='width:10%'>事件类型</th>"
                           + "<th style='width:10%'>记录日期</th>"
                           + "<th style='width:7%'>报告人</th>"
                           + "<th style='width:45%'>记录详情</th>"
                           + "<th style='width:10%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    var room_bed_no = dt.Rows[i]["RoomNo"].ToString() + "-" + dt.Rows[i]["BedNo"].ToString();
                    tblHtml.Append("<td>" + room_bed_no + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Name"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["DailyRecordType"] + "</td>");
                    var createOnDate = Convert.ToDateTime(dt.Rows[i]["CreateOn"]).ToString("yyyy-MM-dd");
                    tblHtml.Append("<td>" + createOnDate + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["StaffName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Remark"] + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"EditDailyRecord(" + dt.Rows[i]["DailyRecordID"] + ")\">编辑</a><a class='ml_20' href='javascript:void(0)' onclick=\"DeleteDailyRecord(" + dt.Rows[i]["DailyRecordID"] + ")\">删除</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 员工考评列表
        private string getRecordPage_StaffEvaluateInfo(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();

            string sortname = " a.CreateOn ";
            DataSet ds = dal.GetStaffEvaluateInfoList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_StaffEvaluateInfo(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_StaffEvaluateInfo(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:10%'>员工编号</th>"
                           + "<th style='width:15%'>姓名</th>"
                           //+ "<th style='width:10%'>考评类型</th>"
                           + "<th style='width:15%'>记录日期</th>"
                           + "<th style='width:40%'>详情</th>"
                           + "<th style='width:10%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["StaffOtherNo"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Name"] + "</td>");
                    //var evaluateType = (dt.Rows[i]["EvaluateType"].ToString() == "0") ? "优评" : "劣评";
                    //tblHtml.Append("<td>" + evaluateType + "</td>");
                    var createOnDate = Convert.ToDateTime(dt.Rows[i]["CreateOn"]).ToString("yyyy-MM-dd");
                    tblHtml.Append("<td>" + createOnDate + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Remark"] + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"EditStaffEvaluate(" + dt.Rows[i]["StaffEvaluateID"] + ")\">编辑</a><a class='ml_20' href='javascript:void(0)' onclick=\"DeleteStaffEvaluate(" + dt.Rows[i]["StaffEvaluateID"] + ")\">删除</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 员工列表
        private string getRecordPage_StaffInfo(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();

            string sortname = " a.ID ";
            DataSet ds = dal.GetStaffInfoList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_StaffInfo(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_StaffInfo(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:5%'>序号</th>"
                           + "<th style='width:20%'>姓名</th>"
                           + "<th style='width:5%'>性别</th>"
                           + "<th style='width:20%'>身份证号</th>"
                           + "<th style='width:10%'>岗位</th>"
                           + "<th style='width:10%'>员工编号</th>"
                           + "<th style='width:10%'>带教师傅</th>"
                           + "<th style='width:10%'>状态</th>"
                           + "<th style='width:10%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Name"] + "</td>");
                    var sexName = dt.Rows[i]["Sex"].ToString() == "1" ? "女" : "男";
                    tblHtml.Append("<td>" + sexName + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["IDCardNo"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["PostName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["StaffOtherNo"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["MasterStaffName"] + "</td>");
                    var status = (dt.Rows[i]["Status"].ToString() == "0") ? "试用" : (dt.Rows[i]["Status"].ToString() == "1") ? "转正" : "离职";
                    tblHtml.Append("<td>" + status + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 客人列表
        private string getRecordPage_GuestInfo(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dal.GetGuestInfoList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_GuestInfo(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_GuestInfo(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:5%'>序号</th>"
                           + "<th style='width:10%'>姓名</th>"
                           + "<th style='width:10%'>房间号</th>"
                           + "<th style='width:10%'>床位号</th>"
                           + "<th style='width:5%'>性别</th>"
                           + "<th style='width:10%'>年龄</th>"
                           + "<th style='width:10%'>护理等级</th>"
                           + "<th style='width:10%'>试住日期</th>"
                           + "<th style='width:10%'>入住日期</th>"
                           + "<th style='width:10%'>状态</th>"
                           + "<th style='width:10%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Name"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["RoomNo"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["BedNo"] + "</td>");
                    var sexName = dt.Rows[i]["Sex"].ToString() == "1" ? "女" : "男";
                    tblHtml.Append("<td>" + sexName + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Age"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["NurseLevel"] + "</td>");
                    var tryAdmissionDate = (dt.Rows[i]["TryAdmissionDate"] == DBNull.Value) ? "" : (Convert.ToDateTime(dt.Rows[i]["TryAdmissionDate"]).ToString("yyyy-MM-dd"));
                    tblHtml.Append("<td>" + tryAdmissionDate + "</td>");
                    var admissionDate = Convert.ToDateTime(dt.Rows[i]["AdmissionDate"]).ToString("yyyy-MM-dd");
                    tblHtml.Append("<td>" + admissionDate + "</td>");
                    var status = ((dt.Rows[i]["Status"].ToString() == "0") ? "入住" : (dt.Rows[i]["Status"].ToString() == "1") ? "试住" : "离院");
                    tblHtml.Append("<td>" + status + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 意外事故类型列表
        private string getRecordPage_AccidentFollow(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();

            string sortname = " FollowTime desc ";
            DataSet ds = dal.GetAccidentFollowList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_AccidentFollow(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_AccidentFollow(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("<th>序号</th>"
                           + "<th>跟进时间</th>"
                           + "<th>跟进情况</th>");
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td style='width:10%'>" + (i + 1).ToString() + "</td>");
                    var followTime = Convert.ToDateTime(dt.Rows[i]["FollowTime"]).ToString("yyyy-MM-dd");
                    tblHtml.Append("<td style='width:20%'>" + followTime + "</td>");
                    tblHtml.Append("<td style='width:70%'>" + dt.Rows[i]["Remark"] + "</td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 根据条件获取意外事件详情
        private string getAccidentInfoHtml(string where)
        {
            DataSet ds = dal.GetAccidentInfoByWhere(where);
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:3%'>序号</th>"
                           + "<th style='width:8%'>床位号</th>"
                           + "<th style='width:8%'>姓名</th>"
                           + "<th style='width:3%'>性别</th>"
                           + "<th style='width:6%'>护理等级</th>"
                           + "<th style='width:12%'>发生时间</th>"
                           + "<th style='width:6%'>事件类型</th>"
                           + "<th style='width:8%'>相关人员</th>"
                           + "<th style='width:6%'>发生地点</th>"
                           + "<th style='width:20%'>伤情</th>"
                           + "<th style='width:20%'>备注</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var dt = ds.Tables[0];
                tblHtml.Append("<tr>");
                tblHtml.Append("<td>" + dt.Rows[0]["AccidentID"] + "</td>");
                var room_bed_no = dt.Rows[0]["RoomNo"].ToString() + "-" + dt.Rows[0]["BedNo"].ToString();
                tblHtml.Append("<td>" + room_bed_no + "</td>");
                tblHtml.Append("<td>" + dt.Rows[0]["Name"] + "</td>");
                var sexName = dt.Rows[0]["Sex"].ToString() == "1" ? "女" : "男";
                tblHtml.Append("<td>" + sexName + "</td>");
                //tblHtml.Append("<td>" + dt.Rows[0]["Age"] + "</td>");
                tblHtml.Append("<td>" + dt.Rows[0]["NurseLevel"] + "</td>");
                var createOnTime = Convert.ToDateTime(dt.Rows[0]["CreateOn"]).ToString("yyyy-MM-dd mm:ss");
                tblHtml.Append("<td>" + createOnTime + "</td>");
                tblHtml.Append("<td>" + dt.Rows[0]["AccidentType"] + "</td>");
                //相关责任人
                string staffList = string.Empty;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var staffName = dt.Rows[i]["StaffName"].ToString();
                    if (i == 0)
                    {
                        staffList += staffName;
                    }
                    else
                    {
                        staffList += "," + staffName;
                    }
                }
                tblHtml.Append("<td>" + staffList + "</td>");

                tblHtml.Append("<td>" + dt.Rows[0]["Place"] + "</td>");
                tblHtml.Append("<td>" + dt.Rows[0]["Condition"] + "</td>");
                tblHtml.Append("<td>" + dt.Rows[0]["Remark"] + "</td>");
                tblHtml.Append("</tr>");
            }
            tblHtml.Append("</tbody></table></div>");
            ds = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 意外事故统计
        private string getRecordPage_AccidentStats(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();

            string sortname = " OccurCount desc ";
            DataSet ds = dal.GetAccidentStats(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_AccidentStats(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_AccidentStats(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append(" <th style='width:25%'>姓名</th>"
                           + "<th style='width:10%'>性别</th>"
                           + "<th style='width:10%'>年龄</th>"
                           + "<th style='width:20%'>床位号</th>"
                           + "<th style='width:25%'>事故类型</th>"
                           + "<th style='width:10%'>发生次数</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var room_bed_no = dt.Rows[i]["RoomNo"].ToString() + "-" + dt.Rows[i]["BedNo"].ToString();
                    var sexName = dt.Rows[i]["Sex"].ToString() == "1" ? "女" : "男";
                    var occurCount = Convert.ToInt16(dt.Rows[i]["OccurCount"]);
                    var trClass = "";
                    if (occurCount == 3)
                    {
                        trClass = "class='bg_yellow' ";
                    }
                    else if (occurCount == 4)
                    {
                        trClass = "class='bg_orange' ";
                    }
                    else if (occurCount >= 5)
                    {
                        trClass = "class='bg_red' ";
                    }
                    else
                    {
                        trClass = "";
                    }
                    tblHtml.Append("<tr " + trClass + ">");
                    tblHtml.Append("<td>" + dt.Rows[i]["Name"] + "</td>");
                    tblHtml.Append("<td>" + sexName + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Age"] + "</td>");
                    tblHtml.Append("<td>" + room_bed_no + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["AccidentType"] + "</td>");
                    tblHtml.Append("<td>" + occurCount + "</td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 日常记录统计
        private string getRecordPage_DailyRecordStats(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();

            string sortname = " OccurCount desc ";
            DataSet ds = dal.GetDailyRecordStats(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_DailyRecordStats(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_DailyRecordStats(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append(" <th style='width:20%'>姓名</th>"
                           + "<th style='width:10%'>性别</th>"
                           + "<th style='width:10%'>年龄</th>"
                           + "<th style='width:15%'>床位号</th>"
                           + "<th style='width:25%'>记录类型</th>"
                           + "<th style='width:10%'>发生次数</th>"
                           + "<th style='width:10%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var room_bed_no = dt.Rows[i]["RoomNo"].ToString() + "-" + dt.Rows[i]["BedNo"].ToString();
                    var sexName = dt.Rows[i]["Sex"].ToString() == "1" ? "女" : "男";
                    var occurCount = Convert.ToInt16(dt.Rows[i]["OccurCount"]);
                    var trClass = "";
                    if (occurCount == 3)
                    {
                        trClass = "class='bg_yellow' ";
                    }
                    else if (occurCount == 4)
                    {
                        trClass = "class='bg_orange' ";
                    }
                    else if (occurCount >= 5)
                    {
                        trClass = "class='bg_red' ";
                    }
                    else
                    {
                        trClass = "";
                    }
                    tblHtml.Append("<tr " + trClass + ">");
                    tblHtml.Append("<td>" + dt.Rows[i]["Name"] + "</td>");
                    tblHtml.Append("<td>" + sexName + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Age"] + "</td>");
                    tblHtml.Append("<td>" + room_bed_no + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["DailyRecordType"] + "</td>");
                    tblHtml.Append("<td>" + occurCount + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"FollowDailyRecord('" + dt.Rows[i]["Name"] + "')\">查看</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 排班模板列表
        private string getRecordPage_SchedualTemplate(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dal.GetSchedualTemplateList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_SchedualTemplate(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_SchedualTemplate(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:30%'>模板名称</th>"
                           + "<th style='width:10%'>人数</th>"
                           + "<th style='width:30%'>模板说明</th>"
                           + "<th style='width:10%'>状态</th>"
                           + "<th style='width:10%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["TemplateName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["PeopleNum"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["TemplateRemark"] + "</td>");
                    var status = dt.Rows[i]["Status"].ToString() == "1" ? "可用" : "停用";
                    tblHtml.Append("<td>" + status + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 其他工时列表
        private string getRecordPage_OtherWorkTime(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dal.GetOtherWorkTimeList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_OtherWorkTime(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_OtherWorkTime(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:5%'>序号</th>"
                           + "<th style='width:10%'>员工</th>"
                           + "<th style='width:10%'>工时类型</th>"
                           + "<th style='width:20%'>开始时间</th>"
                           + "<th style='width:20%'>结束时间</th>"
                           + "<th style='width:5%'>工时</th>"
                           + "<th style='width:10%'>状态</th>"
                           + "<th style='width:10%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Name"] + "</td>");
                    var workType = dt.Rows[i]["WorkType"].ToString() == "0" ? "培训" : "会议";
                    tblHtml.Append("<td>" + workType + "</td>");
                    var beginTime = Convert.ToDateTime(dt.Rows[i]["BeginTime"]).ToString("yyyy-MM-dd HH:mm");
                    tblHtml.Append("<td>" + beginTime + "</td>");
                    var endTime = Convert.ToDateTime(dt.Rows[i]["EndTime"]).ToString("yyyy-MM-dd HH:mm");
                    tblHtml.Append("<td>" + endTime + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Hours"] + "</td>");
                    var status = dt.Rows[i]["Status"].ToString() == "0" ? "申请中" : "确认";
                    tblHtml.Append("<td>" + status + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 工时统计
        private string getRecordPage_SchedualCount(int currentpage, int pagesize, string where_a, string where_b, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dal.GetSchedualCountList(currentpage, pagesize, where_a, where_b, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_SchedualCount(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_SchedualCount(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:15%'>序号</th>"
                           + "<th style='width:20%'>员工工号</th>"
                           + "<th style='width:20%'>姓名</th>"
                           + "<th style='width:15%'>排班工时</th>"
                           + "<th style='width:15%'>其他工时</th>"
                           + "<th style='width:15%'>总工时</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["StaffNo"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Name"] + "</td>");
                    var normalHours = Convert.ToDecimal(dt.Rows[i]["NormalHours"]);
                    var otherHours = Convert.ToDecimal(dt.Rows[i]["OtherHours"]);
                    tblHtml.Append("<td>" + normalHours.ToString("f1") + "</td>");
                    tblHtml.Append("<td>" + otherHours.ToString("f1") + "</td>");
                    tblHtml.Append("<td>" + (normalHours + otherHours).ToString("f1") + "</td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 班次时间列表
        private string getRecordPage_SchedualTime(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();

            string sortname = " ID ";
            DataSet ds = dal.GetSchedualTimeList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_SchedualTime(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_SchedualTime(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("<th>序号</th>"
                           + "<th>班次名称</th>"
                           + "<th>时间</th>"
                           + "<th>操作</th>");
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1) + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["SchedualName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Schedual"] + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick='DeleteSchedualTime(" + dt.Rows[i]["ID"] + ")'>删除</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 意外事故类型列表
        private string getRecordPage_RoomCombine(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();

            string sortname = " ID ";
            DataSet ds = dal.GetRoomCombineList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_RoomCombine(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_RoomCombine(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("<th>序号</th>"
                           + "<th>房间组合</th>"
                           + "<th>备注</th>"
                           + "<th>操作</th>");
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1) + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["RoomList"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Remark"] + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a>  <a class='ml_20' href='javascript:void(0)' onclick='DeleteRoomCombine(" + dt.Rows[i]["ID"] + ")'>删除</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 考核模板列表
        private string getRecordPage_AssessTemplate(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dal.GetAssessTemplateList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_AssessTemplate(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_AssessTemplate(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:15%'>职级</th>"
                           + "<th style='width:20%'>岗位</th>"
                           + "<th style='width:20%'>考核类型</th>"
                           + "<th style='width:10%'>状态</th>"
                           + "<th style='width:15%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Rank"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["PostName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["AssessType"] + "</td>");
                    var status = ((dt.Rows[i]["Status"].ToString() == "0") ? "无效" : "有效");
                    tblHtml.Append("<td>" + status + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a><a class='ml_20' href='javascript:void(0)' onclick=\"DeleteAssessTemplate(" + dt.Rows[i]["ID"] + ")\">删除</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 员工绩效考核列表
        private string getRecordPage_StaffAssess(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dal.GetStaffAssessList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_StaffAssess(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_StaffAssess(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:15%'>姓名</th>"
                           + "<th style='width:10%'>职级</th>"
                           + "<th style='width:20%'>考核时间</th>"
                           + "<th style='width:10%'>考核类型</th>"
                           + "<th style='width:10%'>总分</th>"
                           + "<th style='width:10%'>状态</th>"
                           + "<th style='width:15%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Name"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Rank"] + "</td>");
                    var assessDate = Convert.ToDateTime(dt.Rows[i]["AssessDate"]).ToString("yyyy-MM");
                    tblHtml.Append("<td>" + assessDate + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["AssessType"] + "</td>");
                    var score = dt.Rows[i]["Score"].ToString();
                    decimal sumScore = 0;
                    if (!string.IsNullOrWhiteSpace(score))
                    {
                        for (int m = 0; m < score.Split(',').Length; m++)
                        {
                            sumScore += Convert.ToDecimal(score.Split(',')[m]);
                        }
                    }
                    tblHtml.Append("<td>" + sumScore + "</td>");
                    var status = ((dt.Rows[i]["Status"].ToString() == "0") ? "考核中" : "确认");
                    tblHtml.Append("<td>" + status + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a><a class='ml_20' href='javascript:void(0)' onclick=\"DeleteStaffAssess(" + dt.Rows[i]["ID"] + ")\">删除</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 基础信息列表
        private string getRecordPage_BasicInfo(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();

            string sortname = " ID ";
            DataSet ds = dal.GetBasicInfoList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_BasicInfo(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_BasicInfo(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("<th>序号</th>"
                           + "<th>名称</th>"
                           + "<th>操作</th>");
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ParamOptionName"] + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick='DeleteBasicInfo(" + dt.Rows[i]["ID"] + ")'>删除</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 岗位列表
        private string getRecordPage_PostInfo(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dal.GetPostInfoList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_PostInfo(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_PostInfo(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:30%'>岗位编号</th>"
                           + "<th style='width:30%'>岗位名称</th>"
                           + "<th style='width:10%'>费率</th>"
                           + "<th style='width:20%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["PostLevel"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["PostName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Rate"] + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 员工请假列表
        private string getRecordPage_StaffLeave(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dal.GetStaffLeaveList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_StaffLeave(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_StaffLeave(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:10%'>员工</th>"
                           + "<th style='width:10%'>请假原因</th>"
                           + "<th style='width:20%'>开始日期</th>"
                           + "<th style='width:20%'>结束日期</th>"
                           + "<th style='width:10%'>请假天数</th>"
                           + "<th style='width:10%'>状态</th>"
                           + "<th style='width:10%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Name"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["LeaveType"] + "</td>");
                    var beginDate = Convert.ToDateTime(dt.Rows[i]["BeginDate"]).ToString("yyyy-MM-dd");
                    tblHtml.Append("<td>" + beginDate + "</td>");
                    var endDate = Convert.ToDateTime(dt.Rows[i]["EndDate"]).ToString("yyyy-MM-dd");
                    tblHtml.Append("<td>" + endDate + "</td>");
                    var days = (Convert.ToDateTime(dt.Rows[i]["EndDate"]) - Convert.ToDateTime(dt.Rows[i]["BeginDate"])).TotalDays;
                    tblHtml.Append("<td>" + (days + 1) + "</td>");
                    var status = dt.Rows[i]["Status"].ToString() == "0" ? "申请中" : "批准";
                    tblHtml.Append("<td>" + status + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 员工人脉列表
        private string getRecordPage_StaffConnection(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dal.GetStaffConnectionList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_StaffConnection(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_StaffConnection(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:10%'>员工</th>"
                           + "<th style='width:10%'>关系员工</th>"
                           + "<th style='width:20%'>关系客人</th>"
                           + "<th style='width:20%'>其他关系人</th>"
                           + "<th style='width:20%'>关系类型</th>"
                           + "<th style='width:10%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["StaffName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ConnectStaffName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ConnectGuestName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["OtherConnectName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ConnectionType"] + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a><a class='ml_20' href='javascript:void(0)' onclick=\"DeleteStaffConnection(" + dt.Rows[i]["ID"] + ")\">删除</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 楼层列表
        private string getRecordPage_Floor(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.FloorID ";
            DataSet ds = dal.GetFloorList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_Floor(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_Floor(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:10%'>楼层编号</th>"
                           + "<th style='width:30%'>楼层描述</th>"
                           + "<th style='width:20%'>楼层负责人</th>"
                           + "<th style='width:10%'>状态</th>"
                           + "<th style='width:20%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["FloorID"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Remark"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Name"] + "</td>");
                    var status = (dt.Rows[i]["Status"].ToString() == "0") ? "有效" : (dt.Rows[i]["Status"].ToString() == "1") ? "停用" : (dt.Rows[i]["Status"].ToString() == "2") ? "满员" : "";
                    tblHtml.Append("<td>" + status + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 房间列表
        private string getRecordPage_Room(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.RoomNo ";
            DataSet ds = dal.GetRoomList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_Room(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_Room(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:15%'>房间编号</th>"
                           + "<th style='width:10%'>楼层编号</th>"
                           + "<th style='width:10%'>房间性别</th>"
                           + "<th style='width:10%'>房间人数</th>"
                           + "<th style='width:25%'>描述</th>"
                           + "<th style='width:10%'>状态</th>"
                           + "<th style='width:10%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["RoomNo"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["FloorID"] + "</td>");
                    var roomSex = (dt.Rows[i]["RoomSex"].ToString() == "0") ? "男" : "女";
                    tblHtml.Append("<td>" + roomSex + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["PeopleNum"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Remark"] + "</td>");
                    var status = (dt.Rows[i]["Status"].ToString() == "0") ? "有效" : (dt.Rows[i]["Status"].ToString() == "1") ? "停用" : (dt.Rows[i]["Status"].ToString() == "2") ? "满员" : "";
                    tblHtml.Append("<td>" + status + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 客人信息变更记录
        private string getRecordPage_GuestChangeLog(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();

            string sortname = " ModifyOn desc ";
            DataSet ds = dal.GetGuestChangeLogList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_GuestChangeLog(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_GuestChangeLog(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append(" <th style='width:10%'>姓名</th>"
                           + "<th style='width:10%'>变更类型</th>"
                           + "<th style='width:5%'>原楼层</th>"
                           + "<th style='width:5%'>新楼层</th>"
                           + "<th style='width:10%'>原房间</th>"
                           + "<th style='width:10%'>新房间</th>"
                           + "<th style='width:10%'>原床位</th>"
                           + "<th style='width:10%'>新床位</th>"
                           + "<th style='width:10%'>原护理等级</th>"
                           + "<th style='width:10%'>新护理等级</th>"
                           + "<th style='width:10%'>变更日期</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string oldFloorID = string.Empty;
                    string newFloorID = string.Empty;
                    string oldRoomNo = string.Empty;
                    string newRoomNo = string.Empty;
                    string oldBedNo = string.Empty;
                    string newBedNo = string.Empty;
                    var oldBedInfo = dt.Rows[i]["OldBedNo"].ToString();
                    var newBedInfo = dt.Rows[i]["NewBedNo"].ToString();
                    if (!string.IsNullOrWhiteSpace(oldBedInfo))
                    {
                        oldFloorID = oldBedInfo.Split('-')[0];
                        oldRoomNo = oldBedInfo.Split('-')[1];
                        oldBedNo = oldBedInfo.Split('-')[2];
                    }
                    if (!string.IsNullOrWhiteSpace(newBedInfo))
                    {
                        newFloorID = newBedInfo.Split('-')[0];
                        newRoomNo = newBedInfo.Split('-')[1];
                        newBedNo = newBedInfo.Split('-')[2];
                    }
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Name"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ChangeType"] + "</td>");
                    tblHtml.Append("<td>" + oldFloorID + "</td>");
                    tblHtml.Append("<td>" + newFloorID + "</td>");
                    tblHtml.Append("<td>" + oldRoomNo + "</td>");
                    tblHtml.Append("<td>" + newRoomNo + "</td>");
                    tblHtml.Append("<td>" + oldBedNo + "</td>");
                    tblHtml.Append("<td>" + newBedNo + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["OldNurseLevel"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["NewNurseLevel"] + "</td>");
                    tblHtml.Append("<td>" + Convert.ToDateTime(dt.Rows[i]["ModifyOn"]).ToString("yyyy-MM-dd") + "</td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 员工异动记录
        private string getRecordPage_StaffTransfer(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();

            string sortname = " ModifyOn desc ";
            DataSet ds = dal.GetStaffTransferList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_StaffTransfer(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_StaffTransfer(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append(" <th style='width:20%'>姓名</th>"
                           + "<th style='width:20%'>异动类型</th>"
                           + "<th style='width:10%'>原岗位</th>"
                           + "<th style='width:10%'>新岗位</th>"
                           + "<th style='width:10%'>原职级</th>"
                           + "<th style='width:10%'>新职级</th>"
                           + "<th style='width:20%'>变更日期</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                var ds = dal.GetPostInfo();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string oldPostName = string.Empty;
                    string newPostName = string.Empty;
                    if (!string.IsNullOrWhiteSpace(dt.Rows[i]["OldPost"].ToString()))
                    {
                        oldPostName = ds.Tables[0].Select(" PostLevel='" + dt.Rows[i]["OldPost"] + "' ")[0]["PostName"].ToString();
                    }
                    if (!string.IsNullOrWhiteSpace(dt.Rows[i]["NewPost"].ToString()))
                    {
                        newPostName = ds.Tables[0].Select(" PostLevel='" + dt.Rows[i]["NewPost"] + "' ")[0]["PostName"].ToString();
                    }
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Name"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["TransferType"] + "</td>");
                    tblHtml.Append("<td>" + oldPostName + "</td>");
                    tblHtml.Append("<td>" + newPostName + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["OldRank"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["NewRank"] + "</td>");
                    tblHtml.Append("<td>" + Convert.ToDateTime(dt.Rows[i]["ModifyOn"]).ToString("yyyy-MM-dd") + "</td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 员工关系列表
        private string getRecordPage_GuestConnection(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dal.GetGuestConnectionList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_GuestConnection(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_GuestConnection(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:10%'>客人</th>"
                           + "<th style='width:10%'>关系员工</th>"
                           + "<th style='width:20%'>关系客人</th>"
                           + "<th style='width:20%'>其他关系人</th>"
                           + "<th style='width:20%'>关系类型</th>"
                           + "<th style='width:10%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["GuestName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ConnectStaffName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ConnectGuestName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["OtherConnectName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ConnectionType"] + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a><a class='ml_20' href='javascript:void(0)' onclick=\"DeleteGuestConnection(" + dt.Rows[i]["ID"] + ")\">删除</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 客人评估模板列表
        private string getRecordPage_EvaluateTemplate(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dal.GetEvaluateTemplateList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_EvaluateTemplate(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_EvaluateTemplate(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:30%'>客人评估模板名称</th>"
                           + "<th style='width:30%'>客人评估模板备注</th>"
                           + "<th style='width:15%'>状态</th>"
                           + "<th style='width:15%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Name"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Remark"] + "</td>");
                    var status = ((dt.Rows[i]["Status"].ToString() == "0") ? "无效" : "有效");
                    tblHtml.Append("<td>" + status + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a><a class='ml_20' href='javascript:void(0)' onclick=\"DeleteEvaluateTemplate(" + dt.Rows[i]["ID"] + ")\">删除</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 客人照护需求评估列表
        private string getRecordPage_GuestEvaluate(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dal.GetGuestEvaluateList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_GuestEvaluate(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_GuestEvaluate(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:15%'>姓名</th>"
                           + "<th style='width:10%'>原护理等级</th>"
                           + "<th style='width:20%'>评估时间</th>"
                           + "<th style='width:10%'>评估类型</th>"
                           + "<th style='width:10%'>总分</th>"
                           + "<th style='width:10%'>状态</th>"
                           + "<th style='width:15%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Name"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["NurseLevel"] + "</td>");
                    var evaluateDate = Convert.ToDateTime(dt.Rows[i]["EvaluateDate"]).ToString("yyyy-MM");
                    tblHtml.Append("<td>" + evaluateDate + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["EvaluateType"] + "</td>");
                    var score = dt.Rows[i]["Score"].ToString();
                    decimal sumScore = 0;
                    if (!string.IsNullOrWhiteSpace(score))
                    {
                        for (int m = 0; m < score.Split(',').Length; m++)
                        {
                            sumScore += Convert.ToDecimal(score.Split(',')[m]);
                        }
                    }
                    tblHtml.Append("<td>" + sumScore + "</td>");
                    var status = ((dt.Rows[i]["Status"].ToString() == "0") ? "评估中" : "确认");
                    tblHtml.Append("<td>" + status + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a><a class='ml_20' href='javascript:void(0)' onclick=\"DeleteGuestEvaluate(" + dt.Rows[i]["ID"] + ")\">删除</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 客人照护项目
        private string getRecordPage_NurseProject(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dal.GetNurseProjectList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_NurseProject(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_NurseProject(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:20%'>照护项目名称</th>"
                           + "<th style='width:20%'>照护项目分类</th>"
                           + "<th style='width:30%'>照护项目要求</th>"
                           + "<th style='width:20%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["NurseName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["NurseType"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Requirement"] + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a><a class='ml_20' href='javascript:void(0)' onclick=\"DeleteNurseProject(" + dt.Rows[i]["ID"] + ")\">删除</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 客人照护计划列表
        private string getRecordPage_GuestNursePlan(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dal.GetGuestNursePlanList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_GuestNursePlan(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_GuestNursePlan(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:20%'>姓名</th>"
                           + "<th style='width:10%'>护理等级</th>"
                           + "<th style='width:10%'>年龄</th>"
                           + "<th style='width:20%'>计划开始时间</th>"
                           + "<th style='width:10%'>状态</th>"
                           + "<th style='width:20%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Name"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["NurseLevel"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Age"] + "</td>");
                    var beginDate = Convert.ToDateTime(dt.Rows[i]["BeginDate"]).ToString("yyyy-MM-dd");
                    tblHtml.Append("<td>" + beginDate + "</td>");
                    var status = ((dt.Rows[i]["Status"].ToString() == "0") ? "录入中" : "执行中");
                    tblHtml.Append("<td>" + status + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a><a class='ml_20' href='javascript:void(0)' onclick=\"DeleteGuestNursePlan(" + dt.Rows[i]["ID"] + ")\">删除</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 产品类型列表
        private string getRecordPage_ProductType(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dal.GetProductTypeList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_ProductType(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_ProductType(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:20%'>产品类型编号</th>"
                           + "<th style='width:30%'>产品类型名称</th>"
                           + "<th style='width:20%'>产品大类</th>"
                           + "<th style='width:20%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ProductTypeNo"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ProductTypeName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["MainProductType"] + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 产品列表
        private string getRecordPage_Product(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dal.GetProductList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_Product(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_Product(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:10%'>产品大类</th>"
                           + "<th style='width:10%'>产品类型</th>"
                           + "<th style='width:10%'>产品编号</th>"
                           + "<th style='width:20%'>产品名称</th>"
                           + "<th style='width:20%'>状态</th>"
                           + "<th style='width:20%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["MainProductType"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ProductTypeName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ProductNo"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ProductName"] + "</td>");
                    var status = ((dt.Rows[i]["Status"].ToString() == "0") ? "无效" : "有效");
                    tblHtml.Append("<td>" + status + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a><a class='ml_20' href='javascript:void(0)' onclick=\"DeleteProduct(" + dt.Rows[i]["ID"] + ")\">删除</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 产品服务对象列表
        private string getRecordPage_ProductServiceObject(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dal.GetProductServiceObjectList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_ProductServiceObject(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_ProductServiceObject(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:10%'>产品名称</th>"
                           + "<th style='width:10%'>产品类型</th>"
                           + "<th style='width:20%'>护理等级</th>"
                           + "<th style='width:10%'>对象特性描述</th>"
                           + "<th style='width:20%'>产品效果描述</th>"
                           + "<th style='width:20%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ProductName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ProductTypeName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["NurseLevel"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ObjectRemark"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["EffectRemark"] + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a><a class='ml_20' href='javascript:void(0)' onclick=\"DeleteProductServiceObject(" + dt.Rows[i]["ID"] + ")\">删除</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 产品技能要求列表
        private string getRecordPage_ProductSkillRequired(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dal.GetProductSkillRequiredList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_ProductSkillRequired(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_ProductSkillRequired(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:10%'>产品名称</th>"
                           + "<th style='width:10%'>产品类型</th>"
                           + "<th style='width:20%'>技能要求</th>"
                           + "<th style='width:30%'>产品效果描述</th>"
                           + "<th style='width:20%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ProductName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ProductTypeName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["SkillRequired"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["RequiredRemark"] + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a><a class='ml_20' href='javascript:void(0)' onclick=\"DeleteProductSkillRequired(" + dt.Rows[i]["ID"] + ")\">删除</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 产品价格列表
        private string getRecordPage_ProductPrice(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dal.GetProductPriceList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_ProductPrice(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_ProductPrice(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:20%'>产品名称</th>"
                           + "<th style='width:10%'>价格</th>"
                           + "<th style='width:15%'>开始日期</th>"
                           + "<th style='width:15%'>结束日期</th>"
                           + "<th style='width:10%'>状态</th>"
                           + "<th style='width:20%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ProductName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Amount"] + "</td>");
                    tblHtml.Append("<td>" + (Convert.ToDateTime(dt.Rows[i]["BeginDate"]).ToString("yyyy-MM-dd")) + "</td>");
                    tblHtml.Append("<td>" + (Convert.ToDateTime(dt.Rows[i]["EndDate"]).ToString("yyyy-MM-dd")) + "</td>");
                    var status = ((dt.Rows[i]["Status"].ToString() == "0") ? "无效" : "有效");
                    tblHtml.Append("<td>" + status + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a><a class='ml_20' href='javascript:void(0)' onclick=\"DeleteProductPrice(" + dt.Rows[i]["ID"] + ")\">删除</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 预订服务列表
        private string getRecordPage_ServiceReserve(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dal.GetServiceReserveList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_ServiceReserve(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_ServiceReserve(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:5%'>序号</th>"
                           + "<th style='width:10%'>产品名称</th>"
                           + "<th style='width:5%'>产品编号</th>"
                           + "<th style='width:10%'>客人姓名</th>"
                           + "<th style='width:5%'>楼层</th>"
                           + "<th style='width:5%'>房间</th>"
                           + "<th style='width:10%'>申请日期</th>"
                           + "<th style='width:10%'>开始日期</th>"
                           + "<th style='width:10%'>结束日期</th>"
                           + "<th style='width:5%'>状态</th>"
                           + "<th style='width:25%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ProductName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ProductNo"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Name"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["FloorID"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["RoomNo"] + "</td>");
                    tblHtml.Append("<td>" + (Convert.ToDateTime(dt.Rows[i]["ApplyDate"]).ToString("yyyy-MM-dd")) + "</td>");
                    tblHtml.Append("<td>" + (Convert.ToDateTime(dt.Rows[i]["BeginDate"]).ToString("yyyy-MM-dd")) + "</td>");
                    tblHtml.Append("<td>" + (Convert.ToDateTime(dt.Rows[i]["EndDate"]).ToString("yyyy-MM-dd")) + "</td>");
                    var status = ((dt.Rows[i]["Status"].ToString() == "0") ? "申请中" : (dt.Rows[i]["Status"].ToString() == "1") ? "执行中" : "结束");
                    tblHtml.Append("<td>" + status + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a><a class='ml_20' href='javascript:void(0)' onclick=\"FollowServiceExcute(" + dt.Rows[i]["ID"] + ")\">查看服务执行</a><a class='ml_20' href='javascript:void(0)' onclick=\"FollowServicePay(" + dt.Rows[i]["ID"] + ")\">收费详情</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 预订执行列表
        private string getRecordPage_ServiceExecute(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dal.GetServiceExecuteList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_ServiceExecute(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_ServiceExecute(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:20%'>服务人员</th>"
                           + "<th style='width:20%'>服务日期</th>"
                           + "<th style='width:40%'>服务执行说明</th>"
                           + "<th style='width:10%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Name"] + "</td>");
                    tblHtml.Append("<td>" + (Convert.ToDateTime(dt.Rows[i]["ServiceDate"]).ToString("yyyy-MM-dd")) + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Remark"] + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a><a class='ml_20' href='javascript:void(0)' onclick=\"DeleteServiceExecute(" + dt.Rows[i]["ID"] + ")\">删除</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 预订评价列表
        private string getRecordPage_ServiceEvaluation(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dal.GetServiceEvaluationList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_ServiceEvaluation(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_ServiceEvaluation(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:10%'>客人</th>"
                           + "<th style='width:10%'>产品名称</th>"
                           + "<th style='width:10%'>服务人员</th>"
                           + "<th style='width:20%'>服务日期</th>"
                           + "<th style='width:10%'>评价级别</th>"
                           + "<th style='width:20%'>评价日期</th>"
                           + "<th style='width:10%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["GuestName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ProductName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["StaffName"] + "</td>");
                    tblHtml.Append("<td>" + (Convert.ToDateTime(dt.Rows[i]["ServiceDate"]).ToString("yyyy-MM-dd")) + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Rater"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["EvaluateDate"] + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a><a class='ml_20' href='javascript:void(0)' onclick=\"DeleteServiceEvaluation(" + dt.Rows[i]["ID"] + ")\">删除</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 预订收费列表
        private string getRecordPage_ServicePay(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dal.GetServicePayList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_ServicePay(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_ServicePay(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:10%'>缴费日期</th>"
                           + "<th style='width:10%'>金额</th>"
                           + "<th style='width:10%'>费用开始日期</th>"
                           + "<th style='width:20%'>费用结束日期</th>"
                           + "<th style='width:10%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + (Convert.ToDateTime(dt.Rows[i]["PayDate"]).ToString("yyyy-MM-dd")) + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Amount"] + "</td>");
                    tblHtml.Append("<td>" + (Convert.ToDateTime(dt.Rows[i]["BeginDate"]).ToString("yyyy-MM-dd")) + "</td>");
                    tblHtml.Append("<td>" + (Convert.ToDateTime(dt.Rows[i]["EndDate"]).ToString("yyyy-MM-dd")) + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a><a class='ml_20' href='javascript:void(0)' onclick=\"DeleteServicePay(" + dt.Rows[i]["ID"] + ")\">删除</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 根据条件获取服务预订详情
        private string getServiceReserveInfoHtml(string where)
        {
            DataSet ds = dal.GetServiceReserveInfoByWhere(where);
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>产品名称</th>"
                           + "<th style='width:10%'>产品编号</th>"
                           + "<th style='width:10%'>客人姓名</th>"
                           + "<th style='width:5%'>楼层</th>"
                           + "<th style='width:10%'>房间</th>"
                           + "<th style='width:15%'>申请日期</th>"
                           + "<th style='width:15%'>开始日期</th>"
                           + "<th style='width:15%'>结束日期</th>"
                           + "<th style='width:10%'>状态</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var dt = ds.Tables[0];
                tblHtml.Append("<tr>");
                tblHtml.Append("<td>" + dt.Rows[0]["ProductName"] + "</td>");
                tblHtml.Append("<td>" + dt.Rows[0]["ProductNo"] + "</td>");
                tblHtml.Append("<td>" + dt.Rows[0]["Name"] + "</td>");
                tblHtml.Append("<td>" + dt.Rows[0]["FloorID"] + "</td>");
                tblHtml.Append("<td>" + dt.Rows[0]["RoomNo"] + "</td>");
                tblHtml.Append("<td>" + (Convert.ToDateTime(dt.Rows[0]["ApplyDate"]).ToString("yyyy-MM-dd")) + "</td>");
                tblHtml.Append("<td>" + (Convert.ToDateTime(dt.Rows[0]["BeginDate"]).ToString("yyyy-MM-dd")) + "</td>");
                tblHtml.Append("<td>" + (Convert.ToDateTime(dt.Rows[0]["EndDate"]).ToString("yyyy-MM-dd")) + "</td>");
                var status = ((dt.Rows[0]["Status"].ToString() == "0") ? "申请中" : (dt.Rows[0]["Status"].ToString() == "1") ? "执行中" : "结束");
                tblHtml.Append("<td>" + status + "</td>");
                tblHtml.Append("</tr>");
            }
            tblHtml.Append("</tbody></table></div>");
            ds = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 活动排班列表
        private string getRecordPage_ActivitySchedual(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dal.GetActivitySchedualList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_ActivitySchedual(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_ActivitySchedual(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:5%'>序号</th>"
                           + "<th style='width:10%'>产品名称</th>"
                           + "<th style='width:10%'>产品编号</th>"
                           + "<th style='width:10%'>活动日期</th>"
                           + "<th style='width:10%'>开始日期</th>"
                           + "<th style='width:10%'>结束日期</th>"
                           + "<th style='width:10%'>活动地点</th>"
                           + "<th style='width:10%'>活动负责人</th>"
                           + "<th style='width:5%'>状态</th>"
                           + "<th style='width:20%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ProductName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ProductNo"] + "</td>");
                    tblHtml.Append("<td>" + (Convert.ToDateTime(dt.Rows[i]["ActivityDate"]).ToString("yyyy-MM-dd")) + "</td>");
                    tblHtml.Append("<td>" + (Convert.ToDateTime(dt.Rows[i]["BeginDate"]).ToString("yyyy-MM-dd")) + "</td>");
                    tblHtml.Append("<td>" + (Convert.ToDateTime(dt.Rows[i]["EndDate"]).ToString("yyyy-MM-dd")) + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Place"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ActivityManager"] + "</td>");
                    var status = ((dt.Rows[i]["Status"].ToString() == "0") ? "计划" : (dt.Rows[i]["Status"].ToString() == "1") ? "完成" : "取消");
                    tblHtml.Append("<td>" + status + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a><a class='ml_20' href='javascript:void(0)' onclick=\"FollowActivityReserve(" + dt.Rows[i]["ID"] + ")\">活动预约</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 活动预约列表
        private string getRecordPage_ActivityReserve(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dal.GetActivityReserveList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_ActivityReserve(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_ActivityReserve(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:20%'>客人姓名</th>"
                           + "<th style='width:10%'>楼层</th>"
                           + "<th style='width:10%'>房间</th>"
                           + "<th style='width:10%'>申请日期</th>"
                           + "<th style='width:10%'>状态</th>"
                           + "<th style='width:30%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Name"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["FloorID"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["RoomNo"] + "</td>");
                    tblHtml.Append("<td>" + (Convert.ToDateTime(dt.Rows[i]["ApplyDate"]).ToString("yyyy-MM-dd")) + "</td>");
                    var status = ((dt.Rows[i]["Status"].ToString() == "0") ? "申请中" : (dt.Rows[i]["Status"].ToString() == "1") ? "活动完成" : (dt.Rows[i]["Status"].ToString() == "2") ? "未参加活动" : "活动取消");
                    tblHtml.Append("<td>" + status + "</td>");
                    var executeHtml = "<a class='ml_20' href='javascript:void(0)' onclick=\"ShowInsertPage('edit','execute'," + dt.Rows[i]["ID"] + ")\">执行</a>";
                    var payHtml = "<a class='ml_20' href='javascript:void(0)' onclick=\"ShowInsertPage('edit','pay'," + dt.Rows[i]["ID"] + ")\">缴费</a>";
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit',''," + dt.Rows[i]["ID"] + ")\">编辑</a>" + executeHtml + "" + payHtml + "<a class='ml_20' href='javascript:void(0)' onclick=\"DeleteActivityReserve(" + dt.Rows[i]["ID"] + ")\">删除</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 根据条件获取活动排班详情
        private string getActivitySchedualInfoHtml(string where)
        {
            DataSet ds = dal.GetActivitySchedualInfoByWhere(where);
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:15%'>产品名称</th>"
                           + "<th style='width:10%'>产品编号</th>"
                           + "<th style='width:15%'>活动日期</th>"
                           + "<th style='width:15%'>开始日期</th>"
                           + "<th style='width:15%'>结束日期</th>"
                           + "<th style='width:10%'>活动地点</th>"
                           + "<th style='width:10%'>活动负责人</th>"
                           + "<th style='width:10%'>状态</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var dt = ds.Tables[0];
                tblHtml.Append("<tr>");
                tblHtml.Append("<td>" + dt.Rows[0]["ProductName"] + "</td>");
                tblHtml.Append("<td>" + dt.Rows[0]["ProductNo"] + "</td>");
                tblHtml.Append("<td>" + (Convert.ToDateTime(dt.Rows[0]["ActivityDate"]).ToString("yyyy-MM-dd")) + "</td>");
                tblHtml.Append("<td>" + (Convert.ToDateTime(dt.Rows[0]["BeginDate"]).ToString("yyyy-MM-dd")) + "</td>");
                tblHtml.Append("<td>" + (Convert.ToDateTime(dt.Rows[0]["EndDate"]).ToString("yyyy-MM-dd")) + "</td>");
                tblHtml.Append("<td>" + dt.Rows[0]["Place"] + "</td>");
                tblHtml.Append("<td>" + dt.Rows[0]["ActivityManager"] + "</td>");
                var status = ((dt.Rows[0]["Status"].ToString() == "0") ? "计划" : (dt.Rows[0]["Status"].ToString() == "1") ? "完成" : "取消");
                tblHtml.Append("<td>" + status + "</td>");
                tblHtml.Append("</tr>");
            }
            tblHtml.Append("</tbody></table></div>");
            ds = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 物料类型列表
        private string getRecordPage_MaterielType(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dalGlobal.GetPageingRecord<MaterielType>(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_MaterielType(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_MaterielType(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:10%'>物料类型编号</th>"
                           + "<th style='width:20%'>物料类型名称</th>"
                           + "<th style='width:40%'>说明</th>"
                           + "<th style='width:20%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["TypeNo"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["TypeName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Remark"] + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 供应商列表
        private string getRecordPage_Supplier(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dalGlobal.GetPageingRecord<Supplier>(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_Supplier(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_Supplier(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:20%'>供应商简写</th>"
                           + "<th style='width:20%'>供应商名称</th>"
                           + "<th style='width:20%'>联系人</th>"
                           + "<th style='width:20%'>联系电话</th>"
                           + "<th style='width:10%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ShortName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Name"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ContactName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Phone"] + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 物料列表
        private string getRecordPage_Materiel(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dal.GetMaterielList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_Materiel(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_Materiel(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:10%'>物料类型</th>"
                           + "<th style='width:10%'>物料编号</th>"
                           + "<th style='width:20%'>名称</th>"
                           + "<th style='width:10%'>是否消耗品</th>"
                           + "<th style='width:10%'>供应商</th>"
                           + "<th style='width:10%'>价格</th>"
                           + "<th style='width:10%'>状态</th>"
                           + "<th style='width:10%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["TypeName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["MaterielNo"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Name"] + "</td>");
                    var isConsumable = ((dt.Rows[i]["IsConsumable"].ToString() == "0") ? "否" : "是");
                    tblHtml.Append("<td>" + isConsumable + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["SupplierName"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Price"] + "</td>");
                    var status = ((dt.Rows[i]["Status"].ToString() == "0") ? "无效" : "有效");
                    tblHtml.Append("<td>" + status + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 申请新物料列表
        private string getRecordPage_MaterielApply(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dalGlobal.GetPageingRecord<MaterielApply>(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_MaterielApply(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_MaterielApply(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:20%'>物料名称</th>"
                           + "<th style='width:10%'>申请部门</th>"
                           + "<th style='width:20%'>申请日期</th>"
                           + "<th style='width:10%'>申请人</th>"
                           + "<th style='width:10%'>数量</th>"
                           + "<th style='width:10%'>状态</th>"
                           + "<th style='width:10%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Name"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ApplyDept"] + "</td>");
                    tblHtml.Append("<td>" +(Convert.ToDateTime( dt.Rows[i]["ApplyDate"]).ToString("yyyy-MM-dd")) + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ApplyPeople"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Quantity"] + "</td>");
                    var status = ((dt.Rows[i]["Status"].ToString() == "0") ? "处理中" : (dt.Rows[i]["Status"].ToString() == "1") ? "申请完成" : "申请驳回");
                    tblHtml.Append("<td>" + status + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 采购申请列表
        private string getRecordPage_PurchaseApply(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dalGlobal.GetPageingRecord<PurchaseApply>(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_PurchaseApply(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_PurchaseApply(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:10%'>申请部门</th>"
                           + "<th style='width:10%'>申请人</th>"
                           + "<th style='width:15%'>申请日期</th>"
                           + "<th style='width:10%'>状态</th>"
                           + "<th style='width:10%'>总金额</th>"
                           + "<th style='width:20%'>申请说明</th>"
                           + "<th style='width:15%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ApplyDept"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ApplyPeople"] + "</td>");
                    tblHtml.Append("<td>" + (Convert.ToDateTime(dt.Rows[i]["ApplyDate"]).ToString("yyyy-MM-dd")) + "</td>");
                    var statusCode = dt.Rows[i]["Status"].ToString();
                    var status = ((statusCode == "0") ? "申请中" : (statusCode == "1") ? "申请完成" : (statusCode == "2") ? "处理中" : "申请驳回");
                    tblHtml.Append("<td>" + status + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["TotalPrice"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Remark"] + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 物料入库列表
        private string getRecordPage_StoreManage(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dal.GetStoreManageList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_StoreManage(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_StoreManage(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:20%'>物料</th>"
                           + "<th style='width:10%'>入库数量</th>"
                           + "<th style='width:10%'>入库金额</th>"
                           + "<th style='width:10%'>入库人</th>"
                           + "<th style='width:20%'>入库日期</th>"
                           + "<th style='width:10%'>状态</th>"
                           + "<th style='width:10%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Name"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["StoreQuantity"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["StorePrice"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["StorePeopleName"] + "</td>");
                    tblHtml.Append("<td>" + (Convert.ToDateTime(dt.Rows[i]["StoreDate"]).ToString("yyyy-MM-dd")) + "</td>");
                    var status = ((dt.Rows[i]["Status"].ToString() == "0") ? "入库中" : "入库完成");
                    tblHtml.Append("<td>" + status + "</td>");
                    var actions = string.Empty;
                    if (status == "入库中")
                    {
                        actions += "<a href='javascript:void(0)' onclick=\"ShowInsertPage('edit'," + dt.Rows[i]["ID"] + ")\">编辑</a>";
                        actions += "<a class='ml_20' href='javascript:void(0)' onclick=\"ConfirmStored(" + dt.Rows[i]["ID"] + ")\">确认物料入库</a>";
                    }
                    if (status == "入库完成")
                    {
                        actions += "完成";
                    }
                    tblHtml.Append("<td>" + actions + "</td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 申请完成物料采购明细
        private string getRecordPage_PurchaseApplyDetail(string where)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dal.GetPurchaseApplyDetailList(where, sortname);
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:10%'>申请部门</th>"
                           + "<th style='width:10%'>申请日期</th>"
                           + "<th style='width:20%'>物料</th>"
                           + "<th style='width:10%'>单价</th>"
                           + "<th style='width:10%'>待入库数量</th>"
                           + "<th style='width:10%'>已入库数量</th>"
                           + "<th style='width:10%'>状态</th>"
                           + "<th style='width:10%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            DataTable dt = ds.Tables[0];
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var price = dt.Rows[i]["Price"];
                    var name = dt.Rows[i]["Name"];
                    var storingQuantity = dt.Rows[i]["StoringQuantity"];
                    tblHtml.Append("<tr Price='" + price + "' Name='" + name + "' StoringQuantity='" + storingQuantity + "' IsConsumable='" + dt.Rows[i]["IsConsumable"] + "' MaterielID='" + dt.Rows[i]["MaterielID"] + "' PurchaseApplyID='" + dt.Rows[i]["PurchaseApplyID"] + "'>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["ApplyDept"] + "</td>");
                    tblHtml.Append("<td>" + (Convert.ToDateTime(dt.Rows[i]["ApplyDate"]).ToString("yyyy-MM-dd")) + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Name"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Price"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["StoringQuantity"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["StoredQuantity"] + "</td>");
                    tblHtml.Append("<td>申请完成</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick='ChooseDetail(this)'>入库</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 物料领用列表
        private string getRecordPage_UseManage(int currentpage, int pagesize, string where, string sortfield, string sorttype)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = " a.ID ";
            DataSet ds = dal.GetUseManageList(currentpage, pagesize, where, sortname + sorttype);
            int totalrow = 0;
            int totalpage = 0;
            if (ds != null)
            {
                if (ds.Tables.Count >= 2)
                {
                    totalrow = Convert.ToInt16(ds.Tables[0].Rows[0][0]);
                    totalpage = Convert.ToInt16(Math.Ceiling(1.0 * totalrow / pagesize));
                    dataPage.Append(getTable_UseManage(ds.Tables[1], sortfield, sorttype));
                    dataPage.Append(CommTools.getNav(currentpage, totalpage, totalrow, pagesize, false));
                }
            }
            return dataPage.ToString();
        }

        private string getTable_UseManage(DataTable dt, string sortfield, string sorttype)
        {
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:15%'>物料</th>"
                           + "<th style='width:10%'>领用数量</th>"
                           + "<th style='width:15%'>领用人</th>"
                           + "<th style='width:20%'>领用日期</th>"
                           + "<th style='width:30%'>备注</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tblHtml.Append("<tr>");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Name"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["UseQuantity"] + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["UsePeopleName"] + "</td>");
                    tblHtml.Append("<td>" + (Convert.ToDateTime(dt.Rows[i]["UseDate"]).ToString("yyyy-MM-dd")) + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Remark"] + "</td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        #region 物料库存明细
        private string getRecordPage_MaterielStock(string where)
        {
            StringBuilder dataPage = new StringBuilder();
            string sortname = string.Empty;
            DataSet ds = dal.GetMaterielStockList(where, sortname);
            StringBuilder tblHtml = new StringBuilder();
            tblHtml.Append("<div id='div_maindata' class='xl_container_bingrenlist'  >"
              + "<table cellspacing='0' cellpadding='0' class='list_tb'>"
              + "<tr class=\"\" >");
            tblHtml.Append("  <th style='width:10%'>序号</th>"
                           + "<th style='width:30%'>物料</th>"
                           + "<th style='width:20%'>单价</th>"
                           + "<th style='width:20%'>库存</th>"
                           + "<th style='width:20%'>操作</th>"
                           );
            tblHtml.Append("</tr><tbody id=wjtbl>");
            DataTable dt = ds.Tables[0];
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var name = dt.Rows[i]["Name"];
                    var stockQuantity = dt.Rows[i]["StockQuantity"];
                    tblHtml.Append("<tr Name='" + name + "' StockQuantity='" + stockQuantity + "' MaterielID='" + dt.Rows[i]["MaterielID"] + "' >");
                    tblHtml.Append("<td>" + (i + 1).ToString() + "</td>");
                    tblHtml.Append("<td>" + name + "</td>");
                    tblHtml.Append("<td>" + dt.Rows[i]["Price"] + "</td>");
                    tblHtml.Append("<td>" + stockQuantity + "</td>");
                    tblHtml.Append("<td><a href='javascript:void(0)' onclick='ChooseDetail(this)'>领用</a></td>");
                    tblHtml.Append("</tr>");
                }
            }
            tblHtml.Append("</tbody></table></div>");
            dt = null;
            return tblHtml.ToString();
        }
        #endregion

        public string GetClassBySchedual(string text)
        {
            string bdClass = "";
            if (text == "白")
            {
                bdClass = "bg_white";
            }
            if (text == "夜")
            {
                bdClass = "bg_blue";
            }
            if (text == "两")
            {
                bdClass = "bg_green";
            }
            if (text == "洗澡")
            {
                bdClass = "bg_lithtred";
            }
            if (text == "打扫")
            {
                bdClass = "bg_purple";
            }
            return bdClass;
        }

        public string GetSchedualByText(string text)
        {
            string schedualInfo = string.Empty;
            schedualInfo = (text == "白") ? "06:00-18:00"
                                : (text == "夜") ? "18:00-06:00"
                                : (text == "两") ? "18:00-22:00/06:00-08:00"
                                : (text == "休") ? "休" : "";
            return schedualInfo;
        }

        public string GetSchedualInfoHtml(string strChooseMonth, string paramStaffNo, string paramRooms)
        {
            string strhtml = string.Empty;
            DataSet ds = new DataSet();
            var where_beginDate = Convert.ToDateTime(strChooseMonth + "-01");
            var where_endDate = where_beginDate.AddMonths(1).AddDays(-1);
            if (!string.IsNullOrWhiteSpace(paramRooms))
            {
                paramRooms = paramRooms.TrimEnd(',');
            }
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" and a.DutyDate>= '{0}' ", where_beginDate.ToString("yyyy-MM-dd"));
            strWhere.AppendFormat(" and a.DutyDate<= '{0}' ", where_endDate.ToString("yyyy-MM-dd"));
            if (!string.IsNullOrWhiteSpace(paramStaffNo))
            {
                strWhere.AppendFormat(" and a.StaffNo = '{0}' ", paramStaffNo);
            }
            if (!string.IsNullOrWhiteSpace(paramRooms))
            {
                if (paramRooms.Split(',').Length > 0)
                {
                    for (int i = 0; i < paramRooms.Split(',').Length; i++)
                    {
                        var tempRoomNo = paramRooms.Split(',')[i];
                        if (i == 0)
                        {
                            strWhere.AppendFormat(" and a.RoomNo in ('{0}' ", tempRoomNo);
                        }
                        else
                        {
                            strWhere.AppendFormat(" ,'{0}' ", tempRoomNo);
                        }
                    }
                    strWhere.Append(" ) ");
                }
            }
            ds = dal.GetSchedualInfo(strWhere.ToString());
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var dt = ds.Tables[0];
                string strBeginDate = dt.Select().Min(p => p["DutyDate"]).ToString();
                string strEndDate = dt.Select().Max(p => p["DutyDate"]).ToString();
                var beginDate = Convert.ToDateTime(strBeginDate);
                var endDate = Convert.ToDateTime(strEndDate);
                string[] weekdays = { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };
                StringBuilder tblHtml = new StringBuilder();
                tblHtml.Append("<table cellspacing='0' cellpadding='0' class='list_tb tb_fix'>");
                #region 处理页面布局问题
                StringBuilder tr_cols = new StringBuilder();
                tr_cols.Append("<col style='width:50px'/>");
                tr_cols.Append("<col style='width:100px'/>");
                for (int i = 0; i <= (endDate - beginDate).Days; i++)
                {
                    tr_cols.Append("<col style='width:130px'/>");
                }
                tblHtml.Append(tr_cols);
                #endregion

                #region 眉头
                StringBuilder tr_First = new StringBuilder();
                tr_First.Append("<tr><th colspan='2'>日期</th>");
                StringBuilder tr_Second = new StringBuilder();
                tr_Second.Append("<tr><th colspan='2'>姓名</th>");
                for (int i = 0; i <= (endDate - beginDate).Days; i++)
                {
                    var date = beginDate.AddDays(i).ToString("MM-dd");
                    string week = weekdays[Convert.ToInt32(beginDate.AddDays(i).DayOfWeek)];
                    tr_First.AppendFormat("<th>{0}</th>", date);
                    tr_Second.AppendFormat("<th>{0}</th>", week);
                }
                tr_First.Append("</tr>");
                tr_Second.Append("</tr>");
                tblHtml.Append(tr_First);
                tblHtml.Append(tr_Second);
                #endregion

                #region 正文
                var rows = dt.Select("", "DutyDate").GroupBy(p => (p["StaffNo"] + "|" + p["RoomNo"]));
                List<string> containRoom = new List<string>();
                foreach (var item in rows)
                {
                    string key = item.Key.ToString();
                    string staffNo = key.Split('|')[0];
                    string roomNo = key.Split('|')[1];
                    string staffName = item.First()["Name"].ToString();
                    StringBuilder tr = new StringBuilder();
                    tr.Append("<tr>");
                    //确定下该房间需要几个人护理
                    int rowspan = 0;
                    rowspan = dt.Select("RoomNo='" + roomNo + "'").GroupBy(p => p["StaffNo"]).Count();
                    if (!containRoom.Contains(roomNo))
                    {
                        containRoom.Add(roomNo);
                        var strRooms = "";
                        for (int n = 0; n < roomNo.Split(',').Length; n++)
                        {
                            strRooms += roomNo.Split(',')[n] + "</br>";
                        }
                        tr.AppendFormat("<td rowspan='{0}'><span roomNo='{1}'>{2}</span><select style='display: none;'></select></td>", rowspan, roomNo, strRooms);
                    }
                    tr.AppendFormat("<td><span staffNo='{1}'>{0}</span></td>", staffName, staffNo);

                    for (int j = 0; j <= (endDate - beginDate).Days; j++)
                    {
                        string schedualInfo = string.Empty;
                        string remark = string.Empty;
                        var getScheInfo = item.ToList().Find(p => Convert.ToDateTime(p["DutyDate"]) == beginDate.AddDays(j));
                        if (getScheInfo != null)
                        {
                            schedualInfo = item.ToList().First(p => Convert.ToDateTime(p["DutyDate"]) == beginDate.AddDays(j))["Schedual"].ToString();
                            remark = item.ToList().First(p => Convert.ToDateTime(p["DutyDate"]) == beginDate.AddDays(j))["Remark"].ToString();
                        }
                        tr.AppendFormat("<td ondblclick='ChangeSchedual(this)'><span class='period'>{0}</span></br><span class='remark'>{1}</span></td>", schedualInfo, remark);
                    }
                    tr.Append("</tr>");
                    tblHtml.Append(tr);
                }
                #endregion

                tblHtml.Append("</table>");
                strhtml = tblHtml.ToString();
            }
            return strhtml;
        }

    }
}