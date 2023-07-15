$(document).ready(function () {
    $.noConflict();
 
    axios.get('/Admin/Advs/GetListRecord')
        .then(function (response) {
           
            var data = response.data;


            $('#AdvTable').DataTable({
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
                            return '<img src="/Upload/Adv/' + data + '" style="width:100px;" />';
                        },
                        orderable: false
                    },
                    {
                        data: 'position',
                        render: function (data, type, row) {
                            switch (data) {
                                case 1: {
                                    return 'Banner dưới Service Section Home Page';
                                    break;
                                };
                                case 2: {
                                    return 'Banner dưới Product Section Home Page';
                                    break;
                                };
                                case 3: {
                                    return 'Banner dưới News Section Home Page';
                                    break;
                                };
                                case 4: {
                                    return 'Banner dưới Siderbar Page';
                                    break;
                                };
                            }

                        },
                        orderable: false
                    },
                    {
                        data: 'id',
                        render: function (data, type, row) {
                            var advId = data;
                            var EditUrlUpdate = '/Admin/Advs/Update/' + advId;
                            var EditUrlDelete = '/Admin/Advs/Delete/' + advId;
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

