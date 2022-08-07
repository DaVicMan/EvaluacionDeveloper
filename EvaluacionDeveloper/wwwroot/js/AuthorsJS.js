$(document).ready(function () {
    $("#LoadRegistrationForm").click(function () {
        $("#FirstName").val('');
        $("#LastName").val('');
        $("#DateOfBirth").val('');

        
        $("#SubmitAuthor").removeAttr('hidden');
        $("#SubmitEditedAuthor").attr('hidden', true);
        $("#RegisterAuthorTitle").text("Register User");
    });
    $("#SubmitAuthor").click(function () {
        const Name = $("#FirstName").val();
        const LastName = $("#LastName").val();
        const DateOfBirth = $("#DateOfBirth").val();
        if (Name.length == 0 || LastName.length == 0 || DateOfBirth.length == 0) {
            alert('Please fill every field');
        }
        else {
            AjaxPost('/Authors/InsertNewAuthor', { FirstName: Name, LastName: LastName, DateOfBirth: DateOfBirth }, function (Data) {
                if (Data != 1) {
                    alert('There was an error');
                }
                location.reload(true);
            })
        }
    });

    $("#RegisteredAuthors").on("click", '[id*=Edit ]', function () {
        $("#RegisterAuthorTitle").text("Edit User");
        $("#SubmitEditedAuthor").removeAttr('hidden');
        $("#SubmitAuthor").attr('hidden', true);
        var IDUsuario = this.id.split(' ')[1];
        AjaxPost('/Authors/GetAuthorInformation', { id: IDUsuario }, function (Data) {
            console.log(Data)

            $("#FirstName").val(Data.firstName);
            $("#LastName").val(Data.lastName);
            $("#DateOfBirth").val(Data.dateOfBirth);

        });
        $("#SubmitEditedAuthor").click(function () {
            AjaxPost('/Authors/SubmitEditedUser', { id: IDUsuario, FirstName: $("#FirstName").val(), LastName: $("#LastName").val(), DateOfBirth: $("#DateOfBirth").val() }, function (Data) {
                if (Data != 1) {
                    alert('There was an error');
                }
                location.reload(true);
            })
        });
    });
       
});

function AjaxPost(Url, Data, Funcion) {
    $.ajax({
        url: Url,
        data: Data,
        dataType: "JSON",
        type: "POST",
        success: Funcion
    });
}