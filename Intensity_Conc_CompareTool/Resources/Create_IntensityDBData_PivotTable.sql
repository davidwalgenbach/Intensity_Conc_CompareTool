	SELECT * FROM
	(
	SELECT [Index]
	  ,1 as n
      ,[DateTime]
	  ,[SampleType]
	  ,[Description]
	  ,[TraxId]
	  ,[Mass]+' '+[Analyte] + ' ' + '[ ' +  [Mode] + ' ]' AS Analyte
	  ,[MeanValue] as [Intensity]  FROM [PLASMATRAX].[dbo].[SEQUENCE_DATA] WHERE Analysis = {1}
	) basetable
	PIVOT (
			Max(Intensity)
			FOR Analyte IN (
			--{0} refers to analyte list since this is not a stored procedure. (C# string builder stuff) 
			-- this will change to a declared variable as soon as this is made a stored procedure. 
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
	  ,ROUND(TRY_CAST([Replicate1] AS FLOAT), 0) AS Replicate1  FROM [PLASMATRAX].[dbo].[SEQUENCE_DATA] WHERE Analysis = {1}
	) basetable
	PIVOT (
			Max(Replicate1)
			FOR Analyte IN (
			--{0} refers to analyte list since this is not a stored procedure. (C# string builder stuff) 
			-- this will change to a declared variable as soon as this is made a stored procedure. 
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
	  ,ROUND(TRY_CAST([Replicate2] AS FLOAT), 0) AS Replicate2  FROM [PLASMATRAX].[dbo].[SEQUENCE_DATA] WHERE Analysis = {1}
	) basetable
	PIVOT (
			Max(Replicate2)
			FOR Analyte IN (
			--{0} refers to analyte list since this is not a stored procedure. (C# string builder stuff) 
			-- this will change to a declared variable as soon as this is made a stored procedure. 
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
	  ,ROUND(TRY_CAST([Replicate3] AS FLOAT), 0) AS Replicate3  FROM [PLASMATRAX].[dbo].[SEQUENCE_DATA] WHERE Analysis = {1}
	) basetable
	PIVOT (
			Max(Replicate3)
			FOR Analyte IN (
			--{0} refers to analyte list since this is not a stored procedure. (C# string builder stuff) 
			-- this will change to a declared variable as soon as this is made a stored procedure. 
					{0}
				)
	) AS pivot_table4
	UNION
		SELECT * FROM
	(
	SELECT [Index]
	  ,5 as n
      ,[DateTime]
	  ,[SampleType]
	  ,[Description]
	  ,[TraxId]
	  ,[Mass]+' '+[Analyte] + ' ' + '[ ' +  [Mode] + ' ]' AS Analyte
	  ,ROUND(TRY_CAST([RSD] AS FLOAT), 2) AS RSD FROM [PLASMATRAX].[dbo].[SEQUENCE_DATA] WHERE Analysis = {1}
	) basetable
	PIVOT (
			Max(RSD)
			FOR Analyte IN (
			--{0} refers to analyte list since this is not a stored procedure. (C# string builder stuff) 
			-- this will change to a declared variable as soon as this is made a stored procedure. 
					{0}
				)
	) AS pivot_table5 
	ORDER BY [Description], n
