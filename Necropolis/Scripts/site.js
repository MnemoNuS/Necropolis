$(function () {
    $('#menu').slicknav({
        allowParentLinks: true,
        label: 'Разделы сайта',
        prependTo: '#menu-frame'
    });
});

    $(document).ready(function () {
        jQuery('.camera_wrap').camera();
        $('.magnifier').touchTouch();
    });

    $(document).ready(function () {
        var activeTab = $('#menu').attr('active-tab');
        $('.active').remove("active");
        $('#' + activeTab).addClass("active");
    });
