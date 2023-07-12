$(document).ready(function () {
    $.noConflict();

    axios.get('/Admin/Products/GetListProducts')
        .then(function (response) {

            var data = response.data;

            $('#ProductsTable').DataTable({
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
                            return '<img src="/Upload/Products/' + data + '" style="width:100px;" />';
                        },
                        orderable: false
                    },
                    {
                        data: 'imgSub',
                        render: function (data, type, row) {
                            var html = '';
                            if (data != " ") {
                                var elements = data.split(', ');
                                for (var i = 0; i < elements.length; i++) {
                                    if (elements[i] != "") {
                                        html += '<img src="/Upload/Products/' + elements[i] + '" style="width:100px;" />';
                                    }

                                }
                            }
                            
                            return html;
                        },
                        orderable: false
                    },
                 
                    {
                        data: 'categories',
                        render: function (data, type, row) {
                            return data;
                        },
                        orderable: false
                    },
                    {
                        data: 'tags',
                        render: function (data, type, row) {
                            return data;
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
                        data: 'discount',
                        render: function (data, type, row) {
                            return data;
                        }
                    },
                    {
                        data: 'hot',
                        render: function (data, type, row) {
                            return data;
                        },
                        orderable: false
                    },
                    {
                        data: 'id',
                        render: function (data, type, row) {
                            var advId = data;
                            var EditUrlUpdate = '/Admin/Products/Update/' + advId;
                            var EditUrlDelete = '/Admin/Products/Delete/' + advId;
                            var dropdownHtml = '<div class="dropdown">' +
                                '<button type="button" class="btn p-0 dropdown-toggle hide-arrow" data-bs-toggle="dropdown">' +
                                '<i class="bx bx-dots-vertical-rounded"></i>' +
                                '</button>' +
                                '<div class="dropdown-menu">' +
                                '<a class="dropdown-item" href="' + EditUrlUpdate + '"><i class="bx bx-edit-alt me-1"></i> Edit</a>' +
                                '<a class="dropdown-item" href="' + EditUrlDelete + '"><i class="bx bx-trash me-1"></i> Delete</a>' +
                                '</div>' +
                                '</div>';
                            return dropdownHtml;
                        },
                        orderable: false
                    }
                ]
            });


        })
        .catch(function (error) {
            console.error('Lỗi khi gọi API: ', error);
        });
});

