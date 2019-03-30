/*get请求*/
//同步
    function ajaxSyncGet(url, param, successFunc, callBackError) {
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
function ajaxAsyncGet(url, param, successFunc, callBackError) {
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
function ajaxSyncPost(url, param, callbackSuccess, callBackError) {
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

/*get WebApi请求*/
function ajaxSyncGetWebApi(url, param, successFunc, callBackError) {
    $.ajax({
        url: url,
        type: "GET",
        async: false,
        data: param,
        contentType: "application/json",
        cache: false,
        timeout: 60000,
        success: successFunc,
        error: callBackError
    });
}

/*post WebApi请求*/
//同步
function ajaxSyncPostWebApi(url, param, callbackSuccess, callBackError) {
    //post方法提交数据,以异步与回调函数的方式来进行
    $.ajax({
        url: url,
        type: "POST",
        async: false,
        data: param,
        contentType: "application/json",
        cache: false,
        success: callbackSuccess,
        error: callBackError
    });
}

//异步
function ajaxAsyncPost(url, param, callbackSuccess, callBackError) {
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