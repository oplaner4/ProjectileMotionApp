$(function () {
    var footer = $('footer');
    var bodyContent = $('.body-content');
    var confirmModal = $('.modal.modal-confirm');
    var alertSuccessMessage = $('.alert-success-message');

    window.initAndDisplayModalConfirm = function (confirmUrl, confirmText) {
        $('.modal-body', confirmModal).text(confirmText);
        $('.btn-ok', confirmModal).attr('href', confirmUrl);
        confirmModal.modal('show');
    };


    window.initAndDisplayAlertSuccessMessage = function (strongText, successText) {
        alertSuccessMessage.html('<strong>' + strongText + '</strong> ' + successText).slideDown().delay(3500).slideUp();
    }




    $(window).on('resize', function () {
        bodyContent.css('padding-bottom', footer.outerHeight());
    }).trigger('resize');


    $('[title]').tooltip({ placement: 'bottom' }).on('mouseleave click focus', function () {
        $(this).tooltip('hide');
    });

    $('[data-confirm]').on('click', function (e) {
        var $this = $(this);
        window.initAndDisplayModalConfirm($this.attr('href'), $this.data('confirm-text'));
        e.preventDefault();
        return false;
    });

    $('.carousel').carousel({
        interval: 3000
    }).filter('.carousel-toggle-carousel-fade').on('slide.bs.carousel', function (e) {
        var $this = $(this);
        if (e.from === $('img', $this).length - 1) {
            $this.toggleClass('carousel-fade');
        }
        });

    $('a.copy-to-clipboard').on('click', function (e) {
        e.preventDefault();
        var $this = $(this);
        $($this.data('copy-from')).select();
        if (document.execCommand("copy")) {
            window.initAndDisplayAlertSuccessMessage('Done!', 'Content has been successfully copied.');
        }
        return false;
    });
});