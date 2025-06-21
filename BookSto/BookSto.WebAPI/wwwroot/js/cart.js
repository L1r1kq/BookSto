function addToCart(bookId) {
    $.ajax({
        url: `/Cart/Add/${bookId}`,
        type: 'POST',
        data: { qty: 1 },
        headers: { 'X-Requested-With': 'XMLHttpRequest' },
        success: function (res) {
            // res.count — новое количество
            $('.badge.bg-danger').text(res.count);
        },
        error: function () {
            alert('Ошибка при добавлении в корзину');
        }
    });
}
