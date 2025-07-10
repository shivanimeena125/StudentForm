window.initializeForm = function (formId, formJson, isAdmin) {
    Formio.createForm(
        document.getElementById('formioRender'),
        formJson
    ).then(form => {
        setTimeout(() => {
            const submitBtn = document.querySelector('#formioRender button[type="submit"]');
            if (submitBtn) {
                submitBtn.disabled = true;
            }

            form.on('change', () => {
                const data = form.data;
                const allFields = form.schema.components;

                const allFilled = allFields.every(field => {
                    const value = data[field.key];
                    if (field.type === 'button') return true;
                    if (typeof value === 'object') {
                        return value && Object.keys(value).length > 0;
                    }
                    return value !== undefined && value !== null && value !== '';
                });

                if (submitBtn) {
                    submitBtn.disabled = !allFilled;
                }
            });
        }, 500);

        form.on('submit', function (submission) {
            const submissionData = JSON.stringify(submission.data);

            fetch('/api/submit-form', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                credentials: 'include',
                body: JSON.stringify({
                    formId: formId,
                    submissionData: submissionData
                })
            })
                .then(response => {
                    if (response.status === 401) {
                        window.location.href = "/Identity/Account/Login";
                        return;
                    }
                    return response.json();
                })
                .then(result => {
                    if (result?.success) {
                        alert(result.message);
                        if (isAdmin === "true") {
                            window.location.href = "/Admin/Form/ViewAllForms";
                        } else {
                            window.location.href = "/Home/OnlineForms";
                        }
                    } else {
                        alert("Submission failed.");
                    }
                })
                .catch(error => {
                    console.error("Error submitting form:", error);
                    alert("Something went wrong.");
                });
        });
    });
}
