var Filter = {
    init: function () {
        this.searchForms();
    },

    searchForms: function () {
        $(document).ready(function () {
            var table = $('#formTable').DataTable({
                processing: true,
                serverSide: true,
                pageLength: 10,                
                lengthChange: false,           
                searching: false,              
                info: false,       
                ajax: {
                    url: '/Form/LoadFormData',
                    type: 'POST',
                    data: function (d) {
                        d.startDate = $('#startDate').val();
                        d.endDate = $('#endDate').val();
                        d.title = $('#titleSearch').val();
                    }
                },
                columns: [
                    { data: 'title' }, 
                    { data: 'createdByName' },
                    {
                        data: 'createdUtc',
                        render: function (data) {
                            return new Date(data).toLocaleString();
                        }
                    },
                    {
                        data: 'modifiedUtc',
                        render: function (data) {
                            return data ? new Date(data).toLocaleString() : '—';
                        }
                    },
                    {
                        data: 'formGroupId',
                        render: function (data, type, row) {
                            return `
                        <a href="/Form/ViewForm/${data}" class="btn btn-secondary btn-sm">View</a>
                        <a href="/Form/UpdateForm/${data}" class="btn btn-warning btn-sm mx-1">Edit</a>
                        <button class="btn btn-danger btn-sm deleteBtn" data-id="${row.id}">Delete</button> `;
                        },
                        orderable: false,
                        searchable: false
                    }
                ]
            });
            $('#titleSearch').on('input', function () {
                $('#formTable').DataTable().ajax.reload();
            });

            $('#filterBtn').on('click', function () {
                table.ajax.reload();
            });

            $('#resetBtn').on('click', function () {
                $('#startDate').val('');
                $('#endDate').val('');
                $('#titleSearch').val('');
                
                table.ajax.reload();
            });
        });
    }
}
