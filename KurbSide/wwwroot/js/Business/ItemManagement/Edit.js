window.onload = function () {
    document.getElementById('resetImageEdit').onclick = function () {
        document.getElementById('itemImageEdit').value = '';
        document.getElementById('labelForItemImage').innerHTML = '';
        document.getElementById('imagePreviewEdit').src = document.getElementById('originalLocation').value
    }

    document.getElementById('imagePreviewEdit').onerror = function () {
        this.alt = 'Invalid Image Selected.'
    }

    document.getElementById('itemImageEdit').onchange = function () {
        document.getElementById('imagePreviewEdit').src = window.URL.createObjectURL(this.files[0])
        document.getElementById('imagePreviewEdit').hidden = false;
    }

    $('.custom-file input').change(function (e) {
        if (e.target.files.length) {
            $(this).next('.custom-file-label').html(e.target.files[0].name);
        }
    });
};