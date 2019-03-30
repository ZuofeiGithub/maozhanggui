;
(function ($) {
    $.extend({

        /**
        @功能:向一个Url提交一个表单
        @async:是否以异步的方式提交
        @formId:当前表单id
        @url:需要处理的表单Id
        @beforeSubmitCallBack ：提交之前处理的函数
        @successCallBack：提交成功后的处理函数
        @errorCallBack：发生错误后的处理函数
        */
        asSubmitFormAsync: function (async, formId, url, beforeSubmitCallBack, successCallBack, errorCallBack) {
            var options = {
                type: "post",
                cache: false,
                url: url,
                async: async,
                beforeSubmit: beforeSubmitCallBack,
                success: successCallBack,
                error: errorCallBack
            };
            $("#" + formId).ajaxSubmit(options);
        },
        /**
@功能:向一个Url提交一个表单
@async:是否以异步的方式提交
@formId:当前表单id
@url:需要处理的表单Id
@beforeSubmitCallBack ：提交之前处理的函数
@successCallBack：提交成功后的处理函数
@errorCallBack：发生错误后的处理函数
*/
        asAppSubmitForm: function (url, beforeSubmitCallBack, successCallBack) {
            var options = {
                type: "post",
                cache: false,
                url: url,
                async: false,
                beforeSubmit: beforeSubmitCallBack,
                success: successCallBack,
                error: function () {
                    $.asAlert("网络访问错误");
                }
            };
            $("#curForm").ajaxSubmit(options);
        },
        /**
         * 字符串分隔
         * @param {} inputStr 
         * @param {} splitword 
         * @returns {} 
         */
        asSplitToArray: function (inputStr, splitword) {
            var arrInfo = inputStr.split(splitword);
            return arrInfo;

        },
        /**
        @功能:弹出框
        */
        asAlert: function (sInfo) {
            alert(sInfo);
        },
        /**
@功能:得到checkbox选中的数量
*/
        asCheckboxLength: function (cbName) {
            return $("input[name='" + cbName + "']:checked").length;
        },
        /**
@功能:得到选择中checkbox第一个值
*/
        asCheckBoxFirstValue: function (cbName) {
            return $("input[name='" + cbName + "']:checked")[0].value;
        },
        /**
@功能:得到所有的选择值,并以$分隔开
*/
        asCheckBoxValues: function (cbName) {
            var retValues = "";
            var curSelect = $("input[name='" + cbName + "']:checked");
            for (var i = 0; i < curSelect.length; i++) {
                if (i < curSelect.length - 1) {
                    retValues += curSelect[i].value + "$";
                } else {
                    retValues += curSelect[i].value;
                }

            }

            return retValues;
        },
        asUpdateCkeditor: function () {
            //保存富文本编辑前需要进行这个更新操作
            for (var instances in CKEDITOR.instances) {
                CKEDITOR.instances[instances].updateElement();
            }
        }
        /*
        @功能:url跳转
        @参数:url
        转入需要跳转的Url
        **/
        , asGoUrl: function (url) {
            location.href = url;
        }
        , asAjaxGetAsync: function (async, url, param, beforeFunc, successFunc, callBackError) {
            $.ajax({
                url: url,
                type: "GET",
                async: async,
                data: param,
                cache: false,
                timeout: 60000,
                beforeSend: beforeFunc,
                success: successFunc,
                error: callBackError
            });

        },
        /*
        @:功能:同步的方式得到url的内容
        @:url:提交的目标页面
        @:sp:参数
        **/
        asAjaxGet: function (sUrl, sp) {
            var retValue;
            $.ajax({
                url: sUrl,
                type: "GET",
                async: false,
                data: sp,
                cache: false,
                timeout: 60000,
                success: function (retData) {
                    retValue = retData;
                }
            });
            return retValue;
        }
        /*
        @:功能:Post方式以异步的方式执行一个Url
        @:url:提交的目标页面
        @:param:Post的参数
        @:callbackSuccess:获取数据成功后需要执行的回调函数
        @callBackError:执行发生异常回调函数
        **/
        , asAjaxPostAsync: function (basync, url, param, callbackSuccess, callBackError) {
            //post方法提交数据,以异步与回调函数的方式来进行
            $.ajax({
                url: url,
                type: "POST",
                async: basync,
                data: param,
                cache: false,
                success: callbackSuccess,
                error: callBackError
            });
        }
        , asAppPost: function (url, param, callbackSuccess) {
            //post方法提交数据,以异步与回调函数的方式来进行
            $.ajax({
                url: url,
                type: "POST",
                async: false,
                data: param,
                cache: false,
                success: callbackSuccess,
                error: function () {
                    $.asAlert("网络访问错误,请重试");
                }
            });
        },
        asSubmitForm1: function (formId, url) {
            var retValue = "";
            var options = {
                type: "post",
                cache: false,
                url: url,
                async: false,

                beforeSubmit: function (formArray, jqForm) {

                },
                success: function (data) {
                    retValue = data;
                },
                error: function (aText) {

                }
            };
            $("#" + formId).ajaxSubmit(options);
            return retValue;
        }
                ,
        asAjaxPost1: function (url, param) {
            //post方法提交数据
            var retData = "";
            $.ajax({
                url: url,
                type: "POST",
                async: false,
                timeout: 60000,
                data: param,
                cache: false,
                success: function (responseText) {
                    retData = responseText;
                }
            });
            return retData;
        },
        //全提交的方式进行提交form表单
        asSubmitForm: function (bAsync, formId, url, callbackBefore, callBackSuccess, callbackError) {
            var options = {
                type: "post",
                cache: false,
                url: url,
                async: bAsync,
                beforeSubmit: callbackBefore,
                success: callBackSuccess,
                error: callbackError
            };
            $("#" + formId).ajaxSubmit(options);
        },
        /*
        以同步的方式获取返回的结果
        */
        asSubmitFormSync: function (formId, url, callbackBefore) {
            var rValue;
            var options = {
                type: "post",
                cache: false,
                url: url,
                async: false,
                beforeSubmit: callbackBefore,
                success: function (retData) {
                    rValue = retData;
                },
                error: function () {
                    alert("数据提交发生异常,请联系管理员");
                }
            };
            $("#" + formId).ajaxSubmit(options);

            return rValue;
        }

        , asFormSelect: function (id, jsonData, valueName, textName, defaultValue) {

            var objCtrl = $('#' + id);
            objCtrl.empty();
            var lstOptionStr = "<option value=\"\">-请选择-</option>";
            var optionStr = "";
            for (var i = 0; i < jsonData.length; i++) {
                var selectedInfo = "";
                if (jsonData[i][valueName] == defaultValue) {
                    selectedInfo = " selected='selected' ";
                }
                optionStr = "<option value='" + jsonData[i][valueName] + "' " + selectedInfo + ">" + jsonData[i][textName] + "</option>";
                lstOptionStr += optionStr;
            }
            //alert(lstOptionStr);
            objCtrl.append(lstOptionStr);
        }
        , asEvent: function (opName, evtName, func) {
            $("body").on(evtName, opName, func);
        }
        , asButtonEvent: function (opName, evtName, func) {

            $("body").on(evtName, "." + opName, func);
        },
        //加载层
        asLoadExecuting: function (tipInfo) {
            var layerId = layer.load(tipInfo);
            return layerId;
        }
        /**
        根据id关闭当前层
        **/
        , asLoadClose: function (layId) {
            layer.close(layId);
        }
        /**
        得到最外层的窗口
        **/
        , asGetTopWin: function () {
            var win = window.top;
            while (win.opener) {
                win = win.opener.top;
            }
            return win;
        }
        /**
        Msg提醒用户.类似于Alert
        **/
        , asLayerMsg: function (msgInfo) {
            layer.msg(msgInfo, { time: 1000 });
        }
        /**
        Msg提醒用户.类似于Alert
        **/
        , asLayerDialog: function (layerId, title, url, dataName, width, height, funcEnd) {
            //得到最顶端的窗口
            var topWin = $.asGetTopWin();
            topWin.layer.open({
                type: 2,
                title: title,
                id: layerId,
                maxmin: false,
                area: [width, height],
                content: url,
                end: funcEnd
            });
        }

        /**
@功能:判断是否是电话号码
*/
        , asCheckCellphone: function (src, isAlert, alertMsg) {
            var rValue = true;
            var reg = /^1[3|4|5|7|8][0-9]\d{4,8}$/;
            rValue = reg.test(src);
            if (rValue == false && isAlert == true) {
                $.asAlert(alertMsg);
            }
            return rValue;
        },
        /**
        @功能:检查一个字符串是空值
        */
        asCheckStringEmpty: function (src, isAlert, alertMsg) {
            var rValue = true;
            var length = src.length;
            if (length < 1) {
                rValue = false;
                if (isAlert == true) {
                    $.asAlert(alertMsg);
                }
            }
            return rValue;
        },
        /**
@功能:检查一个字符串是空值
*/
        asCheckCtrlEmpty: function (id, isAlert, alertMsg) {

            var src = $("#" + id).val();

            var rValue = $.asCheckStringEmpty(src, isAlert, alertMsg);
            if (rValue == false) {
                $("#" + id).focus();
            }
            return rValue;
        },
        /**
        @功能:得到一个字符串的长度是不是指定范围
        */
        asCheckStringMinMax: function (src, iMin, iMax, isAlert, alertMsg) {
            var rValue = true;
            var length = src.length;
            if (length < iMin || length > iMax) {
                rValue = false;
                if (isAlert == true) {
                    $.asAlert(alertMsg);
                }
            }
            return rValue;
        },
        /**
@功能:判断是否是数字包含浮点数
*/
        asCheckNum: function (src, isAlert, alertMsg) {
            var rValue = true;
            var reg = /^[0-9]+.?[0-9]*$/;
            rValue = reg.test(src);
            if (rValue == false && isAlert == true) {
                $.asAlert(alertMsg);
            }
            return rValue;
        },
        /**
@功能:判断是否是数字包含浮点数
*/
        asCheckCtrlNum: function (id, isAlert, alertMsg) {
            var src = $("#" + id).val();
            var rValue = $.asCheckNum(src, isAlert, alertMsg);
            if (rValue == false) {
                $("#" + id).focus();
            }

            return rValue;
        },
        /**
        @功能:判断两个字符串是否相等
        @src1:第一个字符串
        @src2:第二个字符串
        @isAlert:是否提示
        @src2:提示的内容
        */
        asCheckEquare: function (src1, src2, isAlert, alertMsg) {
            var rValue = true;
            //alert(src1.trim() != src2.trim());
            if (src1.trim() != src2.trim()) {
                rValue = false;
            }

            if (rValue == false && isAlert) {
                $.asAlert(alertMsg);
            }
            return rValue;
        }
                , asUrlModalDialog: function (id, url, title, dlgParam) {
                    var defaultParam = {
                        id: id,
                        title: title,
                        width: "60%",
                        height: "90%"

                    };
                    var newDlgParam = $.extend({}, defaultParam, dlgParam);
                    art.dialog.open(url, newDlgParam);
                }
        , asGetValuesOfCtrlName: function (selCtrlName, splitchar) {
            var retValue = "";
            var objSelCtrls = document.getElementsByName(selCtrlName);
            var rws = objSelCtrls.length;
            for (var i = 0; i < rws; i++) {
                if ((objSelCtrls[i].type = "checkbox") && (objSelCtrls[i].name == selCtrlName) && (objSelCtrls[i].checked == true)) {
                    retValue += objSelCtrls[i].value + splitchar;
                }
            }
            if (retValue.length > 0) {
                retValue = retValue.substring(0, retValue.length - 1);
            }
            return retValue;
        }
                , asGetCheckBoxValuesOfCtrlName: function (selCtrlName, splitchar) {
                    var retValue = "";
                    var objSelCtrls = document.getElementsByName(selCtrlName);
                    var rws = objSelCtrls.length;
                    for (var i = 0; i < rws; i++) {
                        if ((objSelCtrls[i].type = "checkbox") && (objSelCtrls[i].name == selCtrlName) && (objSelCtrls[i].checked == true)) {
                            retValue += objSelCtrls[i].value + splitchar;
                        }
                    }
                    if (retValue.length > 0) {
                        retValue = retValue.substring(0, retValue.length - 1);
                    }
                    return retValue;
                }
                , asWfLoadNextStepInfo: function (curNodeCode, wfCode, curParams) {
                    var sUrl = "/manage/wfservice/GetNextStepInfo?curNodeCode=" + curNodeCode;
                    sUrl += "&WfCode=" + wfCode;
                    sUrl += "&" + curParams;
                    var retInfo = $.asAjaxGet(sUrl, null);
                    $("#wfNextStepInfo").html(retInfo);
                }

                , asWfLoadHistory: function (curBoCode, wfCode) {
                    var sUrl = "/manage/wfservice/GetWfHistory?wfCode=" + wfCode;
                    sUrl += "&bokeycode=" + curBoCode;
                    var retInfo = $.asAjaxGet(sUrl, null);

                    $("#wfHistoryInfo").html(retInfo);
                }
                        , asUploadList: function (uploadContainId, btnInfo) {
                            alert("xx");
                            $.asEvent(btnInfo, "click", function () {
                                var sUrl = "/manage/upload/doupload";
                                var dlgParam = {
                                    width: "400px", height: "300px",
                                    close: function () {
                                        var rValue = art.dialog.data("RetValue");
                                        var rText = art.dialog.data("RetText");
                                        if (rValue === undefined) {
                                            rValue = "";
                                        }
                                        if (rText === undefined) {
                                            rText = "";
                                        }
                                        if (rValue.length > 0) {
                                            //$("#FoodPic").val(rValue);
                                            $(uploadContainId + ">tbody").append("<tr><td><a href=\"/upload/" + rValue + "\">" + rText + "</a></td></tr>");
                                        }
                                    }
                                }
                                $.asUrlModalDialog(btnInfo, sUrl, "上传文件", dlgParam, {});
                            });
                        }
                                , asExcelImport: function (btnInfo, tableName, fieldNames) {
                                    var sUrl = "/manage/excelservice/Upload?tableName=" + tableName;
                                    sUrl += "&fieldnames=" + fieldNames;
                                    var dlgParam = {
                                        width: "400px", height: "300px",
                                        close: function () {
                                            var rValue = art.dialog.data("RetValue");
                                            var rText = art.dialog.data("RetText");
                                            if (rValue === undefined) {
                                                rValue = "";
                                            }
                                            if (rText === undefined) {
                                                rText = "";
                                            }
                                            if (rValue.length > 0) {
                                                //$("#FoodPic").val(rValue);
                                                //$(uploadContainId + ">tbody").append("<tr><td><a href=\"/upload/" + rValue + "\">" + rText + "</a></td></tr>");
                                            }
                                        }
                                    }
                                    $.asUrlModalDialog(btnInfo, sUrl, "上传文件", dlgParam, {});
                                }

        /**
        参数说明:
        treeId：当前网页树空间存放的dom元素id, 
        ajaxUrl：异步请求的id, 
        autoParmStr:Post请求时所带参数
        captionName：显示树标题, 
        idName：树主编码id名称, 
        pIdName：树父级编码名称, 
        clickFunc：点击树的事件, 
        setInfo：其他设置，见官方setting配置信息
        示例：
        
        //树点击事件
        function treeClick(event, treeId, treeNode) {
            // 事务处理
            alert(treeNode.isParent);
        }
        
        $(function () {
            var curSet= {
                
            }
            doLoadTree("curTree", "/devtest/gettreedata",['ModuleCode'], "ModuleName", "ModuleCode", "ParentCode", treeClick,curSet);
        });
        
        dom元素:
         <ul id="curTree" class="ztree"></ul>
        **/
, asLoadTree: function (treeId, ajaxUrl, autoParmStr, captionName, idName, pIdName, clickFunc, setInfo) {
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
            beforeExpand: function (treeId, treeNode) {
                //这里保证每次都能进行异步刷新,否则下面子节点第二次后将不再向服务器请求,
                //alert("是否响应");
                //测试了还是有点问题，需要重新测试一下
                treeNode.zAsync = false;
                return true;
            }
        }
    };

    $.extend(defaultSetting, setInfo);
    $.fn.zTree.init($("#" + treeId), defaultSetting);
}
        /**
        上传单个图片
        */
        //上传控件专用
        , asUploadImage: function (imgContainer, fileId, uploadurl) {
            //----上传控件初始化
            $.ajaxFileUpload({
                url: uploadurl, //处理图片的脚本路径
                type: 'post', //提交的方式
                secureuri: false, //是否启用安全提交
                fileElementId: fileId, //file控件ID
                dataType: 'json', //服务器返回的数据类型      
                success: function (data, status) {
                    var retData = data;
                    var imgUrl = retData.ServerRelativeFileName;
                    var sImageInfo = "<li><img src='" + imgUrl + "' /><br><button>删除</button></li>";
                    $(imgContainer).append(sImageInfo);
                },
                error: function (data, status, e) { //提交失败自动执行的处理函数
                    //alert(e);
                }
            });

            //------------------
        }
         , asUploadFile: function (container, fileId, controlId, uploadurl) {
             //----上传控件初始化
             $.ajaxFileUpload({
                 url: uploadurl, //处理图片的脚本路径
                 type: 'post', //提交的方式
                 secureuri: false, //是否启用安全提交
                 fileElementId: fileId, //file控件ID
                 dataType: 'json', //服务器返回的数据类型      
                 success: function (data, status) {
                     var retData = data;
                     var imgUrl = retData.ServerRelativeFileName;
                     var sImageInfo = "<a href='" + imgUrl + "'>" + retData.OriginalFileName + "</a>";
                     $("#" + container).html(sImageInfo);
                     $("#" + controlId).val(imgUrl);
                 },
                 error: function (data, status, e) { //提交失败自动执行的处理函数
                     //alert(e);
                 }
             });

             //------------------
         }
        , asUploadMoreFile: function (container, fileId, controlId, uploadurl) {
            var sImageInfo = $("#" + container).html();
            var ImgUrl = $("#" + controlId).val();

            //----上传控件初始化
            $.ajaxFileUpload({
                url: uploadurl, //处理图片的脚本路径
                type: 'post', //提交的方式
                secureuri: false, //是否启用安全提交
                fileElementId: fileId, //file控件ID
                dataType: 'json', //服务器返回的数据类型      
                success: function (data, status) {
                    var retData = data;
                    ImgUrl += retData.ServerRelativeFileName + "|" + retData.OriginalFileName + ",";
                    sImageInfo += "<div><a href='" + retData.ServerRelativeFileName + "'>" + retData.OriginalFileName + "</a><a onclick=\"deleteFlie(this,'" + container + "','" + controlId + "')\"   style=\"color:red\; cursor\: pointer\">×</a></div>";
                    $("#" + container).empty();
                    $("#" + controlId).val("");
                    $("#" + container).append(sImageInfo);
                    $("#" + controlId).val(ImgUrl);
                },
                error: function (data, status, e) { //提交失败自动执行的处理函数
                    //alert(e);
                }
            });

            //------------------
        }
        , asInitImagesForUploadCtrl: function (imgContainer, imageNames, spitInfo) {
            var arrImageNames = imageNames.split(spitInfo);
            if (arrImageNames.length > 0) {
                for (var i = 0; i < arrImageNames.length; i++) {
                    var imgUrl = arrImageNames[i];
                    if (imgUrl.length > 4) {
                        var sImageInfo = "<li><img src='" + imgUrl + "' /><br><button>删除</button></li>";
                        $(imgContainer).append(sImageInfo);
                    }
                }
            }
        }
        /**
        导航至一个模块
        */
        , goModule: function (moduleCode, sParam) {
            var sUrl = "/manage/moduleservice/sm?modulecode=" + moduleCode + sParam;
            $.asGoUrl(sUrl);
        }
    });
})(jQuery);



