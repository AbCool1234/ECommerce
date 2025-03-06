
$(document).on('click', '.btnCart', function () {
    var obj = {
        Id: $(this).data('pk'),
        ProductName: $(this).data('name'),
        UnitPrice: $(this).data('price'),
        Quantity: 1,
        Total: $(this).data('price')
    };

    var oldItems = localStorage.getItem('ls_product') || '[]';
    var oldItemsJSON = JSON.parse(oldItems);  //  []

    var existData = oldItemsJSON.filter(x => x.Id == obj.Id);
    if (existData && existData.length > 0) {
        alert('Product Already in Cart')
        return;
    }


    oldItemsJSON.push(obj)

    localStorage.setItem('ls_product', JSON.stringify(oldItemsJSON));
    alert('Product Added in Cart')
});



function loadItems() {
    var pro = localStorage.getItem('ls_product') || '[]';
    var obj = JSON.parse(pro);  // []

    var htmls = '';

    $.each(obj, function (i, x) {
        htmls += '<tr>';
        htmls += '<td>' + x.Id + '</td>';
        htmls += '<td>' + x.ProductName + '</td>';
        htmls += '<td>' + x.UnitPrice + '</td>';
        htmls += '<td><input data-key="' + x.Id + '" type="text" class="form-control txtQuantity" value="' + x.Quantity + '"></td>';
        htmls += '<td>' + x.Total + '</td>';
        htmls += '<td><button data-key="' + x.Id + '" type="button" class="btn btn-danger btn-sm btnRemoveItem">DEL</button></td>';
        htmls += '</tr>';
    })

    $('.body-item').html(htmls);
}


$(document).on('click', '.btnClearCart', function () {
    var ok = confirm('Are you sure to clear cart?')
    if (ok) {
        localStorage.removeItem('ls_product');
        loadItems();
        CalcuateAndShowTotal();
    }
})


$(document).on('click', '.btnRemoveItem', function () {
    var ok = confirm('Are you sure to remove this product?')
    if (ok) {
        var id = $(this).data('key');

        var pro = localStorage.getItem('ls_product') || '[]';
        var obj = JSON.parse(pro);  // []

        obj = obj.filter(x => x.Id != id);

        localStorage.setItem('ls_product', JSON.stringify(obj));
        loadItems();
        CalcuateAndShowTotal();
    }
})


$(document).on('change', '.txtQuantity', function () {

    var id = $(this).data('key'); 
    var newQuantity = $(this).val();

    var pro = localStorage.getItem('ls_product') || '[]';
    var oldItem = JSON.parse(pro);  // []

    var selectedItemArray = oldItem.filter(x => x.Id == id);
    if (selectedItemArray.length <= 0) {
        alert('Product Item Not Found')
        return;
    }

    var selectedItem = selectedItemArray[0];

    selectedItem.Quantity = newQuantity;
    selectedItem.Total = +newQuantity * +selectedItem.UnitPrice


    localStorage.setItem('ls_product', JSON.stringify(oldItem));
    loadItems();
    CalcuateAndShowTotal();
});



$(document).on('click', '.btnCheckout', function () {
    $('#mdlCheckout').modal('show');
});
$(document).on('click', '.txtCartSubmit', function () {
    var mast = {
        Fullname: $('.txtCartFullname').val() || '',
        MobileNo: $('.txtCartMobileNo').val() || '',
        Email: $('.txtCartEmail').val() || '',
        Address: $('.txtCartAddress').val() || ''
    };
    if (mast.Fullname == '') {
        alert('Enter Fullname')
    } else if (mast.MobileNo == '') {
        alert('Enter Mobile No')
    } else {
        var oldItems = localStorage.getItem('ls_product') || '[]';
        var oldItemsJSON = JSON.parse(oldItems);  //  []

        var payload = {
            mast: mast,
            dtl: oldItemsJSON
        };


        $.ajax({
            method: 'post',
            contentType: "application/json; charset=utf-8",
            url: '/Product/SaveOrder',
            data: JSON.stringify(payload),
            success: function (resp) {
                if (resp.success) {
                    alert(resp.message)

                    localStorage.removeItem('ls_product');
                    loadItems();
                    CalcuateAndShowTotal();

                    clearCloseModal();

                    window.location.href = "/Product/Pay?id=" + resp.pk;
                } else {
                    alert(resp.message)
                }
            }
        });
    }
});


function clearCloseModal() {
    $('.txtCartFullname').val('');
    $('.txtCartMobileNo').val('');
    $('.txtCartEmail').val('');
    $('.txtCartAddress').val('');

    $('#mdlCheckout').modal('hide');
}



function CalcuateAndShowTotal() {
    var oldItems = localStorage.getItem('ls_product') || '[]';
    var oldItemsJSON = JSON.parse(oldItems);  //  []


    var grandTotal = 0;
    $.each(oldItemsJSON, function (i, x) {
        grandTotal += (x.UnitPrice * x.Quantity); 
    });

    $('.tdGrandTotal').html(grandTotal);
}
