// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {

    //******* the following req will get the data for the drop down menu what the DOM is loaded  ********


    $.getJSON('api/Store')
        .done(function (data) {
            // On success, 'data' contains a list of stores.
            let StoreArray = data;
            let storeSelectElement = document.getElementById("storeSelect");
            StoreArray.forEach(function (value) {
                storeSelectElement.appendChild(new Option(value.city, value.storeId)); //***** first char of the prop needs to be lower case even if it is upper in the DB ******
            });
        });

    let selectStore = document.getElementById('storeSelect');

    // https://stackoverflow.com/questions/24875414/addeventlistener-change-and-option-selection
    selectStore.addEventListener("click", function () {
        let storevalue = selectStore.options[selectStore.selectedIndex].value;
        console.log(`storevalue = ${storevalue}`);
        removeOptions(document.getElementById('salesPersonSelect'));
        getStoreEmployees(storevalue);
    });

    // https://stackoverflow.com/questions/3364493/how-do-i-clear-all-options-in-a-dropdown-box
    function removeOptions(selectElement) {
        var i, L = selectElement.options.length - 1;
        for (i = L; i >= 0; i--) {
            selectElement.remove(i);
        }
    }

    function getStoreEmployeesV2(pStoreId) {
        // https://stackoverflow.com/questions/9499794/single-controller-with-multiple-get-methods-in-asp-net-web-api
        $.getJSON('/api/ordersdb/getstoreemployees' + '/' + pStoreId)
            .done(function (data) {
                // On success, 'data' contains a list of products.
                let SalesPersonArray = data;
                let salesPersonSelectElement = document.getElementById("salesPersonSelect");
                SalesPersonArray.forEach(function (value) {
                    salesPersonSelectElement.appendChild(new Option(value.lastName, value.salesPersonId));
                });
            });
    }

    function getStoreEmployees(pStoreId) {
        switch (pStoreId) {
            case '98053':
                console.log("1-4");
                getStoreEmployeesV2(pStoreId)
                break;
            case '98007':
                console.log("5-8");
                getStoreEmployeesV2(pStoreId)
                break;
            case '98077':
                console.log("9-12");
                getStoreEmployeesV2(pStoreId)
                break;
            case '98055':
                console.log("13-16");
                getStoreEmployeesV2(pStoreId)
                break;
            case '98011':
                console.log("17-20");
                getStoreEmployeesV2(pStoreId)
                break;
            case '98046':
                console.log("21-24");
                getStoreEmployeesV2(pStoreId)
                break;
        }
    }

    $.getJSON('api/cd')
        .done(function (data) {
            // On success, 'data' contains a list of products.
            let CdArray = data;
            let cdSelectElement = document.getElementById("cdSelect");
            CdArray.forEach(function (value) {
                cdSelectElement.appendChild(new Option(value.cdname, value.cdId));
            });

        });
});

let Event = function (pStoreId, pEmployeeId, pCdId, pCounted) {
    this.StoreId = parseInt(pStoreId);
    this.SalesPersonId = parseInt(pEmployeeId);
    this.CdId = parseInt(pCdId);
    this.PricePaid = parseInt(pCounted);
}

function addEvent() {
    let selectStore = document.getElementById('storeSelect');
    let storevalue = selectStore.options[selectStore.selectedIndex].value;

    let selectEmployee = document.getElementById('salesPersonSelect');
    let employeevalue = selectEmployee.options[selectEmployee.selectedIndex].value;

    let selectCd = document.getElementById('cdSelect');
    let cdvalue = selectCd.options[selectCd.selectedIndex].value;

    let cdCount = parseInt(document.getElementById('howmany').value);


    let newEvent = new Event(storevalue, employeevalue, cdvalue, cdCount);
    console.log(newEvent, "currentOrder");
    $.ajax({
        // https://stackoverflow.com/questions/9499794/single-controller-with-multiple-get-methods-in-asp-net-web-api
        url: "api/OrdersDB/PostOrder",
        type: "POST",
        data: JSON.stringify(newEvent),
        contentType: "application/json; charset=utf-8",

        success: function (result) {
            console.log(`result is ${result}`);
            alert(result + " was added");
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert("Status: " + textStatus); alert("Error: " + errorThrown); alert("XMLHTTP: " + XMLHttpRequest)
        }
    });
}

function getTotal() {
    let selectStore = document.getElementById('storeSelect');
    let storevalue = selectStore.options[selectStore.selectedIndex].value;

    // https://stackoverflow.com/questions/9499794/single-controller-with-multiple-get-methods-in-asp-net-web-api
    $.getJSON(`api/OrdersDB/GetTotalSale/${storevalue}`).done(function (data) {
        console.log(`data is ${data}`);
        $('#totalDescription').text(`that store sold $${data}.`);
    }).fail(function (jqXHR, textStatus, error) {
        $('#totalDescription').text(`error: ${error}`);
    });
}

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

async function getStoreData() {
    document.getElementById('totalStoreDataDescription').innerHTML = `getting the store data...`;
    await sleep(2000);

    $.getJSON(`api/OrdersDB/Get`).done(function (data) {
        console.log(data)
        let totalStoreDataDescription = document.getElementById('totalStoreDataDescription');
        let txt = `<strong>StoreID | Count</strong><br>`;
        data.forEach((element) => {
            console.log(`StoreID = ${element['storeID']} | Count = ${element['count']}`)
            txt += `=  ${element['storeID']} | ${element['count']}<br>`
        })
        console.log(`txt = ${txt}`)
        totalStoreDataDescription.innerHTML = txt;
        //$('#totalStoreDataDescription').innerHTML = "something else";
        //$('#totalStoreDataDescription').text(`DONE GETTING DATA === ${data}`);
    }).fail(function (jqXHR, textStatus, error) {
        $('#totalStoreDataDescription').text(`error: ${error}`);
    });
}
