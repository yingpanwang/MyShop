$(function () {
    loadTable();
});

function loadTable() {

    var basket;
    $.ajax({
        url: BaseUrl.Api + "/api/app/basket",
        type: "GET",
        async: false,
        success: function (response) {
            basket = response.data;
            localStorage.setItem("basket", JSON.stringify(basket));
            $("#badge-basket").html(basket.length);
        }
    });

    //var json = localStorage.getItem("basket");

    // var basket = JSON.parse(json);


    var table = $("#table-basket tbody");
    var insertHtml = "";
    if (basket.length <= 0) {
        insertHtml += '<tr><td colspan = "7" class="text-center"> <text>暂无商品</text></td></tr >';
    }
    else {
        for (var i = 0; i < basket.length; i++) {
            var item = basket[i];
            var row = "<tr id='" + item.productId + "'>";
            row += '<th scope="row" class="table-row-cbx"><input type="checkbox"  onchange="checkItem(' + item.productId + ');" /></th>';
            row += '<td class="table-row-img"><img src="' + item.coverImage + '" style="max-height:80px;" /></td>';
            row += '<td class="table-row-desc"><text>' + item.productName + '</text ></td >';
            row += ' <td class="table-row-price text-right">' + item.price + '</td>';
            row +=
                '<td class="table-row-count text-right">' +
                '<div class="input-group">' +
                '<span class="input-group-btn input-group-prepend">' +
                '<button class="btn btn-default" type="button" onclick="remove(this);">' +
                '-' +
                '</button>' +
                '</span>' +
                '<input type="number" class="form-control"  style="text-align:center;"  onblur="changeCount(' + item.productId + ',this)" value="' + item.count + '" >' +
                '<span class="input-group-btn input-group-append">' +
                '<button class="btn btn-default" type="button" onclick="add(this);">' +
                '+' +
                '</button>' +
                '</span>' +
                '</div>' +
                '</td>';

            row += '<td class="table-row-total text-right">' + (item.count * item.price).toFixed(2) + '</td>';
            row += '<td class="table-row-options text-center"><button onclick="deleteItem(' + item.productId + ')">删除</button></td>';

            row += "</tr>";

            insertHtml += row;

        }
    }

    table.html(insertHtml);
}

function checkAllItems() {
    var inputs = $("#table-basket tbody tr th input");
    for (var i = 0; i < inputs.length; i++) {
        inputs[i].checked = !inputs[i].checked;
        $(inputs[i]).trigger("onchange");
    }

}

function checkItem(pid) {
    console.log("checked toggle" + pid);
}

function add(sender) {
    $(sender).parent("div");
    var input = $(sender).parents("div").children("input");
    var value = parseFloat(input.val()) + 1;

    input.val(value);
    input.trigger("onblur");
}

function remove(sender) {
    $(sender).parent("div");
    var input = $(sender).parents("div").children("input");
    var value = parseFloat(input.val()) - 1;
    if (value <= 0) {
        alert("数量不能小于0");
        return;
    }
    input.val(value);
    input.trigger("onblur");
}

function changeCount(pid, sender) {
    var val = parseFloat($(sender).val());
    if (val == undefined || val == "" || isNaN(val)) {
        console.log("无效数字");
        return;
    } else {
        $.ajax({
            url: BaseUrl.Api + "/api/app/basket/change",
            type: "POST",
            data: JSON.stringify({
                pId: pid,
                count: val
            }),
            success: function (response) {
                console.log("修改成功:" + response)
            }
        });
    }
    changeTotal(sender);
}

function changeTotal(sender) {
    var tr = $(sender).parents("tr");
    var price = tr.find(".table-row-price").html();
    console.log(price);
    var count = $(sender).val();
    console.log(count);
    var total = parseFloat(price) * parseFloat(count);

    tr.find(".table-row-total").html(total.toFixed(2));
}

function deleteItem(pid) {
    $.ajax({
        url: BaseUrl.Api + "/api/app/basket?PId=" + pid + "&Clear=true",
        type: "DELETE",
        success: function (response) {
            console.log(response);
            loadTable();
        }
    });
}