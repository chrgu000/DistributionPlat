﻿using NFine.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Distribution.Model;
using Distribution.Logic;
using Distribution.Web.Models;

namespace Distribution.Web.Controllers
{
    public class CustomerController : BasicController
    {
        // GET: Product
        public ActionResult Index(ProductModel model)
        {
            var UserInfo = NFine.Code.OperatorProvider.Provider.GetCurrent();
            if (UserInfo == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ProductModel viewModel = new ProductModel();
            if (base.agentInfo != null)
            {
                CommLogic.DeepClone<AgentInfoModel>(viewModel, agentInfo);
            }
            return View(viewModel);
        }
        
        public ActionResult Test()
        {
            return View();
        }

        /// <summary>
        /// 获取客户关系数据
        /// </summary>
        /// <returns></returns>
        public string data()
        {
            string ageId = agentInfo.agent.c_id;


            RelationModel relation = new RelationModel();
            relation.name = agentInfo.agent.c_name;
            relation.number = agentInfo.agent.c_mobile;
            List<RelationModel> children = new List<RelationModel>();
            var list = AgentRelationLogic.GetFirstCustomer(ageId);
            foreach (var item in list)
            {
                if(item != null )
                {
                    RelationModel m = new RelationModel();
                    m.name = item.c_name;
                    m.number = item.c_mobile;
                    m.level = CommConfigLogic.GetValueFromConfig((int)ConfigCategory.PostitionLevel, item.c_levle);
                    List<RelationModel> secondList = new List<RelationModel>();
                    var sList = AgentRelationLogic.GetFirstCustomer(item.c_id);
                    if(sList.Count() > 0 )
                    {
                        foreach (var second in sList)
                        {
                            if (second != null)
                            {
                                RelationModel m2 = new RelationModel();
                                m2.name = second.c_name;
                                m2.number = second.c_mobile;
                                m2.level = CommConfigLogic.GetValueFromConfig((int)ConfigCategory.PostitionLevel, second.c_levle);
                                secondList.Add(m2);
                            }
                        }
                        m.children = secondList;
                    }
                    children.Add(m);
                }
               
            }
            relation.children = children;

            string json = relation.ToJson();
            return json;
        }
        
        
    }
}