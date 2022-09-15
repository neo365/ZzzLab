--[GET]
SELECT
	*
FROM com_company
WHERE company_id = @company_id

--[LIST]
SELECT
	*
FROM com_company
WHERE 1 = 1
--{search}
--{orderby}
;

--[LIST_RECORDS_TOTAL]
SELECT COUNT(*) FROM com_company;

--[LIST_RECORDS_FILTERED]
SELECT COUNT(*) FROM com_company
WHERE 1 = 1
--{search}

--[INSERT]
INSERT INTO com_company(
	company_id, 
	company_name, 
	parent_id, 
	used_yn, 
	memo,
	date_inserted
) VALUES (
	@company_id, 
	@company_name, 
	@parent_id, 
	@used_yn, 
	@memo,
	now(),
);

--[UPDATE]
UPDATE com_company SET
	company_name = @company_name,
	parent_id = @parent_id,
	used_yn = @used_yn,
	memo = @memo,
	date_updated = now() 
WHERE company_id = @company_id;

--[DELETE]
UPDATE com_company SET
	used_yn = 'N'
WHERE	company_id = @company_id
	AND used_yn = 'Y'

--[DELETE_FORCE]
DELETE
FROM com_company
WHERE company_id = @company_id;
