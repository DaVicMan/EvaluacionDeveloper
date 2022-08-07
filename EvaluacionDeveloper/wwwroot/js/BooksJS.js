$(document).ready(function () {
    $("#LoadBookRegistration").click(function () {
        $("#SubmitBook").removeAttr('hidden');
        $("#SubmitEditedBook").attr('hidden', true); 
        $("#ISBN").val('');
        $("#BookName").val('');
        $("#Pages").val('');
        $("#PublishDate").val('');
        $("#RegisterBookTitle").text('Register Book');
        AjaxPost('/Books/GetAuthors', {}, function (Data) {
            $('#AuthorsList').empty();
            $("#AuthorsList").append('<option value="">Select an Author</option>');
            const Authors = Data;
            console.log(Authors);
            for (var i = 0; i < Authors.length; i++) {
                $("#AuthorsList").append('<option value="' + Authors[i].value + '">' + Authors[i].text + '</option>');
            }

        });
      
        $("#SubmitBook").click(function () {
            const ISBN = $("#ISBN").val();
            const BookName = $("#BookName").val();
            const PDate = $("#PublishDate").val();
            const Pages = $("#Pages").val();
            const Author = $("#AuthorsList").val();

            if (ISBN.length < 1 || BookName.length < 1 || PDate.length < 1 || Pages.length < 1 || Author.length < 1) {
                alert('Please fill every field');
            }
            else {
                AjaxPost('/Books/SubmitCreatedBook', { ISBN: ISBN, BookName: BookName, PublishDate: PDate, Pages: Pages, AuthorID: Author }, function (Data) {
                    if (Data != 1) {
                        alert('There was an error');
                    }
                    location.reload(true);
                })
            }
        });
    });

    $("#RegisteredBooks").on("click", '[id*=Edit ]', function () {
        $("#SubmitEditedBook").removeAttr('hidden');
        $("#SubmitBook").attr('hidden', true);
        $("#RegisterBookTitle").text('Edit Book');
        AjaxPost('/Books/GetAuthors', {}, function (Data) {
            $('#AuthorsList').empty();
            $("#AuthorsList").append('<option value="">Select an Author</option>');
            const Authors = Data;
           // console.log(Authors);
            for (var i = 0; i < Authors.length; i++) {
                $("#AuthorsList").append('<option value="' + Authors[i].value + '">' + Authors[i].text + '</option>');
            }

        });
        $("#SubmitEditedAuthor").removeAttr('hidden');
        $("#SubmitAuthor").attr('hidden', true);
        var ISBN = this.id.split(' ')[1];

        

        AjaxPost('/Books/GetBookInformation', { ISBN: ISBN }, function (Data) {
            $("#ISBN").val(ISBN);
            $("#BookName").val(Data.bookName);
            $("#Pages").val(Data.pages);
            $("#PublishDate").val(Data.publishDate);
            $("#AuthorsList").val(Data.authorID);
        });
        $("#SubmitEditedBook").click(function () {
            
            const ISBN = $("#ISBN").val();
            const BookName = $("#BookName").val();
            const PDate = $("#PublishDate").val();
            const Pages = $("#Pages").val();
            const Author = $("#AuthorsList").val();

            if (ISBN.length < 1 || BookName.length < 1 || PDate.length < 1 || Pages.length < 1 || Author.length < 1) {
                alert('Please fill every field');
            }
            else {
                AjaxPost('/Books/SubmitEditedBook', { ISBN: ISBN, BookName: BookName, PublishDate: PDate, Pages: Pages, AuthorID: Author }, function (Data) {
                    if (Data != 1) {
                        alert('There was an error');
                    }
                    location.reload(true);
                })
                
            }
            
        });

        

        //AjaxPost('/Authors/GetAuthorInformation', { id: IDUsuario }, function (Data) {
        //    console.log(Data)

        //    $("#FirstName").val(Data.firstName);
        //    $("#LastName").val(Data.lastName);
        //    $("#DateOfBirth").val(Data.dateOfBirth);

        //});
        //$("#SubmitEditedAuthor").click(function () {
        //    AjaxPost('/Authors/SubmitEditedUser', { id: IDUsuario, FirstName: $("#FirstName").val(), LastName: $("#LastName").val(), DateOfBirth: $("#DateOfBirth").val() }, function (Data) {
        //        if (Data != 1) {
        //            alert('There was an error');
        //        }
        //        location.reload(true);
        //    })
        //});
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