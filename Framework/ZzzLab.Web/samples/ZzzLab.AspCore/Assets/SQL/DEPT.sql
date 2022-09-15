--[GET]
SELECT
	*
FROM com_dept
WHERE com_dept = @com_dept

--[LIST]
SELECT
	*
FROM com_dept
WHERE 1 = 1
--{search}
--{orderby}
;

--[LIST_RECORDS_TOTAL]
SELECT COUNT(*) FROM com_dept;

--[LIST_RECORDS_FILTERED]
SELECT COUNT(*) FROM com_dept
WHERE 1 = 1
--{search}

--[INSERT]
INSERT INTO com_dept(
	dept_id,
	company_id, 
	dept_name, 
	parent_id, 
	used_yn, 
	memo,
	date_inserted
) VALUES (
	@dept_id,
	@company_id, 
	@dept_name, 
	@parent_id, 
	@used_yn, 
	@memo,
	now(),
);

--[UPDATE]
UPDATE com_dept SET
	dept_name = @dept_name,
	parent_id = @parent_id,
	used_yn = @used_yn,
	memo = @memo,
	date_updated = now() 
WHERE dept_id = @dept_id;

--[DELETE]
UPDATE com_dept SET
	used_yn = 'N'
WHERE	dept_id = @dept_id
	AND used_yn = 'Y'

--[DELETE_TRUNCATE]
DELETE
FROM com_dept
WHERE dept_id = @dept_id;
