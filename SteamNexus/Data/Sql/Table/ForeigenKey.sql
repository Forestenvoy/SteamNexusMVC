-- 外來鍵關聯建立
USE SteamNexus;

-- 外來鍵約束 命名規則 FK_參照表名(外)_被參照表名(主)

-- 最低配備 ~ CPU、GPU
ALTER TABLE MinimumRequirements
ADD CONSTRAINT FK_MinimumRequirements_CPUs
FOREIGN KEY (CPUId) REFERENCES CPUs(CPUId);
ALTER TABLE MinimumRequirements
ADD CONSTRAINT FK_MinimumRequirements_GPUs
FOREIGN KEY (GPUId) REFERENCES GPUs(GPUId);

-- 建議配備 ~ CPU、GPU
ALTER TABLE RecommendedRequirements
ADD CONSTRAINT FK_RecommendedRequirements_CPUs
FOREIGN KEY (CPUId) REFERENCES CPUs(CPUId);
ALTER TABLE RecommendedRequirements
ADD CONSTRAINT FK_RecommendedRequirements_GPUs
FOREIGN KEY (GPUId) REFERENCES GPUs(GPUId);

-- 遊戲資料表 ~ 語言、最低配備、推薦配備
ALTER TABLE Games
ADD CONSTRAINT FK_Games_MinimumRequirements
FOREIGN KEY (MinReqId) REFERENCES MinimumRequirements(MinReqId);
ALTER TABLE Games
ADD CONSTRAINT FK_Games_RecommendedRequirements
FOREIGN KEY (RecReqId) REFERENCES RecommendedRequirements(RecReqId);

-- 標籤群組表 ~ 遊戲、標籤
ALTER TABLE TagGroups
ADD CONSTRAINT FK_TagGroups_Games
FOREIGN KEY (GameId) REFERENCES Games(GameId);
ALTER TABLE TagGroups
ADD CONSTRAINT FK_TagGroups_Tags
FOREIGN KEY (TagId) REFERENCES Tags(TagId);

-- 價格歷史表 ~ 遊戲
ALTER TABLE PriceHistories
ADD CONSTRAINT FK_PriceHistories_Games
FOREIGN KEY (GameId) REFERENCES Games(GameId);

-- 遊玩人數歷史表 ~ 遊戲
ALTER TABLE PlayersHistories
ADD CONSTRAINT FK_PlayersHistories_Games
FOREIGN KEY (GameId) REFERENCES Games(GameId);

-- 會員資料表 ~ CPU、GPU
ALTER TABLE Members
ADD CONSTRAINT FK_Members_CPUs
FOREIGN KEY (CPUId) REFERENCES CPUs(CPUId);
ALTER TABLE Members
ADD CONSTRAINT FK_Members_GPUs
FOREIGN KEY (GPUId) REFERENCES GPUs(GPUId);

-- 遊戲追蹤表 ~ 會員、遊戲
ALTER TABLE GameFollows
ADD CONSTRAINT FK_GameFollows_Members
FOREIGN KEY (MemberId) REFERENCES Members(MemberId);
ALTER TABLE GameFollows
ADD CONSTRAINT FK_GameFollows_Games
FOREIGN KEY (GameId) REFERENCES Games(GameId);

-- 遊戲語言表 ~ 遊戲、語言
ALTER TABLE GameLanguages
ADD CONSTRAINT FK_GameLanguages_Games
FOREIGN KEY (GameId) REFERENCES Games(GameId);
ALTER TABLE GameLanguages
ADD CONSTRAINT FK_GameLanguages_Languages
FOREIGN KEY (LanguageId) REFERENCES Languages(LanguageId);

-- 產品資訊表 ~ 種類
ALTER TABLE ProductInformations
ADD CONSTRAINT FK_ProductInformations_ProductCategories
FOREIGN KEY (ProductCategoryId) REFERENCES ProductCategories(ProductCategoryId);

-- CPU產品表 ~ CPU、產品資訊
ALTER TABLE ProductCPUs
ADD CONSTRAINT FK_ProductCPUs_CPUs
FOREIGN KEY (CPUId) REFERENCES CPUs(CPUId);
ALTER TABLE ProductCPUs
ADD CONSTRAINT FK_ProductCPUs_ProductInformations
FOREIGN KEY (ProductInformationId) REFERENCES ProductInformations(ProductInformationId);

-- GPU產品表 ~ GPU、產品資訊
ALTER TABLE ProductGPUs
ADD CONSTRAINT FK_ProductGPUs_GPUs
FOREIGN KEY (GPUId) REFERENCES GPUs(GPUId);
ALTER TABLE ProductGPUs
ADD CONSTRAINT FK_ProductGPUs_ProductInformations
FOREIGN KEY (ProductInformationId) REFERENCES ProductInformations(ProductInformationId);

-- RAM產品表 ~ 產品資訊
ALTER TABLE ProductRAMs
ADD CONSTRAINT FK_ProductRAMs_ProductInformations
FOREIGN KEY (ProductInformationId) REFERENCES ProductInformations(ProductInformationId);