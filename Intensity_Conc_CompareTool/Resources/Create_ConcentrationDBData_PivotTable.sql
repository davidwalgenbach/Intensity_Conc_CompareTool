	SELECT * FROM
	(
	SELECT [Index]
	  ,1 as n
      ,[DateTime]
	  ,[SampleType]
	  ,[Description]
	  ,[TraxId]
	  ,[Mass]+' '+[Analyte] + ' ' + '[ ' +  [Mode] + ' ]' AS Analyte
	  ,[Concentration]  FROM [PLASMATRAX].[dbo].[SEQUENCE_DATA] WHERE Analysis = '011723092726'
	) basetable
	PIVOT (
			Max(Concentration)
			FOR Analyte IN (
					"9 Be [ No Gas ] ",
					"75 As [ No Gas ] ",
					"88 Sr [ No Gas ] ",
					"98 Mo [ No Gas ] ",
					"111 Cd [ No Gas ] ",
					"115 In [ No Gas ] ",
					"137 Ba [ No Gas ] ",
					"181 Ta [ No Gas ] ",
					"182 W [ No Gas ] ",
					"205 Tl [ No Gas ] ",
					"208 Pb [ No Gas ] ",
					"7 Li [ Cool ] ",
					"23 Na [ Cool ] ",
					"24 Mg [ Cool ] ",
					"27 Al [ Cool ] ",
					"39 K [ Cool ] ",
					"44 Ca [ Cool ] ",
					"52 Cr [ Cool ] ",
					"55 Mn [ Cool ] ",
					"59 Co [ Cool ] ",
					"60 Ni [ Cool ] ",
					"63 Cu [ Cool ] "
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
	  ,[Slope]  FROM [PLASMATRAX].[dbo].[SEQUENCE_DATA] WHERE Analysis = '011723092726'
	) basetable
	PIVOT (
			Max(Slope)
			FOR Analyte IN (
					"9 Be [ No Gas ] ",
					"75 As [ No Gas ] ",
					"88 Sr [ No Gas ] ",
					"98 Mo [ No Gas ] ",
					"111 Cd [ No Gas ] ",
					"115 In [ No Gas ] ",
					"137 Ba [ No Gas ] ",
					"181 Ta [ No Gas ] ",
					"182 W [ No Gas ] ",
					"205 Tl [ No Gas ] ",
					"208 Pb [ No Gas ] ",
					"7 Li [ Cool ] ",
					"23 Na [ Cool ] ",
					"24 Mg [ Cool ] ",
					"27 Al [ Cool ] ",
					"39 K [ Cool ] ",
					"44 Ca [ Cool ] ",
					"52 Cr [ Cool ] ",
					"55 Mn [ Cool ] ",
					"59 Co [ Cool ] ",
					"60 Ni [ Cool ] ",
					"63 Cu [ Cool ] "
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
	  ,[Intercept]  FROM [PLASMATRAX].[dbo].[SEQUENCE_DATA] WHERE Analysis = '011723092726'
	) basetable
	PIVOT (
			Max(Intercept)
			FOR Analyte IN (
					"9 Be [ No Gas ] ",
					"75 As [ No Gas ] ",
					"88 Sr [ No Gas ] ",
					"98 Mo [ No Gas ] ",
					"111 Cd [ No Gas ] ",
					"115 In [ No Gas ] ",
					"137 Ba [ No Gas ] ",
					"181 Ta [ No Gas ] ",
					"182 W [ No Gas ] ",
					"205 Tl [ No Gas ] ",
					"208 Pb [ No Gas ] ",
					"7 Li [ Cool ] ",
					"23 Na [ Cool ] ",
					"24 Mg [ Cool ] ",
					"27 Al [ Cool ] ",
					"39 K [ Cool ] ",
					"44 Ca [ Cool ] ",
					"52 Cr [ Cool ] ",
					"55 Mn [ Cool ] ",
					"59 Co [ Cool ] ",
					"60 Ni [ Cool ] ",
					"63 Cu [ Cool ] "
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
	  ,[RS2]  FROM [PLASMATRAX].[dbo].[SEQUENCE_DATA] WHERE Analysis = '011723092726'
	) basetable
	PIVOT (
			Max(RS2)
			FOR Analyte IN (
					"9 Be [ No Gas ] ",
					"75 As [ No Gas ] ",
					"88 Sr [ No Gas ] ",
					"98 Mo [ No Gas ] ",
					"111 Cd [ No Gas ] ",
					"115 In [ No Gas ] ",
					"137 Ba [ No Gas ] ",
					"181 Ta [ No Gas ] ",
					"182 W [ No Gas ] ",
					"205 Tl [ No Gas ] ",
					"208 Pb [ No Gas ] ",
					"7 Li [ Cool ] ",
					"23 Na [ Cool ] ",
					"24 Mg [ Cool ] ",
					"27 Al [ Cool ] ",
					"39 K [ Cool ] ",
					"44 Ca [ Cool ] ",
					"52 Cr [ Cool ] ",
					"55 Mn [ Cool ] ",
					"59 Co [ Cool ] ",
					"60 Ni [ Cool ] ",
					"63 Cu [ Cool ] "
				)
	) AS pivot_table4
	ORDER BY [Description], n