-- SteamNexus 刪除資料表

USE SteamNexus;

-- 有關聯就會有刪除順序問題 !! 從後面開始刪除

DROP TABLE ProductRAMs;
DROP TABLE ProductGPUs;
DROP TABLE ProductCPUs;
DROP TABLE ProductInformations;
DROP TABLE ProductCategories;
DROP TABLE Advertisements;
DROP TABLE CommonQuestions;
DROP TABLE GameFollows;
DROP TABLE Members;
DROP TABLE PlayersHistories;
DROP TABLE PriceHistories;
DROP TABLE TagGroups;
DROP TABLE Tags;
DROP TABLE GameLanguages;
DROP TABLE Games;
DROP TABLE RecommendedRequirements;
DROP TABLE MinimumRequirements;
DROP TABLE Languages;
DROP TABLE GPUs;
DROP TABLE CPUs;
