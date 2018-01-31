$(function () {
    $(".dropdown-category").hover(
        function () { $(this).addClass('open') },
        function () { $(this).removeClass('open') }
    );
});

// Function to display Toast Message
function ShowToast(message, timeout, type) {
    type = (typeof type === 'undefined') ? 'info' : type;
    toastr.options.timeOut = timeout;
    toastr.options.closeButton = true;
    toastr.options.positionClass = "toast-top-right";
    toastr[type](message);
}
