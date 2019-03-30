using System;
using Adf.Core.Db;
using Adf.Core.Entity;
using CYQ.Data;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using Decoration.Interface;

namespace Decoration.Implement.MySql
{
    /// <summary>
    /// 模块实现
    /// </summary>
    public class ModuleImpl : ICompanyModule
    {

        private const String CurrentTableName = "decoration_module";
        private const String VCurrentTableName = "decoration_module";

        #region Implementation of IModule

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        public MDataRow InitDataRow()
        {
            MDataRow mDataRow = new MDataRow(DBTool.GetColumns(CurrentTableName));
            return mDataRow;
        }

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="sModuleCode">模块编码</param>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        public MDataRow GetEntity(string sModuleCode, bool bCache = false)
        {
            String cacheKey = "ModuleImpl-GetEntity" + sModuleCode;
            String strWhere = "ModuleCode='" + sModuleCode + "'";
            return DbService.GetOne(VCurrentTableName, strWhere, cacheKey, false);
        }

        /// <summary>
        /// 得到一个菜单所有的子级菜单
        /// </summary>
        /// <param name="sParentCode">父级菜单编码</param>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        public MDataTable GetChildAll(string sParentCode, bool bCache = false)
        {
            String sWhere = "";
            if (!String.IsNullOrEmpty(sParentCode))
            {
                sWhere = "ModuleParentCode='" + sParentCode + "' order by moduleorder desc";
            }
            String cacheKey = "ModuleImpl-GetChildAll"+sParentCode;
            return DbService.GetTable(VCurrentTableName, 0, sWhere, cacheKey,bCache);
        }

        /// <summary>
        /// 得到当前模块所有的子级模块，包含子级模块的子级模块。
        /// 但不包含当前的父级模块
        /// </summary>
        /// <param name="sParentCode">当前模块编码</param>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        public MDataTable GetSubAll(string sParentCode, bool bCache = false)
        {
            String sWhere = "";
            if (!String.IsNullOrEmpty(sParentCode))
            {
                sWhere = "ModuleParentCode like '%" + sParentCode + "%' and ModuleCode <>" + DbService.SetQuotesValue(sParentCode);
            }
            String cacheKey = "ModuleImpl-GetChildAll" + sParentCode;
            return DbService.GetTable(VCurrentTableName, 0, sWhere, cacheKey, bCache);
        }

        /// <summary>
        /// 得到根级菜单
        /// </summary>
        /// <param name="bCache">是否启用缓存</param>
        /// <returns></returns>
        public MDataTable GetRootModuleAll(bool bCache = false)
        {
            String sWhere = "ModuleParentCode ='000'";
            String cacheKey = "ModuleImpl-GetRootModuleAll";
            return DbService.GetTable(VCurrentTableName, 0, sWhere, cacheKey, bCache);
        }

        /// <summary>
        /// 增加一个模块
        /// 子模块的前三位是父级模块
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public ExeMsgInfo Insert(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String sModuleCode = dataRow["ModuleCode"].ToString();
            String sParentCode = dataRow["ModuleParentCode"].ToString();

            //设置当前是否父节点的值为true
            dataRow["isparent"].Value = "true";

            //ValidateHelper

            if (String.IsNullOrEmpty(sModuleCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编码不能为空";
                return exeMsgInfo;
            }
            if (String.IsNullOrEmpty(sParentCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "父编码不能为空";
                return exeMsgInfo;
            }

            //判断模块是否正确

            if (sParentCode.Length + 3 != sModuleCode.Length)
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "模块编码格式有错.当前模块长度应该等于上级模块编码长度加上3位";
                return exeMsgInfo;
            }

            if (sModuleCode.Substring(0, sParentCode.Length) != sParentCode)
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "模块编码格式有错.当前模块前几位应该是上级模块编码";
                return exeMsgInfo;
            }


            String sWhere = "ModuleCode ='" + sModuleCode + "'";

            if (DbService.Exists(CurrentTableName, sWhere))
            {
                exeMsgInfo.RetStatus = 200;
                exeMsgInfo.RetValue = "当前编码已经存在";
                return exeMsgInfo;
            }

            using (MAction mAction = new MAction(CurrentTableName))
            {
                //将数据传递过来至mAction中
                mAction.Data.LoadFrom(dataRow);
                mAction.Data.SetState(2);

                mAction.Data["ModuleId"].State = 0;

                if (mAction.Insert(false, InsertOp.None))
                {
                    exeMsgInfo.RetStatus = 100;
                    exeMsgInfo.RetValue = "添加成功";
                }
                else
                {
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue = "添加失败";
                }
            }

            return exeMsgInfo;
        }

        /// <summary>
        /// 根据模块编码修改一个模块
        /// </summary>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public ExeMsgInfo UpdateByModuleCode(MDataRow dataRow)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            String sModuleCode = dataRow["ModuleCode"].ToString();
            String sParentCode = dataRow["ModuleParentCode"].ToString();

            //设置一直为父级模块
            dataRow["isparent"].Value = "true";

            if (String.IsNullOrEmpty(sModuleCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编码不能为空";
                return exeMsgInfo;
            }

            if (String.IsNullOrEmpty(sParentCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "父编码不能为空";
                return exeMsgInfo;
            }

            String sWhere = "";

            using (MAction mAction = new MAction(CurrentTableName))
            {
                //将数据传递过来至mAction中
                mAction.Data.LoadFrom(dataRow);

                mAction.Data.SetState(2);

                mAction.Data["ModuleCode"].State = 0;
                mAction.Data["ModuleId"].State = 0;

                sWhere = " ModuleCode='" + sModuleCode + "'";
                if (mAction.Update(sWhere))
                {
                    exeMsgInfo.RetStatus = 101;
                    exeMsgInfo.RetValue = "修改成功";
                }
                else
                {
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue = "修改失败";
                }
            }
            return exeMsgInfo;
        }

        /// <summary>
        /// 根据模块编码删除一个模块
        /// 这里需要加入事务，将模块相关授权的权限，模块参数进行删除.
        /// 后续补充.
        /// </summary>
        /// <param name="sModuleCode"></param>
        /// <returns></returns>
        public ExeMsgInfo DeleteByModuleCode(String sModuleCode)
        {
            ExeMsgInfo exeMsgInfo = new ExeMsgInfo();
            if (String.IsNullOrEmpty(sModuleCode))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "编码不能为空";
                return exeMsgInfo;
            }
            String sWhere = "ModuleParentCode='" + sModuleCode + "'";
            if (DbService.Exists(CurrentTableName, sWhere))
            {
                exeMsgInfo.RetStatus = 400;
                exeMsgInfo.RetValue = "当前模块存在子模块,请先删除子模块";
                return exeMsgInfo;
            }

            using (MAction mAction = new MAction(CurrentTableName))
            {
                sWhere = " ModuleCode='" + sModuleCode + "'";

                if (mAction.Delete(sWhere))
                {
                    exeMsgInfo.RetStatus = 103;
                    exeMsgInfo.RetValue = "删除成功";
                }
                else
                {
                    exeMsgInfo.RetStatus = 400;
                    exeMsgInfo.RetValue = "删除失败";
                }
            }

            return exeMsgInfo;
        }

        #endregion
    }

}
