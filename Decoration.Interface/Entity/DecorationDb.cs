using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decoration.Interface.Entity
{
    /// <summary>
    /// 数据库信息
    /// 奚德荣 2018.10.19
    /// </summary>
    public class DecorationDb
    {
        #region 业务表
        

        /// <summary>
        /// 表-活动推送
        /// </summary>
        public const String Table_Decoration_Activity = "decoration_activity";

        /// <summary>
        /// 表-首页轮换图
        /// </summary>
        public const String Table_Decoration_Banner = "decoration_banner";

        /// <summary>
        /// 表-精选案例
        /// </summary>
        public const String Table_Decoration_Case = "decoration_case";

        /// <summary>
        /// 表-精选案例-角色
        /// </summary>
        public const String Table_Decoration_Case_Role = "decoration_case_role";

        /// <summary>
        /// 表-业主评论
        /// </summary>
        public const String Table_Decoration_Comment = "decoration_comment";

        /// <summary>
        /// 表-装修企业
        /// </summary>
        public const String Table_Decoration_Company = "decoration_company";

        /// <summary>
        /// 表-企业人员角色
        /// </summary>
        public const String Table_Decoration_Companyrole = "decoration_companyrole";

        /// <summary>
        /// 表-企业施工任务
        /// </summary>
        public const String Table_Decoration_Companytask = "decoration_companytask";

        /// <summary>
        /// 表-企业任务分类（阶段）
        /// </summary>
        public const String Table_Decoration_Companytaskcate = "decoration_companytaskcate";

        /// <summary>
        /// 表-企业用户
        /// </summary>
        public const String Table_Decoration_Companyuser = "decoration_companyuser";

        /// <summary>
        /// 表-企业用户-企业人员角色
        /// </summary>
        public const String Table_Decoration_Companyuser_role = "decoration_companyuser_role";

        /// <summary>
        /// 表-设计档案
        /// </summary>
        public const String Table_Decoration_Doc = "decoration_doc";

        /// <summary>
        /// 表-任务执行记录
        /// </summary>
        public const String Table_Decoration_Execute = "decoration_execute";

        /// <summary>
        /// 表-工程材料申请
        /// </summary>
        public const String Table_Decoration_Materialapply = "decoration_materialapply";

        /// <summary>
        /// 表-企业模块菜单
        /// </summary>
        public const String Table_Decoration_Module = "decoration_module";

        /// <summary>
        /// 表-产品信息
        /// </summary>
        public const String Table_Decoration_Product = "decoration_product";

        /// <summary>
        /// 表-产品分类
        /// </summary>
        public const String Table_Decoration_Productcate = "decoration_productcate";

        /// <summary>
        /// 表-工程项目
        /// </summary>
        public const String Table_Decoration_Project = "decoration_project";

        /// <summary>
        /// 表-服务团队
        /// </summary>
        public const String Table_Decoration_Projectteam = "decoration_projectteam";

        /// <summary>
        /// 表-签到记录
        /// </summary>
        public const String Table_Decoration_Signin = "decoration_signin";

        /// <summary>
        /// 表-系统公告
        /// </summary>
        public const String Table_Decoration_Sysmsg = "decoration_sysmsg";

        /// <summary>
        /// 表-公告发送对象
        /// </summary>
        public const String Table_Decoration_Sysmsg_role = "decoration_sysmsg_role";

        /// <summary>
        /// 表-项目施工任务
        /// </summary>
        public const String Table_Decoration_Task = "decoration_task";

        /// <summary>
        /// 表-项目任务阶段
        /// </summary>
        public const String Table_Decoration_Taskcate = "decoration_taskcate";

        /// <summary>
        /// 表-任务所属角色
        /// </summary>
        public const String Table_Decoration_Taskrole = "decoration_taskrole";

        /// <summary>
        /// 表-项目提醒
        /// </summary>
        public const String Table_Decoration_Tip = "decoration_tip";


        /// <summary>
        /// 表-企业施工任务模板
        /// </summary>
        public const String Table_Decoration_TaskTemplate = "decoration_tasktemplate";


        /// <summary>
        /// 表-报备客户信息
        /// </summary>
        public const String Table_Decoration_Potential = "decoration_potential";
        public const String Table_Decoration_VPotential = "decoration_vpotential";

        /// <summary>
        /// 表-前端模块
        /// </summary>
        public const String Table_Decoration_FrontModule = "decoration_frontmodule";

        /// <summary>
        /// 表-前端授权模块
        /// </summary>
        public const String Table_Decoration_Role_FrontModule = "decoration_role_frontmodule";
        /// <summary>
        /// 表-可提醒角色
        /// </summary>
        public const String Table_Decoration_Role_TipRole = "decoration_role_tiprole";


        /// <summary>
        /// 表-平台角色
        /// </summary>
        public const String Table_Decoration_sys_role = "decoration_sys_role";

        /// <summary>
        /// 表-平台提醒角色
        /// </summary>
        public const String Table_Decoration_sys_role_tiprole = "decoration_sys_role_tiprole";

        /// <summary>
        /// 表-平台模板
        /// </summary>
        public const String Table_Decoration_sys_template = "decoration_sys_template";

        /// <summary>
        /// 表-平台类型
        /// </summary>
        public const String Table_Decoration_sys_taskcate = "decoration_sys_taskcate";

        /// <summary>
        /// 表-平台任务
        /// </summary>
        public const String Table_Decoration_sys_task = "decoration_sys_task";


        /// <summary>
        /// 表-平台角色
        /// </summary>
        public const String Table_Decoration_sys_taskrole = "decoration_sys_taskrole";

        #endregion

        #region 视图
        /// <summary>
        /// 视图-产品活动
        /// </summary>
        public const String View_Decoration_Activity = "decoration_vactivity";







        #endregion




    }
}
