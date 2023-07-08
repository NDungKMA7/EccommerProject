$(document).ready(function () {
    $.noConflict();

    axios.get('/Admin/Slides/GetListRecord')
        .then(function (response) {

            var data = response.data;


            $('#SlidesTable').DataTable({
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
                            return '<img src="/Upload/Slides/' + data + '" style="width:100px;" />';
                        }, orderable: false
                    },
                    {
                        data: 'title',
                        render: function (data, type, row) {
                            return data ;
                        }, orderable: false
                    },
                    {
                        data: 'subTitle',
                        render: function (data, type, row) {
                            return data;
                        },
                        orderable: false
                    },
                    {
                        data: 'info',
                        render: function (data, type, row) {
                            return data;
                        },
                        orderable: false
                    },
                    {
                        data: 'link',
                        render: function (data, type, row) {
                            return data;
                        },
                        orderable: false
                    },
                    {
                        data: 'id',
                        render: function (data, type, row) {
                            var advId = data;
                            var EditUrlUpdate = '/Admin/Slides/Update/' + advId;
                            var EditUrlDelete = '/Admin/Slides/Delete/' + advId;
                            var dropdownHtml = '<div class="dropdown">' +
                                '<button type="button" class="btn p-0 dropdown-toggle hide-arrow" data-bs-toggle="dropdown">' +
                                '<i class="bx bx-dots-vertical-rounded"></i>' +
                                '</button>' +
                                '<div class="dropdown-menu">' +
                                '<a class="dropdown-item" href="' + EditUrlUpdate + '"><i class="bx bx-edit-alt me-1"></i> Edit</a>' +
                                '<a class="dropdown-item" href="' + EditUrlDelete + '" ><i class="bx bx-trash me-1"></i> Delete</a>' +
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

