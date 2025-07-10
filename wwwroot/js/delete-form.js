var deleteForm = {
    init: function () {
        this.DeleteButton();
    },
    DeleteButton: function () {
        $(document).on("click", ".deleteBtn", function () {
            const formId = $(this).data("id");
            const row = $(this).closest("tr");
            Swal.fire({
                title: 'Are you sure?',
                text: "Once deleted, you will not be able to recover this form!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#d33',        
                cancelButtonColor: '#aaa',        
                confirmButtonText: 'Yes, delete it!',
                cancelButtonText: 'Cancel'
            }).then((result) => {
                if (result.isConfirmed) {
                   
                $.post('/Form/DeleteForm', { id: formId }, function (response) {
                        if (response.success) {
                            Swal.fire(
                                'Deleted!',
                                response.message || 'Form has been deleted.',
                                'success'
                            );
                            row.remove(); 
                        } else {
                            Swal.fire(
                                'Failed!',
                                response.message || 'Something went wrong.',
                                'error'
                            );
                        }
                    }).fail(function () {
                        Swal.fire('Error', 'Something went wrong during deletion.', 'error');
                    });
                }
            });
        });
    }
};
