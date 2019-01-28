$(document).ready(function () {
    $('#menu').slicknav({
        allowParentLinks: true,
        label: 'Разделы сайта',
        prependTo: '#menu-frame'
    });

    jQuery('.camera_wrap').camera();
    $('.magnifier').touchTouch();

    var activeTab = $('#menu').attr('active-tab');
    $('.active').remove("active");
    $('#' + activeTab).addClass("active");

    $('.sub-categories-handler').on("click", function (e) {
        e.stopPropagation();
        var categories = $('.sub-categories');
        if (categories.hasClass("show")) {
            categories.removeClass("show");
        } else {
            categories.addClass("show");
        }
    });
    var w = window.innerHeight;
    var d = $("body").height();
    var h = $('header').height();
    var f = $('footer').height();
    if(d<w)
        $('.body-content').css("height", w - h - f);


    $("#brand").click(function (evt) {
        if (evt.ctrlKey && evt.shiftKey) {
            evt.preventDefault();
            evt.stopPropagation();
            document.location.replace('/auth');
        }
    });


    $("#goUp").click(function (evt) {
        $('html,body').animate({
            scrollTop: 0
        });
    });

    $("#goUp").click(function (evt) {
        $('html,body').animate({
            scrollTop: 0
        });
    });

    $(document).on( "scroll", function () {
        var height = $(window).height();
        var scrollTop = $(window).scrollTop();
        if (scrollTop > height) {
            $("#goUp").css('display', 'block');
        } else {
            $("#goUp").css('display', 'none');

        }
    });

});
