
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


$("#formCustomer").submit(function (event) {

    event.preventDefault();
    if (document.getElementById('formCustomer').checkValidity()) {
        var name = $("#oName").val();
        var surname = $("#oSurname").val();
        var email = $("#oEmail").val();
        var mobile = $("#oMobile").val();


        $.post("/Customer/Add", {
            name: name, surname: surname, email: email, mobile: mobile
        }, function (res) {
            if (res.success) {
                console.log(res._acts)
                $("#inputAutoComplete").attr("data-id", res._acts.customerId);
                $("#inputAutoComplete").val(res._acts.surname + " " + res._acts.name);
                $("input[name='" + "searchType" + "'][value='" + "Prenom_Nom" + "']").prop('checked', true);
                autocomplete(document.getElementById("inputAutoComplete"), "Prenom_Nom");
                toastSuccess("Succès", res._acts.title);
            } else {
                
                toastError("Erreur", res._acts.title)
            }
            $("#exampleModalCustomer").modal("hide");
        })



    }


});
