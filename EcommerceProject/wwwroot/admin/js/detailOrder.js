
$(document).ready(function () {
    $.noConflict();
    var id = $("#Id_order").val();

    axios.get('/Admin/Orders/GetDetailJson/' + id)
        .then(function (response) {

            var data = response.data;


            $('#OrderDetailTable').DataTable({
                data: data,
                pageLength: 10,
                columns: [
                    {
                        data: 'name',
                        render: function (data, type, row) {
                            return '<strong>' + data + '</strong>';
                        }
                    },
                    {
                        data: 'photo',
                        render: function (data, type, row) {
                            var html = '';
                            if (data != " ") {
                                var elements = data.split(', ');
                                for (var i = 0; i < elements.length; i++) {
                                    if (elements[i] != "") {
                                        html += '<img src="/Upload/Products/' + elements[i] + '" style="width:100px;" />';
                                        break;
                                    }

                                }
                            }

                            return html;
                        },
                        orderable: false
                    },
                    {
                        data: 'price',
                        render: function (data, type, row) {
                            return data;
                        }
                    },
                    {
                        data: 'quantity',
                        render: function (data, type, row) {
                            return data;
                        }
                    }
                ]
            });

        })
        .catch(function (error) {
            console.error('Lỗi khi gọi API: ', error);
        });
});

