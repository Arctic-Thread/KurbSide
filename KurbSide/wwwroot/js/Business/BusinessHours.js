window.onload = function () {
    // When the checkboxes are clicked the input boxes are enabled/disabled.
    document.getElementById('chkMonday').onchange = function () {
        document.getElementById('MonOpen').disabled = !this.checked;
        document.getElementById('MonClose').disabled = !this.checked;
    };

    document.getElementById('chkTuesday').onchange = function () {
        document.getElementById('TuesOpen').disabled = !this.checked;
        document.getElementById('TuesClose').disabled = !this.checked;
    };

    document.getElementById('chkWednesday').onchange = function () {
        document.getElementById('WedOpen').disabled = !this.checked;
        document.getElementById('WedClose').disabled = !this.checked;
    };

    document.getElementById('chkThursday').onchange = function () {
        document.getElementById('ThuOpen').disabled = !this.checked;
        document.getElementById('ThuClose').disabled = !this.checked;
    };

    document.getElementById('chkFriday').onchange = function () {
        document.getElementById('FriOpen').disabled = !this.checked;
        document.getElementById('FriClose').disabled = !this.checked;
    };

    document.getElementById('chkSaturday').onchange = function () {
        document.getElementById('SatOpen').disabled = !this.checked;
        document.getElementById('SatClose').disabled = !this.checked;
    };

    document.getElementById('chkSunday').onchange = function () {
        document.getElementById('SunOpen').disabled = !this.checked;
        document.getElementById('SunClose').disabled = !this.checked;
    };

    // If the length (cannot check for null as they are strings with length of 0) of
    // Open time & Close time are both greater 0 then the time is set.
    // The checkbox is then clicked, enabling the input boxes.
    if ((document.getElementById('MonOpen').value).length > 0 || (document.getElementById('MonClose').value).length > 0) {
        document.getElementById('chkMonday').click();
    }

    if ((document.getElementById('TuesOpen').value).length > 0 || (document.getElementById('TuesClose').value).length > 0) {
        document.getElementById('chkTuesday').click();
    }

    if ((document.getElementById('WedOpen').value).length > 0 || (document.getElementById('WedClose').value).length > 0) {
        document.getElementById('chkWednesday').click();
    }

    if ((document.getElementById('ThuOpen').value).length > 0 || (document.getElementById('ThuClose').value).length > 0) {
        document.getElementById('chkThursday').click();
    }

    if ((document.getElementById('FriOpen').value).length > 0 || (document.getElementById('FriClose').value).length > 0) {
        document.getElementById('chkFriday').click();
    }

    if ((document.getElementById('SatOpen').value).length > 0 || (document.getElementById('SatClose').value).length > 0) {
        document.getElementById('chkSaturday').click();
    }

    if ((document.getElementById('SunOpen').value).length > 0 || (document.getElementById('SunClose').value).length > 0) {
        document.getElementById('chkSunday').click();
    }
}