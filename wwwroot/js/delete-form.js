

var FormList = {
    init: function () {
        this.DeleteButton();
    },
    DeleteButton: function () {
        $(document).on("click", ".deleteBtn", function () {
            const formId = $(this).data("id");
            const row = $(this).closest("tr");
            console.log("Delete button clicked:", formId);

            if (confirm("Do you really want to delete this form?")) {
                $.post('/Form/DeleteForm', { id: formId }, function (response) {
                    alert(response.message);
                    if (response.success) {
                        row.remove();
                    }
                });
            }
        });
    }
};
