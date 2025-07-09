var AddEditPage = {
    init: function () {

        const formJsonString = $('#formJsonString').val();
        const IsModelNull = $('#IsModelNull').val();

        const formJson =JSON.parse(formJsonString);
        console.log("Parsed formJson:", formJson);
        if (IsModelNull === "True") {
            alert("Model is null");
        }

        Formio.builder(document.getElementById('FormBuilder'), formJson).
            then(function (builder) {

                builder.on('change', () => {


                    const schema = builder.schema;
                    $('#jsonSchema').val(JSON.stringify(schema));
                });
            });

        this.AddEditForm();

        $('#title').on('input', function () {
            $('#error').hide();
        });

    },

    AddEditForm: function () {
        $('#saveForm').on('submit', function (event) {
            event.preventDefault();
            const formId = $('#formId').val();
            const formTitle = $('#title').val().trim();
            const formFields = $('#jsonSchema').val().trim();
            $('#error').hide();   
            if (!formTitle) {
                $('#error').show();    
                $('#title').focus();   
                return;                
            }
            const isEdit = formId && parseInt(formId) > 0;
            const activeUrl = isEdit ? '/Form/UpdateForm' : '/Form/AddForms';
            $.ajax({
                url: activeUrl,
                type: 'POST',
                data: {
                    Id: formId,
                    Title: formTitle,
                    FormFields: formFields
                },
                success: function (response) {
                    if (response.success) {
                        const message = isEdit ? "Form updated successfully." : "Form added successfully.";
                        showSuccessPopup(message, response.redirectUrl);
                    } else {
                        Swal.fire('Oops!', response.message || 'Something went wrong.', 'error');
                    }
                }, 
                error: function (xhr, status, result) {
                    if (xhr.status === 401) {
                        alert("You are not authorized to perform this action. Please log in first.");
                    } else if (xhr.status === 400) {
                        alert("User not found! Please try again.");
                    } else {
                        alert("Something went wrong. Please try again.");
                    }
                }
            });


        });

    }

}
function showSuccessPopup(message, redirectUrl = null) {
    Swal.fire({
        icon: 'success',
        title: 'Success!',
        text: message,
        confirmButtonColor: '#7a5eff',
        confirmButtonText: 'OK'
    }).then(() => {
        if (redirectUrl) {
            window.location.href = redirectUrl;
        }
    });
}


