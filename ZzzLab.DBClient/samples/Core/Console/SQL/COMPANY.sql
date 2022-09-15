--[GET]
SELECT
	*
FROM com_company
WHERE company = @id

--[LIST]
SELECT
	*
FROM com_company
WHERE use_yn = 'Y'
