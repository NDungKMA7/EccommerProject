$(document).ready(function () {
    $.noConflict();

    axios.get('/Admin/Orders/GetListRecord')
        .then(function (response) {

            var data = response.data;


            $('#OrderTable').DataTable({
                data: data,
                pageLength: 10,
                columns: [
                    {
                        data: 'nameUser',
                        render: function (data, type, row) {
                            return '<strong>' + data + '</strong>';
                        }
                    },
                    {
                        data: 'addressUser',
                        render: function (data, type, row) {
                            return data;
                        }
                    },
                    {
                        data: 'phone',
                        render: function (data, type, row) {
                            return data;
                        }
                    },
                    {
                        data: 'create',
                        render: function (data, type, row) {
                            return data;
                        }
                    },
                    {
                        data: 'status',
                        render: function (data, type, row) {
                            if (data == 0) {
                                return '<span class="badge bg-danger">' + 'Chưa giao hàng' +'</span>';

                            } else {
                                return '<span class="badge bg-success">' + 'Giao hàng thành công ' + '</span>';
                                 
                            }
                            
                        }
                    },
                    {
                        data: 'price',
                        render: function (data, type, row) {
                            
                            return data;
                        }
                    },
                    {
                        data: 'id',
                        render: function (data, type, row) {
                            var status = row.status;
                            var idOrder = data;
                            var Delivery = '/Admin/Orders/Delivery/' + idOrder;
                            var Detail = '/Admin/Orders/Detail/' + idOrder;
                            var dropdownHtml = '<div class="dropdown">' +
                                '<button type="button" class="btn p-0 dropdown-toggle hide-arrow" data-bs-toggle="dropdown">' +
                                '<i class="bx bx-dots-vertical-rounded"></i>' +
                                '</button>' +
                                '<div class="dropdown-menu">';

                            if (status == 0) {
                                dropdownHtml += '<a class="dropdown-item" href="' + Delivery + '"><i class="bx bx-package"></i> Delivery</a>';
                            }

                            dropdownHtml += '<a  class="dropdown-item" href="' + Detail + '"><i class="bx bx-detail"></i> Detail</a>' +
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

