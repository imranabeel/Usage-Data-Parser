DROP TABLE #temp1
DROP TABLE #temp2
DROP TABLE #temp3
SELECT
	TouchPointID,
	HandNumber,
	Date,
	DATEDIFF(
		DAY,
		LAG(Date, 1) OVER(PARTITION BY HandNumber ORDER BY Date),
		Date)
	AS [Number of Days]
INTO #temp1
FROM touchPoints 
ORDER BY HandNumber, Date

SELECT
	CollectedInTouchPoint,
	SUM(DATEDIFF(second, '0:00:00', onTime)) AS [Total On Time (s)],
	SUM(DATEDIFF(second, '0:00:00', activeTime)) AS [Total Active Time (s)]
	INTO #temp2
FROM sessions GROUP BY CollectedInTouchPoint

SELECT
	a.TouchPointID,
	a.HandNumber,
	a.Date, 
	a.[Number of Days], 
	b.[Total On Time (s)],
	b.[Total Active Time (s)],
	CAST(b.[Total On Time (s)] AS float)/CAST(a.[Number of Days] AS float) AS [Average On Time Per Day (s)],
	CAST(b.[Total Active Time (s)] AS float)/CAST(a.[Number of Days] AS float) AS [Average Active Time Per Day (s)]
INTO #temp3
FROM #temp1 a JOIN #temp2 b ON a.TouchPointID=b.CollectedInTouchPoint

SELECT * FROM #temp1
SELECT * FROM #temp2
SELECT * FROM #temp3

SELECT
	HandNumber,
	CONVERT(DECIMAL(10,2), AVG([Average On Time Per Day (s)])) AS [Average On Time Per Day (s)],
	CONVERT(DECIMAL(10,2),AVG([Average Active Time Per Day (s)])) AS [Average Active) Time Per Day (s)]
FROM #temp3
GROUP BY HandNumber