BaseUrl =
{
    User: "http://localhost:5005",
    Api: "https://localhost:5001"
};
MyShopDefaultPageSize = 10;

MyShopDailyGoodsTmplate = '<div class="col-sm-6 col-md-3"><div class="thumbnail rounded"><img class="img-fluid" style="min-width:40px;min-height:40px" src="$Cover" alt="通用的占位符缩略图" /><div class="caption"><h6>$Title</h6><span style="font-size:1px;"  class="font-desc">$Description</span><div class="btn-group btn-group-justified"><button class="btn btn-primary" onclick="loadDetails($DPId);">详情</button><button class="btn btn-info">关注</button><button class="btn btn-warning" onclick="addBasketItem($PId);">加入购物车</button></div></p></div></div></div>';

ModalClass = ["modal-error", "modal-success", "modal-warning"];
