$(function MyTest () {
    $('#FirstDD').change(function () {
        // получаем выбранный id
        var id = $(this).val();
        $.ajax({
            type: 'GET',
            url: 'GetNewDDList/' + id,
            success: function (data) {

                // заменяем содержимое присланным частичным представлением
                $('#SecondDD').replaceWith(data);
            }
        });
    });
})