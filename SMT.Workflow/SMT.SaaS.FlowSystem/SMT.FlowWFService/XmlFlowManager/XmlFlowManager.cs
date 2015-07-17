using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SMT.FlowWFService.XmlFlowManager
{
   public static class XMLFlowManager
    {
        /// <summary>
        /// 根据XML定义文件创建流程
        /// </summary>
        /// <param name="strFlowXml"></param>
        /// <returns></returns>
        public static WF1_WorkFlow CreateWFInstanceFromXmlDefine(string strFlowXml)
        {
            WF1_WorkFlow workflowInstanc = new WF1_WorkFlow();
            XElement FlowDefine = XDocument.Parse(strFlowXml).Root;

            var FlowDefineActivitys = (from ent in FlowDefine.Elements()
                                        where ent.Name == "Activitys"
                                   select ent).ToList();
           
            foreach(var item in FlowDefineActivitys.Elements())
            {
                WF2_WorkFlowActivitysActivity acivity= new WF2_WorkFlowActivitysActivity();
                acivity.Name = item.Attribute("Name").Value;
                acivity.X = item.Attribute("X").Value;
                acivity.Y = item.Attribute("Y").Value;
                acivity.RoleName = item.Attribute("RoleName").Value;
                acivity.UserType = item.Attribute("UserType").Value;
                acivity.IsOtherCompany = item.Attribute("IsOtherCompany").Value;
                acivity.OtherCompanyID = item.Attribute("OtherCompanyID").Value;
                acivity.OtherCompanyName = item.Attribute("OtherCompanyName").Value;
                acivity.Remark = item.Attribute("Remark").Value;
                
                var CountersignsList = from ent in item.Elements()
                                         where ent.Name == "Countersigns"
                                                   select ent;
                if (CountersignsList.Count() > 0)
                {
                    foreach (var CountersignsItem in CountersignsList)
                    {
                        WF21_WorkFlowActivitysActivityCountersigns Countersigns = new WF21_WorkFlowActivitysActivityCountersigns();
                        Countersigns.CountersignType = CountersignsItem.Attribute("CountersignType").Value;
                        var CountersignChildList = from ent in CountersignsItem.Elements()
                                               where ent.Name == "Countersign"
                                               select ent;
                        if(CountersignChildList.Count()>0)
                        {
                            foreach(var CountersignChild in CountersignChildList)
                            {
                                WF211_WorkFlowActivitysActivityCountersignsCountersign Countersign = new             WF211_WorkFlowActivitysActivityCountersignsCountersign();
                                Countersign.StateType = CountersignChild.Attribute("StateType").Value;
                                Countersign.RoleName = CountersignChild.Attribute("RoleName").Value;
                                Countersign.IsOtherCompany = CountersignChild.Attribute("IsOtherCompany").Value;
                                Countersign.OtherCompanyID = CountersignChild.Attribute("OtherCompanyID").Value;
                                Countersign.OtherCompanyName = CountersignChild.Attribute("OtherCompanyName").Value;
                                Countersign.UserType = CountersignChild.Attribute("UserType").Value;
                                Countersign.UserTypeName = CountersignChild.Attribute("UserTypeName").Value;
                                //添加会签角色子节点
                                Countersigns.Countersign.Add(Countersign);
                            }
                        }
                        //添加会签节点
                        acivity.Countersigns.Add(Countersigns);
                    }
                }
                //添加活动节点
                workflowInstanc.Activitys.Add(acivity);
            }

            //workflowInstanc.Activitys

            var FlowDefineRules = (from ent in FlowDefine.Elements()
                                     where ent.Name == "Rules"
                                                select ent).ToList();
            foreach (var item in FlowDefineRules.Elements())
            {
                WF3_WorkFlowRulesRule role = new WF3_WorkFlowRulesRule();
                role.Name = item.Attribute("Name").Value;
                role.Remark = item.Attribute("Remark").Value;
                role.StrStartActive = item.Attribute("StrStartActive").Value;
                role.StrEndActive = item.Attribute("StrEndActive").Value;

                var conditionList = from ent in item.Elements()
                                    where ent.Name=="Conditions"
                                 select ent;
                if (conditionList.Count() > 0)
                {
                    foreach (var conditionItem in conditionList)
                    {
                        WF31_WorkFlowRulesRuleConditions condition = new WF31_WorkFlowRulesRuleConditions();
                        condition.Name = conditionItem.Attribute("Name").Value;
                        condition.Object = conditionItem.Attribute("Object").Value;
                        condition.CodiCombMode = conditionItem.Attribute("CodiCombMode").Value;
                        //condition.Condition = conditionItem.Attribute("Condition").Value;
                          var conditionChildList = from ent in conditionItem.Elements()
                                                   where ent.Name=="Condition"
                                                   select ent;
                        if(conditionChildList.Count()>0)
                        {
                            foreach(var ChildConditionItem in conditionChildList)
                            {
                                WF311_WorkFlowRulesRuleConditionsCondition
                                conditionChild = new WF311_WorkFlowRulesRuleConditionsCondition();
                                conditionChild.Name = ChildConditionItem.Attribute("Name").Value;
                                conditionChild.Description = ChildConditionItem.Attribute("Description").Value;
                                conditionChild.CompAttr = ChildConditionItem.Attribute("CompAttr").Value;
                                conditionChild.DataType = ChildConditionItem.Attribute("DataType").Value;
                                conditionChild.Operate = ChildConditionItem.Attribute("Operate").Value;
                                conditionChild.CompareValueMark = ChildConditionItem.Attribute("CompareValueMark").Value;
                                conditionChild.CompareValue = ChildConditionItem.Attribute("CompareValue").Value;
                                //添加子条件描述至条件中
                                condition.Condition.Add(conditionChild);
                            }
                        }
                        //添加条件描述至roles
                        role.Conditions.Add(condition);
                    }
                }
                //添加活动连线规则
                workflowInstanc.Rules.Add(role);
              }
            return workflowInstanc;
        }

        /// <summary>
        /// 获取流程下一节点角色
        /// </summary>
        /// <param name="WfDefine"></param>
        /// <param name="strStartActive"></param>
        /// <param name="BussinessObj"></param>
        /// <returns></returns>
        public static string GetNextStepRoles(WF1_WorkFlow workFlowDefine, string strStartActive, string BussinessObj)
        {

            var WfDefine = workFlowDefine;

            List<string> rolesList = new List<string>();
            var nextRoles = from ent in WfDefine.Rules
                            where ent.StrStartActive == strStartActive
                            select ent;
            int i = nextRoles.Count();
            //获取从传入的节点为开始的所有连线
            if (i > 0)
            {
                //只有一条连线情况下
                if (i == 1)
                {
                    //rolesList=getRoleIdsFromEndActivit(WfDefine,nextRoles.FirstOrDefault().StrEndActive);
                    return nextRoles.FirstOrDefault().StrEndActive;
                }
                else
                {
                    //拥有2个以上的连线，说明带有条件
                    //1.获取所有条件
                    //2.遍历所有条件判断条件是否满足
                    //3.判断同样的条件情况下谁更优先

                    //用来保存最终满足的条件Role
                    List<WFCondition> ConditionList = new List<WFCondition>();
                    List<WFRoles> WFRolesList = new List<WFRoles>();
                    foreach (var role in nextRoles)
                    {
                        //如果条件成立，添加当前activit
                        CheckConditionList(role.StrStartActive, role.StrEndActive, role.Conditions, BussinessObj, ref WFRolesList, ref ConditionList);
                    }
                    //最终条件满足的
                    var active = ConditionList.FirstOrDefault();
                    return active.StrEndActive;
                    //rolesList=getRoleIdsFromEndActivit(WfDefine, active.StrEndActive);
                }
            }
            else
            {
                throw new Exception("未找到下一节点：" + strStartActive);
            }
        }

       public  static string GetRolesFromActivit()
        {
           string rolesid=string.Empty;
           return rolesid;
        }

        #region 根据结束的Activit获取角色id
        private static List<string> getRoleIdsFromActivit(WF1_WorkFlow WfDefine, string ActivitId)
        {
            List<string> roleIds = new List<string>();
            var rolesList = from ent in WfDefine.Activitys
                            where ent.Name == ActivitId
                            select ent;
            if (rolesList.Count() > 0)
            {
                var ActiviObj = rolesList.FirstOrDefault();
                //存在会签的情况下
                if (ActiviObj.Countersigns.Count() > 0)
                {
                    var Countersign = ActiviObj.Countersigns[0];
                    if (Countersign.CountersignType == "0")
                    {
                        //会签并要求所有人都要终审通过
                        foreach (var CountersignChild in Countersign.Countersign)
                        {
                            //获取所有角色id
                            roleIds.Add(CountersignChild.UserType);
                        }
                    }
                }
                else
                {
                    //不存在会签，直接返回roleid
                    roleIds.Add(ActiviObj.RoleName);
                }
            }
            return roleIds;
        }
        #endregion

        #region 条件判断

        private static void CheckConditionList(string StrStartActive, string StrEndActive, List<WF31_WorkFlowRulesRuleConditions> conditions, string BussinessObj,ref  List<WFRoles> WFRolesList, ref  List<WFCondition> ConditionList)
        {
            bool flag = true;
            foreach (var conditionItem in conditions)
            {
                switch(conditionItem.CodiCombMode.ToUpper())
                {
                    case "AND":
                        flag = (flag && CheckCondition(StrStartActive, StrEndActive,conditionItem.Condition.FirstOrDefault(), BussinessObj, ref ConditionList));
                        break;
                    case "OR":
                        flag = (flag || CheckCondition(StrStartActive, StrEndActive,conditionItem.Condition.FirstOrDefault(), BussinessObj, ref ConditionList));
                        break;
                }
            }
            if(flag)
            {
                //条件满足，添加此对象
                WFRoles wfrole = new WFRoles();
                wfrole.StrEndActive = StrEndActive;
                wfrole.StrStartActive = StrStartActive;
                WFRolesList.Add(wfrole);
            }
        }

        private static bool CheckCondition(string StrStartActive, string StrEndActive, WF311_WorkFlowRulesRuleConditionsCondition conditionItem, string BussinessObj, ref  List<WFCondition> ConditionList)
        {
            bool flag = false;
            var ConditionCode = conditionItem.CompAttr;
            var ConditoinOperate = conditionItem.Operate;
            var conditionValue = conditionItem.CompareValue;
            var conditionType = conditionItem.DataType;

            WFCondition condition = new WFCondition();
            condition.StrEndActive = StrEndActive;
            condition.StrStartActive = StrStartActive;
            condition.ConditoinOperate = ConditoinOperate;
            condition.conditionValue = conditionValue;
            condition.conditionType = conditionType;

            var alreadItem = (from ent in ConditionList
                              where ent.ConditionCode == ConditionCode
                              && ent.ConditoinOperate == ConditoinOperate
                              && ent.StrStartActive == StrStartActive
                              select ent).FirstOrDefault();
            if (alreadItem != null)
            {
                switch (ConditoinOperate)
                {
                    case ">":
                    case ">=":
                        if (objToDecimal(conditionValue) > objToDecimal(alreadItem.conditionValue))
                        {
                            ConditionList.Remove(alreadItem);
                            ConditionList.Add(condition);
                        }
                        else
                        {
                            condition = alreadItem;
                        }
                        break;
                    case "<":
                    case "<=":
                        if (objToDecimal(conditionValue) < objToDecimal(alreadItem.conditionValue))
                        {
                            ConditionList.Remove(alreadItem);
                            ConditionList.Add(condition);
                        }
                        else
                        {
                            condition = alreadItem;
                        }
                        break;
                    case "like":
                    case "=":
                        break;
                }
            }
            else
            {
                ConditionList.Add(condition);
            }
            flag = CheckBussinessObj(BussinessObj, condition.ConditionCode, condition.ConditoinOperate, condition.conditionValue, condition.conditionType);
            return flag;
        }

        private static bool CheckBussinessObj(string strBussinessObj, string ConditionCode, string ConditoinOperate, string conditionValue, string conditionType)
        {
            try
            {
                XElement xmlBussinessObj = XDocument.Parse(strBussinessObj).Root;

                var BussinessObj = (from ent in xmlBussinessObj.Elements().Elements()
                                    where ent.Name == "Object"
                                    select ent).ToList();

                var BussinessDataValue = from ent in BussinessObj.Elements()
                                         where ent.Attribute("Name").Value == ConditionCode
                                         select ent.Attribute("DataValue").Value;
                if (conditionType.ToLower() == "decimal" || conditionType.ToLower() == "string")
                {
                    bool flag = false;
                    switch (ConditoinOperate)
                    {
                        case ">":
                            flag = objToDecimal(BussinessDataValue) > objToDecimal(conditionValue);
                            break;
                        case ">=":
                            flag = objToDecimal(BussinessDataValue) >= objToDecimal(conditionValue);
                            break;
                        case "<":
                            flag = objToDecimal(BussinessDataValue) < objToDecimal(conditionValue);
                            break;
                        case "<=":
                            flag = objToDecimal(BussinessDataValue) <= objToDecimal(conditionValue);
                            break;
                        case "=":
                            flag = objToDecimal(BussinessDataValue) == objToDecimal(conditionValue);
                            break;
                        default:
                            flag = false;
                            break;

                    }
                    return flag;
                }
                else
                {
                    throw new Exception("流程条件设置中存在未知比较类型，请检查");
                }
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        private static decimal objToDecimal(object obj)
        {
            try
            {
                return decimal.Parse(obj.ToString());
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}
