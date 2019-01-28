

$(document).ready(function () {

    var frame = $("#slider-frame");

    var maxPrice = frame.attr('max');
    var minPrice = frame.attr('min');

    if (minPrice < 100)
        minPrice = 0;
    maxPrice = Math.ceil(maxPrice / 100) * 100;

    $("#slider").rangeSlider(
  { arrows: false },
  { bounds: { min: +minPrice, max: +maxPrice } },
  { defaultValues: { min: +minPrice, max: +maxPrice } },
  { step: 100 },
  {
      symmetricPositionning: true,
      range: { min: 0 }
  });

    var items = $(".item");


    $("#slider").bind("valuesChanging", function (e, data) {
        items.each(function () {
            var i = $(this);
            var price = i.find(".item-price").attr("price");
            if (+price >= +data.values.min && +price <= +data.values.max) {
                i.css('display', 'block');
            } else {
                i.css('display', 'none');
            }

        });
    });

});

