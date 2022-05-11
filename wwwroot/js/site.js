const uri = 'api/todoitems';
let todos = [];

function getItems()
{
    //an HTTP GET request is sent to the api/todoitems route
    fetch(uri)
        .then(response => response.json())
        //When the web API returns a successful status code, the _displayItems function is invoked
        //Each to-do item in the array parameter accepted by _displayItems is added to a table with Edit and Delete buttons.
        .then(data => _displayItems(data))
        //If the web API request fails, an error is logged to the browser's console.
        .catch(error => console.error('Unable to get items.', error));
}

function addItem()
{
    //get the name entered from the form
    const addNameTextbox = document.getElementById('add-name');
    //An item variable is declared to construct an object literal representation of the to-do item.
    const item = {
        isComplete: false,
        name: addNameTextbox.value.trim()
    };

    //fetch and post the object
    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept' : 'application/json',
            'Content-Type' : 'application/json'
        },
        //The JSON is produced by passing the object literal stored in item to the JSON.stringify function.
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        //When the web API returns a successful status code, the getItems function is invoked to update the HTML table.
        .then(() =>{
            getItems();
            addNameTextbox.value = '';
        })
        //If the web API request fails, an error is logged to the browser's console.
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id)
{
    fetch('${uri}/${id}', {
        method: 'DELETE'       
    })
    .then(() => getItems())
    .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = todos.find(item => item.id === id);
    
    document.getElementById('edit-name').value = item.name;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-isComplete').checked = item.isComplete;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() 
{
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: parseInt(itemId, 10),
        isComplete: document.getElementById('edit-isComplete').checked,
        name: document.getElementById('edit-name').value.trim()
    };

    //The route is suffixed with the unique identifier of the item to update. For example, api/todoitems/1.
    fetch('${uri}/{itemId}', {
        //The HTTP action verb is PUT, as indicated by the method option.
        method: 'PUT',
        headers: {
            'Accept' : 'application/json',
            'Content-Type' : 'application/json'
        },
        body: JSON.stringify(item)    
    })
    .then(() => getItems())
    .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

function closeInput()
{
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount)
{
    const name = (itemCount === 1) ? 'to-do' : 'to-dos';
    document.getElementById('counter').innerText = '${itemCount} ${name}';
}

function _displayItems(data)
{
    const tBody = document.getElementById('todos');
    tBody.innerHTML = '';
    
    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        let isCompleteCheckbox = document.createElement('input');
        isCompleteCheckbox.type = 'checkbox';
        isCompleteCheckbox.disabled = true;
        isCompleteCheckbox.checked = item.isComplete;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', 'displayEditForm(${item.id})');

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', 'deleteItem(${item.id})');

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(isCompleteCheckbox);

        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(item.name);
        td2.appendChild(textNode);

        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);    
    });

    todos = data;
}