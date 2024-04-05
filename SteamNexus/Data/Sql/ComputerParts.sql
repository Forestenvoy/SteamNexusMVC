USE SteamNexus;

-- 電腦零件種類表 靜態資料

INSERT INTO ComputerPartCategories(Name)
	VALUES ('CPU 中央處理器'),('GPU 顯示卡'),('RAM 記憶體'),
	('MotherBoard 主機板'),('SSD 固態硬碟'),('HDD 傳統硬碟'),
	('AirCooler 風冷散熱器'),('LiquidCooler 水冷散熱器'),('CASE 機殼'),
	('PSU 電源供應器'),('OS 作業系統');
