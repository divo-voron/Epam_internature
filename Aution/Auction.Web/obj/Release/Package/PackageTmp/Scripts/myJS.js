function makeBid() {
    // объект для отправки
    var data = {
        ID: 15,
        Name: "nameData"
    };

    var request = new XMLHttpRequest();

    function reqReadyStateChange() {
        if (request.readyState == 4) {
            var status = request.status;
            if (status == 200) {
                //document.getElementById("output").innerHTML = request.responseText;
                alert(request.responseText);
            }
        }
    }

    //var params = "ID=" + encodeURIComponent(data.ID) + "&Name=" + encodeURIComponent(data.Name);
    //request.open("POST", "/Auction/MakeBid", true);
    //request.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    //request.onreadystatechange = reqReadyStateChange;
    //request.send(params);
    
    //var params = "ID=" + data.ID + "&Name=" + data.Name;
    //request.open("GET", "/Auction/MakeBid?" + params, true);
    //request.onreadystatechange = reqReadyStateChange;
    //request.send(null);

    var params = JSON.stringify({
        ID: data.ID,
        Name: data.Name
    });
    request.open("POST", "/Auction/MakeBid", true);
    request.setRequestHeader("Content-Type", 'application/json; charset=utf-8');
    request.send(params);
}

function loadData() {

    var xhr = new XMLHttpRequest();

    xhr.open('GET', '/Auction/GetLot', true);
    xhr.send();

    xhr.onreadystatechange = function () {
        if (xhr.readyState != 4) return;

        if (xhr.status != 200) {
            // обработать ошибку
            alert(xhr.status + ': ' + xhr.statusText);
        } else {
            // вывести результат
            var data = JSON.parse(xhr.responseText);

            if (data.length == 2) {
                
                document.getElementById('LotName1').innerHTML = data[0].Title;
                document.getElementById('time1_1').innerHTML = data[0].TimeToEnd;
                document.getElementById('time2_1').innerHTML = data[0].Bider;

                document.getElementById('LotName2').innerHTML = data[1].Title;
                document.getElementById('time1_2').innerHTML = data[1].TimeToEnd;
                document.getElementById('time2_2').innerHTML = data[1].Bider;
            }
        }
    }
}

var timerId = setInterval(loadData, 5000);