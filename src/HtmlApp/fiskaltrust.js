
//$(document).ready(function(){
//    $("#test").click(test());
//});


function test() {
    console.log("test start");

    var url = $("#serviceurl").val();
    url += "/json/sign";
    console.log(url);

    var reqdata = $("#reqdata").val();
    console.log(reqdata);
    var obj = JSON.parse(reqdata);
    console.log(obj);

    $.ajax({
        url: url,
        headers:{
            "cashboxid": "654fd35d-75eb-49e2-90cd-2980c67350cf",
            "accesstoken": "BIlyGYZPJXOnwx+MyVekpAFXOjpPFim5+U8EHxuKbOcF/CAA6kSNg1Im+mZstNiIuT0F946RbRPRTO+H6Ngq9q8="
        },
        type: "POST",
        contentType: "application/json",
        crossDomain: true,
        data: reqdata,
        success: success,
        error: error
    });

    console.log("test end");
}

function success(data,textStatus,jqXHR ){
    console.log(textStatus);
    console.log(data);
    $("#respdata").val(JSON.stringify(data));
}

function error(jqXHR,textStatus,errorThrown){
    console.log(textStatus);
    console.log(errorThrown);
    $("#respdata").val(textStatus + " " + errorThrown);
}