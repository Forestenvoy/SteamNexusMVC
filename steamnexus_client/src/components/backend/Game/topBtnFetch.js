const apiUrl = import.meta.env.VITE_APP_API_BASE_URL
export function GetGamePriceDataToDB(){
    fetch(`${apiUrl}/api/GamesManagement/GetGamePriceDataToDB`, {
        method: "GET"
    }).then(result => {
        // 此時 result 是一個請求結果的物件
        // 注意傳回值型態，字串用 text()，JSON 用 json()
        if (result.ok) {
            return result.text();
        }
    }).then(data => {
        console.log(data);
    }).catch(error => {
        console.log(error);
    });
}

export function GetOnlineUsersDataToDB(){
    fetch(`${apiUrl}/api/GamesManagement/GetOnlineUsersDataToDB`, {
        method: "GET"
    }).then(result => {
        // 此時 result 是一個請求結果的物件
        // 注意傳回值型態，字串用 text()，JSON 用 json()
        if (result.ok) {
            return result.text();
        }
    }).then(data => {
        console.log(data);
    }).catch(error => {
        console.log(error);
    });
}

export function GetNumberOfCommentsDataToDB(){
    fetch(`${apiUrl}/api/GamesManagement/GetNumberOfCommentsDataToDB`, {
        method: "GET"
    }).then(result => {
        // 此時 result 是一個請求結果的物件
        // 注意傳回值型態，字串用 text()，JSON 用 json()
        if (result.ok) {
            return result.text();
        }
    }).then(data => {
        console.log(data);
    }).catch(error => {
        console.log(error);
    });
}

export function GetMinDataToDB(){
    fetch(`${apiUrl}/api/GamesManagement/GetDataToDB?isMinimumRequirement=true`, {
        method: "GET"
    }).then(result => {
        // 此時 result 是一個請求結果的物件
        // 注意傳回值型態，字串用 text()，JSON 用 json()
        if (result.ok) {
            return result.text();
        }
    }).then(data => {
        console.log(data);
    }).catch(error => {
        console.log(error);
    });
}

export function GetRecDataToDB(){
    fetch(`${apiUrl}/api/GamesManagement/GetDataToDB?isMinimumRequirement=false`, {
        method: "GET"
    }).then(result => {
        // 此時 result 是一個請求結果的物件
        // 注意傳回值型態，字串用 text()，JSON 用 json()
        if (result.ok) {
            return result.text();
        }
    }).then(data => {
        console.log(data);
    }).catch(error => {
        console.log(error);
    });
}