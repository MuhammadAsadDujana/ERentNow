
function Get(data, divElement,Find) {

    if (data != null || data != undefined) {
        var div = document.getElementById(divElement);

        $(div).find(Find).each(function (index, item) {

            var elemen = $(this)[0].localName;
            var Type = $(this).attr('type');
            var id = $(this).attr('id');

            if (Type == 'text') {

                var name = $(this).attr('name');
                var val = data[name];
                $(this).val(data[name]);                
            }
            if (Type == 'hidden') {
                var name = $(this).attr('name');
                var sadas = data[name];
                $(this).val(data[name]);
            }

            if (Type == 'password') {
                var name = $(this).attr('name');
                var sadas = data[name];
                $(this).val(data[name]);

            }
            if (Type == 'checkbox') {

                var name = $(this).attr('name');
                var sadas = data[name];
                $(this).prop('checked', data[name]);

            }

            if ($(this)[0].localName == 'select') {

                var id;
                var name = $(this).attr('name');
                var sadas = data[name];
                if (name != data[name]) {
                    var id = $(this).attr('id');
                    $(this).val(data[id]);
                } else {
                    $(this).val(data[name]);
                }
            }

            if ($(this)[0].localName == 'span') {
                var name = $(this).attr('id');
                var sadas = data[name];
                $(this).html(data[name]);
            }
            if ($(this)[0].localName == 'img') {

                var id = $(this).attr('id');
                var sadas = data[id];
                $(this).attr('src', '../' + data[id]);

            }
        });
    }
}


function GetDetail(data, BtnElement, DtlElement) {
    
    if (data != null || data != undefined || data.length > 0) {
        for (var i = 0; i < data.length; i++) {
            if (i > 0)
                $(BtnElement).click();
        }
        $(DtlElement + ' > tbody > tr').each(function (index, item) {

            for (var key in data[0]) {
                $(this).find("#" + key).val(data[index][key]);
            }
        });
    }
}