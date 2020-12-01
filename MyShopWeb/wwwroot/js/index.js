var pageIndex = 1;
var pageSize = MyShopDefaultPageSize;
var totalCount = 0;

$(function () {
    loadDailyGoods();
});

function loadDailyGoods() {
    try {
        getGoodList(pageIndex, pageSize);
    } catch (e) {

    }
    var list = [
        {
            Cover: "images/phone.jpg",
            Title: "测试商品",
            Description: "一些示例文本。一些示例文本。"
        }, {
            Cover: "images/phone.jpg",
            Title: "测试商品",
            Description: "一些示例文本。一些示例文本。"
        }, {
            Cover: "images/phone.jpg",
            Title: "测试商品",
            Description: "一些示例文本。一些示例文本。"
        }, {
            Cover: "images/phone.jpg",
            Title: "测试商品",
            Description: "一些示例文本。一些示例文本。"
        }, {
            Cover: "images/phone.jpg",
            Title: "测试商品",
            Description: "一些示例文本。一些示例文本。"
        }, {
            Cover: "images/phone.jpg",
            Title: "测试商品",
            Description: "一些示例文本。一些示例文本。"
        }, {
            Cover: "images/phone.jpg",
            Title: "测试商品",
            Description: "一些示例文本。一些示例文本。"
        }, {
            Cover: "images/phone.jpg",
            Title: "测试商品",
            Description: "一些示例文本。一些示例文本。"
        }
    ];
    ////var dom = $("#dailyGoods");

    ////for (var i = 0; i < list.length; i++) {
    ////    var data = list[i];
    ////    var tmp = MyShopDailyGoodsTmplate;
    ////    var html = tmp
    ////        .replace("$Cover", data.Cover)
    ////        .replace("$Title", data.Title)
    ////        .replace("$Description", data.Description);
    ////    dom.append(html);
    ////}
}
function getGoodList(pageIndex, pageSize) {
    var reqData = {
        MaxResultCount: pageSize,
        SkipCount : (pageIndex - 1) * pageSize
    };

    $.ajax({
        url: BaseUrl.Api + "/api/app/product/page",
        data: reqData,
        contentType: "application/json",
        type: "get",
        success: function (response) {
            handleGoodList(response.data.list,response.data.total);
        },
        error: function (response) {

        }
    });
}

function handleGoodList(list, total) {
    totalCount = total;
    var dom = $("#dailyGoods");
    dom.html("");
    for (var i = 0; i < list.length; i++) {
        var data = list[i];
        var tmp = MyShopDailyGoodsTmplate;
        var html = tmp
            .replace("$Cover", data.coverImage)
            .replace("$Title", data.name)
            .replace("$Description", data.description)
            .replace("$PId", data.id)
            .replace("$DPId", data.id);
            
        dom.append(html);
    }
}

function pre()
{
    pageIndex = pageIndex - 1;
    if (pageIndex <= 0)
        pageIndex = 1;

    getGoodList(pageIndex, pageSize);
}

function next() {
    if (pageIndex * pageSize >= totalCount) {

    } else
    {
        pageIndex = pageIndex + 1;
        getGoodList(pageIndex, pageSize);
    }
}
