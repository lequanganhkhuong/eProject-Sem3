
$.cookie.json = true;
$.cookie.defaults.path = '/';

function getCartItems() {
    if ($.cookie('productlist')) {
        return $.cookie('productlist').cartItems;
    } else {
        return [];
    }
}

function saveCartItems(cart_items) {
    var obj = { "cartItems": cart_items };
    $.cookie('productlist', obj);
}

function emptyCartItems() {
    $.removeCookie('productlist');
}

function addItem(productid, productname, thumbnail, price, quantity) {

    var cart_items = getCartItems();

    var is_exist = false;
    $(cart_items).each(function (i, v) {
        if (v && v.productid == productid) {
            v.quantity = parseInt(v.quantity, 10) + parseInt(quantity, 10);
             saveCartItems(cart_items);;
            alert("Added " + quantity + " product(s) to your cart!");
            is_exist = true;
        }
    });
    if (!is_exist) {
        var new_item = { "productid": productid, "productname": productname, "thumbnail": thumbnail, "price": price.replace(/,/g, ''), "quantity": quantity };
        cart_items.push(new_item);
        saveCartItems(cart_items);
        alert("Added " + quantity +" product(s) to your cart!");
    } else {
        
    }
}

function updateItem(productid, q) {

    var cart_items = getCartItems();
    var t = 0;
    $(cart_items).each(function (i, v) {
        if (v && v.productid == productid) {
            cart_items[i].quantity = q;
            saveCartItems(cart_items);
            t = cart_items[i].price * q;
        }
    });
    return t;
}

function removeItem(productid) {

    var cart_items = getCartItems();

    $(cart_items).each(function (i, v) {
        if (v && v.productid == productid) {
            cart_items.splice(i, 1);
        }
    });

    saveCartItems(cart_items);
}

function showTotal() {
    var total = 0;
    var cart_items = getCartItems();
    $(cart_items).each(function (i, v) {
        if (v) {
            total += v.price * v.quantity;
        }
    });
    $("#sumtotal").html(" "+total.toLocaleString('en'));
}

function loadCartItems() {

    var cart_items = getCartItems();
  
    var total = 0;
    $("#carts").html("");
    $(cart_items).each(function (i, v) {
        var t = v.price * v.quantity;
        total += t;
        $("#carts").append("<tr>"
            + "<td align='center'>" + v.productid + "</td>"
            + "<td align='center'>" + v.productname + "</td>"
            + "<td align='center'><img style='height:80px;width:auto;max-width:150px;' src='/Uploads/Products/" + v.productid + "/" + v.thumbnail+"'></td>"
            + "<td align='right'>" + parseFloat(v.price).toLocaleString('en') + "</td>"
            + "<td align='right'><input type='number' class='quantity' value='" + v.quantity + "' min='1' max='1000'></td>"
            + "<td align='right'>" + t.toLocaleString('en') + "</td>"
            + "<td><button class='removeitem'>Remove</button></td>"
            + "</tr>");
    });
    $("#sumtotal").html(total.toLocaleString('en'));

    $(".quantity").bind("keyup change click", function () {
        var tr = $(this).closest("tr").find("td");
        var t = updateItem(tr.eq(0).html(), $(this).val());
        tr.eq(5).html(t.toLocaleString('en'));
        showTotal();
    });

    $(".removeitem").click(function () {
        if (confirm("Are you sure to remove this item?")) {
            var tr = $(this).closest("tr").find("td");
            removeItem(tr.eq(0).html());
            tr.remove();
            showTotal();
            CartItemsCount();
        }
    });
}

function Checkout() {
    var cart_items = getCartItems();

    $.ajax({
        url: '/Carts/Checkout',
        type: "POST",
        data: "data=" + JSON.stringify(cart_items),
        success: function (response) {
            if (response == "OK") {
                alert("Checked out successfully");
                emptyCartItems();
                location.href = "/Products/";
            } else {
                alert(response);
            }
        },
        error: function () {

        }
    });

    
}

function CartItemsCount() {
    var count = 0;
    var cartitem = getCartItems();
    $(cartitem).each(function (i, v) {
        count++;
    });
    if (count != 0) {
        $(".cart-total").css("background-color", "red");
    } else {
        $(".cart-total").css("background-color", "#919fb3");
    }
    //alert(count);
    $(".cart-total").html(count);
}
    $(document).ready(function () {
        $(".quick-add-to-cart").click(function () {
            var a = $(this);
            
            addItem(a.attr("pid"), a.attr("pname"), a.attr("pthumbnail"), a.attr("pprice"), 1);
            CartItemsCount();
            return false;

        });

        loadCartItems();
        CartItemsCount();
        $("#emptycart").click(function () {
                if (confirm("Are you sure to remove all items?")) {
                    emptyCartItems();
                    loadCartItems();
                    CartItemsCount();
                }
            });
            $("#checkout").click(function () {
                Checkout();
                CartItemsCount();
        });

    });


