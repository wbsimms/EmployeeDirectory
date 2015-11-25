$(document).ready(function() {
        dataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: "/Home/GetAll",
                    dataType: "json"
                },
                update: {
                    url: "/Home/Update",
                    dataType: "json",
                    type: "POST"
                },
                destroy: {
                    url: "/Home/Delete",
                    dataType: "json",
                    type: "POST"
                },
                create: {
                    url: "/Home/Create",
                    dataType: "json",
                    type: "POST"
                }
        },
            batch: false,
        pageSize: 20,
        schema: {
            model: {
                id: "Id",
                fields: {
                    Id: { editable: false, nullable: true, type:"number" },
                    FirstName: { type:"string", validation: { required: true } },
                    LastName: { type: "string", validation: { required: true } },
                    Address: { type: "string", validation: {required: true} },
                    Phone: { type: "string", validation: { min: 0, required: true } },
                    Email: { type: "string", validation: { min: 0, required: true } },
                    JobTitle: { type: "string", validation: { min: 0, required: true } },
                }
            }
        }
    });

    $("#grid").kendoGrid({
        dataSource: dataSource,
        pageable: true,
        height: 550,
        toolbar: ["create"],
        columns: [
            "FirstName",
            { field: "LastName", title: "Last Name", width: "120px" },
            { field: "Address", title: "Address", width: "120px" },
            { field: "Phone", width: "120px" },
            { field: "Email", width: "120px" },
            { field: "JobTitle", width: "120px" },
            { command: ["edit", "destroy"], title: "&nbsp;", width: "250px" }],
        editable: "inline"
    });
});