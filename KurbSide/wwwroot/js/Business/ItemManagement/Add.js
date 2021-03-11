window.onload = function () {
    document.getElementById('resetImage').onclick = function () {
        document.getElementById('itemImage').value = '';
        document.getElementById('labelForItemImage').innerHTML = '';
        document.getElementById('imagePreview').hidden = true;
    }

    document.getElementById('imagePreview').onerror = function () {
        this.alt = 'Invalid Image Selected.'
    }

    document.getElementById('itemImage').onchange = function () {
        document.getElementById('imagePreview').src = window.URL.createObjectURL(this.files[0])
        document.getElementById('imagePreview').hidden = false;
    }

    $(document).on('change', '.custom-file-input', function (event) {
        $(this).next('.custom-file-label').html(event.target.files[0].name);
    })
};