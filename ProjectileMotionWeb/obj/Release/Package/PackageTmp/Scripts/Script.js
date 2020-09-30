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
    };


    $(window).on('resize', function () {
        bodyContent.css('padding-bottom', footer.outerHeight());
    }).trigger('resize');

    $(this).tooltip({
        selector: '[title]',
        placement: 'bottom',
        offset: 0,
        trigger: 'hover'
    }).on('click mouseleave focus', '[title]', function (e) {
        $(e.currentTarget).tooltip('hide');
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


    $('.color-picker').each(function () {
        var inp = $('input[type="text"]', this);

        var picker = new Picker({
            parent: this,
            alpha: false,
            color: inp.val(),
            popup: 'bottom',
            editorFormat: 'hex'
        }).onDone = function (color) {
            inp.val(color.hex.substring(0, 7)).css({ 'color': color.hex, 'border-color' : color.hex });
        };
        inp.css({ 'color': inp.val(), 'border-color': inp.val() });
    });
});