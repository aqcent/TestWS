document.addEventListener('DOMContentLoaded', function () {      

    var tagsBtn = document.querySelectorAll('.timeslot-tag-js');

    var container = document.querySelector('.container-schedule');
    var cube = document.querySelector('.cube');
    var bg = document.querySelector('.bg');

    document.querySelector('.close').addEventListener('click', () => {
        container.classList.remove('active');
        cube.classList.remove('active');
        bg.classList.remove('blur');
    });

    let currentTimeslotID, currentCost;

    for (el of tagsBtn) {
        el.addEventListener('click', function (e) {

            bg.classList.add('blur');

            var screen = document.querySelector('.screen');    
            screen.style.backgroundImage = 'url(' + e.target.dataset.imgurl + ')';
            console.log(e.target.dataset);

            sendRequest('GET', '/Tickets/GetHallInfoWithLayout?timeslotID=' + e.target.dataset.timeslotid)
                .then(data => fitLayout(data));

            currentTimeslotID = parseInt(e.target.dataset.timeslotid);
            currentCost = parseFloat(e.target.dataset.currentcost);                        

            container.classList.add('active');
            cube.classList.add('active');
        });
    }


    document.querySelector('.schedule-layout').addEventListener('click', selectSeats);

    document.querySelector('.buy-btn').addEventListener('click', orderBtnEvent);
    document.querySelector('.reserve-btn').addEventListener('click', orderBtnEvent);

    function orderBtnEvent(e) {
        let selectedSeats = document.querySelectorAll('.seat.selected');
        let action = e.target.classList.contains('buy-btn') ? 'sold' : 'reserved';        

        let model = {
            TimeslotID: currentTimeslotID,
            SeatsRequest: {
                AddedSeats: [],
                Sum: 0
            },
            SelectedStatus: action == 'sold' ? 0 : 1
        };

        Array.from(selectedSeats).forEach(seat => {
            seat.classList.remove('selected');
            seat.classList.add(action);
            model.SeatsRequest.AddedSeats.push({
                Row: parseInt(seat.dataset.row),
                Seat: parseInt(seat.dataset.seat)
            });
            model.SeatsRequest.Sum = model.SeatsRequest.AddedSeats.length * currentCost;
            document.querySelector('.selected-seats-row').innerHTML = '';
        });

        sendRequest('POST', '/tickets/processRequest', model);

        document.querySelector('.buy-btn').classList.add('disabled');
        document.querySelector('.reserve-btn').classList.add('disabled');
    }
});

function fitLayout(hall) {    

    var layout = document.querySelector('.schedule-layout');
    layout.innerHTML = '';

    let orderRow = document.querySelector('.selected-seats-row');
    orderRow.innerHTML = '';
    
    document.querySelector('.buy-btn').classList.add('disabled');
    document.querySelector('.reserve-btn').classList.add('disabled');

    var model = hall.HallLayout;
    console.log(hall);

    if (model)
        model = JSON.parse(model)
    else
        return;

    var rows = model.length;
    var cols = model[0].length;

    _width = layout.offsetWidth;
    _height = layout.offsetHeight;
    width = _width / cols * 0.9;
    height = _height / rows * 0.9;
    width < height ? seatSize = width : seatSize = height;

    for (i = 0; i < rows; i++) {
        currentRow = document.createElement('div');
        currentRow.classList.add('row', 'side');
        currentRow.style.height = Math.floor(seatSize * 0.9) + 'px';
        currentRow.style.lineHeight = Math.floor(seatSize * 0.8) + 'px'; //currentRow.style.height;
        currentRow.style.marginBottom = Math.floor(seatSize * 0.1) + 'px';
        for (j = 0; j < cols; j++) {
            currentSeat = document.createElement('div');
            currentSeat.classList.add('side', 'seat');
            var a = hall.RequestedSeats.find(x => x.Row == i+1 && x.Seat == j+1);
            if (a !== undefined)
                a.Status == 0 ? currentSeat.classList.add('sold') : currentSeat.classList.add('reserved');
            currentSeat.style.width = Math.floor(seatSize * 0.9) + 'px';
            currentSeat.style.marginRight = Math.floor(seatSize * 0.1) + 'px';
            currentSeat.style.fontSize = Math.floor(seatSize * 0.9 / 2) + 'px';

            currentSeat.dataset.seat = model[i][j];
            currentSeat.dataset.row = i + 1;

            if (model[i][j] == 0)
                currentSeat.style.visibility = 'hidden';
            else
                currentSeat.innerText = model[i][j];

            currentRow.append(currentSeat);
        }
        layout.append(currentRow);
    };
};

function selectSeats(e) {
    var target = e.target;
    let addedSeatCell, addedSeat;
    let orderRow = document.querySelector('.selected-seats-row');

    if (!target.classList.contains('seat'))
        return;
    if (target.classList.contains('reserved') || target.classList.contains('sold'))
        return;

    if (target.classList.contains('selected')) {
        target.classList.remove('selected');

        for (seat of orderRow.children) {
            if (seat.dataset.seat == target.dataset.seat && seat.dataset.row == target.dataset.row)
                seat.remove();
        }

        if (orderRow.children.length == 0) {
            document.querySelector('.buy-btn').classList.add('disabled');
            document.querySelector('.reserve-btn').classList.add('disabled');
        }
    }
    else {
        if (orderRow.children.length > 5)
            return;

        target.classList.add('selected');

        document.querySelector('.buy-btn').classList.remove('disabled');
        document.querySelector('.reserve-btn').classList.remove('disabled');

        addedSeatCell = document.createElement('div');
        addedSeatCell.classList.add('seat-cell');
        addedSeat = document.createElement('div');
        addedSeat.classList.add('seat');
        labelRow = document.createElement('label');
        labelRow.innerHTML = 'ROW <span>' + target.dataset.row + '</span>';
        addedSeat.innerText = target.dataset.seat;
        addedSeatCell.dataset.seat = target.dataset.seat;
        addedSeatCell.dataset.row = target.dataset.row;

        addedSeatCell.append(addedSeat);
        addedSeatCell.append(labelRow);
        orderRow.append(addedSeatCell);
    }
};

function sendRequest(method, url, body = null) {
    return new Promise((resolve, reject) => {
        let xhr = new XMLHttpRequest();

        xhr.open(method, url);
        xhr.responseType = 'json';
        xhr.setRequestHeader('Content-Type', 'application/json')

        xhr.onload = () => {
            if (xhr.status >= 400) {
                reject(xhr.response);
            } else {
                resolve(xhr.response);
            }
        }

        xhr.onerror = () => {
            reject(xhr.response);
        }

        xhr.send(JSON.stringify(body));
    });

};