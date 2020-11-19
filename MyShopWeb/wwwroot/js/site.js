// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(function ()
{
    loadNav();
    loadBasket();
    initEvent();
});

function loadNav() {
    var userName = localStorage.getItem("userName");
    var nav = "";
    if (userName == null || userName == undefined || userName == "") {
        nav = '<li><a href="#"><span class="glyphicon glyphicon - user"></span> 注册</a></li><li><a href="javascript:jumpToLoginForm();"><span class="glyphicon glyphicon-log-in"></span> 登录</a></li >';
    }
    else {
        nav = '<li><a href="#"><span class="glyphicon glyphicon-user"></span> ' + userName + '</a></li>';
        $("#unlogin-alert").alert("close");
    }
    $("#nav-user").html(nav);
}

function loadBasket()
{
    $.ajax({
        url: BaseUrl.Api + "/api/app/basket",
        type: "GET",
        success: function (response) {
            var list = response.data;
            localStorage.setItem("basket", JSON.stringify(list));
            $("#badge-basket").html(list.length);
        }
    });
}

function initEvent()
{

    $("#btn-login").on("click", function () {
        var account = $("#login-account").val();
        var pwd = $("#login-password").val();

        $.ajax({
            url: BaseUrl.User + "/api/app/user/login",
            type: "POST",
            data: JSON.stringify({
                account: account,
                password: pwd
            }),
            contentType: "application/json",
            success: function (response) {
                handleLogin(response);
            }
        });
    });
}

function addBasketItem(pid)
{
    $.ajax({
        url: BaseUrl.Api + "/api/app/basket",
        type: "POST",
        data: JSON.stringify({
            pid: pid
        }),
        contentType: "application/json",
        success: function (response) {
            loadBasket();
        }
    });
}

function showError(titleText,message)
{

    var title = $("#myModal .modal-header .modal-title")
    title.html(titleText);

    removeClass(title, ModalClass);
    title.addClass("modal-error");

    var body = $("#myModal .modal-body")
    body.html(message);

    $("#myModal").modal("show");
}


function handleLogin(resp) {
    if (resp.code == 200) {
        window.localStorage.setItem("token", resp.data.token);
        window.localStorage.setItem("userName", resp.data.userName);
        loadNav();
        $("#loginModal").modal("hide");
    }
}

function removeClass(dom,classList)
{
    for (var i = 0; i < classList; i++) {
        dom.removeClass(classList[i]);
    }
}

function showUnLoginAlert()
{
    // $("#unlogin-alert").alert();
}

function jumpToLoginForm()
{
    $("#loginModal").modal("show");
}

function clearLocalTokenAndUserInfo()
{
    localStorage.setItem("token", "");
    localStorage.setItem("user-info","");
}

//全局的ajax访问，处理ajax清求时异常
$.ajaxSetup({
    complete: function (XMLHttpRequest, textStatus) {
        
        var data = XMLHttpRequest.responseJSON;
        switch (data.code) {
            case 401:
                clearLocalTokenAndUserInfo();
                showUnLoginAlert();
                break;
            case 500:
                alert("服务器开小差了!");
                break;
            case 200:
                break;
            default:
                
                break;
        }
    },
    headers: { 'Authorization': 'Bearer ' + window.localStorage.getItem("token") },
});