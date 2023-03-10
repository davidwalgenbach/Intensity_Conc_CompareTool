	SELECT * FROM
	(
	SELECT [Index]
	  ,1 as n
      ,[DateTime]
	  ,[SampleType]
	  ,[Description]
	  ,[TraxId]
	  ,[Mass]+' '+[Analyte] + ' ' + '[ ' +  [Mode] + ' ]' AS Analyte
	  ,[Concentration]  FROM [PLASMATRAX].[dbo].[SEQUENCE_DATA] WHERE [Description] != 'No Cal Blk' AND Analysis = {1} 
	) basetable
	PIVOT (
			Max(Concentration)
			FOR Analyte IN (
					{0}
				)
	) AS pivot_table1
	UNION
		SELECT * FROM
	(
	SELECT [Index]
	  ,2 as n
      ,[DateTime]
	  ,[SampleType]
	  ,[Description]
	  ,[TraxId]
	  ,[Mass]+' '+[Analyte] + ' ' + '[ ' +  [Mode] + ' ]' AS Analyte
	  ,[Slope] FROM [PLASMATRAX].[dbo].[SEQUENCE_DATA] WHERE [Description] != 'No Cal Blk' AND Analysis = {1} 
	) basetable
	PIVOT (
			Max(Slope)
			FOR Analyte IN (
					{0}
				)
	) AS pivot_table2
	UNION
		SELECT * FROM
	(
	SELECT [Index]
	  ,3 as n
      ,[DateTime]
	  ,[SampleType]
	  ,[Description]
	  ,[TraxId]
	  ,[Mass]+' '+[Analyte] + ' ' + '[ ' +  [Mode] + ' ]' AS Analyte
	  ,[Intercept]  FROM [PLASMATRAX].[dbo].[SEQUENCE_DATA] WHERE [Description] != 'No Cal Blk' AND Analysis = {1} 
	) basetable
	PIVOT (
			Max(Intercept)
			FOR Analyte IN (
					{0}
				)
	) AS pivot_table3
	UNION
		SELECT * FROM
	(
	SELECT [Index]
	  ,4 as n
      ,[DateTime]
	  ,[SampleType]
	  ,[Description]
	  ,[TraxId]
	  ,[Mass]+' '+[Analyte] + ' ' + '[ ' +  [Mode] + ' ]' AS Analyte
	  ,[RS2]  FROM [PLASMATRAX].[dbo].[SEQUENCE_DATA] WHERE [Description] != 'No Cal Blk' AND Analysis = {1} 
	) basetable
	PIVOT (
			Max(RS2)
			FOR Analyte IN (
					{0}
				)
	) AS pivot_table4
	ORDER BY [Index], n