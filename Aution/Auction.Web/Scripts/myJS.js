function makeBid() {
    
    var request = new XMLHttpRequest();

    function reqReadyStateChange() {
        if (request.readyState == 4) {
            var status = request.status;
            if (status == 200) {
                alert(request.responseText);
            }
        }
    }

    var data = $("[data-id=1]").children("form").children("input");
    if (data.length == 3) {
        var params = JSON.stringify({
            ID: data[0].value,
            Name: data[1].value
        });
        request.open("POST", "/Auction/MakeBid", true);
        request.setRequestHeader("Content-Type", 'application/json; charset=utf-8');
        request.send(params);

        $('#yourbids').append("<li><span class=\"Name\">" + data[1].value + "</span> <span class=\"time\">" + formatDate(new Date()) + "</span> <span class=\"lotid\">" + data[0].value + "</span></li>");
    }
}

function formatDate(date) {

    var day = date.getDate() < 10 ? '0' + date.getDate() : date.getDate();
    var month = date.getMonth() < 10 ? '0' + date.getMonth() : date.getMonth();
    var year = date.getFullYear();

    var hour = date.getHours() < 10 ? '0' + date.getHours() : date.getHours();
    var min = date.getMinutes() < 10 ? '0' + date.getMinutes() : date.getMinutes();
    var sec = date.getSeconds() < 10 ? '0' + date.getSeconds() : date.getSeconds();

    return day + '.' + month + '.' + year + ' ' + hour + ':' + min + ':' + sec;
}

function loadData() {

    var xhr = new XMLHttpRequest();

    xhr.open('GET', '/Auction/GetLots', true);
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
                
                //var lot1 = $("[data-id=1]").children("h1").children("Label");
                //var lot2 = $("[data-id=2]").children("h1").children("Label");

                var lots = $("Label");
                if (lots.length == 2) {
                    lots[0].innerHTML = data[0].Title;
                    lots[1].innerHTML = data[1].Title;
                }

                document.getElementById('time1_1').innerHTML = data[0].TimeToEnd;
                document.getElementById('time2_1').innerHTML = data[0].Bider;

                document.getElementById('time1_2').innerHTML = data[1].TimeToEnd;
                document.getElementById('time2_2').innerHTML = data[1].Bider;
            }
        }
    }
}

var timerId = setInterval(loadData, 5000);