$(function ()
{
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        e.target // newly activated tab
        e.relatedTarget // previous active tab
        console.log(e);
        console.log("e.target"+e.target );
        console.log("e.relatedTarget"+e.relatedTarget);
    });
});

function loadDetails(pid)
{
    $.ajax({
        url: BaseUrl.Api + "/api/app/product/" + pid,
        type: "GET",
        success: function (response)
        {
            handleDetails(response.data)
        }
    });
}

function handleDetails(data)
{
    initImages(data.images);
    initDetails(data);

    var product = getLocalBasketItem(data.id);
    if (product != undefined) {
        loadDetailsBadge(product.count);
    } else
    {
        loadBasket();
        loadDetailsBadge(0);
    }

    $("#detailsModal").modal("show");
}

function initImages(imageList)
{
    var html = ""; 
    for (var i = 0; i < imageList.length; i++) {
        if (i == 0) {
            html += '<div class="carousel-item active"><img src="' + imageList[i] + '" style="width: 100%;height: 100%;" alt="..."></div>'
        } else
        {
            html += '<div class="carousel-item"><img src="' + imageList[i] + '" style="width: 100%;height: 100%;" alt="..."></div>'
        }
    }
    $("#modal-details-image").html(html);
}
function addBasketItemInDetails(pid) {
    $.ajax({
        url: BaseUrl.Api + "/api/app/basket",
        type: "POST",
        data: JSON.stringify({
            pid: pid
        }),
        contentType: "application/json",
        success: function (response) {
            var basketJSON = localStorage.getItem("basket");
            var list = JSON.parse(basketJSON);
            var product;
            var index = 0;
            if (basketJSON != "" && (list != null || list != undefined)) {
                for (var i = 0; i < list.length; i++) {
                    if (list[i].productId == pid) {
                        index = i;
                        product = list[i];
                    }
                }
            }
            if (product == undefined) {
                loadBasket();
                loadDetailsBadge(1);
            } else
            {
                product.count = product.count + 1;
                list[index] = product;
                localStorage.setItem("basket", JSON.stringify(list));
                loadDetailsBadge(product.count);
            }

        }
    });
}

function getLocalBasketItem(pid)
{
    var basketJSON = localStorage.getItem("basket");
    var list = JSON.parse(basketJSON);
    var product;
    if (basketJSON != "" && (list != null || list != undefined)) {
        for (var i = 0; i < list.length; i++) {
            if (list[i].productId == pid) {
                index = i;
                product = list[i];
            }
        }
    }
    return product;
}
function initDetails(data)
{
    $("#modal-details-title").html(data.name);
    $("#modal-details-description").html(data.description);
    $("#modal-details-stock").html(data.stock);
    $("#table-pane-details").html(data.summary);
    $("#modal-details-add").unbind('click').click(function ()
    {
        addBasketItemInDetails(data.id);
    });
}

function loadDetailsBadge(count)
{
    var setValue;
    if (count <= 0) {
        setValue = "";
    } else
    {
        setValue = count;
    }
    $("#badge-details-count").html(setValue);
}