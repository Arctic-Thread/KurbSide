function addToSale(saleId, itemId, addOrRemove) {
    $.ajax({
        type: "POST",
        url: 'ManageSaleItem?saleId=' + saleId + '&itemId=' + itemId + '&addOrRemove=' + addOrRemove,
        dataType: "text"
    });
}