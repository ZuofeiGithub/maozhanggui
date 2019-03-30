using Decoration.Interface;

namespace Decoration.Implement.MySql
{
    public class AppFactoryImpl: IAppFactory
    {
        /// <summary>
        /// 工程项目
        /// </summary>
        /// <returns></returns>
        public IProject Project()
        {
            return new ProjectImpl();
        }

        /// <summary>
        /// 模块
        /// </summary>
        /// <returns></returns>
        public ICompanyModule CompanyModule()
        {
            return new ModuleImpl();
        }
        /// <summary>
        /// 公司列表
        /// 王浩
        /// 20148-10-13
        /// </summary>
        /// <returns></returns>
        public ICompany Company()
        {
           return  new CompanyImpl();
        }

        /// <summary>
        /// 企业用户信息
        /// 王浩
        /// 2018-10-13
        /// </summary>
        /// <returns></returns>
        public ICompanyUser CompanyUser()
        {
            return new CompanyUserlmpl();
        }

        /// <summary>
        /// 产品分类
        /// 侯鑫辉
        /// 2018.10.17
        /// </summary>
        /// <returns></returns>
        public IProductCate ProductCate()
        {
            return new ProductCateImpl();
        }

        /// <summary>
        /// 产品信息
        /// 侯鑫辉
        /// 2018.10.17
        /// </summary>
        /// <returns></returns>
        public IProduct Product()
        {
            return new ProductImpl();
        }

        /// <summary>
        /// 活动推送
        /// 侯鑫辉
        /// 2018.10.18
        /// </summary>
        /// <returns></returns>
        public IActivity Activity()
        {
            return new ActivityImpl();
        }

        /// <summary>
        /// 系统公告
        /// 侯鑫辉
        /// 2018.10.20
        /// </summary>
        /// <returns></returns>
        public ISysMsg SysMsg()
        {
            return new SysMsgImpl();
        }

        public ISystemConfiguration SystemConfiguration()
        {
            return new SystemConfigurationImpl();
        }

        /// <summary>
        /// 管理员信息
        /// 王浩
        /// 2018-10-17
        /// </summary>
        /// <returns></returns>
        public IAdminCompanyUser AdminCompanyUser()
        {
            return  new AdminCompanyUserlmpl();
        }

        /// <summary>
        /// 首页轮转图
        /// 王浩
        /// 2018-10-17
        /// </summary>
        /// <returns></returns>
        public IBanner Banner()
        {
            return new BannerImpl();
        }

        /// <summary>
        /// 企业人员角色
        /// 王浩
        /// 2018-10-18
        /// </summary>
        /// <returns></returns>
        public ICompanyRole CompanyRole()
        {
            return new CompanyRolelmpl();
        }

        /// <summary>
        ///施工任务
        /// </summary>
        /// <returns></returns>
        public ICompanyTask CompanyTask()
        {
            return new CompanyTaskImpl();
        }

        /// <summary>
        /// 任务阶段
        /// </summary>
        /// <returns></returns>
        public ICompanyTaskcate CompanyTaskcate()
        {
           return new CompanyTaskcateImpl();
        }

        /// <summary>
        /// 项目阶段分类
        /// 顾健
        /// 2018-10-18
        /// </summary>
        /// <returns></returns>
        public ITaskCate TaskCate()
        {
            return new TaskCateImpl();
        }

        /// <summary>
        /// 项目施工任务
        /// 顾健
        /// 2018-10-19
        /// </summary>
        /// <returns></returns>
        public ITask Task()
        {
            return new TaskImpl();
        }

        /// <summary>
        /// 精选案例
        /// 顾健
        /// 2018-10-19
        /// </summary>
        /// <returns></returns>
        public ICase Case()
        {
            return new CaseImpl();
        }

        /// <summary>
        /// 施工任务角色
        /// 顾健
        /// 2018-10-19
        /// </summary>
        /// <returns></returns>
        public ITaskRole TaskRole()
        {
            return new TaskRoleImpl();
        }

        /// <summary>
        /// 项目提醒
        /// 顾健
        /// 2018-10-19
        /// </summary>
        /// <returns></returns>
        public ITip Tip()
        {
            return new TipImpl();
        }

        /// <summary>
        /// 项目成员（团队服务）
        /// 顾健
        /// 2018-10-19
        /// </summary>
        /// <returns></returns>
        public IProjectTeam ProjectTeam()
        {
            return new ProjectTeamImpl();
        }

        /// <summary>
        /// 设计档案
        /// 顾健
        /// 2018-10-19
        /// </summary>
        /// <returns></returns>
        public IDoc Doc()
        {
            return new DocImpl();
        }

        /// <summary>
        /// 任务执行情况
        /// 顾健
        /// 2018-10-19
        /// </summary>
        /// <returns></returns>
        public IExecute Execute()
        {
            return new ExecuteImpl();
        }

        /// <summary>
        /// 精选案例角色
        /// 顾健
        /// 2018-10-19
        /// </summary>
        /// <returns></returns>
        public ICaseRole CaseRole()
        {
            return new CaseRoleImpl();
        }

        /// <summary>
        /// 申请工程材料
        /// 顾健
        /// 2018-10-19
        /// </summary>
        /// <returns></returns>
        public IMateriaApplay MateriaApplay()
        {
            return new MateriaApplayImpl();
        }

        /// <summary>
        /// 签到记录
        /// 王浩
        /// 2018-10-20
        /// </summary>
        /// <returns></returns>
        public ISignIn SignIn()
        {
            return new SignInImpl();
        }

        /// <summary>
        /// 申请延期
        /// 顾健
        /// </summary>
        /// <returns></returns>
        public IApplyDelay ApplyDelay()
        {
            return new ApplyDelayImpl();
        }

        /// <summary>
        /// 商品收藏
        /// 顾健
        /// </summary>
        /// <returns></returns>
        public IProductCollection ProductCollection()
        {
            return new ProductCollectionImpl();
        }


        /// <summary>
        /// 报备客户信息
        /// 王孝为
        /// </summary>
        /// <returns></returns>
        public IPotential Potential()
        {
            return  new PotentialImpl();
        }

        /// <summary>
        /// 购物车
        /// 顾健
        /// </summary>
        /// <returns></returns>
        public IShopCart ShopCart()
        {
            return new ShopCartImpl();
        }

        /// <summary>
        /// 支出记录
        /// 顾健
        /// </summary>
        /// <returns></returns>
        public ICompanyPay CompanyPay()
        {
            return new CompanyPayImpl();
        }

        /// <summary>
        /// 前端模块配置
        /// </summary>
        /// <returns></returns>
        public IFrontModule FrontModule()
        {
            return new FrontModuleImpl();
        }

        /// <summary>
        /// 处罚
        /// </summary>
        /// <returns></returns>
        public IDecoraTionPenalize DecoraTionPenalize()
        {
            return new DecortTionPenalizeImpl();
        }

        /// <summary>
        /// 任务模版
        /// </summary>
        /// <returns></returns>
        public ITaskTemplate TaskTemplate()
        {
            return new TaskTemplateImpl();
        }

        /// <summary>
        /// 功能：平台角色
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        public IDecorationSysRole DecorationSysRole() {

            return new DecorationSysRoleImpl();
        }

        /// <summary>
        /// 功能：平台模板
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        public IDecorationSysTemplate DecorationSysTemplate() {

            return new DecorationSysTemplateImpl();
        }

        /// <summary>
        /// 功能：平台类型
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        public IDecorationSysTaskCate DecorationSysTaskCate() {

            return new DecorationSysTaskCateImpl();
        }

        /// <summary>
        /// 功能：平台任务
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        public IDecorationSysTask DecorationSysTask() {

            return new DecorationSysTaskImpl();
        }

        /// <summary>
        /// 模板设置
        /// 顾健 2018-11-20
        /// </summary>
        /// <returns></returns>
        public ITaskMessage TaskMessage()
        {
            return new TaskMessageImpl();
        }
    }
}
