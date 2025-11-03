// Entry point: Chạy khi trang đã được tải xong.
$(function () {
    initAll();
});

/**
 * Hàm khởi tạo chính cho tất cả các trang thuộc module Training.
 * Sẽ gọi các hàm khởi tạo con tương ứng với từng chức năng.
 */
function initAll() {
    // Khởi tạo chức năng bảng đóng/mở cho trang Index.
    initCollapsibleTable();

    // Khởi tạo chức năng checkbox "chỉ chọn một" cho trang Survey.
    initSurveyCheckbox();
}

/* ========================================================================== */
/* [Trang Index] - Bảng đóng/mở nhóm với hiệu ứng xoay icon                  */
/* ========================================================================== */

/**
 * Khởi tạo logic cho bảng đóng/mở nhóm với hiệu ứng xoay icon mượt.
 *
 * Cách hoạt động:
 * - Lắng nghe sự kiện show.bs.collapse: thêm class 'is-open' để icon xoay xuống.
 * - Lắng nghe sự kiện hide.bs.collapse: xóa class 'is-open' để icon xoay sang phải.
 * - Đồng thời đồng bộ trạng thái khi trang load (nếu có nhóm đang mở sẵn).
 */
function initCollapsibleTable() {
    // Duyệt qua tất cả các dòng header có thể đóng/mở
    $('.group-header').each(function () {
        const $header = $(this);
        const targetSelector = $header.data('bs-target');

        if (!targetSelector) return;

        // Lắng nghe sự kiện show/hide của Bootstrap collapse
        $(targetSelector)
            .on('show.bs.collapse', function () {
                $header.addClass('is-open expanded'); // icon xoay xuống
            })
            .on('hide.bs.collapse', function () {
                $header.removeClass('is-open expanded'); // icon xoay lại
            });

        // Đồng bộ trạng thái khi trang load
        const $targets = $(targetSelector);
        const anyShown = $targets.toArray().some(el => $(el).hasClass('show'));
        if (anyShown) {
            $header.addClass('is-open expanded');
        } else {
            $header.removeClass('is-open expanded');
        }

        // Cho phép click trực tiếp trên header cũng hoạt động (nếu chưa gán bootstrap trigger)
        $header.off('click').on('click', function () {
            const $targets = $(targetSelector);
            $targets.collapse('toggle');
        });
    });
}

/* ========================================================================== */
/* [Trang Survey] - Checkbox chỉ chọn một trong mỗi hàng                     */
/* ========================================================================== */

/**
 * Khởi tạo chức năng cho các checkbox trong bảng khảo sát.
 * Đảm bảo người dùng chỉ có thể chọn một checkbox trên mỗi hàng.
 */
function initSurveyCheckbox() {
    const $surveyCheckboxes = $('table.table-bordered tbody input[type="checkbox"]');

    // Nếu không có checkbox khảo sát thì không cần chạy
    if ($surveyCheckboxes.length === 0) return;

    $surveyCheckboxes.on('change', function () {
        const $clicked = $(this);
        if ($clicked.is(':checked')) {
            // Bỏ chọn các checkbox khác trong cùng hàng
            $clicked
                .closest('tr')
                .find('input[type="checkbox"]')
                .not(this)
                .prop('checked', false);
        }
    });
}
