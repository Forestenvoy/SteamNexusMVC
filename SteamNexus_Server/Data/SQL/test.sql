-- 抓 GPU

-- 最低配備
-- UPDATE MinimumRequirements SET GPUId = 10412 WHERE OriGpu LIKE '%770%';
-- SELECT GPUId,OriGpu FROM MinimumRequirements WHERE OriGpu LIKE '%Iris%';

-- 推薦配備
-- UPDATE RecommendedRequirements SET GPUId = 10474 WHERE OriGpu LIKE '%3080%';
SELECT GPUId,OriGpu FROM RecommendedRequirements WHERE OriGpu LIKE '%980%';


