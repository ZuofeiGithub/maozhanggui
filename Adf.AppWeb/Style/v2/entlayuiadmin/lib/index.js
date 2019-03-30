
layui.extend({
  setter: 'config' //配置模块
  ,admin: 'lib/admin' //核心模块
  , view: 'lib/view' //视图渲染模块
  , adfCommon: 'lib/adfCommon' //通用模块
}).define(['setter', 'admin', 'adfCommon'], function (exports) {
    var setter = layui.setter,
        element = layui.element,
        admin = layui.admin,
        tabsPage = admin.tabsPage,
        view = layui.view,
        adfCommon = layui.adfCommon

  
  
  //打开标签页
  ,openTabsPage = function(url, text){
    //遍历页签选项卡
    var matchTo
    ,tabs = $('#LAY_app_tabsheader>li')
    ,path = url.replace(/(^http(s*):)|(\?[\s\S]*$)/g, '');
    
    tabs.each(function(index){
      var li = $(this)
      ,layid = li.attr('lay-id');
      
      if(layid === url){
        matchTo = true;
        tabsPage.index = index;
      }
    });
    
    text = text || '新标签页';
    
    //如果未在选项卡中匹配到，则追加选项卡
    if(!matchTo){
      $(APP_BODY).append([
        '<div class="layadmin-tabsbody-item layui-show">'
          ,'<iframe src="'+ url +'" frameborder="0" class="layadmin-iframe"></iframe>'
        ,'</div>'
      ].join(''));
      tabsPage.index = tabs.length;
      element.tabAdd(FILTER_TAB_TBAS, {
        title: '<span>'+ text +'</span>'
        ,id: url
        ,attr: path
      });
    }

    //定位当前tabs
    element.tabChange(FILTER_TAB_TBAS, url);
    admin.tabsBodyChange(tabsPage.index, {
      url: url
      ,text: text
    });
  }
  
  ,APP_BODY = '#LAY_app_body', FILTER_TAB_TBAS = 'layadmin-layout-tabs'
  ,$ = layui.$, $win = $(window);
  
  //将模块根路径设置为 controller 目录
  layui.config({
    base: setter.base + 'modules/'
  });
  
  //扩展 lib 目录下的其它模块
  layui.each(setter.extend, function(index, item){
    var mods = {};
    mods[item] = '{/}' + setter.base + 'lib/extend/' + item;
    layui.extend(mods);
  });

    //判断当前是否有顶级菜单
    //var objTopMenu =



  view().autoRender();

  //顶部菜单点击事件
  $(".topMenu").click(function () {
      var curModuleCode = $(this).attr("data");
      var sUrl = "/ent/home/getleftnav?modulecode=" + curModuleCode;

      //
      adfCommon.ajaxSyncPost(sUrl, {}, function (retData) {
          $("#LAY-system-side-menu").html(retData);
          element.init();
      }, function () {
          adfCommon.alert("网络存在异常,请重试");
      });

  });

    //清空缓存
    $("#btnClearCache").click(function() {
        var sUrl = "/ent/Home/RefreshCache";
        var param = {};
        adfCommon.ajaxSyncPost(sUrl,
            param,
            function (retData) {
                adfCommon.alert(retData.RetValue);
            },
            function() {
                adfCommon.alert("网络发生异常");
            });
    });

    //点击第一个菜单
    if ($(".topMenu").length > 0) {
        $(".topMenu").eq(0).trigger("click");
  }

    

  //对外输出
  exports('index', {
    openTabsPage: openTabsPage
  });
});
