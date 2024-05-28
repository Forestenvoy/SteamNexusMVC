-- 抓 CPU

-- 最低配備
-- UPDATE MinimumRequirements SET CPUId = 11949 WHERE OriCpu LIKE '%2500%' AND CPUId = 10000;
SELECT CPUId,OriCpu FROM MinimumRequirements WHERE OriCpu LIKE '%2500%' AND CPUId = 10000;

-- 推薦配備
-- UPDATE RecommendedRequirements SET CPUId = 11878 WHERE  OriCpu LIKE '%12400%' AND CPUId = 10000;
-- SELECT CPUId,OriCpu FROM RecommendedRequirements WHERE OriCpu LIKE '%12400%' AND CPUId = 10000;
