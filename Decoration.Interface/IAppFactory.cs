namespace Decoration.Interface
{
    public interface IAppFactory
    {
        /// <summary>
        /// 工程项目
        /// </summary>
        /// <returns></returns>
        IProject Project();

        /// <summary>
        /// 模块
        /// </summary>
        /// <returns></returns>
        ICompanyModule CompanyModule();

        /// <summary>
        /// 王浩
        /// 2018-10-13
        /// 公司列表
        /// </summary>
        /// <returns></returns>
        ICompany Company();

        /// <summary>
        /// 企业用户
        /// 侯鑫辉
        /// 2018.10.16
        /// </summary>
        /// <returns></returns>
        ICompanyUser CompanyUser();

        /// <summary>
        /// 产品分类
        /// 侯鑫辉
        /// 2018.10.17
        /// </summary>
        /// <returns></returns>
        IProductCate ProductCate();

        /// <summary>
        /// 产品信息
        /// 侯鑫辉
        /// 2018.10.17
        /// </summary>
        /// <returns></returns>
        IProduct Product();

        /// <summary>
        /// 活动推送
        /// 侯鑫辉
        /// 2018.10.18
        /// </summary>
        /// <returns></returns>
        IActivity Activity();

        /// <summary>
        /// 系统公告
        /// 侯鑫辉
        /// 2018.10.20
        /// </summary>
        /// <returns></returns>
        ISysMsg SysMsg();

        ISystemConfiguration SystemConfiguration();

        /// <summary>
        /// 管理员信息
        /// 王浩
        /// 2018-10-17
        /// </summary>
        /// <returns></returns>
        IAdminCompanyUser AdminCompanyUser();

        /// <summary>
        /// 首页轮转图
        /// 王浩
        /// 2018-10-17
        /// </summary>
        /// <returns></returns>
        IBanner Banner();

        /// <summary>
        /// 企业人员角色
        /// 王浩
        /// 2018-10-18
        /// </summary>
        /// <returns></returns>
        ICompanyRole CompanyRole();

        /// <summary>
        /// 企业施工任务
        /// 王浩
        /// 2018-10-18
        /// </summary>
        /// <returns></returns>
        ICompanyTask CompanyTask();

        /// <summary>
        /// 企业任务分类
        /// 王浩
        /// 2018-10-18
        /// </summary>
        /// <returns></returns>
        ICompanyTaskcate CompanyTaskcate();

        /// <summary>
        /// 项目阶段分类
        /// 顾健
        /// 2018-10-18
        /// </summary>
        /// <returns></returns>
        ITaskCate TaskCate();

        /// <summary>
        /// 项目施工任务
        /// 顾健
        /// 2018-10-19
        /// </summary>
        /// <returns></returns>
        ITask Task();

        /// <summary>
        /// 精选案例
        /// 顾健
        /// 2018-10-19
        /// </summary>
        /// <returns></returns>
        ICase Case();

        /// <summary>
        /// 施工任务角色
        /// 顾健
        /// 2018-10-19
        /// </summary>
        /// <returns></returns>
        ITaskRole TaskRole();


        /// <summary>
        /// 项目提醒
        /// 顾健
        /// 2018-10-19
        /// </summary>
        /// <returns></returns>
        ITip Tip();

        /// <summary>
        /// 项目成员（团队服务）
        /// 顾健
        /// 2018-10-19
        /// </summary>
        /// <returns></returns>
        IProjectTeam ProjectTeam();

        /// <summary>
        /// 设计档案
        /// 顾健
        /// 2018-10-19
        /// </summary>
        /// <returns></returns>
        IDoc Doc();

        /// <summary>
        /// 任务执行情况
        /// 顾健
        /// 2018-10-19
        /// </summary>
        /// <returns></returns>
        IExecute Execute();


        /// <summary>
        /// 精选案例角色
        /// 顾健
        /// 2018-10-19
        /// </summary>
        /// <returns></returns>
        ICaseRole CaseRole();

        /// <summary>
        /// 申请工程材料
        /// 顾健
        /// 2018-10-19
        /// </summary>
        /// <returns></returns>
        IMateriaApplay MateriaApplay();

        /// <summary>
        /// 签到记录
        /// 王浩
        /// 2018-10-20
        /// </summary>
        /// <returns></returns>
        ISignIn SignIn();

        /// <summary>
        /// 申请延期
        /// 顾健
        /// </summary>
        /// <returns></returns>
        IApplyDelay ApplyDelay();

        /// <summary>
        /// 商品收藏
        /// 顾健
        /// </summary>
        /// <returns></returns>
        IProductCollection ProductCollection();
        
        /// <summary>
        /// 报备客户信息
        /// 王孝为
        /// </summary>
        /// <returns></returns>
        IPotential Potential();
        
        /// <summary>
        /// 购物车
        /// 顾健
        /// </summary>
        /// <returns></returns>
        IShopCart ShopCart();

        /// <summary>
        /// 支出记录
        /// 顾健
        /// </summary>
        /// <returns></returns>
        ICompanyPay CompanyPay();

        /// <summary>
        /// 前端模块配置
        /// </summary>
        /// <returns></returns>
        IFrontModule FrontModule();

        /// <summary>
        /// 处罚
        /// </summary>
        /// <returns></returns>
        IDecoraTionPenalize DecoraTionPenalize();

        /// <summary>
        /// 任务模版
        /// </summary>
        /// <returns></returns>
        ITaskTemplate TaskTemplate();

        /// <summary>
        /// 功能：平台角色
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        IDecorationSysRole DecorationSysRole();

        /// <summary>
        /// 功能：平台模板
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        IDecorationSysTemplate DecorationSysTemplate();


        /// <summary>
        /// 功能：平台类型
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        IDecorationSysTaskCate DecorationSysTaskCate();

        /// <summary>
        /// 功能：平台任务
        /// 创建时间：2018.11.13
        /// 创建人：张硕
        /// </summary>
        IDecorationSysTask DecorationSysTask();


        /// <summary>
        /// 模板设置
        /// 顾健 2018-11-20
        /// </summary>
        /// <returns></returns>
        ITaskMessage TaskMessage();
    }
}
