/**

 @Name：layuiAdmin 公共业务
 @Author：贤心
 @Site：http://www.layui.com/admin/
 @License：LPPL
    
 */
 
layui.define(function(exports) {
    var $ = layui.$,
        layer = layui.layer,
        laytpl = layui.laytpl,
        setter = layui.setter;
  
  //公共业务的逻辑处理可以写在此处，切换任何页面都会执行
  //……

    //弹框
    var o = {
        /*弹框*/
        alert: function (sMsg) {
            layer.msg(sMsg);
        }
        //弹窗延迟并执行函数
        ,msgDo : function(sMsg, doEvent) {
            layer.msg(sMsg, {
                offset: '15px',
                icon: 1,
                time: 500
            },doEvent);
        }
        //打开frame子窗口
        ,dlgOpen : function(title,frameUrl, width,height,endFunc) {
            layer.open({
                type: 2,
                title: title,
                scrollbar:true,
                maxmin: false,
                isOutAnim: false,
                area: [width, height],
                content: frameUrl,
                end:endFunc
            });
        }
        //在窗口中关闭当前窗口
        , dlgClose: function () {
            var index = parent.layer.getFrameIndex(window.name);
            parent.layer.close(index);
        }
        //在窗口中关闭当前窗口
        , uploadImage: function (btnId,upload,func) {
            upload.render({
                //允许上传的文件后缀
                elem: btnId,
                url: '/manage/boupload/douploadimage'
                 , accept: 'file' //普通文件
                 , exts: 'jpg|jpeg|png' //只允许上传压缩文件
                 , done:func
                 
            });
        }


        , loadWfUploadFiles: function (bocode,upload) {
            var sUrl = "/manage/boupload/getall?bocode=" + bocode;
            o.ajaxSyncGet(sUrl, null, function(retData) {
                $("#BoUploadFiles").html(retData);
            }, function() {
                $("#BoUploadFiles").html("-");
            });

            upload.render({
                //允许上传的文件后缀
                elem: '#boSelectFile',
                url: '/manage/boupload/doupload?bocode=' + bocode
                 , accept: 'file' //普通文件
                 , exts: 'doc|docx|jpeg|jpg|png|xlsx|xls|zip|rar|7z' //只允许上传压缩文件
                 , done: function (retData) {
                     if (retData.hasOwnProperty("uploadfilename")) {
                         var tplInfo = $("#tplFileList").html();
                         tplInfo = tplInfo.replace("pt_uploadfilename", retData.uploadfilename);
                         tplInfo = tplInfo.replace("pt_uploaddate", retData.uploaddate);
                         tplInfo = tplInfo.replace("pt_uploadusername", retData.uploadusername);
                         tplInfo = tplInfo.replace("pt_uploadfileserverpath", retData.uploadfileserverpath);

                         $("#boFileList").append(tplInfo);
                     }
                 }
            });
        }

        , asLoadTreeData: function (treeId, captionName, idName, pIdName, clickFunc, setInfo, zNodes) {
            var defaultSetting = {

                data: {
                    key: {
                        name: captionName
                    },
                    simpleData: {
                        enable: true,
                        idKey: idName,
                        pIdKey: pIdName
                    }
                },
                callback: {
                    onClick: clickFunc

                }
            };

            $.extend(defaultSetting, setInfo);
            $.fn.zTree.init($("#" + treeId), defaultSetting, zNodes);
        }
        /*get请求*/
        //同步
        , ajaxSyncGet: function(url, param, successFunc, callBackError) {
            $.ajax({
                url: url,
                type: "GET",
                async: false,
                data: param,
                cache: false,
                timeout: 60000,               
                success: successFunc,
                error: callBackError
            });
        }
        //异步
        , ajaxAsyncGet: function (url, param, successFunc, callBackError) {
            $.ajax({
                url: url,
                type: "GET",
                async: true,
                data: param,
                cache: false,
                timeout: 60000,
                success: successFunc,
                error: callBackError
            });
        }

        /*post请求*/
        //同步
        , ajaxSyncPost: function (url, param, callbackSuccess, callBackError) {
                //post方法提交数据,以异步与回调函数的方式来进行
                $.ajax({
                    url: url,
                    type: "POST",
                    async: false,
                    data: param,
                    cache: false,
                    success: callbackSuccess,
                    error: callBackError
                });
            }
        //异步
        , ajaxAsyncPost: function (url, param, callbackSuccess, callBackError) {
            //post方法提交数据,以异步与回调函数的方式来进行
            $.ajax({
                url: url,
                type: "POST",
                async: true,
                data: param,
                cache: false,
                success: callbackSuccess,
                error: callBackError
            });
        }
        /*
        标准Grid定义方法
        */
        //初始化表格
       ,setStdTable: function (table,colInfo,dataUrl, pagesie,sWhere, eventFunc) {
           table.render({
               elem: '#MainGrid' //渲染DOM
               ,remoteSort:true
			        , url: dataUrl //数据接口
			        , height: 'full-137'
			        , where: sWhere
                    , cellMinWidth: 100 //全局定义常规单元格的最小宽度
			        , cols: colInfo
                    , limit: pagesie //每页数目限制
			        , page: { theme: '#1E9FFF' } //开启分页，并设定颜色
			        , request: {
			            pageName: "p"
                        , limitName: "ps"
			        }
               
            });

           //监听表格事件
            table.on('tool(curMainGrid)', eventFunc);
        }
        /*
         标准表格加载，并设置加载完成后事件对表格进行处理
         */
        , setStdTableWithDone: function (table, colInfo, dataUrl, pagesie, sWhere, eventFunc, doneFunc) {
            table.render({
                elem: '#MainGrid' //渲染DOM
                , remoteSort: true
                , url: dataUrl //数据接口
                , height: 'full-137'
                , where: sWhere
                , cellMinWidth: 100 //全局定义常规单元格的最小宽度
                , cols: colInfo
                , limit: pagesie //每页数目限制
                , page: { theme: '#1E9FFF' } //开启分页，并设定颜色
                , request: {
                    pageName: "p"
                    , limitName: "ps"
                }, done: doneFunc

            });

            //监听表格事件
            table.on('tool(curMainGrid)', eventFunc);
        }

        //重载表格
        ,reloadStdTable : function(table, sWhere) {
            var option = { where: sWhere, page: { curr: 1 } };
            table.reload("MainGrid", option);
        }
        //得到表格中选择的一行
        , getStdTableSelectedRows: function (table) {
            var obj = table.checkStatus('MainGrid');
            return obj;
        }
        /*验证相关*/
        //是否是Email
        , isEmail: function (value) {
            var mPattern = /^([A-Za-z0-9_\-\.])+\@([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;
            return mPattern.test(value);
        }
          
        //是否是Url
        , isUrl: function (value) {
            var mPattern = /^((https?|ftp|file):\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$/;
            return mPattern.test(value);
        }

        //是否为Num
        , isNumber: function (value) {
            return new RegExp("/^\d+(\.{1}\d+)?$/").test(value);
        }

        //是否为整数
        , isInt: function (value) {
            return new RegExp("/^-?\d+$/").test(value);
        }

        //是否负整数
        , isNegativeInt: function (value) {
            return new RegExp("/^-?\d+$/").test(value);
        }

        //是否是用户名 用户名正则，6到16位（字母，数字，下划线，减号）
        , isUserCode: function (value) {
            return new RegExp("/^[a-zA-Z0-9_-]{4,16}$/").test(value);
        }

        //是否手机号
        , isCellPhone: function (value) {
            var mPattern = /^1[34578]\d{9}$/;
            return mPattern.test(value);
        }

        //是否身份证号
        , isIdCard: function (value) {
            var mPattern = /^[1-9]\d{5}(18|19|([23]\d))\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$/;
            return mPattern.test(value);
        }

        //是否isIpv4
        , isIpv4: function (value) {
            var mPattern = /^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
            return mPattern.test(value);
        }

        //
        /*树控件*/
        //这个与layui进行并存，需要独立使用jquery引入,不使用layui内置的jquery
        , loadZtree: function (treeId, ajaxUrl, autoParmStr, captionName, idName, pIdName, clickFunc, setInfo) {
            //alert("x");
            var defaultSetting = {
                async: {
                    enable: true,
                    url: ajaxUrl,
                    autoParam: autoParmStr
                },
                data: {
                    key: {
                        name: captionName
                        
                    },
                    simpleData: {
                        enable: true,
                        idKey: idName,
                        pIdKey: pIdName
                    }
                },
                callback: {
                    onClick: clickFunc,
                    beforeExpand: function(treeId, treeNode) {
                        var zTree = $.fn.zTree.getZTreeObj(treeId);
                        zTree.reAsyncChildNodes(treeNode, "refresh", true);
                        //zTree.reAsyncChildNodes(treeNode, reloadType, true);
                        //这里保证每次都能进行异步刷新,否则下面子节点第二次后将不再向服务器请求,
                        //alert("是否响应");
                        //测试了还是有点问题，需要重新测试一下
                        //treeNode.zAsync = false;
                        return true;
                    }
                }

            }
            $.extend(defaultSetting, setInfo);
            $.fn.zTree.init($("#" + treeId), defaultSetting);
        }


        /*树控件*/
        //这个与layui进行并存，需要独立使用jquery引入,不使用layui内置的jquery
        , loadZtreeAnsyc: function (treeId, ajaxUrl, autoParmStr, captionName, idName, pIdName, clickFunc, setInfo) {
            //alert("x");
            var setting = {
                data: {
                    key: {
                        name: captionName
                    },

                    simpleData: {
                        enable: true,
                        idKey: idName,
                        pIdKey: pIdName
                    }
                },
                callback:
                {
                    onClick: clickFunc
                }

            };
            var sUrl = ajaxUrl;
            $.post(sUrl, function (retData) {
                $.fn.zTree.init($("#" + treeId), setting, retData);

                //展开所有的节点信息
                var treeObj = $.fn.zTree.getZTreeObj("TreeInfo");
                //var nodes = treeObj.getNodes();
                // treeObj.expandNode(nodes[0], true, false, true);
            });
        }

        //------选择一个用户--------------------------------
        , dlgSelectUser: function (width, height, endFunc) {
            //title,frameUrl, width,height,endFunc
            var title = "选择用户";
            var frameUrl = "/manage/user/select";
            //打开用户选择框
            o.dlgOpen(title,frameUrl,width,height,endFunc);
        }
        //------选择一个用户--------------------------------

        , dlgSelectOneOrg: function (width, height, endFunc) {
            //title,frameUrl, width,height,endFunc
            var title = "选择机构";
            var frameUrl = "/manage/org/SelectOneOrg";
            //打开用户选择框
            o.dlgOpen(title, frameUrl, width, height, endFunc);
        }
        , dlgSelectOneRole: function (width, height, endFunc) {
            //title,frameUrl, width,height,endFunc
            var title = "选择角色";
            var frameUrl = "/manage/role/SelectOneRole";
            //打开用户选择框
            o.dlgOpen(title, frameUrl, width, height, endFunc);
        }
        //--------------------------------------


    
    }
    
  
  //对外暴露的接口
    exports('adfCommon', o);
});