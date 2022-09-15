--[GET]
SELECT
	*
FROM com_dept
WHERE dept_id = @_id

--[LIST]
SELECT
	*
FROM com_dept
WHERE used_yn = 'Y'

--[CHILDREN]
SELECT
	'dept' AS code_type,
	dept_id as id,
	dept_name as name,
	case
		WHEN (select count(*) from com_dept where parent_id = dept.dept_id) > 0 THEN 'Y'
		WHEN (select count(*) from com_user where dept_id = dept.dept_id) > 0 THEN 'Y'
		ELSE 'N' 
	END AS HAS_CHILD
FROM com_dept dept
WHERE use_yn = 'Y'
AND parent_id = @current_id
union
