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
    },

    AddEditForm: function () {
        $('#saveForm').on('submit', function (event) {
            event.preventDefault();
            const formId = $('#formId').val();
            const formTitle = $('#title').val().trim();
            const formFields = $('#jsonSchema').val().trim();

            if (!formTitle) {
                alert("Please enter a form title.");
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
                    alert(response.message);

                    if (response.redirectUrl) {
                        window.location.href = response.redirectUrl;
                    }
                } else {
                    alert(response.message);
                }
            },
                error: function (xhr, status, result) {
                    if (xhr.status === 401) {
                        alert("You are not authorized to perform this action. Please log in first.");
                    }
                    else if (xhr.status === 400) {
                        alert("user not found!. Please try again.");
                    }

                    else {
                        alert("something went wrong not form submit!. Please try again.");
                    }
                }
            })

        });

    }

}

