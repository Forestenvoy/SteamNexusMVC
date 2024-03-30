-- SteamNexus 資料表建立

USE SteamNexus;

-- 中央處理器 CPUs 
CREATE TABLE CPUs
(
    CPUId INT NOT NULL PRIMARY KEY IDENTITY(10000,1),
    -- CPU 名稱
    Name NVARCHAR(100) NOT NULL,
    -- CPU 跑分
    Score INT NOT NULL
);

-- 顯示卡 GPUs
CREATE TABLE GPUs
(
    GPUId INT NOT NULL PRIMARY KEY IDENTITY(10000,1),
    -- GPU 名稱
    Name NVARCHAR(100) NOT NULL,
    -- GPU 跑分
    Score INT NOT NULL
);

-- 語言表
CREATE TABLE Languages
(
    LanguageId int NOT NULL PRIMARY KEY IDENTITY(100,1),
    -- 語言名稱
    Name NVARCHAR(100) NOT NULL
);


-- 最低配備
CREATE TABLE MinimumRequirements
(
    MinReqId int NOT NULL PRIMARY KEY IDENTITY(10000,1),
    -- CPU，外來鍵，預設 10000
    CPUId int NOT NULL DEFAULT 10000,
    -- GPU，外來鍵，預設 10000
    GPUId int NOT NULL DEFAULT 10000,
    -- 記憶體，預設 4 
    RAM int NOT NULL DEFAULT 4,
    -- 作業系統
    OS NVARCHAR(100),
    -- DirectX
    DirectX NVARCHAR(100),
    -- 網路需求
    Network NVARCHAR(100),
    -- 儲存空間
    Storage NVARCHAR(100),
    -- 音效卡
    Audio NVARCHAR(100),
    -- 備註
    Note NVARCHAR(500)
);

-- 建議配備
CREATE TABLE RecommendedRequirements
(
    RecReqId int NOT NULL PRIMARY KEY IDENTITY(10000,1),
    -- CPU，外來鍵，預設 10000
    CPUId int NOT NULL DEFAULT 10000,
    -- GPU，外來鍵，預設 10000
    GPUId int NOT NULL DEFAULT 10000,
    -- 記憶體，預設 4 
    RAM int NOT NULL DEFAULT 4,
    -- 作業系統
    OS NVARCHAR(100),
    -- DirectX
    DirectX NVARCHAR(100),
    -- 網路需求
    Network NVARCHAR(100),
    -- 儲存空間
    Storage NVARCHAR(100),
    -- 音效卡
    Audio NVARCHAR(100),
    -- 備註
    Note NVARCHAR(500)
);

-- 遊戲資料表
CREATE TABLE Games
(
    GameId int NOT NULL PRIMARY KEY IDENTITY(10000,1),
    -- Steam 內部的 遊戲 ID
    AppId int NOT NULL,
    -- 遊戲名稱
    Name NVARCHAR(100) NOT NULL,
    -- 遊戲原始價格
    OriginalPrice int NOT NULL,
    -- 遊戲現在價格
    CurrentPrice int NOT NULL,
    -- 歷史最低價格
    LowestPrice int NOT NULL,
    -- 遊戲分級
    AgeRating NVARCHAR(100) NOT NULL,
    -- 遊戲評論
    Comment NVARCHAR(100),
    -- 評論數量
    CommentNum int DEFAULT 0,
    -- 發行日期
    ReleaseDate DATETIME,
    -- 發行商
    Publisher NVARCHAR(100),
    -- 遊戲介紹
    Description NVARCHAR(500),
    -- 當前遊玩人數
    Players int,
    -- 24小時高峰人數
    PeakPlayers int,
    -- 遊戲圖片路徑
    ImagePath NVARCHAR(300),
    -- 遊戲影片路徑
    VideoPath NVARCHAR(300),
    -- 最低配備
    MinReqId int NOT NULL,
    -- 建議配備
    RecReqId int NOT NULL
);

-- 遊戲語言表
CREATE TABLE GameLanguages
(
    GameLanguageId int NOT NULL PRIMARY KEY IDENTITY(10000,1),
    -- 遊戲ID
    GameId int NOT NULL,
    -- 語言ID
    LanguageId int NOT NULL
);


-- 標籤表
CREATE TABLE Tags
(
    TagId int NOT NULL PRIMARY KEY IDENTITY(10000,1),
    -- 標籤名稱
    Name NVARCHAR(100) NOT NULL
);

-- 標籤群組表 
CREATE TABLE TagGroups
(
    TagGroupId int NOT NULL PRIMARY KEY IDENTITY(10000,1),
    -- 遊戲ID，外來鍵
    GameId int NOT NULL,
    -- 標籤ID，外來鍵
    TagId int NOT NULL
);

-- 價格歷史表
CREATE TABLE PriceHistories
(
    PriceHistoryId int NOT NULL PRIMARY KEY IDENTITY(10000,1),
    -- 遊戲ID，外來鍵
    GameId int NOT NULL,
    -- 日期
    Date DATE NOT NULL,
    -- 價格
    Price int NOT NULL
)

-- 遊玩人數歷史表
CREATE TABLE PlayersHistories
(
    PlayersHistoryId int NOT NULL PRIMARY KEY IDENTITY(10000,1),
    -- 遊戲ID，外來鍵
    GameId int NOT NULL,
    -- 日期 and 時間
    Date DATETIME NOT NULL,
    -- 遊玩人數
    Players int NOT NULL
)

-- 會員資料表
CREATE TABLE Members
(
    MemberId int NOT NULL PRIMARY KEY IDENTITY(10000,1),
    -- 會員帳號 = 信箱
    Email NVARCHAR(100) NOT NULL UNIQUE,
    -- 會員密碼
    Password NVARCHAR(100) NOT NULL,
    -- 名稱
    Name NVARCHAR(100),
    -- 性別
    Gender bit DEFAULT 0,
    -- 電話
    Phone NVARCHAR(100),
    -- 生日
    Birthday DATE DEFAULT '1900-01-01',
    -- 大頭照
    Photo NVARCHAR(300),
    -- 會員的 CPU
    CPUId int NOT NULL DEFAULT 10000,
    -- 會員的 GPU
    GPUId int NOT NULL DEFAULT 10000,
    -- 會員的 RAM
    RAM int NOT NULL DEFAULT 4
)

-- 遊戲追蹤表
CREATE TABLE GameFollows
(
    GameFollowId int NOT NULL PRIMARY KEY IDENTITY(10000,1),
    -- 會員ID，外來鍵
    MemberId int NOT NULL,
    -- 遊戲ID，外來鍵
    GameId int NOT NULL,
    -- 追蹤時間，預設現在時間
    FollowTime DATETIME DEFAULT GETDATE()
)

-- 常見問題表
CREATE TABLE CommonQuestions
(
    CommonQuestionId int NOT NULL PRIMARY KEY IDENTITY(10000,1),
    -- 問題標題
    Title NVARCHAR(100) NOT NULL,
    -- 問題內容
    Content NVARCHAR(500)
)

-- 廣告資料表
CREATE TABLE Advertisements
(
    AdvertisementId int NOT NULL PRIMARY KEY IDENTITY(10000,1),
    -- 標題
    Title NVARCHAR(100) NOT NULL,
    -- 敘述
    Discription NVARCHAR(200),
    -- 網址
    Url NVARCHAR(300),
    -- 圖片路徑
    ImagePath NVARCHAR(300)
)

-- 硬體系統

-- 產品類別表
CREATE TABLE ProductCategories
(
    ProductCategoryId int NOT NULL PRIMARY KEY IDENTITY(10000,1),
    -- 類別名稱
    Name NVARCHAR(100) NOT NULL
)

-- 產品資訊表
CREATE TABLE ProductInformations
(
    ProductInformationId int NOT NULL PRIMARY KEY IDENTITY(10000,1),
    -- 產品類別ID，外來鍵
    ProductCategoryId int NOT NULL,
    -- 產品名稱
    Name NVARCHAR(200) NOT NULL,
    -- 規格
    Specification NVARCHAR(500),
    -- 價格
    Price int NOT NULL,
    -- 瓦數
    Wattage int NOT NULL,
    -- 推薦
    Recommend int DEFAULT 0
)

-- CPU產品表
CREATE TABLE ProductCPUs
(
    ProductCPUId int NOT NULL PRIMARY KEY IDENTITY(10000,1),
    -- 產品資訊ID，外來鍵
    ProductInformationId int NOT NULL,
    -- CPU跑分ID，外來鍵
    CPUId int NOT NULL
)

-- GPU產品表
CREATE TABLE ProductGPUs
(
    ProductGPUId int NOT NULL PRIMARY KEY IDENTITY(10000,1),
    -- 產品資訊ID，外來鍵
    ProductInformationId int NOT NULL,
    -- GPU跑分ID，外來鍵
    GPUId int NOT NULL
)

-- RAM產品表
CREATE TABLE ProductRAMs
(
    ProductRAMId int NOT NULL PRIMARY KEY IDENTITY(10000,1),
    -- 產品資訊ID，外來鍵
    ProductInformationId int NOT NULL,
    -- RAM大小
    Size int NOT NULL
)