document.addEventListener('DOMContentLoaded', function () {

    var savedLayout = document.getElementById('Layout').value;

    var layout = document.querySelector('.layout-map');
    layout.addEventListener('click', selectSeat);
    document.querySelector('.layout-btn').addEventListener('click', generateLayout.bind(null, null));

    //savedModel = '{"Layout":[[1,2,0,3,4],[1,2,0,3,4],[1,2,0,3,4],[1,2,3,4,5]],"Total":17}';
    let savedModel = {
        Layout: [],
        Total: 0
    };

    if (savedLayout !== '') {
        savedModel.Layout = JSON.parse(savedLayout);
        count = document.getElementById('Count');
        count != null ? savedModel.Total = parseInt(count.value) : savedModel.Total = 0;
        generateLayout(savedModel);
    }

});

function selectSeat(e) {
    let target = e.target;

    if (target.classList.value === 'seat')
        target.classList.add('selected');
    else
        if (target.classList.value === 'seat selected')
            target.classList.remove('selected');
        else return;

    updateRow(target);
    document.querySelector('.layout-btn.save').disabled = false;
};

function updateRow(e) {

    let pickedSeat = e.dataset.seat;
    let currentRow = e.parentNode;
    let unselectedCollection;
    let check;

    if (e.classList.contains('selected')) {
        e.dataset.seat = 0;
        unselectedCollection = currentRow.querySelectorAll('.seat:not(.selected)');
        for (i = pickedSeat - 1; i < unselectedCollection.length; i++) {
            unselectedCollection[i].dataset.seat--;
            unselectedCollection[i].innerText = unselectedCollection[i].dataset.seat;
        }
    }
    else {
        unselectedCollection = currentRow.querySelectorAll('.seat:not(.selected)');
        let count = unselectedCollection.length;
        for (i = unselectedCollection.length - 1; i >= 0; i--) {
            check = unselectedCollection[i].dataset.seat;
            unselectedCollection[i].dataset.seat = count;
            unselectedCollection[i].innerText = unselectedCollection[i].dataset.seat;
            count--;
            if (check == 0) break;
        }
    }
    let total = document.querySelectorAll('.seat:not(.selected)').length;
    document.querySelector('.total-label').innerText = 'Total: ' + total + ' seats.';
};

function generateLayout(model) {
    let inputRows = document.getElementById('input-rows');
    let inputColumns = document.getElementById('input-columns');

    if (model) {
        rows = model.Layout.length;
        columns = model.Layout[0].length;
    }
    else {
        rows = parseInt(inputRows.value);
        columns = parseInt(inputColumns.value);
    }

    inputRows.value = '';
    inputColumns.value = '';

    if (isNaN(rows) || isNaN(columns))
        return;

    if (rows === 0 || columns === 0 || rows < 0 || columns < 0)
        return;

    var layout = document.querySelector('.layout-map');
    layout.innerHTML = '';

    let currentRow, currentSeat;

    for (i = 1; i <= rows; i++) {

        currentRow = document.createElement('div');
        currentRowNumber = document.createElement('span');

        currentRow.classList.add('row');
        currentRowNumber.innerText = i;

        currentRow.append(currentRowNumber);
        layout.append(currentRow);

        for (j = 1; j <= columns; j++) {
            currentSeat = document.createElement('div');
            currentSeat.classList.add('seat');
            if (model) {
                currentSeat.dataset.seat = model.Layout[i - 1][j - 1];
                if (currentSeat.dataset.seat == 0)
                    currentSeat.classList.add('selected');
            }
            else
                currentSeat.dataset.seat = j;

            currentSeat.innerText = currentSeat.dataset.seat;
            currentRow.append(currentSeat);
        }
    }

    let saveBtn = document.createElement('button');
    saveBtn.classList.add('layout-btn', 'save');
    saveBtn.innerText = 'SAVE';
    saveBtn.onclick = saveLayout;
    layout.append(saveBtn);

    let totalLabel = document.createElement('label');
    totalLabel.classList.add('total-label');

    if (model) {
        saveBtn.disabled = true;
        total = model.Total;
    }
    else
        total = rows * columns;

    totalLabel.innerText = 'Total: ' + total + ' seats.';
    layout.append(totalLabel);

};

function saveLayout() {
    let layoutRows = document.querySelectorAll('.layout-map .row');

    if (layoutRows == undefined)
        return;

    let result = {
        Layout: [],
        Total: 0
    }

    for (i = 0; i < layoutRows.length; i++) {
        result.Layout.push([]);
        for (j = 1; j < layoutRows[i].children.length; j++) {
            currentSeat = parseInt(layoutRows[i].children[j].dataset.seat);
            result.Layout[i][j - 1] = currentSeat;
            if (currentSeat !== 0)
                result.Total++;
        }
    }

    let layout = JSON.stringify(result.Layout);
    let count = result.Total;

    document.getElementById('Layout').value = layout;
    document.getElementById('Count').value = count;

    let saveBtn = document.querySelector('.layout-btn.save');
    saveBtn.disabled = true;
};