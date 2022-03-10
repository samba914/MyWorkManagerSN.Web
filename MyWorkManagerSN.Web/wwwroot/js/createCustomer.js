
$("#addNewCustomer").click(function () {
    $("#editModelCustomerSubmit").attr("data-acionType", "edit");
    var name = $(this).attr("data-name");
    var surname = $(this).attr("data-surname");
    var email = $(this).attr("data-email");
    var mobile = $(this).attr("data-mobile");
    var address = $(this).attr("data-address");
    $("#oName").val(name);
    $("#oSurname").val(surname);
    $("#oEmail").val(email);
    $("#oMobile").val(mobile);
    $("#oAddress").val(address);
    currentId = $(this).attr("data-oId");
    $("#exampleModalCustomer").modal();

});


$("#catForm").submit(function (event) {

    event.preventDefault();
    if (document.getElementById('catForm').checkValidity()) {
        var name = $("#oName").val();
        var surname = $("#oSurname").val();
        var email = $("#oEmail").val();
        var mobile = $("#oMobile").val();


        $.post("/Customer/Add", {
            name: name, surname: surname, email: email, mobile: mobile
        }, function (res) {
            if (res.success) {
                $("#inputAutoComplete").attr("data-id", res._acts.CustomerId);
                $("#inputAutoComplete").val(res._acts.Surname + " " + res._acts.Name);
                autocomplete(document.getElementById("inputAutoComplete"), seachType);
                toastSuccess("Succès", res._acts.title);
            } else {
                console.log(res)
                toastError("Erreur", res._acts.title)
            }
            $("#exampleModalCustomer").modal("hide");
        })



    }


});
