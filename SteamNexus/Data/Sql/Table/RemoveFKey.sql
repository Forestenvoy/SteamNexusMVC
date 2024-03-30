-- 移除外來鍵關聯
USE SteamNexus;

-- 有關聯就會有刪除順序問題 !! 從後面開始刪除

-- 解除RAM產品表的外鍵約束
ALTER TABLE ProductRAMs
DROP CONSTRAINT FK_ProductRAMs_ProductInformations;

-- 解除GPU產品表的外鍵約束
ALTER TABLE ProductGPUs
DROP CONSTRAINT FK_ProductGPUs_ProductInformations;
ALTER TABLE ProductGPUs
DROP CONSTRAINT FK_ProductGPUs_GPUs;

-- 解除CPU產品表的外鍵約束
ALTER TABLE ProductCPUs
DROP CONSTRAINT FK_ProductCPUs_ProductInformations;
ALTER TABLE ProductCPUs
DROP CONSTRAINT FK_ProductCPUs_CPUs;

-- 解除產品資訊表的外鍵約束
ALTER TABLE ProductInformations
DROP CONSTRAINT FK_ProductInformations_ProductCategories;

-- 解除遊戲語言表的外鍵約束
ALTER TABLE GameLanguages
DROP CONSTRAINT FK_GameLanguages_Languages;
ALTER TABLE GameLanguages
DROP CONSTRAINT FK_GameLanguages_Games;

-- 解除遊戲追蹤表的外鍵約束
ALTER TABLE GameFollows
DROP CONSTRAINT FK_GameFollows_Games;
ALTER TABLE GameFollows
DROP CONSTRAINT FK_GameFollows_Members;

-- 解除會員資料表的外鍵約束
ALTER TABLE Members
DROP CONSTRAINT FK_Members_GPUs;
ALTER TABLE Members
DROP CONSTRAINT FK_Members_CPUs;

-- 解除遊玩人數歷史表的外鍵約束
ALTER TABLE PlayersHistories
DROP CONSTRAINT FK_PlayersHistories_Games;

-- 解除價格歷史表的外鍵約束
ALTER TABLE PriceHistories
DROP CONSTRAINT FK_PriceHistories_Games;

-- 解除標籤群組表的外鍵約束
ALTER TABLE TagGroups
DROP CONSTRAINT FK_TagGroups_Tags;
ALTER TABLE TagGroups
DROP CONSTRAINT FK_TagGroups_Games;

-- 解除遊戲資料表的外鍵約束
ALTER TABLE Games
DROP CONSTRAINT FK_Games_RecommendedRequirements;
ALTER TABLE Games
DROP CONSTRAINT FK_Games_MinimumRequirements;

-- 解除建議配備的外鍵約束
ALTER TABLE RecommendedRequirements
DROP CONSTRAINT FK_RecommendedRequirements_GPUs;
ALTER TABLE RecommendedRequirements
DROP CONSTRAINT FK_RecommendedRequirements_CPUs;

-- 解除最低配備的外鍵約束
ALTER TABLE MinimumRequirements
DROP CONSTRAINT FK_MinimumRequirements_GPUs;
ALTER TABLE MinimumRequirements
DROP CONSTRAINT FK_MinimumRequirements_CPUs;

