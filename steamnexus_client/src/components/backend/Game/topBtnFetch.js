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