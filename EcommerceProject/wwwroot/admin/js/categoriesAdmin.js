$(document).ready(function () {
    $.noConflict();

    axios.get('/Admin/Categories/GetListRecord')
        .then(function (response) {

            var data = response.data;


            $('#CategoriesTable').DataTable({
                data: data,
                columns: [
                    {
                        data: 'name',
                        render: function (data, type, row) {
                            return '<strong>' + data + '</strong>';
                        }
                    },
                    {
                        data: 'id',
                        render: function (data, type, row) {
                            var advId = data;
                            var EditUrlUpdate = '/Admin/Categories/Update/' + advId;
                            var EditUrlDelete = '/Admin/Categories/Delete/' + advId;
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
                        }
                    }
                ]
            });

        })
        .catch(function (error) {
            console.error('Lỗi khi gọi API: ', error);
        });
});

